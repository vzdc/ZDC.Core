using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Quartz;
using Serilog;
using ZDC.Core.Data;
using ZDC.Models;

namespace ZDC.Core.Jobs
{
    public class ControllersJob : IJob
    {
        private IConfiguration _configuration;
        private ZdcContext _context;
        private bool _datafile;

        public async Task Execute(IJobExecutionContext context)
        {
            Log.Information($"Controller job running: {DateTime.Now}");

            var scheduler = context.Scheduler.Context;
            _context = (ZdcContext) scheduler.Get("context");
            _configuration = (IConfiguration) scheduler.Get("configuration");

            var lastUpdated = _context.LastUpdated.FirstOrDefault();
            if (lastUpdated == null)
                await _context.LastUpdated.AddAsync(new LastUpdated
                {
                    Time = DateTime.UtcNow
                });
            else
                lastUpdated.Time = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var json = await GetDataFile();

            if (_datafile)
            {
                var controllers = await DeserializeControllers(json);
                await UpdateOnlineControllers(controllers);
                await CheckControllerLogoff();
                var newControllers = await UpdateAlreadyOnlineControllers(controllers);
                await AddOnlineControllersToDatabase(newControllers);
                await CheckControllerLoas(controllers);
            }
            else
            {
                await UpdateOnlineTimes();
            }
        }

        /// <summary>
        ///     Function to get the datafile
        /// </summary>
        /// <returns>List of JToken objects</returns>
        public async Task<List<JToken>> GetDataFile()
        {
            try
            {
                using var httpClient = new HttpClient();
                using var response = await httpClient.GetAsync(_configuration.GetValue<string>("DataUrl"));
                var content = await response.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(content);
                var results = jsonObject["controllers"]?.Children().ToList();

                if (results == null || results.Count <= 1)
                    _datafile = false;
                else
                    _datafile = true;

                // ReSharper disable once RedundantAssignment
                content = null;
                GC.WaitForPendingFinalizers();
                GC.Collect();

                return results;
            }
            catch (Exception ex)
            {
                Log.Error($"Getting vatusa data file encountered an error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        ///     Function to deserialize all controllers from json response
        /// </summary>
        /// <param name="json">Json response from datafeed</param>
        /// <returns>List of controller log objects</returns>
        public async Task<List<ControllerLog>> DeserializeControllers(List<JToken> json)
        {
            try
            {
                var controllers = new List<ControllerLog>();

                foreach (var client in json)
                {
                    var facility = client.Value<string>("callsign");
                    var frequency = client.Value<string>("frequency");
                    var cid = int.Parse(client.Value<string>("cid") ?? "");
                    var login = client.Value<DateTime>("logon_time");

                    if (facility != null && (!Constants.ControllerPositions.Contains(
                                                 facility.Length > 4 ? facility.Substring(0, 4) : facility) ||
                                             facility.Contains("OBS"))) continue;
                    var user = await _context.Users.FindAsync(cid);

                    if (user != null)
                        controllers.Add(new ControllerLog
                        {
                            Login = login,
                            Logout = DateTime.UtcNow,
                            User = user,
                            Position = facility,
                            Frequency = frequency,
                            Duration = 0.0
                        });
                }

                return controllers;
            }
            catch (Exception ex)
            {
                Log.Error($"Deserializing online controllers encountered an error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        ///     Function for handling of updating controller logs
        /// </summary>
        /// <param name="logs">List of all online zdc controllers</param>
        public async Task UpdateOnlineControllers(List<ControllerLog> logs)
        {
            try
            {
                foreach (var controller in _context.OnlineControllers.ToList())
                    _context.OnlineControllers.Remove(controller);

                await _context.SaveChangesAsync();

                foreach (var controller in logs.ToList())
                {
                    var timeOnline = DateTime.UtcNow - controller.Login;
                    await _context.OnlineControllers.AddAsync(new OnlineController
                    {
                        User = controller.User,
                        Frequency = controller.Frequency,
                        Position = controller.Position,
                        Online = $"{timeOnline.Hours}h {timeOnline.Minutes}m",
                        Created = DateTime.UtcNow
                    });
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error($"Updating online controllers encountered an error: {ex.Message}");
            }
        }

        public async Task CheckControllerLogoff()
        {
            try
            {
                var lastUpdated = _context.LastUpdated.FirstOrDefault()?.Time.AddMinutes(-1);

                foreach (var log in _context.ControllerLogs
                    .AsQueryable()
                    .Where(x => x.Duration == 0.0).ToList())
                    if (log.Logout <= lastUpdated)
                    {
                        var login = log.Login;
                        var logoff = log.Logout;
                        var length = logoff - login;
                        log.Duration = Math.Round(length.TotalHours, 3);

                        var monthHours = _context.Hours
                            .FirstOrDefault(
                                x => x.Month == DateTime.UtcNow.Month
                                     && x.Year == DateTime.UtcNow.Year
                                     && x.User.Id == log.User.Id
                            );

                        if (monthHours == null)
                        {
                            var hours = new Hours
                            {
                                User = log.User,
                                Year = DateTime.UtcNow.Year,
                                Month = DateTime.UtcNow.Month
                            };

                            if (log.Position.Contains("DEL", StringComparison.OrdinalIgnoreCase) ||
                                log.Position.Contains("GND", StringComparison.OrdinalIgnoreCase) ||
                                log.Position.Contains("TWR", StringComparison.OrdinalIgnoreCase))
                                hours.LocalHours += log.Duration;

                            if (log.Position.Contains("APP", StringComparison.OrdinalIgnoreCase) ||
                                log.Position.Contains("DEP", StringComparison.OrdinalIgnoreCase))
                                hours.TraconHours += log.Duration;

                            if (log.Position.Contains("CTR")) hours.CenterHours += log.Duration;

                            await _context.Hours.AddAsync(hours);

                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (log.Position.Contains("DEL", StringComparison.OrdinalIgnoreCase) ||
                                log.Position.Contains("GND", StringComparison.OrdinalIgnoreCase) ||
                                log.Position.Contains("TWR", StringComparison.OrdinalIgnoreCase))
                                monthHours.LocalHours += log.Duration;

                            if (log.Position.Contains("APP", StringComparison.OrdinalIgnoreCase) ||
                                log.Position.Contains("DEP", StringComparison.OrdinalIgnoreCase))
                                monthHours.TraconHours += log.Duration;

                            if (log.Position.Contains("CTR")) monthHours.CenterHours += log.Duration;

                            await _context.SaveChangesAsync();
                        }

                        await _context.SaveChangesAsync();
                    }
            }
            catch (Exception ex)
            {
                Log.Error($"Controller Logoff encountered an error: {ex.Message}");
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        ///     Checks if controller already exists.
        ///     If so update time and remove from list.
        /// </summary>
        /// <returns>List without controllers that were already online.</returns>
        public async Task<List<ControllerLog>> UpdateAlreadyOnlineControllers(List<ControllerLog> onlineControllers)
        {
            try
            {
                foreach (var log in onlineControllers.ToList())
                {
                    var controller = _context.ControllerLogs
                        .AsQueryable()
                        .Where(x => x.User.Id == log.User.Id)
                        .Where(x => x.Position.Equals(log.Position))
                        .Where(x => x.Login.Equals(log.Login))
                        .FirstOrDefault(x => x.Frequency.Equals(log.Frequency));

                    if (controller != null)
                    {
                        controller.Logout = DateTime.UtcNow;
                        onlineControllers.Remove(log);
                    }
                }

                await _context.SaveChangesAsync();

                return onlineControllers;
            }
            catch (Exception ex)
            {
                Log.Error($"Updating existing online controllers encountered an error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        ///     Adds online controllers to database.
        ///     If controller is already being tracked, update their time.
        /// </summary>
        public async Task AddOnlineControllersToDatabase(List<ControllerLog> newControllers)
        {
            try
            {
                foreach (var log in newControllers.ToList())
                    await _context.ControllerLogs.AddAsync(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error($"Adding new online controller encountered an error: {ex.Message}");
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        ///     Function to check if an online controller has an LOA
        /// </summary>
        /// <param name="controllers">List of new controllers</param>
        public async Task CheckControllerLoas(List<ControllerLog> controllers)
        {
            try
            {
                foreach (var controller in controllers)
                {
                    var loa = controller.User.Loas
                        .Where(x => x.Start <= DateTime.UtcNow)
                        .Where(x => x.End >= DateTime.UtcNow)
                        .FirstOrDefault(x => x.Status == LoaStatus.Started);
                    if (loa == null) continue;
                    controller.User.Status = UserStatus.Active;
                    loa.Status = LoaStatus.Controlled;
                    // todo send email
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Checking if controller is on LOA encountered an error: {ex.Message}");
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        ///     Function to update the logoff time of controller logs
        ///     Will only be used if datafile is down so we don't loose online time
        ///     May add more hours than actually controller but it's better
        ///     than loosing hours.
        /// </summary>
        public async Task UpdateOnlineTimes()
        {
            try
            {
                foreach (var log in _context.ControllerLogs
                    .AsQueryable()
                    .Where(x => x.Duration == 0.0).ToList())
                    log.Logout = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error($"Updating controller online time encountered an error: {ex.Message}");
                await _context.SaveChangesAsync();
            }
        }
    }
}
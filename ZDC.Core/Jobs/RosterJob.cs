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
using ZDC.Core.Models;

namespace ZDC.Core.Jobs
{
    public class RosterJob : IJob
    {
        private IConfiguration _configuration;
        private ZdcContext _context;

        public async Task Execute(IJobExecutionContext context)
        {
            Log.Information($"Roster job running: {DateTime.Now}");

            var scheduler = context.Scheduler.Context;
            _context = (ZdcContext) scheduler.Get("context");
            _configuration = (IConfiguration) scheduler.Get("configuration");

            await RosterUpdate();
        }

        public async Task RosterUpdate()
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(
                $"{_configuration.GetValue<string>("RosterUrl")}?apikey={_configuration.GetValue<string>("VatusaApiKey")}");
            var content = await response.Content.ReadAsStringAsync();
            var roster = JObject.Parse(content)["data"]?.Children();

            content = null;
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var users = new List<User>();

            if (roster != null)
                foreach (var user in roster)
                {
                    var cid = user.Value<int>("cid");

                    if (await _context.Users.FindAsync(cid) != null)
                        continue;

                    var firstName = user.Value<string>("fname");
                    var lastName = user.Value<string>("lname");
                    var email = user.Value<string>("email");
                    UserRating rating;
                    if (Enum.IsDefined(typeof(UserRating), user.Value<int>("rating")))
                        rating = (UserRating) user.Value<int>("rating");
                    else
                        rating = UserRating.OBS;
                    var joinDate = user.Value<DateTime>("facility_join");
                    var homeController = user.Value<bool>("flag_homecontroller");

                    await _context.AddAsync(new User
                    {
                        Id = cid,
                        LastName = lastName,
                        FirstName = firstName,
                        Initials = GetInitials(firstName, lastName),
                        Email = email,
                        UserRating = rating,
                        Role = UserRole.None,
                        TrainingRole = TrainingRole.None,
                        Training = true,
                        Events = true,
                        Visitor = homeController,
                        VisitorFrom = string.Empty,
                        Status = UserStatus.Active,
                        Joined = joinDate,
                        Created = DateTime.UtcNow,
                        Updated = DateTime.UtcNow
                    });
                }
        }

        public string GetInitials(string firstName, string lastName)
        {
            var initials = $"{firstName[0]}{lastName[0]}";

            var initialsExist = _context.Users.AsQueryable()
                .Where(x => x.Initials.Equals(initials, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!initialsExist.Any()) return initials;

            foreach (var letter in lastName)
            {
                initials = $"{firstName[0]}{letter.ToString().ToUpper()}";

                var exists = _context.Users.AsQueryable()
                    .Where(x => x.Initials.Equals(initials, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (!exists.Any()) return initials;
            }

            foreach (var letter in firstName)
            {
                initials = $"{letter.ToString().ToUpper()}{lastName[0]}";

                var exists = _context.Users.AsQueryable()
                    .Where(x => x.Initials.Equals(initials, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (!exists.Any()) return initials;
            }

            return string.Empty;
        }
    }
}
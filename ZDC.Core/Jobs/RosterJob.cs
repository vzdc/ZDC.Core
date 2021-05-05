using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Quartz;
using Serilog;
using ZDC.Core.Data;
using ZDC.Models;

namespace ZDC.Core.Jobs
{
    [DisallowConcurrentExecution]
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

            if (roster != null)
                foreach (var user in roster)
                {
                    var cid = user.Value<int>("cid");

                    var dbUser = await _context.Users.FindAsync(cid);
                    if (dbUser != null)
                    {
                        dbUser.FirstName = user.Value<string>("fname");
                        dbUser.LastName = user.Value<string>("lname");
                        dbUser.Email = user.Value<string>("email");
                        if (Enum.IsDefined(typeof(UserRating), user.Value<int>("rating")))
                            dbUser.UserRating = (UserRating) user.Value<int>("rating");
                        else
                            dbUser.UserRating = UserRating.OBS;
                        dbUser.Visitor = user.Value<string>("membership")?.Equals("visit") ?? false;
                        if (dbUser.Visitor)
                            dbUser.VisitorFrom = user.Value<string>("facility");
                        await _context.SaveChangesAsync();
                        continue;
                    }

                    var firstName = user.Value<string>("fname");
                    var lastName = user.Value<string>("lname");
                    var email = user.Value<string>("email");
                    UserRating rating;
                    if (Enum.IsDefined(typeof(UserRating), user.Value<int>("rating")))
                        rating = (UserRating) user.Value<int>("rating");
                    else
                        rating = UserRating.OBS;
                    var certification = new Certification();
                    var joinDate = user.Value<DateTime>("facility_join");
                    var visitor = user.Value<string>("membership")?.Equals("visit") ?? false;
                    var visitorFrom = string.Empty;
                    if (visitor)
                        visitorFrom = user.Value<string>("facility");

                    await _context.Users.AddAsync(new User
                    {
                        Id = cid,
                        LastName = lastName,
                        FirstName = firstName,
                        Initials = await GetInitials(firstName, lastName),
                        Email = email,
                        UserRating = rating,
                        Role = UserRole.None,
                        TrainingRole = TrainingRole.None,
                        Certifications = certification,
                        Training = true,
                        Events = true,
                        Visitor = visitor,
                        VisitorFrom = visitorFrom,
                        Status = UserStatus.Active,
                        Joined = joinDate,
                        Created = DateTime.UtcNow,
                        Updated = DateTime.UtcNow
                    });

                    await _context.SaveChangesAsync();
                }
        }

        public async Task<string> GetInitials(string firstName, string lastName)
        {
            var initials = $"{firstName[0]}{lastName[0]}";

            var initialsExist = await _context.Users.AsQueryable()
                .Where(x => x.Initials.Equals(initials))
                .ToListAsync();

            if (!initialsExist.Any()) return initials;

            foreach (var letter in lastName)
            {
                initials = $"{firstName[0]}{letter.ToString().ToUpper()}";

                var exists = await _context.Users.AsQueryable()
                    .Where(x => x.Initials.Equals(initials))
                    .ToListAsync();

                if (!exists.Any()) return initials;
            }

            foreach (var letter in firstName)
            {
                initials = $"{letter.ToString().ToUpper()}{lastName[0]}";

                var exists = await _context.Users.AsQueryable()
                    .Where(x => x.Initials.Equals(initials))
                    .ToListAsync();

                if (!exists.Any()) return initials;
            }

            return string.Empty;
        }
    }
}
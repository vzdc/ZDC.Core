using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Quartz;
using Serilog;
using ZDC.Core.Data;
using ZDC.Core.Models;

namespace ZDC.Core.Jobs
{
    [DisallowConcurrentExecution]
    public class OverflightsJob : IJob
    {
        private IConfiguration _configuration;
        private ZdcContext _context;

        public async Task Execute(IJobExecutionContext context)
        {
            Log.Information($"Overflights job running: {DateTime.Now}");

            var scheduler = context.Scheduler.Context;
            _context = (ZdcContext) scheduler.Get("context");
            _configuration = (IConfiguration) scheduler.Get("configuration");

            var overflights = await GetOverflights();

            _context.Overflights.RemoveRange(await _context.Overflights.ToListAsync());

            await _context.Overflights.AddRangeAsync(overflights);

            await _context.SaveChangesAsync();
        }

        public async Task<IList<Overflight>> GetOverflights()
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(_configuration.GetValue<string>("OverflightUrl"));
            var content = await response.Content.ReadAsStringAsync();
            var array = JArray.Parse(content);

            return array.Select(overflight => new Overflight
                {
                    Callsign = overflight.Value<string>("callsign"),
                    Departure = overflight.Value<string>("dep"),
                    Arrival = overflight.Value<string>("arr"),
                    Route = overflight.Value<string>("route")?.Replace('+', ' ').TrimEnd().TrimStart(),
                    Latitude = overflight.Value<string>("lat"),
                    Longitude = overflight.Value<string>("lon")
                })
                .ToList();
        }
    }
}
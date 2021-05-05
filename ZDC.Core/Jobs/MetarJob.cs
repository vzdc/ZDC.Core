using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Quartz;
using Serilog;
using ZDC.Core.Data;
using ZDC.Core.Extensions;
using ZDC.Models;

namespace ZDC.Core.Jobs
{
    [DisallowConcurrentExecution]
    public class MetarJob : IJob
    {
        private IConfiguration _configuration;
        private ZdcContext _context;

        public async Task Execute(IJobExecutionContext context)
        {
            Log.Information($"Metar job running: {DateTime.Now}");

            var scheduler = context.Scheduler.Context;
            _context = (ZdcContext) scheduler.Get("context");
            _configuration = (IConfiguration) scheduler.Get("configuration");
            await UpdateMetar();
        }

        public async Task UpdateMetar()
        {
            var airports = _context.Airports.ToList();

            _context.Metar.Clear();

            foreach (var airport in airports)
            {
                using var httpClient = new HttpClient();
                using var response =
                    await httpClient.GetAsync($"{_configuration.GetValue<string>("MetarUrl")}{airport.Icao}");
                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);

                var gusting = string.Empty;
                if (json.SelectToken("wind_gust").Children().Any())
                    gusting = $" Gusting {json.SelectToken("wind_gust.repr")}";

                airport.Metar = new Metar
                {
                    MetarRaw = json.Value<string>("sanitized"),
                    Conditions = json.Value<string>("flight_rules"),
                    Wind = $"{json.SelectToken("wind_direction.repr")}° at " +
                           $"{json.SelectToken("wind_speed.repr")} Knots" +
                           $"{gusting}",
                    Altimeter =
                        $"{json.SelectToken("altimeter.repr").ToString().Replace("A", "").Insert(2, ".")} inHg",
                    Temp = $"{json.SelectToken("temperature.repr")}"
                };
            }

            await _context.SaveChangesAsync();
        }
    }
}
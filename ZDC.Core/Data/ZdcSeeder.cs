using System;
using System.Collections.Generic;
using System.Linq;
using ZDC.Models;

namespace ZDC.Core.Data
{
    public class ZdcSeeder
    {
        public static async void SeedDatabase(ZdcContext context)
        {
            if (context.Airports.Any()) return;

            var airports = new List<Airport>
            {
                new()
                {
                    Icao = "KIAD",
                    Name = "Dulles International Airport",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                },
                new()
                {
                    Icao = "KDCA",
                    Name = "Ronald Reagan International Airport",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                },
                new()
                {
                    Icao = "KBWI",
                    Name = "Baltimore-Washington International Airport",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                },
                new()
                {
                    Icao = "KORF",
                    Name = "Norfolk International Airport",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                }
            };

            await context.Airports.AddRangeAsync(airports);
            await context.SaveChangesAsync();
        }
    }
}
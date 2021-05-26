using System;
using System.Collections.Generic;
using System.Linq;
using ZDC.Core.Models;

namespace ZDC.Core.Data
{
    public class ZdcSeeder
    {
        public static async void SeedDatabase(ZdcContext context)
        {
            if (!context.Airports.Any())
            {
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
            }

            if (!context.Facilities.Any())
            {
                var facilities = new List<Facility>
                {
                    new()
                    {
                        Name = "DC Center",
                        Online = false
                    },
                    new()
                    {
                        Name = "PCT Tracon",
                        Online = false
                    },
                    new()
                    {
                        Name = "DCA ATCT",
                        Online = false
                    },
                    new()
                    {
                        Name = "IAD ATCT",
                        Online = false
                    },
                    new()
                    {
                        Name = "BWI ATCT",
                        Online = false
                    }
                };

                await context.Facilities.AddRangeAsync(facilities);
            }

            await context.SaveChangesAsync();
        }
    }
}
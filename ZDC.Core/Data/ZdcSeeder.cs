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

            if (!context.Positions.Any())
            {
                var positions = new List<Position>
                {
                    new()
                    {
                        Name = "DC_C",
                        Level = Level.Center
                    },
                    new()
                    {
                        Name = "DC_0",
                        Level = Level.Center
                    },
                    new()
                    {
                        Name = "DC_1",
                        Level = Level.Center
                    },
                    new()
                    {
                        Name = "DC_2",
                        Level = Level.Center
                    },
                    new()
                    {
                        Name = "DC_5",
                        Level = Level.Center
                    },
                    new()
                    {
                        Name = "DC_N",
                        Level = Level.Center
                    },
                    new()
                    {
                        Name = "DC_S",
                        Level = Level.Center
                    },
                    new()
                    {
                        Name = "DC_E",
                        Level = Level.Center
                    },
                    new()
                    {
                        Name = "DC_W",
                        Level = Level.Center
                    },
                    new()
                    {
                        Name = "DC_I",
                        Level = Level.Center
                    },
                    new()
                    {
                        Name = "DCA_",
                        Level = Level.Bravo
                    },
                    new()
                    {
                        Name = "IAD_",
                        Level = Level.Bravo
                    },
                    new()
                    {
                        Name = "BWI_",
                        Level = Level.Bravo
                    },
                    new()
                    {
                        Name = "PCT_",
                        Level = Level.Bravo
                    },
                    new()
                    {
                        Name = "ADW_",
                        Level = Level.Bravo
                    },
                    new()
                    {
                        Name = "RIC_",
                        Level = Level.Charlie
                    },
                    new()
                    {
                        Name = "ROA_",
                        Level = Level.Charlie
                    },
                    new()
                    {
                        Name = "ORF_",
                        Level = Level.Charlie
                    },
                    new()
                    {
                        Name = "ACY_",
                        Level = Level.Charlie
                    },
                    new()
                    {
                        Name = "NGU_",
                        Level = Level.Charlie
                    },
                    new()
                    {
                        Name = "NTU_",
                        Level = Level.Charlie
                    },
                    new()
                    {
                        Name = "NHK_",
                        Level = Level.Charlie
                    },
                    new()
                    {
                        Name = "RDU_",
                        Level = Level.Charlie
                    },
                    new()
                    {
                        Name = "CHO_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "HGR_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "LYH_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "EWN_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "LWB_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "ISO_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "MTN_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "HEF_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "MRB_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "PHF_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "SBY_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "NUI_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "FAY_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "ILM_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "NKT_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "NCA_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "NYG_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "DAA_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "DOV_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "POB_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "GSB_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "WAL_",
                        Level = Level.Delta
                    },
                    new()
                    {
                        Name = "CVN_",
                        Level = Level.Delta
                    }
                };

                await context.Positions.AddRangeAsync(positions);
            }

            await context.SaveChangesAsync();
        }
    }
}
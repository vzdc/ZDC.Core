using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Serilog;
using ZDC.Core.Data;
using ZDC.Models;

namespace ZDC.Core.Jobs
{
    public class FacilitiesJob : IJob
    {
        private ZdcContext _context;

        public async Task Execute(IJobExecutionContext context)
        {
            Log.Information($"Facilities job running: {DateTime.Now}");

            var scheduler = context.Scheduler.Context;
            _context = (ZdcContext) scheduler.Get("context");

            var online = await _context.OnlineControllers.Select(x => x.Position).ToListAsync();
            var facilities = new List<Facility>();

            foreach (var position in online)
            {
                if (position.Contains("DCA") &&
                    (position.Contains("DEL") || position.Contains("GND") || position.Contains("TWR")))
                    facilities.Add(_context.Facilities.First(x => x.Name.Equals("DCA ATCT")));
                if (position.Contains("IAD") &&
                    (position.Contains("DEL") || position.Contains("GND") || position.Contains("TWR")))
                    facilities.Add(_context.Facilities.First(x => x.Name.Equals("IAD ATCT")));
                if (position.Contains("BWI") &&
                    (position.Contains("DEL") || position.Contains("GND") || position.Contains("TWR")))
                    facilities.Add(_context.Facilities.First(x => x.Name.Equals("BWI ATCT")));

                if ((position.Contains("PCT") || position.Contains("IAD") || position.Contains("BWI") ||
                     position.Contains("DCA")) &&
                    (position.Contains("APP") || position.Contains("DEP")))
                    facilities.Add(_context.Facilities.First(x => x.Name.Equals("PCT Tracon")));
                if (position.Contains("DC") && position.Contains("CTR"))
                    facilities.Add(_context.Facilities.First(x => x.Name.Equals("DC Center")));
            }

            foreach (var facility in _context.Facilities.ToList())
            {
                var found = facilities.FirstOrDefault(x => x.Name == facility.Name);
                facility.Online = found != null;
            }

            await _context.SaveChangesAsync();
        }
    }
}
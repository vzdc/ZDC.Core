using System;
using System.Collections.Specialized;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using ZDC.Core.Data;

namespace ZDC.Core.Jobs
{
    public class JobService
    {
        private readonly IConfiguration _config;
        private readonly ZdcContext _context;

        public JobService(ZdcContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async void StartJobs()
        {
            var props = new NameValueCollection
            {
                {"quartz.serializer.type", "binary"}
            };
            var factory = new StdSchedulerFactory(props);
            var scheduler = factory.GetScheduler().Result;

            scheduler.Context.Put("context", _context);
            scheduler.Context.Put("configuration", _config);

            var controllersJob = JobBuilder.Create<ControllersJob>()
                .WithIdentity("ControllersJob", "Jobs")
                .Build();

            var controllersTrigger = TriggerBuilder.Create()
                .WithIdentity("ControllersTrigger", "Jobs")
                .StartAt(DateTime.UtcNow.AddSeconds(10))
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(_config.GetValue<int>("DatafileInterval"))
                    .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(controllersJob, controllersTrigger);

            var rosterJob = JobBuilder.Create<RosterJob>()
                .WithIdentity("RosterJob", "Jobs")
                .Build();

            var rosterTrigger = TriggerBuilder.Create()
                .WithIdentity("RosterTrigger", "Jobs")
                .StartAt(DateTime.UtcNow.AddSeconds(20))
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(_config.GetValue<int>("RosterInterval"))
                    .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(rosterJob, rosterTrigger);

            var metarJob = JobBuilder.Create<MetarJob>()
                .WithIdentity("MetarJob", "Jobs")
                .Build();

            var metarTrigger = TriggerBuilder.Create()
                .WithIdentity("MetarTrigger", "Jobs")
                .StartAt(DateTime.UtcNow.AddSeconds(30))
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(_config.GetValue<int>("DatafileInterval"))
                    .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(metarJob, metarTrigger);

            await scheduler.Start();
        }
    }
}
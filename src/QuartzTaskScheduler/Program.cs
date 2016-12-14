using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace QuartzTaskScheduler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("task Scheduler Begins");

            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler, start the schedular before triggers or anything else
            IScheduler sched = schedFact.GetScheduler().Result;
            sched.Start();

            // create job
            IJobDetail job = JobBuilder.Create<HelloWorldJob>()
                    .WithIdentity("job1", "group1")
                    .Build();

            // create trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever())
                .Build();

            // Schedule the job using the job and trigger 
            sched.ScheduleJob(job, trigger);


            Console.ReadLine();

            Console.WriteLine("Program ends successfully");
        }
    }
}

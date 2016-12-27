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
            Console.WriteLine("Task Scheduler Begins");

            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler, start the schedular before triggers or anything else
            IScheduler sched = schedFact.GetScheduler().Result;
            sched.Start();

            // create job
            IJobDetail job = JobBuilder.Create<HelloWorldJob>()
                    .WithIdentity("job1", "group1")
                    .Build();
            
            int option = 9;
            HashSet<ITrigger> triggers = new HashSet<ITrigger>();
            //ITrigger trigger = TriggerBuilder.Create().Build();
            ITrigger trigger;
            while (option != 4)
            {
                Console.Write("1. Fire an one time event base on a specific interval specified by the user. \n" +
                "2. Fire events on a periodic interval(every 5, 10, 15, 20 minutes) using the top of the hour as the starting point. \n" +
                "3. Fire events on specific days Monday, Wednesday and Friday at a specific times. \n"+
                "4. Exit \n"+
                "Enter Option: ");

                #region User Input
                bool tryAgain = true;
                while (tryAgain)
                {
                    try
                    {
                        option = Int32.Parse(Console.ReadLine());
                        tryAgain = false;
                    }
                    catch (System.FormatException e)
                    {
                        Console.WriteLine("Invalid input. Try again");
                    }
                }
                #endregion

                switch (option)
                {
                    case 1:
                        #region Case1
                        Console.WriteLine("Firing an event 2 seconds in the future (OR) at a specific time");

                        trigger = TriggerBuilder.Create()
                           .WithIdentity("trigger1", "group1")
                           .StartAt(DateTime.Now.AddSeconds(2))
                           .Build();

                        sched.ScheduleJob(job, trigger); // Schedule the job using the job and trigger 

                        Console.ReadLine();

                        sched.UnscheduleJob(trigger.Key); // Unschedule job
                        break;
                        #endregion
                    case 2:
                        #region Case2
                        Console.WriteLine("Firing an event every 1 second. The trigger will begin after 2 seconds and will continue until 10 seconds from the start time. (Press ENTER key to stop)");

                        trigger = TriggerBuilder.Create()
                        .WithIdentity("trigger1", "group1")
                        .StartAt(DateTime.Now.AddSeconds(2))
                        .WithSimpleSchedule(x => x.WithIntervalInSeconds(1).RepeatForever())
                        .EndAt(DateTime.Now.AddSeconds(10))
                        .Build();

                        sched.ScheduleJob(job, trigger); // Schedule the job using the job and trigger 

                        Console.ReadLine();

                        sched.UnscheduleJob(trigger.Key); // Unschedule job
                        break;
                        #endregion
                    case 3:
                        #region Case3
                        // Scheduling events using CRON expressions
                        Console.WriteLine("Fire events on specific days Monday, Wednesday and Friday at a specific times. (Press ENTER key to stop)");

                        trigger = TriggerBuilder.Create()
                        .WithIdentity("trigger1", "group1")
                        .StartAt(DateTime.Now)
                        .WithCronSchedule("0/30 55-59 12 ? * TUE,WED")
                        .Build();

                        triggers.Add(trigger);

                        trigger = TriggerBuilder.Create()
                        .WithIdentity("trigger2", "group1")
                        .StartAt(DateTime.Now)
                        .WithCronSchedule("0/7 56-59 12 ? * TUE,THU")
                        .Build();

                        triggers.Add(trigger);

                        trigger = TriggerBuilder.Create()
                        .WithIdentity("trigger3", "group1")
                        .StartAt(DateTime.Now)
                        .WithCronSchedule("0/10 57-49 12 ? * THU,FRI")
                        .Build();

                        triggers.Add(trigger);
                        
                        sched.ScheduleJob(job, triggers, false); // Schedule the job using the job and trigger list 

                        Console.ReadLine();

                        sched.UnscheduleJobs(triggers.Select(x => x.Key).ToList()); // Unschedule all jobs associated with the triggers
                        break;
                    #endregion
                }
            }
        }
    }
}

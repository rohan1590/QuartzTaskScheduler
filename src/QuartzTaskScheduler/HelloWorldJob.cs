using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Common.Logging;


namespace QuartzTaskScheduler
{
    public class HelloWorldJob : IJob
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HelloWorldJob));

        /// <summary> 
        /// Empty constructor for job initilization
        /// <para>
        /// Quartz requires a public empty constructor so that the
        /// scheduler can instantiate the class whenever it needs.
        /// </para>
        /// </summary>
        public HelloWorldJob()
        {

        }

        //public Task Execute(IJobExecutionContext context)
        Task IJob.Execute(IJobExecutionContext context)
        {
            try
            {
                Console.WriteLine("Executing : "+ DateTime.Now.ToString("HH:mm:ss"));
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;
        }
    }
}

using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TimerTest563
{
    public class timer
    {
        private readonly ILogger _logger;

        public timer(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<timer>();
        }

        [Function("timer")]
        public void Run([TimerTrigger("*/10 * * * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            int maxRetries = 3;
            int retryCount = 0;

            while (retryCount < maxRetries)
            {
                try
                {
                    if (Environment.GetEnvironmentVariable("fail") == "true" && retryCount < 2)
                    {
                        throw new Exception("fail set to : " + "true");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.ToString());
                }

                retryCount++;
            }

            //_logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MeetupCCDays03242021.CodeSamples.FunctionApp
{
    public class WeatherRequest
    {
        public string City { get; set; }
    }

    public class DailyWeather
    {
        public DateTime Date { get; set; }
        public double CelciusLow { get; set; }
        public double CelciusHigh { get; set; }
    }

    public class WeatherResponse
    {
        public string City { get; set; }
        public IEnumerable<DailyWeather> DailyWeatherReport { get; set; }
    }

    public static class HttpTriggerCSharp1
    {
        [FunctionName("HttpTriggerCosmosInputCSharp1")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB("mydb1", "mycollection1",
                ConnectionStringSetting = "AzureWebJobsCosmosDB",
                SqlQuery = "select * from c")] // replace later as appropriate
                IEnumerable<DailyWeather> dailyWeathers,
            ILogger log)
        {
            foreach (var dailyWeather in dailyWeathers)
            {
                log.LogInformation($"document: {JsonConvert.SerializeObject(dailyWeather)}");
                dailyWeather.CelciusHigh = 50.1;
                dailyWeather.Date = DateTime.Now.AddDays(1);
            }

            return new OkResult();
        }
    }
}

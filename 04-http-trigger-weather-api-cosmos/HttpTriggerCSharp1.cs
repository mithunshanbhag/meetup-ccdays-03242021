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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "city/{city}")] HttpRequest req,
            [CosmosDB("mydb1", "mycollection1",
                ConnectionStringSetting = "AzureWebJobsCosmosDB",
                SqlQuery = "select * from c where c.city = {city}")] // replace later as appropriate
                IEnumerable<DailyWeather> dailyWeathers,
            string city,
            ILogger log)
        {
            var dailyWeatherReport = new List<DailyWeather>();

            foreach (var dailyWeather in dailyWeathers)
            {
                dailyWeatherReport.Add(dailyWeather);
            }

            return new OkObjectResult(new WeatherResponse
            {
                City = city,
                DailyWeatherReport = dailyWeatherReport,
            });
        }
    }
}

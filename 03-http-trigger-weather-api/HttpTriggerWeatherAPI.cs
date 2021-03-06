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
using System.Linq;

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

    public static class HttpTriggerWeatherAPI
    {
        [FunctionName("HttpTriggerWeatherAPI")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] WeatherRequest req,
            ILogger log)
        {
            var weatherReport = Enumerable
                .Range(0, 7)
                .Select(x => new DailyWeather
                {
                    Date = DateTime.Now.AddDays(x),
                    CelciusLow = new Random().Next(20,30),
                    CelciusHigh = new Random().Next(30,40),
                });

            var weatherResponse = new WeatherResponse
            {
                City = req.City,
                DailyWeatherReport = weatherReport,
            };

            return new OkObjectResult(weatherResponse);
        }
    }
}

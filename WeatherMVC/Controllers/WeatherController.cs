using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherMVC.Models;
using WeatherMVC.Services;

namespace WeatherMVC.Controllers
{
    public class WeatherController : Controller
    {
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherService) {
            _weatherService = weatherService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Weather(int zipcode){
            IEnumerable<WeatherModel> weathers =  _weatherService.GetWeatherForZipcode(zipcode);
            return View(weathers);
        }
    }
}

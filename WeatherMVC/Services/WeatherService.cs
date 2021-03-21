using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherMVC.Models;

namespace WeatherMVC.Services
{
    public class WeatherService
    {

        public WeatherService() {

        }

        public IEnumerable<WeatherModel> GetWeatherForZipcode(int zipcode) {
            
            return Enumerable.Empty<WeatherModel>();
        }
    }
}

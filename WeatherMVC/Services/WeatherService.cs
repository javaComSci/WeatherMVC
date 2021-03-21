using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Net.Http.Headers;
using WeatherMVC.Models;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherMVC.Services
{
    public class WeatherService
    {

        public WeatherService() {

        }

        public IEnumerable<WeatherModel> Get(string uri)
        {
            Dictionary<string, List<double>> temps = new Dictionary<string, List<double>>();
            Dictionary<string, List<string>> weathers = new Dictionary<string, List<string>>();

            List<WeatherModel> weatherInfos = new List<WeatherModel>();
            
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(uri);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            // List data response.
            HttpResponseMessage response = client.GetAsync(uri).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            string responseBody = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                
                // Parse the response body.
                dynamic res = JsonConvert.DeserializeObject<dynamic>(responseBody);
                Console.WriteLine(res);
                List<dynamic> listy = res["list"].ToObject<List<object>>();

                for(int i = 0; i < listy.Count; i++) {
                    var currDate = listy[i].dt_txt;
                    var currD = currDate.ToString().Split(' ')[0];

                    var newTemp = listy[i].main.temp;
                    var newMain = listy[i].weather[0].main; 

                    if(temps.ContainsKey(currD)) {
                        temps[currD].Add((double)newTemp);

                        weathers[currD].Add((string)newMain);
                    } else {
                        temps[currD] = new List<double>();
                        temps[currD].Add((double)newTemp);

                        weathers[currD] = new List<string>();
                        weathers[currD].Add((string)newMain);
                    }
                }
            }

            Console.WriteLine("The weather models and keys");
            Console.WriteLine(temps);
            Console.WriteLine(temps.Keys);
            foreach(var item in temps.Keys) {
                Console.WriteLine(item);
                double t = temps[item].Average();
                string w = weathers[item].GroupBy(x => x)
                    .OrderByDescending(x => x.Count()).ThenBy(x => x.Key)
                    .Select(x => (string?)x.Key)
                    .FirstOrDefault();
                
                Console.WriteLine(item);
                Console.WriteLine(t);
                Console.WriteLine(w);
                var weatherModel = new WeatherModel() {
                    Date = item,
                    Temp = t,
                    Weather = w
                };
                weatherInfos.Add(weatherModel);
            }

            Console.WriteLine("FINAL LENGTH");
            Console.WriteLine(weatherInfos.Count());
            return weatherInfos;
        }

        public IEnumerable<WeatherModel> GetWeatherForZipcode(int zipcode) {
            string geturl = "http://api.openweathermap.org/data/2.5/forecast?zip=" + zipcode + ",us&units=imperial&appid=" + "APIKEY";
            return this.Get(geturl);
        }
    }
}

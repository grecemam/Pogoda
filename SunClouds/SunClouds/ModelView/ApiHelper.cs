using Newtonsoft.Json;
using SunClouds.Models;
using System.Net.Http;


namespace SunClouds.ModelView
{
    class ApiHelper
    {
        public WeatherModel.Root GetData(string city)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage message = client.GetAsync($"https://api.weatherapi.com/v1/forecast.json?key=474aea72831943db9f5124545232106&q=" + city + "&days=1&aqi=no&alerts=no").Result;
            string response = message.Content.ReadAsStringAsync().Result;
            WeatherModel.Root obj = JsonConvert.DeserializeObject<WeatherModel.Root>(response);
            return obj;
        }
    }
}
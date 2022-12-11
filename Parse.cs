using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lefty
{
    class Joke
    {
        public string setup { get; set; } //initializate setup
        public string punchline { get; set; } //initializate punchline
    }
    public class WeatherDescription
    {
        public string country { get; set; }
        public double temp { get; set; }
        public string description { get; set; }
    }
    class WeatherInfo
    {
        public string name { get; set; }
        public WeatherDescription Main { get; set; }
        public List<WeatherDescription> Weather { get; set; }
    }
}

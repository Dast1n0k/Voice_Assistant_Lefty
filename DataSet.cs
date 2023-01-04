using System.Collections.Generic;

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

    class NewsDescription
    {
        public string title { get; set; }
    }

    class NewsInfo
    {
        public List<NewsDescription> results { get; set; }
    }

    class CourseDescription
    {
        public string ccy { get; set; }
        public double buy { get; set; }
    }
}

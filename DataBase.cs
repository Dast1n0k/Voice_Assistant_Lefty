using System;
using System.Data;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Lefty
{

    
    class DataSet 
    { 
        
        String[] greetings = new String[4] { "Hello", "Hi", "Hi, how are you", "I'm here, how can I help you" };
        String[] heads_or_tails = new String[2] { "tail","head" };
        String Error = "Lost internet connection"; //Error: without internet

        public String greetings_action()
        {
            Random r = new Random();

            return greetings[r.Next(3)];
        }

        public String get_jokes() //joke parse
        {
            string url = "https://official-joke-api.appspot.com/random_joke";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();

                using (var responseReader = new StreamReader(webStream))
                {
                    var response = responseReader.ReadToEnd();
                    Joke joke = JsonConvert.DeserializeObject<Joke>(response);

                    return joke.setup + " " + joke.punchline;
                }
            }
            catch
            {
                return Error;
            }
        }
        public String get_weather() //weather parse
        {
            string url = "http://api.openweathermap.org/data/2.5/weather?q=Kyiv&units=metric&appid=6fa095c114c44b8983cf448560847507";

            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string response;
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }
                WeatherInfo weatherResponse = JsonConvert.DeserializeObject<WeatherInfo>(response);

                return "In city " + weatherResponse.name + " " + weatherResponse.Weather[0].description + " " + "temperature" + " " + Math.Round(weatherResponse.Main.temp) + " "  + "degrees Celsius";
            }
            catch
            {
                return Error;
            }

        }
        public String get_news() //news parse
        {
            string url = "https://api.nytimes.com/svc/topstories/v2/home.json?api-key=xNdLOT46LfLS2I1cIOv5IVHXqaF2vdDE";

            try
            {


                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string response;
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }
                NewsInfo newsResponse = JsonConvert.DeserializeObject<NewsInfo>(response);
                return newsResponse.results[1].title;
            }
            catch
            {
                return Error;
            }
        }
        public String toss_a_coin() //toss coin
        {
            Random random = new Random();
            return heads_or_tails[random.Next(2)];

        }
    }
    
}

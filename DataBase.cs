using System;
using System.Data;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Lefty
{

    
    class DataSet 
    { 
        
        String[] greetings = new String[4] { "Hello", "Hi", "Hi, how are you", "I'am here, how can I help you" };

        public String greetings_action()
        {
            Random r = new Random();

            return greetings[r.Next(3)];
        }

        public String get_jokes()
        {
            string url = "https://official-joke-api.appspot.com/random_joke";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            var webResponse = request.GetResponse();
            var webStream = webResponse.GetResponseStream();

            using (var responseReader = new StreamReader(webStream))
            {
                var response = responseReader.ReadToEnd();
                Joke joke = JsonConvert.DeserializeObject<Joke>(response);

                return joke.setup + "\n"+ joke.punchline;
            }

        }
    }
    
}

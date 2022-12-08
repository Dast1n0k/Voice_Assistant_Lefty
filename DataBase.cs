using System;
using System.Data;

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
    }
    
}

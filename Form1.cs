using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Diagnostics;
using System.Xml;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;

namespace Lefty
{
    public partial class Form1 : Form
    {
        SpeechSynthesizer s = new SpeechSynthesizer();//Add voice of assistant
        Boolean wake = false; //statement of voice assistant
        Choices list = new Choices();//create list of commands
        DataSet data = new DataSet();// create obj of class dataset
        public Form1()

        {
            //create obj of class for recognize your voice and words
            SpeechRecognitionEngine rec = new SpeechRecognitionEngine(new CultureInfo("en-US"));
            // list of commands 
            list.Add(File.ReadAllLines(@"Voice Bot Commands\commands.txt")); 

            //create a obj of class for grammar of voice bot
            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = new System.Globalization.CultureInfo("en-US"); // add Culture Info

            gb.Append(list);//add to grammar our commands 
            Grammar g = new Grammar(gb);
            try
            {
                rec.RequestRecognizerUpdate();
                rec.LoadGrammar(g);//load this grammar to bot that recognizer understand what commands I need and what bot need to recognized
                rec.SpeechRecognized += rec_SpeachRecognized;
                rec.SetInputToDefaultAudioDevice();
                rec.RecognizeAsync(RecognizeMode.Multiple);

            }
            catch { return; }


            CultureInfo myLang = new CultureInfo("en-US");

            s.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Senior, 60, myLang);//Choice the voice bot gender, age and lang


            InitializeComponent();
        }
        // func for saying
        public void say(String h)
        {

            s.SpeakAsync(h);
            wake = false;
            listBox2.Items.Add(h);
        }

        public void search_music (String s)
        {
            
            say("What music to you want?");
            wake = true;
            Process.Start($"https://soundcloud.com/search?q={s}");
        }
        //commands
        private void rec_SpeachRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            String speech = e.Result.Text;//convert your voice to text
            listBox1.Items.Add(speech);//add your text to input box
            
            if (speech == "lefty")
            {
                label3.Text = "State: Awake";
                wake = true;
                listBox2.Items.Add("I am listening");
            }

            if (speech == "sleep") 
            { 
                wake = false;
                label3.Text = "State: Sleeping";
                say("Just say Lefty if you need me");
            }


            if (wake == true)
            {
            

                if (speech == "hello")
                {
                    say(data.greetings_action());// func from database
                }

                if (speech == "search music")
                {
                    
                    search_music(speech);

                }

                if (speech == "what time is it")
                {
                    say(DateTime.Now.ToString("h.mm tt"));
                }

                if (speech == "what is today")
                {
                    say(DateTime.Now.ToString("M/d/yyyy"));
                }

                if (speech == "how are you")
                {
                    say("Great, and you?");

                }

                if (speech == "open google")
                {
                    Process.Start("https://www.google.com/");
                }

                if (speech == "weather")
                {
                    //continue
                }

                if (speech == "show commands")
                {
                    
                    MessageBox.Show(" Hello\n\n How are you\n\n What time is it\n\n What is today\n\n Open google\n\n Wake\n\n Sleep\n\n Weather\n\n What about weather\n\n Lefty\n\n Show commands\n");
                 
                }
                if (speech == "joke")
                {
                    say(data.get_jokes());
                }
                
            }
            else
            {
                listBox2.Items.Add("Just say Lefty if you need me");
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

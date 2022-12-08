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

namespace Lefty
{
    public partial class Form1 : Form
    {
        String condition;
        String temp;
        SpeechSynthesizer s = new SpeechSynthesizer();
        Boolean wake = false;
        Choices list = new Choices();

        public Form1()

        {

            SpeechRecognitionEngine rec = new SpeechRecognitionEngine(new CultureInfo("en-US"));

            list.Add(new String[] {"hello", "how are you", "what time is it", "what is today", "open google", "wake", "sleep",
            "weather", "what about weather", "lefty"
            }); 

           
            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = new System.Globalization.CultureInfo("en-US"); // add Culture Info

            gb.Append(list);
            Grammar g = new Grammar(gb);
            rec.LoadGrammar(g);
            try
            {
                rec.RequestRecognizerUpdate();
                //rec.LoadGrammar(gr);
                rec.SpeechRecognized += rec_SpeachRecognized;
                rec.SetInputToDefaultAudioDevice();
                rec.RecognizeAsync(RecognizeMode.Multiple);

            }
            catch { return; }


            CultureInfo myLang = new CultureInfo("es-US");

            s.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult, 25, myLang);


            InitializeComponent();
        }

        public void say(String h)
        {

            s.Speak(h);
            wake = false;
            listBox2.Items.Add(h);
        }

        private void rec_SpeachRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            String speech = e.Result.Text;
            listBox1.Items.Add(speech);
            if (speech == "lefty")
            {
                wake = true;
                listBox2.Items.Add("I am listening");
            }
           

            if (speech == "wake")
            {
                wake = true;
                label3.Text = "State: Awake";
            }

            if (speech == "sleep") 
            { 
                wake = false;
                label3.Text = "State: Sleeping";
                say("Just say wake, if you need me");
            }

            if (wake == true)
            {
            

                if (speech == "hello")
                {
                    say("I'am here, how can I help you");
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

            }

            else
            {
                listBox2.Items.Add("Say Lefty to wake me");
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

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
using System.Windows;
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
       

            rec.RequestRecognizerUpdate();
            rec.LoadGrammar(g);//load this grammar to bot that recognizer understand what commands I need and what bot need to recognized
            rec.SpeechRecognized += rec_SpeachRecognized;
            rec.SetInputToDefaultAudioDevice();
            rec.RecognizeAsync(RecognizeMode.Multiple);

            CultureInfo myLang = new CultureInfo("en-US");

            s.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Senior, 60, myLang);//Choice the voice bot gender, age and lang


            InitializeComponent();

        }
        // func for saying
        public void say(String h)
        {
            notifyIcon1.Icon = new Icon("sleep.ico");
            wake = false;
            s.SpeakAsync(h);
            guna2TextBox2.Text = h;
            guna2TextBox3.Text = "State: Sleeping";
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
            guna2TextBox1.Text = speech; ;//add your text to input box
            

            if (speech == "lefty")
            {
                notifyIcon1.Icon = new Icon("active.ico");
                guna2TextBox3.Text = "State: Awake";
                wake = true;
                guna2TextBox2.Text = ("I am listening");
            }
            if (speech == "wake")
            {
                this.Show();
                
            }

            if (speech == "hide")
            {
                this.Hide();
                
                notifyIcon1.Visible = true;
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
                    say(data.get_weather());//func from database
                }

                if (speech == "help")
                {
                    MessageBox.Show(" Hello\n\n How are you\n\n What time is it\n\n What is today\n\n Open google\n\n Wake\n\n Sleep\n\n Weather\n\n What about weather\n\n Lefty\n\n Show commands\n");
                }
                if (speech == "joke")
                {
                    say(data.get_jokes());//func from database
                }
                if (speech == "toss coin")
                {
                    say(data.toss_a_coin());
                }

                if (speech == "exit")
                {
                    Application.Exit();
                }
            }
        }

        
           
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = true;
        }

        private void guna2ControlBox2_Click(object sender, EventArgs e)
        {
            Hide();
            notifyIcon1.Visible = true;
        }

    }
}

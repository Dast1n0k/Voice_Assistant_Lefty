using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
namespace Lefty
{
    public partial class Form1 : Form
    {
        //Add voice of assistant
        SpeechSynthesizer s = new SpeechSynthesizer();

        //Statement of voice assistant
        Boolean wake = false; 

        //Create list of commands
        Choices list = new Choices();

        //Create obj of class dataset
        DataSet data = new DataSet();

        //Сurrent time
        DateTime now = DateTime.Now;
        public Form1()

        {
            InitializeComponent();
            //Create obj of class for recognize your voice and words
            SpeechRecognitionEngine rec = new SpeechRecognitionEngine(new CultureInfo("en-US"));

            //List of commands 
            list.Add(File.ReadAllLines(@"Voice Bot Commands\commands.txt"));

            //Create a obj of class for grammar of voice bot
            GrammarBuilder gb = new GrammarBuilder();

            //Add Culture Info
            gb.Culture = new System.Globalization.CultureInfo("en-US");

            //Add to grammar our commands 
            gb.Append(list);
            Grammar g = new Grammar(gb);

            try
            {
                rec.RequestRecognizerUpdate();
                rec.LoadGrammarAsync(g);//Load this grammar to bot that recognizer understand what commands I need and what bot need to recognized
                rec.SpeechRecognized += rec_SpeachRecognized;
                rec.SetInputToDefaultAudioDevice();
                rec.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

            CultureInfo myLang = new CultureInfo("en-US");

            //Choice the voice bot gender, age and lang
            s.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Senior, 60, myLang);

            //Startposition of Form
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 320, Screen.PrimaryScreen.Bounds.Height - 458);

            //Welcome phrase
            string time = now.GetDateTimeFormats('t')[0];
            if (now.Hour >= 0 && now.Hour < 12)
            { s.SpeakAsync("Good morning, user"); }
            if (now.Hour >= 12 && now.Hour < 18)
            { s.SpeakAsync("Good afternoon, user"); }
            if (now.Hour >= 18 && now.Hour < 24)
            { s.SpeakAsync("Good evening, user"); }

            
        }

        //Assistant speak function
        public void say(String h)
        {
            notifyIcon1.Icon = new Icon("sleep.ico");
            wake = false;
            s.SpeakAsync(h);
            guna2TextBox2.Text = h;
            guna2TextBox3.Text = "State: Sleeping";
        }

        public void search_music(String s)
        {
            say("What music to you want?");
            Process.Start($"https://soundcloud.com/search?q=");
        }

        //Commands
        private void rec_SpeachRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //Convert your voice to text
            String speech = e.Result.Text;

            //Add your text to input box
            guna2TextBox4.Text = speech;

            //Lefty active commands in sleep mode 
            switch (speech)
            {
                case "Lefty":
                    notifyIcon1.Icon = new Icon("active.ico");
                    guna2TextBox3.Text = "State: Awake";
                    wake = true;
                    guna2TextBox2.Text = ("I am listening");
                    break;

                case "Wake":
                    this.Show();
                    break;

                case "Hide":
                    this.Hide();
                    notifyIcon1.Visible = true;
                    break;
            }

            //Lefty active mode  
            if (wake == true)
            {
                switch (speech)
                {
                    case "Hello":
                        say(data.greetings_action());//func from database
                        break;

                    case "Search music":
                        search_music(speech);
                        break;

                    case "Open word":
                        Process.Start("winword");
                        say("");
                        break;

                    case "Open excel":
                        Process.Start("excel");
                        say("");
                        break;

                    case "Open powerpoint":
                        Process.Start("powerpnt");
                        say("");
                        break;
                    
                    case "What time is it":
                        say(DateTime.Now.ToString("h.mm tt"));
                        break;

                    case "What is today":
                        say(DateTime.Now.ToString("M/d/yyyy"));
                        break;

                    case "How are you":
                        say("Great, and you?");
                        break;

                    case "Open google":
                        Process.Start("https://www.google.com/");
                        say("");
                        break;

                    case "Weather":
                        say(data.get_weather());//func from database
                        break;

                    case "Notepad":
                        Process.Start("notepad");
                        say("");
                        break;

                    case "Paint":
                        Process.Start("mspaint");
                        say("");
                        break;

                    case "Help":
                        MessageBox.Show(" Hello\n\n How are you\n\n What time is it\n\n What is today\n\n Open google\n\n Wake\n\n Sleep\n\n Weather\n\n What about weather\n\n Lefty\n\n Show commands\n");
                        say("");
                        break;

                    case "Joke":
                        say(data.get_jokes());//func from database
                        break;

                    case "Toss coin":
                        say(data.toss_a_coin());
                        break;

                    case "News":
                        say(data.get_news());
                        break;

                    case ("Open my computer"):
                        Process.Start("explorer.exe", "::{20d04fe0-3aea-1069-a2d8-08002b30309d}");
                        say("");
                        break;

                    case ("Stop"):
                        Process.Start("taskmgr.exe");
                        say("");
                        break;

                    case ("Open facebook"):
                        Process.Start("https://www.facebook.com/");
                        say("");
                        break;

                    case ("Open mail"):
                        Process.Start("https://www.gmail.com");
                        say("");
                        break;

                    case "Exit":
                        Application.Exit();
                        break;
                }
            }
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

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
using System.Media;
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
        
        Boolean search = false;

        //Create list of commands
        Choices list = new Choices();

        //Create obj of class dataset
        Parse data = new Parse();

        //Сurrent time
        DateTime now = DateTime.Now;

        //Add sound
        private SoundPlayer notification_sound;

        //List of commands 
        String[] grammarFile = (File.ReadAllLines(@"Voice Bot Commands\commands.txt"));

        //List of group names
        //String[] groupnamesFile = (File.ReadAllLines(@"Voice Bot Commands\name_groups.txt"));

        //List of groups url
        String[] groupsFile = (File.ReadAllLines(@"Voice Bot Commands\groups.txt"));

        public Form1()

        {
            //Create obj of class for recognize your voice and words
            SpeechRecognitionEngine rec = new SpeechRecognitionEngine(new CultureInfo("en-US"));

            //Create a obj of class for grammar of voice bot
            GrammarBuilder gb = new GrammarBuilder();

            list.Add(grammarFile);

            //Add Culture Info
            gb.Culture = new System.Globalization.CultureInfo("en-US");

            //Add to grammar our commands 
            gb.Append(list);
            Grammar g = new Grammar(gb);

            notification_sound = new SoundPlayer("listening_sound.wav");

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

            InitializeComponent();
        }

        //Assistant speak function
        public void Say(String h)
        {
            s.SpeakAsync(h);
            guna2TextBox2.Text = h;
        }

        //All what you need for sleep
        public void Sleeping()
        {
            notifyIcon1.Icon = new Icon("sleep.ico");
            wake = false;
            guna2TextBox3.Text = "State: Sleeping";
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
                    notification_sound.Play();
                    notifyIcon1.Icon = new Icon("active.ico");
                    guna2TextBox3.Text = "State: Awake";
                    wake = true;
                    guna2TextBox2.Text = ("I am listening");
                    break;

                case "Wake":
                    Show();
                    break;

                case "Hide":
                    Hide();
                    break;
            }

            //Search groups in kpi site
            if (search)
            {
                
                int resp = Array.IndexOf(grammarFile, speech);
                try
                {   
                    Process.Start($"https://schedule.kpi.ua/?groupId={groupsFile[resp]}");
                    search = false;
                    Sleeping();
                }
                catch (System.IndexOutOfRangeException) 
                {
                    Say("INVALID GROUP!");
                    search = false;
                    Sleeping();
                }
            }

            //Lefty active mode  
            if (wake == true && search == false)
            {
                try
                {
                    switch (speech)
                    {
                        case "Hello":
                            Say(data.greetings_action());//func from database
                            Sleeping();
                            break;

                        case "Open word":
                            Process.Start("winword");
                            Sleeping();
                            break;

                        case "Open excel":
                            Process.Start("excel");
                            Sleeping();
                            break;

                        case "Open powerpoint":
                            Process.Start("powerpnt");
                            Sleeping();
                            break;

                        case "What time is it":
                            Say(DateTime.Now.ToString("h.mm tt"));
                            Sleeping();
                            break;

                        case "What is today":
                            Say(DateTime.Now.ToString("M/d/yyyy"));
                            Sleeping();
                            break;

                        case "How are you":
                            Say("Great, and you?");
                            Sleeping();
                            break;

                        case "Open google":
                            Process.Start("https://www.google.com/");
                            Sleeping();
                            break;

                        case "Weather":
                            Say(data.get_weather());//func from database
                            Sleeping();
                            break;

                        case "Notepad":
                            Process.Start("notepad");
                            Sleeping();
                            break;

                        case "Paint":
                            Process.Start("mspaint");
                            Sleeping();
                            break;

                        case "Help":
                            MessageBox.Show(" Hello\n\n How are you\n\n What time is it\n\n What is today\n\n Open google\n\n Wake\n\n Sleep\n\n Weather\n\n What about weather\n\n Lefty\n\n Show commands\n");
                            Sleeping();
                            break;

                        case "Joke":
                            Say(data.get_jokes());//func from database
                            Sleeping();
                            break;

                        case "Toss coin":
                            Say(data.toss_a_coin());
                            Sleeping();
                            break;

                        case "News":
                            Say(data.get_news());
                            Sleeping();
                            break;

                        case "Currency rate":
                            Say(data.get_course());
                            Sleeping();
                            break;

                        case ("Open my computer"):
                            Process.Start("explorer.exe", "::{20d04fe0-3aea-1069-a2d8-08002b30309d}");
                            Sleeping();
                            break;

                        case ("Task manager"):
                            Process.Start("taskmgr.exe");
                            Sleeping();
                            break;

                        case ("Open facebook"):
                            Process.Start("https://www.facebook.com/");
                            Sleeping();
                            break;

                        case ("Open mail"):
                            Process.Start("https://www.gmail.com");
                            Sleeping();
                            break;

                        case ("Moodle"):
                            Process.Start("https://do.ipo.kpi.ua");
                            Sleeping(); 
                            break;

                        case ("Campus"):
                            Process.Start("https://ecampus.kpi.ua");
                            Sleeping();
                            break;

                        case ("Classroom"):
                            Process.Start("https://classroom.google.com");
                            Sleeping(); 
                            break;

                        case ("Coursera"):
                            Process.Start("https://www.coursera.org");
                            Sleeping();
                            break;

                        case ("Timetable"):
                            search = true;
                            Say("Say group name");
                            break;

                        case "Exit":
                            Application.Exit();
                            break;
                    }
                }                
                catch (Win32Exception w)
                {
                    MessageBox.Show(w.Message);
                    Sleeping();
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

        private void Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

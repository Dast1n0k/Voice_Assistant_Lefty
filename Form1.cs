using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;



namespace Lefty
{
    public partial class Form1 : Form
    {
        SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
        SpeechSynthesizer Lefty = new SpeechSynthesizer();
        SpeechRecognitionEngine startlistening = new SpeechRecognitionEngine();
        Random rnd = new Random();
        int RecTimeOut = 0;
        DateTime TimeNow = DateTime.Now;



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommansds.txt")))));
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Default_SpeechRecognized);
            _recognizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(_recognizer_SpeechRecognized);
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);

            startlistening.SetInputToDefaultAudioDevice();
            startlistening.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommansds.txt")))));
            startlistening.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(startlistening_SpeechRecognized);

            Lefty.SelectVoiceByHints(VoiceGender.Female);

        }

        public void Default_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            int ranNum;
            string speech = e.Result.Text;

            if (speech == "Hello")
            {
                Lefty.SpeakAsync("Hello, I am here");
            }

            if (speech == "How are you")
            {
                Lefty.SpeakAsync("I am working at home, what about you");
            }

            if (speech == "What time is it")
            {
                Lefty.SpeakAsync(DateTime.Now.ToString("h mm tt"));
            }

            if (speech == "Stop talking")
            {
                Lefty.SpeakAsyncCancelAll();
                ranNum = rnd.Next(1);

                if (ranNum == 1)
                {
                    Lefty.SpeakAsync("Yes sir");
                }

                if (ranNum == 2)
                {
                    Lefty.SpeakAsync("I am sorry i will be quiet");
                }
            }

            if (speech == "Stop Listening")
            {
                Lefty.SpeakAsync("if you need me just ask");
                _recognizer.RecognizeAsyncCancel();
                startlistening.RecognizeAsync(RecognizeMode.Multiple);
            }
            if (speech == "Show Commands")
            {
                string[] commands = (File.ReadAllLines(@"DefaultCommansds.txt"));
                LstCommands.Items.Clear();
                LstCommands.SelectionMode = SelectionMode.None;
                LstCommands.Visible = true;
                foreach (string command in commands)
                {
                    LstCommands.Items.Add(command);
                }

            }
            if (speech == "Hide Commands")
            {
                LstCommands.Visible = false;
            }

        }

        private void _recognizer_SpeechRecognized(object sender, SpeechDetectedEventArgs e)
        {
            RecTimeOut = 0;

        }

        private void startlistening_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;

            if (speech == "Wake up")
            {
                startlistening.RecognizeAsyncCancel();
                Lefty.SpeakAsync("Yes, I am here");
                _recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        private void TmrSpeaking_Tick(object sender, EventArgs e)
        {
            if (RecTimeOut == 10)
            {
                _recognizer.RecognizeAsyncCancel();
            }
            else if (RecTimeOut == 11)
            {
                TmrSpeaking.Stop();
                startlistening.RecognizeAsync(RecognizeMode.Multiple);
                RecTimeOut = 0;
            }
        }
    }
}

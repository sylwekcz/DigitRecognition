using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using Patagames.Ocr;
using Patagames.Ocr.Enums;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Timers;

namespace DigitRecognition
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;
        private HSLFilteringForm colorForm;
        private bool captureInitialized = false;
        private bool capture = false;
        private bool active = false;
        private Bitmap originalVideo, filteredVideo, binarizedVideo, outputImage = new Bitmap(320, 240);
        private IntPoint newPoint = new IntPoint(0, 0), oldPoint = new IntPoint(0, 0);
        private Blob[] blobs;
        private Filters Filters = new Filters();

        //speech
        private SpeechSynthesizer reader;
        
        private int time = 0; //miliseconds
        private int userTime = 4000; //1s
        System.Timers.Timer timer;
        private int[] DigitTable = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        ResizeBicubic resizeFilter = new ResizeBicubic(320, 240);




        public Form1()
        {
            InitializeComponent();
            {
                InitVideo();
                colorForm = new HSLFilteringForm();
                reader = new SpeechSynthesizer();

                timer = new System.Timers.Timer();
                timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                timer.Interval = 100;
                timer.Enabled = true;
            }

        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            time+=100;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FinalVideo != null)
            {
                FinalVideo.Stop();
            }
        }

        private void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            originalVideo = resizeFilter.Apply((Bitmap)eventArgs.Frame.Clone());
            if (checkBoxMirror.Checked)
            {
                Filters.ApplyMirrorFilter(originalVideo);
            }
            filteredVideo = Filters.ApplyColorFilter((Bitmap)originalVideo.Clone(), colorForm.Filter);
            binarizedVideo = Filters.ApplyBinaryFilter((Bitmap)filteredVideo.Clone());
            blobs = Filters.LocalizeBlobs(binarizedVideo);
            drawOutput();

            pictureBox1.Image = originalVideo;
            pictureBox2.Image = binarizedVideo;

            try
            {
                pictureBox4.Image = outputImage;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void InitVideo()
        {
            VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo VideoCaptureDevice in VideoCaptureDevices)
            {
                comboBox1.Items.Add(VideoCaptureDevice.Name);
            }

            if (VideoCaptureDevices.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }

        }

        private void InitVideoCapture(int videoCameraResolutionMode)
        {
            InitVideo();
            FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[comboBox1.SelectedIndex].MonikerString);
            FinalVideo.VideoResolution = FinalVideo.VideoCapabilities[videoCameraResolutionMode];  // resolution
            FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
            captureInitialized = true;

            FinalVideo.Start();
        }

        private void buttonColorPick_Click(object sender, EventArgs e)
        {
            colorForm.Show();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            userTime = (int)((float)numericUpDown1.Value * 1000);
        }

        private void btnPlayOrPause_Click(object sender, EventArgs e)
        {

            if (capture)
            {
                FinalVideo.Stop();
                capture = false;
                btnPlayOrPause.Text = "Start";
            }
            else
            {
                if (VideoCaptureDevices.Count > 0)
                {
                    if (!captureInitialized)
                    {
                        InitVideoCapture(2);
                        capture = true;
                        btnPlayOrPause.Text = "Pause";
                    }
                    else
                    {
                        FinalVideo.Start();
                        capture = true;
                        btnPlayOrPause.Text = "Pause";
                    }
                }
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Text = "";
            InitVideo();
        }



        private void drawOutput()
        {

            if (blobs != null && blobs.Length > 0)
            {
                newPoint.X = (int)blobs[0].CenterOfGravity.X; newPoint.Y = (int)blobs[0].CenterOfGravity.Y;
                if ((newPoint.X - oldPoint.X) < 8 && (newPoint.X - oldPoint.X) > -8 && (newPoint.Y - oldPoint.Y) < 8 && (newPoint.Y - oldPoint.Y) > -8)
                {
                    
                    if (time >= userTime && active == false) 
                    {
                        active = true;
                        outputImage = new Bitmap(320, 240);
                        reader.SpeakAsync("Zaczynasz rysowanie");

                        time = 0;
                    }
                    if (time >= userTime && active == true  )  
                    {
                        active = false;
                        reader.SpeakAsync("Rysowanie zakończone!");
                        ExtractTextFromBitmap();
                        time = 0;
                    }

                }
                else time = 0;
                if (active)
                {

                    Pen blackPen = new Pen(Color.Black, 7);
                    using (var graphics = Graphics.FromImage(outputImage))
                    {
                        graphics.DrawLine(blackPen, newPoint.X, newPoint.Y, oldPoint.X, oldPoint.Y);
                    }

                }
                oldPoint.X = newPoint.X; oldPoint.Y = newPoint.Y;
            } else if (active)
            {
                
                    active = false;
                    reader.SpeakAsync("Rysowanie zakończone!");
                    ExtractTextFromBitmap();
                    time = 0;
                
            }

        }

        public void ExtractTextFromBitmap()
        {
            string plainText;
            bool tell = false;
            int b;
            using (var api = OcrApi.Create())
            {
                api.Init(Languages.English);
                api.SetVariable("tessedit_char_whitelist", "0123456789");
                plainText = api.GetTextFromImage(outputImage);


            }

            foreach (int a in DigitTable)
            {
                if (int.TryParse(plainText, out b))
                    if (a == b)
                        tell = true;


            }

            if (tell)
            {
                string toSpeech = "Rozpoznana cyfra to " + plainText;
                reader.SpeakAsync(toSpeech);
                tell = false;
            }
            else
            {

                reader.SpeakAsync("Nie rozpoznano. Spróbuj jeszcze raz");

            }
        }
    }
}

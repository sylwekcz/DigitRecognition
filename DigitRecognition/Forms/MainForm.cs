
using AForge;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using Patagames.Ocr;
using Patagames.Ocr.Enums;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DigitRecognition
{
    public partial class Form1 : Form
    {
        private VideoCaptureDevice videoInput;
        private FilterInfoCollection videoCaptureDevices;        
        private Filters filters = new Filters();
        private HSLFilteringForm colorForm;
        private bool videoCaptureInitialized = false, videoCaptureInProgress = false, drawOn = false;

        private Bitmap originalVideo, filteredVideo, binarizedVideo, outputImage = new Bitmap(320, 240);
        private IntPoint newPoint = new IntPoint(0, 0), oldPoint = new IntPoint(0, 0);
        private Blob[] blobs;
        
        
        public Form1()
        {
            InitializeComponent();
            {
                GetVideoDevices();
                colorForm = new HSLFilteringForm();
            }          
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoInput != null)
            {
                videoInput.Stop();
            }
        }

        private void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {       
            originalVideo = (Bitmap)eventArgs.Frame.Clone();
            if (checkBoxMirror.Checked)   filters.ApplyMirrorFilter(originalVideo);       
            
            filteredVideo = filters.ApplyColorFilter((Bitmap)originalVideo.Clone(), colorForm.Filter);            
            binarizedVideo = filters.ApplyBinaryFilter((Bitmap)filteredVideo.Clone());         
            blobs = filters.LocalizeBlobs(binarizedVideo);
            DrawOutput(drawOn);

            pictureBox1.Image = originalVideo;
            pictureBox2.Image = filteredVideo;
            pictureBox3.Image = binarizedVideo;            
            pictureBox4.Image = outputImage;
            
        }      

        private void buttonColorPick_Click(object sender, EventArgs e)
        {
            colorForm.Show();
        }

        private void btnPlayOrPause_Click(object sender, EventArgs e)
        {
            if (videoCaptureInProgress)
            {
                videoInput.Stop();
                videoCaptureInProgress = false;
                btnPlayOrPause.Text = "Start";
            }
            else
            {
                if (videoCaptureDevices.Count > 0)
                {
                    if (!videoCaptureInitialized)
                    {
                        InitVideoCapture(4);
                        videoCaptureInProgress = true;
                        btnPlayOrPause.Text = "Pause";
                    }
                    else
                    {
                        videoInput.Start();
                        videoCaptureInProgress = true;
                        btnPlayOrPause.Text = "Pause";
                    }
                }
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Text = "";
            GetVideoDevices();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.X)
            {
                drawOn = false;
                ExtractTextFromBitmap();
            }
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.X) drawOn = true;
            else if (e.KeyCode == Keys.Z) outputImage = new Bitmap(320, 240);
        }

        private void GetVideoDevices()
        {
            videoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo VideoCaptureDevice in videoCaptureDevices)
            {
                comboBox1.Items.Add(VideoCaptureDevice.Name);
            }

            if (videoCaptureDevices.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }

        }

        private void InitVideoCapture(int videoCameraResolutionMode)
        {
            videoInput = new VideoCaptureDevice(videoCaptureDevices[comboBox1.SelectedIndex].MonikerString);
            videoInput.VideoResolution = videoInput.VideoCapabilities[videoCameraResolutionMode];
            videoInput.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
            videoInput.Start();
            videoCaptureInitialized = true;
        }

        private void DrawOutput(bool active)
        {
            if (blobs != null && blobs.Length > 0)
            {
                newPoint.X = (int)blobs[0].CenterOfGravity.X; newPoint.Y = (int)blobs[0].CenterOfGravity.Y;
                if (active)
                {                    
                    Pen blackPen = new Pen(Color.Black, 7);
                    using (var graphics = Graphics.FromImage(outputImage))
                    {
                        graphics.DrawLine(blackPen, newPoint.X, newPoint.Y, oldPoint.X, oldPoint.Y);
                    }
                }
                oldPoint.X = newPoint.X; oldPoint.Y = newPoint.Y;
            }
        }        

        public void ExtractTextFromBitmap()
        {
            using (var api = OcrApi.Create())
            {
                api.Init(Languages.English);
                api.SetVariable("tessedit_char_whitelist", "0123456789");
                string plainText = api.GetTextFromImage(outputImage);
                labelRecognized.Text = plainText;
            }
        }  
    }
}

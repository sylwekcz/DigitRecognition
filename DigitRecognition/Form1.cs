
using AForge;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DigitRecognition
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;
        HSLFilteringForm colorForm;
        private bool CaptureNotInitialized = true;
        private bool CaptureOn = false;
        Bitmap originalVideo, filteredVideo, binarizedVideo;

        //filters
        HSLFiltering hlsColorFilter = new HSLFiltering();
        Grayscale grayFilter = new Grayscale(0.2125, 0.7154, 0.0721);
        Median medianFilter = new Median();
        Threshold binarizeFilter = new Threshold(25);
        Mirror mirrorFilter = new Mirror(false, true);

        public Form1()
        {
            InitializeComponent();
            {
                InitVideo();
                colorForm = new HSLFilteringForm();
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FinalVideo != null)
            {
                FinalVideo.Stop();
            }
        }

        private void buttonColorPick_Click(object sender, EventArgs e)
        {            
            colorForm.Show();            
        }
       

        private void btnPlayOrPause_Click(object sender, EventArgs e)
        {                    

            if (CaptureOn)
            {          
                FinalVideo.Stop();
                CaptureOn = false;
                btnPlayOrPause.Text = "Start";
            }
            else
            {
                if (VideoCaptureDevices.Count>0)
                {
                    if (CaptureNotInitialized)
                    {
                        FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[comboBox1.SelectedIndex].MonikerString);
                        FinalVideo.VideoResolution = FinalVideo.VideoCapabilities[4];  // resolution
                        FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
                        CaptureNotInitialized = false;
                        FinalVideo.Start();
                        CaptureOn = true;
                        btnPlayOrPause.Text = "Pause";
                    }
                    else
                    {
                        FinalVideo.Start();
                        CaptureOn = true;
                        btnPlayOrPause.Text = "Pause";
                    }
                }                
             }
          
        }

        void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            originalVideo = (Bitmap)eventArgs.Frame.Clone();
            if (checkBoxMirror.Checked)
            {   // mirror
                mirrorFilter.ApplyInPlace(originalVideo);
            }
            pictureBox1.Image = originalVideo;
            // color filter
            filteredVideo = DetectedColor((Bitmap)originalVideo.Clone());
            pictureBox2.Image = filteredVideo;              
            //binarize
            binarizedVideo = DetectedToBinary((Bitmap)filteredVideo.Clone());
            pictureBox3.Image = binarizedVideo;
        }

        void InitVideo()
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

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Text = "";
            InitVideo();

        }

        Bitmap DetectedColor(Bitmap video)
        {            
           hlsColorFilter = colorForm.Filter; 
           //color filter
           hlsColorFilter.ApplyInPlace(video);    
                    
           return video;
        }
        Bitmap DetectedToBinary(Bitmap video)
        {
            // color filter to rgb    
            video = grayFilter.Apply(video);
            // binarize
            binarizeFilter.ApplyInPlace(video);
            // median filter noise reduction
            medianFilter.ApplyInPlace(video);            

            return video;
        }

    }

   
}

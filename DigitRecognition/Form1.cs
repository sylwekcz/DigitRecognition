
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitRecognition
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;
        Bitmap originalVideo, filteredVideo;
        ColorFiltering colorFilter = new ColorFiltering();
        private bool CaptureNotInitialized = true;
        private bool CaptureOn = false;
        IntRange red, blue, green;


        public Form1()
        {
            InitializeComponent();
            {
                VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                foreach (FilterInfo VideoCaptureDevice in VideoCaptureDevices)
                {
                    comboBox1.Items.Add(VideoCaptureDevice.Name);
                }
                comboBox1.SelectedIndex = 0;

                red = new IntRange(0, 50);
                green = new IntRange(100, 255);
                blue = new IntRange(0, 50);


                colorFilter.Red = red;
                colorFilter.Green = green;
                colorFilter.Blue = blue;

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

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

                if (CaptureNotInitialized)
                {

                    FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[comboBox1.SelectedIndex].MonikerString);
                    FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
                    CaptureNotInitialized = false;
                }

                FinalVideo.Start();
                CaptureOn = true;
                btnPlayOrPause.Text = "Pause";

            }
          
        }

        void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            originalVideo = (Bitmap)eventArgs.Frame.Clone();

            filteredVideo = (Bitmap)originalVideo.Clone();
            pictureBox1.Image = originalVideo;


            // apply the filter
            colorFilter.ApplyInPlace(filteredVideo);
            pictureBox2.Image = filteredVideo;

        }

    }

   
}

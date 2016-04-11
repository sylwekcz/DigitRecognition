
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DigitRecognition
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;
        private HSLFilteringForm colorForm;
        private bool captureNotInitialized = true;
        private bool captureOn = false;
        private bool drawOn = false;
        private bool resetOutput = false;
        private Bitmap originalVideo, filteredVideo, binarizedVideo, outputImage = new Bitmap(320, 240);
        private String coordinates;
        private IntPoint newPoint = new IntPoint(0, 0), oldPoint = new IntPoint(0, 0);

        //filters
        private HSLFiltering hlsColorFilter = new HSLFiltering();
        private Grayscale grayFilter = new Grayscale(0.2125, 0.7154, 0.0721);
        private Median medianFilter = new Median();
        private Threshold binarizeFilter = new Threshold(25);
        private Mirror mirrorFilter = new Mirror(false, true);
        private ExtractBiggestBlob biggestBlobFilter = new ExtractBiggestBlob();
        private BlobCounterBase bc = new BlobCounter();
        private Blob[] blobs;

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

        private void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            originalVideo = (Bitmap)eventArgs.Frame.Clone();
            if (checkBoxMirror.Checked)
            {   // mirror
                mirrorFilter.ApplyInPlace(originalVideo);
            }
            // color filter
            filteredVideo = DetectedColor((Bitmap)originalVideo.Clone());
            // binarize
            binarizedVideo = DetectedToBinary((Bitmap)filteredVideo.Clone());
            // biggest
            blobs = LocalizeBlobs(binarizedVideo);

            drawOutput(drawOn);

            pictureBox1.Image = originalVideo;
            pictureBox2.Image = filteredVideo;
            pictureBox3.Image = binarizedVideo;
            pictureBox4.Image = outputImage;
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
            FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[comboBox1.SelectedIndex].MonikerString);
            FinalVideo.VideoResolution = FinalVideo.VideoCapabilities[videoCameraResolutionMode];  // resolution
            FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
            captureNotInitialized = false;
            FinalVideo.Start();
        }

        private void buttonColorPick_Click(object sender, EventArgs e)
        {
            colorForm.Show();
        }

        private void btnPlayOrPause_Click(object sender, EventArgs e)
        {

            if (captureOn)
            {
                FinalVideo.Stop();
                captureOn = false;
                btnPlayOrPause.Text = "Start";
            }
            else
            {
                if (VideoCaptureDevices.Count > 0)
                {
                    if (captureNotInitialized)
                    {
                        InitVideoCapture(4);
                        captureOn = true;
                        btnPlayOrPause.Text = "Pause";
                    }
                    else
                    {
                        FinalVideo.Start();
                        captureOn = true;
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

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.X) drawOn = false;


        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.X) drawOn = true;
            else if (e.KeyCode == Keys.Z) outputImage = new Bitmap(320, 240);
        }

        private Bitmap DetectedColor(Bitmap video)
        {
            hlsColorFilter = colorForm.Filter;
            //color filter
            hlsColorFilter.ApplyInPlace(video);

            return video;
        }

        private Bitmap DetectedToBinary(Bitmap video)
        {
            // color filter to rgb    
            video = grayFilter.Apply(video);
            // binarize
            binarizeFilter.ApplyInPlace(video);
            // median filter noise reduction
            medianFilter.ApplyInPlace(video);

            return video;
        }

        private Blob[] LocalizeBlobs(Bitmap binarizedInput)
        {
            bc.FilterBlobs = true;
            bc.MinWidth = 5;
            bc.MinHeight = 5;
            bc.ObjectsOrder = ObjectsOrder.Size;
            bc.ProcessImage(binarizedInput);
            return bc.GetObjectsInformation();

        }

        private void drawOutput(bool active)
        {
            if (blobs != null && blobs.Length > 0)
            {
                newPoint.X = (int)blobs[0].CenterOfGravity.X; newPoint.Y = (int)blobs[0].CenterOfGravity.Y;
                if (active)
                {
                    BitmapData data = outputImage.LockBits(new Rectangle(0, 0, outputImage.Width, outputImage.Height), ImageLockMode.ReadWrite, outputImage.PixelFormat);
                    Drawing.Line(data, oldPoint, newPoint, Color.Black);
                    oldPoint.X = newPoint.X; oldPoint.Y = newPoint.Y;
                    outputImage.UnlockBits(data);
                }
                oldPoint.X = newPoint.X; oldPoint.Y = newPoint.Y;
            }

        }
    }


}

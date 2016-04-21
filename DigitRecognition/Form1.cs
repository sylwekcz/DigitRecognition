
using Accord.MachineLearning;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using Patagames.Ocr;
using Patagames.Ocr.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DigitRecognition
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection videoCaptureDevices;
        private VideoCaptureDevice finalVideo;
        private HSLFilteringForm colorForm;
        private bool captureNotInitialized = true;
        private bool captureOn = false;
        private bool drawOn = false;
        private Bitmap originalVideo, filteredVideo, binarizedVideo, outputImage = new Bitmap(320, 240), knnImage = new Bitmap(320, 240);
        private IntPoint newPoint = new IntPoint(0, 0), oldPoint = new IntPoint(0, 0);

        //filters
        private HSLFiltering hlsColorFilter = new HSLFiltering();
        private Grayscale grayFilter = new Grayscale(0.2125, 0.7154, 0.0721);
        private Median medianFilter = new Median();
        private Threshold binarizeFilter = new Threshold(25);
        private Mirror mirrorFilter = new Mirror(false, true);
        Dilatation dylatationFilter = new Dilatation();
        private ExtractBiggestBlob biggestBlobFilter = new ExtractBiggestBlob();
        private BlobCounterBase bc = new BlobCounter();
        private Blob[] blobs;

        private List<double[]> patterns = new List<double[]>();
        private List<int> patternsClasses = new List<int>();
        private int classesCount=0;
        
       

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
            if (finalVideo != null)
            {
                finalVideo.Stop();
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
            // dylatationFilter.ApplyInPlace(outputImage);

            pictureBox1.Image = originalVideo;
            pictureBox2.Image = filteredVideo;
            pictureBox3.Image = binarizedVideo;

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
            finalVideo = new VideoCaptureDevice(videoCaptureDevices[comboBox1.SelectedIndex].MonikerString);
            finalVideo.VideoResolution = finalVideo.VideoCapabilities[videoCameraResolutionMode];  // resolution
            finalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
            captureNotInitialized = false;
            finalVideo.Start();
        }

        private void buttonColorPick_Click(object sender, EventArgs e)
        {
            colorForm.Show();
        }

        private void btnPlayOrPause_Click(object sender, EventArgs e)
        {

            if (captureOn)
            {
                finalVideo.Stop();
                captureOn = false;
                btnPlayOrPause.Text = "Start";
            }
            else
            {
                if (videoCaptureDevices.Count > 0)
                {
                    if (captureNotInitialized)
                    {
                        InitVideoCapture(4);
                        captureOn = true;
                        btnPlayOrPause.Text = "Pause";
                    }
                    else
                    {
                        finalVideo.Start();
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
            if (e.KeyCode == Keys.X)
            {
                drawOn = false;
                ExtractTextFromBitmap();
                ExtractTextFromBitmapCustom();

            }
            
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

        private void btnPattern_Click(object sender, EventArgs e)
        {
            patterns.Add(ExtractData(knnImage));
            patternsClasses.Add(int.Parse(txtPattern.Text));
            WriteToXmlFile("patterns.xml", patterns);
            WriteToXmlFile("patternsClasses.xml", patternsClasses);
            classesCount = CountClasses(patternsClasses);
            Console.WriteLine(classesCount);


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
            bc.MinWidth = 15;
            bc.MinHeight = 15;
            bc.ObjectsOrder = ObjectsOrder.Size;
            bc.ProcessImage(binarizedInput);
            return bc.GetObjectsInformation();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                patterns = ReadFromXmlFile<List<double[]>>("patterns.xml");
                patternsClasses = ReadFromXmlFile<List<int>>("patternsClasses");

            }
            catch
            {
                MessageBox.Show("File not found, or empty!");
            }

            classesCount = CountClasses(patternsClasses);
            Console.WriteLine(classesCount);

        }

        private void drawOutput(bool active)
        {

            if (blobs != null && blobs.Length > 0)
            {
                newPoint.X = (int)blobs[0].CenterOfGravity.X; newPoint.Y = (int)blobs[0].CenterOfGravity.Y;
                if (active)
                {
                    // BitmapData data = outputImage.LockBits(new Rectangle(0, 0, outputImage.Width, outputImage.Height), ImageLockMode.ReadWrite, outputImage.PixelFormat);
                    // Drawing.Line(data, oldPoint, newPoint, Color.Black);
                    // oldPoint.X = newPoint.X; oldPoint.Y = newPoint.Y;
                    // outputImage.UnlockBits(data);


                    Pen blackPen = new Pen(Color.Black, 7);
                    using (var graphics = Graphics.FromImage(outputImage))
                    {
                        graphics.DrawLine(blackPen, newPoint.X, newPoint.Y, oldPoint.X, oldPoint.Y);
                    }

                    knnImage = (Bitmap)outputImage.Clone();
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

        public void ExtractTextFromBitmapCustom()
        {
            if (patterns.Count > 0)
            {

                KNearestNeighbors knn = new KNearestNeighbors(k: 4, classes: classesCount, inputs: patterns.ToArray(), outputs: patternsClasses.ToArray());

                int answer = knn.Compute(ExtractData(knnImage)); // 2
                lblWeRecognized.Text = ""+answer;
            }
        }

        public double[] ExtractData(Bitmap bmp)
        {
            double[] data = new double[bmp.Width * bmp.Height];

            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    // wyszlo z debugowania ze jak nie ma to Name = "0" a jak jest to "ff00000" 
                    data[i * bmp.Width + j] = (bmp.GetPixel(j, i).Name == "ff000000") ? 1 : 0;

                }

            }
            return data;
        }       

        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        public static int CountClasses(List<int> classPattern)
        {
            List<int> temp = new List<int>(classPattern);
            temp.Sort();

            int tmpClassesCount = 0;
            int x = -1;
            foreach (int a in temp)
            {
                if (a != x)
                {
                    tmpClassesCount++;
                    x = a;
                }

            }

            return tmpClassesCount;
        }

    }
}

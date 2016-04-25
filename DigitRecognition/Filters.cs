using AForge.Imaging;
using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitRecognition
{
    class Filters
    {
        private Grayscale grayFilter = new Grayscale(0.2125, 0.7154, 0.0721);
        private Median medianFilter = new Median();
        private Threshold binarizeFilter = new Threshold(25);
        private Mirror mirrorFilter = new Mirror(false, true);
        private Dilatation dylatationFilter = new Dilatation();
        private ExtractBiggestBlob biggestBlobFilter = new ExtractBiggestBlob();
        private HSLFiltering hlsColorFilter;
        private BlobCounterBase bc = new BlobCounter();
       
        

        public Bitmap ApplyMirrorFilter(Bitmap videoInput)
        {
            mirrorFilter.ApplyInPlace(videoInput);
            return videoInput;
        }

        public Bitmap ApplyColorFilter(Bitmap videoInput, HSLFiltering filter )
        {
            hlsColorFilter = filter;
            hlsColorFilter.ApplyInPlace(videoInput);
            return videoInput;
        }

        public Bitmap ApplyBinaryFilter(Bitmap colorFilteredInput)
        {
            colorFilteredInput = grayFilter.Apply(colorFilteredInput);
            binarizeFilter.ApplyInPlace(colorFilteredInput);
            medianFilter.ApplyInPlace(colorFilteredInput);
            return colorFilteredInput;
        }

        public Blob[] LocalizeBlobs(Bitmap binarizedInput)
        {
            bc.FilterBlobs = true;
            bc.MinWidth = 15;
            bc.MinHeight = 15;
            bc.ObjectsOrder = ObjectsOrder.Size;
            bc.ProcessImage(binarizedInput);
            return bc.GetObjectsInformation();
        }

    }
}

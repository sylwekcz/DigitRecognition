
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using AForge;
using AForge.Math;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace DigitRecognition { 
    public partial class HSLFilteringForm : System.Windows.Forms.Form
    {
        private HSLFiltering filter = new HSLFiltering( );
        private IntRange hue = new IntRange( 70, 140 );
        private Range saturation = new Range( 0.5f, 1 );
        private Range luminance = new Range( 0.2f, 0.4f );
        private int fillH = 0;
        private float fillS = 0, fillL = 0;

        
        

        
        public HSLFiltering Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                hue = filter.Hue;
                saturation = filter.Saturation;
                luminance = filter.Luminance;

                minHBox.Text = hue.Min.ToString();
                maxHBox.Text = hue.Max.ToString();

                minSBox.Text = saturation.Min.ToString("F3");
                maxSBox.Text = saturation.Max.ToString("F3");

                minLBox.Text = luminance.Min.ToString("F3");
                maxLBox.Text = luminance.Max.ToString("F3");
                
            }
        }
        
        public HSLFilteringForm( )
        {
            
            InitializeComponent( );
            minHBox.Text = hue.Min.ToString( );
            maxHBox.Text = hue.Max.ToString( );

            minSBox.Text = saturation.Min.ToString( "F3" );
            maxSBox.Text = saturation.Max.ToString( "F3" );

            minLBox.Text = luminance.Min.ToString( "F3" );
            maxLBox.Text = luminance.Max.ToString( "F3" );

            
        }
        protected override void Dispose( bool disposing )
        {
            if ( disposing )
            {
                if ( components != null )
                {
                    components.Dispose( );
                }
            }
            base.Dispose( disposing );
        }

       

        private void UpdateFilter( )
        {
            filter.Hue = hue;
            filter.Saturation = saturation;
            filter.Luminance = luminance;
        }

        private void minHBox_TextChanged( object sender, System.EventArgs e )
        {
            try
            {
                huePicker.Min = hue.Min = Math.Max( 0, Math.Min( 359, int.Parse( minHBox.Text ) ) );
                UpdateFilter( );
            }
            catch ( Exception )
            {
            }
        }

        private void maxHBox_TextChanged( object sender, System.EventArgs e )
        {
            try
            {
                huePicker.Max = hue.Max = Math.Max( 0, Math.Min( 359, int.Parse( maxHBox.Text ) ) );
                UpdateFilter( );
            }
            catch ( Exception )
            {
            }
        }

        private void minSBox_TextChanged( object sender, System.EventArgs e )
        {
            try
            {
                saturation.Min = float.Parse( minSBox.Text );
                saturationSlider.Min = (int) ( saturation.Min * 255 );
                UpdateFilter( );
            }
            catch ( Exception )
            {
            }
        }

        private void maxSBox_TextChanged( object sender, System.EventArgs e )
        {
            try
            {
                saturation.Max = float.Parse( maxSBox.Text );
                saturationSlider.Max = (int) ( saturation.Max * 255 );
                UpdateFilter( );
            }
            catch ( Exception )
            {
            }
        }

        private void minLBox_TextChanged( object sender, System.EventArgs e )
        {
            try
            {
                luminance.Min = float.Parse( minLBox.Text );
                luminanceSlider.Min = (int) ( luminance.Min * 255 );
                UpdateFilter( );
            }
            catch ( Exception )
            {
            }
        }

        private void maxLBox_TextChanged( object sender, System.EventArgs e )
        {
            try
            {
                luminance.Max = float.Parse( maxLBox.Text );
                luminanceSlider.Max = (int) ( luminance.Max * 255 );
                UpdateFilter( );
            }
            catch ( Exception )
            {
            }
        }

        private void huePicker_ValuesChanged( object sender, System.EventArgs e )
        {
            minHBox.Text = huePicker.Min.ToString( );
            maxHBox.Text = huePicker.Max.ToString( );
        }

        private void saturationSlider_ValuesChanged( object sender, System.EventArgs e )
        {
            minSBox.Text = ( (double) saturationSlider.Min / 255 ).ToString( "F3" );
            maxSBox.Text = ( (double) saturationSlider.Max / 255 ).ToString( "F3" );
        }

        private void luminanceSlider_ValuesChanged( object sender, System.EventArgs e )
        {
            minLBox.Text = ( (double) luminanceSlider.Min / 255 ).ToString( "F3" );
            maxLBox.Text = ( (double) luminanceSlider.Max / 255 ).ToString( "F3" );
        }
                     

        private void UpdateFillColor( )
        {
            int v;

            v = (int) ( fillS * 255 );
            saturationSlider.FillColor = Color.FromArgb( v, v, v );
            v = (int) ( fillL * 255 );
            luminanceSlider.FillColor = Color.FromArgb( v, v, v );


            filter.FillColor = new HSL( fillH, fillS, fillL );
        }

        private void HSLFilteringForm_Load(object sender, EventArgs e)
        {

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}

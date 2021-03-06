﻿

namespace DigitRecognition
{
    public partial class HSLFilteringForm 
    {

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.huePicker = new AForge.Controls.HuePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.maxHBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.minHBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.saturationSlider = new AForge.Controls.ColorSlider();
            this.maxSBox = new System.Windows.Forms.TextBox();
            this.minSBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.luminanceSlider = new AForge.Controls.ColorSlider();
            this.maxLBox = new System.Windows.Forms.TextBox();
            this.minLBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();

            this.huePicker.Location = new System.Drawing.Point(53, 50);
            this.huePicker.Name = "huePicker";
            this.huePicker.Size = new System.Drawing.Size(170, 170);
            this.huePicker.TabIndex = 0;
            this.huePicker.Type = AForge.Controls.HuePicker.HuePickerType.Range;
            this.huePicker.ValuesChanged += new System.EventHandler(this.huePicker_ValuesChanged);

            this.groupBox1.Controls.Add(this.maxHBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.minHBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.huePicker);
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 230);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hue";

            this.maxHBox.Location = new System.Drawing.Point(218, 20);
            this.maxHBox.Name = "maxHBox";
            this.maxHBox.Size = new System.Drawing.Size(50, 20);
            this.maxHBox.TabIndex = 4;
            this.maxHBox.TextChanged += new System.EventHandler(this.maxHBox_TextChanged);

            this.label2.Location = new System.Drawing.Point(186, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Max:";

            this.minHBox.Location = new System.Drawing.Point(40, 20);
            this.minHBox.Name = "minHBox";
            this.minHBox.Size = new System.Drawing.Size(50, 20);
            this.minHBox.TabIndex = 2;
            this.minHBox.TextChanged += new System.EventHandler(this.minHBox_TextChanged);

            this.label1.Location = new System.Drawing.Point(10, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Min:";

            this.groupBox2.Controls.Add(this.saturationSlider);
            this.groupBox2.Controls.Add(this.maxSBox);
            this.groupBox2.Controls.Add(this.minSBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(10, 245);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(280, 75);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Saturation";

            this.saturationSlider.Location = new System.Drawing.Point(8, 45);
            this.saturationSlider.Name = "saturationSlider";
            this.saturationSlider.Size = new System.Drawing.Size(262, 23);
            this.saturationSlider.TabIndex = 4;
            this.saturationSlider.Type = AForge.Controls.ColorSlider.ColorSliderType.InnerGradient;
            this.saturationSlider.ValuesChanged += new System.EventHandler(this.saturationSlider_ValuesChanged);

            this.maxSBox.Location = new System.Drawing.Point(218, 20);
            this.maxSBox.Name = "maxSBox";
            this.maxSBox.Size = new System.Drawing.Size(50, 20);
            this.maxSBox.TabIndex = 3;
            this.maxSBox.TextChanged += new System.EventHandler(this.maxSBox_TextChanged);

            this.minSBox.Location = new System.Drawing.Point(40, 20);
            this.minSBox.Name = "minSBox";
            this.minSBox.Size = new System.Drawing.Size(50, 20);
            this.minSBox.TabIndex = 2;
            this.minSBox.TextChanged += new System.EventHandler(this.minSBox_TextChanged);

            this.label4.Location = new System.Drawing.Point(186, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "Max:";

            this.label3.Location = new System.Drawing.Point(10, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Min:";

            this.groupBox3.Controls.Add(this.luminanceSlider);
            this.groupBox3.Controls.Add(this.maxLBox);
            this.groupBox3.Controls.Add(this.minLBox);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(10, 325);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(280, 75);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Luminance";

            this.luminanceSlider.Location = new System.Drawing.Point(8, 45);
            this.luminanceSlider.Name = "luminanceSlider";
            this.luminanceSlider.Size = new System.Drawing.Size(262, 23);
            this.luminanceSlider.TabIndex = 9;
            this.luminanceSlider.Type = AForge.Controls.ColorSlider.ColorSliderType.InnerGradient;
            this.luminanceSlider.ValuesChanged += new System.EventHandler(this.luminanceSlider_ValuesChanged);

            this.maxLBox.Location = new System.Drawing.Point(218, 20);
            this.maxLBox.Name = "maxLBox";
            this.maxLBox.Size = new System.Drawing.Size(50, 20);
            this.maxLBox.TabIndex = 8;
            this.maxLBox.TextChanged += new System.EventHandler(this.maxLBox_TextChanged);

            this.minLBox.Location = new System.Drawing.Point(40, 20);
            this.minLBox.Name = "minLBox";
            this.minLBox.Size = new System.Drawing.Size(50, 20);
            this.minLBox.TabIndex = 7;
            this.minLBox.TextChanged += new System.EventHandler(this.minLBox_TextChanged);

            this.label5.Location = new System.Drawing.Point(186, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "Max:";

            this.label6.Location = new System.Drawing.Point(10, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 16);
            this.label6.TabIndex = 5;
            this.label6.Text = "Min:";

            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.okButton.Location = new System.Drawing.Point(133, 406);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(157, 23);
            this.okButton.TabIndex = 11;
            this.okButton.Text = "Close";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);

            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(301, 435);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSLFilteringForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HSL Filtering";
            this.Load += new System.EventHandler(this.HSLFilteringForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion


        private AForge.Controls.HuePicker huePicker;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox maxHBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox minSBox;
        private System.Windows.Forms.TextBox maxSBox;
        private AForge.Controls.ColorSlider saturationSlider;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox minLBox;
        private System.Windows.Forms.TextBox maxLBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox minHBox;
        private AForge.Controls.ColorSlider luminanceSlider;
        private System.ComponentModel.Container components = null;


    }
}

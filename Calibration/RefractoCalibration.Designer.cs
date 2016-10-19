namespace Calibration
{
    partial class RefractoCalibration
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Liquid_lens_control_Reset = new System.Windows.Forms.TrackBar();
            this.Liquid_lens_control_Min = new System.Windows.Forms.TrackBar();
            this.Liquid_lens_control_Max = new System.Windows.Forms.TrackBar();
            this.ResetValue = new System.Windows.Forms.Label();
            this.MinValue = new System.Windows.Forms.Label();
            this.MaxValue = new System.Windows.Forms.Label();
            this.Reset_Value = new System.Windows.Forms.Label();
            this.Min_Value = new System.Windows.Forms.Label();
            this.Max_Value = new System.Windows.Forms.Label();
            this.RingCapture = new System.Windows.Forms.Button();
            this.IsCalibration = new System.Windows.Forms.CheckBox();
            this.IsZeroPoint = new System.Windows.Forms.Button();
            this.SaveCalib_btn = new System.Windows.Forms.Button();
            this.AOI_X = new System.Windows.Forms.Label();
            this.AOI_Y = new System.Windows.Forms.Label();
            this.AOI_X_UPDOWN = new System.Windows.Forms.NumericUpDown();
            this.AOI_Y_UPDOWN = new System.Windows.Forms.NumericUpDown();
            this.Refracto_Gain_Updown = new System.Windows.Forms.NumericUpDown();
            this.RefractoGain = new System.Windows.Forms.Label();
            this.DisplayCenter_Y_UpDown = new System.Windows.Forms.NumericUpDown();
            this.DisplayCenter_X_UpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.DisplayCenterX = new System.Windows.Forms.Label();
            this.IsLiveReading = new System.Windows.Forms.CheckBox();
            this.Reset_lbl = new System.Windows.Forms.Label();
            this.Retake_btn = new System.Windows.Forms.Button();
            this.Measure_ring_btn = new System.Windows.Forms.Button();
            this.Proximity_status = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Liquid_lens_control_Reset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Liquid_lens_control_Min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Liquid_lens_control_Max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AOI_X_UPDOWN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AOI_Y_UPDOWN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Refracto_Gain_Updown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayCenter_Y_UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayCenter_X_UpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // Liquid_lens_control_Reset
            // 
            this.Liquid_lens_control_Reset.Location = new System.Drawing.Point(16, 415);
            this.Liquid_lens_control_Reset.Maximum = 255;
            this.Liquid_lens_control_Reset.Minimum = 1;
            this.Liquid_lens_control_Reset.Name = "Liquid_lens_control_Reset";
            this.Liquid_lens_control_Reset.Size = new System.Drawing.Size(164, 45);
            this.Liquid_lens_control_Reset.TabIndex = 1;
            this.Liquid_lens_control_Reset.Value = 1;
            this.Liquid_lens_control_Reset.Scroll += new System.EventHandler(this.Liquid_lens_control_Scroll);
            // 
            // Liquid_lens_control_Min
            // 
            this.Liquid_lens_control_Min.Location = new System.Drawing.Point(16, 480);
            this.Liquid_lens_control_Min.Maximum = 255;
            this.Liquid_lens_control_Min.Minimum = 1;
            this.Liquid_lens_control_Min.Name = "Liquid_lens_control_Min";
            this.Liquid_lens_control_Min.Size = new System.Drawing.Size(164, 45);
            this.Liquid_lens_control_Min.TabIndex = 2;
            this.Liquid_lens_control_Min.Value = 1;
            this.Liquid_lens_control_Min.Scroll += new System.EventHandler(this.Liquid_lens_control_Min_Scroll);
            // 
            // Liquid_lens_control_Max
            // 
            this.Liquid_lens_control_Max.Location = new System.Drawing.Point(16, 549);
            this.Liquid_lens_control_Max.Maximum = 255;
            this.Liquid_lens_control_Max.Minimum = 1;
            this.Liquid_lens_control_Max.Name = "Liquid_lens_control_Max";
            this.Liquid_lens_control_Max.Size = new System.Drawing.Size(164, 45);
            this.Liquid_lens_control_Max.TabIndex = 3;
            this.Liquid_lens_control_Max.Value = 1;
            this.Liquid_lens_control_Max.Scroll += new System.EventHandler(this.Liquid_lens_control_Max_Scroll);
            // 
            // ResetValue
            // 
            this.ResetValue.AutoSize = true;
            this.ResetValue.Location = new System.Drawing.Point(13, 399);
            this.ResetValue.Name = "ResetValue";
            this.ResetValue.Size = new System.Drawing.Size(62, 13);
            this.ResetValue.TabIndex = 4;
            this.ResetValue.Text = "ResetValue";
            // 
            // MinValue
            // 
            this.MinValue.AutoSize = true;
            this.MinValue.Location = new System.Drawing.Point(13, 463);
            this.MinValue.Name = "MinValue";
            this.MinValue.Size = new System.Drawing.Size(54, 13);
            this.MinValue.TabIndex = 5;
            this.MinValue.Text = "MaxValue";
            // 
            // MaxValue
            // 
            this.MaxValue.AutoSize = true;
            this.MaxValue.Location = new System.Drawing.Point(13, 528);
            this.MaxValue.Name = "MaxValue";
            this.MaxValue.Size = new System.Drawing.Size(51, 13);
            this.MaxValue.TabIndex = 6;
            this.MaxValue.Text = "MinValue";
            // 
            // Reset_Value
            // 
            this.Reset_Value.AutoSize = true;
            this.Reset_Value.Location = new System.Drawing.Point(186, 430);
            this.Reset_Value.Name = "Reset_Value";
            this.Reset_Value.Size = new System.Drawing.Size(7, 13);
            this.Reset_Value.TabIndex = 7;
            this.Reset_Value.Text = "\r\n";
            this.Reset_Value.Click += new System.EventHandler(this.Reset_Value_Click);
            // 
            // Min_Value
            // 
            this.Min_Value.AutoSize = true;
            this.Min_Value.Location = new System.Drawing.Point(186, 480);
            this.Min_Value.Name = "Min_Value";
            this.Min_Value.Size = new System.Drawing.Size(7, 13);
            this.Min_Value.TabIndex = 11;
            this.Min_Value.Text = "\r\n";
            this.Min_Value.Click += new System.EventHandler(this.Min_Value_Click);
            // 
            // Max_Value
            // 
            this.Max_Value.AutoSize = true;
            this.Max_Value.Location = new System.Drawing.Point(186, 549);
            this.Max_Value.Name = "Max_Value";
            this.Max_Value.Size = new System.Drawing.Size(7, 13);
            this.Max_Value.TabIndex = 12;
            this.Max_Value.Text = "\r\n";
            this.Max_Value.Click += new System.EventHandler(this.Max_Value_Click);
            // 
            // RingCapture
            // 
            this.RingCapture.Location = new System.Drawing.Point(43, 343);
            this.RingCapture.Name = "RingCapture";
            this.RingCapture.Size = new System.Drawing.Size(150, 41);
            this.RingCapture.TabIndex = 13;
            this.RingCapture.TabStop = false;
            this.RingCapture.Text = "RingCapture";
            this.RingCapture.UseVisualStyleBackColor = true;
            this.RingCapture.Click += new System.EventHandler(this.RingCapture_Click);
            this.RingCapture.Enter += new System.EventHandler(this.RingCapture_Enter);
            // 
            // IsCalibration
            // 
            this.IsCalibration.AutoSize = true;
            this.IsCalibration.Checked = true;
            this.IsCalibration.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsCalibration.Location = new System.Drawing.Point(14, 299);
            this.IsCalibration.Name = "IsCalibration";
            this.IsCalibration.Size = new System.Drawing.Size(72, 17);
            this.IsCalibration.TabIndex = 14;
            this.IsCalibration.Text = "R1andR2";
            this.IsCalibration.UseVisualStyleBackColor = true;
            this.IsCalibration.CheckedChanged += new System.EventHandler(this.IsCalibration_CheckedChanged);
            // 
            // IsZeroPoint
            // 
            this.IsZeroPoint.Location = new System.Drawing.Point(16, 63);
            this.IsZeroPoint.Name = "IsZeroPoint";
            this.IsZeroPoint.Size = new System.Drawing.Size(88, 23);
            this.IsZeroPoint.TabIndex = 15;
            this.IsZeroPoint.Text = "SaveZero";
            this.IsZeroPoint.UseVisualStyleBackColor = true;
            this.IsZeroPoint.Click += new System.EventHandler(this.IsZeroPoint_Click);
            // 
            // SaveCalib_btn
            // 
            this.SaveCalib_btn.Location = new System.Drawing.Point(134, 63);
            this.SaveCalib_btn.Name = "SaveCalib_btn";
            this.SaveCalib_btn.Size = new System.Drawing.Size(87, 23);
            this.SaveCalib_btn.TabIndex = 16;
            this.SaveCalib_btn.Text = "SaveClibration";
            this.SaveCalib_btn.UseVisualStyleBackColor = true;
            this.SaveCalib_btn.Click += new System.EventHandler(this.SaveCalib_btn_Click);
            // 
            // AOI_X
            // 
            this.AOI_X.AutoSize = true;
            this.AOI_X.Location = new System.Drawing.Point(13, 116);
            this.AOI_X.Name = "AOI_X";
            this.AOI_X.Size = new System.Drawing.Size(35, 13);
            this.AOI_X.TabIndex = 17;
            this.AOI_X.Text = "AOI X";
            // 
            // AOI_Y
            // 
            this.AOI_Y.AutoSize = true;
            this.AOI_Y.Location = new System.Drawing.Point(144, 116);
            this.AOI_Y.Name = "AOI_Y";
            this.AOI_Y.Size = new System.Drawing.Size(35, 13);
            this.AOI_Y.TabIndex = 18;
            this.AOI_Y.Text = "AOI Y";
            // 
            // AOI_X_UPDOWN
            // 
            this.AOI_X_UPDOWN.Location = new System.Drawing.Point(16, 132);
            this.AOI_X_UPDOWN.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.AOI_X_UPDOWN.Name = "AOI_X_UPDOWN";
            this.AOI_X_UPDOWN.Size = new System.Drawing.Size(48, 20);
            this.AOI_X_UPDOWN.TabIndex = 19;
            this.AOI_X_UPDOWN.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.AOI_X_UPDOWN.ValueChanged += new System.EventHandler(this.AOI_X_UPDOWN_ValueChanged);
            // 
            // AOI_Y_UPDOWN
            // 
            this.AOI_Y_UPDOWN.Location = new System.Drawing.Point(147, 132);
            this.AOI_Y_UPDOWN.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.AOI_Y_UPDOWN.Name = "AOI_Y_UPDOWN";
            this.AOI_Y_UPDOWN.Size = new System.Drawing.Size(46, 20);
            this.AOI_Y_UPDOWN.TabIndex = 20;
            this.AOI_Y_UPDOWN.Value = new decimal(new int[] {
            310,
            0,
            0,
            0});
            this.AOI_Y_UPDOWN.ValueChanged += new System.EventHandler(this.AOI_Y_UPDOWN_ValueChanged);
            // 
            // Refracto_Gain_Updown
            // 
            this.Refracto_Gain_Updown.Location = new System.Drawing.Point(16, 234);
            this.Refracto_Gain_Updown.Maximum = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.Refracto_Gain_Updown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Refracto_Gain_Updown.Name = "Refracto_Gain_Updown";
            this.Refracto_Gain_Updown.Size = new System.Drawing.Size(46, 20);
            this.Refracto_Gain_Updown.TabIndex = 22;
            this.Refracto_Gain_Updown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.Refracto_Gain_Updown.ValueChanged += new System.EventHandler(this.Refracto_Gain_Updown_ValueChanged);
            // 
            // RefractoGain
            // 
            this.RefractoGain.AutoSize = true;
            this.RefractoGain.Location = new System.Drawing.Point(13, 218);
            this.RefractoGain.Name = "RefractoGain";
            this.RefractoGain.Size = new System.Drawing.Size(73, 13);
            this.RefractoGain.TabIndex = 23;
            this.RefractoGain.Text = "Refracto Gain";
            // 
            // DisplayCenter_Y_UpDown
            // 
            this.DisplayCenter_Y_UpDown.Location = new System.Drawing.Point(147, 180);
            this.DisplayCenter_Y_UpDown.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.DisplayCenter_Y_UpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DisplayCenter_Y_UpDown.Name = "DisplayCenter_Y_UpDown";
            this.DisplayCenter_Y_UpDown.Size = new System.Drawing.Size(46, 20);
            this.DisplayCenter_Y_UpDown.TabIndex = 24;
            this.DisplayCenter_Y_UpDown.Value = new decimal(new int[] {
            415,
            0,
            0,
            0});
            this.DisplayCenter_Y_UpDown.ValueChanged += new System.EventHandler(this.DisplayCenter_Y_UpDown_ValueChanged);
            // 
            // DisplayCenter_X_UpDown
            // 
            this.DisplayCenter_X_UpDown.Location = new System.Drawing.Point(16, 180);
            this.DisplayCenter_X_UpDown.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.DisplayCenter_X_UpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DisplayCenter_X_UpDown.Name = "DisplayCenter_X_UpDown";
            this.DisplayCenter_X_UpDown.Size = new System.Drawing.Size(46, 20);
            this.DisplayCenter_X_UpDown.TabIndex = 25;
            this.DisplayCenter_X_UpDown.Value = new decimal(new int[] {
            510,
            0,
            0,
            0});
            this.DisplayCenter_X_UpDown.ValueChanged += new System.EventHandler(this.DisplayCenter_X_UpDown_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(144, 164);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "DisplayCenter Y";
            // 
            // DisplayCenterX
            // 
            this.DisplayCenterX.AutoSize = true;
            this.DisplayCenterX.Location = new System.Drawing.Point(13, 164);
            this.DisplayCenterX.Name = "DisplayCenterX";
            this.DisplayCenterX.Size = new System.Drawing.Size(82, 13);
            this.DisplayCenterX.TabIndex = 27;
            this.DisplayCenterX.Text = "DisplayCenter X";
            // 
            // IsLiveReading
            // 
            this.IsLiveReading.AutoSize = true;
            this.IsLiveReading.Location = new System.Drawing.Point(16, 276);
            this.IsLiveReading.Name = "IsLiveReading";
            this.IsLiveReading.Size = new System.Drawing.Size(89, 17);
            this.IsLiveReading.TabIndex = 31;
            this.IsLiveReading.Text = "Live Reading";
            this.IsLiveReading.UseVisualStyleBackColor = true;
            this.IsLiveReading.CheckedChanged += new System.EventHandler(this.IsLiveReading_CheckedChanged);
            // 
            // Reset_lbl
            // 
            this.Reset_lbl.AutoSize = true;
            this.Reset_lbl.Location = new System.Drawing.Point(186, 417);
            this.Reset_lbl.Name = "Reset_lbl";
            this.Reset_lbl.Size = new System.Drawing.Size(0, 13);
            this.Reset_lbl.TabIndex = 32;
            // 
            // Retake_btn
            // 
            this.Retake_btn.Location = new System.Drawing.Point(80, 24);
            this.Retake_btn.Name = "Retake_btn";
            this.Retake_btn.Size = new System.Drawing.Size(75, 23);
            this.Retake_btn.TabIndex = 33;
            this.Retake_btn.Text = "Retake";
            this.Retake_btn.UseVisualStyleBackColor = true;
            this.Retake_btn.Click += new System.EventHandler(this.Retake_btn_Click);
            // 
            // Measure_ring_btn
            // 
            this.Measure_ring_btn.Location = new System.Drawing.Point(118, 231);
            this.Measure_ring_btn.Name = "Measure_ring_btn";
            this.Measure_ring_btn.Size = new System.Drawing.Size(85, 23);
            this.Measure_ring_btn.TabIndex = 34;
            this.Measure_ring_btn.Text = "MeasureRing";
            this.Measure_ring_btn.UseVisualStyleBackColor = true;
            this.Measure_ring_btn.Click += new System.EventHandler(this.Measure_ring_btn_Click);
            // 
            // Proximity_status
            // 
            this.Proximity_status.AutoSize = true;
            this.Proximity_status.Location = new System.Drawing.Point(120, 280);
            this.Proximity_status.Name = "Proximity_status";
            this.Proximity_status.Size = new System.Drawing.Size(0, 13);
            this.Proximity_status.TabIndex = 35;
            // 
            // RefractoCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Proximity_status);
            this.Controls.Add(this.Measure_ring_btn);
            this.Controls.Add(this.Retake_btn);
            this.Controls.Add(this.Reset_lbl);
            this.Controls.Add(this.IsLiveReading);
            this.Controls.Add(this.DisplayCenterX);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DisplayCenter_X_UpDown);
            this.Controls.Add(this.DisplayCenter_Y_UpDown);
            this.Controls.Add(this.RefractoGain);
            this.Controls.Add(this.Refracto_Gain_Updown);
            this.Controls.Add(this.AOI_Y_UPDOWN);
            this.Controls.Add(this.AOI_X_UPDOWN);
            this.Controls.Add(this.AOI_Y);
            this.Controls.Add(this.AOI_X);
            this.Controls.Add(this.SaveCalib_btn);
            this.Controls.Add(this.IsZeroPoint);
            this.Controls.Add(this.IsCalibration);
            this.Controls.Add(this.RingCapture);
            this.Controls.Add(this.Max_Value);
            this.Controls.Add(this.Min_Value);
            this.Controls.Add(this.Reset_Value);
            this.Controls.Add(this.MaxValue);
            this.Controls.Add(this.MinValue);
            this.Controls.Add(this.ResetValue);
            this.Controls.Add(this.Liquid_lens_control_Max);
            this.Controls.Add(this.Liquid_lens_control_Min);
            this.Controls.Add(this.Liquid_lens_control_Reset);
            this.Name = "RefractoCalibration";
            this.Size = new System.Drawing.Size(237, 609);
            this.Load += new System.EventHandler(this.RefractoCalibration_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Liquid_lens_control_Reset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Liquid_lens_control_Min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Liquid_lens_control_Max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AOI_X_UPDOWN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AOI_Y_UPDOWN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Refracto_Gain_Updown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayCenter_Y_UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayCenter_X_UpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar Liquid_lens_control_Reset;
        private System.Windows.Forms.NumericUpDown liquid_lens_value;
        private System.Windows.Forms.TrackBar Liquid_lens_control_Min;
        private System.Windows.Forms.TrackBar Liquid_lens_control_Max;
        private System.Windows.Forms.Label ResetValue;
        private System.Windows.Forms.Label MinValue;
        private System.Windows.Forms.Label MaxValue;
        private System.Windows.Forms.Label Reset_Value;
        private System.Windows.Forms.Label Min_Value;
        private System.Windows.Forms.Label Max_Value;
        private System.Windows.Forms.Button RingCapture;
        private System.Windows.Forms.CheckBox IsCalibration;
        private System.Windows.Forms.Button SaveCalib_btn;
        private System.Windows.Forms.Label AOI_X;
        private System.Windows.Forms.Label AOI_Y;
        private System.Windows.Forms.NumericUpDown AOI_X_UPDOWN;
        private System.Windows.Forms.NumericUpDown AOI_Y_UPDOWN;
        private System.Windows.Forms.NumericUpDown Refracto_Gain_Updown;
        private System.Windows.Forms.Label RefractoGain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label DisplayCenterX;
        private System.Windows.Forms.CheckBox IsLiveReading;
        private System.Windows.Forms.Label Reset_lbl;
        public System.Windows.Forms.Button IsZeroPoint;
        private System.Windows.Forms.Button Retake_btn;
        private System.Windows.Forms.Button Measure_ring_btn;
        public System.Windows.Forms.NumericUpDown DisplayCenter_X_UpDown;
        public System.Windows.Forms.Label Proximity_status;
        public System.Windows.Forms.NumericUpDown DisplayCenter_Y_UpDown;


    }
}

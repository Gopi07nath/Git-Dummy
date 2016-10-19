namespace Calibration
{
    partial class AlignmentUI
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
            this.IrImageCapture_btn = new System.Windows.Forms.Button();
            this.whiteLightCapture_btn = new System.Windows.Forms.Button();
            this.whiteLightMode_rb = new System.Windows.Forms.RadioButton();
            this.IrMode_rb = new System.Windows.Forms.RadioButton();
            this.resume_btn = new System.Windows.Forms.Button();
            this.deviceId_lbl = new System.Windows.Forms.Label();
            this.perToInnerVarVal_lbl = new System.Windows.Forms.Label();
            this.AvgRangeVal_lbl = new System.Windows.Forms.Label();
            this.covVal_lbl = new System.Windows.Forms.Label();
            this.avgIntensityVal_lbl = new System.Windows.Forms.Label();
            this.covRange_lbl = new System.Windows.Forms.Label();
            this.cov_lbl = new System.Windows.Forms.Label();
            this.peripheryToInnerRange_lbl = new System.Windows.Forms.Label();
            this.peripherytoinnerVar_lbl = new System.Windows.Forms.Label();
            this.AvgBrightness_lbl = new System.Windows.Forms.Label();
            this.Image_Metrics = new System.Windows.Forms.Label();
            this.uniformillumination_gbx = new System.Windows.Forms.GroupBox();
            this.SaveImage_IlluminationGrid_btn = new System.Windows.Forms.Button();
            this.illuminationGrid_cbx = new System.Windows.Forms.CheckBox();
            this.ModeSelection_panel = new System.Windows.Forms.GroupBox();
            this.CaptureNResume_gbx = new System.Windows.Forms.GroupBox();
            this.uniformillumination_gbx.SuspendLayout();
            this.ModeSelection_panel.SuspendLayout();
            this.CaptureNResume_gbx.SuspendLayout();
            this.SuspendLayout();
            // 
            // IrImageCapture_btn
            // 
            this.IrImageCapture_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IrImageCapture_btn.ForeColor = System.Drawing.SystemColors.Desktop;
            this.IrImageCapture_btn.Location = new System.Drawing.Point(9, 20);
            this.IrImageCapture_btn.Name = "IrImageCapture_btn";
            this.IrImageCapture_btn.Size = new System.Drawing.Size(103, 46);
            this.IrImageCapture_btn.TabIndex = 3;
            this.IrImageCapture_btn.Text = "&IR Capture";
            this.IrImageCapture_btn.UseVisualStyleBackColor = true;
            this.IrImageCapture_btn.Click += new System.EventHandler(this.IrImageCapture_btn_Click);
            // 
            // whiteLightCapture_btn
            // 
            this.whiteLightCapture_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.whiteLightCapture_btn.ForeColor = System.Drawing.SystemColors.Desktop;
            this.whiteLightCapture_btn.Location = new System.Drawing.Point(118, 20);
            this.whiteLightCapture_btn.Name = "whiteLightCapture_btn";
            this.whiteLightCapture_btn.Size = new System.Drawing.Size(102, 46);
            this.whiteLightCapture_btn.TabIndex = 4;
            this.whiteLightCapture_btn.Text = "&White light Capture";
            this.whiteLightCapture_btn.UseVisualStyleBackColor = true;
            this.whiteLightCapture_btn.Click += new System.EventHandler(this.whiteLightCapture_btn_Click);
            // 
            // whiteLightMode_rb
            // 
            this.whiteLightMode_rb.AutoSize = true;
            this.whiteLightMode_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.whiteLightMode_rb.ForeColor = System.Drawing.SystemColors.Desktop;
            this.whiteLightMode_rb.Location = new System.Drawing.Point(14, 56);
            this.whiteLightMode_rb.Name = "whiteLightMode_rb";
            this.whiteLightMode_rb.Size = new System.Drawing.Size(86, 19);
            this.whiteLightMode_rb.TabIndex = 79;
            this.whiteLightMode_rb.Text = "White Light";
            this.whiteLightMode_rb.UseVisualStyleBackColor = true;
            // 
            // IrMode_rb
            // 
            this.IrMode_rb.AutoSize = true;
            this.IrMode_rb.Checked = true;
            this.IrMode_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IrMode_rb.ForeColor = System.Drawing.SystemColors.Desktop;
            this.IrMode_rb.Location = new System.Drawing.Point(14, 31);
            this.IrMode_rb.Name = "IrMode_rb";
            this.IrMode_rb.Size = new System.Drawing.Size(67, 19);
            this.IrMode_rb.TabIndex = 76;
            this.IrMode_rb.TabStop = true;
            this.IrMode_rb.Text = "IR Light";
            this.IrMode_rb.UseVisualStyleBackColor = true;
            this.IrMode_rb.CheckedChanged += new System.EventHandler(this.IrMode_rb_CheckedChanged);
            // 
            // resume_btn
            // 
            this.resume_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resume_btn.ForeColor = System.Drawing.SystemColors.Desktop;
            this.resume_btn.Location = new System.Drawing.Point(14, 72);
            this.resume_btn.Name = "resume_btn";
            this.resume_btn.Size = new System.Drawing.Size(201, 53);
            this.resume_btn.TabIndex = 74;
            this.resume_btn.Text = "Resume";
            this.resume_btn.UseVisualStyleBackColor = true;
            this.resume_btn.Click += new System.EventHandler(this.resume_btn_Click);
            // 
            // deviceId_lbl
            // 
            this.deviceId_lbl.AutoSize = true;
            this.deviceId_lbl.Location = new System.Drawing.Point(14, 207);
            this.deviceId_lbl.Name = "deviceId_lbl";
            this.deviceId_lbl.Size = new System.Drawing.Size(0, 13);
            this.deviceId_lbl.TabIndex = 83;
            // 
            // perToInnerVarVal_lbl
            // 
            this.perToInnerVarVal_lbl.AutoSize = true;
            this.perToInnerVarVal_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.perToInnerVarVal_lbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.perToInnerVarVal_lbl.Location = new System.Drawing.Point(198, 372);
            this.perToInnerVarVal_lbl.Name = "perToInnerVarVal_lbl";
            this.perToInnerVarVal_lbl.Size = new System.Drawing.Size(0, 13);
            this.perToInnerVarVal_lbl.TabIndex = 35;
            // 
            // AvgRangeVal_lbl
            // 
            this.AvgRangeVal_lbl.AutoSize = true;
            this.AvgRangeVal_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AvgRangeVal_lbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.AvgRangeVal_lbl.Location = new System.Drawing.Point(17, 278);
            this.AvgRangeVal_lbl.Name = "AvgRangeVal_lbl";
            this.AvgRangeVal_lbl.Size = new System.Drawing.Size(0, 13);
            this.AvgRangeVal_lbl.TabIndex = 36;
            // 
            // covVal_lbl
            // 
            this.covVal_lbl.AutoSize = true;
            this.covVal_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.covVal_lbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.covVal_lbl.Location = new System.Drawing.Point(174, 312);
            this.covVal_lbl.Name = "covVal_lbl";
            this.covVal_lbl.Size = new System.Drawing.Size(0, 13);
            this.covVal_lbl.TabIndex = 34;
            // 
            // avgIntensityVal_lbl
            // 
            this.avgIntensityVal_lbl.AutoSize = true;
            this.avgIntensityVal_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.avgIntensityVal_lbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.avgIntensityVal_lbl.Location = new System.Drawing.Point(104, 260);
            this.avgIntensityVal_lbl.Name = "avgIntensityVal_lbl";
            this.avgIntensityVal_lbl.Size = new System.Drawing.Size(0, 13);
            this.avgIntensityVal_lbl.TabIndex = 33;
            // 
            // covRange_lbl
            // 
            this.covRange_lbl.AutoSize = true;
            this.covRange_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.covRange_lbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.covRange_lbl.Location = new System.Drawing.Point(17, 334);
            this.covRange_lbl.Name = "covRange_lbl";
            this.covRange_lbl.Size = new System.Drawing.Size(0, 13);
            this.covRange_lbl.TabIndex = 38;
            // 
            // cov_lbl
            // 
            this.cov_lbl.AutoSize = true;
            this.cov_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cov_lbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cov_lbl.Location = new System.Drawing.Point(5, 310);
            this.cov_lbl.Name = "cov_lbl";
            this.cov_lbl.Size = new System.Drawing.Size(166, 13);
            this.cov_lbl.TabIndex = 22;
            this.cov_lbl.Text = " Top to Bottom Intensity Variation:";
            // 
            // peripheryToInnerRange_lbl
            // 
            this.peripheryToInnerRange_lbl.AutoSize = true;
            this.peripheryToInnerRange_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.peripheryToInnerRange_lbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.peripheryToInnerRange_lbl.Location = new System.Drawing.Point(16, 397);
            this.peripheryToInnerRange_lbl.Name = "peripheryToInnerRange_lbl";
            this.peripheryToInnerRange_lbl.Size = new System.Drawing.Size(0, 13);
            this.peripheryToInnerRange_lbl.TabIndex = 39;
            // 
            // peripherytoinnerVar_lbl
            // 
            this.peripherytoinnerVar_lbl.AutoSize = true;
            this.peripherytoinnerVar_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.peripherytoinnerVar_lbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.peripherytoinnerVar_lbl.Location = new System.Drawing.Point(9, 372);
            this.peripherytoinnerVar_lbl.Name = "peripherytoinnerVar_lbl";
            this.peripherytoinnerVar_lbl.Size = new System.Drawing.Size(183, 13);
            this.peripherytoinnerVar_lbl.TabIndex = 25;
            this.peripherytoinnerVar_lbl.Text = "Periphery To Inner Intensity Variation:";
            // 
            // AvgBrightness_lbl
            // 
            this.AvgBrightness_lbl.AutoSize = true;
            this.AvgBrightness_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.AvgBrightness_lbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.AvgBrightness_lbl.Location = new System.Drawing.Point(9, 258);
            this.AvgBrightness_lbl.Name = "AvgBrightness_lbl";
            this.AvgBrightness_lbl.Size = new System.Drawing.Size(92, 13);
            this.AvgBrightness_lbl.TabIndex = 18;
            this.AvgBrightness_lbl.Text = "Average Intensity:";
            // 
            // Image_Metrics
            // 
            this.Image_Metrics.AutoSize = true;
            this.Image_Metrics.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Image_Metrics.ForeColor = System.Drawing.SystemColors.Highlight;
            this.Image_Metrics.Location = new System.Drawing.Point(5, 233);
            this.Image_Metrics.Name = "Image_Metrics";
            this.Image_Metrics.Size = new System.Drawing.Size(105, 16);
            this.Image_Metrics.TabIndex = 84;
            this.Image_Metrics.Text = "Image Metrics";
            // 
            // uniformillumination_gbx
            // 
            this.uniformillumination_gbx.Controls.Add(this.SaveImage_IlluminationGrid_btn);
            this.uniformillumination_gbx.Controls.Add(this.illuminationGrid_cbx);
            this.uniformillumination_gbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uniformillumination_gbx.ForeColor = System.Drawing.SystemColors.Highlight;
            this.uniformillumination_gbx.Location = new System.Drawing.Point(4, 429);
            this.uniformillumination_gbx.Name = "uniformillumination_gbx";
            this.uniformillumination_gbx.Size = new System.Drawing.Size(230, 100);
            this.uniformillumination_gbx.TabIndex = 85;
            this.uniformillumination_gbx.TabStop = false;
            this.uniformillumination_gbx.Text = "Illumination Grid";
            // 
            // SaveImage_IlluminationGrid_btn
            // 
            this.SaveImage_IlluminationGrid_btn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.SaveImage_IlluminationGrid_btn.Location = new System.Drawing.Point(48, 62);
            this.SaveImage_IlluminationGrid_btn.Name = "SaveImage_IlluminationGrid_btn";
            this.SaveImage_IlluminationGrid_btn.Size = new System.Drawing.Size(104, 23);
            this.SaveImage_IlluminationGrid_btn.TabIndex = 1;
            this.SaveImage_IlluminationGrid_btn.Text = "Save Image";
            this.SaveImage_IlluminationGrid_btn.UseVisualStyleBackColor = true;
            this.SaveImage_IlluminationGrid_btn.Click += new System.EventHandler(this.SaveImage_IlluminationGrid_btn_Click);
            // 
            // illuminationGrid_cbx
            // 
            this.illuminationGrid_cbx.AutoSize = true;
            this.illuminationGrid_cbx.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.illuminationGrid_cbx.Location = new System.Drawing.Point(26, 30);
            this.illuminationGrid_cbx.Name = "illuminationGrid_cbx";
            this.illuminationGrid_cbx.Size = new System.Drawing.Size(133, 19);
            this.illuminationGrid_cbx.TabIndex = 0;
            this.illuminationGrid_cbx.Text = "Illumination Grid";
            this.illuminationGrid_cbx.UseVisualStyleBackColor = true;
            this.illuminationGrid_cbx.CheckedChanged += new System.EventHandler(this.illuminationGrid_cbx_CheckedChanged);
            // 
            // ModeSelection_panel
            // 
            this.ModeSelection_panel.Controls.Add(this.whiteLightMode_rb);
            this.ModeSelection_panel.Controls.Add(this.IrMode_rb);
            this.ModeSelection_panel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ModeSelection_panel.ForeColor = System.Drawing.SystemColors.Highlight;
            this.ModeSelection_panel.Location = new System.Drawing.Point(8, 3);
            this.ModeSelection_panel.Name = "ModeSelection_panel";
            this.ModeSelection_panel.Size = new System.Drawing.Size(226, 80);
            this.ModeSelection_panel.TabIndex = 86;
            this.ModeSelection_panel.TabStop = false;
            this.ModeSelection_panel.Text = "Live Mode Guidance";
            // 
            // CaptureNResume_gbx
            // 
            this.CaptureNResume_gbx.Controls.Add(this.IrImageCapture_btn);
            this.CaptureNResume_gbx.Controls.Add(this.whiteLightCapture_btn);
            this.CaptureNResume_gbx.Controls.Add(this.resume_btn);
            this.CaptureNResume_gbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CaptureNResume_gbx.ForeColor = System.Drawing.SystemColors.Highlight;
            this.CaptureNResume_gbx.Location = new System.Drawing.Point(8, 89);
            this.CaptureNResume_gbx.Name = "CaptureNResume_gbx";
            this.CaptureNResume_gbx.Size = new System.Drawing.Size(226, 131);
            this.CaptureNResume_gbx.TabIndex = 87;
            this.CaptureNResume_gbx.TabStop = false;
            this.CaptureNResume_gbx.Text = "Image Capture";
            // 
            // AlignmentUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CaptureNResume_gbx);
            this.Controls.Add(this.ModeSelection_panel);
            this.Controls.Add(this.uniformillumination_gbx);
            this.Controls.Add(this.Image_Metrics);
            this.Controls.Add(this.perToInnerVarVal_lbl);
            this.Controls.Add(this.peripheryToInnerRange_lbl);
            this.Controls.Add(this.peripherytoinnerVar_lbl);
            this.Controls.Add(this.AvgBrightness_lbl);
            this.Controls.Add(this.deviceId_lbl);
            this.Controls.Add(this.covVal_lbl);
            this.Controls.Add(this.covRange_lbl);
            this.Controls.Add(this.cov_lbl);
            this.Controls.Add(this.avgIntensityVal_lbl);
            this.Controls.Add(this.AvgRangeVal_lbl);
            this.Name = "AlignmentUI";
            this.Size = new System.Drawing.Size(237, 609);
            this.uniformillumination_gbx.ResumeLayout(false);
            this.uniformillumination_gbx.PerformLayout();
            this.ModeSelection_panel.ResumeLayout(false);
            this.ModeSelection_panel.PerformLayout();
            this.CaptureNResume_gbx.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button IrImageCapture_btn;
        private System.Windows.Forms.Button whiteLightCapture_btn;
        private System.Windows.Forms.RadioButton whiteLightMode_rb;
        private System.Windows.Forms.RadioButton IrMode_rb;
        private System.Windows.Forms.Button resume_btn;
        private System.Windows.Forms.Label deviceId_lbl;
        private System.Windows.Forms.Label perToInnerVarVal_lbl;
        private System.Windows.Forms.Label AvgRangeVal_lbl;
        private System.Windows.Forms.Label covVal_lbl;
        private System.Windows.Forms.Label avgIntensityVal_lbl;
        private System.Windows.Forms.Label covRange_lbl;
        private System.Windows.Forms.Label cov_lbl;
        private System.Windows.Forms.Label peripheryToInnerRange_lbl;
        private System.Windows.Forms.Label peripherytoinnerVar_lbl;
        private System.Windows.Forms.Label AvgBrightness_lbl;
        private System.Windows.Forms.Label Image_Metrics;
        public System.Windows.Forms.GroupBox uniformillumination_gbx;
        public System.Windows.Forms.CheckBox illuminationGrid_cbx;
        private System.Windows.Forms.Button SaveImage_IlluminationGrid_btn;
        private System.Windows.Forms.GroupBox ModeSelection_panel;
        private System.Windows.Forms.GroupBox CaptureNResume_gbx;
    }
}

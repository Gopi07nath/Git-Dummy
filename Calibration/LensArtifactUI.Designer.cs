namespace Calibration
{
    partial class LensArtifactUI
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
            this.whiteLightCapture_btn = new System.Windows.Forms.Button();
            this.grading_btn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.avgPeakVal_lbl = new System.Windows.Forms.Label();
            this.perPixelsAffVal_lbl = new System.Windows.Forms.Label();
            this.avgPeakAfterCorrVal_lbl = new System.Windows.Forms.Label();
            this.perPixelsAfterCorrVal_lbl = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LA_avgPeakRange_lbl = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.percent_lbl = new System.Windows.Forms.Label();
            this.resume_btn = new System.Windows.Forms.Button();
            this.whiteLightMode_rb = new System.Windows.Forms.RadioButton();
            this.IrMode_rb = new System.Windows.Forms.RadioButton();
            this.isLACod_cbx = new System.Windows.Forms.CheckBox();
            this.LaCord_gbx = new System.Windows.Forms.GroupBox();
            this.LA_BottomPanel = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.la_BottomX_nud = new System.Windows.Forms.NumericUpDown();
            this.la_BottomY_nud = new System.Windows.Forms.NumericUpDown();
            this.LA_TopPanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.la_TopY_nud = new System.Windows.Forms.NumericUpDown();
            this.la_TopX_nud = new System.Windows.Forms.NumericUpDown();
            this.LA_Bottom_rb = new System.Windows.Forms.RadioButton();
            this.La_Top_rb = new System.Windows.Forms.RadioButton();
            this.ModeSelection_gbx = new System.Windows.Forms.GroupBox();
            this.CaptureNResume_gbx = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.LaCord_gbx.SuspendLayout();
            this.LA_BottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.la_BottomX_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.la_BottomY_nud)).BeginInit();
            this.LA_TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.la_TopY_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.la_TopX_nud)).BeginInit();
            this.ModeSelection_gbx.SuspendLayout();
            this.CaptureNResume_gbx.SuspendLayout();
            this.SuspendLayout();
            // 
            // whiteLightCapture_btn
            // 
            this.whiteLightCapture_btn.ForeColor = System.Drawing.SystemColors.Desktop;
            this.whiteLightCapture_btn.Location = new System.Drawing.Point(6, 28);
            this.whiteLightCapture_btn.Name = "whiteLightCapture_btn";
            this.whiteLightCapture_btn.Size = new System.Drawing.Size(102, 44);
            this.whiteLightCapture_btn.TabIndex = 5;
            this.whiteLightCapture_btn.Text = "Capture";
            this.whiteLightCapture_btn.UseVisualStyleBackColor = true;
            this.whiteLightCapture_btn.Click += new System.EventHandler(this.whiteLightCapture_btn_Click);
            // 
            // grading_btn
            // 
            this.grading_btn.ForeColor = System.Drawing.SystemColors.Desktop;
            this.grading_btn.Location = new System.Drawing.Point(9, 78);
            this.grading_btn.Name = "grading_btn";
            this.grading_btn.Size = new System.Drawing.Size(202, 44);
            this.grading_btn.TabIndex = 50;
            this.grading_btn.Text = "Grading";
            this.grading_btn.UseVisualStyleBackColor = true;
            this.grading_btn.Visible = false;
            this.grading_btn.Click += new System.EventHandler(this.grading_btn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(6, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.label5.TabIndex = 51;
            this.label5.Text = "Average Peak  =";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(6, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 13);
            this.label1.TabIndex = 52;
            this.label1.Text = "Percentage Affected Pixels  =";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(3, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 13);
            this.label2.TabIndex = 54;
            this.label2.Text = "Percentage Pixels After Correction  =";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(3, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 13);
            this.label3.TabIndex = 53;
            this.label3.Text = "Avg Peak After Correction  =";
            // 
            // avgPeakVal_lbl
            // 
            this.avgPeakVal_lbl.AutoSize = true;
            this.avgPeakVal_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.avgPeakVal_lbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.avgPeakVal_lbl.Location = new System.Drawing.Point(106, 31);
            this.avgPeakVal_lbl.Name = "avgPeakVal_lbl";
            this.avgPeakVal_lbl.Size = new System.Drawing.Size(13, 13);
            this.avgPeakVal_lbl.TabIndex = 55;
            this.avgPeakVal_lbl.Text = "0";
            // 
            // perPixelsAffVal_lbl
            // 
            this.perPixelsAffVal_lbl.AutoSize = true;
            this.perPixelsAffVal_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.perPixelsAffVal_lbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.perPixelsAffVal_lbl.Location = new System.Drawing.Point(160, 77);
            this.perPixelsAffVal_lbl.Name = "perPixelsAffVal_lbl";
            this.perPixelsAffVal_lbl.Size = new System.Drawing.Size(13, 13);
            this.perPixelsAffVal_lbl.TabIndex = 56;
            this.perPixelsAffVal_lbl.Text = "0";
            // 
            // avgPeakAfterCorrVal_lbl
            // 
            this.avgPeakAfterCorrVal_lbl.AutoSize = true;
            this.avgPeakAfterCorrVal_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.avgPeakAfterCorrVal_lbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.avgPeakAfterCorrVal_lbl.Location = new System.Drawing.Point(163, 132);
            this.avgPeakAfterCorrVal_lbl.Name = "avgPeakAfterCorrVal_lbl";
            this.avgPeakAfterCorrVal_lbl.Size = new System.Drawing.Size(13, 13);
            this.avgPeakAfterCorrVal_lbl.TabIndex = 57;
            this.avgPeakAfterCorrVal_lbl.Text = "0";
            // 
            // perPixelsAfterCorrVal_lbl
            // 
            this.perPixelsAfterCorrVal_lbl.AutoSize = true;
            this.perPixelsAfterCorrVal_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.perPixelsAfterCorrVal_lbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.perPixelsAfterCorrVal_lbl.Location = new System.Drawing.Point(184, 160);
            this.perPixelsAfterCorrVal_lbl.Name = "perPixelsAfterCorrVal_lbl";
            this.perPixelsAfterCorrVal_lbl.Size = new System.Drawing.Size(13, 13);
            this.perPixelsAfterCorrVal_lbl.TabIndex = 58;
            this.perPixelsAfterCorrVal_lbl.Text = "0";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LA_avgPeakRange_lbl);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.percent_lbl);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.perPixelsAfterCorrVal_lbl);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.perPixelsAffVal_lbl);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.avgPeakVal_lbl);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.avgPeakAfterCorrVal_lbl);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.groupBox1.Location = new System.Drawing.Point(0, 223);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(237, 188);
            this.groupBox1.TabIndex = 59;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "LA Grading Metrics";
            // 
            // LA_avgPeakRange_lbl
            // 
            this.LA_avgPeakRange_lbl.AutoSize = true;
            this.LA_avgPeakRange_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LA_avgPeakRange_lbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LA_avgPeakRange_lbl.Location = new System.Drawing.Point(6, 54);
            this.LA_avgPeakRange_lbl.Name = "LA_avgPeakRange_lbl";
            this.LA_avgPeakRange_lbl.Size = new System.Drawing.Size(168, 13);
            this.LA_avgPeakRange_lbl.TabIndex = 63;
            this.LA_avgPeakRange_lbl.Text = "(Recommended Range < 14)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(217, 160);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 13);
            this.label7.TabIndex = 62;
            this.label7.Text = "%";
            // 
            // percent_lbl
            // 
            this.percent_lbl.AutoSize = true;
            this.percent_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.percent_lbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.percent_lbl.Location = new System.Drawing.Point(196, 77);
            this.percent_lbl.Name = "percent_lbl";
            this.percent_lbl.Size = new System.Drawing.Size(15, 13);
            this.percent_lbl.TabIndex = 61;
            this.percent_lbl.Text = "%";
            // 
            // resume_btn
            // 
            this.resume_btn.ForeColor = System.Drawing.SystemColors.Desktop;
            this.resume_btn.Location = new System.Drawing.Point(114, 28);
            this.resume_btn.Name = "resume_btn";
            this.resume_btn.Size = new System.Drawing.Size(106, 44);
            this.resume_btn.TabIndex = 60;
            this.resume_btn.Text = "Resume\r\n\r\n";
            this.resume_btn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.resume_btn.UseVisualStyleBackColor = true;
            this.resume_btn.Click += new System.EventHandler(this.resume_btn_Click);
            // 
            // whiteLightMode_rb
            // 
            this.whiteLightMode_rb.AutoSize = true;
            this.whiteLightMode_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.whiteLightMode_rb.ForeColor = System.Drawing.SystemColors.Desktop;
            this.whiteLightMode_rb.Location = new System.Drawing.Point(12, 54);
            this.whiteLightMode_rb.Name = "whiteLightMode_rb";
            this.whiteLightMode_rb.Size = new System.Drawing.Size(86, 19);
            this.whiteLightMode_rb.TabIndex = 81;
            this.whiteLightMode_rb.Text = "White Light";
            this.whiteLightMode_rb.UseVisualStyleBackColor = true;
            // 
            // IrMode_rb
            // 
            this.IrMode_rb.AutoSize = true;
            this.IrMode_rb.Checked = true;
            this.IrMode_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IrMode_rb.ForeColor = System.Drawing.SystemColors.Desktop;
            this.IrMode_rb.Location = new System.Drawing.Point(12, 28);
            this.IrMode_rb.Name = "IrMode_rb";
            this.IrMode_rb.Size = new System.Drawing.Size(67, 19);
            this.IrMode_rb.TabIndex = 80;
            this.IrMode_rb.TabStop = true;
            this.IrMode_rb.Text = "IR Light";
            this.IrMode_rb.UseVisualStyleBackColor = true;
            this.IrMode_rb.CheckedChanged += new System.EventHandler(this.IrMode_rb_CheckedChanged);
            // 
            // isLACod_cbx
            // 
            this.isLACod_cbx.AutoSize = true;
            this.isLACod_cbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.isLACod_cbx.ForeColor = System.Drawing.SystemColors.ControlText;
            this.isLACod_cbx.Location = new System.Drawing.Point(3, 420);
            this.isLACod_cbx.Name = "isLACod_cbx";
            this.isLACod_cbx.Size = new System.Drawing.Size(101, 17);
            this.isLACod_cbx.TabIndex = 90;
            this.isLACod_cbx.Text = "LA Co-ordinates";
            this.isLACod_cbx.UseVisualStyleBackColor = true;
            this.isLACod_cbx.CheckedChanged += new System.EventHandler(this.isLACod_cbx_CheckedChanged);
            // 
            // LaCord_gbx
            // 
            this.LaCord_gbx.Controls.Add(this.LA_BottomPanel);
            this.LaCord_gbx.Controls.Add(this.LA_TopPanel);
            this.LaCord_gbx.Controls.Add(this.LA_Bottom_rb);
            this.LaCord_gbx.Controls.Add(this.La_Top_rb);
            this.LaCord_gbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LaCord_gbx.ForeColor = System.Drawing.SystemColors.Highlight;
            this.LaCord_gbx.Location = new System.Drawing.Point(3, 442);
            this.LaCord_gbx.Name = "LaCord_gbx";
            this.LaCord_gbx.Size = new System.Drawing.Size(237, 164);
            this.LaCord_gbx.TabIndex = 91;
            this.LaCord_gbx.TabStop = false;
            this.LaCord_gbx.Text = "LA Co-ordinates";
            // 
            // LA_BottomPanel
            // 
            this.LA_BottomPanel.Controls.Add(this.label8);
            this.LA_BottomPanel.Controls.Add(this.label9);
            this.LA_BottomPanel.Controls.Add(this.la_BottomX_nud);
            this.LA_BottomPanel.Controls.Add(this.la_BottomY_nud);
            this.LA_BottomPanel.Location = new System.Drawing.Point(6, 99);
            this.LA_BottomPanel.Name = "LA_BottomPanel";
            this.LA_BottomPanel.Size = new System.Drawing.Size(202, 59);
            this.LA_BottomPanel.TabIndex = 92;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(11, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 96;
            this.label8.Text = "LA_Bottom X";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(11, 32);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 13);
            this.label9.TabIndex = 97;
            this.label9.Text = "LA_Bottom Y";
            // 
            // la_BottomX_nud
            // 
            this.la_BottomX_nud.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.la_BottomX_nud.Location = new System.Drawing.Point(122, 7);
            this.la_BottomX_nud.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.la_BottomX_nud.Name = "la_BottomX_nud";
            this.la_BottomX_nud.Size = new System.Drawing.Size(77, 20);
            this.la_BottomX_nud.TabIndex = 92;
            this.la_BottomX_nud.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.la_BottomX_nud.ValueChanged += new System.EventHandler(this.la_TopX_nud_ValueChanged_1);
            // 
            // la_BottomY_nud
            // 
            this.la_BottomY_nud.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.la_BottomY_nud.Location = new System.Drawing.Point(122, 30);
            this.la_BottomY_nud.Maximum = new decimal(new int[] {
            1536,
            0,
            0,
            0});
            this.la_BottomY_nud.Name = "la_BottomY_nud";
            this.la_BottomY_nud.Size = new System.Drawing.Size(77, 20);
            this.la_BottomY_nud.TabIndex = 93;
            this.la_BottomY_nud.Value = new decimal(new int[] {
            768,
            0,
            0,
            0});
            this.la_BottomY_nud.ValueChanged += new System.EventHandler(this.la_TopX_nud_ValueChanged_1);
            // 
            // LA_TopPanel
            // 
            this.LA_TopPanel.Controls.Add(this.label4);
            this.LA_TopPanel.Controls.Add(this.label6);
            this.LA_TopPanel.Controls.Add(this.la_TopY_nud);
            this.LA_TopPanel.Controls.Add(this.la_TopX_nud);
            this.LA_TopPanel.Location = new System.Drawing.Point(6, 40);
            this.LA_TopPanel.Name = "LA_TopPanel";
            this.LA_TopPanel.Size = new System.Drawing.Size(202, 56);
            this.LA_TopPanel.TabIndex = 92;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(11, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 94;
            this.label4.Text = "LA_Top X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(11, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 95;
            this.label6.Text = "LA_Top Y";
            // 
            // la_TopY_nud
            // 
            this.la_TopY_nud.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.la_TopY_nud.Location = new System.Drawing.Point(122, 33);
            this.la_TopY_nud.Maximum = new decimal(new int[] {
            1536,
            0,
            0,
            0});
            this.la_TopY_nud.Name = "la_TopY_nud";
            this.la_TopY_nud.Size = new System.Drawing.Size(76, 20);
            this.la_TopY_nud.TabIndex = 91;
            this.la_TopY_nud.Value = new decimal(new int[] {
            768,
            0,
            0,
            0});
            this.la_TopY_nud.ValueChanged += new System.EventHandler(this.la_TopX_nud_ValueChanged_1);
            // 
            // la_TopX_nud
            // 
            this.la_TopX_nud.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.la_TopX_nud.Location = new System.Drawing.Point(122, 7);
            this.la_TopX_nud.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.la_TopX_nud.Name = "la_TopX_nud";
            this.la_TopX_nud.Size = new System.Drawing.Size(77, 20);
            this.la_TopX_nud.TabIndex = 90;
            this.la_TopX_nud.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.la_TopX_nud.ValueChanged += new System.EventHandler(this.la_TopX_nud_ValueChanged_1);
            // 
            // LA_Bottom_rb
            // 
            this.LA_Bottom_rb.AutoSize = true;
            this.LA_Bottom_rb.Enabled = false;
            this.LA_Bottom_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LA_Bottom_rb.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LA_Bottom_rb.Location = new System.Drawing.Point(129, 20);
            this.LA_Bottom_rb.Name = "LA_Bottom_rb";
            this.LA_Bottom_rb.Size = new System.Drawing.Size(58, 17);
            this.LA_Bottom_rb.TabIndex = 93;
            this.LA_Bottom_rb.Text = "Bottom";
            this.LA_Bottom_rb.UseVisualStyleBackColor = true;
            this.LA_Bottom_rb.CheckedChanged += new System.EventHandler(this.LA_Bottom_rb_CheckedChanged);
            // 
            // La_Top_rb
            // 
            this.La_Top_rb.AutoSize = true;
            this.La_Top_rb.Enabled = false;
            this.La_Top_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.La_Top_rb.ForeColor = System.Drawing.SystemColors.ControlText;
            this.La_Top_rb.Location = new System.Drawing.Point(28, 20);
            this.La_Top_rb.Name = "La_Top_rb";
            this.La_Top_rb.Size = new System.Drawing.Size(44, 17);
            this.La_Top_rb.TabIndex = 92;
            this.La_Top_rb.Text = "Top";
            this.La_Top_rb.UseVisualStyleBackColor = true;
            this.La_Top_rb.CheckedChanged += new System.EventHandler(this.La_Top_rb_CheckedChanged);
            // 
            // ModeSelection_gbx
            // 
            this.ModeSelection_gbx.Controls.Add(this.whiteLightMode_rb);
            this.ModeSelection_gbx.Controls.Add(this.IrMode_rb);
            this.ModeSelection_gbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ModeSelection_gbx.ForeColor = System.Drawing.SystemColors.Highlight;
            this.ModeSelection_gbx.Location = new System.Drawing.Point(3, 6);
            this.ModeSelection_gbx.Name = "ModeSelection_gbx";
            this.ModeSelection_gbx.Size = new System.Drawing.Size(229, 77);
            this.ModeSelection_gbx.TabIndex = 92;
            this.ModeSelection_gbx.TabStop = false;
            this.ModeSelection_gbx.Text = "Live Mode Guidance";
            // 
            // CaptureNResume_gbx
            // 
            this.CaptureNResume_gbx.Controls.Add(this.whiteLightCapture_btn);
            this.CaptureNResume_gbx.Controls.Add(this.grading_btn);
            this.CaptureNResume_gbx.Controls.Add(this.resume_btn);
            this.CaptureNResume_gbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CaptureNResume_gbx.ForeColor = System.Drawing.SystemColors.Highlight;
            this.CaptureNResume_gbx.Location = new System.Drawing.Point(6, 89);
            this.CaptureNResume_gbx.Name = "CaptureNResume_gbx";
            this.CaptureNResume_gbx.Size = new System.Drawing.Size(226, 128);
            this.CaptureNResume_gbx.TabIndex = 93;
            this.CaptureNResume_gbx.TabStop = false;
            this.CaptureNResume_gbx.Text = "Image capture and LA Grading";
            // 
            // LensArtifactUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CaptureNResume_gbx);
            this.Controls.Add(this.ModeSelection_gbx);
            this.Controls.Add(this.LaCord_gbx);
            this.Controls.Add(this.isLACod_cbx);
            this.Controls.Add(this.groupBox1);
            this.Name = "LensArtifactUI";
            this.Size = new System.Drawing.Size(237, 609);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.LaCord_gbx.ResumeLayout(false);
            this.LaCord_gbx.PerformLayout();
            this.LA_BottomPanel.ResumeLayout(false);
            this.LA_BottomPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.la_BottomX_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.la_BottomY_nud)).EndInit();
            this.LA_TopPanel.ResumeLayout(false);
            this.LA_TopPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.la_TopY_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.la_TopX_nud)).EndInit();
            this.ModeSelection_gbx.ResumeLayout(false);
            this.ModeSelection_gbx.PerformLayout();
            this.CaptureNResume_gbx.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button whiteLightCapture_btn;
        private System.Windows.Forms.Button grading_btn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label avgPeakVal_lbl;
        private System.Windows.Forms.Label perPixelsAffVal_lbl;
        private System.Windows.Forms.Label avgPeakAfterCorrVal_lbl;
        private System.Windows.Forms.Label perPixelsAfterCorrVal_lbl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button resume_btn;
        private System.Windows.Forms.Label percent_lbl;
        private System.Windows.Forms.RadioButton whiteLightMode_rb;
        private System.Windows.Forms.RadioButton IrMode_rb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox isLACod_cbx;
        private System.Windows.Forms.GroupBox LaCord_gbx;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown la_BottomY_nud;
        private System.Windows.Forms.NumericUpDown la_BottomX_nud;
        private System.Windows.Forms.NumericUpDown la_TopY_nud;
        private System.Windows.Forms.NumericUpDown la_TopX_nud;
        private System.Windows.Forms.RadioButton La_Top_rb;
        private System.Windows.Forms.RadioButton LA_Bottom_rb;
        private System.Windows.Forms.Label LA_avgPeakRange_lbl;
        private System.Windows.Forms.Panel LA_TopPanel;
        private System.Windows.Forms.Panel LA_BottomPanel;
        private System.Windows.Forms.GroupBox ModeSelection_gbx;
        private System.Windows.Forms.GroupBox CaptureNResume_gbx;
    }
}

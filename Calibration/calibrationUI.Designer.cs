namespace Calibration
{
    partial class calibrationUI
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(calibrationUI));
            this.prev_btn = new System.Windows.Forms.Button();
            this.next_btn = new System.Windows.Forms.Button();
            this.commonControls_p = new System.Windows.Forms.Panel();
            this.connectControls_p = new System.Windows.Forms.Panel();
            this.connect_btn = new System.Windows.Forms.Button();
            this.saveReport_btn = new System.Windows.Forms.Button();
            this.browse_btn = new System.Windows.Forms.Button();
            this.resume_btn = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.nextPrev_p = new System.Windows.Forms.Panel();
            this.exit_btn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.displayArea1 = new Calibration.DisplayArea();
            this.Controls_p = new System.Windows.Forms.Panel();
            this.about_lnkLbl = new System.Windows.Forms.LinkLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.CameraID_Value_lab = new System.Windows.Forms.Label();
            this.cameraID_lab = new System.Windows.Forms.Label();
            this.StageName_lbl = new System.Windows.Forms.Label();
            this.commonControls_p.SuspendLayout();
            this.connectControls_p.SuspendLayout();
            this.nextPrev_p.SuspendLayout();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // prev_btn
            // 
            this.prev_btn.Location = new System.Drawing.Point(3, 0);
            this.prev_btn.Name = "prev_btn";
            this.prev_btn.Size = new System.Drawing.Size(75, 46);
            this.prev_btn.TabIndex = 1;
            this.prev_btn.Text = "<<";
            this.prev_btn.UseVisualStyleBackColor = true;
            this.prev_btn.Click += new System.EventHandler(this.prev_btn_Click);
            // 
            // next_btn
            // 
            this.next_btn.Location = new System.Drawing.Point(84, 0);
            this.next_btn.Name = "next_btn";
            this.next_btn.Size = new System.Drawing.Size(75, 46);
            this.next_btn.TabIndex = 2;
            this.next_btn.Text = ">>";
            this.next_btn.UseVisualStyleBackColor = true;
            this.next_btn.Click += new System.EventHandler(this.next_btn_Click);
            // 
            // commonControls_p
            // 
            this.commonControls_p.Controls.Add(this.connectControls_p);
            this.commonControls_p.Controls.Add(this.nextPrev_p);
            this.commonControls_p.Dock = System.Windows.Forms.DockStyle.Top;
            this.commonControls_p.Location = new System.Drawing.Point(0, 622);
            this.commonControls_p.Name = "commonControls_p";
            this.commonControls_p.Size = new System.Drawing.Size(1284, 47);
            this.commonControls_p.TabIndex = 3;
            this.commonControls_p.Visible = false;
            // 
            // connectControls_p
            // 
            this.connectControls_p.Controls.Add(this.connect_btn);
            this.connectControls_p.Controls.Add(this.saveReport_btn);
            this.connectControls_p.Controls.Add(this.browse_btn);
            this.connectControls_p.Controls.Add(this.resume_btn);
            this.connectControls_p.Controls.Add(this.panel3);
            this.connectControls_p.Dock = System.Windows.Forms.DockStyle.Left;
            this.connectControls_p.Location = new System.Drawing.Point(0, 0);
            this.connectControls_p.Name = "connectControls_p";
            this.connectControls_p.Size = new System.Drawing.Size(595, 47);
            this.connectControls_p.TabIndex = 0;
            this.connectControls_p.Visible = false;
            // 
            // connect_btn
            // 
            this.connect_btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.connect_btn.Location = new System.Drawing.Point(0, 0);
            this.connect_btn.Name = "connect_btn";
            this.connect_btn.Size = new System.Drawing.Size(75, 46);
            this.connect_btn.TabIndex = 95;
            this.connect_btn.Text = "Connect";
            this.connect_btn.UseVisualStyleBackColor = true;
            this.connect_btn.Click += new System.EventHandler(this.connect_btn_Click_1);
            // 
            // saveReport_btn
            // 
            this.saveReport_btn.Enabled = false;
            this.saveReport_btn.Location = new System.Drawing.Point(165, 1);
            this.saveReport_btn.Name = "saveReport_btn";
            this.saveReport_btn.Size = new System.Drawing.Size(75, 46);
            this.saveReport_btn.TabIndex = 93;
            this.saveReport_btn.Text = "Save-Report";
            this.saveReport_btn.UseVisualStyleBackColor = true;
            this.saveReport_btn.Click += new System.EventHandler(this.saveReport_btn_Click_1);
            // 
            // browse_btn
            // 
            this.browse_btn.Location = new System.Drawing.Point(84, 0);
            this.browse_btn.Name = "browse_btn";
            this.browse_btn.Size = new System.Drawing.Size(75, 46);
            this.browse_btn.TabIndex = 92;
            this.browse_btn.Text = "Browse";
            this.browse_btn.UseVisualStyleBackColor = true;
            this.browse_btn.Click += new System.EventHandler(this.browse_btn_Click_1);
            // 
            // resume_btn
            // 
            this.resume_btn.Enabled = false;
            this.resume_btn.Location = new System.Drawing.Point(255, 1);
            this.resume_btn.Name = "resume_btn";
            this.resume_btn.Size = new System.Drawing.Size(75, 46);
            this.resume_btn.TabIndex = 96;
            this.resume_btn.Text = "Resume";
            this.resume_btn.UseVisualStyleBackColor = true;
            this.resume_btn.Click += new System.EventHandler(this.resume_btn_Click);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(595, 47);
            this.panel3.TabIndex = 96;
            this.panel3.Visible = false;
            // 
            // nextPrev_p
            // 
            this.nextPrev_p.Controls.Add(this.next_btn);
            this.nextPrev_p.Controls.Add(this.prev_btn);
            this.nextPrev_p.Controls.Add(this.exit_btn);
            this.nextPrev_p.Dock = System.Windows.Forms.DockStyle.Right;
            this.nextPrev_p.Location = new System.Drawing.Point(949, 0);
            this.nextPrev_p.Name = "nextPrev_p";
            this.nextPrev_p.Size = new System.Drawing.Size(335, 47);
            this.nextPrev_p.TabIndex = 96;
            this.nextPrev_p.Visible = false;
            // 
            // exit_btn
            // 
            this.exit_btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.exit_btn.Location = new System.Drawing.Point(248, -1);
            this.exit_btn.Name = "exit_btn";
            this.exit_btn.Size = new System.Drawing.Size(75, 46);
            this.exit_btn.TabIndex = 94;
            this.exit_btn.Text = "Exit";
            this.exit_btn.UseVisualStyleBackColor = true;
            this.exit_btn.Click += new System.EventHandler(this.exit_btn_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1284, 600);
            this.panel1.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.displayArea1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.Controls_p);
            this.splitContainer1.Size = new System.Drawing.Size(1284, 600);
            this.splitContainer1.SplitterDistance = 1023;
            this.splitContainer1.TabIndex = 1;
            // 
            // displayArea1
            // 
            this.displayArea1.BackColor = System.Drawing.Color.Transparent;
            this.displayArea1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.displayArea1.Location = new System.Drawing.Point(0, 0);
            this.displayArea1.Name = "displayArea1";
            this.displayArea1.Size = new System.Drawing.Size(1023, 600);
            this.displayArea1.TabIndex = 4;
            // 
            // Controls_p
            // 
            this.Controls_p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls_p.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controls_p.Location = new System.Drawing.Point(0, 0);
            this.Controls_p.Name = "Controls_p";
            this.Controls_p.Size = new System.Drawing.Size(257, 600);
            this.Controls_p.TabIndex = 5;
            // 
            // about_lnkLbl
            // 
            this.about_lnkLbl.AutoSize = true;
            this.about_lnkLbl.Location = new System.Drawing.Point(1165, 6);
            this.about_lnkLbl.Name = "about_lnkLbl";
            this.about_lnkLbl.Size = new System.Drawing.Size(35, 13);
            this.about_lnkLbl.TabIndex = 6;
            this.about_lnkLbl.TabStop = true;
            this.about_lnkLbl.Text = "About";
            this.about_lnkLbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.CameraID_Value_lab);
            this.panel2.Controls.Add(this.cameraID_lab);
            this.panel2.Controls.Add(this.StageName_lbl);
            this.panel2.Controls.Add(this.about_lnkLbl);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1284, 22);
            this.panel2.TabIndex = 5;
            // 
            // CameraID_Value_lab
            // 
            this.CameraID_Value_lab.AutoSize = true;
            this.CameraID_Value_lab.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CameraID_Value_lab.Location = new System.Drawing.Point(886, 6);
            this.CameraID_Value_lab.Name = "CameraID_Value_lab";
            this.CameraID_Value_lab.Size = new System.Drawing.Size(20, 13);
            this.CameraID_Value_lab.TabIndex = 9;
            this.CameraID_Value_lab.Text = "ID";
            // 
            // cameraID_lab
            // 
            this.cameraID_lab.AutoSize = true;
            this.cameraID_lab.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cameraID_lab.Location = new System.Drawing.Point(815, 6);
            this.cameraID_lab.Name = "cameraID_lab";
            this.cameraID_lab.Size = new System.Drawing.Size(70, 13);
            this.cameraID_lab.TabIndex = 8;
            this.cameraID_lab.Text = "Camera ID:";
            this.cameraID_lab.Visible = false;
            // 
            // StageName_lbl
            // 
            this.StageName_lbl.AutoSize = true;
            this.StageName_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StageName_lbl.Location = new System.Drawing.Point(374, 6);
            this.StageName_lbl.Name = "StageName_lbl";
            this.StageName_lbl.Size = new System.Drawing.Size(0, 16);
            this.StageName_lbl.TabIndex = 7;
            // 
            // calibrationUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.exit_btn;
            this.ClientSize = new System.Drawing.Size(1284, 676);
            this.Controls.Add(this.commonControls_p);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1300, 715);
            this.MinimumSize = new System.Drawing.Size(1300, 715);
            this.Name = "calibrationUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " 3nethra Alignment and Calibration Tool";
            this.TransparencyKey = System.Drawing.Color.Red;
            this.Load += new System.EventHandler(this.calibrationUI_Load);
            this.Shown += new System.EventHandler(this.calibrationUI_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.calibrationUI_KeyDown);
            this.commonControls_p.ResumeLayout(false);
            this.connectControls_p.ResumeLayout(false);
            this.nextPrev_p.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button prev_btn;
        private System.Windows.Forms.Button next_btn;
        private System.Windows.Forms.Panel commonControls_p;
        private System.Windows.Forms.Button connect_btn;
        private System.Windows.Forms.Button exit_btn;
        private System.Windows.Forms.Button saveReport_btn;
        private System.Windows.Forms.Button browse_btn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel nextPrev_p;
        private System.Windows.Forms.Panel connectControls_p;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DisplayArea displayArea1;
        private System.Windows.Forms.Panel Controls_p;
        private System.Windows.Forms.LinkLabel about_lnkLbl;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label StageName_lbl;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button resume_btn;
        private System.Windows.Forms.Label cameraID_lab;
        private System.Windows.Forms.Label CameraID_Value_lab;

    }
}


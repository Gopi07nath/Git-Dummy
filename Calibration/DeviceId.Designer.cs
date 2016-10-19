namespace Calibration
{
    partial class DeviceId
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
            this.deviceIdP1_tbx = new System.Windows.Forms.TextBox();
            this.deviceIdP2_tbx = new System.Windows.Forms.TextBox();
            this.deviceIdP3_tbx = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // deviceIdP1_tbx
            // 
            this.deviceIdP1_tbx.BackColor = System.Drawing.Color.Silver;
            this.deviceIdP1_tbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deviceIdP1_tbx.Location = new System.Drawing.Point(92, 38);
            this.deviceIdP1_tbx.MaxLength = 4;
            this.deviceIdP1_tbx.Name = "deviceIdP1_tbx";
            this.deviceIdP1_tbx.Size = new System.Drawing.Size(54, 23);
            this.deviceIdP1_tbx.TabIndex = 0;
            this.deviceIdP1_tbx.Text = "3CXX";
            this.deviceIdP1_tbx.TextChanged += new System.EventHandler(this.deviceIdP1_tbx_TextChanged);
            // 
            // deviceIdP2_tbx
            // 
            this.deviceIdP2_tbx.BackColor = System.Drawing.Color.Silver;
            this.deviceIdP2_tbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deviceIdP2_tbx.Location = new System.Drawing.Point(162, 38);
            this.deviceIdP2_tbx.MaxLength = 4;
            this.deviceIdP2_tbx.Name = "deviceIdP2_tbx";
            this.deviceIdP2_tbx.Size = new System.Drawing.Size(54, 23);
            this.deviceIdP2_tbx.TabIndex = 1;
            this.deviceIdP2_tbx.Text = "XXXX";
            this.deviceIdP2_tbx.TextChanged += new System.EventHandler(this.deviceIdP2_tbx_TextChanged);
            // 
            // deviceIdP3_tbx
            // 
            this.deviceIdP3_tbx.BackColor = System.Drawing.Color.Silver;
            this.deviceIdP3_tbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deviceIdP3_tbx.Location = new System.Drawing.Point(232, 38);
            this.deviceIdP3_tbx.MaxLength = 4;
            this.deviceIdP3_tbx.Name = "deviceIdP3_tbx";
            this.deviceIdP3_tbx.Size = new System.Drawing.Size(52, 23);
            this.deviceIdP3_tbx.TabIndex = 2;
            this.deviceIdP3_tbx.Text = "XXXX";
            this.deviceIdP3_tbx.TextChanged += new System.EventHandler(this.deviceIdP3_tbx_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(151, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(219, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "-";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(14, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 17);
            this.label3.TabIndex = 98;
            this.label3.Text = "Device ID:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.deviceIdP1_tbx);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.deviceIdP2_tbx);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.deviceIdP3_tbx);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(350, 248);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(296, 100);
            this.panel1.TabIndex = 99;
            // 
            // DeviceId
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "DeviceId";
            this.Size = new System.Drawing.Size(1024, 600);
            this.Load += new System.EventHandler(this.DeviceId_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TextBox deviceIdP1_tbx;
        public System.Windows.Forms.TextBox deviceIdP2_tbx;
        public System.Windows.Forms.TextBox deviceIdP3_tbx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
    }
}

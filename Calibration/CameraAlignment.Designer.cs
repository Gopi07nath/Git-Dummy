namespace Calibration
{
    partial class CameraAlignment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraAlignment));
            this.whiteLightMode_rb = new System.Windows.Forms.RadioButton();
            this.IrMode_rb = new System.Windows.Forms.RadioButton();
            this.gridValues_pbx = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.eigth_tbx = new System.Windows.Forms.TextBox();
            this.sixth_tbx = new System.Windows.Forms.TextBox();
            this.seventh_tbx = new System.Windows.Forms.TextBox();
            this.fourth_tbx = new System.Windows.Forms.TextBox();
            this.fifth_tbx = new System.Windows.Forms.TextBox();
            this.third_tbx = new System.Windows.Forms.TextBox();
            this.second_tbx = new System.Windows.Forms.TextBox();
            this.first_tbx = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridValues_pbx)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // whiteLightMode_rb
            // 
            this.whiteLightMode_rb.AutoSize = true;
            this.whiteLightMode_rb.Location = new System.Drawing.Point(13, 73);
            this.whiteLightMode_rb.Name = "whiteLightMode_rb";
            this.whiteLightMode_rb.Size = new System.Drawing.Size(138, 17);
            this.whiteLightMode_rb.TabIndex = 81;
            this.whiteLightMode_rb.Text = "White Light Live  Mode ";
            this.whiteLightMode_rb.UseVisualStyleBackColor = true;
            // 
            // IrMode_rb
            // 
            this.IrMode_rb.AutoSize = true;
            this.IrMode_rb.Checked = true;
            this.IrMode_rb.Location = new System.Drawing.Point(13, 44);
            this.IrMode_rb.Name = "IrMode_rb";
            this.IrMode_rb.Size = new System.Drawing.Size(92, 17);
            this.IrMode_rb.TabIndex = 80;
            this.IrMode_rb.TabStop = true;
            this.IrMode_rb.Text = "IR Live Mode ";
            this.IrMode_rb.UseVisualStyleBackColor = true;
            this.IrMode_rb.CheckedChanged += new System.EventHandler(this.IrMode_rb_CheckedChanged);
            // 
            // gridValues_pbx
            // 
            this.gridValues_pbx.InitialImage = ((System.Drawing.Image)(resources.GetObject("gridValues_pbx.InitialImage")));
            this.gridValues_pbx.Location = new System.Drawing.Point(6, 20);
            this.gridValues_pbx.Name = "gridValues_pbx";
            this.gridValues_pbx.Size = new System.Drawing.Size(222, 170);
            this.gridValues_pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.gridValues_pbx.TabIndex = 82;
            this.gridValues_pbx.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.eigth_tbx);
            this.groupBox1.Controls.Add(this.sixth_tbx);
            this.groupBox1.Controls.Add(this.seventh_tbx);
            this.groupBox1.Controls.Add(this.fourth_tbx);
            this.groupBox1.Controls.Add(this.fifth_tbx);
            this.groupBox1.Controls.Add(this.third_tbx);
            this.groupBox1.Controls.Add(this.second_tbx);
            this.groupBox1.Controls.Add(this.first_tbx);
            this.groupBox1.Controls.Add(this.gridValues_pbx);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 165);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(234, 206);
            this.groupBox1.TabIndex = 83;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Grid Values";
            // 
            // eigth_tbx
            // 
            this.eigth_tbx.Location = new System.Drawing.Point(13, 63);
            this.eigth_tbx.MaxLength = 1;
            this.eigth_tbx.Name = "eigth_tbx";
            this.eigth_tbx.Size = new System.Drawing.Size(30, 21);
            this.eigth_tbx.TabIndex = 97;
            this.eigth_tbx.TabStop = false;
            this.eigth_tbx.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.first_tbx_PreviewKeyDown);
            this.eigth_tbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // sixth_tbx
            // 
            this.sixth_tbx.Location = new System.Drawing.Point(49, 151);
            this.sixth_tbx.MaxLength = 1;
            this.sixth_tbx.Name = "sixth_tbx";
            this.sixth_tbx.Size = new System.Drawing.Size(30, 21);
            this.sixth_tbx.TabIndex = 96;
            this.sixth_tbx.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.first_tbx_PreviewKeyDown);
            this.sixth_tbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // seventh_tbx
            // 
            this.seventh_tbx.Location = new System.Drawing.Point(13, 124);
            this.seventh_tbx.MaxLength = 1;
            this.seventh_tbx.Name = "seventh_tbx";
            this.seventh_tbx.Size = new System.Drawing.Size(30, 21);
            this.seventh_tbx.TabIndex = 95;
            this.seventh_tbx.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.first_tbx_PreviewKeyDown);
            this.seventh_tbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // fourth_tbx
            // 
            this.fourth_tbx.Location = new System.Drawing.Point(189, 124);
            this.fourth_tbx.MaxLength = 1;
            this.fourth_tbx.Name = "fourth_tbx";
            this.fourth_tbx.Size = new System.Drawing.Size(30, 21);
            this.fourth_tbx.TabIndex = 94;
            this.fourth_tbx.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.first_tbx_PreviewKeyDown);
            this.fourth_tbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // fifth_tbx
            // 
            this.fifth_tbx.Location = new System.Drawing.Point(151, 151);
            this.fifth_tbx.MaxLength = 1;
            this.fifth_tbx.Name = "fifth_tbx";
            this.fifth_tbx.Size = new System.Drawing.Size(30, 21);
            this.fifth_tbx.TabIndex = 93;
            this.fifth_tbx.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.first_tbx_PreviewKeyDown);
            this.fifth_tbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // third_tbx
            // 
            this.third_tbx.Location = new System.Drawing.Point(189, 63);
            this.third_tbx.MaxLength = 1;
            this.third_tbx.Name = "third_tbx";
            this.third_tbx.Size = new System.Drawing.Size(30, 21);
            this.third_tbx.TabIndex = 92;
            this.third_tbx.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.first_tbx_PreviewKeyDown);
            this.third_tbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // second_tbx
            // 
            this.second_tbx.Location = new System.Drawing.Point(151, 36);
            this.second_tbx.MaxLength = 1;
            this.second_tbx.Name = "second_tbx";
            this.second_tbx.Size = new System.Drawing.Size(30, 21);
            this.second_tbx.TabIndex = 91;
            this.second_tbx.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.first_tbx_PreviewKeyDown);
            this.second_tbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // first_tbx
            // 
            this.first_tbx.Location = new System.Drawing.Point(49, 36);
            this.first_tbx.MaxLength = 1;
            this.first_tbx.Name = "first_tbx";
            this.first_tbx.Size = new System.Drawing.Size(30, 21);
            this.first_tbx.TabIndex = 83;
            this.first_tbx.TabStop = false;
            this.first_tbx.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.first_tbx_PreviewKeyDown);
            this.first_tbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // CameraAlignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.whiteLightMode_rb);
            this.Controls.Add(this.IrMode_rb);
            this.Name = "CameraAlignment";
            this.Size = new System.Drawing.Size(237, 609);
            ((System.ComponentModel.ISupportInitialize)(this.gridValues_pbx)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton whiteLightMode_rb;
        private System.Windows.Forms.RadioButton IrMode_rb;
        private System.Windows.Forms.PictureBox gridValues_pbx;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox first_tbx;
        private System.Windows.Forms.TextBox sixth_tbx;
        private System.Windows.Forms.TextBox seventh_tbx;
        private System.Windows.Forms.TextBox fourth_tbx;
        private System.Windows.Forms.TextBox fifth_tbx;
        private System.Windows.Forms.TextBox third_tbx;
        private System.Windows.Forms.TextBox second_tbx;
        private System.Windows.Forms.TextBox eigth_tbx;
    }
}

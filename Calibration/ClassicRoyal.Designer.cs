namespace Calibration
{
    partial class ClassicRoyal
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
            this.Classic_rb = new System.Windows.Forms.RadioButton();
            this.Royal_rb = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Classic_rb
            // 
            this.Classic_rb.AutoSize = true;
            this.Classic_rb.Checked = true;
            this.Classic_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Classic_rb.Location = new System.Drawing.Point(35, 35);
            this.Classic_rb.Name = "Classic_rb";
            this.Classic_rb.Size = new System.Drawing.Size(82, 22);
            this.Classic_rb.TabIndex = 0;
            this.Classic_rb.TabStop = true;
            this.Classic_rb.Text = "Classic";
            this.Classic_rb.UseVisualStyleBackColor = true;
            this.Classic_rb.CheckedChanged += new System.EventHandler(this.Classic_rb_CheckedChanged);
            // 
            // Royal_rb
            // 
            this.Royal_rb.AutoSize = true;
            this.Royal_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Royal_rb.Location = new System.Drawing.Point(138, 35);
            this.Royal_rb.Name = "Royal_rb";
            this.Royal_rb.Size = new System.Drawing.Size(69, 22);
            this.Royal_rb.TabIndex = 1;
            this.Royal_rb.Text = "Royal";
            this.Royal_rb.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Royal_rb);
            this.panel1.Controls.Add(this.Classic_rb);
            this.panel1.Location = new System.Drawing.Point(409, 205);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(242, 100);
            this.panel1.TabIndex = 2;
            // 
            // ClassicRoyal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "ClassicRoyal";
            this.Size = new System.Drawing.Size(1059, 788);
            this.Load += new System.EventHandler(this.ClassicRoyal_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton Classic_rb;
        private System.Windows.Forms.RadioButton Royal_rb;
        private System.Windows.Forms.Panel panel1;

    }
}

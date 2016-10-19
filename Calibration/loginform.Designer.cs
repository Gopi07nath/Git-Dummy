namespace Calibration
{
    partial class loginform
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.exit_b = new System.Windows.Forms.Button();
            this.login_b = new System.Windows.Forms.Button();
            this.password_tb = new System.Windows.Forms.TextBox();
            this.username_tb = new System.Windows.Forms.TextBox();
            this.login_gbx = new System.Windows.Forms.GroupBox();
            this.invalidUNrPW_lbl = new System.Windows.Forms.Label();
            this.login_gbx.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(17, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(17, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "Username";
            // 
            // exit_b
            // 
            this.exit_b.BackColor = System.Drawing.SystemColors.Control;
            this.exit_b.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.exit_b.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exit_b.ForeColor = System.Drawing.SystemColors.ControlText;
            this.exit_b.Location = new System.Drawing.Point(127, 106);
            this.exit_b.Name = "exit_b";
            this.exit_b.Size = new System.Drawing.Size(83, 29);
            this.exit_b.TabIndex = 7;
            this.exit_b.Text = "Exit";
            this.exit_b.UseVisualStyleBackColor = false;
            this.exit_b.Click += new System.EventHandler(this.exit_b_Click);
            // 
            // login_b
            // 
            this.login_b.BackColor = System.Drawing.SystemColors.Control;
            this.login_b.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.login_b.ForeColor = System.Drawing.SystemColors.ControlText;
            this.login_b.Location = new System.Drawing.Point(20, 106);
            this.login_b.Name = "login_b";
            this.login_b.Size = new System.Drawing.Size(86, 29);
            this.login_b.TabIndex = 6;
            this.login_b.Text = "Login";
            this.login_b.UseVisualStyleBackColor = false;
            this.login_b.Click += new System.EventHandler(this.login_b_Click);
            // 
            // password_tb
            // 
            this.password_tb.BackColor = System.Drawing.SystemColors.Control;
            this.password_tb.ForeColor = System.Drawing.SystemColors.ControlText;
            this.password_tb.Location = new System.Drawing.Point(89, 62);
            this.password_tb.Name = "password_tb";
            this.password_tb.PasswordChar = '*';
            this.password_tb.Size = new System.Drawing.Size(100, 21);
            this.password_tb.TabIndex = 5;
            // 
            // username_tb
            // 
            this.username_tb.BackColor = System.Drawing.SystemColors.Control;
            this.username_tb.ForeColor = System.Drawing.SystemColors.ControlText;
            this.username_tb.Location = new System.Drawing.Point(89, 24);
            this.username_tb.Name = "username_tb";
            this.username_tb.Size = new System.Drawing.Size(100, 21);
            this.username_tb.TabIndex = 4;
            // 
            // login_gbx
            // 
            this.login_gbx.Controls.Add(this.invalidUNrPW_lbl);
            this.login_gbx.Controls.Add(this.label1);
            this.login_gbx.Controls.Add(this.label2);
            this.login_gbx.Controls.Add(this.username_tb);
            this.login_gbx.Controls.Add(this.password_tb);
            this.login_gbx.Controls.Add(this.exit_b);
            this.login_gbx.Controls.Add(this.login_b);
            this.login_gbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
            this.login_gbx.ForeColor = System.Drawing.SystemColors.Highlight;
            this.login_gbx.Location = new System.Drawing.Point(386, 166);
            this.login_gbx.Name = "login_gbx";
            this.login_gbx.Size = new System.Drawing.Size(244, 176);
            this.login_gbx.TabIndex = 10;
            this.login_gbx.TabStop = false;
            this.login_gbx.Text = "Login";
            // 
            // invalidUNrPW_lbl
            // 
            this.invalidUNrPW_lbl.AutoSize = true;
            this.invalidUNrPW_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.invalidUNrPW_lbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.invalidUNrPW_lbl.Location = new System.Drawing.Point(17, 147);
            this.invalidUNrPW_lbl.Name = "invalidUNrPW_lbl";
            this.invalidUNrPW_lbl.Size = new System.Drawing.Size(0, 13);
            this.invalidUNrPW_lbl.TabIndex = 10;
            // 
            // loginform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.login_gbx);
            this.Name = "loginform";
            this.Size = new System.Drawing.Size(1040, 750);
            this.login_gbx.ResumeLayout(false);
            this.login_gbx.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button exit_b;
        private System.Windows.Forms.Button login_b;
        private System.Windows.Forms.TextBox password_tb;
        private System.Windows.Forms.TextBox username_tb;
        private System.Windows.Forms.GroupBox login_gbx;
        private System.Windows.Forms.Label invalidUNrPW_lbl;
    }
}

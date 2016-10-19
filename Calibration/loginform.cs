using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Calibration
{ 
    public partial class loginform : UserControl
    {
        Facade f;
        Dictionary<string, string> UserNamePasswords;
        public loginform()
        {
            InitializeComponent();

            f = Facade.getInstance();
            this.login_gbx.Location = new Point((this.Width / 2) -50, (this.Height / 2)-100);
            username_tb.Text = UserName;
            password_tb.Text = PassWord;
            UserNamePasswords = new Dictionary<string, string>();

            SetUserNamePasswords();
        }
        string UserName = "service";
        string PassWord = "service";

       
        private void login_b_Click(object sender, EventArgs e)
        {
                           
            //if (string.Equals(username_tb.Text, UserName) && string.Equals(password_tb.Text, PassWord))
            //{
            //    invalidUNrPW_lbl.Text = "";
            //    f.Publish(f.Set_ClassicRoyalPage, new Args());
            //}
            if (validateUsernamePasswords())
                setApplicationFlow(username_tb.Text);
            else
                invalidUNrPW_lbl.Text = "Invalid Username or Password";
        }
        Args arg = new Args();
        private void setApplicationFlow(string UNPW)
        {
            switch (UNPW)
            {
                case "production":
                    {
                        arg["ApplicationMode"] = Globals.ApplicationFlow.production;
                        break;
                    }
                case "service":
                    {
                        arg["ApplicationMode"] = Globals.ApplicationFlow.service;
                        break;
                    }
                case "qa":
                    {
                        arg["ApplicationMode"] = Globals.ApplicationFlow.qa;
                        break;
                    }
                case "distributor":
                    {
                        arg["ApplicationMode"] = Globals.ApplicationFlow.distributor;
                        break;
                    }
                default:
                    break;
            }
            f.Publish(f.ApplicationMode, arg);
        }
        private void SetUserNamePasswords()
        {
            UserNamePasswords.Add("service", "service");
            UserNamePasswords.Add("production", "production");
            UserNamePasswords.Add("qa", "qa");
            UserNamePasswords.Add("distributor", "distributor");

        }

        private bool validateUsernamePasswords()
        {
            foreach (KeyValuePair<string, string> s in UserNamePasswords)
            {
                if (string.Equals(s.Key, username_tb.Text) && string.Equals(s.Value, password_tb.Text))
                    return true;
                else
                    continue;
            }
            return false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
                login_b_Click(null, null);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void exit_b_Click(object sender, EventArgs e)
        {
            
            f.Publish(f.Close_Camera, arg);
        }



    }
}

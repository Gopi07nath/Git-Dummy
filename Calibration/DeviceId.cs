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
    public partial class DeviceId : UserControl
    {
        Facade f;
        public string dID;
        public DeviceId()
        {
            InitializeComponent();
            f = Facade.getInstance();
            deviceIdP1_tbx.CharacterCasing = CharacterCasing.Upper;
            deviceIdP2_tbx.CharacterCasing = CharacterCasing.Upper;
            deviceIdP3_tbx.CharacterCasing = CharacterCasing.Upper;
            f.Subscribe(f.SetDeviceIDPage, new NotificationHandler(SetDeviceIdPage));
            dID = Globals.DeviceId;
            
        }
        private void SetDeviceIdPage(string s, Args n)
        {
            //If diD is null (Occurs when application started without connecting device and connected after DeviceID page.
            if (string.IsNullOrEmpty(dID))
                dID = Globals.DeviceId;

            //If the device is new one i.e not calibrated even once.
            if (Globals.DeviceId.Contains("3CXX") || Globals.DeviceId.Contains("3RXX"))
            {
                List<string> a = new List<string>();
                for (int i = 0; i < dID.Length; i += 4)
                {
                    if ((i + 4) < dID.Length)
                        a.Add(dID.Substring(i, 4));
                    else
                        a.Add(dID.Substring(i));
                }
                if (a.Count != 0)
                {
                    deviceIdP1_tbx.Text = a[0];
                    deviceIdP2_tbx.Text = a[1];
                    deviceIdP3_tbx.Text = a[2];
                }
                else
                {
                    if (!Globals.isRoyal)
                    {
                        deviceIdP1_tbx.Text = "3CXX";
                    }
                    else
                    {
                        deviceIdP1_tbx.Text = "3RXX";
                    }
                }
            }
            else
            {
                //If device is already calibrated then it will be updated with NVRAM value.
                if (Globals.DeviceId.Length == 12)   
                {
                    dID = Globals.DeviceId;
                    List<string> a = new List<string>();
                    for (int i = 0; i < dID.Length; i += 4)
                    {
                        if ((i + 4) < dID.Length)
                            a.Add(dID.Substring(i, 4));
                        else
                            a.Add(dID.Substring(i));
                    }
                    if (a.Count != 0)
                    {
                        deviceIdP1_tbx.Text = a[0];
                        deviceIdP2_tbx.Text = a[1];
                        deviceIdP3_tbx.Text = a[2];
                    }
                }
                else  //If deviceID written with any junk value then the default value is assigned.
                {
                    if (!Globals.isRoyal)
                    {
                        deviceIdP1_tbx.Text = "3CXX";
                    }
                    else
                    {
                        deviceIdP1_tbx.Text = "3RXX";
                    }
                    deviceIdP2_tbx.Text = "XXXX";
                    deviceIdP3_tbx.Text = "XXXX";
                }
            }

            Globals.DeviceId = deviceIdP1_tbx.Text + deviceIdP2_tbx.Text + deviceIdP3_tbx.Text;
            this.deviceIdP1_tbx.Focus();
            this.deviceIdP1_tbx.SelectAll();
        }
        private void DeviceId_Load(object sender, EventArgs e)
        {
            string id1 = "";
            string id2 = "";
            string id3 = "";
       
            //if (!Globals.isRoyal)
            //{
            //    deviceIdP1_tbx.Text = "3CXX";
            //}
            //else
            //{
            //    deviceIdP1_tbx.Text = "3RXX";
 
            //}
            //Globals.DeviceId = deviceIdP1_tbx.Text + deviceIdP2_tbx.Text + deviceIdP3_tbx.Text;
            this.deviceIdP1_tbx.Focus();
            this.deviceIdP1_tbx.SelectAll();
            //deviceIdP1_tbx.Text = id1;
            //deviceIdP2_tbx.Text = id2;
            //deviceIdP3_tbx.Text = id3;
        }
     
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void deviceIdP1_tbx_TextChanged(object sender, EventArgs e)
        {
           // Globals.CurrentNVRAM.DeviceID = deviceIdP1_tbx.Text + deviceIdP2_tbx.Text + deviceIdP3_tbx.Text;
            Globals.DeviceId = deviceIdP1_tbx.Text + deviceIdP2_tbx.Text + deviceIdP3_tbx.Text; 

        }

        private void deviceIdP2_tbx_TextChanged(object sender, EventArgs e)
        {
           // Globals.CurrentNVRAM.DeviceID = deviceIdP1_tbx.Text + deviceIdP2_tbx.Text + deviceIdP3_tbx.Text;
            Globals.DeviceId = deviceIdP1_tbx.Text + deviceIdP2_tbx.Text + deviceIdP3_tbx.Text; 

        }

        private void deviceIdP3_tbx_TextChanged(object sender, EventArgs e)
        {
          //  Globals.CurrentNVRAM.DeviceID = deviceIdP1_tbx.Text + deviceIdP2_tbx.Text + deviceIdP3_tbx.Text;
            Globals.DeviceId = deviceIdP1_tbx.Text + deviceIdP2_tbx.Text + deviceIdP3_tbx.Text; 

        }

        //Check DeviceID (each textBox) if it is empty when Next button is pressed
        public bool checkDeviceID()
        {
            if (string.IsNullOrEmpty(deviceIdP1_tbx.Text) || string.IsNullOrEmpty(deviceIdP2_tbx.Text) || string.IsNullOrEmpty(deviceIdP3_tbx.Text))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

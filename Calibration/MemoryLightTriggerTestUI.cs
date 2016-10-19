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
    public partial class MemoryLightTriggerTestUI : UserControl
    {
        Tricam tricam;
        Facade f;
        Bitmap cameraConnectedImage, cameraDisconnectedImage,TriggerImage,BlackLEDImage;
        string PassStr = "OK";
        string FailStr = "NOK";
        public MemoryLightTriggerTestUI()
        {
            InitializeComponent();
            tricam = Tricam.createTricam();
            f = Facade.getInstance();
            cameraConnectedImage = new Bitmap(@"Resources\GREENLED.png");
            cameraDisconnectedImage = new Bitmap(@"Resources\redLED.png");
            TriggerImage = new Bitmap(@"Resources\GOLDLED.png");
            BlackLEDImage = new Bitmap(@"Resources\BLACKLED.png");
            FlashLightLUXValue_tbx.Enabled = false;

            f.Subscribe(f.SET_LATERALITY, new NotificationHandler(SetLaterality));
            f.Subscribe(f.SET_Trigger, new NotificationHandler(SetTrigger));
            f.Subscribe(f.SetMemoryTestMode, new NotificationHandler(SetMemoryTestMode));
            f.Subscribe(f.ManageLightOnOff, new NotificationHandler(lightOnOff));
           
        }

        private void SetMemoryTestMode(string s, Args n)
        {
            if (Globals.isRoyal)
            {
                ring_cbx.Text = "Baloon Light";
                refractoRing_cbx.Visible = true;
                corneaWhiteRing_cbx.Visible = true;
                CorneaIr_cbx.Visible = true;
                proximitySensor_cbx.Visible = true;
            }
            else
            {
                ring_cbx.Text = "Cornea";
                refractoRing_cbx.Visible = false;
                corneaWhiteRing_cbx.Visible = false;
                CorneaIr_cbx.Visible = false;
                proximitySensor_cbx.Visible = false;
            }

            //If user selects any one light and click Next or Prev button then comes back to light test page, previous light should be ONed
            lightOnOff(null, null);
        }

        //Added because when user turns ON any light and then if user disconnects and connects the camera, it as to retain previous light status 
        private void lightOnOff(string s, Args n)
        {
            if (Globals.CameraInitialized)
            {
                if (flash_cbx.Checked)
                    flash_cbx_CheckedChanged(null, null);
                if (ir_cbx.Checked)
                    ir_cbx_CheckedChanged(null, null);
                if (ring_cbx.Checked)
                    ring_cbx_CheckedChanged(null, null);
                if (trigger_cbx.Checked)
                    trigger_cbx_CheckedChanged(null, null);
                if (leftRight_cbx.Checked)
                    leftRight_cbx_CheckedChanged(null, null);

            }
        }

        #region -----------Lights Checkboxes Events--------------
        private void flash_cbx_CheckedChanged(object sender, EventArgs e)
        {
            if (flash_cbx.Checked)
            {
                bool retVal = tricam.SetFundusFlash(flash_cbx.Checked);
                flash_cbx.Refresh();
                FlashLightLUXValue_tbx.Enabled = true;
                ir_cbx.Checked = false;
                ring_cbx.Checked = false;
            }
            else
            {
                bool retVal = tricam.SetFundusFlash(flash_cbx.Checked);
                flash_cbx.Refresh();
                FlashLightLUXValue_tbx.Enabled = false;
                DialogResult res = MessageBox.Show("Is Flash Light working ?", "CalibrationTool", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    Globals.currentSettings.memoryTestStruct.FlashLight = PassStr;
                }
                else
                {
                    Globals.currentSettings.memoryTestStruct.FlashLight = FailStr;
                }
            }
        }

        private void ir_cbx_CheckedChanged(object sender, EventArgs e)
        {
            if (ir_cbx.Checked)
            {
                tricam.SetIR(ir_cbx.Checked);
                ir_cbx.Refresh();
                flash_cbx.Checked = false;
                ring_cbx.Checked = false;
            }
            else
            {
                tricam.SetIR(ir_cbx.Checked);
                ir_cbx.Refresh();
                DialogResult res = MessageBox.Show("Is IR Light working ?", "CalibrationTool", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                    Globals.currentSettings.memoryTestStruct.IRLight = PassStr;
                else
                    Globals.currentSettings.memoryTestStruct.IRLight = FailStr;
            }
        }

        private void refractoRing_cbx_CheckedChanged(object sender, EventArgs e)
        {
            tricam.SetRefractoIRRing(refractoRing_cbx.Checked);
            refractoRing_cbx.Refresh();
            DialogResult res = MessageBox.Show("Is Refracto Ring working ?", "CalibrationTool", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
                Globals.currentSettings.memoryTestStruct.IRRing = PassStr;
            else
                Globals.currentSettings.memoryTestStruct.IRRing = FailStr;
        }

        private void corneaWhiteRing_cbx_CheckedChanged(object sender, EventArgs e)
        {
            tricam.SetCorneaWhiteRing(corneaWhiteRing_cbx.Checked);
            corneaWhiteRing_cbx.Refresh();
            DialogResult res = MessageBox.Show("Is Cornea White Light working ?", "CalibrationTool", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
                Globals.currentSettings.memoryTestStruct.WhiteLightRing = PassStr;
            else
                Globals.currentSettings.memoryTestStruct.WhiteLightRing = FailStr;
        }

        private void CorneaIr_cbx_CheckedChanged(object sender, EventArgs e)
        {
            tricam.SetCorneaIR(CorneaIr_cbx.Checked);
            CorneaIr_cbx.Refresh();
            DialogResult res = MessageBox.Show("Is Cornea IR Light working ?", "CalibrationTool", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
                Globals.currentSettings.memoryTestStruct.RefractoIR = PassStr;
            else
                Globals.currentSettings.memoryTestStruct.RefractoIR = FailStr;
        }

        private void proximitySensor_cbx_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void SetLaterality(string s, Args arg)
        {
            if (leftRight_cbx.Checked)
            {
                string str = arg["Laterality"].ToString();
                if (str == "L")
                {
                    leftRight_pbx.Image = cameraDisconnectedImage;
                }
                else
                {
                    leftRight_pbx.Image = cameraConnectedImage;
                }
                leftRight_pbx.Refresh();
            }
        }

        bool istrigger = false;
        private void SetTrigger(string s, Args arg)
        {
            if (trigger_cbx.Checked)
            {
                if ((int)arg["triggerValue"] == 0)
                {
                    trigger_pbx.Image = TriggerImage;
                }
                else
                {
                    trigger_pbx.Image = BlackLEDImage;
                }
                trigger_pbx.Refresh();
            }
        }
        #endregion

        #region----------Memory Test Events--------------
        private void cam_mem_test_Click(object sender, EventArgs e)
        {
            tricam.TestCameraMemory();
            tricam.TestPcbMemory1();
            tricam.TestPcbMemory2();
            if(tricam.isMemoryTest1Success && tricam.isMemoryTest2Success && tricam.isCameraMemoryTestSuccess)
                Globals.currentSettings.memoryTestStruct.MemoryTest = PassStr;
            else
            Globals.currentSettings.memoryTestStruct.MemoryTest = FailStr ;
       
        }

        #endregion

        private void ring_cbx_CheckedChanged(object sender, EventArgs e)
        {
            if (!Globals.isRoyal)
            {
                if (ring_cbx.Checked)
                {
                    tricam.SetCorneaWhiteRing(ring_cbx.Checked);
                    ring_cbx.Refresh();
                    flash_cbx.Checked = false;
                    ir_cbx.Checked = false;
                }
                else
                {
                    tricam.SetCorneaWhiteRing(ring_cbx.Checked);
                    ring_cbx.Refresh();
                    DialogResult res = MessageBox.Show("Is Cornea White Light working ?", "CalibrationTool", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                        Globals.currentSettings.memoryTestStruct.WhiteLightRing = PassStr;
                    else
                        Globals.currentSettings.memoryTestStruct.WhiteLightRing = FailStr;
                }
            }
            else
            {
                tricam.SetBaloonLight(ring_cbx.Checked);

                DialogResult res = MessageBox.Show("Is Baloon Light working ?", "CalibrationTool", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                    Globals.currentSettings.memoryTestStruct.BaloonLight = PassStr;
                else
                    Globals.currentSettings.memoryTestStruct.BaloonLight = FailStr;

            }
        }

        private void MemoryLightTriggerTestUI_Load(object sender, EventArgs e)
        {
            if (Globals.isRoyal)
            {
                ring_cbx.Text = "Baloon Light";
                refractoRing_cbx.Visible = true;
                corneaWhiteRing_cbx.Visible = true;
                CorneaIr_cbx.Visible = true;
                proximitySensor_cbx.Visible = true;
            }
            else
            {
                ring_cbx.Text = "Cornea";
                refractoRing_cbx.Visible = false;
                corneaWhiteRing_cbx.Visible = false;
                CorneaIr_cbx.Visible = false;
                proximitySensor_cbx.Visible = false;
            }
        }

        private void trigger_cbx_CheckedChanged(object sender, EventArgs e)
        {
            if (trigger_cbx.Checked)
            {
                tricam.stopLiveMode();
                tricam.trigger_LeftRightTimer.Start();
                if (ir_cbx.Checked || flash_cbx.Checked || ring_cbx.Checked)
                    ir_cbx.Checked = flash_cbx.Checked = ring_cbx.Checked = false;
            }
            else
            {
                DialogResult res = MessageBox.Show("Is Trigger Working ?", "CalibrationTool", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
                if (res == DialogResult.Yes)
                    Globals.currentSettings.memoryTestStruct.Trigger = PassStr;
                else
                    Globals.currentSettings.memoryTestStruct.Trigger = FailStr;
                if (leftRight_cbx.Checked == false)
                {
                    tricam.trigger_LeftRightTimer.Stop();
                    tricam.startLiveMode();
                }
            }
        }

        private void leftRight_cbx_CheckedChanged(object sender, EventArgs e)
        {
            if (leftRight_cbx.Checked)
            {
                tricam.stopLiveMode();
                tricam.trigger_LeftRightTimer.Start();
                if (ir_cbx.Checked || flash_cbx.Checked || ring_cbx.Checked)
                    ir_cbx.Checked = flash_cbx.Checked = ring_cbx.Checked = false;
            }
            else
            {
                DialogResult res = MessageBox.Show("Is Left Right Sensor Working ?", "CalibrationTool", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                    Globals.currentSettings.memoryTestStruct.LeftRight = PassStr;
                else
                    Globals.currentSettings.memoryTestStruct.LeftRight = FailStr;
                if (trigger_cbx.Checked == false)
                {
                    tricam.trigger_LeftRightTimer.Stop();
                    tricam.startLiveMode();
                }
            }
        }

        private void FlashLightLUXValue_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allows 0-9 and backspace
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 ))
            {
                e.Handled = true;
                return;
            }

            // checks to make sure only 1 decimal is allowed
            if (e.KeyChar == 46)
            {
                if ((sender as TextBox).Text.IndexOf(e.KeyChar) != -1)
                    e.Handled = true;
            }

        }

        private void FlashLightLUXValue_tbx_TextChanged(object sender, EventArgs e)
        {
            if (FlashLightLUXValue_tbx.Text != "")
            {
                Globals.currentSettings.memoryTestStruct.LUXvalue = int.Parse(FlashLightLUXValue_tbx.Text);
            }
            else
            {
                Globals.currentSettings.memoryTestStruct.LUXvalue = 0;
            }
        }

    }
}
 
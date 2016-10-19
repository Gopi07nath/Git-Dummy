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
    public partial class CameraAlignment : UserControl
    {
        Facade f;
        Tricam tricam;

        public CameraAlignment()
        {
            InitializeComponent();
            tricam = Tricam.createTricam();
            f = Facade.getInstance();
            gridValues_pbx.Image = Image.FromFile(@"Resources\CameraAlignment.png");
            f.Subscribe(f.CameraAlignmentMode, new NotificationHandler(SetCameraAlignmentMode));
            f.Subscribe(f.CameraAlignmentSaveReport, new NotificationHandler(saveReportCA));
        }

        private void saveReportCA(string s, Args arg)
        {
            Globals.currentSettings.cameraAlignment.firstValue = first_tbx.Text ;
            Globals.currentSettings.cameraAlignment.secondValue = second_tbx.Text;
            Globals.currentSettings.cameraAlignment.thirdValue = third_tbx.Text;
            Globals.currentSettings.cameraAlignment.fourthValue = fourth_tbx.Text;
            Globals.currentSettings.cameraAlignment.fifthValue = fifth_tbx.Text;
            Globals.currentSettings.cameraAlignment.sixthValue = sixth_tbx.Text;
            Globals.currentSettings.cameraAlignment.seventhValue = seventh_tbx.Text;
            Globals.currentSettings.cameraAlignment.eighthValue = eigth_tbx.Text;
        }

        private void SetCameraAlignmentMode(string s, Args arg)
        {
            IrMode_rb_CheckedChanged(null, null);
        }

        private void IrMode_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (IrMode_rb.Checked)
            {
                tricam.stopLiveMode();
                Globals.isHighResolution = !IrMode_rb.Checked;
                tricam.startLiveMode();
            }
            else
            {
                tricam.stopLiveMode();
                Globals.isHighResolution = !IrMode_rb.Checked;
                tricam.startLiveMode();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        public bool isTextEnteredInTB = false;
        private void first_tbx_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            isTextEnteredInTB = true;
        }

    }
}

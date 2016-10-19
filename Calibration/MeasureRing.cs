using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Calibration
{
    public partial class MeasureRing : Form
    {
        DirectoryInfo dirInf;

        public MeasureRing()
        {
            InitializeComponent();
        }

        private void Save_scrsht_btn_Click(object sender, EventArgs e)
        {
            Rectangle bounds = this.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }
                if (!Directory.Exists(Globals.calibrationPath))
                    Directory.CreateDirectory(Globals.calibrationPath);
                if (!Directory.Exists(Globals.calibrationPath + Globals.path + Path.DirectorySeparatorChar + Globals.DeviceId))
                    dirInf = Directory.CreateDirectory(Globals.calibrationPath + Path.DirectorySeparatorChar + Globals.DeviceId);
                else
                {
                    dirInf = new DirectoryInfo(Globals.calibrationPath + Path.DirectorySeparatorChar + Globals.DeviceId);
                }
                dirInf = new DirectoryInfo(dirInf.FullName + Path.DirectorySeparatorChar + DateTime.Now.ToString("dd-MM-yyyy"));
                if (!Directory.Exists(dirInf.FullName))
                    dirInf = Directory.CreateDirectory(dirInf.FullName);
                Globals.SaveImagePath = dirInf.FullName;
                string fileName = "";
                fileName = Globals.SaveImagePath + Path.DirectorySeparatorChar + Globals.DeviceId + "_" + DateTime.Now.ToString("HHmmss");
                bitmap.Save(fileName +"_Measure.png");
                MessageBox.Show("Ring Parameters saved", "CalibrationTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}

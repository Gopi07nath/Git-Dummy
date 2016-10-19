using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Forus.Refracto;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
namespace Calibration
{
    public partial class RefractoRingCalculation : UserControl
    {
        public event ExitMeasurement exitmeasurement;
        BackgroundWorker bw = new BackgroundWorker();
        public static Image<Bgr, byte> inputRing;
        Image<Bgr, byte> forDrawing;
        Image<Bgr, byte> overlay;
        bool firstcalc = true;
        Facade f;
        string str = "";
        string results = "";

        double circularityLowRange = 0;
        double circularityUpRange = 3;
        double centricityLowRange = 0;
        double centricityUpRange = 100;
        double sharpnessLowRange = 40;
        double sharpnessUpRange = 60;
        double thicknessLowRange = 35;
        double thicknessUpRange = 45;
        double intensityLowRange = 15;
        double intensityUpRange = 25;

        Ellipse ellipse;
        RefractoCalculations rc;

        int Angle = 0;
        double Radius = 0;
        double Thickness = 0;
        double Sharpness = 0;
        double Intensity = 0;
        public static bool calculateRing = false;
        Tricam tricam;
        public RefractoRingCalculation()
        {
            InitializeComponent();
            f = Facade.getInstance();
            tricam = Tricam.createTricam();
        }

        private void calc_btn_Click(object sender, EventArgs e)
        {
            tricam.stopLiveMode();
            DoRingCalculations();

        }
        private void DoRingCalculations()
        {
            this.Cursor = Cursors.WaitCursor;
            //Calculate ring and show
            rc = RefractoCalculations.GetInstance();
            inputRing = new Image<Bgr, byte>(Globals.whiteBmp);
            forDrawing = inputRing.Copy();
            ellipse = rc.CalculateRing(inputRing.Convert<Gray, byte>());
            forDrawing = rc.drawEllipse(forDrawing, ellipse, new Bgr(0, 0, 255), 1);
            overlay = forDrawing;
            rc.ComputeFullRingThickness();
            rc.ComputeRadii();
            Sharpness = rc.GetRingSharpness();
            showRingDetails();
            Args arg = new Args();
            Globals.whiteBmp = forDrawing.ToBitmap();
            forDrawing.Dispose();
            f.Publish(f.displayImage, arg);
            // = forDrawing.ToBitmap();
            this.Cursor = Cursors.Default;
        }

        private void showRingDetails()
        {
            string R1 = rc.R1.ToString();
            string R2 = rc.R2.ToString();
            string axis = rc.Axis.ToString();
            str = "R1 : " + R1 + "  R2 : " + R2 + "  Axis : " + axis;

            //centricity
            double dist = Math.Sqrt((rc.center.X - rc.input.Width / 2) * (rc.center.X - rc.input.Width / 2) +
                (rc.center.Y - rc.input.Height / 2) * (rc.center.Y - rc.input.Height / 2));

            //sharpness
            double sharpness = 0;
            double thickness = 0;
            double intensity = 0;
            if (firstcalc)
            {
                sharpness = rc.GetRingSharpness() / 100.0;
                thickness = rc.Ringthickness.Average();
                intensity = rc.GetRingIntensityLevel();
            }
            else
            {
                sharpness = rc.GetRingSharpness() / 100.0;
                thickness = rc.Ringthickness[Angle];
                intensity = rc.GetIntensityLevelAt(Angle);
            }
            //circularity
            circularity_lbl.Text = Math.Abs(rc.R1 - rc.R2).ToString();
            if (Math.Abs(rc.R1 - rc.R2) < circularityUpRange && Math.Abs(rc.R1 - rc.R2) >= circularityLowRange)
            {
                circularity_lbl.ForeColor = Color.Green;
            }
            else
            {
                circularity_lbl.ForeColor = Color.Red;
            }

            centricity_lbl.Text = dist.ToString();
            if (Math.Abs(dist) < centricityUpRange && Math.Abs(dist) > centricityLowRange)
            {
                centricity_lbl.ForeColor = Color.Green;
            }
            else
            {
                centricity_lbl.ForeColor = Color.Red;
            }

            sharpness_lbl.Text = sharpness.ToString();
            if (sharpness < sharpnessUpRange && sharpness > sharpnessLowRange)
            {
                sharpness_lbl.ForeColor = Color.Green;
            }
            else
            {
                sharpness_lbl.ForeColor = Color.Red;
            }

            intensity_lbl.Text = intensity.ToString();
            if (intensity < intensityUpRange && intensity > intensityLowRange)
            {
                intensity_lbl.ForeColor = Color.Green;
            }
            else
            {
                intensity_lbl.ForeColor = Color.Red;
            }

            thickness_lbl.Text = thickness.ToString();
            if (thickness < thicknessUpRange && thickness > thicknessLowRange)
            {
                thickness_lbl.ForeColor = Color.Green;
            }
            else
            {
                thickness_lbl.ForeColor = Color.Red;
            }
        }

    }
}

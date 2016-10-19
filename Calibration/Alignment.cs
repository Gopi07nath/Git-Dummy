using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.Util;
using System.IO;
using Emgu.CV.UI;
namespace Calibration
{
    class Alignment 
    {
        [DllImport("imageMetricsCalculation.dll", EntryPoint = "ImageMetrics_Init")]
        public extern static void ImageMetrics_Init();
        [DllImport("imageMetricsCalculation.dll", EntryPoint = "ImageMetrics_Exit")]
        public extern static void ImageMetrics_Exit();
        [DllImport("imageMetricsCalculation.dll", EntryPoint = "ImageMetrics_Calculate")]
        unsafe public extern static void ImageMetrics_Calculate(IntPtr srcImg, ref ImageMetricsInfo imageMetrics, int[] data, bool isSectorCalc);

        ImageMetricsInfo imageMetrics;
        Facade f;
        Tricam tricam;
       
        public struct ImageMetricsInfo
        {
            public int AverageIntensity;
            public double peripheryToInnerVariation;
            public double topToBottomVariation;

        }

        public Alignment()
        {
            tricam = Tricam.createTricam();
            f = Facade.getInstance();
            f.Subscribe(f.Alignment_Image_Metrics, new NotificationHandler(CalculateImageMetrics));
            f.Subscribe(f.ApplyMask, new NotificationHandler(ApplyMask));
            ImageMetrics_Init();
        }

        Image<Bgr, byte> im;
        public void ApplyMask(string n ,Args arg)
        {
            if ((bool)arg["isIR"])
            {
                //im = new Image<Bgr, byte>(Globals.IrBmp);
                im = new Image<Bgr, byte>(Globals.Bmp);
                //im.Save(fileName + "_Frame.png");
                try
                {
                    //im._And(new Image<Bgr, byte>(Globals.mask1mp));
                    //Bitmap mask3MP = new Bitmap(Globals.mask3mp);
                    //mask3MP.Save(fileName+ "_mask.png");
                    im._And(new Image<Bgr, byte>(Globals.mask3mp));
                    //im.Save(fileName + "_ImageNmask.png");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                //Globals.IrBmp = im.ToBitmap();
                Globals.Bmp = im.ToBitmap();
                im.Dispose();
            }
            else
            {
                im = new Image<Bgr, byte>(Globals.whiteBmp);
                im._And(new Image<Bgr, byte>(Globals.mask3mp));
                Globals.whiteBmp = im.ToBitmap();
                im.Dispose();
            }
           
        }

        int frameCnt = 0;
        public void rowProfile(Bitmap bm)
        {
            Image<Bgr, byte> im = new Image<Bgr, byte>(bm);

            // To initialize data array
            imageMetrics = new ImageMetricsInfo();
            int[] data = { 0,0,0,0,0,0,0,0};

            ImageMetrics_Calculate(im.Ptr, ref imageMetrics, data, Globals.isIlluminationGrid  );
            im.Dispose();

            double cv = Math.Round(imageMetrics.topToBottomVariation + 0.005, 2);
            double peripheryToInnerImageRatio = imageMetrics.peripheryToInnerVariation + 0.005;
            peripheryToInnerImageRatio = (Math.Round(peripheryToInnerImageRatio, 2));
            //if (bm.Width == Globals.IRWidth)
            if (!Globals.isHighResolution)
            {
                Globals.currentSettings.imageMetrics.Periphery_To_Inner_IR_Light_Intensity_Variation = peripheryToInnerImageRatio;
                Globals.currentSettings.imageMetrics.Top_To_Bottom_IR_Light_Intensity_Variation = cv;

                if (peripheryToInnerImageRatio >= 0.81)
                    Globals.currentSettings.imageMetrics.peripheryToInnerIntensityVarLabelColor_IR = Color.Green;
                else
                    Globals.currentSettings.imageMetrics.peripheryToInnerIntensityVarLabelColor_IR = Color.FromArgb(254, 0, 0);

                if (cv <= 0.25)
                    Globals.currentSettings.imageMetrics.topToBottomIntensityVarLabelColor_IR = Color.Green;
                else
                    Globals.currentSettings.imageMetrics.topToBottomIntensityVarLabelColor_IR = Color.FromArgb(254, 0, 0);
            }
            else
            {
                Globals.currentSettings.imageMetrics.Periphery_To_Inner_White_Light_Intensity_Variation = peripheryToInnerImageRatio;
                Globals.currentSettings.imageMetrics.Top_To_Bottom_White_Light_Intensity_Variation = cv;

                if (peripheryToInnerImageRatio >= 0.81)
                    Globals.currentSettings.imageMetrics.peripheryToInnerIntensityVarLabelColor_WL = Color.Green;
                else
                    Globals.currentSettings.imageMetrics.peripheryToInnerIntensityVarLabelColor_WL = Color.FromArgb(254, 0, 0);

                if (cv <= 0.25)
                    Globals.currentSettings.imageMetrics.topToBottomIntensityVarLabelColor_WL = Color.Green;
                else
                    Globals.currentSettings.imageMetrics.topToBottomIntensityVarLabelColor_WL = Color.FromArgb(254, 0, 0);

            }

            //Update sector intensities values (8 sectors) from image metrics to Globals 
            if (Globals.isIlluminationGrid)
            {
                Globals.currentSettings.imageMetrics.firstValue = data[0];
                Globals.currentSettings.imageMetrics.secondValue = data[1];
                Globals.currentSettings.imageMetrics.thirdValue = data[2];
                Globals.currentSettings.imageMetrics.fourthValue = data[3];
                Globals.currentSettings.imageMetrics.fifthValue = data[4];
                Globals.currentSettings.imageMetrics.sixthValue = data[5];
                Globals.currentSettings.imageMetrics.seventhValue = data[6];
                Globals.currentSettings.imageMetrics.eighthValue = data[7];
            }
            
        }

        private void CalculateImageMetrics(string s, Args arg)
        {
            if (Globals.whiteBmp == null && Globals.IrBmp==null)
                return;

            if (!Globals.isHighResolution)
            {
                if (tricam.isLiveMode())
                    rowProfile(Globals.IrBmp);  // IR 1Mp
                else
                    rowProfile(Globals.Bmp);  // IR 3MP
            }
            else
                rowProfile(Globals.whiteBmp);

            CalculateAvgBrightness(!Globals.isHighResolution);
 
        }

        public void CalculateMetrics()
        {
            if (Globals.whiteBmp == null && Globals.IrBmp == null)
                return;

            if (!Globals.isHighResolution)
                rowProfile(Globals.IrBmp);
            else
                rowProfile(Globals.whiteBmp);

            CalculateAvgBrightness(!Globals.isHighResolution);
        }

        public void CalculateAvgBrightness(bool isIR)
        {
            Tricam tricam = Tricam.createTricam();
            if (isIR)
            {
                Globals.currentSettings.imageMetrics.Average_IR_Light_Intensity = imageMetrics.AverageIntensity;
                if (Globals.currentSettings.imageMetrics.Average_IR_Light_Intensity >= 70 && Globals.currentSettings.imageMetrics.Average_IR_Light_Intensity <= 73)
                    Globals.currentSettings.imageMetrics.averageIntensityLabelColor_IR = Color.Green;
                else
                    Globals.currentSettings.imageMetrics.averageIntensityLabelColor_IR = Color.FromArgb(254, 0, 0);
            }
            else
            {
                Globals.currentSettings.imageMetrics.Average_White_Light_Intensity = imageMetrics.AverageIntensity;
                if (tricam.isLiveMode())
                {
                    if (Globals.currentSettings.imageMetrics.Average_White_Light_Intensity >= 190 && Globals.currentSettings.imageMetrics.Average_White_Light_Intensity <= 200)
                        Globals.currentSettings.imageMetrics.averageIntensityLabelColor_WL = Color.Green;
                    else
                        Globals.currentSettings.imageMetrics.averageIntensityLabelColor_WL = Color.FromArgb(254, 0, 0);
                       
                }
                else
                {
                    if (Globals.currentSettings.imageMetrics.Average_White_Light_Intensity >= 220 && Globals.currentSettings.imageMetrics.Average_White_Light_Intensity <= 230)
                        Globals.currentSettings.imageMetrics.averageIntensityLabelColor_WL = Color.Green;
                    else
                        Globals.currentSettings.imageMetrics.averageIntensityLabelColor_WL = Color.FromArgb(254, 0, 0);
                        
                }
            }

        }

    }
}

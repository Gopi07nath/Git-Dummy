using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.Util;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV;
using System.IO;
using Nvram;

namespace Calibration
{
    public partial class LensArtifactUI : UserControl
    {
        Facade f;
        Tricam tricam;
        Args args;
        Image<Gray, double> gauss;// gaussian distribution image.
        Image<Gray, double> gauss_bottom;
        Image<Gray, byte> greenTopZone;// Green channel Mask image
        Image<Gray, byte> redTopZone;//Red channel Mask image
        Image<Gray, byte> blueTopZone;//Blue channel Mask image 
        double redScale = 0.7;
        double greenScale = 0.7;
        double blueScale = 0.7;
        #region*********/////-----constants used-----/////***********
        const int ImageHeight = 1536;
        const int ImageWidth = 2048;
        const int GreenChannelOuterEdgeBlurSize = 51;
        const int BlueChannelOuterEdgeBlurSize = 51;
        const int redChannelOuterEdgeBlurSize = 9;
        const int ChannelOuterEdgeErodeSize = 3;
        const double zoneThresholdSize = 1;
        const double zoneThresholdMaxSize = 255;
        const int filter_size_big = 105;//odd value for the High pass filter value varies between 91 to 151
        const int filter_size_small = 7;//odd value for the Low pass filter value varies between 5 to 31
        const double val = 3;
      
        #endregion

        public LensArtifactUI()
        {
            InitializeComponent();
            f = Facade.getInstance();
            f.Subscribe(f.GradingLive, new NotificationHandler(gradingLive));
            f.Subscribe(f.GradingStill, new NotificationHandler(gradingStill));
            f.Subscribe(f.LASaveReport, new NotificationHandler(saveReportLA));
            f.Subscribe(f.LensArtifactMeasurementMode, new NotificationHandler(LensArtifactMeasurementMode));
            f.Subscribe(f.LAchangeCoordinates, new NotificationHandler(LAchangeCoordinates));
            f.Subscribe("ManageControlsAfterSaveReport", new NotificationHandler(manageControlsAfterSaveReport));
            
            tricam = Tricam.createTricam();
            LaCord_gbx.Enabled = false;
            
           
        }

        private void manageControlsAfterSaveReport(string s, Args n)
        {
            if (!(bool)n["FundusPage"])
            {
                //if (Globals.browseBtnClicked)
                //{
                //    ModeSelection_gbx.Enabled = true;
                //    whiteLightCapture_btn.Visible = true;
                //    //whiteLightCapture_btn.Enabled = true;
                //    grading_btn.Visible = false;
                //    resume_btn.Enabled = false;
                //}
                //else
                //{
                    ModeSelection_gbx.Enabled = false;
                    whiteLightCapture_btn.Visible = false;
                    //whiteLightCapture_btn.Enabled = true;
                    //grading_btn.Visible = true;
                    resume_btn.Enabled = true;
                //}

                if (IrMode_rb.Checked)
                    Globals.isHighResolution = false;
                //resume_btn_Click(null, null);
            }
        
        }

        private void LAchangeCoordinates(string s, Args arg)
        {
            if (LA_Bottom_rb.Checked)
            {
                la_BottomX_nud.Value = (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.ClassicSettings.LensArtifactBottomX);
                la_BottomY_nud.Value = (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.ClassicSettings.LensArtifactBottomY);
            }
            else
            {
                la_TopX_nud.Value= (int) Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.ClassicSettings.LensArtifactTopX);
                la_TopY_nud.Value= (int) Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.ClassicSettings.LensArtifactTopY);
            }           
        }

        private void LensArtifactMeasurementMode(string s, Args arg)
        {
            Globals.isHighResolution = false;
            IrMode_rb.Checked = true;

            Globals.digitalGain = 50;
            tricam.setDigitalGain();
            tricam.startLiveMode();
            resume_btn.Enabled = false;
            isLACod_cbx.Checked = false;
            grading_btn.Visible = false;
            whiteLightCapture_btn.Visible = true;
            ModeSelection_gbx.Enabled = true;
            //IrMode_rb_CheckedChanged(null, null);
            if (Globals.browseBtnClicked)
            {
                tricam.stopLiveMode();
                whiteLightMode_rb.Checked = true;
                f.Publish(f.GradingStill, null);
                whiteLightMode_rb.Checked = false;
                grading_btn.Visible = true;
                whiteLightCapture_btn.Visible = false;
                resume_btn.Enabled = true;
                ModeSelection_gbx.Enabled = false;
            }
            //else if (IrMode_rb.Checked)
            //{
            //    Globals.isHighResolution = false;
            //    IrMode_rb.Checked = true;
            //    //IRGradingCapture();
            //}
            else if (whiteLightMode_rb.Checked)
            {
                whiteGradingCapture();
            }

            //Recommanded range for Avg peak is only for white light, so disabling when IR selected
            if (tricam.isLiveMode())
            {
                if (IrMode_rb.Checked)
                {
                    if (LA_avgPeakRange_lbl.Visible)
                    {
                        LA_avgPeakRange_lbl.Visible = false;
                        LA_avgPeakRange_lbl.Refresh();
                    }
                }
                else
                {
                    if (LA_avgPeakRange_lbl.Visible)
                    {
                        LA_avgPeakRange_lbl.Visible = true;
                        LA_avgPeakRange_lbl.Refresh();
                    }
                }
            }
            
        }

        private void saveReportLA(string s, Args arg)
        {
            IRGradingCapture();
            whiteGradingCapture();
            //Added because when SaveReport button is clicked, application was directly going to white light mode
            if(IrMode_rb.Checked)
                Globals.isHighResolution = false;
            //If LA coordinates check box is checked and if user clicks on save report button then both LA coordinates and resume buttons will be disabled.
            if (isLACod_cbx.Checked)
                isLACod_cbx.Checked = false;
            //if (resume_btn.Enabled)
            //    resume_btn.Enabled = false;
        }

        private void whiteGradingCapture()
        {
            Globals.isHighResolution = true;
            tricam.WhiteLightCapture();
            Args args = new Args();
            args["isIR"] = false;
            f.Publish(f.ApplyMask, args);
            f.Publish(f.Save_Image, args);
            Globals.browseBtnClicked = false;
            f.Publish(f.GradingStill, new Args());
        }

        private void IRGradingCapture()
        {
            Globals.isHighResolution = false;
            tricam.IRCapture();
            Args args = new Args();
            args["isIR"] = true;
            f.Publish(f.ApplyMask, args);
            f.Publish(f.Save_Image, args);
            Globals.browseBtnClicked = false;
            f.Publish(f.GradingStill, new Args());
        }

        public void whiteLightCapture_btn_Click(object sender, EventArgs e)
        {
            resume_btn.Enabled = true;
            if (tricam.isLiveMode())
            {
                if (IrMode_rb.Checked)
                    IRGradingCapture();
                else
                    whiteGradingCapture();
            }
            ModeSelection_gbx.Enabled = false;
        }
        
        private void grading_btn_Click(object sender, EventArgs e)
        {
                f.Publish(f.GradingStill, new Args());
        }

        Rectangle rect;
        private void gradingLive(string s, Args arg)
        {
            if (Globals.whiteBmp == null && Globals.IrBmp == null)
                return;
            f.Publish(f.SetAoiLensArtifactMeasurement, new Args());
            if (Globals.isHighResolution)
                rect = Globals.WLRoiRect;
            else
                rect = Globals.IRroiRect;

                int[] nonZeroCount = Globals.ThreshImg.CountNonzero();
                Image<Gray, byte> temp = new Image<Gray, byte>(Globals.ChannelImg.Width, Globals.ChannelImg.Height);
                CvInvoke.cvCopy(Globals.ChannelImg, temp, Globals.ThreshImg);
                float channelImgSum = (float)temp.GetSum().Intensity;
                float redArtifact = 0f;
                if (nonZeroCount[0] != 0 && channelImgSum != 0)
                {
                    redArtifact = channelImgSum / nonZeroCount[0];
                    mathError = false;
                }
                else
                    mathError = true;

            double val = 0;
                if (IrMode_rb.Checked)
                {
                    Globals.currentSettings.laMetrics.Average_Peak_Before_Correction_IR = Math.Round(redArtifact, 2);
                    val = Globals.currentSettings.laMetrics.Average_Peak_Before_Correction_IR;
                }
                else
                {
                    Globals.currentSettings.laMetrics.Average_Peak_Before_Correction_WL = Math.Round(redArtifact, 2);
                    val = Globals.currentSettings.laMetrics.Average_Peak_Before_Correction_WL;
                }
                if (mathError)
                {
                    avgPeakVal_lbl.ForeColor = Color.Green;
                    avgPeakVal_lbl.Text = "< 9";
                    avgPeakVal_lbl.Refresh();
                }
                else
                {
                    if (val > 14)
                    {
                        avgPeakVal_lbl.ForeColor = Color.FromArgb(254, 0, 0);
                        Globals.currentSettings.laMetrics.Average_Peak_Before_Correction_Color_WL = Color.FromArgb(254, 0, 0);
                    }
                    else
                    {
                        avgPeakVal_lbl.ForeColor = Color.Green;
                        Globals.currentSettings.laMetrics.Average_Peak_Before_Correction_Color_WL = Color.Green;

                    }
                    avgPeakVal_lbl.Text = val.ToString();
                    avgPeakVal_lbl.Refresh();
                }
                nonZeroCount = Globals.RedChannelImg.CountNonzero();
                redArtifact = 0f;
                float divider = ((float)rect.Width * rect.Height);
                if (nonZeroCount[0] != 0 && divider !=0)
                redArtifact = (float)(nonZeroCount[0] * 100) /divider ;
                if (IrMode_rb.Checked)
                {
                    Globals.currentSettings.laMetrics.Percentage_Affected_Pixels_Before_Correction_IR = Math.Round(redArtifact, 2);
                    val = Globals.currentSettings.laMetrics.Percentage_Affected_Pixels_Before_Correction_IR;
                }
                else
                {
                    Globals.currentSettings.laMetrics.Percentage_Affected_Pixels_Before_Correction_WL = Math.Round(redArtifact, 2);
                    val = Globals.currentSettings.laMetrics.Percentage_Affected_Pixels_Before_Correction_WL;
                }
            if (val < 1)
                {
                    Globals.currentSettings.laMetrics.Percentage_Affected_Pixels_Before_Correction_Color_WL = Color.Green;
                    perPixelsAffVal_lbl.ForeColor = Color.Green;
                    percent_lbl.ForeColor = Color.Green;
                    
                }
                else
                {
                    Globals.currentSettings.laMetrics.Percentage_Affected_Pixels_Before_Correction_Color_WL = Color.FromArgb(254, 0, 0);
                    perPixelsAffVal_lbl.ForeColor = Color.FromArgb(254, 0, 0);
                    percent_lbl.ForeColor = Color.FromArgb(254, 0, 0);

                }
                perPixelsAffVal_lbl.Text = val.ToString();
                perPixelsAffVal_lbl.Refresh();

        }

        bool mathError = false;
        private void gradingStill(string s ,Args arg)
        {
            Rectangle rect1 = new Rectangle();

            if (Globals.whiteBmp == null && Globals.IrBmp == null)
                return;

             f.Publish(f.SetAoiLensArtifactMeasurement, new Args());
             if (Globals.isHighResolution)
             {
                 if (isLACod_cbx.Checked)
                     rect = Globals.WLRoiRectLAcoordinates;
                 else
                     rect = Globals.WLRoiRect;
             }
             else
             {
                 if (isLACod_cbx.Checked)
                     rect = Globals.WLRoiRectLAcoordinates;
                 else
                     rect = Globals.IRroiRect;
             }
               List<Rectangle> roiRects = new List<Rectangle>();
                int[] nonZeroCount = Globals.ThreshImg.CountNonzero();
                Image<Gray, byte> temp = new Image<Gray, byte>(Globals.ChannelImg.Width, Globals.ChannelImg.Height);
                CvInvoke.cvCopy(Globals.ChannelImg, temp, Globals.ThreshImg);
             float redArtifact =0f;
             if (nonZeroCount[0] != 0 && temp.GetSum().Intensity != 0)
             {
                 redArtifact = (float)temp.GetSum().Intensity / nonZeroCount[0];
                 mathError = false;
             }
             else
                 mathError = true; 
            double val = 0;
                //Globals.currentSettings.laMetrics.Average_Peak_Before_Correction_IR = Math.Round(redArtifact, 2);
                //val = Globals.currentSettings.laMetrics.Average_Peak_Before_Correction_IR;
                Globals.currentSettings.laMetrics.Average_Peak_Before_Correction_WL = Math.Round(redArtifact, 2);
                val = Globals.currentSettings.laMetrics.Average_Peak_Before_Correction_WL;
                if (mathError)
                {
                    avgPeakVal_lbl.Text = "< 9";
                    avgPeakVal_lbl.ForeColor = Color.Green;

                }
                else
                {
                    if (val > 14)
                    {
                        avgPeakVal_lbl.ForeColor = Color.FromArgb(254, 0, 0);
                        Globals.currentSettings.laMetrics.Average_Peak_Before_Correction_Color_WL = Color.FromArgb(254, 0, 0);
                    }
                    else
                    {
                        avgPeakVal_lbl.ForeColor = Color.Green;
                        Globals.currentSettings.laMetrics.Average_Peak_Before_Correction_Color_WL = Color.Green;
                    }
                    avgPeakVal_lbl.Text = val.ToString();
                }
                MemoryStream storeContours = new MemoryStream();
                nonZeroCount = Globals.RedChannelImg.CountNonzero();
                redArtifact = 0f;
                nonZeroCount[0] = nonZeroCount[0] * 100;
                float divider =(float) (rect.Width * rect.Height);
                if (divider != 0f && nonZeroCount[0]!= 0)
                    redArtifact = (float)nonZeroCount[0]/divider;
                //if (IrMode_rb.Checked)
                //{
                //    Globals.currentSettings.laMetrics.Percentage_Affected_Pixels_Before_Correction_IR = Math.Round(redArtifact, 2);
                //    val = Globals.currentSettings.laMetrics.Percentage_Affected_Pixels_Before_Correction_IR;
                //}
                //else
                {
                    Globals.currentSettings.laMetrics.Percentage_Affected_Pixels_Before_Correction_WL = Math.Round(redArtifact, 2);
                    val = Globals.currentSettings.laMetrics.Percentage_Affected_Pixels_Before_Correction_WL;
                }
                if(val < 1)
                    {
                        Globals.currentSettings.laMetrics.Percentage_Affected_Pixels_Before_Correction_Color_WL = Color.Green;
                        perPixelsAffVal_lbl.ForeColor = Color.Green;
                        percent_lbl.ForeColor = Color.Green;

                    }
                    else
                    {
                        Globals.currentSettings.laMetrics.Percentage_Affected_Pixels_Before_Correction_Color_WL = Color.FromArgb(254, 0, 0);
                        perPixelsAffVal_lbl.ForeColor = Color.FromArgb(254, 0, 0);
                        percent_lbl.ForeColor = Color.FromArgb(254, 0, 0);
                      
                    }
                perPixelsAffVal_lbl.Text = val.ToString();
                for (Contour<Point> c = Globals.ThreshImg.FindContours(); c != null; c = c.HNext)
                {
                    if (c.Area > 50)
                    {
                        rect1 = c.BoundingRectangle;
                        rect1.X += rect1.X;
                        rect1.Y += rect1.Y;
                        roiRects.Add ( rect1);
                    }
                }
                storeContours.Dispose();
                if (tempImg != null)
                    tempImg.Dispose();
                tempImg = new Image<Bgr,byte>(Globals.whiteBmp);
            if(isLACod_cbx.Checked)
               LensArtifactRemove(Globals.WLRoiRectLAcoordinates);
            else
                LensArtifactRemove(Globals.WLRoiRect);
                arg["LACorrectedImage"] = tempImg.ToBitmap();
                f.Publish(f.DisplayCorrectedImage, arg);
             Image<Gray, byte> tempImgGray ;
             int[] nonZeroCountWL;
                if (IrMode_rb.Checked)
                {
                    Globals.currentSettings.laMetrics.Average_Peak_After_Correction_IR = Math.Round(redArtifact, 2);
                    val = Globals.currentSettings.laMetrics.Average_Peak_After_Correction_IR;
                }
                else
                {
                    if (isLACod_cbx.Checked)
                        tempImg.ROI = Globals.WLRoiRectLAcoordinates;
                    else
                    tempImg.ROI = Globals.WLRoiRect;
//                    tempImg.ROI = new Rectangle(Globals.WLRoiRect.X, Globals.WLRoiRect.Y, rect1.Width, rect1.Height);

                    Globals.ChannelImg = tempImg[1];
                    Globals.RedChannelImg = tempImg[2];
                    Globals.RedChannelImg = Globals.RedChannelImg.ThresholdBinary(new Gray(12), new Gray(255));
                    Globals.RedChannelImg = Globals.RedChannelImg.SmoothMedian(3);
                    Image<Gray, byte> tempWL = new Image<Gray, byte>(Globals.ChannelImg.Width, Globals.ChannelImg.Height);
                    nonZeroCountWL = Globals.ThreshImg.CountNonzero();                    
                    CvInvoke.cvCopy(Globals.ChannelImg, tempWL, Globals.ThreshImg);
                    redArtifact = 0f;
                    float channelSum = (float)tempWL.GetSum().Intensity;
                    if (nonZeroCountWL[0] != 0 && channelSum != 0)
                        redArtifact = channelSum / nonZeroCountWL[0];
                    Globals.currentSettings.laMetrics.Average_Peak_After_Correction_WL = Math.Round(redArtifact, 2);
                    val = Globals.currentSettings.laMetrics.Average_Peak_After_Correction_WL;
                }
                avgPeakAfterCorrVal_lbl.Text = val.ToString();

                nonZeroCountWL = Globals.RedChannelImg.CountNonzero();

                nonZeroCountWL[0] = nonZeroCountWL[0] * 100;
                divider = (float)(rect.Width * rect.Height);
               redArtifact = 0f;
               if (nonZeroCountWL[0] != 0 && divider != 0)
                   redArtifact = (float)nonZeroCountWL[0] / divider;
            if (IrMode_rb.Checked)
            {
                Globals.currentSettings.laMetrics.Percentage_Affected_Pixels_After_Correction_IR = Math.Round(redArtifact, 2);
                val = Globals.currentSettings.laMetrics.Percentage_Affected_Pixels_After_Correction_IR;
            }
            else
                {
                    Globals.currentSettings.laMetrics.Percentage_Affected_Pixels_After_Correction_WL = Math.Round(redArtifact, 2);
                    val = Globals.currentSettings.laMetrics.Percentage_Affected_Pixels_After_Correction_WL;
                }
            perPixelsAfterCorrVal_lbl.Text = val.ToString();
            tempImg.ROI = new Rectangle();
            tempImg.Dispose();
            }

        Image<Bgr, byte> tempImg;
        private void PlotIntensityMapping(List<byte> avgValue)
        {
            //float avgMin = avgValue.Min();
            //float avgMax = avgValue.Max();
            //if (avgMax != 0)
            //{
            //    for (int i = 1; i < avgValue.Count; i++)
            //    {

            //        float centVal3 = ((avgValue[i - 1] - avgMin) / (avgMax - avgMin)) * (((float)intensityMap_pbx.Height - 2));
            //        float centVal4 = ((avgValue[i] - avgMin) / (avgMax - avgMin)) * (((float)intensityMap_pbx.Height - 2));
            //        intensityPlot.DrawLine(new Pen(Color.Blue, 1.0f), new PointF(i - 1, centVal3), new PointF(i, centVal4));

            //    }
            //}
        }

        //public void LensArtifactRemove(List<Rectangle> roi)
        public void LensArtifactRemove(Rectangle roi)

        {
            Image<Bgr, byte> bgr_img = tempImg.Copy();
            Image<Gray, byte> G, R, B;
            int count = 0;
            //foreach (Rectangle r in roi)
            {
                gauss = new Image<Gray,double>(roi.Width,roi.Height);
                
                bgr_img.ROI = roi; // set the top watermark ROI
                G = bgr_img[1].Copy();
                R = bgr_img[2].Copy();
                B = bgr_img[0].Copy();

                gauss = Gauss2D(gauss,val);
              
               // Image<Gray, byte> tempR = R.ThresholdBinary(new Gray(128), new Gray(255));

                //double RedSum = tempR.GetSum().Intensity;
                //RedSum = RedSum / 255;
                //if (RedSum < (tempR.Width * tempR.Height * 0.1))
                {
                    bgr_img[1] = ReduceGreenLevel(G, greenScale);
                    bgr_img[2] = ReduceRedLevel(R, redScale);
                    bgr_img[0] = ReduceBlueLevel(B, blueScale);
                }
                bgr_img.ROI = new Rectangle();// Reset the ROI
                if (gauss != null)
                    gauss.Dispose();

                //tempR.Dispose();

            }
            bgr_img.ROI = new Rectangle();
            tempImg = bgr_img.Copy();
            bgr_img.ToBitmap().Save("3.png");
            bgr_img.Dispose();
        }
        /// <summary>
        ///  Function to remove the water mark in the green channel  
        /// </summary>
        /// </summary>
        /// <param name="GreenChannelImage">An Gray scale image </param>
        /// <param name="reduceValue">double value as an multiplication factor varies between 0 to 1  </param>
        ///<param name="rectX"></param>
        /// <returns> returns an Gray scale image  </returns>
        private Image<Gray, byte> ReduceGreenLevel(Image<Gray, byte> GreenChannelImage, double reduceValue)
        {
            Image<Gray, byte> G = GreenChannelImage.Copy();
            Image<Gray, byte> G_f1 = G.SmoothMedian(filter_size_big);
            Image<Gray, byte> G_f2 = G.SmoothMedian(filter_size_small);
            Image<Gray, byte> G_wm;// = G_f2.Sub(G_f1);
            Image<Gray, byte> G_f2_gauss = G_f2.Copy();

            Image<Gray, double> tempG_f2 = G_f2.Convert<Gray, double>();
            tempG_f2 = tempG_f2.Mul(gauss);
            G_f2_gauss = tempG_f2.Convert<Gray, byte>();
            //G_f2_gauss = G_f2.Copy();
            G_wm = G_f2_gauss.Sub(G_f1);

            greenTopZone = G_wm.ThresholdBinary(new Gray(zoneThresholdSize), new Gray(zoneThresholdMaxSize)); // Biggest zone
            Rectangle rect = new Rectangle();
            // Green_top_zone._Dilate(5); // dilating for smoothness
            greenTopZone = contourDetection(greenTopZone, ref rect);


            int zone3_min = 0, zone3_max = 0;
            findMinMaxInZone(G_f2, greenTopZone, ref zone3_min, ref zone3_max);
            //  // using small filtered one to find min and max
            // if (!var)
            {
                for (int a = 0; a < G.Height; a++)
                {
                    for (int b = 0; b < G.Width; b++)
                    {

                        double x;
                        double x1 = G.Data[a, b, 0];
                        double x2 = G.Data[a, b, 0];

                        if (greenTopZone.Data[a, b, 0] != 0)
                        {

                            x1 -= (double)(G_f2.Data[a, b, 0] - G_f1.Data[a, b, 0]) * gauss.Data[a, b, 0] * reduceValue;

                            x = x1;//+y;
                            if (x >= zone3_min)
                                G.Data[a, b, 0] = (byte)(x);
                            else
                                G.Data[a, b, 0] = (byte)(zone3_min);//+y );
                        }
                    }
                }
                Image<Gray, byte> temp_green_zone = greenTopZone.Copy();
                temp_green_zone._Erode(ChannelOuterEdgeErodeSize);
                temp_green_zone = greenTopZone.Sub(temp_green_zone);
                Image<Gray, byte> temp_G = G.CopyBlank();
                G.Copy(temp_G, temp_green_zone);
                temp_G.SmoothMedian(GreenChannelOuterEdgeBlurSize);
                temp_G.Copy(G, temp_green_zone);
                temp_G.Dispose();
                temp_green_zone.Dispose();
            }
            G_wm.Dispose();
            G_f2_gauss.Dispose();
            G_f2.Dispose();
            G_f1.Dispose();
            return G;
        }

        /// <summary>
        ///  Function to remove the water mark in the Blue channel  
        /// </summary>
        /// </summary>
        /// <param name="BlueChannelImage">An Gray scale image </param>
        /// <param name="filter_size_big"> odd value for the High pass filter value varies between 91 to 151</param>
        /// <param name="filter_size_small">odd value for the Low pass filter value varies between 5 to 31</param>
        /// <param name="reduceValue">double value as an multiplication factor varies between 0 to 1  </param>
        /// <returns> returns an Gray scale image  </returns>
        private Image<Gray, byte> ReduceBlueLevel(Image<Gray, byte> BlueChannelImage, double reduceValue)
        {
            Image<Gray, byte> B = BlueChannelImage.Copy();
            Image<Gray, byte> B_f1 = B.SmoothMedian(filter_size_big);
            Image<Gray, byte> B_f2 = B.SmoothMedian(filter_size_small);
            Image<Gray, byte> B_wm;
            Image<Gray, byte> B_f2_gauss = B_f2.Copy();

            Image<Gray, double> tempB_f2 = B_f2.Convert<Gray, double>();
            tempB_f2 = tempB_f2.Mul(gauss);
            B_f2_gauss = tempB_f2.Convert<Gray, byte>();
            B_wm = B_f2_gauss.Sub(B_f1);


            blueTopZone = B.CopyBlank();
            B_wm = B_f2_gauss.Sub(B_f1);

            blueTopZone = B_wm.ThresholdBinary(new Gray(zoneThresholdSize), new Gray(zoneThresholdMaxSize)); // Biggest zone
            Rectangle rect = new Rectangle();
            // Blue_top_zone._Dilate(5); // dilating for smoothness
            blueTopZone = contourDetection(blueTopZone, ref rect);

            int zone3_min = 0, zone3_max = 0;

            // using small filtered one to find min and max

            findMinMaxInZone(B_f2, blueTopZone, ref zone3_min, ref zone3_max);

            //bool var = OdRect.IntersectsWith(rect);
            //  if (!var)
            {
                for (int a = 0; a < B.Height; a++)
                {
                    for (int b = 0; b < B.Width; b++)
                    {

                        double x;
                        double x1 = B.Data[a, b, 0];

                        if (blueTopZone.Data[a, b, 0] != 0)
                        {

                            x1 -= (B_f2.Data[a, b, 0] - B_f1.Data[a, b, 0]) * gauss.Data[a, b, 0] * reduceValue;
                            {
                                x = x1;// +y;
                            }


                            if (x >= zone3_min)
                                B.Data[a, b, 0] = (byte)(x);
                            else
                                B.Data[a, b, 0] = (byte)(zone3_min);// + y);
                        }
                    }
                }

                Image<Gray, byte> temp_blue_zone = blueTopZone.Copy();
                temp_blue_zone._Erode(ChannelOuterEdgeErodeSize);
                temp_blue_zone = blueTopZone.Sub(temp_blue_zone);
                Image<Gray, byte> temp_B = B.CopyBlank();
                B.Copy(temp_B, temp_blue_zone);
                temp_B.SmoothMedian(BlueChannelOuterEdgeBlurSize);
                temp_B.Copy(B, temp_blue_zone);
                temp_B.Dispose();
                temp_blue_zone.Dispose();
            }

            return B;
        }

        /// <summary>
        ///  Function to remove the water mark in the Red channel  
        /// </summary>
        /// </summary>
        /// <param name="RedChannelImage">An Gray scale image </param>
        /// <param name="filter_size_big"> odd value for the High pass filter value varies between 91 to 151</param>
        /// <param name="filter_size_small">odd value for the Low pass filter value varies between 5 to 31</param>
        /// <param name="reduceValue">double value as an multiplication factor varies between 0 to 1  </param>
        /// <returns> returns an Gray scale image  </returns>
        private Image<Gray, byte> ReduceRedLevel(Image<Gray, byte> RedChannelImage, double reduceValue)
        {
            Image<Gray, byte> R = RedChannelImage.Copy();

            Image<Gray, byte> redHighPassFiltImage = R.SmoothMedian(filter_size_big);
            Image<Gray, byte> redLowPassFiltImage = R.SmoothMedian(filter_size_small);
            Image<Gray, byte> redWaterMarkImage;// = R_f2.Sub(R_f1);
            Image<Gray, byte> redLowPassGaussImage = redLowPassFiltImage.Copy();

            Image<Gray, double> tempRedLowPassFiltImage = redLowPassFiltImage.Convert<Gray, double>();
            tempRedLowPassFiltImage = tempRedLowPassFiltImage.Mul(gauss);
            redLowPassGaussImage = tempRedLowPassFiltImage.Convert<Gray, byte>();
            redWaterMarkImage = redLowPassGaussImage.Sub(redHighPassFiltImage);

            // to find zone thresholds, (minimum level is 10)

            redTopZone = R.CopyBlank();
            redWaterMarkImage = redLowPassGaussImage.Sub(redHighPassFiltImage);
            redTopZone = redWaterMarkImage.ThresholdBinary(new Gray(zoneThresholdSize), new Gray(zoneThresholdMaxSize)); // Biggest zone


            //Red_top_zone._Dilate(5); // dilating for smoothness
            Rectangle rect = new Rectangle();
            redTopZone = contourDetection(redTopZone, ref rect);

            int zoneMin = 0, zoneMax = 0;

            // using small filtered one to find min and max

            findMinMaxInZone(redLowPassFiltImage, redTopZone, ref zoneMin, ref zoneMax);
            // bool var = OdRect.IntersectsWith(rect);
            // if (!var)
            {
                for (int a = 0; a < R.Height; a++)
                {
                    for (int b = 0; b < R.Width; b++)
                    {

                        double x;
                        double x1 = R.Data[a, b, 0];

                        if (redTopZone.Data[a, b, 0] != 0)
                        {

                            x1 -= (redLowPassFiltImage.Data[a, b, 0] - redHighPassFiltImage.Data[a, b, 0]) * gauss.Data[a, b, 0] * reduceValue;

                            x = x1;// +y;

                            if (x >= zoneMin)
                                R.Data[a, b, 0] = (byte)(x);
                            else
                                R.Data[a, b, 0] = (byte)(zoneMin);// + y);
                        }
                    }
                }
                Image<Gray, byte> tempRedZone = redTopZone.Copy();
                tempRedZone._Erode(ChannelOuterEdgeErodeSize);
                tempRedZone = redTopZone.Sub(tempRedZone);
                Image<Gray, byte> tempR = R.CopyBlank();
                R.Copy(tempR, tempRedZone);
                tempR.SmoothMedian(redChannelOuterEdgeBlurSize);
                tempR.Copy(R, tempRedZone);
                tempR.Dispose();
                tempRedZone.Dispose();
            }
            tempRedLowPassFiltImage.Dispose();
            redHighPassFiltImage.Dispose();
            redLowPassFiltImage.Dispose();
            redWaterMarkImage.Dispose();
            redLowPassGaussImage.Dispose();
            return R;
        }

        /// <summary>
        /// Finds the minimum and maximum of an gray scale image in the masked regions and returns the minimum and maximum values
        /// as min and max
        /// </summary>
        /// <param name="srcImage"> Gray scale image whose minimum and maximum to be found in the mask region</param>
        /// <param name="mask">Gray scale mask </param>
        /// <param name="min">minimum gray scale value in the mask region of the image as an reference</param>
        /// <param name="max">maximum gray scale value in the mask region of the image as an reference</param>
        private void findMinMaxInZone(Image<Gray, byte> srcImage, Image<Gray, byte> mask, ref int min, ref int max)
        {
            Image<Gray, byte> copyImage = srcImage.Copy();

            copyImage._And(mask);

            double[] minVal = new double[3];
            double[] maxVal = new double[3];
            Point[] minLoc = new Point[3];
            Point[] maxLoc = new Point[3];
            copyImage.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);
            max = (int)maxVal[0];

            copyImage = srcImage.Copy();
            Image<Gray, byte> mask_invert = mask.Not();
            copyImage._Or(mask_invert);
            copyImage.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);
            min = (int)minVal[0];
            copyImage.Dispose();
            mask_invert.Dispose();
            //mask.Dispose();
        }

        /// <summary>
        /// Returns a  gaussian distribution taking a blank image and the width of the gaussian distribution as parameters 
        /// </summary>
        /// <param name="gauss"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        private Image<Gray, double> Gauss2D(Image<Gray, double> gauss, double val)
        {
            double a = 1;
            double xc = gauss.Width / 2;
            double yc = gauss.Height / 2;
            for (int y = 0; y < gauss.Height; y++)
            {
                for (int x = 0; x < gauss.Width; x++)
                {
                    double xtmp = ((x - xc) / gauss.Width / val) * ((x - xc) / gauss.Width / val) / 2;
                    double ytmp = ((y - yc) / gauss.Height / val) * ((y - yc) / gauss.Height / val) / 2;
                    gauss.Data[y, x, 0] = a * Math.Exp(-xtmp - ytmp);
                }
            }
            return gauss;
        }

        private void Gauss2D_bottom()
        {
            gauss_bottom = new Image<Gray, double>(130, 140);
            double a = 1;
            for (int i = 0; i < gauss_bottom.Height; i++)
            {
                for (int j = 0; j < gauss_bottom.Width; j++)
                {
                    double x = j - (gauss_bottom.Width / 2);
                    double tempd = ((double)x / ((double)gauss_bottom.Width / 4));
                    tempd = tempd * tempd / 2;
                    double y = i - (gauss_bottom.Height / 2);
                    double tempf = y / ((double)gauss_bottom.Height / 4);
                    tempf = tempf * tempf / 2;

                    tempd = tempd + tempf;
                    gauss_bottom.Data[i, j, 0] = a * Math.Exp(-tempd);

                }
            }

        }

        /// <summary>
        /// Performs the contour detection on a binary image and returns the biggest contour
        /// </summary>
        /// <param name="maskImg">Binary Image for contour detection</param>
        /// <param name="BoundingRect">the pointer for the bounding rectangle of the biggest contour</param>
        /// <returns>returns the biggest contour in the mask image.</returns>
        private Image<Gray, byte> contourDetection(Image<Gray, byte> maskImg, ref  Rectangle BoundingRect)
        {
            Image<Gray, byte> ret_img = new Image<Gray, byte>(maskImg.Width, maskImg.Height);
            List<double> contArray = new List<double>();
            for (Contour<Point> contour = maskImg.FindContours(); contour != null; contour = contour.HNext)
            {
                contArray.Add(contour.Area);
            }
            if (contArray.Count > 0)
            {
                double area = contArray.Max();
                for (Contour<Point> contour = maskImg.FindContours(); contour != null; contour = contour.HNext)
                {
                    if (contour.Area == area)
                    {
                        ret_img.Draw(contour, new Gray(255), -1);
                        BoundingRect = contour.BoundingRectangle;
                        break;
                    }
                }
                return ret_img;
            }
            else
            {
                BoundingRect = new Rectangle(0, 0, 100, 100);
                return maskImg;
            }
        }

        public void resume_btn_Click(object sender, EventArgs e)
        {
            if (Globals.browseBtnClicked)
            {
                IrMode_rb.Checked = true;
                Globals.isHighResolution = false;
                Globals.isTopCord = false;
                Globals.isBottomCord = false;
                Globals.browseBtnClicked = false;
            }
            tricam.resumeLiveMode();
            ModeSelection_gbx.Enabled = true;
            isLACod_cbx.Checked = false;
            grading_btn.Visible = false;
            whiteLightCapture_btn.Visible = true;
            resume_btn.Enabled = false;
            
        }

        private void IrMode_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (tricam.isLiveMode())
            {
                tricam.stopLiveMode();
                Globals.isHighResolution = !IrMode_rb.Checked;
                if (!Globals.isHighResolution)
                {
                    if (LA_avgPeakRange_lbl.Visible)
                    {
                        LA_avgPeakRange_lbl.Visible = false;
                        LA_avgPeakRange_lbl.Refresh();
                    }
                    //if (LA_PerAffPixelsRange_lbl.Visible)
                    //{
                    //    LA_PerAffPixelsRange_lbl.Visible = false;
                    //    LA_PerAffPixelsRange_lbl.Refresh();
                    //}
                }
                else
                {
                    if (!LA_avgPeakRange_lbl.Visible)
                    {
                        LA_avgPeakRange_lbl.Visible = true;
                        LA_avgPeakRange_lbl.Refresh();
                    }
                    //if (!LA_PerAffPixelsRange_lbl.Visible)
                    //{
                    //    LA_PerAffPixelsRange_lbl.Visible = true;
                    //    LA_PerAffPixelsRange_lbl.Refresh();
                    //}
                }
                f.Publish(f.SET_DISPLAY_AREA, null);
                tricam.startLiveMode();
            }
            else
            {
                f.Publish(f.GradingStill, null);
            }
        }

        private void isLACod_cbx_CheckedChanged(object sender, EventArgs e)
        {
                LaCord_gbx.Enabled = isLACod_cbx.Checked;
                Globals.isSetLACord = isLACod_cbx.Checked;
                LA_Bottom_rb.Enabled = isLACod_cbx.Checked;
                La_Top_rb.Enabled = isLACod_cbx.Checked;
                LA_TopPanel.Enabled = false;
                LA_BottomPanel.Enabled = false;
                if (isLACod_cbx.Checked)
                {
                    resume_btn.Enabled = true;
                    ModeSelection_gbx.Enabled = false;
                    tricam.stopLiveMode();
                }
                else
                {
                    La_Top_rb.Checked = false;
                    LA_Bottom_rb.Checked = false;
                    f.Publish(f.SET_DISPLAY_AREA, args);
                }
        }

        private void La_Top_rb_CheckedChanged(object sender, EventArgs e)
        {
            Globals.isTopCord = La_Top_rb.Checked;
            LA_TopPanel.Enabled = true;
            LA_BottomPanel.Enabled = false;
        }

        private void LA_Bottom_rb_CheckedChanged(object sender, EventArgs e)
        {
            Globals.isBottomCord = LA_Bottom_rb.Checked;
            LA_BottomPanel.Enabled = true;
            LA_TopPanel.Enabled = false;
        }

        private void la_TopX_nud_ValueChanged_1(object sender, EventArgs e)
        {
            Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.ClassicSettings.LensArtifactTopX, (int)la_TopX_nud.Value);
            Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.ClassicSettings.LensArtifactTopY, (int)la_TopY_nud.Value);
            Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.ClassicSettings.LensArtifactBottomX, (int)la_BottomX_nud.Value);
            Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.ClassicSettings.LensArtifactBottomY, (int)la_BottomY_nud.Value);
            f.Publish(f.SET_LACoordinates, null);
        }

    }
}

















































































































































































































































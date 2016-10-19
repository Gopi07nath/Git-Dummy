using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.Util;
using Emgu.CV.UI;

namespace Calibration
{
    public partial class AlignmentUI : UserControl
    {
        
        public int deviceId = 0;
        Alignment alignment;
        Facade f; 
        Tricam tricam;
        Args args;
        

        public AlignmentUI()
        {
            InitializeComponent();
            tricam = Tricam.createTricam();
            f = Facade.getInstance();
            f.Subscribe("Update_Image_Metrics_UI", new NotificationHandler(Update_Image_Metrics_UI));
            f.Subscribe("AlignmentSaveReport", new NotificationHandler(saveReportAlignment));
            f.Subscribe("FundusAlignmentMode", new NotificationHandler(FundusAlignmentMode));
            f.Subscribe("ManageControlsAfterSaveReport", new NotificationHandler(manageControlsAfterSaveReport));
            //f.Subscribe("ManageIlluminationGrid", new NotificationHandler(manageIlluminationGrid));
            if (alignment == null)
                alignment = new Alignment();
            covRange_lbl.Text = "(Recommended Value < 0.25)";
            peripheryToInnerRange_lbl.Text = "(Recommended Value > 0.81)";
            SaveImage_IlluminationGrid_btn.Enabled = false;
            resume_btn.Enabled = false;
        }

        private void FundusAlignmentMode(String n, Args args)
        {
            if (Globals.browseBtnClicked)
            {
                ModeSelection_panel.Enabled = false;
                IrImageCapture_btn.Enabled = false;
                whiteLightCapture_btn.Enabled = false;
                resume_btn.Enabled = true;
                Globals.isIlluminationGrid = false;
                manageIlluminationGrid();
            }
            else
            {
                ModeSelection_panel.Enabled = true;
                IrImageCapture_btn.Enabled = true;
                whiteLightCapture_btn.Enabled = true;
                resume_btn.Enabled = false;
                Globals.isIlluminationGrid = true;
                manageIlluminationGrid();
            }

            IrMode_rb_CheckedChanged(null, null);
        }

        private void manageControlsAfterSaveReport(string s, Args n)
        {
            if ((bool)n["FundusPage"])
            {
                //if (Globals.browseBtnClicked)
                //{
                //    ModeSelection_panel.Enabled = true;
                //    IrImageCapture_btn.Enabled = true;
                //    whiteLightCapture_btn.Enabled = true;
                //    resume_btn.Enabled = false;
                //    Globals.isIlluminationGrid = true;
                //    manageIlluminationGrid();
                //}
                //else
                //{
                    ModeSelection_panel.Enabled = false;
                    IrImageCapture_btn.Enabled = false;
                    whiteLightCapture_btn.Enabled = false;
                    resume_btn.Enabled = true;
                    Globals.isIlluminationGrid = false;
                    manageIlluminationGrid();
                //}

                if (IrMode_rb.Checked)
                    Globals.isHighResolution = false;
            }
        }

        public void resume_btn_Click(object sender, EventArgs e)
        {
            if (Globals.browseBtnClicked)
            {
                Globals.browseBtnClicked = false;
                IrMode_rb_CheckedChanged(null, null);
            }
            if (IrMode_rb.Checked)
                Globals.isHighResolution = false;
            tricam.startLiveMode();
            ModeSelection_panel.Enabled = true;
            IrImageCapture_btn.Enabled = true;
            whiteLightCapture_btn.Enabled = true;
            Globals.isIlluminationGrid = true;
            manageIlluminationGrid();
            resume_btn.Enabled = false;
            this.Refresh();
        }

        public void whiteLightCapture_btn_Click(object sender, EventArgs e)
        {
            if (tricam.isLiveMode())
            {
                WhiteLightCapture();
                IrImageCapture_btn.Enabled = false;
                resume_btn.Enabled = true;
                ModeSelection_panel.Enabled = false;
                Globals.isIlluminationGrid = false;
                manageIlluminationGrid();
            }
        }

        public void IrImageCapture_btn_Click(object sender, EventArgs e)
        {
            if (tricam.isLiveMode())
            {
                IRCapture();
                whiteLightCapture_btn.Enabled = false;
                resume_btn.Enabled = true;
                ModeSelection_panel.Enabled = false;
                Globals.isIlluminationGrid = false;
                manageIlluminationGrid();
            }
        }

        private  void IRCapture()
        {
            tricam.IRCapture();
            args = new Args();
            args["isIR"] = true;
            f.Publish(f.ApplyMask, args);
            f.Publish(f.Save_Image, args);
            f.Publish(f.Alignment_Image_Metrics, args);
            f.Publish(f.Update_Image_Metrics_UI, args);

        }
       
        private void Update_Image_Metrics_UI(string n, Args arg)
        {
            if (!Globals.isHighResolution)
            {   
                
                avgIntensityVal_lbl.Text = Globals.currentSettings.imageMetrics.Average_IR_Light_Intensity.ToString();
                perToInnerVarVal_lbl.Text = Globals.currentSettings.imageMetrics.Periphery_To_Inner_IR_Light_Intensity_Variation.ToString();
                covVal_lbl.Text = Globals.currentSettings.imageMetrics.Top_To_Bottom_IR_Light_Intensity_Variation.ToString();

                avgIntensityVal_lbl.ForeColor = Globals.currentSettings.imageMetrics.averageIntensityLabelColor_IR;
                perToInnerVarVal_lbl.ForeColor = Globals.currentSettings.imageMetrics.peripheryToInnerIntensityVarLabelColor_IR;
                covVal_lbl.ForeColor = Globals.currentSettings.imageMetrics.topToBottomIntensityVarLabelColor_IR;
                AvgRangeVal_lbl.Text = "(Recommended Range  70 - 73)";
            }
            else
            {
                avgIntensityVal_lbl.Text = Globals.currentSettings.imageMetrics.Average_White_Light_Intensity.ToString();
                perToInnerVarVal_lbl.Text = Globals.currentSettings.imageMetrics.Periphery_To_Inner_White_Light_Intensity_Variation.ToString();
                covVal_lbl.Text = Globals.currentSettings.imageMetrics.Top_To_Bottom_White_Light_Intensity_Variation.ToString();

                avgIntensityVal_lbl.ForeColor = Globals.currentSettings.imageMetrics.averageIntensityLabelColor_WL;
                perToInnerVarVal_lbl.ForeColor = Globals.currentSettings.imageMetrics.peripheryToInnerIntensityVarLabelColor_WL;
                covVal_lbl.ForeColor = Globals.currentSettings.imageMetrics.topToBottomIntensityVarLabelColor_WL;
                if (tricam.isLiveMode())
                {
                    if(Globals.isRoyal)
                        AvgRangeVal_lbl.Text = "(Recommended Range  150 - 170)";
                    else
                    AvgRangeVal_lbl.Text = "(Recommended Range  190 - 200)";
                }
                else
                {
                    if(Globals.isRoyal)
                        AvgRangeVal_lbl.Text = "(Recommended Range  180 - 200)";
                    else
                    AvgRangeVal_lbl.Text = "(Recommended Range  220 - 230)";
                }
            }

            avgIntensityVal_lbl.Refresh();
            perToInnerVarVal_lbl.Refresh();
            covVal_lbl.Refresh();
            AvgRangeVal_lbl.Refresh();
           
        }

        private void WhiteLightCapture()
        {
            tricam.WhiteLightCapture();
            args = new Args();
            args["isIR"] = false;
            f.Publish(f.ApplyMask, args);
            f.Publish(f.Save_Image, args);
            f.Publish(f.Alignment_Image_Metrics, args);
            f.Publish(f.Update_Image_Metrics_UI, args);

        }

        private void saveReportAlignment(string s, Args arg)
        {
            WhiteLightCapture();
            IRCapture();
        }

        private void IrMode_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (!Globals.browseBtnClicked)
                Globals.isHighResolution = !IrMode_rb.Checked;

            if (!Globals.isIlluminationGrid)
            {
                args = new Args();
                args["page"] = Globals.mode.isFundusAlignment;
                f.Publish(f.SET_DISPLAY_AREA, args);
                if (!Globals.browseBtnClicked)
                    tricam.startLiveMode();
                //IrImageCapture_btn.Enabled = true;
                //whiteLightCapture_btn.Enabled = true;
                f.Publish(f.Alignment_Image_Metrics, args);
                f.Publish(f.Update_Image_Metrics_UI, args);
            }
            else
            {
                IlliuminationGrid();
            }
        }

        private void illuminationGrid_cbx_CheckedChanged(object sender, EventArgs e)
        {
            IlliuminationGrid();
        }

        private void IlliuminationGrid()
        {
            if (illuminationGrid_cbx.Checked)
            {
                Globals.isIlluminationGrid = true;
                f.Publish(f.DisableBrowseBtn, args);
                tricam.stopLiveMode();
                args = new Args();
                args["page"] = Globals.mode.isFundusAlignment;
                f.Publish(f.SET_DISPLAY_AREA, args);
                SaveImage_IlluminationGrid_btn.Enabled = true;
                //if (Globals.browseBtnClicked)
                //{
                //    Globals.isHighResolution = false;
                //}
                tricam.startLiveMode();
            }
            else
            {
                Globals.isIlluminationGrid = false;
                f.Publish(f.DisableBrowseBtn, args);
                tricam.stopLiveMode();
                args = new Args();
                args["page"] = Globals.mode.isFundusAlignment;
                f.Publish(f.SET_DISPLAY_AREA, args);
                SaveImage_IlluminationGrid_btn.Enabled = false;
                tricam.startLiveMode();
            }
        }

        Image<Bgr, byte> im;
        private void SaveImage_IlluminationGrid_btn_Click(object sender, EventArgs e)
        {
            tricam.stopLiveMode();
            if (Globals.isHighResolution)
            {
                im = new Image<Bgr, byte>(Globals.whiteBmp);
                Bitmap mask = new Bitmap(@"Resources\Grid_Mask3MP.png");
                im._And(new Image<Bgr, byte>(mask));
            }
            else
            {
                im = new Image<Bgr, byte>(Globals.IrBmp);
                Bitmap mask = new Bitmap(@"Resources\Grid_Mask1MP.png");
                im._And(new Image<Bgr, byte>(mask));
            }

            Bitmap bitmap = new Bitmap(im.ToBitmap());
            im.Dispose();
            Graphics g = Graphics.FromImage(bitmap);
            Font font_Wl = new Font("Arial", 44, FontStyle.Bold);
            Font font_IR = new Font("Arial", 30, FontStyle.Bold);
            SolidBrush brush = new SolidBrush(Color.Green);

            string filePath = Globals.calibrationPath + Path.DirectorySeparatorChar + Globals.DeviceId + Path.DirectorySeparatorChar + DateTime.Now.ToString("dd-MM-yyyy");
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            if (Globals.isHighResolution)
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    g.DrawString(Globals.currentSettings.imageMetrics.firstValue.ToString(), font_Wl, brush, new PointF(1450.0F, 540.0F));
                    g.DrawString(Globals.currentSettings.imageMetrics.secondValue.ToString(), font_Wl, brush, new PointF(1165.0F, 300.0F));
                    g.DrawString(Globals.currentSettings.imageMetrics.thirdValue.ToString(), font_Wl, brush, new PointF(780.0F, 300.0F));
                    g.DrawString(Globals.currentSettings.imageMetrics.fourthValue.ToString(), font_Wl, brush, new PointF(500.0F, 540.0F));
                    g.DrawString(Globals.currentSettings.imageMetrics.fifthValue.ToString(), font_Wl, brush, new PointF(500.0F, 885.0F));
                    g.DrawString(Globals.currentSettings.imageMetrics.sixthValue.ToString(), font_Wl, brush, new PointF(780.0F, 1210.0F));
                    g.DrawString(Globals.currentSettings.imageMetrics.seventhValue.ToString(), font_Wl, brush, new PointF(1165.0F, 1210.0F));
                    g.DrawString(Globals.currentSettings.imageMetrics.eighthValue.ToString(), font_Wl, brush, new PointF(1450.0F, 885.0F));
                }
                bitmap.Save(filePath + Path.DirectorySeparatorChar + Globals.DeviceId + "_" + DateTime.Now.ToString("hhmmss") + "_WL_Grid.png");
            }
            else
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {

                    g.DrawString(Globals.currentSettings.imageMetrics.firstValue.ToString(), font_IR, brush, new PointF(725.0F, 270.0F));
                    g.DrawString(Globals.currentSettings.imageMetrics.secondValue.ToString(), font_IR, brush, new PointF(580.0F, 150.0F));
                    g.DrawString(Globals.currentSettings.imageMetrics.thirdValue.ToString(), font_IR, brush, new PointF(390.0F, 150.0F));
                    g.DrawString(Globals.currentSettings.imageMetrics.fourthValue.ToString(), font_IR, brush, new PointF(250.0F, 270.0F));
                    g.DrawString(Globals.currentSettings.imageMetrics.fifthValue.ToString(), font_IR, brush, new PointF(250.0F, 440.0F));
                    g.DrawString(Globals.currentSettings.imageMetrics.sixthValue.ToString(), font_IR, brush, new PointF(390.0F, 605.0F));
                    g.DrawString(Globals.currentSettings.imageMetrics.seventhValue.ToString(), font_IR, brush, new PointF(580.0F, 605.0F));
                    g.DrawString(Globals.currentSettings.imageMetrics.eighthValue.ToString(), font_IR, brush, new PointF(725.0F, 440.0F));
                }
                bitmap.Save(filePath + Path.DirectorySeparatorChar + Globals.DeviceId + "_" + DateTime.Now.ToString("hhmmss") + "_IR_Grid.png");
            }

            MessageBox.Show("Image Saved", "Calibration Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
            tricam.startLiveMode();
        }

        private void manageIlluminationGrid()
        {
            if (Globals.isIlluminationGrid)
            {
                uniformillumination_gbx.Enabled = true;
                uniformillumination_gbx.Refresh();
            }
            else
            {
                uniformillumination_gbx.Enabled = false;
                uniformillumination_gbx.Refresh();
            }
        }

    }
}

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
using Forus.Refracto;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Drawing.Drawing2D;

namespace Calibration
{
    public partial class DisplayArea : UserControl
    {
        Bitmap maskOverlay, overlayImage;
        Facade facade;
        Graphics intensityPlot, gridLines;
        Rectangle prevroiRect;
        int width = 0, height = 0, pbWidth = 0, pbHeight = 0;
        int xCord, yCord;
        Image<Bgr, byte> tempImg;
        Tricam tricam;
        public Camera mycamera;
        private MouseEventHandler mouse_wheel_handler;
        private MouseEventHandler mouse_move_handler;
        private MouseEventHandler mouse_down_handler;
        private MouseEventHandler mouse_up_handler;
        private EventHandler mouse_enter_handler;
        private EventHandler displayWindow_pbx_double_click_handler;
        private EventHandler this_double_click_handler;
        private string power = "";
        private string powerLive = "";

        //for refracto table
        DirectoryInfo dirInf;
        AlignmentUI alignmentUI;
        string eyeside;
        string sph;
        string cyl;
        string axis;
        //for refracto table
        public DisplayArea()
        {
            InitializeComponent();
            tricam = Tricam.createTricam();
            facade = Facade.getInstance();
            overLay_pbx.Parent = displayWindow_pbx;
            overLay_pbx.Dock = DockStyle.Fill;
            overLay_pbx.BackColor = Color.Transparent;
            ODoverlay_pbx.Parent = overLay_pbx;
            ODoverlay_pbx.Dock = DockStyle.Fill;
            ODoverlay_pbx.BackColor = Color.Transparent;
            facade.Subscribe(facade.SET_DISPLAY_AREA, new NotificationHandler(setDisplayArea));
            facade.Subscribe(facade.displayImage, new NotificationHandler(displayImage));
            facade.Subscribe(facade.DisplayCorrectedImage, new NotificationHandler(DisplayLACorrectedImage));
            facade.Subscribe(facade.REFRACTO_CAPTURE_START, new NotificationHandler(onRefractoCaptureStart));
            facade.Subscribe(facade.REFRACTO_CAPTURE_COMPLETE, new NotificationHandler(onRefractoCaptureComplete));
            facade.Subscribe(facade.GO_STANDBY, new NotificationHandler(standBy));
            facade.Subscribe(facade.REFRACTO_CALCULATIONS_COMPLETE, new NotificationHandler(onRefractionCalculation));//To display power after capture and calculation
            //facade.Subscribe(facade.REFRACTO_FOCUSKNOB_STATUS, new NotificationHandler(onFocusKnobChanged));
            facade.Subscribe(facade.REFRACTO_NO_OF_SPOTS, new NotificationHandler(onSpotsDetection));
            facade.Subscribe(facade.REFRACTO_CAPTURE_COMPLETE_INLIVE, new NotificationHandler(onRefractoCaptureCompleteInLive));
            facade.Subscribe(facade.Refracto_Retake, new NotificationHandler(RefractoRetake));//To display retake message on display area.
            facade.Subscribe(facade.SET_LACoordinates, new NotificationHandler(DrawCoordinates));
            facade.Subscribe(facade.Update_IlluminationGrid, new NotificationHandler(updateIlluminationGrid));
            facade.Subscribe(facade.RefreshGridLabels, new NotificationHandler(refreshGridLabels));
            //refracto table.
            bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            if (designMode == false)
            {
              
                initializeLang();
            }
            populateReadings();
            facade.Subscribe(facade.REFRACTO_CALCULATIONS_COMPLETE, new NotificationHandler(onRingCalculations));//To save power in datagridview
        
            //
            facade.Subscribe(facade.SetAoiLensArtifactMeasurement, new NotificationHandler(SetAOILensArtifact));
            if (Globals.isHighResolution)
            {
                Globals.WLRoiRect = new Rectangle(950, 450, 300, 500);
                prevroiRect = new Rectangle(950, 450, 300, 500);
            }
            else
            {
                Globals.IRroiRect = new Rectangle(425, 225, 150, 250); ;
                prevroiRect = new Rectangle(425, 225, 150, 250); ;
            }
            pbWidth = displayWindow_pbx.Width;
            pbHeight = displayWindow_pbx.Height;

        }
        private void initializeLang()
        {
            string s = ("Eye");
            dataGridView1.Columns[0].HeaderText = ("Eye");
            dataGridView1.Columns[0].ValueType = typeof(string);
            dataGridView1.Columns[1].HeaderText = ("SPH");
            dataGridView1.Columns[2].HeaderText = ("CYL");
            dataGridView1.Columns[3].HeaderText = ("Axis");
            dataGridView1.Columns[4].HeaderText = ("Remove");
        }
        public void populateReadings()
        {
            try
            {
                dataGridView1.Rows.Clear();
                if (!File.Exists(Globals.refractoReadingsFile)) return;

                StreamReader sr = new StreamReader(Globals.refractoReadingsFile);
                var lines = File.ReadAllLines(Globals.refractoReadingsFile).Reverse();
                foreach (string line in lines)
                {
                    if (line == "") continue;
                    string[] readings = line.Split('\t');
                    eyeside = readings[0];
                    sph = readings[1];
                    cyl = readings[2];
                    int axis_temp = Convert.ToInt32(readings[3]);
                    axis_temp = (axis_temp);
                    axis = axis_temp.ToString();
                    //axis = readings[3];
                    addReadings(eyeside, sph, cyl, axis);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void addReadings(string eyeside, string sph, string cyl, string axis)
        {
            DataGridViewRow row = new DataGridViewRow();
            dataGridView1.Rows.Insert(0, row);
            dataGridView1.Rows[0].Cells[0].Value = eyeside;
            dataGridView1.Rows[0].Cells[1].Value = sph;
            dataGridView1.Rows[0].Cells[2].Value = cyl;
            dataGridView1.Rows[0].Cells[3].Value = axis;
        }
        private void onRingCalculations(string n, Args arg)
        {
           RefractoReadings readings = arg["readings"] as RefractoReadings;
           if (readings.Eyeside == null)
               readings.Eyeside = "L";
            if (readings.IsProperRing)
            {
                addReadings(readings.Eyeside, readings.SPH.ToString("0.00"), readings.CYL.ToString("0.00"), ((readings.AXIS)%180).ToString());
                saveReadings();
            }
        }
        private void saveReadings()
        {
            string TAB = "\t";
            List<string> lines = new List<string>();
            try
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    eyeside = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    sph = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    cyl = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    axis = dataGridView1.Rows[i].Cells[3].Value.ToString();
                    string text = eyeside + TAB + sph + TAB + cyl + TAB + axis;
                    lines.Add(text);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
           saveRefractoReadings(lines);
        }
        public static void saveRefractoReadings(List<string> lines)
        {

            FileInfo fInf = new FileInfo(Globals.refractoReadingsFile);
            if (!Directory.Exists(fInf.DirectoryName))
            {
                Directory.CreateDirectory(fInf.DirectoryName);
            }
            if (!File.Exists(Globals.refractoReadingsFile))
            {
                FileStream f = File.Create(Globals.refractoReadingsFile);
                f.Close();
            }
            StreamWriter sw = new StreamWriter(Globals.refractoReadingsFile, false);
            foreach (string item in lines)
            {
                sw.WriteLine(item);
                sw.WriteLine(sw.NewLine);
            }
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
        //////////////////////////////////////////////////refracto table end
        private void setDisplayArea(String n, Args args)
        {
            ClearDisplayArea();
            isLensArtifactMeasurement = false;
            thresholded_pbx.Visible = false;
            LA_AfterCorrection_pbx.Visible = false;
            displayWindow_pbx.BackColor = Color.Black;

            switch ( Globals.currentMode)
            {
                case Globals.mode.isMemoryTest:
                    {
                        ODoverlay_pbx.Visible = false;
                        overLay_pbx.Visible = false;
                        dataGridView1.Visible = false;
                        Clear_btn.Visible = false;
                        break;
                    }
                case Globals.mode.isCameraAlignment:
                    {

                        ODoverlay_pbx.Visible = false;
                        overLay_pbx.Visible = true;
                        maskOverlay = new Bitmap(@"Resources\AlignmentTemplate.bmp");
                        maskOverlay.MakeTransparent(Color.Black);
                        overLay_pbx.Image = maskOverlay;
                        dataGridView1.Visible = false;
                        Clear_btn.Visible = false;
                        
                        break;

                    }
                case Globals.mode.isFundusAlignment:
                    {
                        overLay_pbx.Visible = true;
                        ODoverlay_pbx.Visible = true;
                        dataGridView1.Visible = false;
                        Clear_btn.Visible = false;

                        if (!Globals.isIlluminationGrid)
                        {
                            refreshSectorLables(); 

                            if (maskOverlay != null)
                            {
                                if (!Globals.isHighResolution)
                                    maskOverlay = new Bitmap(@"Resources\Mask_1MP.bmp");
                                else
                                    maskOverlay = new Bitmap(@"Resources\Mask_3MP.bmp");
                                maskOverlay.MakeTransparent(Color.White);
                            }

                            Bitmap odOverlaybm = new Bitmap(Globals.IRWidth, Globals.IRheight);
                            Graphics g = Graphics.FromImage(odOverlaybm);
                            g.DrawRectangle(new Pen(Color.FromArgb(255, 0, 148, 255), 2.0f), 550, 160, 75, 110);
                            g.DrawEllipse(new Pen(Color.Red, 2.0f), 580, 230, 2, 2);
                            odOverlaybm.MakeTransparent(Color.Black);
                            ODoverlay_pbx.Image = odOverlaybm;
                            overLay_pbx.Image = maskOverlay;
                        }
                        else
                        {
                            refreshSectorLables();
                            drawIlluminationGrid();
                            updateIlluminationGrid(null,null);
                        }
                        break;

                    }
                case Globals.mode.isLensArtifactMeasurement:
                    {
                        //if (Globals.CameraInitialized)
                        {
                            //facade.Publish(facade.Read_Nvram, null);
                        }
                        overLay_pbx.Visible = true;
                        ODoverlay_pbx.Visible = false;
                        thresholded_pbx.Visible = true;
                        LA_AfterCorrection_pbx.Visible = true;
                        dataGridView1.Visible = false;
                        Clear_btn.Visible = false;
                        if (overlayImage != null)
                            overlayImage.Dispose();
                        if (Globals.isHighResolution)
                        {
                            overlayImage = new Bitmap(Globals.WlWidth, Globals.Wlheight);
                            prevroiRect = Globals.WLRoiRect;
                        }
                        else
                        {
                            overlayImage = new Bitmap(Globals.IRWidth, Globals.IRheight);
                            prevroiRect = Globals.IRroiRect;
                        }
                        width = overlayImage.Width;
                        height = overlayImage.Height;
                        drawGridLines();
                        overLay_pbx.BackColor = Color.Transparent;
                        overlayImage.MakeTransparent(Color.Black);
                        overLay_pbx.Image = overlayImage;
                        isLensArtifactMeasurement = true;
                        break;
                    }
                case Globals.mode.isRefractoAlignment:
                    {
                        if (!Globals.isRoyal)
                            this.Visible = false;
                        break;
                    }
                case Globals.mode.isRefractoCalibration:
                    {
                        displayWindow_pbx.BackColor = Color.White;
                        ODoverlay_pbx.Visible = false;
                        overLay_pbx.Visible = true;
                        maskOverlay = new Bitmap(1024, 768);
                        maskOverlay.MakeTransparent(Color.Black);
                        overLay_pbx.Image = maskOverlay;
                        dataGridView1.Visible = true;
                        Clear_btn.Visible = true;
                        dataGridView1.Rows.Clear();
                        if (Globals.CameraInitialized)
                        {
                            facade.Publish(facade.Read_Nvram, null);
                            drawSpotIndication();
                        } 
                        break;
                    }
            }
        }

        private void drawIlluminationGrid()
        {
            if (maskOverlay != null)
            {
                if (!Globals.isHighResolution)
                    maskOverlay = new Bitmap(@"Resources\Grid_Mask1MP.png");
                else
                    maskOverlay = new Bitmap(@"Resources\Grid_Mask3MP.png");
                maskOverlay.MakeTransparent(Color.White);
            }

            Bitmap odOverlaybm = new Bitmap(Globals.IRWidth, Globals.IRheight);
            Graphics g = Graphics.FromImage(odOverlaybm);
            g.DrawRectangle(new Pen(Color.FromArgb(255, 0, 148, 255), 2.0f), 550, 160, 75, 110);
            g.DrawEllipse(new Pen(Color.Red, 2.0f), 580, 230, 2, 2);

            updateGridValues();

            odOverlaybm.MakeTransparent(Color.Black);
            ODoverlay_pbx.Image = odOverlaybm;
            overLay_pbx.Image = maskOverlay;
        
        }

        public void updateIlluminationGrid(string s,object o)
        {
            updateGridValues();
        }

        public void refreshGridLabels(string s, object o)
        {
            Globals.isIlluminationGrid = false;
            refreshSectorLables();
        }

        public void refreshSectorLables()
        {
            if (Globals.isIlluminationGrid)
            {
                this.label1.BackColor = System.Drawing.Color.Transparent;

                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                label8.Visible = true;
            }
            else
            {
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
            }
            label1.Refresh();
            label2.Refresh();
            label3.Refresh();
            label4.Refresh();
            label5.Refresh();
            label6.Refresh();
            label7.Refresh();
            label8.Refresh();
        }

        private void updateGridValues()
        {
            label1.Text = Globals.currentSettings.imageMetrics.firstValue.ToString();
            label2.Text = Globals.currentSettings.imageMetrics.secondValue.ToString();
            label3.Text = Globals.currentSettings.imageMetrics.thirdValue.ToString();
            label4.Text = Globals.currentSettings.imageMetrics.fourthValue.ToString();
            label5.Text = Globals.currentSettings.imageMetrics.fifthValue.ToString();
            label6.Text = Globals.currentSettings.imageMetrics.sixthValue.ToString();
            label7.Text = Globals.currentSettings.imageMetrics.seventhValue.ToString();
            label8.Text = Globals.currentSettings.imageMetrics.eighthValue.ToString();

            label1.Refresh();
            label2.Refresh();
            label3.Refresh();
            label4.Refresh();
            label5.Refresh();
            label6.Refresh();
            label7.Refresh();
            label8.Refresh();
        }

        bool isLensArtifactMeasurement = false;
        private void ClearDisplayArea()
        {
            maskOverlay = new Bitmap(overLay_pbx.Width, overLay_pbx.Height);
            overLay_pbx.Image = maskOverlay;
            ODoverlay_pbx.Image = maskOverlay;
        }
        private void displayImage(string n, Args arg)
        {
            displayWindow_pbx.Image = Globals.whiteBmp;
        }
        bool mouseDrag = false;

        private void DrawCoordinates(string n ,Args arg)
        {
             gridLines = Graphics.FromImage(overlayImage);
             gridLines.FillRectangle(Brushes.Black, new Rectangle(0, 0, overlayImage.Width, overlayImage.Height));
             if (Globals.isTopCord)
             {
                 if (mouseDrag)
                 {
                     Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.ClassicSettings.LensArtifactTopX, xCord);
                     Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.ClassicSettings.LensArtifactTopY, yCord);
                     Globals.WLRoiRectLAcoordinates = new Rectangle(xCord, yCord, 200, 200);
                     //facade.Publish("LAchangeCoordinates");
                 }
                 else
                 {
                 xCord = (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.ClassicSettings.LensArtifactTopX);
                 yCord = (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.ClassicSettings.LensArtifactTopY);
                 }
                 gridLines.DrawLine(new Pen(Color.FromArgb(254, 0, 0), 3.0f), new Point((xCord)-50 , yCord), new Point((xCord)+50, yCord ));
                 gridLines.DrawLine(new Pen(Color.FromArgb(254, 0, 0), 3.0f), new Point(xCord , (yCord)-50), new Point(xCord,(yCord) + 50));
             }
             else
             {
                 if (mouseDrag)
                 {
                     Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.ClassicSettings.LensArtifactBottomX, xCord);
                     Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.ClassicSettings.LensArtifactBottomY, yCord);
                     Globals.WLRoiRectLAcoordinates = new Rectangle(xCord, yCord, 200, 200);
                     //facade.Publish("LAchangeCoordinates");
                 }
                 else
                 {
                 xCord = (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.ClassicSettings.LensArtifactBottomX);
                 yCord = (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.ClassicSettings.LensArtifactBottomY);
                 }
                 gridLines.DrawLine(new Pen(Color.FromArgb(0, 254, 0), 3.0f), new Point(xCord - 50, yCord ), new Point(xCord + 50, yCord ));
                 gridLines.DrawLine(new Pen(Color.FromArgb(0, 254, 0), 3.0f), new Point(xCord , yCord - 50), new Point(xCord,yCord + 50));
             }
            overlayImage.MakeTransparent(Color.Black);
            overLay_pbx.Image = overlayImage;
            overLay_pbx.Refresh();
        }
        private void overLay_pbx_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDrag && isLensArtifactMeasurement)
            {
                if (!Globals.isSetLACord)
                {
                    if (Globals.isHighResolution)
                    {
                        prevroiRect.X = Globals.WLRoiRect.X;
                        prevroiRect.Y = Globals.WLRoiRect.Y;
                        float x = width / pbWidth;
                        x = e.X * x;
                        Globals.WLRoiRect.X = (int)x + 100;
                        float y = height / pbHeight;
                        y = e.Y * y;
                        Globals.WLRoiRect.Y = (int)y - 100;
                    }
                    else
                    {
                        prevroiRect.X = Globals.IRroiRect.X;
                        prevroiRect.Y = Globals.IRroiRect.Y;
                        float x = width / pbWidth;
                        x = e.X * x;
                        Globals.IRroiRect.X = (int)x + 100;
                        float y = height / pbHeight;
                        y = e.Y * y;
                        Globals.IRroiRect.Y = (int)y - 100;
                    }
                    drawGridLines();
                    if (!tricam.isLiveMode())
                        facade.Publish(facade.SetAoiLensArtifactMeasurement, new Args());
                }
                else
                {
                    float x = width / pbWidth;
                    x = e.X * x;
                    xCord = (int)x;
                    float y = height / pbHeight;
                    y = e.Y * y;
                    yCord = (int)y;
                    facade.Publish(facade.LAchangeCoordinates, null);
                    facade.Publish(facade.SET_LACoordinates, null);
                }
               
            }
        }

        private void overLay_pbx_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDrag = false;
        }

        private void overLay_pbx_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDrag = true;
        }

        private void drawGridLines()
        {
            if (gridLines != null)
                gridLines.Dispose();
            gridLines = Graphics.FromImage(overlayImage);
            gridLines.DrawRectangle(new Pen(Color.Black, 2.0f), prevroiRect);
            if (Globals.isHighResolution)
            {
                gridLines.DrawRectangle(new Pen( Color.FromArgb(254, 0, 0), 2.0f), Globals.WLRoiRect);
            }
            else
            {
                gridLines.DrawRectangle(new Pen(Color.FromArgb(254, 0, 0), 2.0f), Globals.IRroiRect);
            }
            
            overlayImage.MakeTransparent(Color.Black);
            overLay_pbx.Image = overlayImage;
            overLay_pbx.Refresh();
        }
        Image<Bgr, byte> SegmentImg;
        private void SetAreaOfInterest()
        {
            try
            {
                if (Globals.isHighResolution)
                {
                    SegmentImg = new Image<Bgr, byte>(Globals.whiteBmp);
                    if (Globals.isTopCord || Globals.isBottomCord)
                        SegmentImg.ROI = Globals.WLRoiRectLAcoordinates;
                    else
                        SegmentImg.ROI = Globals.WLRoiRect;
                }
                else
                {
                    SegmentImg = new Image<Bgr, byte>(Globals.IrBmp);
                    if (Globals.isTopCord || Globals.isBottomCord)
                        SegmentImg.ROI = Globals.WLRoiRectLAcoordinates;
                    else
                        SegmentImg.ROI = Globals.IRroiRect;
                }
                if (Globals.ChannelImg != null)
                    Globals.ChannelImg.Dispose();
                if (Globals.ThreshImg != null)
                    Globals.ThreshImg.Dispose();
                if (Globals.RedChannelImg != null)
                    Globals.RedChannelImg.Dispose();

                Globals.ChannelImg = SegmentImg[1].Copy();
                Globals.RedChannelImg = SegmentImg[2].Copy();
                SegmentImg.ROI = new Rectangle();
                Globals.ThreshImg = Globals.ChannelImg.ThresholdBinary(new Gray(9), new Gray(255));
                Globals.ThreshImg = Globals.ThreshImg.SmoothMedian(3);
                Globals.RedChannelImg = Globals.RedChannelImg.ThresholdBinary(new Gray(12), new Gray(255));
                Globals.RedChannelImg = Globals.RedChannelImg.SmoothMedian(3);
                thresholded_pbx.Image = Globals.ThreshImg.ToBitmap();
                thresholded_pbx.Refresh();
            }
            catch(Exception ex)
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter("Calibration.log", true);

                string exception = DateTime.Now.ToString() + Environment.NewLine;
                exception += ex.Message + Environment.NewLine;
                exception += ex.StackTrace + Environment.NewLine;
                sw.WriteLine(exception);
                sw.Close();
            }
        }
        private void DisplayLACorrectedImage(string n, Args arg)
        {
            Image<Bgr, byte> temp1 = new Image<Bgr, byte>((Bitmap)arg["LACorrectedImage"]);
           if(Globals.isHighResolution)
            temp1.ROI = Globals.WLRoiRect;
           else
            temp1.ROI = Globals.IRroiRect;
            Globals.ChannelImg = temp1[0].Copy();
            Globals.RedChannelImg = temp1[2].Copy();
            Image<Gray, byte> temp2 = Globals.ChannelImg.Copy();
            temp2 = Globals.ChannelImg.ThresholdBinary(new Gray(9), new Gray(255));
            temp2 = temp2.SmoothMedian(3);
            Globals.RedChannelImg = Globals.RedChannelImg.ThresholdBinary(new Gray(12), new Gray(255));
            Globals.RedChannelImg = Globals.RedChannelImg.SmoothMedian(3);
            LA_AfterCorrection_pbx.Image = temp2.ToBitmap();
            temp1.ROI = new Rectangle();
            temp1.Dispose();

        }
        private void SetAOILensArtifact(string s, Args arg)
        {
            SetAreaOfInterest();
        }
        //to display "capturing" string on the display area.
        private void onRefractoCaptureStart(String n, Args arg)
        {
            Graphics g = Graphics.FromImage(maskOverlay);
            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, maskOverlay.Width, maskOverlay.Height));
            g.DrawString("Capturing..", new Font("Arial", 24), Brushes.Green, new PointF((overLay_pbx.Width / 2) - 100, overLay_pbx.Height / 2));
            maskOverlay.MakeTransparent(Color.Black);
            overLay_pbx.Image = maskOverlay;
            overLay_pbx.BackColor = Color.Transparent;
            overLay_pbx.Refresh();
            g.Dispose();
        }

        private bool focused = false;
        private string direction = "";
        private int totalSpots = 0;
        private int readyState = 0;
        //to display captured text on display area
        private void onRefractoCaptureComplete(String n, Args arg)
        {
            if (showLensN0)
            {
                lensIdx++;
                //Globals.LensID = lensIdx;
            }
            overLay_pbx.Visible = true;
            //to show lens to be kept check IsR1R2 and startCalib Button
            if (Globals.IsR1R2Mode && Globals.CalibStart)
            {
                showLensN0 = true;
            }
        }
        private void onRefractoCaptureCompleteInLive(String n, Args arg)
        {
            Forus.Refracto.RefractoReadings reading = arg["reading"] as Forus.Refracto.RefractoReadings;
            powerLive = "SPH : " + reading.SPH.ToString("0.00") + Environment.NewLine + "CYL : " + reading.CYL.ToString("0.00") + Environment.NewLine + "AXIS : " + (((reading.AXIS))%180).ToString() + Environment.NewLine + "Diff : " + Math.Abs((reading.SPH - reading.CYL)).ToString("0.00");
        
        }
        private void drawPower(ref Graphics g)
        {
            SolidBrush sB = new SolidBrush(Color.FromArgb(254, 0, 0));
            if (Globals.IsLiveCalib)
            {
                g.DrawString(powerLive, new Font("Arial", 25), sB, new PointF((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX) - 40, 40));// Globals.DisplayCenter.DisplayCenter_Y + 150));
            }
        }
        private void standBy(String n, Args args)
        {
            removeMouseEvents();
            attachMouseEvents();
            overLay_pbx.Visible = false;
        }
        private void removeMouseEvents()
        {
            displayWindow_pbx.MouseWheel -= mouse_wheel_handler;
            displayWindow_pbx.MouseEnter -= mouse_enter_handler;
            displayWindow_pbx.MouseMove -= mouse_move_handler;
            displayWindow_pbx.MouseDown -= mouse_down_handler;
            displayWindow_pbx.MouseUp -= mouse_up_handler;
            displayWindow_pbx.DoubleClick -= displayWindow_pbx_double_click_handler;
            this.DoubleClick -= this_double_click_handler;

            overLay_pbx.MouseWheel -= mouse_wheel_handler;
            overLay_pbx.MouseEnter -= mouse_enter_handler;
            overLay_pbx.MouseMove -= mouse_move_handler;
            overLay_pbx.MouseDown -= mouse_down_handler;
            overLay_pbx.MouseUp -= mouse_up_handler;
            overLay_pbx.DoubleClick -= displayWindow_pbx_double_click_handler;
            this.DoubleClick -= this_double_click_handler;
        }
        private void attachMouseEvents()
        {
            overLay_pbx.MouseWheel += mouse_wheel_handler;
            displayWindow_pbx.MouseWheel += mouse_wheel_handler;
            displayWindow_pbx.MouseEnter += mouse_enter_handler;
            displayWindow_pbx.MouseMove += mouse_move_handler;
            displayWindow_pbx.MouseDown += mouse_down_handler;
            displayWindow_pbx.MouseUp += mouse_up_handler;
            displayWindow_pbx.DoubleClick += displayWindow_pbx_double_click_handler;
            this.DoubleClick += this_double_click_handler;
        }
        //To display sph,cyl and axis values on display area
        private void onRefractionCalculation(String n, Args arg)
        {
            overLay_pbx.Visible = true;
            Forus.Refracto.RefractoReadings reading = arg["readings"] as Forus.Refracto.RefractoReadings;
            if(!Globals.IsR1R2Mode)
                power = "SPH : " + reading.SPH.ToString("0.00") + Environment.NewLine + "CYL : " + reading.CYL.ToString("0.00") + Environment.NewLine + "AXIS : " + reading.AXIS.ToString();
            else
            power = "SPH : " + reading.SPH.ToString("0.00") + Environment.NewLine + "CYL : " + reading.CYL.ToString("0.00") + Environment.NewLine + "AXIS : " + (((reading.AXIS))%180).ToString() + Environment.NewLine + "Diff : " + Math.Abs((reading.SPH - reading.CYL)).ToString("0.00"); ;
            
            Graphics g = Graphics.FromImage(maskOverlay);
            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, maskOverlay.Width,maskOverlay.Height));
            if (reading.IsProperRing)
            {
                g.DrawString(power, new Font("Arial", 24), Brushes.Green, new PointF(0, 50));
            }
            else if(Globals.IsR1R2Mode == false)
            {
                if (!Globals.FileNotFound)
                {
                    g.DrawString(("Retake the image"),
                        new Font("Arial", 24), Brushes.Red, new PointF((overLay_pbx.Width / 2) - 300, overLay_pbx.Height / 2));
                }
            }
            maskOverlay.MakeTransparent(Color.Black);
            overLay_pbx.Image = maskOverlay;
            overLay_pbx.BackColor = Color.Transparent;
            overLay_pbx.Refresh();
            g.Dispose();
        }
       //to clear all the readings in the refracto table.
        private void Clear_btn_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }
        private void onFocusKnobChanged(String n, Args arg)
        {
            Graphics g = Graphics.FromImage(maskOverlay);
            focused = (bool)arg["focusknobstatus"];
            DrawFocusStatus(ref g);
        }
        private void onSpotsDetection(String n, Args arg)
        {
            drawSpotIndication();
        }
        Color tempClr = Color.FromArgb(255, 0, 148, 255);
        Pen p;
        private void drawSpotIndication()
        {
            
            Graphics g = Graphics.FromImage(maskOverlay);
            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, maskOverlay.Width, maskOverlay.Height));
            p = new Pen(tempClr, 2.0f);
            //bool isFocusKnobAligned = mycamera.IsFocusKnobAligned();
            //Args args = new Args();
            //args["focusknobstatus"] = isFocusKnobAligned;
            //facade.Publish(facade.REFRACTO_FOCUSKNOB_STATUS, args);
            //if (!focused)
            //{
            //    //SolidColorBrush brush = new SolidColorBrush( myColor );
            //    SolidBrush sB = new SolidBrush(Color.FromArgb(254, 0, 0));

            //    g.DrawString("Not Focussed", new Font("Arial", 16),
            //       sB, new PointF(Globals.currentSettings.royalSettings.DisplayCenter_X - 40, Globals.currentSettings.royalSettings.DisplayCenter_Y + 250));
            //}
            g.DrawLine(p, new Point(0, (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreY) - 170), new Point(1024, (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreY) - 170));
            g.DrawLine(p, new Point(0, (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreY) + 90), new Point(1024, (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreY) + 90));


            g.DrawLine(p, new Point((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX)- 435, 0), new Point((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX) - 435, 768));
            g.DrawLine(p, new Point((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX) - 255, 0), new Point((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX) - 255, 768));

            g.DrawLine(p, new Point((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX) + 250, 0), new Point((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX) + 250, 768));
            g.DrawLine(p, new Point((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX) + 430, 0), new Point((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX) + 430, 768));
            // Added to display the lens placement during refracto calibration added by sriram from the trisoft 3.0 branch.
            DrawCalibLens(ref g);
            maskOverlay.MakeTransparent(Color.Black);
            overLay_pbx.Image = maskOverlay;
            overLay_pbx.BackColor = Color.Transparent;
            overLay_pbx.Visible = true;
            g.Dispose();
        }
        Bitmap upArrow;
        Bitmap downArrow;
        Rectangle upRect = new Rectangle(500, 100, 30, 75);
        Rectangle downRect = new Rectangle(500, 625, 30, 75);
        private void DrawDirection(ref Graphics g)
        {
            if (direction == "READY")
            {
                g.FillEllipse(Brushes.White, new Rectangle((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX) - 5, (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreY) - 5, 10, 10));
 switch (readyState)
                {
                    case 1:
                        g.DrawEllipse(Pens.White, new Rectangle((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX) - 10, (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreY) - 10, 20, 20));
                        break;
                    case 2:
                        g.DrawEllipse(Pens.White, new Rectangle((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX) - 10, (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreY) - 10, 20, 20));
                        g.DrawEllipse(Pens.White, new Rectangle((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX) - 15, (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreY) - 15, 30, 30));
                        break;
                    case 3:
                        g.DrawEllipse(Pens.White, new Rectangle((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX) - 10, (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreY) - 10, 20, 20));
                        g.DrawEllipse(Pens.White, new Rectangle((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX) - 15, (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreY) - 15, 30, 30));
                        g.DrawEllipse(Pens.White, new Rectangle((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX) - 20, (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreY) - 20, 40, 40));
                        readyState = 0;
                        break;
                }
            }        
        }
        private void DrawFocusStatus(ref Graphics g)
        {
            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, maskOverlay.Width, maskOverlay.Height));
            if(!focused)
            {
                SolidBrush sB = new SolidBrush(Color.FromArgb(254, 0, 0));

                g.DrawString("Not Focussed", new Font("Arial", 16),
                   sB, new PointF((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX) - 40, (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreY) + 250));
            }
           
            //drawPower(ref g);
            maskOverlay.MakeTransparent(Color.Black);
            overLay_pbx.Image = maskOverlay;
            overLay_pbx.BackColor = Color.Transparent;
            overLay_pbx.Visible = true;
            g.Dispose();
        }
        SolidBrush sB = new SolidBrush(Color.FromArgb(254, 0, 0));
        double[] CalibLenses = { -12.0 ,- 6.0, -3.0, 0,+3.0,+6.0,+12.0 };
        //double[] CalibLenses = {-12.0, -11, -10, -9.0, -8.0, -7.0, -6.0, -5.5, -5.0, -4.5 ,- 4.0, -3.5, -3.0, -2.5, -2.0, -1.5, -1.0, -0.5,
        //    0, +0.5, +1.0, +1.5, +2.0, +2.5, +3.0, +3.5, +4.0, +4.5, +5.0, +5.5, +6.0, +7.0, +8.0, +9.0, +10.0, +11.0, +12.0};
        public int lensIdx = 0;
        bool showLensN0 = false;
        //To display lens numbers on display area during callibration
        private void DrawCalibLens(ref Graphics g)
        {
            if (lensIdx >= CalibLenses.Length)
            {
                showLensN0 = false;
                lensIdx = 0;
                return;
            }
            if (Globals.IsR1R2Mode && showLensN0)
            {
                if (!Globals.RingRetake)
                {
                    if (CalibLenses[lensIdx] == CalibLenses[6])
                    {
                        Globals.CalibOver = true;
                    }
                    g.DrawString("please keep " + CalibLenses[lensIdx].ToString() + " diopter", new Font("Arial", 22),
                        sB, new PointF(maskOverlay.Width / 2 - 120, 100));
                }
                else
                    lensIdx--;
                    Globals.RingRetake = false;
            }
        }
        //To display retake message on display area
        private void RefractoRetake(string n, Args arg)
        {
            Graphics g = Graphics.FromImage(maskOverlay);
            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, maskOverlay.Width, maskOverlay.Height));
            g.DrawString("Refracto Capture Failed, Please retake", new Font("Arial", 22),
                   sB, new PointF(maskOverlay.Width / 2 - 120, 100));
            maskOverlay.MakeTransparent(Color.Black);
            overLay_pbx.Image = maskOverlay;
            overLay_pbx.BackColor = Color.Transparent;
            overLay_pbx.Visible = true;
            g.Dispose();
        }
    }
}

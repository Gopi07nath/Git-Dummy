// $URL: https://forussrv01/svn/CalibrationFundusImages/trunk/Calibration/calibrationUI.cs $
// $LastChangedBy: Shivaraj $ on $LastChangedDate: 2016-09-16 14:54:29 +0530 (Fri, 16 Sep 2016) $  
//$Rev: 261 $
/* **********************************************************************************
 * File name: calibrationUI.cs
 *
 *  
 *This class is used in CalibrationForFundusImages to Calibrate the thrinethra device for the fundus imaging,Where in the IR light,White light are
 *calibrated for the following metrics Average Intensity, Periphery to Inner Variation and Top to bottom variation.
 * A report of the same is generated and saved in a file. The class takes a device id as the only input and creates a directory of the same
 * name in C:/CalibrationImages.
 * Author: Sriram
 * Last modifed: 
 * 
 *************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Reflection;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.Util;
using System.IO;
using Emgu.CV.UI;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using ReportGlobals;
using System.Xml.Serialization;
using System.Xml;
using Forus.Refracto;
using System.Text.RegularExpressions;
using Nvram;
using System.Drawing.Drawing2D;

namespace Calibration
{
    public partial class calibrationUI : Form
    {
        #region ////----Declarations----////
        string SaveReportfileName = "";
        string SaveReportfileNameMerged = "";
        DirectoryInfo dirInf;
        Facade f;
        Tricam tricam;
        Alignment alignment;
        ClassicRoyal classicRoyal;
        loginform loginForm;
        DeviceId deviceIDPage;
        AlignmentUI alignmentui;
        MemoryLightTriggerTestUI memoryTest;
        CameraAlignment cameraAlignment;
        RefractoRingCalculation refractoRingCalculation;
        LensArtifactUI lensArtifact;
        RefractoCalibration refractoCalibration;
        PdfDocument doc = new PdfDocument();
        PdfPage page;
        XGraphics xg;
        XGraphics xg1;
        XGraphics gfx;
        int xCor = 0;
        int controlsCount = 0;
        string csvFileName = "";
        string csvFileNameMerged = "";
        XProperties xp = new XProperties();
        Args args;
        string formText = "";

        Globals.mode[] ProductionFlow, ServiceFlow, QaFlow, DistributorFlow;
        Globals.mode[] ApplicationFlow;
        Globals.ApplicationFlow appFlow;
        List<string> ReportRowNames;
        List<string> ReportRecommendedRange;
        List<string> ReportStageName;
        List<object> ReportValues;
        List<Color> ReportTextColors;
        List<Globals.mode> ReportSaved;
        List<Globals.mode> ReportSavedZero;
        Bitmap cameraConnectedImage, cameraDisconnectedImage;

        #endregion


        public calibrationUI()
        {
            InitializeComponent();

            #region Facade subscriptions
            f = Facade.getInstance();
            f.Subscribe(f.Set_ClassicRoyalPage, new NotificationHandler(setRoyalClassicPage));
            f.Subscribe(f.DeviceID_Page, new NotificationHandler(setDeviceIdPage));
            f.Subscribe(f.setControls, new NotificationHandler(SetControls));
            f.Subscribe(f.ApplicationMode, new NotificationHandler(SetApplicationMode));
            f.Subscribe(f.SetCurrentMode, new NotificationHandler(setCurrentMode));
            f.Subscribe(f.CAMERA_CONNECTED, new NotificationHandler(NewCameraConnected));
            f.Subscribe(f.CAMERA_DISCONNECTED, new NotificationHandler(CameraDisconnected));
            f.Subscribe(f.REFRACTO_CAPTURE_COMPLETE, new NotificationHandler(onRefractoCaptureComplete));
            f.Subscribe(f.REFRACTO_CAPTURE, new NotificationHandler(ringCapture));
            f.Subscribe(f.Save_Image, new NotificationHandler(saveImage));
            f.Subscribe(f.DisableBrowseBtn, new NotificationHandler(enableOrDisableBrowseBtn));


            #endregion

            ProductionFlow = new Globals.mode[7] { Globals.mode.classicRoyalPage, Globals.mode.DeviceIdPage, Globals.mode.isMemoryTest,Globals.mode.isCameraAlignment, Globals.mode.isFundusAlignment, Globals.mode.isRefractoCalibration, Globals.mode.isLensArtifactMeasurement };
            ReportSaved = new List<Globals.mode>();
            ReportSavedZero = new List<Globals.mode>();
            ReportStageName = new List<string>();
            #region  //To be used for the future for scope management
            // ProductionFlow = new Globals.mode[8] { Globals.mode.classicRoyalPage, Globals.mode.DeviceIdPage, Globals.mode.isMemoryTest, Globals.mode.isCameraAlignment, Globals.mode.isFundusAlignment, Globals.mode.isLensArtifactMeasurement, Globals.mode.isRefractoAlignment, Globals.mode.isRefractoCalibration };
            // ServiceFlow = new Globals.mode[8] { Globals.mode.classicRoyalPage, Globals.mode.DeviceIdPage, Globals.mode.isMemoryTest, Globals.mode.isCameraAlignment, Globals.mode.isFundusAlignment, Globals.mode.isLensArtifactMeasurement, Globals.mode.isRefractoAlignment, Globals.mode.isRefractoCalibration };
            //QaFlow = new Globals.mode[5] { Globals.mode.classicRoyalPage, Globals.mode.DeviceIdPage,Globals.mode.isFundusAlignment,Globals.mode.isLensArtifactMeasurement,Globals.mode.isRefractoCalibration};
            //DistributorFlow = new  Globals.mode[1]{ Globals.mode.isRefractoCalibration};
            #endregion

            #region///-------Image initialization-------///
            Globals.mask1mp = new Bitmap(@"Resources\Mask_1MP.bmp");
            Globals.mask3mp = new Bitmap(@"Resources\Mask_3MP.bmp");
            cameraConnectedImage = new Bitmap(@"Resources\GREENLED.png");
            cameraDisconnectedImage = new Bitmap(@"Resources\redLED.png");
            #endregion
            Globals.whiteBmp = new Bitmap(Globals.WlWidth, Globals.Wlheight);
            Globals.IrBmp = new Bitmap(Globals.IRWidth, Globals.IRheight);
            #region///-----Initialize camera-----///
            tricam = Tricam.createTricam();
            tricam.container_handle = this.Handle;
            tricam.picture_box = displayArea1.displayWindow_pbx;
            tricam.myCamera = Camera.getInstance(tricam.container_handle, tricam.picture_box.Handle, eventhandler);
            #endregion


            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            ReportRowNames = new List<string>();
            ReportRecommendedRange = new List<string>();
            ReportValues = new List<object>();
            Globals.initSettings();

            
        }

        XmlDocument xmlDoc;
        XmlElement xmlRoot;
        /// <summary>
        /// 
        /// </summary>
        private void LoadTempReportData()
        {
            DirectoryInfo dirInf = new DirectoryInfo(Path.GetDirectoryName( Application.ExecutablePath));
            
            string reportName = "";
            FileInfo[] finfArr  =    dirInf.GetFiles("*.xml");
            foreach (FileInfo item in finfArr)
            {
                string xmlName = item.Name;
                string[] xmlNameArr = xmlName.Split('.');
                xmlNameArr = xmlNameArr[0].Split('_');
                if (xmlNameArr[xmlNameArr.Length-1] == Globals.DeviceId)
                {
                    reportName = xmlName;
                    break;
                }
            }

            if (File.Exists(reportName))
            {
            xmlDoc = new XmlDocument();
            //xmlDoc.Load("reportData_" + Globals.DeviceId + ".xml");
            XmlReader xmlStreamReader = XmlReader.Create(reportName);
            //xmlRoot = xmlDoc.DocumentElement;
            //string a = xmlRoot.GetElementsByTagName("DisplayCenter_X")[0].InnerText;
            //refractoCalibration.DisplayCenter_X_UpDown.Value = Convert.ToDecimal(a);
            XmlSerializer xmlSerialization = new XmlSerializer(typeof(Globals.Settings));
            Globals.currentSettings = (Globals.Settings)xmlSerialization.Deserialize(xmlStreamReader);
            xmlStreamReader.Close();
            Globals.currentSettings.memoryTestStruct.LUXvalue = 0;
            }
        }

        private void getCameraID()
        {
            if (Globals.isCameraConnected)
            {
                uEye.CAMINFO cameraInfo = tricam.getCameraID();
                string serialNumber = cameraInfo.SerNo;
                string[] temp = serialNumber.Split('\0');
                Globals.cameraID = temp[0];
                cameraID_lab.Visible = true;
                CameraID_Value_lab.Visible = true;
                CameraID_Value_lab.Text = Globals.cameraID;
                CameraID_Value_lab.Refresh();
            }
            else
            {
                cameraID_lab.Visible = false;
                CameraID_Value_lab.Visible = false;
                CameraID_Value_lab.Refresh();
            }
        }

        private void saveTempReportData()
        {
            xmlDoc = new XmlDocument();
            Globals.currentSettings.deviceId = Globals.DeviceId;
            {
                MemoryStream memStream = new MemoryStream();

                XmlWriter xmlStreamWriter = XmlWriter.Create(memStream);
                XmlSerializer xmlSerialization = new XmlSerializer(typeof(Globals.Settings));
                xmlSerialization.Serialize(xmlStreamWriter, Globals.currentSettings);
                memStream.Position = 0;
                xmlDoc.Load(memStream);
                xmlDoc.Save("reportData_"+Globals.DeviceId+".xml");
                xmlStreamWriter.Close();
                memStream.Dispose();
            }
        }
        private void eventhandler(object sender, EventArgs e)
        {
            int x = 0;
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (tricam != null)
            {
                tricam.CallWndProc(ref m);
            }
            base.WndProc(ref m);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            f.Publish(f.Close_Camera, args);
            this.Dispose();
        }

        private void calibrationUI_Shown(object sender, EventArgs e)
        {
        }
        int a = 0;
        bool keypressedW = false;
        int b = 0;
        bool keypressedI = false;
        string PassStr = "OK";
        string FailStr = "NOK";
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                    {
                        if (next_btn.Visible)
                            next_btn_Click(null, null);
                        break;
                    }

                case Keys.Left:
                    {
                        if (prev_btn.Visible)
                            prev_btn_Click(null, null);

                        break;
                    }
                case Keys.C:
                    {
                        // "C" alphabet is also using in Camera alignment to enter Grid value 
                        if (cameraAlignment == null)
                            cameraAlignment = new CameraAlignment();
                        if (connect_btn.Visible && !cameraAlignment.isTextEnteredInTB)
                        {
                            connect_btn_Click_1(null, null);
                        }
                        cameraAlignment.isTextEnteredInTB = false;
                        break;
                    }
                case Keys.Space:
                    {
                        if (Globals.currentMode == Globals.mode.isRefractoCalibration)
                            refractoCalibration.RingCapture_Click(null, null);
                        else if (Globals.currentMode == Globals.mode.isLensArtifactMeasurement)
                            lensArtifact.whiteLightCapture_btn_Click(null, null);
                        break;
                    }
                case Keys.W:
                    {
                        if (Globals.currentMode == Globals.mode.isFundusAlignment)
                        {
                            a++;
                            if (a < 2)
                            {
                                alignmentui.whiteLightCapture_btn_Click(null, null);
                            }
                            else
                            {
                                alignmentui.resume_btn_Click(null, null);
                                a = 0;
                            }
                        }
                        break;
                    }
                case Keys.I:
                    {
                        if (Globals.currentMode == Globals.mode.isFundusAlignment)
                        {
                            b++;
                            if (b < 2)
                                alignmentui.IrImageCapture_btn_Click(null, null);
                            else
                            {
                                alignmentui.resume_btn_Click(null, null);
                                b = 0;
                            }
                        }
                        break;

                    }


            }
            return false;
        }

        private void calibrationUI_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Control && e.Alt & e.KeyCode == Keys.N)
            {
                if(Globals.CameraInitialized)
                Globals.nvramHelper.ShowNvramDialog();
            }

        }

        private void connect_btn_Click_1(object sender, EventArgs e)
        {
            if (connect_btn.Text == "Connect")
            {
                if (Globals.currentMode == Globals.mode.isRefractoCalibration)
                    f.Publish(f.REFRACTO_NO_OF_SPOTS, null);
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
                Globals.CameraInitialized = tricam.open_camera(null, null);
                if (Globals.CameraInitialized && !Globals.isPowerFailure)
                {
                    connect_btn.Text = "Disconnect";
                    connect_btn.Refresh();
                    if (!saveReport_btn.Enabled)
                        saveReport_btn.Enabled = true;

                    if (Globals.currentMode == Globals.mode.isFundusAlignment || Globals.currentMode == Globals.mode.isLensArtifactMeasurement)
                    {
                        if (!browse_btn.Enabled)
                            browse_btn.Enabled = true;
                    }

                    f.Publish(f.SetCurrentMode, null);
                    f.Publish(f.Read_Nvram, null);
                    
                    string devID = (string)Globals.nvramHelper.GetNvramValue(NvramHelper.CommonSettings.DeviceId);
                    if (devID.Contains("XXXX"))
                        Globals.nvramHelper.SetNvramValue(NvramHelper.CommonSettings.DeviceId, Globals.DeviceId);
                    tricam.startLiveMode();
                    //Added because when user turns ON any light and then if user disconnects and connects the camera, it as to retain previous light status 
                    if (Globals.currentMode == Globals.mode.isMemoryTest)
                        f.Publish(f.ManageLightOnOff, null);
                }
                
            }
            else
            {
                Globals.CameraInitialized = false;
                tricam.stopLiveMode();
                f.Publish(f.Close_Camera,null);
                connect_btn.Text = "Connect";
                connect_btn.Refresh();
                saveReport_btn.Enabled = false;
                setPages();
               
            }
            args["page"] = ApplicationFlow[controlsCount];
            this.Controls_p.Enabled = Globals.CameraInitialized;
            Console.WriteLine("controls enabled" + this.Controls_p.Enabled.ToString());
        }

        //bool enableIlluminationGrid = false;
        private void browse_btn_Click_1(object sender, EventArgs e)
        {
            Cursor cur = Cursors.WaitCursor;
            tricam.stopLiveMode();
            OpenFileDialog ofd = new OpenFileDialog();
            string str = "";
            ofd.Filter = "PNG Files (*.png)|*.png|BMP Files (*.bmp)|*.bmp|JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|GIF Files (*.gif)|*.gif|CSV Files  (*.csv)|*.csv";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                str = ofd.FileName;
            }
            else
            {
                tricam.startLiveMode();
                return;
            }

            //Globals.isIlluminationGrid = false;
            //f.Publish(f.ManageIlluminationGrid,args) ; //  "ManageIlluminationGrid", args);

            ofd.RestoreDirectory = true;
            FileInfo finf = new FileInfo(str);
            string fileName = finf.Name;

            //calibrationUI.ActiveForm.Text = formText + "   " + str;
            Globals.browseBtnClicked = true;
            Bitmap bm = new Bitmap(str);
            displayArea1.displayWindow_pbx.Image = bm;
            displayArea1.displayWindow_pbx.Refresh();

            

            if (bm.Width == Globals.WlWidth)
            {
                Globals.whiteBmp = new Bitmap(bm);
                Globals.isHighResolution = true;
            }
            else
            {
                Globals.IrBmp = new Bitmap(bm);
                args["isIR"] = true;
                Globals.isHighResolution = false;
            }
            f.Publish(f.SetCurrentMode, args);

            Globals.isHighResolution = (bm.Width == Globals.WlWidth) ? true : false ;


            //if (bm.Width == Globals.WlWidth) 
            //{
            //   // Globals.whiteBmp = new Bitmap(bm);
            //    Globals.isHighResolution = true;
            //}
            //else
            //{
            //   // Globals.IrBmp = new Bitmap(bm);
            //   // args["isIR"] = true;
            //    Globals.isHighResolution = false;
            //}


            cur = Cursors.Default;
             //Globals.browseBtnClicked = false;
        }

        private void saveReport_btn_Click_1(object sender, EventArgs e)
        {
            //saveTempReportData();
            //LoadTempReportData();

            if (Globals.currentMode == Globals.mode.isFundusAlignment || Globals.currentMode == Globals.mode.isLensArtifactMeasurement)  // because resume button will be disabled at that time
            {
                if (!Globals.browseBtnClicked)
                {
                    args = new Args();
                    if (Globals.currentMode == Globals.mode.isFundusAlignment)
                    {
                        args["FundusPage"] = true;
                    }
                    else
                    {
                        args["FundusPage"] = false;
                    }
                    f.Publish(f.ManageControlsAfterSaveReport, args);
                }
            }

            saveReport();
            DirectoryInfo tempDirInf = new DirectoryInfo(dirInf.FullName + Path.DirectorySeparatorChar + "Report");
            if (!Directory.Exists(dirInf.FullName + Path.DirectorySeparatorChar + "Report"))
                tempDirInf = Directory.CreateDirectory(dirInf.FullName + Path.DirectorySeparatorChar + "Report");
            DirectoryInfo tempDir = new DirectoryInfo(Globals.calibrationPath + Path.DirectorySeparatorChar + Globals.DeviceId + Path.DirectorySeparatorChar + "Report");
            if (!Directory.Exists(Globals.calibrationPath + Path.DirectorySeparatorChar + Globals.DeviceId + Path.DirectorySeparatorChar + "Report"))
                tempDir = Directory.CreateDirectory(Globals.calibrationPath + Path.DirectorySeparatorChar + Globals.DeviceId + Path.DirectorySeparatorChar + "Report");
            SaveReportfileName = tempDirInf.FullName + Path.DirectorySeparatorChar + Globals.DeviceId.ToString() + "_" + "_" + DateTime.Now.ToString("HHmmss");
            createReport();
            //if (Globals.currentMode == Globals.mode.isFundusAlignment || Globals.currentMode == Globals.mode.isLensArtifactMeasurement)  // because resume button will be disabled at that time
            //{
            //    args = new Args();
            //    if (Globals.currentMode == Globals.mode.isFundusAlignment)
            //    {
            //        args["FundusPage"] = true;
            //    }
            //    else
            //    {
            //        args["FundusPage"] = false;
            //    }
            //    f.Publish(f.ManageControlsAfterSaveReport, args);
            //    tricam.startLiveMode();
            //}
        }

        private void exit_btn_Click(object sender, EventArgs e)
        {
            f.Publish(f.Write_Nvram, null);
            if (Globals.CameraInitialized)
            {
                string str = Globals.nvramHelper.GetNvram();
                Globals.DeviceId = Convert.ToString(Globals.nvramHelper.GetNvramValue(NvramHelper.CommonSettings.DeviceId));
                StreamWriter stWriter = new StreamWriter("Nvram_" + Globals.DeviceId + ".txt");
                stWriter.WriteLine(str);
                stWriter.Close();
                stWriter.Dispose();
                if (!string.IsNullOrEmpty(Globals.SaveImagePath))
                    File.Copy("Nvram_" + Globals.DeviceId + ".txt", Globals.SaveImagePath + Path.DirectorySeparatorChar + "Nvram_" + Globals.DeviceId + ".txt", true);

            }
            saveTempReportData();
            f.Publish(f.Close_Camera, args);
            this.Close();
            this.Dispose();
        }

        private bool checkDeviceID()
        {
            if (Globals.DeviceId.IndexOf("X") != -1 || !deviceIDPage.checkDeviceID())
            {
                controlsCount--;
                MessageBox.Show("Enter Device ID", "Callibration Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void next_btn_Click(object sender, EventArgs e)
        {
            if (next_btn.Enabled)
            {
                controlsCount++;
                if (controlsCount < ApplicationFlow.Length)
                {
                    //Added by shivaraj - If classic then Refracto Calibration window should not 
                    if (controlsCount == 5 && !Globals.isRoyal)
                    {
                        controlsCount++;
                    }

                    if (controlsCount == ApplicationFlow.Length - 1)
                        next_btn.Enabled = false;

                    if (!prev_btn.Enabled)
                        prev_btn.Enabled = true;

                    if (controlsCount == 2)
                    {
                        if (!checkDeviceID())
                            return;
                    }

                    if (tricam.isLiveMode())
                    {
                        tricam.stopLiveMode();
                        if(Globals.currentMode == Globals.mode.isMemoryTest || Globals.currentMode == Globals.mode.isCameraAlignment || Globals.currentMode == Globals.mode.isFundusAlignment ||Globals.currentMode == Globals.mode.isRefractoCalibration|| Globals.currentMode == Globals.mode.isLensArtifactMeasurement)
                        PromptDialogForSaveReport();
                    }
                    setPages();
                }
                else
                {
                    if (next_btn.Enabled)
                    {
                        next_btn.Enabled = false;
                        controlsCount = ApplicationFlow.Length - 1;

                        setPages();
                    }
                }
            }
        }

        private void prev_btn_Click(object sender, EventArgs e)
        {
            controlsCount--;

            if (controlsCount > 0)
            {
                if (controlsCount == 5 && !Globals.isRoyal)
                {
                    controlsCount--;
                }

                if (!next_btn.Enabled)
                    next_btn.Enabled = true;
                if (tricam.isLiveMode())
                {
                    tricam.stopLiveMode();
                    if (Globals.currentMode == Globals.mode.isMemoryTest || Globals.currentMode == Globals.mode.isCameraAlignment || Globals.currentMode == Globals.mode.isFundusAlignment || Globals.currentMode == Globals.mode.isRefractoCalibration || Globals.currentMode == Globals.mode.isLensArtifactMeasurement)
                    PromptDialogForSaveReport();
                }
                if (Globals.currentMode == Globals.mode.isLensArtifactMeasurement)
                    Globals.isHighResolution = false;
                setPages();
            }
            else
            {
                prev_btn.Enabled = false;
                controlsCount = 0;
                setPages();
            }

        }

        private void calibrationUI_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);
            args = new Args();
            args["ApplicationMode"] = Globals.ApplicationFlow.production;
            f.Publish(f.ApplicationMode, args);
            prev_btn.Enabled = false;
            //setPages();

        }

        private void MakeDisplayControlAreaVisible(bool enable)
        {
            splitContainer1.Visible = enable;
            this.panel1.Refresh();
        }

        private void NewCameraConnected(string s, Args arg)
        {
            connect_btn.Enabled = true;
        }

        private void CameraDisconnected(string s, Args arg)
        {
            connect_btn.Text = "Connect";
            connect_btn.Enabled = false;
            saveReport_btn.Enabled = false;
            this.Refresh();
        }

        /// <summary>
        /// This function creates a collated report of the assembly
        /// </summary>
        private void createReport()
        {
            doc = new PdfDocument();
            csvFileName = SaveReportfileName + ".pdf";
            int count = 0;
            string csvFileNameMerged = SaveReportfileNameMerged + ".pdf";
            if (ReportSaved.Count == 0)
            {
                if (Globals.currentSettings.memoryTestStruct.isMemoryTestStructureSaved || Globals.currentMode == Globals.mode.isMemoryTest)
                {
                    if (!ReportStageName.Contains("Memory Light Test"))
                        ReportStageName.Add("Memory Light Test");
                    if (!ReportSavedZero.Contains(Globals.mode.isMemoryTest))
                    ReportSavedZero.Add(Globals.mode.isMemoryTest);
                }
                if (Globals.currentSettings.cameraAlignment.isCameraAlignmentSaved || Globals.currentMode == Globals.mode.isCameraAlignment)
                {
                    if (!ReportStageName.Contains("Camera Alignment"))
                        ReportStageName.Add("Camera Alignment");
                    if (!ReportSavedZero.Contains(Globals.mode.isCameraAlignment))
                        ReportSavedZero.Add(Globals.mode.isCameraAlignment);
                }
                if (Globals.currentSettings.royalSettings.isRoyalSettingsSaved || Globals.currentMode == Globals.mode.isRefractoCalibration)
                {
                    if (!ReportStageName.Contains("Refracto Calibration"))
                        ReportStageName.Add("Refracto Calibration");
                    if (!ReportSavedZero.Contains(Globals.mode.isRefractoCalibration))
                        ReportSavedZero.Add(Globals.mode.isRefractoCalibration);
                }
                if (Globals.currentSettings.laMetrics.isLAMetricsSaved || Globals.currentMode==Globals.mode.isLensArtifactMeasurement)
                {
                    f.Publish(f.LASaveReport, args);
                    if (!ReportStageName.Contains("Lens Artifact Measurement"))
                        ReportStageName.Add("Lens Artifact Measurement");
                    if (!ReportSavedZero.Contains(Globals.mode.isLensArtifactMeasurement))
                    ReportSavedZero.Add(Globals.mode.isLensArtifactMeasurement);
                }
                if (Globals.currentSettings.imageMetrics.isImageMetricsSaved ||Globals.currentMode == Globals.mode.isFundusAlignment)
                {
                    f.Publish(f.AlignmentSaveReport, args);
                    if (!ReportStageName.Contains("Fundus Alignment"))
                        ReportStageName.Add("Fundus Alignment");
                    if(!ReportSavedZero.Contains(Globals.mode.isFundusAlignment))
                    ReportSavedZero.Add(Globals.mode.isFundusAlignment);
                }
                
                foreach(Globals.mode val in ReportSavedZero)
                {
                    SetReportRowNames(val);
                    SetReportValues(val);
                    SetReportRecommendedRange(val);
                    write2Pdf(ReportStageName[count]);
                    count++;
                }
            }
            else
            {
                foreach (Globals.mode val in ReportSaved)
                {
                    if (val != Globals.mode.isCameraAlignment)
                    {
                        SetReportRowNames(val);
                        SetReportValues(val);
                        SetReportRecommendedRange(val);
                        if (ReportStageName.Count > 0)
                        {
                            write2Pdf(ReportStageName[count]);
                            count++;
                        }
                    }
                    else
                    {
                        saveGridValues();
                        count++;
                    }
                }
            }
            if (doc.PageCount>0)
            doc.Save(csvFileName);
            doc.Close();
            doc.Dispose();
            if(File.Exists(csvFileName))
            System.Diagnostics.Process.Start(@csvFileName);
        }

        private void setPages()
        {
            Globals.currentMode = ApplicationFlow[controlsCount];
            f.Publish(f.SET_DISPLAY_AREA, args);
            f.Publish(f.setControls, args);
        }

        private void setRoyalClassicPage(string s, Args arg)
        {

            if (classicRoyal == null)
            {
                classicRoyal = new ClassicRoyal();
                classicRoyal.Dock = DockStyle.Fill;
            }
            if (this.panel1.Controls.Contains(loginForm))
                this.panel1.Controls.Remove(loginForm);
            if (this.panel1.Controls.Contains(deviceIDPage))
                this.panel1.Controls.Remove(deviceIDPage);
            this.panel1.Controls.Add(classicRoyal);
            this.commonControls_p.Visible = true;
            nextPrev_p.Visible = true;
            tricam.open_camera(null, null);
            if(Globals.nvramHelper!=null)
            Globals.DeviceId =(string) Globals.nvramHelper.GetNvramValue(NvramHelper.CommonSettings.DeviceId);
          //  f.Publish(f.EXIT_CAMERA, null);
            f.Publish(f.Close_Camera, null);
            LoadTempReportData();

            //In device selection page, Camera ID related controls are made visible false
            CameraID_Value_lab.Visible = false;
            cameraID_lab.Visible = false;

        }

        private void setDeviceIdPage(string s, Args arg)
        {

            if (this.panel1.Controls.Contains(classicRoyal))
                this.panel1.Controls.Remove(classicRoyal);
            this.commonControls_p.Visible = true;
            this.connectControls_p.Visible = false;
            if (deviceIDPage == null)
            {
                deviceIDPage = new DeviceId();
                deviceIDPage.Dock = DockStyle.Fill;
            }
            this.panel1.Controls.Add(deviceIDPage);
            next_btn.TabStop = false;
            exit_btn.TabStop = false;

            
        }

        private void SetControls(String n, Args args)
        {
            switch (Globals.currentMode)
            {

                case Globals.mode.classicRoyalPage:
                    {
                        StageName_lbl.Text = "";
                        MakeDisplayControlAreaVisible(false);

                        f.Publish(f.Set_ClassicRoyalPage, new Args());
                        break;
                    }

                case Globals.mode.DeviceIdPage:
                    {
                        StageName_lbl.Text = "";
                        MakeDisplayControlAreaVisible(false);
                        Globals.currentMode = Globals.mode.DeviceIdPage;

                        //Added by shivaraj - CHRQ-158: To display Camera ID in DeviceID page
                        Globals.isCameraConnected = tricam.ConnectCamera();
                        getCameraID();
                        f.Publish(f.Close_Camera, args);

                        f.Publish(f.DeviceID_Page, new Args());
                        if (!nextPrev_p.Visible)
                            nextPrev_p.Visible = true;
                        deviceIDPage.deviceIdP1_tbx.Enabled = deviceIDPage.deviceIdP2_tbx.Enabled = deviceIDPage.deviceIdP3_tbx.Enabled = true;

                        break;
                    }
                case Globals.mode.isMemoryTest:
                    {
                        StageName_lbl.Text = "Basic Test";
                        saveReport_btn.Enabled = Globals.CameraInitialized;

                        MakeDisplayControlAreaVisible(true);
                        Controls_p.Controls.Clear();
                        if (!connectControls_p.Visible)
                            connectControls_p.Visible = true;
                        if (!nextPrev_p.Visible)
                            nextPrev_p.Visible = false;
                        if (memoryTest == null)
                            memoryTest = new MemoryLightTriggerTestUI();

                        //f.Publish(f.Close_Camera, args);
                        browse_btn.Enabled = resume_btn.Enabled = false;

                        memoryTest.Dock = DockStyle.Fill;
                        this.Controls_p.Controls.Add(memoryTest);
                        this.Controls_p.Enabled = Globals.CameraInitialized;
                        Globals.currentMode = Globals.mode.isMemoryTest;

                        //Already camera is connected using connect button and if user goes back to deviceID page, then camera gets closed so following code
                        if (Globals.CameraInitialized)
                        {
                            tricam.open_camera(null,null);
                            tricam.startLiveMode();
                            connect_btn.Text = "Disconnect";
                        }

                        bool val = this.splitContainer1.Visible;
                        break;
                    }

                case Globals.mode.isCameraAlignment:
                    {
                        StageName_lbl.Text = "Camera Alignment";
                        deviceIDPage.deviceIdP1_tbx.Enabled = deviceIDPage.deviceIdP2_tbx.Enabled = deviceIDPage.deviceIdP3_tbx.Enabled = false;
                        //Added by shivaraj - ralated to IlluminationGrid of FundusAlignment 
                        if (alignmentui != null)
                        {
                            alignmentui.illuminationGrid_cbx.Checked = false;
                            alignmentui.uniformillumination_gbx.Enabled = true;
                        }
                        f.Publish(f.RefreshGridLabels, new Args());

                        MakeDisplayControlAreaVisible(true);
                        Controls_p.Controls.Clear();
                        if (!connectControls_p.Visible)
                            connectControls_p.Visible = true;
                        if (!nextPrev_p.Visible)
                            nextPrev_p.Visible = true;
                        if (cameraAlignment == null)
                            cameraAlignment = new CameraAlignment();

                        browse_btn.Enabled = resume_btn.Enabled = false;
                        Globals.browseBtnClicked = false;

                        // commented because grid values to be saved in this page
                        //saveReport_btn.Enabled = false;  
                        cameraAlignment.Dock = DockStyle.Fill;
                        this.Controls_p.Controls.Add(cameraAlignment);
                        
                        Globals.currentMode = Globals.mode.isCameraAlignment;
                        break;
                    }
                case Globals.mode.isFundusAlignment:
                    {
                        StageName_lbl.Text = "Fundus Alignment";
                        saveReport_btn.Enabled = Globals.CameraInitialized;

                        Controls_p.Controls.Clear();
                        MakeDisplayControlAreaVisible(true);
                        if (!connectControls_p.Visible)
                            connectControls_p.Visible = true;
                        if (!nextPrev_p.Visible)
                            nextPrev_p.Visible = true;
                        if (alignmentui == null)
                            alignmentui = new AlignmentUI();

                        alignmentui.Dock = DockStyle.Fill;
                        this.Controls_p.Controls.Add(alignmentui);
                        if (Globals.browseBtnClicked)
                            Globals.browseBtnClicked = false;
                        if(connect_btn.Text == "Connect")
                            browse_btn.Enabled = false;
                        else
                            browse_btn.Enabled = true;
                        Globals.currentMode = Globals.mode.isFundusAlignment;
                        
                        break;
                    }
                case Globals.mode.isRefractoAlignment:
                    {
                        if (Globals.isRoyal)
                        {
                            StageName_lbl.Text = "Refracto Alignment";

                            Controls_p.Controls.Clear();
                            MakeDisplayControlAreaVisible(true);
                            if (!connectControls_p.Visible)
                                connectControls_p.Visible = true;
                            if (!nextPrev_p.Visible)
                                nextPrev_p.Visible = false;
                            if (refractoRingCalculation == null)
                                refractoRingCalculation = new RefractoRingCalculation();
                            refractoRingCalculation.Dock = DockStyle.Fill;
                            if (!this.Controls_p.Controls.Contains(refractoRingCalculation))
                                this.Controls_p.Controls.Add(refractoRingCalculation);
                            this.Controls_p.Enabled = true;
                            Globals.currentMode = Globals.mode.isRefractoCalibration;
                            if (Globals.CameraInitialized)
                                f.Publish(f.Refracto_Init, args);

                        }
                        break;

                    }
                case Globals.mode.isRefractoCalibration:    
                    {
                        StageName_lbl.Text = "Refracto Calibration";
                        saveReport_btn.Enabled = Globals.CameraInitialized;

                        //Added by shivaraj - ralated to IlluminationGrid of FundusAlignment 
                        if (alignmentui != null)
                            alignmentui.illuminationGrid_cbx.Checked = false;
                        f.Publish(f.RefreshGridLabels, new Args());

                        Controls_p.Controls.Clear();
                        MakeDisplayControlAreaVisible(true);
                        if (!connectControls_p.Visible)
                            connectControls_p.Visible = true;

                        if (!nextPrev_p.Visible)
                            nextPrev_p.Visible = false;
                        if (refractoCalibration == null)
                            refractoCalibration = new RefractoCalibration();
                        refractoCalibration.Dock = DockStyle.Fill;
                        if (!this.Controls_p.Contains(refractoCalibration))
                            this.Controls_p.Controls.Add(refractoCalibration);
                        Globals.currentMode = Globals.mode.isRefractoCalibration;
                        break;
                    }
                case Globals.mode.isLensArtifactMeasurement:
                    {
                        StageName_lbl.Text = "Lens Artifact Measurement";
                        saveReport_btn.Enabled = Globals.CameraInitialized;

                        //Added by shivaraj - ralated to IlluminationGrid of FundusAlignment 
                        if (alignmentui != null)
                        {
                            alignmentui.illuminationGrid_cbx.Checked = false;
                            alignmentui.uniformillumination_gbx.Enabled = true;
                        }
                        f.Publish(f.RefreshGridLabels, new Args());

                        Controls_p.Controls.Clear();
                        MakeDisplayControlAreaVisible(true);
                        if (!connectControls_p.Visible)
                            connectControls_p.Visible = true;
                        if (!nextPrev_p.Visible)
                            nextPrev_p.Visible = false;
                        if (lensArtifact == null)
                            lensArtifact = new LensArtifactUI();
                        lensArtifact.Dock = DockStyle.Fill;
                        this.Controls_p.Controls.Add(lensArtifact);
                        this.Controls_p.Enabled = true;
                        if (Globals.browseBtnClicked)
                            Globals.browseBtnClicked = false;
                        if (connect_btn.Text == "Connect")
                            browse_btn.Enabled = false;
                        else
                            browse_btn.Enabled = true;
                        Globals.currentMode = Globals.mode.isLensArtifactMeasurement;

                        break;
                    }
            }
            this.Controls_p.Refresh();
            StageName_lbl.Refresh();
            f.Publish(f.SetCurrentMode, args);
            this.Controls_p.Enabled = Globals.CameraInitialized;

        }

        private void SetApplicationMode(string s, Args n)
        {
            appFlow = (Globals.ApplicationFlow)n["ApplicationMode"];
            switch (appFlow)
            {
                case Globals.ApplicationFlow.production:
                    {
                        ApplicationFlow = new Globals.mode[ProductionFlow.Length];
                        Array.Copy(ProductionFlow, ApplicationFlow, ProductionFlow.Length);
                        break;

                    }
                case Globals.ApplicationFlow.service:
                    {
                        ApplicationFlow = new Globals.mode[ServiceFlow.Length];
                        Array.Copy(ServiceFlow, ApplicationFlow, ServiceFlow.Length);
                        break;

                    }
                case Globals.ApplicationFlow.qa:
                    {
                        ApplicationFlow = new Globals.mode[QaFlow.Length];
                        Array.Copy(QaFlow, ApplicationFlow, QaFlow.Length);
                        break;

                    }
                case Globals.ApplicationFlow.distributor:
                    {
                        ApplicationFlow = new Globals.mode[DistributorFlow.Length];
                        Array.Copy(DistributorFlow, ApplicationFlow, DistributorFlow.Length);
                        break;

                    }
            }
            setPages();
        }

        private void setCurrentMode(string s, Args args)
        {
          
            switch (Globals.currentMode)
            {
                case Globals.mode.DeviceIdPage:
                    {
                        f.Publish(f.SetDeviceIDPage, null);
                        break;
                    }
                case Globals.mode.isCameraAlignment:
                    {
                        f.Publish(f.CameraAlignmentMode, null);
                        break;
                    }
                case Globals.mode.isFundusAlignment:
                    {
                        if (Globals.browseBtnClicked)
                            this.Controls_p.Enabled = true;
                        f.Publish(f.FundusAlignmentMode, args);
                        break;
                    }
                case Globals.mode.isLensArtifactMeasurement:
                    {
                        if (Globals.browseBtnClicked)
                        {
                            this.Controls_p.Enabled = true;
                            f.Publish(f.SET_DISPLAY_AREA, null);
                        }
                        f.Publish(f.LensArtifactMeasurementMode, args);
                        resume_btn.Visible = false;
                        break;
                    }
                case Globals.mode.isMemoryTest:
                    {
                        if (!Globals.browseBtnClicked)
                        {
                            Globals.isHighResolution = false;
                            tricam.startLiveMode();
                        }
                        f.Publish(f.SetMemoryTestMode, null);
                        break;
                    }
                case Globals.mode.isRefractoAlignment:
                    {
                        break;
                    }
                case Globals.mode.isRefractoCalibration:
                    {
                        f.Publish(f.SetRefractoCalibrationMode);
                        { tricam.startLiveMode(); }
                        break;
                    }
            }
        }
        public bool isNVRAM = false;
        
        private void write2Pdf(string modeName)
        {
            page = new PdfPage();
            XFont font = new XFont(xp.FontName, xp.FontSize, XFontStyle.Regular);
            XPen clr = new XPen(XColor.FromKnownColor(KnownColor.Black));
            XSolidBrush xsb = new XSolidBrush(XColor.FromName(xp.ForeColor));
            XSolidBrush xsb0 = new XSolidBrush(XColor.FromName(xp.ForeColor));

            #region Write NVRAM values
            if (!isNVRAM)
            {
                string str = Globals.nvramHelper.GetNvram();
                string[] str1 = Regex.Split(str, "\n");
                page = doc.AddPage();

                xg1 = XGraphics.FromPdfPage(page);
                xCor = xp.X;
                XFont font1 = new XFont(xp.FontName, xp.FontSize, XFontStyle.Regular);
                XPen clr1 = new XPen(XColor.FromKnownColor(KnownColor.Black));
                XSolidBrush xsb1 = new XSolidBrush(XColor.FromName(xp.ForeColor));
                xg1.DrawString("NVRAM Values", font, xsb1, new RectangleF(xp.X + 200, xp.Y, 10, 10), XStringFormats.Center);
                xg1.DrawString(Globals.DeviceId, font, xsb1, new RectangleF(xCor - 50, (float)(page.Height) - 80, 100, 40), XStringFormats.BottomCenter);
                if(Globals.cameraID != null)
                    xg1.DrawString(Globals.cameraID, font, xsb1, new RectangleF(xCor + 350, (float)(page.Height) - 80, 100, 40), XStringFormats.BottomCenter);
                xg1.DrawLine(clr, new Point(xp.X, xp.Y + 20), new Point(xp.X + xp.Width + 260, xp.Y + 20));
                int counter = 0;
                int a = 40;
                int b = 40;
                foreach (string line in str1)
                {
                    if (counter <= 25)
                    {
                        counter++;
                        xg1.DrawString(line, font, xsb1, new RectangleF(xCor + 1, b = b + 25, xp.Width, xp.Height), XStringFormats.TopLeft);
                    }
                    else
                    {
                        xg1.DrawString(line, font, xsb, new RectangleF(xCor + 250, a = a + 25, xp.Width, xp.Height), XStringFormats.TopLeft);
                    }
                }
                isNVRAM = true;
            }

            #endregion

            page = doc.AddPage();
            xg = XGraphics.FromPdfPage(page);
            xCor = xp.X;
            int count = 0;
            xg.DrawString(modeName, font, xsb, new RectangleF(xp.X + 200, xp.Y, 10, 10), XStringFormats.Center);
            xg.DrawString(Globals.DeviceId, font, xsb, new RectangleF(xCor - 50, (float)(page.Height) - 100, 100, 40), XStringFormats.BottomCenter);
            if (Globals.cameraID != null)
                xg.DrawString(Globals.cameraID, font, xsb, new RectangleF(xCor + 350, (float)(page.Height) - 100, 100, 40), XStringFormats.BottomCenter);
            xg.DrawLine(clr, new Point(xp.X, xp.Y + 20), new Point(xp.X + xp.Width + 260, xp.Y + 20));
            int index = 0;


            foreach (string s in ReportRowNames)
            {
                if (index % ReportRowNames.Count == 0)
                {
                    count = 20;
                    font = new XFont(xp.FontName, xp.FontSize, XFontStyle.Bold);
                }
                xsb = new XSolidBrush(XColor.FromName(xp.ForeColor));
                xsb0 = new XSolidBrush(XColor.FromName(xp.ForeColor));

                #region Define boundary conditions
                if (ReportValues[index].ToString() == "NOK")
                    xsb = new XSolidBrush(XColor.FromName(xp.ForeColor2));
                if (ReportValues[index].ToString() == "OK" || ReportValues[index].ToString() == "Observed Metriccs")
                    xsb = new XSolidBrush(XColor.FromName(xp.ForeColor));
                if (s == "White Light LUX Value")
                {
                    if ((int)ReportValues[index] < Globals.LuxMin || (int)ReportValues[index] > Globals.LuxMax)
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor2));
                    else
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor));
                }
                if (s == "White Light Average Intensity")
                {
                    if (!Globals.isRoyal)
                    {
                        if ((int)ReportValues[index] < 220 || (int)ReportValues[index] > 230)// || ReportValues[index].ToString() != "Observed Metriccs")

                            xsb = new XSolidBrush(XColor.FromName(xp.ForeColor2));
                        else
                            xsb = new XSolidBrush(XColor.FromName(xp.ForeColor));
                    }
                    else
                        if ((int)ReportValues[index] < 180 || (int)ReportValues[index] > 200)// || ReportValues[index].ToString() != "Observed Metriccs")

                            xsb = new XSolidBrush(XColor.FromName(xp.ForeColor2));
                        else
                            xsb = new XSolidBrush(XColor.FromName(xp.ForeColor));
                }
                if (s == "White Light Periphery to Inner Intensity Variation")
                {
                    if ((double)ReportValues[index] < 0.81)// || (double)ReportValues[index] > 1.00)
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor2));
                    else
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor));
                }
                if (s == "White Light Top to Bottom Intensity Variation")
                {
                    if ((double)ReportValues[index] < 0.00 || (double)ReportValues[index] > 0.25)
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor2));
                    else
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor));
                }
                if (s == "IR Light Average Intensity")
                {
                    if ((int)ReportValues[index] < 70 || (int)ReportValues[index] > 73)
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor2));
                    else
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor));
                }
                if (s == "IR Light Periphery to Inner Intensity Variation")
                {
                    if ((double)ReportValues[index] < 0.81) //|| (double)ReportValues[index] > 1.00)
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor2));
                    else
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor));
                }
                if (s == "IR Light Top to Bottom Intensity Variation")
                {
                    if ((double)ReportValues[index] < 0.00 || (double)ReportValues[index] > 0.25)
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor2));
                    else
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor));
                }
                if (s == "LiquidLensResetValue" || s == "AOI X" || s == "AOI Y" || s == "Refracto Gain" ||
                    s == "Display Center X" || s == "Display Center Y" || s == "LiquidLensMinValue" || s == "LiquidLensMaxValue")
                    xsb = new XSolidBrush(XColor.FromName(xp.ForeColor));
                if (s == "LA Average Peak Before Correction")
                {
                    if ((double)ReportValues[index] < 0.00 || (double)ReportValues[index] > 14.00)
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor2));
                    else
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor));
                }
                if (s == "LA Percentage Affected Pixels Before Correction")
                {
                    if ((double)ReportValues[index] < 0.00 || (double)ReportValues[index] > 1.00)
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor)); // Use always green,because range is removed
                    else
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor));
                }
                if (s == "LA Average Peak  After Correction")
                {
                    if ((double)ReportValues[index] < 0.00 || (double)ReportValues[index] > 14.00)
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor2));
                    else
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor));
                }
                if (s == "LA Percentage Affected Pixels After Correction")
                {
                    if ((double)ReportValues[index] < 0.00 || (double)ReportValues[index] > 1.00)
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor));
                    else
                        xsb = new XSolidBrush(XColor.FromName(xp.ForeColor));
                }
               
                #endregion

                if (index != 0 && index % ReportRowNames.Count != 0)
                    font = new XFont(xp.FontName, 8, XFontStyle.Regular);

                xg.DrawString(s, font, xsb0, new RectangleF(xCor + 1, xp.Y + count, xp.Width, xp.Height), XStringFormats.TopLeft);
                xg.DrawString(ReportValues[index].ToString(), font, xsb, new RectangleF(xCor + 190 + 1, xp.Y + count, xp.Width, xp.Height), XStringFormats.TopLeft);
                xg.DrawString(ReportRecommendedRange[index].ToString(), font, xsb0, new RectangleF(xCor + 190 + 100 + 1, xp.Y + count, xp.Width, xp.Height), XStringFormats.TopLeft);

                index++;
                count += 10;
                xg.DrawLine(clr, new Point(xp.X, xp.Y + count + 10), new Point(xp.X + xp.Width + 250, xp.Y + count + 10));
                count += 10;
            }
            
        }

        private void SetReportRowNames(Globals.mode val)
        {
            if (ReportRowNames.Count > 0)
                ReportRowNames.Clear();

            ReportRowNames.Add("Quality Metric");
            switch (val)
            {
                //case Globals.mode.NVRAM:
                //    {
                //        string str = Globals.nvramHelper.GetNvram();
                //        ReportRowNames.Add( str);
                //        break; 
                //    }
                case Globals.mode.isFundusAlignment:
                    {
                        ReportRowNames.Add("White Light Average Intensity");
                        ReportRowNames.Add("White Light Periphery to Inner Intensity Variation");
                        ReportRowNames.Add("White Light Top to Bottom Intensity Variation");

                        ReportRowNames.Add("IR Light Average Intensity");
                        ReportRowNames.Add("IR Light Periphery to Inner Intensity Variation");
                        ReportRowNames.Add("IR Light Top to Bottom Intensity Variation");
                        break;
                    }
                case Globals.mode.isLensArtifactMeasurement:
                    {
                        ReportRowNames.Add("LA Average Peak Before Correction");
                        ReportRowNames.Add("LA Percentage Affected Pixels Before Correction");
                        ReportRowNames.Add("LA Average Peak  After Correction");
                        ReportRowNames.Add("LA Percentage Affected Pixels After Correction");
                        break;
                    }
                case Globals.mode.isRefractoCalibration:
                    {
                        ReportRowNames.Add("LiquidLensResetValue");
                        ReportRowNames.Add("LiquidLensMaxValue");
                        ReportRowNames.Add("LiquidLensMinValue");
                        ReportRowNames.Add("AOI X");
                        ReportRowNames.Add("AOI Y");
                        //ReportRowNames.Add("Height Reference point");
                        ReportRowNames.Add("Refracto Gain");
                        ReportRowNames.Add("Display Center X");
                        ReportRowNames.Add("Display Center Y");
                        break;
                    }
                case Globals.mode.isMemoryTest:
                    {
                        ReportRowNames.Add("IR LED ON OFF TEST");
                        ReportRowNames.Add("Flash Light LED ON OFF TEST");
                        ReportRowNames.Add("Flash Light LUX Value");
                        ReportRowNames.Add("Cornea LED ON OFF TEST");
                        if (Globals.isRoyal)
                        {
                            ReportRowNames.Add("Refracto Ring LED ON OFF TEST");
                            ReportRowNames.Add("Baloon LED ON OFF TEST");
                            ReportRowNames.Add("Cornea IR LED ON OFF TEST");
                            //ReportRowNames.Add("Proximity Sensor ON OFF TEST");
                        }
                        ReportRowNames.Add("Memory Test ");
                        ReportRowNames.Add("Trigger ON OFF TEST");
                        ReportRowNames.Add("Left Right Sensor Test");

                        break;
                    }

            }            

        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        private void SetReportValues(Globals.mode val)
        {
            ReportValues.Clear();
            ReportValues.Add("Observed Metrics");
            switch (val)
            {
                //case Globals.mode.NVRAM:
                //    {
                //        ReportValues.Add(" ");
                //        break;
                //    }
                case Globals.mode.isFundusAlignment:
                    {

                        ReportValues.Add(Globals.currentSettings.imageMetrics.Average_White_Light_Intensity);
                        ReportValues.Add(Globals.currentSettings.imageMetrics.Periphery_To_Inner_White_Light_Intensity_Variation);
                        ReportValues.Add(Globals.currentSettings.imageMetrics.Top_To_Bottom_White_Light_Intensity_Variation);
                        ReportValues.Add(Globals.currentSettings.imageMetrics.Average_IR_Light_Intensity);
                        ReportValues.Add(Globals.currentSettings.imageMetrics.Periphery_To_Inner_IR_Light_Intensity_Variation);
                        ReportValues.Add(Globals.currentSettings.imageMetrics.Top_To_Bottom_IR_Light_Intensity_Variation);

                        //ReportTextColors.Add(Globals.currentSettings.imageMetrics.averageIntensityLabelColor_WL);
                        //ReportTextColors.Add(Globals.currentSettings.imageMetrics.peripheryToInnerIntensityVarLabelColor_WL);
                        //ReportTextColors.Add(Globals.currentSettings.imageMetrics.topToBottomIntensityVarLabelColor_WL);
                        //ReportTextColors.Add(Globals.currentSettings.imageMetrics.averageIntensityLabelColor_IR);
                        // ReportTextColors.Add(Globals.currentSettings.imageMetrics.peripheryToInnerIntensityVarLabelColor_IR);
                        // ReportTextColors.Add(Globals.currentSettings.imageMetrics.topToBottomIntensityVarLabelColor_WL);

                        break;
                    }
                case Globals.mode.isLensArtifactMeasurement:
                    {
                        ReportValues.Add(Globals.currentSettings.laMetrics.Average_Peak_Before_Correction_WL);
                        ReportValues.Add(Globals.currentSettings.laMetrics.Percentage_Affected_Pixels_Before_Correction_WL);
                        ReportValues.Add(Globals.currentSettings.laMetrics.Average_Peak_After_Correction_WL);
                        ReportValues.Add(Globals.currentSettings.laMetrics.Percentage_Affected_Pixels_After_Correction_WL);
                        break;
                    }
                case Globals.mode.isRefractoCalibration:
                    {
                        ReportValues.Add((byte)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.LiquidLensResetValue));
                        ReportValues.Add((byte)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.LiquidLensMinValue));
                        ReportValues.Add((byte)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.LiquidLensMaxValue));
                        ReportValues.Add((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.AOIRectangleX));
                        ReportValues.Add((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.AOIRectangleY));
                        ReportValues.Add((byte)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.RefractoGain));
                        ReportValues.Add((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX));
                        ReportValues.Add((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreY));
                        break;
                    }
                case Globals.mode.isMemoryTest:
                    {
                        ReportValues.Add(Globals.currentSettings.memoryTestStruct.IRLight);
                        ReportValues.Add(Globals.currentSettings.memoryTestStruct.FlashLight);
                        ReportValues.Add(Globals.currentSettings.memoryTestStruct.LUXvalue);
                        ReportValues.Add(Globals.currentSettings.memoryTestStruct.WhiteLightRing);
                        if (Globals.isRoyal)
                        {
                            ReportValues.Add(Globals.currentSettings.memoryTestStruct.IRRing);
                            ReportValues.Add(Globals.currentSettings.memoryTestStruct.BaloonLight);
                            ReportValues.Add(Globals.currentSettings.memoryTestStruct.RefractoIR);
                           // ReportValues.Add(Globals.currentSettings.memoryTestStruct.ProximitySensor);
                        }
                        ReportValues.Add(Globals.currentSettings.memoryTestStruct.MemoryTest);
                        ReportValues.Add(Globals.currentSettings.memoryTestStruct.Trigger);
                        ReportValues.Add(Globals.currentSettings.memoryTestStruct.LeftRight);
                        break;
                    }

            }
        }

        string Check<T>(Expression<Func<T>> expr)
        {
            var body = ((MemberExpression)expr.Body);
            return (body.Member.Name);
        }

        private void SetReportRecommendedRange(Globals.mode val)
        {
            if (ReportRecommendedRange.Count > 0)
                ReportRecommendedRange.Clear();
            ReportRecommendedRange.Add("Recommended Range");
            switch (val)
            {
                //case Globals.mode.NVRAM:
                //    {
                //        ReportRecommendedRange.Add(" ");
                //        break;
                //    }
                case Globals.mode.isFundusAlignment:
                    {
                        if (Globals.isRoyal)
                        {
                            ReportRecommendedRange.Add("180-200");
                        }
                        else
                        {
                            ReportRecommendedRange.Add("220 - 230");
                        }
                        ReportRecommendedRange.Add("> 0.81");
                        ReportRecommendedRange.Add("0.00- 0.25");
                        ReportRecommendedRange.Add("70 - 73");
                        ReportRecommendedRange.Add("> 0.81");
                        ReportRecommendedRange.Add("0.00- 0.25");
                        break;
                    }
                case Globals.mode.isLensArtifactMeasurement:
                    {

                        ReportRecommendedRange.Add("0 - 14");
                        ReportRecommendedRange.Add("NA");
                        ReportRecommendedRange.Add("0 - 14");
                        ReportRecommendedRange.Add("NA");
                        break;
                    }
                case Globals.mode.isRefractoCalibration:
                    {
                        ReportRecommendedRange.Add("170");
                        ReportRecommendedRange.Add("200");
                        ReportRecommendedRange.Add("175");
                        ReportRecommendedRange.Add("Camera center");
                        ReportRecommendedRange.Add("Camera center");
                        //ReportRecommendedRange.Add(" ");
                        ReportRecommendedRange.Add(" ");
                        ReportRecommendedRange.Add(" ");
                        ReportRecommendedRange.Add(" ");
                        break;
                    }
                case Globals.mode.isMemoryTest:
                    {
                        ReportRecommendedRange.Add(PassStr);
                        ReportRecommendedRange.Add(PassStr);
                        ReportRecommendedRange.Add("6500 - 8000");
                        ReportRecommendedRange.Add(PassStr);
                        if (Globals.isRoyal)
                        {
                            ReportRecommendedRange.Add(PassStr);
                            ReportRecommendedRange.Add(PassStr);
                            ReportRecommendedRange.Add(PassStr);
                           // ReportRecommendedRange.Add(PassStr);

                        }
                        ReportRecommendedRange.Add(PassStr);
                        ReportRecommendedRange.Add(PassStr);
                        ReportRecommendedRange.Add(PassStr);
                        break;
                    }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string folder = Application.StartupPath + Path.DirectorySeparatorChar + "Calibration.exe";

            MessageBox.Show("Version - " + Assembly.LoadFile(folder).GetName().Version.ToString(),"CalibrationTool",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void onRefractoCaptureComplete(String n, Args arg)
        {
            standByRefracto();
        }

        private void standByRefracto()
        {
            f.Publish(f.GO_STANDBY, null);
            tricam.stopLiveMode();
            tricam.SetBaloonLight(false);
        }

        private void ringCapture(string s, Args arg)
        {
            tricam.RingCapture();
        }

        private void saveImage(string s, Args args)
        {
            string fileName = "";
            fileName = Globals.SaveImagePath + Path.DirectorySeparatorChar + Globals.DeviceId + "_" + DateTime.Now.ToString("HHmmss");
            if (Globals.currentMode == Globals.mode.isRefractoCalibration)
            {
                Globals.refractoReadingsFile = fileName;
            }
            else
            {
                if ((bool)args["isIR"])
                {
                    //Globals.IrBmp.Save(fileName + "_IR.png");
                    Globals.Bmp.Save(fileName + "_IR.png");
                }
                else
                {
                    Globals.whiteBmp.Save(fileName + "_WL.png");
                }
            }
        }

        private void saveReport()
        {
                if (!ReportSaved.Contains(Globals.currentMode))
                    ReportSaved.Add(Globals.currentMode);

                switch (Globals.currentMode)
                {
                    case Globals.mode.isLensArtifactMeasurement:
                        {
                            Globals.currentSettings.laMetrics.isLAMetricsSaved = true;
                            f.Publish(f.LASaveReport, args);
                            if (!ReportStageName.Contains("Lens Artifact Measurement"))
                                ReportStageName.Add("Lens Artifact Measurement");
                            break;
                        }
                    case Globals.mode.DeviceIdPage:
                        {
                            break;
                        }
                    case Globals.mode.isCameraAlignment:
                        {
                            Globals.currentSettings.cameraAlignment.isCameraAlignmentSaved = true;
                            f.Publish(f.CameraAlignmentSaveReport, args);
                            if (!ReportStageName.Contains("Camera Alignment"))
                                ReportStageName.Add("Camera Alignment");
                            break;
                        }
                    case Globals.mode.isFundusAlignment:
                        {
                            Globals.currentSettings.imageMetrics.isImageMetricsSaved = true;
                            f.Publish(f.AlignmentSaveReport, args);
                            if (!ReportStageName.Contains("Fundus Alignment"))
                                ReportStageName.Add("Fundus Alignment");
                            break;
                        }
                    case Globals.mode.isMemoryTest:
                        {
                            Globals.currentSettings.memoryTestStruct.isMemoryTestStructureSaved = true;
                            if (!ReportStageName.Contains("Basic Test"))
                                ReportStageName.Add("Basic Test");
                            break;
                        }

                    case Globals.mode.isRefractoAlignment:
                        {
                            break;
                        }
                    case Globals.mode.isRefractoCalibration:
                        {
                            if (!ReportStageName.Contains("Refracto Calibration"))
                                ReportStageName.Add("Refracto Calibration");
                            Globals.currentSettings.royalSettings.isRoyalSettingsSaved = true;
                            break;
                        }
                }
            }

        private void PromptDialogForSaveReport()
        {
            DialogResult diaRes = MessageBox.Show("Values have not been saved in report, Do you want to save?", "CalibrationTool", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (diaRes == DialogResult.Yes)
                saveReport();
         }

        private void resume_btn_Click(object sender, EventArgs e)
        {

            if (Globals.CameraInitialized)
            {
                if (saveReport_btn.Enabled == false && Globals.currentMode != Globals.mode.isCameraAlignment)
                    saveReport_btn.Enabled = true;
                Globals.browseBtnClicked = false;
                f.Publish(f.SetCurrentMode, null);
               // tricam.startLiveMode();
                
            }
        }

        private void saveGridValues()
        {
            string imgLocation = @"Resources\CameraAlignment.png";

            int X = 150;
            int Y = 175;
            int xCor = 100;
            XFont font = new XFont(xp.FontName, xp.FontSize, XFontStyle.Regular);
            XPen clr = new XPen(XColor.FromKnownColor(KnownColor.Black));
            XSolidBrush xsb = new XSolidBrush(XColor.FromName(xp.ForeColor));
            XFont CAfont = new XFont("Arial", 10, XFontStyle.Regular);
            XFont CAfont1 = new XFont("Arial", 20, XFontStyle.Regular);
            XSolidBrush xsb2 = new XSolidBrush(XColor.FromName("Black"));
           
            // same pdf page which was created in write2pdf()
            page = doc.AddPage();

            gfx = XGraphics.FromPdfPage(page);
            gfx.DrawString("Camera Alignment", CAfont1, xsb2, X + 160, Y - 90, XStringFormats.Center);
            gfx.DrawLine(clr, new Point(X - 50, Y - 75), new Point(X + 100 + 260, Y - 75));
            gfx.DrawString("Grid Values", CAfont1, xsb2, X + 160, Y - 40, XStringFormats.Center);
            gfx.DrawString(Globals.DeviceId, font, xsb, new RectangleF(xCor - 50, (float)(page.Height) - 100, 100, 40), XStringFormats.BottomCenter);
            if (Globals.cameraID != null)
                gfx.DrawString(Globals.cameraID, font, xsb, new RectangleF(xCor + 350, (float)(page.Height) - 100, 100, 40), XStringFormats.BottomCenter);

            // Get an XGraphics object for drawing
            XImage image = XImage.FromFile(imgLocation);
            gfx.DrawImage(image, X - 25, Y, 350, 300);

            //Added Values to Grid
            gfx.DrawString(Globals.currentSettings.cameraAlignment.firstValue, CAfont, xsb2, X + 75, Y + 50, XStringFormats.Center);
            gfx.DrawString(Globals.currentSettings.cameraAlignment.secondValue, CAfont, xsb2, X + 230, Y + 50, XStringFormats.Center);
            gfx.DrawString(Globals.currentSettings.cameraAlignment.thirdValue, CAfont, xsb2, X + 260, Y + 110, XStringFormats.Center);
            gfx.DrawString(Globals.currentSettings.cameraAlignment.fourthValue, CAfont, xsb2, X + 260, Y + 195, XStringFormats.Center);
            gfx.DrawString(Globals.currentSettings.cameraAlignment.fifthValue, CAfont, xsb2, X + 240, Y + 225, XStringFormats.Center);
            gfx.DrawString(Globals.currentSettings.cameraAlignment.sixthValue, CAfont, xsb2, X + 60, Y + 225, XStringFormats.Center);
            gfx.DrawString(Globals.currentSettings.cameraAlignment.seventhValue, CAfont, xsb2, X + 35, Y + 195, XStringFormats.Center);
            gfx.DrawString(Globals.currentSettings.cameraAlignment.eighthValue, CAfont, xsb2, X + 35, Y + 110, XStringFormats.Center);

        }

        private void enableOrDisableBrowseBtn(string s, Args e)
        {
            if (Globals.isIlluminationGrid)
                browse_btn.Enabled = false;
            else
                browse_btn.Enabled = true;
        }

    }
}
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
using Forus.Refracto;
using Nvram;
using System.Drawing.Drawing2D;
namespace Calibration
{
    public class Tricam : CameraAbstract
    {
        private static Tricam tricam;
        public static Byte[] byteArrHighRes;// = new Byte[WlWidth*Wlheight*3];
        public static Byte[] byteArrLowRes;//= new Byte[IRWidth*IRheight*3];
        public Camera myCamera;
        BitmapData bData;
        Facade f;
        GCHandle gchIR;
        GCHandle gchWL;

        private int FlashDelay = 210;
        private int FlashDuration = 72;
        private int IrExp = 40;
        private int lensArtifactGain = 50;
        private int whiteLightPixelClock = 16;
        private int iRLightPixelClock = 22;

        private int IrGain = 10;
        private int whiteGain = 5;
        private int redGain = 22;
        private int greenGain = 25;
        private int blueGain = 32;
        public IntPtr container_handle = (IntPtr)null;
        public IntPtr picture_box_handle = (IntPtr)null;
        public PictureBox picture_box = null;
        private bool assistedFocusInitialized = false;
        private Bitmap IrBmp = null;
        private RefractoCaller refracto;
        private RefractoCalculations refcal;
        private Timer tmr= new Timer();
        private Timer powerStatusTimer = new Timer();
        private Timer LiquidLens_Timer = new Timer();
        public Timer trigger_LeftRightTimer = new Timer();
        Rectangle AOIrect;
        Args arg = new Args();
        int frameCnt;
        List<Forus.Refracto.RefractoReadings> cartesianReadings = new List<Forus.Refracto.RefractoReadings>();
        List<Bitmap> imageList = new List<Bitmap>();
        DirectoryInfo dirInf;

        #region I2c Interface -----------------------------------------------------------------
        // For I2C interface
        public byte I2C_IO_PORT_EXP_ADDRESS = 0x70; // I2C address of TI PCA9538 IO Port expander
        public byte I2C_IO_PORT_EXP_ADDRESS_READ = 0xE1; // I2C address of TI PCA9538 IO Port expander
        public byte I2C_IO_PORT_EXP_ADDRESS_WRITE = 0xE0; // I2C address of TI PCA9538 IO Port expander
        public byte I2C_IO_PORT_EXP_COMMAND_INPUT_READ = 0x00;
        public byte I2C_IO_PORT_EXP_COMMAND_OUTPUT_WRITE = 0x01;
        public byte I2C_IO_PORT_EXP_COMMAND_POLARITY_INVERT = 0x02;
        public byte I2C_IO_PORT_EXP_COMMAND_CONFIGURATION = 0x03;

        // For I2C interface
        public byte I2C_MEMORY_ADDRESS = 0x53; // 0b0101 0011 - I2C address of Microchip I2C memory IOPort expander
        public byte I2C_MEMORY_ADDRESS_READ = 0xA7;
        public byte I2C_MEMORY_ADDRESS_WRITE = 0xA6;

        // For I2C Memory 2 interface
        public byte I2C_MEMORY2_ADDRESS = 0x57; // 0b0101 0111 - I2C address of Microchip I2C memory IO Port expander
        public byte I2C_MEMORY2_ADDRESS_READ = 0xAF;
        public byte I2C_MEMORY2_ADDRESS_WRITE = 0xAE;

        #endregion
        //private RefractoCaller refracto;

        public Tricam()
        {
            f = Facade.getInstance();
            f.Subscribe(f.Read_Nvram, new NotificationHandler(ReadNVRAM));
            f.Subscribe(f.Write_Nvram, new NotificationHandler(WriteNVRAM));
            f.Subscribe(f.Reset_Nvram, new NotificationHandler(ResetNVRAMStruct));
            f.Subscribe(f.EXIT_CAMERA, new NotificationHandler(exitCamera));
            f.Subscribe(f.PowerFailure, new NotificationHandler(GetPowerStatus));
            f.Subscribe(f.Close_Camera, new NotificationHandler(Close_Camera));
            f.Subscribe(f.LIQUIDLENS_COMPLETE, new NotificationHandler(onLiquidLensComplete));
            powerStatusTimer.Tick += new EventHandler(powerStatusTimer_Tick);
            trigger_LeftRightTimer.Interval = 100;
            trigger_LeftRightTimer.Tick += new EventHandler(trigger_LeftRightTimer_Tick);

            powerStatusTimer.Interval = 1000;
            LiquidLens_Timer.Interval = 100;
            LiquidLens_Timer.Tick += new EventHandler(LiquidLens_Timer_Tick);
            arg = new Args();
        }

        public uEye.CAMINFO getCameraID()
        {
             uEye.CAMINFO info = new uEye.CAMINFO();
             if (myCamera != null)
             {
                 myCamera.cameraInfo(ref info);
             }
             return info;
        }
        void trigger_LeftRightTimer_Tick(object sender, EventArgs e)
        {
            arg["Laterality"] = GetLaterality().ToString();
            f.Publish(f.SET_LATERALITY, arg);
            arg["triggerValue"] = myCamera.GetTrigger();
            f.Publish(f.SET_Trigger, arg);
        }
        Double diopterSteps;
        LensControl liquidLens;
        public override void initLL()
        {
            liquidLens = new LensControl(myCamera);
            double retVal = -3;
            myCamera.setLiquidLensLight(true);
            double range = liquidLens.MaxDiopter - liquidLens.MinDiopter;
            diopterSteps = range / 6;
            liquidLens.SetVal((byte)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.LiquidLensResetValue));
        }
      private  void LiquidLens_Timer_Tick(object sender, EventArgs e)
        {
            #region // If power failure stop liquid lens and calculation to handle TS1X-1191//
            if (Globals.isPowerFailure)
            {
                LiquidLens_Timer.Stop();
                myCamera.LiveMode(false);
                f.Publish(f.LIQUIDLENS_COMPLETE, null);
                Forus.Refracto.RefractoReadings refReadings = new Forus.Refracto.RefractoReadings();
                refReadings.AXIS = 0;
                refReadings.CYL = 0;
                refReadings.SPH = 0;
                refReadings.Eyeside = "L";
                refReadings.IsProperRing = false;
                Args arg = new Args();
                arg["readings"] = refReadings;
                f.Publish(f.REFRACTO_CALCULATIONS_COMPLETE, arg);

                return;

            }
            #endregion

            if (Globals.IsR1R2Mode)
            {
                LiquidLens_Timer.Stop();
                f.Publish(f.LIQUIDLENS_COMPLETE, null);
                return;
            }
            Double diopter = liquidLens.GetDiopter();
            diopter -= (3 * diopterSteps);

             currentVal += 8;
             if (currentVal < (byte)Globals.nvramHelper.GetNvramValue(NvramHelper.RoyalSettings.LiquidLensMaxValue))
            {
                SetLiquidLensValue(currentVal);
            }
            else
            {
                LiquidLens_Timer.Stop();
                f.Publish(f.LIQUIDLENS_COMPLETE, null);
            }
        }

        void powerStatusTimer_Tick(object sender, EventArgs e)
        {
            f.Publish(f.PowerFailure, null);
        }
        private void onLiquidLensComplete(string s, Args args)
        {
            Forus.Refracto.RefractoReadings readings = new Forus.Refracto.RefractoReadings();
            if (!Globals.IsR1R2Mode)
            {
                f.Publish(f.REFRACTO_CAPTURE_COMPLETE, null);
                refracto.IsCalibrationMode = Globals.IsR1R2Mode;
                try
                {
                    DoCartesianMethod();
                    //Forus.Refracto.RefractoReadings readings = getMostConsistent(cartesianReadings);
                    if (Globals.FileNotFound)
                    {
                        Globals.FileNotFound = false;
                        return;
                    }
                    readings = getMostConsistent(cartesianReadings);
                }
                catch (Exception ex) {
                    //to Display please retake the image when the ring is not proper
                    readings.IsProperRing = false;
                }
                arg["readings"] = readings;
                f.Publish(f.REFRACTO_CALCULATIONS_COMPLETE, arg);
                cartesianReadings.Clear();
                imageList.Clear();
            }
        }
        public static Tricam createTricam()
        {
            if (tricam == null)
            {
                tricam = new Tricam();
            }
            return tricam;

        }
        private void eventHandler(object sender, EventArgs e)
        {
            int x = 0;
        }
        public void CallWndProc(ref System.Windows.Forms.Message m)
        {
            WndProc(ref m);
        }
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        void WndProc(ref System.Windows.Forms.Message m)
        {
            // Listen for operating system messages
            switch (m.Msg)
            {
                // Ueye Messagepatho
                case uEye.IS_UEYE_MESSAGE:
                    {
                        if (m.WParam.ToInt32() == uEye.IS_DEVICE_REMOVAL)
                            CameraDisconnected();
                        else if (m.WParam.ToInt32() == uEye.IS_NEW_DEVICE)
                            NewCameraConnected();
                        else if (myCamera.IsLiveMode())
                        {
                                    framecounter++;
                                    if (framecounter % 30 == 0)
                                    {
                                        SetRefractoLiveCalculation();
                                    }
                            getFrame();

                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Camera settings for refracto live readings.
        /// </summary>
        private void SetRefractoLiveCalculation()
        {
            if (Globals.IsLiveCalib && Globals.IsR1R2Mode)
            {
                myCamera.LiveMode(false);
                Globals.isHighResolution = true;
                myCamera.SetBinning(uEye.IS_BINNING_DISABLE);
                myCamera.SetPixelClock(whiteLightPixelClock);
                double newExp = 0;
                double maxExp = 0;
                double minExp = 0;
                double curExp = 0;
                myCamera.getExposureRange(ref minExp, ref maxExp, ref newExp, ref curExp);
                myCamera.setCamExposure(50 - 1, ref newExp);
                myCamera.Rotate180(false);
                myCamera.LiveMode(true);

            }
        }
        private void SetCurrentLiveMode()
        {
            switch (Globals.currentMode)
            {
                case Globals.mode.isFundusAlignment:
                    {
                        if (Globals.isHighResolution)
                        {
                            SetFundusFlash(true);
                            myCamera.SetIRLED(false);
                        }
                        break;
                    }
                case Globals.mode.isLensArtifactMeasurement:
                    {
                        if (Globals.isHighResolution)
                        {
                            SetFundusFlash(true);
                            myCamera.SetIRLED(false);
                            myCamera.SetGain(lensArtifactGain);

                        }
                        break;
                    }
                case Globals.mode.isRefractoAlignment:
                    {

                        break;
                    }
                case Globals.mode.isRefractoCalibration:
                    {
                        SetBaloonLight(true);
                        SetCorneaIR(true);
                        SetRefractoIRRing(true);
                        SetFundusFlash(false);
                        SetIR(false);
                        break;
                    }
                case Globals.mode.isCameraAlignment:
                    {
                        if (Globals.isHighResolution)
                        {
                            SetFundusFlash(true);
                            SetIR(false);
                        }
                        else
                        {
                            SetFundusFlash(false);
                            SetIR(true);
                        }
                        break;
                    }
                case Globals.mode.isMemoryTest:
                    {
                        SetAllLightsOff();
                        break;
                    }
            }
        }
        public override void startLiveMode()
        {
            myCamera.SetRGBGain(redGain, greenGain, blueGain);
            if (Globals.isHighResolution)
            {
                myCamera.SetBinning(uEye.IS_BINNING_DISABLE | uEye.IS_BINNING_DISABLE);
                myCamera.SetPixelClock(whiteLightPixelClock);
                myCamera.LiveMode(true);
                myCamera.SetExposure(50);
                myCamera.SetGain(whiteGain);
                SetCurrentLiveMode();
            }
            else
            {
                myCamera.SetBinning(uEye.IS_BINNING_2X_HORIZONTAL | uEye.IS_BINNING_2X_VERTICAL);
                myCamera.SetPixelClock(iRLightPixelClock);
                myCamera.SetRGBGain(redGain, greenGain, blueGain);
                myCamera.SetExposure(IrExp);
                myCamera.SetGain(IrGain);
                myCamera.SetWhiteLED(false);
                myCamera.SetIRLED(true);
                SetCurrentLiveMode();
                myCamera.LiveMode(true);
            }
        }
        public override void stopLiveMode()
        {
            myCamera.SetExternalTrigger(uEye.IS_SET_TRIGGER_SOFTWARE);
            if (myCamera.IsLiveMode())
                myCamera.LiveMode(false);
            myCamera.SetPixelClock(whiteLightPixelClock);
            Bitmap bm = new Bitmap(Globals.IRWidth, Globals.IRheight);
            bm.MakeTransparent(Color.Black);
            SetAllLightsOff();
        }
        private void SetAllLightsOff()
        {
            SetFundusFlash(false);
            SetIR(false);
            SetBaloonLight(false);
            SetCorneaIR(false);
            SetRefractoIRRing(false);
            SetBaloonLight(false);
            SetCorneaIR(false);
            SetRefractoIRRing(false);
        }
        public bool ConnectCamera()
        {
            if (!myCamera.IsOpen())
            {
                // If camera not connected, then connect now
                int bin_mode = uEye.IS_BINNING_2X_HORIZONTAL | uEye.IS_BINNING_2X_VERTICAL;
                return myCamera.Open(bin_mode);
            }
            else
            {
                // Camera connected
                return true;
            }
        }

        public override Boolean open_camera(object sender, EventArgs e)
        {
            if (!myCamera.IsOpen())
            {
                int bin_mode = uEye.IS_BINNING_2X_HORIZONTAL | uEye.IS_BINNING_2X_VERTICAL;
                Globals.isHighResolution = false;
                bool bcamera_open_status = myCamera.Open(bin_mode);
                if (bcamera_open_status)
                {
                    f.Publish(f.CAMERA_CONNECTED, null);
                    myCamera.SetPixelClock(iRLightPixelClock);
                    myCamera.SetExternalTrigger(uEye.IS_SET_TRIGGER_SOFTWARE);
                    myCamera.EnableHotPixelCorrection(true);
                    myCamera.SetColorCorrection(1);
                    myCamera.SetColorCorrection(0);// ENABLE DISABLE 
                    myCamera.SetRGBGain(redGain, greenGain, blueGain);
                    gchIR = GCHandle.Alloc(byteArrLowRes, GCHandleType.Pinned);
                    gchWL = GCHandle.Alloc(byteArrHighRes, GCHandleType.Pinned);
                    Globals.IrBmp = new Bitmap(Globals.IRWidth, Globals.IRheight);
                    Globals.whiteBmp = new Bitmap(Globals.WlWidth, Globals.Wlheight);
                    byteArrHighRes = new Byte[Globals.WlWidth * Globals.Wlheight * 3];
                    byteArrLowRes = new Byte[Globals.IRWidth * Globals.IRheight * 3];
                    initializeI2C();
                    f.Publish(f.PowerFailure, null);
                    powerStatusTimer.Start();
                    f.Publish(f.Read_Nvram, null);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return true;
        }
        private void Close_Camera(string s, Args arg)
        {
            if (myCamera != null)
                if (myCamera.IsOpen())
                {
                    myCamera.SetIRLED(false);
                    myCamera.LiveMode(false);
                    myCamera.SetWhiteLED(false);
                    myCamera.Close();
                }
        }



        System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
        int framecounter = 0;
        public override void getFrame()
        {
            #region Low Resolution Frame Grab
            if (!Globals.isHighResolution)
            {

                IntPtr ppMem;
                bData = Globals.IrBmp.LockBits(new Rectangle(new Point(), Globals.IrBmp.Size),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format24bppRgb);
                ppMem = gchIR.AddrOfPinnedObject();
                if (myCamera.getImageMem(ref ppMem) != uEye.IS_SUCCESS)
                {
                    return;
                } 
                Marshal.Copy(ppMem, byteArrLowRes, 0, byteArrLowRes.Length);
                Marshal.Copy(byteArrLowRes, 0, bData.Scan0, byteArrLowRes.Length);
                Globals.IrBmp.UnlockBits(bData);
                Globals.IrBmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                Globals.IrBmp.RotateFlip(RotateFlipType.Rotate180FlipY);
                picture_box.Image = Globals.IrBmp;

                if (Globals.currentMode == Globals.mode.isFundusAlignment)
                {
                    f.Publish(f.Alignment_Image_Metrics, arg);
                    f.Publish(f.Update_Image_Metrics_UI, arg);
                    if (Globals.isIlluminationGrid)
                    {
                        f.Publish(f.Update_IlluminationGrid, arg);
                    }

                }
                else if (Globals.currentMode == Globals.mode.isLensArtifactMeasurement)
                {
                    if (Globals.ToSegmentImage != null)
                        Globals.ToSegmentImage.Dispose();
                    Globals.ToSegmentImage = new Image<Bgr, byte>(Globals.IrBmp);
                    f.Publish(f.GradingLive, arg);
                }
                else if (Globals.currentMode == Globals.mode.isRefractoCalibration)
                {
                    refracto = RefractoCaller.GetInstance(Globals.DeviceId);
                    refractoLiveProcessing();
                    string A = Globals.nvramHelper.GetNvramValue(NvramHelper.RoyalSettings.RefractoGain).GetType().ToString();
                    if(A=="System.Int32")
                        myCamera.SetGain((int)Globals.nvramHelper.GetNvramValue(NvramHelper.RoyalSettings.RefractoGain));
                    else
                    myCamera.SetGain((byte)Globals.nvramHelper.GetNvramValue(NvramHelper.RoyalSettings.RefractoGain));
                }
            }
            #endregion

            #region HighResolution Frame Grab
            else
            {
                IntPtr ppMem1;
                ppMem1 = gchWL.AddrOfPinnedObject();
                if (myCamera.getImageMem(ref ppMem1) != uEye.IS_SUCCESS)
                {
                    Console.WriteLine("failure");
                    MessageBox.Show("failure");
                    return;
                }
                Marshal.Copy(ppMem1, byteArrHighRes, 0, byteArrHighRes.Length);
                bData = Globals.whiteBmp.LockBits(new Rectangle(new Point(), Globals.whiteBmp.Size),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format24bppRgb);
                Marshal.Copy(byteArrHighRes, 0, bData.Scan0, byteArrHighRes.Length);
                Globals.whiteBmp.UnlockBits(bData);
                Globals.whiteBmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                Globals.whiteBmp.RotateFlip(RotateFlipType.Rotate180FlipY);
                Args arg = new Args();
                //after ring capture in high resolution
                picture_box.Image = Globals.whiteBmp;

                if (Globals.currentMode == Globals.mode.isRefractoCalibration)
                {
                    myCamera.SetGain((byte)Globals.nvramHelper.GetNvramValue(NvramHelper.RoyalSettings.RefractoGain));
                    if (Globals.IsR1R2Mode)
                    {
                        if(refracto!=null)
                            refractoCalculate();
                    }
                    else
                    {
                        AOIrect.X = (int)Globals.nvramHelper.GetNvramValue(NvramHelper.RoyalSettings.AOIRectangleX);
                        AOIrect.Y = (int)Globals.nvramHelper.GetNvramValue(NvramHelper.RoyalSettings.AOIRectangleY);
                        AOIrect.Width = 960;
                        AOIrect.Height = 960;
                        imageList.Add(Globals.whiteBmp.Clone( AOIrect, Globals.whiteBmp.PixelFormat));
                        Console.WriteLine(imageList.Count);
                    }
                }

                arg["isIR"] = false;
                if (Globals.currentMode == Globals.mode.isFundusAlignment)
                {
                    f.Publish(f.Alignment_Image_Metrics, arg);
                    f.Publish(f.Update_Image_Metrics_UI, arg);
                    if (Globals.isIlluminationGrid)
                    {
                        f.Publish(f.Update_IlluminationGrid, arg);
                    }
                }
                else if (Globals.currentMode == Globals.mode.isLensArtifactMeasurement)
                {
                    if (Globals.ToSegmentImage != null)
                        Globals.ToSegmentImage.Dispose();
                    Globals.ToSegmentImage = new Image<Bgr, byte>(Globals.whiteBmp);
                    f.Publish(f.GradingLive, arg);
                }
            }
            #endregion
        }
        RefractoReadings readings;
        private void refractoCalculate()
        {
            cartesianReadings.Clear();

            AOIrect = new Rectangle((int)Globals.nvramHelper.GetNvramValue(NvramHelper.RoyalSettings.AOIRectangleX), (int)Globals.nvramHelper.GetNvramValue(NvramHelper.RoyalSettings.AOIRectangleY), 960, 960);
            Image<Gray, Byte> im = new Image<Gray, byte>(Globals.whiteBmp);
            im.ROI = AOIrect; //set 960,960 image to im
            if(refracto!=null)
                refracto.IsCalibrationMode = Globals.IsR1R2Mode;
            //read calibdata and zeropoints files if IsR1R2 is false
            if (refracto.IsCalibrationMode == false)
            {
                try
                {
                    refracto.readZEROPoints();
                    refracto.ReadCalibData();
                }
                catch (FileNotFoundException ex) { MessageBox.Show("File not found.Please check if the device is calibrated or not"); }
            }
            refracto.IsPolarMethod = false;
            //to get the live readings of R1 and R2 Globals.IsLiveCalib &&
            if ( Globals.IsR1R2Mode)
            {
                myCamera.LiveMode(false);
                LiquidLens_Timer.Stop();
                ///////
                if (Globals.IsLiveCalib)
                {
                    RefractoReadings reading = refracto.calculateRefraction(im);
                    f.Publish(f.REFRACTO_CALCULATIONS_COMPLETE, new Args("readings", reading));
                    myCamera.SetBinning(uEye.IS_BINNING_2X_HORIZONTAL | uEye.IS_BINNING_2X_VERTICAL);
                    Globals.isHighResolution = false;
                    myCamera.LiveMode(true);
                    return;
                }
                this.picture_box.Image = im.ToBitmap();
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
                string folderName = Globals.SaveImagePath + Path.DirectorySeparatorChar + "Refracto";
                if (!Directory.Exists(folderName)) Directory.CreateDirectory(folderName);
                string fileName = "";
                //fileName = Globals.SaveImagePath + Path.DirectorySeparatorChar + Globals.DeviceId + "_" + DateTime.Now.ToString("HHmmss");
                fileName = folderName+ Path.DirectorySeparatorChar + Globals.DeviceId + "_" + DateTime.Now.ToString("HHmmss");
                im.Save(fileName +"Ring.png");
                refracto.IsZeroPoint = Globals.IsZeroPoint;
                try
                {
                     readings = refracto.calculateRefraction(im);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                try
                {
                    double[] checkArr = refracto.rc.cartesianCorrectedolarPts;
                    double diff = checkArr.Max() - checkArr.Min();

                    if (diff < 50 && diff != 0)
                    {
                        if (!Globals.RingRetake)
                        {
                            f.Publish(f.REFRACTO_CAPTURE_COMPLETE, new Args()); //for standby and display lens numbers
                            f.Publish(f.REFRACTO_CALCULATIONS_COMPLETE, new Args("readings", readings));
                        }
                        else
                        {
                            Globals.Retake = true;
                        }
                    }
                    else
                    {
                        Globals.Retake = true;
                        f.Publish(f.Refracto_Retake, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No ring detected. Please retake the image","CalibrationTool",MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                Globals.Retake = false; ;
                myCamera.SetBinning(uEye.IS_BINNING_2X_HORIZONTAL | uEye.IS_BINNING_2X_VERTICAL);
                Globals.isHighResolution = false;
                im.ROI = new Rectangle();
                im.Dispose();
                return;
            }
        }
        public override void resumeLiveMode()
        {
            if (!myCamera.IsLiveMode())
                startLiveMode();
        }
        public override void NewCameraConnected()
        {
            f.Publish(f.CAMERA_CONNECTED, null);
        }
        public override void CameraDisconnected()
        {
            myCamera.SetIRLED(false);
            myCamera.SetWhiteLED(false);
            myCamera.Close();
            f.Publish(f.CAMERA_DISCONNECTED, null);
        }
        public override void setRGBGain()
        {
            myCamera.SetRGBGain(redGain, greenGain, blueGain);
        }
        public override void setDigitalGain()
        {
            myCamera.SetGain(Globals.digitalGain);
        }
        public override void setExposure()
        {
            myCamera.SetExposure(Globals.exposure);
        }
        public void RingCapture()
        {
            f.Publish(f.REFRACTO_CAPTURE_START, new Args()); // display "capturing on the display area.
            //on capture disable binning            
            myCamera.LiveMode(false);
            myCamera.SetBinning(uEye.IS_BINNING_DISABLE);
            myCamera.SetCorneaIR(false);
            myCamera.Rotate180(false);
            myCamera.SetGain((byte)Globals.nvramHelper.GetNvramValue(NvramHelper.RoyalSettings.RefractoGain));//to check later..gain not applying
            double newExp = 0;
            double maxExp = 0;
            double minExp = 0;
            double curExp = 0;
            myCamera.getExposureRange(ref minExp, ref maxExp, ref newExp, ref curExp);
            myCamera.setCamExposure(50 - 1, ref newExp);
            refracto = RefractoCaller.GetInstance(Globals.DeviceId);
            Globals.isHighResolution = true; //convert to 
            initLL();
            myCamera.SetPixelClock(whiteLightPixelClock);
            LiquidLens_Timer = new System.Windows.Forms.Timer();
            LiquidLens_Timer.Interval = 100;
            LiquidLens_Timer.Tick += new EventHandler(LiquidLens_Timer_Tick);
            byte llMinVal = Convert.ToByte(Globals.nvramHelper.GetNvramValue(NvramHelper.RoyalSettings.LiquidLensMinValue));
            SetLiquidLensValue(llMinVal);
            currentVal = (byte)Globals.nvramHelper.GetNvramValue(NvramHelper.RoyalSettings.LiquidLensCurrentValue);
           
            Globals.currentSettings.royalSettings.LLValue_Current = llMinVal;
            if (!Globals.IsR1R2Mode)
            {
                LiquidLens_Timer.Start();

            } myCamera.LiveMode(true);//start
        }

        byte currentVal;
        public override void IRCapture()
        {
            stopLiveMode();
            if (Globals.currentMode == Globals.mode.isFundusAlignment)
            {
                try
                {
                    int res = myCamera.SetBinning(uEye.IS_BINNING_DISABLE | uEye.IS_BINNING_DISABLE);
                    myCamera.SetPixelClock(iRLightPixelClock);
                    myCamera.SetRGBGain(redGain, greenGain, blueGain);
                    myCamera.SetExposure(IrExp);
                    myCamera.SetGain(IrGain);
                    myCamera.SetIRLED(true);
                    myCamera.Capture();
                    IntPtr ppMem1;
                    ppMem1 = gchIR.AddrOfPinnedObject();
                    myCamera.getImageMem(ref ppMem1);
                    Marshal.Copy(ppMem1, byteArrHighRes, 0, byteArrHighRes.Length);

                    bData = Globals.Bmp.LockBits(new Rectangle(new Point(), Globals.Bmp.Size),
                        ImageLockMode.WriteOnly,
                        PixelFormat.Format24bppRgb);
                    Marshal.Copy(byteArrHighRes, 0, bData.Scan0, byteArrHighRes.Length);
                    Globals.Bmp.UnlockBits(bData);
                    Globals.Bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                    Globals.Bmp.RotateFlip(RotateFlipType.Rotate180FlipY);
                    picture_box.Image = Globals.Bmp;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                myCamera.SetBinning(uEye.IS_BINNING_2X_HORIZONTAL | uEye.IS_BINNING_2X_VERTICAL);
                myCamera.SetPixelClock(iRLightPixelClock);
                myCamera.SetRGBGain(redGain, greenGain, blueGain);
                myCamera.SetExposure(IrExp);
                myCamera.SetGain(IrGain);
                myCamera.SetIRLED(true);
                myCamera.Capture();
                IntPtr ppMem1;
                ppMem1 = gchIR.AddrOfPinnedObject();
                myCamera.getImageMem(ref ppMem1);
                Marshal.Copy(ppMem1, byteArrLowRes, 0, byteArrLowRes.Length);

                bData = Globals.IrBmp.LockBits(new Rectangle(new Point(), Globals.IrBmp.Size),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format24bppRgb);
                Marshal.Copy(byteArrLowRes, 0, bData.Scan0, byteArrLowRes.Length);
                Globals.IrBmp.UnlockBits(bData);
                Globals.IrBmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                Globals.IrBmp.RotateFlip(RotateFlipType.Rotate180FlipY);
                picture_box.Image = Globals.IrBmp;
            }
        }
        
        public override void WhiteLightCapture()
        {
            stopLiveMode();
            myCamera.SetBinning(uEye.IS_BINNING_DISABLE | uEye.IS_BINNING_DISABLE);
            myCamera.SetColorConverter(uEye.IS_SET_CM_RGB24, uEye.IS_CONV_MODE_SOFTWARE_5X5);
            int cameraHeight = myCamera.GetDisplayHeight();
            int cameraWidth = myCamera.GetDisplayWidth();

            if (Globals.currentMode == Globals.mode.isLensArtifactMeasurement)
            {
                myCamera.SetGain(lensArtifactGain);
                myCamera.SetExposure(50);
            }
            else
                myCamera.SetGain(whiteGain);

            if (Globals.currentMode != Globals.mode.isLensArtifactMeasurement)
                myCamera.CaptureAndFlash(true, FlashDelay, FlashDuration);
            else
                myCamera.CaptureAndFlash(false, 0, 0);

            IntPtr ppMem1;
            ppMem1 = gchWL.AddrOfPinnedObject();
            myCamera.getImageMem(ref ppMem1);

            Marshal.Copy(ppMem1, byteArrHighRes, 0, byteArrHighRes.Length);

            bData = Globals.whiteBmp.LockBits(new Rectangle(new Point(), Globals.whiteBmp.Size),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            Marshal.Copy(byteArrHighRes, 0, bData.Scan0, byteArrHighRes.Length);
            Globals.whiteBmp.UnlockBits(bData);
            Globals.whiteBmp.RotateFlip(RotateFlipType.Rotate180FlipX);
            Globals.whiteBmp.RotateFlip(RotateFlipType.Rotate180FlipY);
            picture_box.Image = Globals.whiteBmp;
        }

        public override char GetLaterality()
        {
            byte[] I2C_Data = new byte[2];

            I2C_Data[0] = 0x00; // Initialize register 
            myCamera.ReadI2C(I2C_IO_PORT_EXP_ADDRESS_READ, I2C_IO_PORT_EXP_COMMAND_INPUT_READ, I2C_Data, 1);
            char side = 'L';
            if ((I2C_Data[0] & 0x80) == 0)
            {
                //Its right
                side = 'R';
            }
            return side;
        }

        public override void GetPowerStatus(string n, Args arg)
        {

            byte[] I2C_Data = new byte[2];
            I2C_Data[0] = 0x40; // Initialize register 
            int retVal = myCamera.ReadI2C(I2C_IO_PORT_EXP_ADDRESS_READ, I2C_IO_PORT_EXP_COMMAND_INPUT_READ, I2C_Data, 1);
            if (retVal == uEye.IS_SUCCESS)
            {
                //initializeRefracto();
                Globals.isPowerFailure = false;
            }
            else
            {
                Globals.isPowerFailure = true;
            }
        }
        public override bool isLiveMode()
        {
            return myCamera.IsLiveMode();
        }
        #region Lights on/off
        public override bool SetFundusFlash(bool enable)
        {
            return myCamera.SetWhiteLED(enable);
        }
        public override bool SetIR(bool enable)
        {
            return myCamera.SetIRLED(enable);
        }
        public override bool SetRefractoIRRing(bool enable)
        {
            return myCamera.SetRefractoIRRing(enable);
        }
        public override bool SetCorneaWhiteRing(bool enable)
        {
            if (!Globals.isRoyal)
                return myCamera.SetRingLED(enable);
            else
                return myCamera.SetCorneaRingLED(enable);
        }
        public override bool SetCorneaIR(bool enable)
        {
            return myCamera.SetCorneaIR(enable);
        }
        public override void SetBaloonLight(bool enable)
        {
            myCamera.SetInternalFixation(enable);
        }
        #endregion

       // Globals.NVRAM Nvram;
        /// <summary>
        /// Writes Values in the CurrentNVRAM to camera memory. 
        /// </summary>
        /// <returns></returns>
        public override void WriteNVRAM(String n, Args arg)
        {
            int ramSize = 256;
            int retVal = 0;
            bool isSuccess = false;
            isSuccess = true;
            byte[] val = new byte[2]; // temp copy
            if (Globals.nvramHelper != null)
            {
                byte[] ramBuf = Globals.nvramHelper.GetNvramByteArray();
                ramSize = ramBuf.Length;
                
                for (int i = 0; i < ramSize; i++)
                {
                    val[0] = ramBuf[i];
                    retVal = myCamera.WriteI2C(I2C_MEMORY_ADDRESS_WRITE, i, val, 1);//writes to expanded Camera memory
                }
            }
        }
        /// <summary>
        /// Values in camera memory is read back to NvRam struct.
        /// </summary>
        /// <returns></returns>
        public override void ReadNVRAM(String n, Args arg)
        {
            if (myCamera != null)
            {
                int ramSize = 256;
                byte[] ramBuf = new byte[ramSize];

                byte[] readBuf = new byte[2];
                int retVal;
                bool isSuccess = true;
                byte[] val = new byte[2]; // temp copy
                for (int i = 0; i < ramSize; i++)
                {
                    retVal = myCamera.ReadI2C(I2C_MEMORY_ADDRESS_READ, i, readBuf, 1);
                    ramBuf[i] = readBuf[0];
                }

                Globals.nvramHelper = new Nvram.NvramHelper(Globals.isRoyal, ramBuf);
                if (Globals.nvramHelper.WriteNvram)
                {
                    //f.Publish(f.Write_Nvram, null);
                }
            }
        }
        //currently not used
        public override void ResetNVRAMStruct(String n, Args arg)
        {
        }
        public override void exitCamera(String n, Args args)
        {
            if (myCamera.IsOpen())
            {
                //f.Publish(f.Write_Nvram, new Args());
                myCamera.SetIRLED(false);
                myCamera.SetWhiteLED(false);
                myCamera.LiveMode(false);
                myCamera.Close();
            }
        }
      public  bool isMemoryTest1Success = false;
      public bool isMemoryTest2Success = false;
      public bool isCameraMemoryTestSuccess = false;

        public override void TestPcbMemory1()
        {
            // I2C Code for testing the Memory
            byte[] I2C_Data = new byte[256];
            byte[] I2C_Data_copy = new byte[256];
            byte[] val = new byte[2]; // temp copy

            // int I2C_Address_16 = uEye.IS_I2C_16_BIT_REGISTER | 0x01; // memory address
            int I2C_Address; // 8 bit, MSB Address
            int return_value;

            Random random = new Random();

            // Memory writes
            // memory address Byte
            for (I2C_Address = 0x00; I2C_Address < 256; I2C_Address++)
            {
                I2C_Data[I2C_Address] = (byte)random.Next(0, 255); // memory data Byte
                val[0] = I2C_Data[I2C_Address];
                I2C_Data_copy[I2C_Address] = val[0];
                return_value = myCamera.WriteI2C(I2C_MEMORY_ADDRESS_WRITE, I2C_Address, val, 1);
                if ((return_value == uEye.IS_SUCCESS))
                {
                    continue;
                    //MessageBox.Show("Write 1 Successfull");  
                }
                else
                {
                    isMemoryTest1Success = false;
                    MessageBox.Show("PCB Memory 1 either not present or Write Not Successfull : " + I2C_Address.ToString(), "I2C Memory Write Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // memory reads
            for (I2C_Address = 0x00; I2C_Address < 256; I2C_Address++)
            {
                val[0] = (byte)random.Next(0, 255);// reset read copy                
                return_value = myCamera.ReadI2C(I2C_MEMORY_ADDRESS_READ, I2C_Address, val, 1);
                I2C_Data[I2C_Address] = val[0];
                if ((return_value == uEye.IS_SUCCESS))
                {
                    continue;
                    //MessageBox.Show("Write 1 Successfull");  
                }
                else
                {
                    isMemoryTest1Success = false;
                    //MessageBox.Show("Read Not Successfull : " + I2C_Address.ToString());
                    MessageBox.Show("ERROR:Read Not Successfull : " + I2C_Address.ToString(), "I2C Memory Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            for (I2C_Address = 0; I2C_Address < 256; I2C_Address++)
            {
                if (I2C_Data[I2C_Address] == I2C_Data_copy[I2C_Address])
                {
                    continue;
                }
                else
                {
                    isMemoryTest1Success = false;
                    // MessageBox.Show("Write and Read data mismatch : " + I2C_Address.ToString());
                    MessageBox.Show("ERROR:Write and Read data mismatch : " + I2C_Address.ToString(), "I2C Memory Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            //Fill EEPROM with zeros - Manj 25-may-2011
            for (I2C_Address = 0x00; I2C_Address < 256; I2C_Address++)
            {
                I2C_Data_copy[I2C_Address] = (byte)0;
            }

            if (I2C_Address == 256)
            {
                isMemoryTest1Success = true;
                MessageBox.Show("PCB Memory1 Read write test successfull", "CalibrationTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public override void TestPcbMemory2()
        {
            // I2C Code for testing the Memory
            byte[] I2C_Data = new byte[256];
            byte[] I2C_Data_copy = new byte[256];
            byte[] val = new byte[2]; // temp copy

            // int I2C_Address_16 = uEye.IS_I2C_16_BIT_REGISTER | 0x01; // memory address
            int I2C_Address; // 8 bit, MSB Address
            int return_value;

            Random random = new Random();

            // Memory writes
            // memory address Byte
            for (I2C_Address = 0x00; I2C_Address < 256; I2C_Address++)
            {
                I2C_Data[I2C_Address] = (byte)random.Next(0, 255); // memory data Byte
                val[0] = I2C_Data[I2C_Address];
                I2C_Data_copy[I2C_Address] = val[0];
                return_value = myCamera.WriteI2C(I2C_MEMORY2_ADDRESS_WRITE, I2C_Address, val, 1);
                if ((return_value == uEye.IS_SUCCESS))
                {
                    continue;
                    //MessageBox.Show("Write 1 Successfull");  
                }
                else
                {
                    isMemoryTest2Success = false;
                    MessageBox.Show("ERROR:Write Not Successfull : " + I2C_Address.ToString(), "I2C Memory Write Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // memory reads
            for (I2C_Address = 0x00; I2C_Address < 256; I2C_Address++)
            {
                val[0] = (byte)random.Next(0, 255);// reset read copy                
                return_value = myCamera.ReadI2C(I2C_MEMORY2_ADDRESS_READ, I2C_Address, val, 1);
                I2C_Data[I2C_Address] = val[0];
                if ((return_value == uEye.IS_SUCCESS))
                {
                    continue;
                    //MessageBox.Show("Write 1 Successfull");  
                }
                else
                {
                    isMemoryTest2Success = false;
                    //MessageBox.Show("Read Not Successfull : " + I2C_Address.ToString());
                    MessageBox.Show("ERROR:Read Not Successfull : " + I2C_Address.ToString(), "I2C Memory Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            for (I2C_Address = 0; I2C_Address < 256; I2C_Address++)
            {
                if (I2C_Data[I2C_Address] == I2C_Data_copy[I2C_Address])
                {
                    continue;
                }
                else
                {
                    isMemoryTest2Success = false;
                    // MessageBox.Show("Write and Read data mismatch : " + I2C_Address.ToString());
                    MessageBox.Show("ERROR:Write and Read data mismatch : " + I2C_Address.ToString(), "I2C Memory Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            //Fill EEPROM with zeros - Manj 25-may-2011
            for (I2C_Address = 0x00; I2C_Address < 256; I2C_Address++)
            {
                I2C_Data_copy[I2C_Address] = (byte)0;
            }

            if (I2C_Address == 256)
            {
                isMemoryTest2Success = true;
                MessageBox.Show("PCB Memory2 Read write test successfull", "CalibrationTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public override void TestCameraMemory()
        {

            // I2C Code for testing the Memory
            byte[] Mem_Data = new byte[64];
            byte[] Mem_Data_copy = new byte[64];
            byte[] val = new byte[2]; // temp copy

            int Address; // 8 bit, 0 - 63
            int return_value;

            Random random = new Random();

            // Memory writes
            // memory address Byte
            for (Address = 0x00; Address < 64; Address++)
            {
                Mem_Data[Address] = (byte)random.Next(0, 255); // memory data Byte
                val[0] = Mem_Data[Address];
                Mem_Data_copy[Address] = val[0];
                return_value = myCamera.WriteEEPROM(Address, val, 1);
                if ((return_value == uEye.IS_SUCCESS))
                {
                    continue;
                    //MessageBox.Show("Write 1 Successfull");  
                }
                else
                {
                    isCameraMemoryTestSuccess = false;
                    //MessageBox.Show("Write Not Successfull : " + Address.ToString());
                    MessageBox.Show("ERROR:Write Not Successfull : " + Address.ToString(), "Camera Memory Write Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // memory reads
            for (Address = 0x00; Address < 64; Address++)
            {
                val[0] = (byte)random.Next(0, 255);// reset read copy                
                return_value = myCamera.ReadEEPROM(Address, val, 1);
                Mem_Data[Address] = val[0];
                if ((return_value == uEye.IS_SUCCESS))
                {
                    continue;
                    //MessageBox.Show("Write 1 Successfull");  
                }
                else
                {
                    isCameraMemoryTestSuccess = false;
                    System.Windows.Forms.MessageBox.Show("ERROR:Read Not Successfull : " + Address.ToString(), "Camera Memory Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //MessageBox.Show("Read Not Successfull : " + Address.ToString());
                    return;
                }
            }
            for (Address = 0x00; Address < 64; Address++)
            {
                if (Mem_Data[Address] == Mem_Data_copy[Address])
                {
                    continue;
                }
                else
                {
                    isCameraMemoryTestSuccess = false;
                    //MessageBox.Show("Write and Read data mismatch : " + Address.ToString());
                    System.Windows.Forms.MessageBox.Show("ERROR:Write and Read data mismatch : " + Address.ToString(), "Camera Memory Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            if (Address == 64)
            {
                isCameraMemoryTestSuccess = true;
                System.Windows.Forms.MessageBox.Show("Camera Memory Read write test successfull", "CalibrationTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void SetLiquidLensValue(byte value)
        {
            byte[] retval = new byte[1];
            myCamera.ReadI2C(0x46, Convert.ToByte(value), retval, 1);
        }
        private int continousReadyState = 0;
        private void refractoLiveProcessing()
        {
            //string direction = "";
            try
            {
            //    Image<Gray, byte> im = new Image<Gray, byte>(bmp);
            //    Rectangle left = new Rectangle((int)Globals.nvramHelper.GetNvramValue(NvramHelper.RoyalSettings.DisplayCentreX) - 435, (int)Globals.nvramHelper.GetNvramValue(NvramHelper.RoyalSettings.DisplayCentreY) - 170, 180, 260);
            //    Rectangle right = new Rectangle((int)Globals.nvramHelper.GetNvramValue(NvramHelper.RoyalSettings.DisplayCentreX) + 250, (int)Globals.nvramHelper.GetNvramValue(NvramHelper.RoyalSettings.DisplayCentreY) - 170, 180, 260);
            //    bool isLedInPos = refracto.IsLEDInPosition(im, left, right);
            //    if (isLedInPos)
            //    {
            //        direction = "READY";
            //        continousReadyState++;
            //    }
            //    im.Dispose();
                bool isFocusKnobAligned = myCamera.IsFocusKnobAligned();
                Args args = new Args();
                args["focusknobstatus"] = isFocusKnobAligned;
                //args["direction"] = direction;
                f.Publish(f.REFRACTO_FOCUSKNOB_STATUS, args);
                //f.Publish(f.REFRACTO_NO_OF_SPOTS, args);//Draw rectangles
                //if (continousReadyState > 4)
                //{
                //    //continousReadyState = 0;
                //    f.Publish(f.REFRACTO_SPOTS_DETECTED, null);//For autocapture
                //}
            }
            catch (Exception ex)
            {
                //  Console.WriteLine(ex.Message);
            }
        
        }
        private void initializeI2C()
        {
            int return_value;
            byte[] I2C_Data = new byte[2];
            I2C_Data[0] = 0x00; // Reset the output register 
            I2C_Data[0] = 0xF0; // Setting P7-4 as inputs and P3-0 as outputs 
            return_value = myCamera.WriteI2C(I2C_IO_PORT_EXP_ADDRESS_WRITE, I2C_IO_PORT_EXP_COMMAND_OUTPUT_WRITE, I2C_Data, 1);
            I2C_Data[0] = 0xE0; // Setting P7-5 as inputs and P4-0 as outputs 
            return_value = myCamera.WriteI2C(I2C_IO_PORT_EXP_ADDRESS_WRITE, I2C_IO_PORT_EXP_COMMAND_CONFIGURATION, I2C_Data, 1);
        }
        int fileCnt = 0;

        private void DoCartesianMethod()
        {
            refracto.IsPolarMethod = false;
            try
            {
                refracto.readZEROPoints();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Zero point reading error");
            }
            if (!refracto.IsCalibrationMode)
            {
                refracto.ReadCalibData();
            }

            FileInfo fInf = new FileInfo(Globals.refractoReadingsFile);
            if (!fInf.Directory.Exists)
            {
                Directory.CreateDirectory(fInf.DirectoryName);
            }
            int cnt = fInf.Directory.GetFiles("*00.png").Length;

            StreamWriter writer = new StreamWriter("readingsCartesianLog.csv", true);

            string name = fInf.DirectoryName + Path.DirectorySeparatorChar + cnt.ToString("00");
            int gainValue = 0;
            for (int i = 0; i < imageList.Count; i++)
            {
                Image<Gray, byte> im = new Image<Gray, byte>(imageList[i]);
                //if Gain is less than 25, it means user is using the artificial eye for diopter testing.So disable VD correction//10-03-2015
                myCamera.GetGain(ref gainValue);
                if (gainValue <= 25)
                {
                    refracto.IsVD_Twelve = false;
                }
                Forus.Refracto.RefractoReadings readings = refracto.calculateRefraction(im);
                readings.Eyeside = GetLaterality().ToString();
                cartesianReadings.Add(readings);
                refracto.saveRingDataCartesian(Globals.refractoReadingsFile, readings.Eyeside);
                imageList[i].Save(name + i.ToString("00") + readings.Eyeside + ".png");
                writer.WriteLine(fileCnt.ToString("00") + i.ToString("00") + readings.Eyeside + "," + readings.SPH.ToString("0.00") + "," + readings.CYL.ToString("0.00") + ","
                        + readings.AXIS.ToString("0.00") + "," + readings.OldAxis.ToString("0.00") + ",");
                //im[i].Save(name + i.ToString("00") + ".png");
            }
            writer.Close();
        }
        private Forus.Refracto.RefractoReadings getMostConsistent(List<Forus.Refracto.RefractoReadings> read)
        {
            Forus.Refracto.RefractoReadings readings = new Forus.Refracto.RefractoReadings();

            double sphMode = read.GroupBy(v => v.SPH).OrderByDescending(g => g.Count()).First().Key;
            double cylMode = read.GroupBy(v => v.CYL).OrderByDescending(g => g.Count()).First().Key;
            double axisMode = 0;
            int axisModeCnt = 0;
            for (int i = 0; i < read.Count; i++)
            {
                if (read[i].CYL == cylMode)
                {
                    axisMode += read[i].AXIS;
                    axisModeCnt++;
                }
            }
            axisMode = axisMode / axisModeCnt;

            readings.SPH = sphMode;
            readings.CYL = cylMode;
            readings.AXIS = axisMode;

            readings.IsProperRing = read[0].IsProperRing;
            readings.Eyeside = read[0].Eyeside;

            return readings;
        }

       
    }
}

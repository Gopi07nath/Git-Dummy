using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Resources;

namespace Calibration
{
    public class Camera
    {

        // For I2C interface
        public byte I2C_IO_PORT_EXP_ADDRESS = 0x70; // I2C address of TI PCA9538 IO Port expander
        public byte I2C_IO_PORT_EXP_ADDRESS_READ = 0xE1; // I2C address of TI PCA9538 IO Port expander
        public byte I2C_IO_PORT_EXP_ADDRESS_WRITE = 0xE0; // I2C address of TI PCA9538 IO Port expander
        public byte I2C_IO_PORT_EXP_COMMAND_INPUT_READ = 0x00;
        public byte I2C_IO_PORT_EXP_COMMAND_OUTPUT_WRITE = 0x01;
        public byte I2C_IO_PORT_EXP_COMMAND_POLARITY_INVERT = 0x02;
        public byte I2C_IO_PORT_EXP_COMMAND_CONFIGURATION = 0x03;


        /// <summary>
        /// Making camera Singleton so that no duplication occures in the program
        /// </summary>     


        private static Camera _instance = null;
        public static Camera getInstance(IntPtr wndProcHandle, IntPtr windowHandle, EventHandler StatusChangedHandler)
        {
            if (_instance == null)
                _instance = new Camera(wndProcHandle, windowHandle, StatusChangedHandler);
            return _instance;
        }

        public static String s3NethraDataPath = @"D:\3nethra\img\";
        private uEyeCamera uEyeCamera;
        public static int IS_CAMERA_MESSAGE = uEye.IS_UEYE_MESSAGE;


        public Camera(IntPtr wndProcHandle, IntPtr windowHandle, EventHandler StatusChangedHandler)
        {
            uEyeCamera = new uEyeCamera(wndProcHandle, windowHandle);
            uEyeCamera.StatusChanged += StatusChangedHandler;

        }
        //Added to get camera info to handle TS1X-1055
        public bool cameraInfo(ref uEye.CAMINFO camInfo)
        {
            return uEyeCamera.GetCameraInfo(ref camInfo);
        }
        public Size GetSensorSize()
        {
            return uEyeCamera.GetSensorSize();
        }

        public bool IsOpen()
        {
            return uEyeCamera.IsOpen();
        }

        public bool IsLiveMode()
        {
            return uEyeCamera.IsLiveMode();
        }

        public bool Open(int bin_mode)
        {
            return uEyeCamera.Open(bin_mode);
        }

        public bool Close()
        {
            return uEyeCamera.Close();
        }

        public bool LiveMode(bool en)
        {
            return uEyeCamera.LiveMode(en);
        }

        public bool Capture()
        {
            return uEyeCamera.CaptureSingle();
        }

        public int SetFlashDelay(int ulDelay, int ulDuration)
        {
            uEyeCamera.SetFlashDelay(ulDelay, ulDuration);
            return 1;
        }

        public int SetFlashStrobe(int nMode, int nField)
        {
            uEyeCamera.SetFlashStrobe(nMode, nField);
            return 1;
        }

        public int SetColorCorrection(int nEnable)
        {
            return uEyeCamera.SetColorCorrection(nEnable);
        }
        public int SetColorConverter(int ColorMode, int ConvertMode)
        {
            return uEyeCamera.SetColorConverter(ColorMode, ConvertMode);
        }

        public int SetEdgeEnhancement(int nEnable)
        {
            return uEyeCamera.SetEdgeEnhancement(nEnable);
        }

        public int SetGamma(int gamma)
        {
            return uEyeCamera.SetGamma(gamma);
        }

        public int SetGainBoost(int mode)
        {
            return uEyeCamera.SetGainBoost(mode);
        }

        public bool CaptureAndFlash(bool isGlobalFlash, int flashDelay, int flashDuration)
        {
            bool bLed2 = false;
            bool is_success = false;
            //Work around for bug 636 added by Ajith
            uEyeCamera.SetLedWhite(false, out bLed2); //turn off white LED
            if (!uEyeCamera.FlashOn(isGlobalFlash, flashDelay, flashDuration))
            {
                //STRING_Camera_068
                //System.Windows.Forms.MessageBox.Show(Globals.languageResource.GetString("STRING_Camera_068"), Globals.languageResource.GetString("STRING_3nethra"));
            }
            is_success = uEyeCamera.CaptureSingle();
            uEyeCamera.FlashOff(); //disable flash
            return is_success;
        }

        public bool FlashOn(bool isGlobalFlash, int flashDelay, int flashDuration)
        {
            uEyeCamera.FlashOn(false, flashDelay, flashDuration); //disable flash
            return true;
        }

        public bool FlashOff()
        {
            uEyeCamera.FlashOff(); //disable flash
            return true;
        }

        public int SetExternalTrigger(int trig)
        {
            return uEyeCamera.SetExternalTrigger(trig);
        }

        public int SetPixelClock(int Clock)
        {
            return uEyeCamera.SetPixelClock(Clock);
        }

        public int GetPixelClockRange(ref int pnMin, ref int pnMax)
        {
            return uEyeCamera.GetPixelClockRange(ref  pnMin, ref  pnMax);
        }

        public int SetBinning(int mode)
        {
            return uEyeCamera.SetBinning(mode);
        }

        public bool Capture(out double ContrastVal)
        {
            return uEyeCamera.CaptureSingle(out ContrastVal);
        }

        public int GetDisplayHeight()
        {
            return uEyeCamera.GetDisplayHeight();
        }

        public int GetDisplayWidth()
        {
            return uEyeCamera.GetDisplayWidth();
        }


        public void HandleMessage(ref System.Windows.Forms.Message m)
        {
            uEyeCamera.HandleUeyeMessage(m.WParam.ToInt32(), m.LParam.ToInt32());
        }

        public double GetContrast(IntPtr ppMem)
        {
            double cVal = 0;
            uEyeCamera.GetContrast(ppMem, out cVal);
            return cVal;
        }

        public bool SetAutoParameters(double brightnessRef, double maxGain)
        {
            uEyeCamera.SetAutoParameters(brightnessRef, maxGain);
            return true;
        }
        public bool SetAutoGain(bool bEnable)
        {
            return uEyeCamera.SetAutoGain(bEnable);
        }

        public bool SetAutoExposure(bool bEnable)
        {
            return uEyeCamera.SetAutoExposure(bEnable);
        }

        public bool SetGain(int val)
        {
            return uEyeCamera.SetGain(val);
        }

        public bool GetGain(ref int val)
        {
            return uEyeCamera.GetGain(ref val);
        }

        public bool SetRGBGain(int nRed, int nGreen, int nBlue)
        {
            return uEyeCamera.SetRGBGain(nRed, nGreen, nBlue);
        }

        public bool GetRGBGain(ref int nRed, ref int nGreen, ref int nBlue)
        {
            return uEyeCamera.GetRGBGain(ref nRed, ref nGreen, ref nBlue);
        }

        public double SetFrameRate(double fps_new, double fps_cur)
        {
            uEyeCamera.SetFrameRate(fps_new, ref fps_cur);
            return fps_cur;
        }

        public bool SetExposure(double val, ref double newval)
        {
            return uEyeCamera.SetExposure(val, ref newval);
        }

        public int GetTrigger()
        {
            return uEyeCamera.GetTrigger();
        }

        public bool SetExposure(int val)
        {
            Double newVal = 0;
            uEyeCamera.SetExposure((double)val, ref newVal);
            Console.WriteLine("newExp:" + newVal);
            return true;

        }

        //Added by Ajith Bug 513

        public bool getExposureRange(ref double exp_min, ref  double exp_max, ref  double exp_iv, ref double exp_cur)
        {
            return (uEyeCamera.GetExposure(ref exp_min, ref exp_max, ref exp_iv, ref exp_cur));
        }
        public bool setCamExposure(double exp, ref double newExp)
        {
            double expMin = 0, expMax = 0, expCur = 0, exp_iv = 0;
            double fps_Min = 0, fps_max = 0, fps_none = 0, fps_cur = 0, newFPS = 0;

            getExposureRange(ref expMin, ref expMax, ref exp_iv, ref expCur);
            if (exp > expMax)
            {
                bool exposure_set = false;
                while (!exposure_set)
                {
                    //decrease framerate 
                    getFPSRange(ref fps_Min, ref fps_max, ref fps_none, ref fps_cur);
                    fps_cur -= 1;
                    setFrameRate(fps_cur, ref newFPS);
                    if (uEyeCamera.SetExposure(exp, ref newExp))
                    {
                        if (exp - 1 <= newExp && newExp <= exp + 1)
                        {
                            exposure_set = true;
                        }
                    }
                }
            }
            else
            {
                uEyeCamera.SetExposure(exp, ref newExp);
                getFPSRange(ref fps_Min, ref fps_max, ref fps_none, ref fps_cur);

                fps_max = 1000.0 / newExp - 1;

                setFrameRate(fps_max, ref newFPS);
            }
            return false;
        }
        public bool setFrameRate(double val, ref double newVal)
        {
            return uEyeCamera.SetFrameRate(val, ref newVal);
        }
        public bool getFPSRange(ref double fps_min, ref double fps_max, ref double fps_none, ref double fps_cur)
        {
            if (uEyeCamera.GetFramerate(ref fps_min, ref fps_max, ref fps_none, ref fps_cur))
            {
                fps_min = 1 / fps_min;
                fps_max = 1 / fps_max;
                fps_none = 1 / fps_none;
                return true;
            }
            return false;
        }
        // ---------------------  WriteI2C ---------------------------
        //
        public int WriteI2C(int nDeviceAddr, int nRegisterAddr, byte[] pbData, int nLen)
        {
            return uEyeCamera.WriteI2C(nDeviceAddr, nRegisterAddr, pbData, nLen);
        }


        // ---------------------  Read I2C  ----------------------------------------
        // 
        public int ReadI2C(int nDeviceAddr, int nRegisterAddr, byte[] pbData, int nLen)
        {
            return uEyeCamera.ReadI2C(nDeviceAddr, nRegisterAddr, pbData, nLen);
        }

        /*
        public bool GetShutter(ref int val)
        {
            return uEyeCamera.GetExposure(ref val);
        }*/

        public void Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            uEyeCamera.Paint(sender, e);
        }

        public bool SaveImage(String fname)
        {
            uEyeCamera.SaveImage(fname);
            return true;
        }

        public bool SetWhiteLED(bool bVal)
        {
            return uEyeCamera.SetLedWhite(bVal, out bVal);
        }

        public bool SetIRLED(bool bVal)
        {
            return uEyeCamera.SetLedIR(bVal, out bVal);
        }

        public bool SetRingLED(bool bVal)
        {
            if (!Globals.isRoyal)
            {
                return uEyeCamera.SetLedRing(bVal, out bVal);
            }
            else
            {
                byte[] I2C_Data = new byte[2];
                int return_value;
                I2C_Data[0] = 0x00; // Reset the output register 
                return_value = uEyeCamera.ReadI2C(I2C_IO_PORT_EXP_ADDRESS_READ, I2C_IO_PORT_EXP_COMMAND_OUTPUT_WRITE, I2C_Data, 1);
                if (bVal)
                {
                    I2C_Data[0] |= 0x12;
                }
                else
                {
                    I2C_Data[0] &= (0xED);
                }
                return_value = uEyeCamera.WriteI2C(I2C_IO_PORT_EXP_ADDRESS_WRITE, I2C_IO_PORT_EXP_COMMAND_OUTPUT_WRITE, I2C_Data, 1);

                return true;
            }
        }
        public bool SetLaser(bool bVal)
        {
            byte[] I2C_Data = new byte[2];
            int return_value;
            I2C_Data[0] = 0x00; // Reset the output register 
            return_value = uEyeCamera.ReadI2C(I2C_IO_PORT_EXP_ADDRESS_READ, I2C_IO_PORT_EXP_COMMAND_OUTPUT_WRITE, I2C_Data, 1);
            if (bVal)
            {
                I2C_Data[0] |= 0x02;
            }
            else
            {
                I2C_Data[0] &= 0xFD;
            }
            return_value = uEyeCamera.WriteI2C(I2C_IO_PORT_EXP_ADDRESS_WRITE, I2C_IO_PORT_EXP_COMMAND_OUTPUT_WRITE, I2C_Data, 1);

            return true; // uEyeCamera.SetLedRing(bVal, out bVal);
        }
        public bool GetFramerate(ref double fps_min, ref double fps_max, ref double fps_iv, ref double fps_cur)
        {
            return uEyeCamera.GetFramerate(ref fps_min, ref fps_max, ref fps_iv, ref fps_cur);
        }

        public bool GetExposure(ref double exp_min, ref double exp_max, ref double exp_iv, ref double exp_cur)
        {
            return uEyeCamera.GetExposure(ref exp_min, ref exp_max, ref exp_iv, ref exp_cur);
        }

        public bool GetGlobalExposure(ref long exp_delay, ref long exp_duration)
        {
            return uEyeCamera.GetGlobalExposure(ref exp_delay, ref exp_duration);
        }

        public bool GetSensorInfo(ref string sensorname)
        {
            return uEyeCamera.GetSensorInfo(ref sensorname);
        }

        public bool Rotate180(bool bEnable)
        {
            return uEyeCamera.Rotate180(bEnable);
        }

        public int getImageMem(ref IntPtr ppMem)
        {
            return uEyeCamera.GetImageMem(ref ppMem);
        }

        public int WriteEEPROM(int Adr, byte[] pcString, int count)
        {

            return uEyeCamera.WriteEEPROM(Adr, pcString, count);
        }



        // ---------------------  ReadEEPROM  -----------------------
        //
        public int ReadEEPROM(int Adr, byte[] pcString, int count)
        {
            return uEyeCamera.ReadEEPROM(Adr, pcString, count);
        }
        //public bool GetExposure(out int exp_min, out int exp_max, out int exp_cur)
        //{
        //    return uEyeCamera.GetExposure(out exp_min, out exp_max, out exp_cur);
        //}

        public int SetImageAOI(int pXPos, int pYPos, int pWidth, int pHeight)
        {
            return uEyeCamera.SetImageAOI(pXPos, pYPos, pWidth, pHeight);
        }

        public void setLiquidLensLight(Boolean val)
        {
            uEyeCamera.setLiquidLensLight(val);
        }
        public Boolean SetSupertex(Byte val)
        {
            return uEyeCamera.SetSupertex(val);
        }
        public Boolean GetSupertex(Byte val)
        {
            return uEyeCamera.GetSupertex(val);
        }
        public bool SetRefractoIRRing(bool bVal)
        {
            byte[] I2C_Data = new byte[2];
            int return_value;
            I2C_Data[0] = 0x00; // Reset the output register 
            return_value = uEyeCamera.ReadI2C(I2C_IO_PORT_EXP_ADDRESS_READ, I2C_IO_PORT_EXP_COMMAND_OUTPUT_WRITE, I2C_Data, 1);
            if (bVal)
            {
                I2C_Data[0] |= 0x01;
            }
            else
            {
                I2C_Data[0] &= 0xFE;
            }
            return_value = uEyeCamera.WriteI2C(I2C_IO_PORT_EXP_ADDRESS_WRITE, I2C_IO_PORT_EXP_COMMAND_OUTPUT_WRITE, I2C_Data, 1);
            //in test board the LASER is connected to CORNEA RING
            return return_value == 0;//SetRingLED(bVal);//uEyeCamera.SetLedRing(bVal, out bVal);
        }

        public Boolean SetCorneaIR(Boolean value)
        {
            byte[] I2C_Data = new byte[2];
            int return_value = 0;
            I2C_Data[0] = 0x00; // Initialize register 
            return_value = uEyeCamera.ReadI2C(I2C_IO_PORT_EXP_ADDRESS_READ, I2C_IO_PORT_EXP_COMMAND_INPUT_READ, I2C_Data, 1);
            if (value)
            {
                I2C_Data[0] |= 0x08;
            }
            else
            {
                I2C_Data[0] &= 0xF7;
            }
            return_value = uEyeCamera.WriteI2C(I2C_IO_PORT_EXP_ADDRESS_WRITE, I2C_IO_PORT_EXP_COMMAND_OUTPUT_WRITE, I2C_Data, 1);
            return return_value == 0;
        }

        public Boolean SetInternalFixation(Boolean value)
        {
            Boolean bVal = value;
            return uEyeCamera.SetLedRing(bVal, out bVal);
        }

        public bool SetCorneaRingLED(bool bVal)
        {
            byte[] I2C_Data = new byte[2];
            int return_value;
            I2C_Data[0] = 0x00; // Reset the output register 
            return_value = uEyeCamera.ReadI2C(I2C_IO_PORT_EXP_ADDRESS_READ, I2C_IO_PORT_EXP_COMMAND_OUTPUT_WRITE, I2C_Data, 1);
            if (bVal)
            {
                I2C_Data[0] |= 0x02;
            }
            else
            {
                I2C_Data[0] &= 0xFD;
            }
            return_value = uEyeCamera.WriteI2C(I2C_IO_PORT_EXP_ADDRESS_WRITE, I2C_IO_PORT_EXP_COMMAND_OUTPUT_WRITE, I2C_Data, 1);

            return true;
            //}
        }
        public Boolean IsFocusKnobAligned()
        {
            // Checking of I2C inputs
            byte[] I2C_Data1 = new byte[2];
            int return_value = 0;
            I2C_Data1[0] = 0x00; // Initialize register 
            return_value = uEyeCamera.ReadI2C(I2C_IO_PORT_EXP_ADDRESS_READ, I2C_IO_PORT_EXP_COMMAND_INPUT_READ, I2C_Data1, 1);
            //Check I2C PORT 6 
            if ((I2C_Data1[0] & 0x40) != 0)
            {
                //FOCUS KNOB IS ALIGNED
                return true;
            }
            else
            {
                //FOCUS KNOB IS NOT ALIGNED
                return false;
            }
        }
        // Added by sriram 10 april 2013 to fix TS1X-1107 this is the api for enabling dead pixel correction from the ueyeTricam.cs.
        public Boolean EnableHotPixelCorrection(bool enableCorrection)
        {
            return uEyeCamera.setBadPixelCorrection(enableCorrection);
        }

    }
}

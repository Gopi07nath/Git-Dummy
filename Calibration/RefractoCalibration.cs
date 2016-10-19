// $URL: http://forus_server:81/svn/CalibrationFundusImages/trunk/Calibration/calibrationUI.cs $
// $LastChangedBy: vinay $ on $LastChangedDate: 2013-10-29 15:03:19 +0530 (Tue, 29 Oct 2013) $  
//$Rev: 96 $
/* **********************************************************************************
 * File name: calibrationUI.cs
 *
 * This class is used in alignment and calibration of the Royal device used for refraction of the eye. The operator can align the device using 
 * live mode reading method or can manually capture the ring images. All settings made in this UI will automatically saved in NVRAM while exiting.
 * When calibration is done, it saves the calinbration file and zero points file in the bin directory. This files needs to be manually copied to the 
 * relevant 3nethra software directory. It also generates a comprehensive report. When clicked on measure, it genrates the values which can be copied 
 * to OQC report for validation of the device.
 * 
 * 
 * name in C:/CalibrationImages.
 * Author: Vinay
 * Last modifed: 
 * 
 *************************************************************************************
 */
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
using System.IO;
namespace Calibration
{
    public partial class RefractoCalibration : UserControl
    {
        Tricam mycamera;
        Facade f;
        public Camera newcamera;
        DirectoryInfo dirInf;
        private bool StartCalibClicked = false;
        private Ellipse ellipse;
        public Image<Gray, Byte> InputRing;
        public static Image<Bgr, byte> inputRing;

        public RefractoCalibration()
        {
            InitializeComponent();
            mycamera = Tricam.createTricam();
            f = Facade.getInstance();
            f.Subscribe(f.REFRACTO_CALCULATIONS_COMPLETE, new NotificationHandler(onRefractoCalculationsComplete));//To save calibration file
            f.Subscribe(f.REFRACTO_SPOTS_DETECTED, new NotificationHandler(onSpotsDetected));//For autoCapture
            f.Subscribe(f.REFRACTO_FOCUSKNOB_STATUS, new NotificationHandler(onFocusKnobChanged));//to display proximity status
            f.Subscribe(f.SetRefractoCalibrationMode, new NotificationHandler(GetValuesFromNvram));
            //IsZeroPoint.Enabled = false;
            SaveCalib_btn.Enabled = false;
            if (!Globals.IsR1R2Mode)
                mycamera.initLL(); //initialize Liquid lens
            if (Globals.CameraInitialized)
            {
                Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.RoyalSettings.RefractoGain, (int)Refracto_Gain_Updown.Value);
                Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX, (int)DisplayCenter_X_UpDown.Value);
                Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreY, (int)DisplayCenter_Y_UpDown.Value);
                Globals.currentSettings.royalSettings.DisplayCenter_X = (int)DisplayCenter_X_UpDown.Value;
                Globals.currentSettings.royalSettings.DisplayCenter_Y = (int)DisplayCenter_Y_UpDown.Value;
                Globals.currentSettings.royalSettings.AOI_X = (int)AOI_X_UPDOWN.Value;
                Globals.currentSettings.royalSettings.AOI_Y = (int)AOI_Y_UPDOWN.Value;
                Globals.currentSettings.royalSettings.Refracto_Gain = (int)Refracto_Gain_Updown.Value;
            }
        }
            //Liquid Lens Scroll for reset value
            private void Liquid_lens_control_Scroll(object sender, EventArgs e)
        {
            Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.RoyalSettings.LiquidLensResetValue, Convert.ToByte(Liquid_lens_control_Reset.Value));
            mycamera.SetLiquidLensValue((byte)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.LiquidLensResetValue));
            Reset_Value.Text = Liquid_lens_control_Reset.Value.ToString();
            Reset_Value.Refresh();
            GetLiquidLensValue((byte)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.LiquidLensResetValue));
        }
            private void Reset_Value_Click(object sender, EventArgs e)
        {
            byte Reset_Value = Convert.ToByte(Liquid_lens_control_Reset.Value);
        }
            private void RefractoCalibration_Load(object sender, EventArgs e)
            {
                Liquid_lens_control_Reset.Enabled = true;
                Liquid_lens_control_Min.Enabled = true;
                Liquid_lens_control_Max.Enabled = true;
               
            }
            private void GetValuesFromNvram(string s, Args arg)
            {
                if (Globals.CameraInitialized)
                {
                    Globals.currentSettings.royalSettings.AOI_X = (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.AOIRectangleX);
                    Globals.currentSettings.royalSettings.AOI_Y = (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.AOIRectangleY);
                    Globals.currentSettings.royalSettings.DisplayCenter_X = (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX);
                    Globals.currentSettings.royalSettings.DisplayCenter_Y = (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreY);
                    Globals.currentSettings.royalSettings.Refracto_Gain = (byte)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.RefractoGain);
                    Globals.currentSettings.royalSettings.LLValue_Reset = (byte)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.LiquidLensResetValue);
                    Globals.currentSettings.royalSettings.LLValue_Min = (byte)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.LiquidLensMinValue);
                    Globals.currentSettings.royalSettings.LLValue_Max = (byte)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.LiquidLensMaxValue);
                    AOI_X_UPDOWN.Value = (decimal)Globals.currentSettings.royalSettings.AOI_X;
                    AOI_Y_UPDOWN.Value = (decimal)Globals.currentSettings.royalSettings.AOI_Y;
                    DisplayCenter_X_UpDown.Value = (decimal)Globals.currentSettings.royalSettings.DisplayCenter_X;
                    DisplayCenter_Y_UpDown.Value = (decimal)Globals.currentSettings.royalSettings.DisplayCenter_Y;
                    Refracto_Gain_Updown.Value = (decimal)Globals.currentSettings.royalSettings.Refracto_Gain;
                    Liquid_lens_control_Reset.Value = Globals.currentSettings.royalSettings.LLValue_Reset;
                    Liquid_lens_control_Min.Value = Globals.currentSettings.royalSettings.LLValue_Min;
                    Liquid_lens_control_Max.Value = Globals.currentSettings.royalSettings.LLValue_Max;
                    Reset_Value.Text = Globals.currentSettings.royalSettings.LLValue_Reset.ToString();
                    Min_Value.Text = Globals.currentSettings.royalSettings.LLValue_Min.ToString();
                    Max_Value.Text = Globals.currentSettings.royalSettings.LLValue_Max.ToString();
                }
            }

            // Liquid Lens Scroll for minimum value
            private void Liquid_lens_control_Min_Scroll(object sender, EventArgs e)
            {
                Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.RoyalSettings.LiquidLensMinValue, Convert.ToByte(Liquid_lens_control_Min.Value));
                byte llvalue = Convert.ToByte(Liquid_lens_control_Min.Value);
                //mycamera.SetLiquidLensValue((byte)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.LiquidLensMinValue));
                Min_Value.Text = Liquid_lens_control_Min.Value.ToString();
                Min_Value.Refresh();
                GetLiquidLensValue(llvalue);

            }
            private void Min_Value_Click(object sender, EventArgs e)
            {
                byte Min_Value = Convert.ToByte(Liquid_lens_control_Min.Value);

            }
            // Liquid Lens Scroll for maximum value
            private void Liquid_lens_control_Max_Scroll(object sender, EventArgs e)
            {
                Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.RoyalSettings.LiquidLensMaxValue, Convert.ToByte(Liquid_lens_control_Max.Value));
                byte llvalue = Convert.ToByte(Liquid_lens_control_Max.Value);
                //mycamera.SetLiquidLensValue(llvalue);
                Max_Value.Text = Liquid_lens_control_Max.Value.ToString();
                Max_Value.Refresh();
                GetLiquidLensValue(llvalue);
            }
            private void Max_Value_Click(object sender, EventArgs e)
            {
                byte Max_Value = Convert.ToByte(Liquid_lens_control_Max.Value);
            }
            public void GetLiquidLensValue(byte Value) {
                byte value1 = (byte)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.LiquidLensResetValue);
                Args arg = new Args();
                arg["resetVal"] = (byte)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.LiquidLensResetValue);
            }
            public enum state
            {
                CaptureState, ResumeState
            };
            state currentState;
            int i = 0;
            public void RingCapture_Click(object sender, EventArgs e)
            {
               // Measure_ring_btn.Enabled = true;//enable measure ring button only on first capture
                if (Globals.Retake)
                    IsZeroPoint.Enabled = true;
                if (!Globals.IsLiveCalib)
                {
                    //to enable save calib button after capture is done for 37time(-12 to +12) when start calib is clicked.
                    Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.RoyalSettings.AOIRectangleX, (int)AOI_X_UPDOWN.Value);
                    Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.RoyalSettings.AOIRectangleY, (int)AOI_Y_UPDOWN.Value);
                    if (StartCalibClicked == true)
                    {
                        if (Globals.CalibOver)
                        {
                            SaveCalib_btn.Enabled = true;
                            RingCapture.Enabled = false;
                        }
                    }
                    if (currentState == state.CaptureState)
                    {
                        Args arg = new Args();
                        RingCapture.Text = "Resume";
                        RingCapture.Refresh();
                        arg["isIR"] = false;
                        currentState = state.ResumeState;
                        f.Publish(f.REFRACTO_CAPTURE, arg);//call tricam.ringcapture
                    }
                    else
                    {
                        Globals.isHighResolution = false;
                        RingCapture.Text = "RingCapture";
                        RingCapture.Refresh();
                            f.Publish(f.REFRACTO_NO_OF_SPOTS, null);//set display area with grids after resuming to live mode
                        if (!Globals.IsR1R2Mode)
                            mycamera.initLL();
                        mycamera.startLiveMode();
                        currentState = state.CaptureState;
                    }
                }
            }
        //If checked R1R2 mode. If unchecked, Diopter mode
            public void IsCalibration_CheckedChanged(object sender, EventArgs e)
            {
                if (Globals.IsR1R2Mode == false)
                    Globals.IsR1R2Mode = true;
                else
                    Globals.IsR1R2Mode = false;
            }
            RefractoCaller rc;

            public void IsZeroPoint_Click(object sender, EventArgs e)
            {
                Globals.IsZeroPoint = true;
                StartCalibClicked = true;
                Globals.CalibStart = true;
                RingCapture_Click(null, null);
                IsZeroPoint.Enabled = false;
                if(rc !=null)
                rc.calibrationPointsCart.Clear();
            }
           // RefractoCaller rc;
        /// <summary>
        /// Save calibration file. 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="arg"></param>
            private void onRefractoCalculationsComplete(String n, Args arg)
            {
                RefractoReadings readings = (RefractoReadings)arg["readings"];
                if (readings.IsProperRing)
                {
                    rc = Forus.Refracto.RefractoCaller.GetInstance(Globals.DeviceId);
                    if (Globals.IsZeroPoint == true)
                    {
                        File.Delete(@"Calibration_" + Globals.DeviceId + ".csv");
                        rc.SaveCartesianZeroPoints();
                        rc.readZEROPoints();
                        Globals.IsZeroPoint = false;
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
                    string str = dirInf.FullName + Path.DirectorySeparatorChar + "ring";
                    if (readings.Eyeside == null)
                        readings.Eyeside = "L";
                    rc.saveRingDataCartesian(str, readings.Eyeside);
                }
            }

            public void SaveCartesianCalibFile()
            {
                string cartCalib = @"Calibration_" + Globals.DeviceId + "_cartesian.csv";
                string cartDiff = @"CalibDiff_" + Globals.DeviceId + "_cartesian.csv";
                if (File.Exists(cartCalib))
                {
                    File.Delete(cartCalib);
                }
                if(File.Exists(cartDiff))
                {
                File.Delete(cartDiff);
                }

                StreamWriter writer1 = new StreamWriter(cartCalib);
                string header = "";
                //double headerIdx = 12.5;
                double headerIdx = 18.5;
                double headerStep = 0.5;
                while (true)
                {
                    if ((headerIdx > 6) || (headerIdx <= -6))
                    {
                        headerStep = 0.5;
                    }
                    else
                    {
                        headerStep = 0.25;
                    }
                    headerIdx = headerIdx - headerStep;
                    header += headerIdx + ",";
//                    if (headerIdx == -12) break;
                    if (headerIdx == -18) break;
                }
                writer1.WriteLine(header.Substring(0, header.Length - 1));

                //extendCartesianCalibration();
                interpolateCartesianCalibration();
                extendCartesianCalibration(); 
                for (int k = 0; k < rc.calibrationPointsCart[0].Length; k++)
                {
                    string row = "";
                    for (int i = 0; i < rc.calibrationPointsCart.Count; i++)
                    {
                        row += rc.calibrationPointsCart[i][k] + ",";
                    }
                    writer1.WriteLine(row.Substring(0, row.Length - 1));
                    writer1.Flush();
                }
                
                writer1.Close();
            }
            double interPolateFactor;
            private void interpolateCartesianCalibration()
            {
                int len = rc.calibrationPointsCart.Count;
                List<double[]> tempList = new List<double[]>();
                    for (int k = 0; k < len - 1; k++)
                    {
                        //interPolateFactor = (rc.calibrationPointsCart[k + 1][0] - rc.calibrationPointsCart[k][0]) / 12;
                        tempList.Add(rc.calibrationPointsCart[k]);
                        for (int j = 0; j <= 10; j++) 
                        {
                            double[] avgArr = new double[rc.calibrationPointsCart[0].Length];
                            for (int i = 0; i < rc.calibrationPointsCart[0].Length; i++)
                            {
                                interPolateFactor = (rc.calibrationPointsCart[k + 1][i] - rc.calibrationPointsCart[k][i]) / 12;
                                avgArr[i] = interPolateFactor + tempList[tempList.Count-1][i];
                                //avgArr[i] = (rc.calibrationPointsCart[k][i] + rc.calibrationPointsCart[k + 1][i]) / 2.0;
                            }
                            tempList.Add(avgArr);
                        }
                    }
                tempList.Add(rc.calibrationPointsCart[len - 1]);
                rc.calibrationPointsCart = tempList;
            }

            private void extendCartesianCalibration()
            {
                double[] leftSide = new double[rc.calibrationPointsCart[0].Length];
                double[] rightSide = new double[rc.calibrationPointsCart[0].Length];

                for (int i = 0; i < rc.calibrationPointsCart[0].Length; i++)
                {
                    leftSide[i] = Math.Abs(rc.calibrationPointsCart[1][i] - rc.calibrationPointsCart[0][i]);
                    rightSide[i] = Math.Abs(rc.calibrationPointsCart[rc.calibrationPointsCart.Count - 1][i] - rc.calibrationPointsCart[rc.calibrationPointsCart.Count - 2][i]);
                }

                for (int k = 0; k < 12; k++)
                {
                    double[] templ = new double[leftSide.Length];
                    double[] tempr = new double[rightSide.Length];
                    for (int i = 0; i < rc.calibrationPointsCart[0].Length; i++)
                    {
                        templ[i] = rc.calibrationPointsCart[0][i] - leftSide[i];
                        tempr[i] = rc.calibrationPointsCart[rc.calibrationPointsCart.Count - 1][i] + rightSide[i];
                    }
                    rc.calibrationPointsCart.Insert(0, templ);
                    rc.calibrationPointsCart.Add(tempr);
                }
            }

            private void SaveCalib_btn_Click(object sender, EventArgs e)
            {
                this.Cursor = Cursors.WaitCursor;
                //rc.SaveCartesianCalibFile();
                SaveCartesianCalibFile();
                this.Cursor = Cursors.Default;
                SaveCalib_btn.Enabled = false;
                IsZeroPoint.Enabled = true;
                RingCapture.Enabled = true;
                StartCalibClicked = false;
                MessageBox.Show("Calibration file saved");
            }
        //to set aoi from updown X and Y;
            private void AOI_X_UPDOWN_ValueChanged(object sender, EventArgs e)
            {
                Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.RoyalSettings.AOIRectangleX, (int)AOI_X_UPDOWN.Value);
            }
            private void AOI_Y_UPDOWN_ValueChanged(object sender, EventArgs e)
            {
                Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.RoyalSettings.AOIRectangleY, (int)AOI_Y_UPDOWN.Value);
            }
        //Enable savezero and save calib buttons only when start calib button is clicked
            private void Start_Calib_btn_Click(object sender, EventArgs e)
            {
                IsZeroPoint.Enabled = true;
                SaveCalib_btn.Enabled = true;
                StartCalibClicked = true;
                Globals.CalibStart = true;

            }
            private void Refracto_Gain_Updown_ValueChanged(object sender, EventArgs e)
            {
                Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.RoyalSettings.RefractoGain, (byte)Refracto_Gain_Updown.Value);
            }
            private void DisplayCenter_X_UpDown_ValueChanged(object sender, EventArgs e)
            {
                Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreX,(int)DisplayCenter_X_UpDown.Value);
                DisplayCenter_Y_UpDown.Refresh();
                DisplayCenter_X_UpDown.Refresh();
                f.Publish(f.REFRACTO_NO_OF_SPOTS, null);
            }
            private void DisplayCenter_Y_UpDown_ValueChanged(object sender, EventArgs e)
            {
                Globals.nvramHelper.SetNvramValue(Nvram.NvramHelper.RoyalSettings.DisplayCentreY, (int)DisplayCenter_Y_UpDown.Value);
                DisplayCenter_Y_UpDown.Refresh();
                DisplayCenter_X_UpDown.Refresh();
                f.Publish(f.REFRACTO_NO_OF_SPOTS, null);

            }
            private void onSpotsDetected(String n, Args arg)
            {
                RingCapture_Click(null, null);
            }
        // Disable the buttons when in live reading.
            public void IsLiveReading_CheckedChanged(object sender, EventArgs e)
            {
                Globals.IsLiveCalib = true;
                AOI_X_UPDOWN.Enabled = false;
                AOI_Y_UPDOWN.Enabled = false;
                DisplayCenter_X_UpDown.Enabled = false;
                DisplayCenter_Y_UpDown.Enabled = false;
                Refracto_Gain_Updown.Enabled = false;
                RingCapture.Enabled = false;
                if(IsLiveReading.Checked == false){
                    Globals.IsLiveCalib = false;
                    AOI_X_UPDOWN.Enabled = true;
                    AOI_Y_UPDOWN.Enabled = true;
                    DisplayCenter_X_UpDown.Enabled = true;
                    DisplayCenter_Y_UpDown.Enabled = true;
                    Refracto_Gain_Updown.Enabled = true;
                    RingCapture.Enabled = true;
                }
                
            }
            private string direction = "";
        /// <summary>
        /// If during calibration, ring is captured wrongly, click once on retake button. So that it will discard the latest array and allows you to retake the ring image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
            private void Retake_btn_Click(object sender, EventArgs e)
            {
                RefractoCaller rc = RefractoCaller.GetInstance(Globals.DeviceId);
                Globals.RingRetake = true;
                Args args = new Args();
                direction = "READY";
                args["direction"] = direction;
                f.Publish(f.REFRACTO_NO_OF_SPOTS, args);
            }
        // to defocus the ring capture button. Set the focus to other text area.
            private void RingCapture_Enter(object sender, EventArgs e)
            {
                Refracto_Gain_Updown.Focus();
            }
            MeasureRing MR;
        //to display ring paraters in a seperate window needed for OQC
            private void Measure_ring_btn_Click(object sender, EventArgs e)
            {
                if (rc != null)
                {
                    MR = new MeasureRing();
                    DoRingCalculations();
                    MR.Show();
                }
            }
            Rectangle AOIrectangle;
        /// <summary>
            /// Computing from refacto DLL to get Ring parameters when clicked on measure button
        /// </summary>
            private void DoRingCalculations() 
            {
                double zeroDiopterR1 = 0.0;
                double sixDiopterR1 = 0.0;
                if (mycamera.isLiveMode() == false)
                    RingCapture_Click(null, null);
                MessageBox.Show("please attach the JIG without any lens and press OK");
                //Capture
                RingCapture_Click(null, null);  
                mycamera.getFrame();
                GetRingParameters();
                zeroDiopterR1= rc.rc.R1;
                //GetSensitivity();
                showRingDetails();
                //Display msg box 6D 
                if (mycamera.isLiveMode() == false)
                    RingCapture_Click(null, null);
                MessageBox.Show("please keep +6D lens inside the JIG and press OK");
                //Capture
                RingCapture_Click(null, null);
                mycamera.getFrame();
                GetRingParameters();
                sixDiopterR1 = rc.rc.R1;
                //getRingParameters
                sensitivity = sixDiopterR1 - zeroDiopterR1;
                MR.Sensitivity_tbx.Text = sensitivity.ToString("0.00");
                if (sensitivity < 23)
                    MR.Sensitivity_tbx.BackColor = Color.Red;
                else
                    MR.Sensitivity_tbx.BackColor = Color.Green;

            }
            public void GetRingParameters() {
                Image<Gray, Byte> inputRing = new Image<Gray, byte>(Globals.whiteBmp);
                AOIrectangle = new Rectangle((int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.AOIRectangleX), (int)Globals.nvramHelper.GetNvramValue(Nvram.NvramHelper.RoyalSettings.AOIRectangleY), 960, 960);
                inputRing.ROI = AOIrectangle;
                rc.rc = RefractoCalculations.GetInstance();
                try
                {
                    ellipse = rc.rc.CalculateRing(inputRing.Convert<Gray, byte>());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please check if the calibration jig is attched");
                }
            }

         /// <summary>
        ///  Calculate sensitivity by reading the calibration file in the bin directory
        /// </summary>
        /// 
            private Dictionary<int, double> IndexToDiopter;
            private List<double[]> Lookup;
            private List<double> Lookup_zero = new List<double>();
            private List<double> Lookup_six = new List<double>();
            private double sensitivity = 0.0;
            public void GetSensitivity()
            {
                string file = @"Calibration_" + Globals.DeviceId + "_cartesian.csv";
                StreamReader reader = new StreamReader(file);
                Lookup = new List<double[]>();
                IndexToDiopter = new Dictionary<int, double>();
                String headerLine = reader.ReadLine();
                String[] headers = headerLine.Split(',');
                for (int i = 0; i < headers.Length; i++)
                {
                    IndexToDiopter.Add(i, double.Parse(headers[i]));
                }
                while (!reader.EndOfStream)
                {
                    String line = reader.ReadLine();
                    String[] parts = line.Split(',');
                    {
                        double[] lookupVals = parts.Select(o => double.Parse(o)).ToArray();
                        Lookup.Add(lookupVals);

                    }
                }
                reader.Close();
                for (int i = 0; i < Lookup.Count; i++)
                {
                    double[] temp = new double[97];
                    temp = Lookup[i];
                    for (int j = 0; j < temp.Length; j++)
                    {
                        if (j == 72)
                            Lookup_six.Add(temp[j]);           //get the list for +6 and 0D
                        if (j == 48)
                            Lookup_zero.Add(temp[j]);
                    }
                }
                sensitivity = Lookup_six.Max() - Lookup_zero.Max(); 
            }
        /// <summary>
        ///  display ring details when clicked on measure.
        /// </summary>
            private void showRingDetails() {
                double ringcenterMin = 380.00;
                double ringcenterMax = 570.00;
                double radiusMin = 330.00;
                double radiusMax = 348.00;
                double axis_Seventy = 70.00;
                double axis_oneTen = 110.00;
                double radiusDiff = 1.50;
                double ringThickMin = 18.00;
                double ringThickMax = 27.00;
                double ringIntensityMin = 35.00;
                double ringIntensityMax = 155.00;
                //center shift
                double dist = Math.Sqrt((rc.rc.ellipse_centerX - rc.rc.input.Width / 2) * (rc.rc.ellipse_centerX - rc.rc.input.Width / 2) +
                    (rc.rc.ellipse_centerY - rc.rc.input.Height / 2) * (rc.rc.ellipse_centerY - rc.rc.input.Height / 2));
                if (dist > ringcenterMax || dist < ringcenterMin)
                    MR.RingCentre_txt.BackColor = Color.Red;
                else
                    MR.RingCentre_txt.BackColor = Color.Green;
                MR.RingCentre_txt.Text = dist.ToString("0.00");
                List<double> FullRingThickness = new List<double>();
                List<double> FullRingIntensity = new List<double>();
                List<Color> FullRingThicknessColor = new List<Color>();
                List<Color> FullRingIntensityColor = new List<Color>();
                FullRingThickness=rc.rc.ComputeFullRingThickness();
                double FullRingThicknessAvg = FullRingThickness.Average();
                if (FullRingThicknessAvg > ringThickMax || FullRingThicknessAvg < ringThickMin)
                    MR.RingThickness_txt.BackColor = Color.Red;
                else
                    MR.RingThickness_txt.BackColor = Color.Green;
                MR.RingThickness_txt.Text = FullRingThicknessAvg.ToString("0.00");
                FullRingIntensity = rc.rc.ComputeIntensity();
                double FullRingIntensityAvg = FullRingIntensity.Average();
                if (FullRingIntensityAvg > ringIntensityMax || FullRingIntensityAvg < ringIntensityMin)
                    MR.RingInt_txt.BackColor = Color.Red;
                else
                    MR.RingInt_txt.BackColor = Color.Green;
                MR.RingInt_txt.Text = FullRingIntensityAvg.ToString("0.00");
                
                int i = 0;
                int j = 0;
                foreach (double thickness in FullRingThickness)
                {
                    if (FullRingThickness[i] < ringThickMin || FullRingThickness[i] > ringThickMax)
                        FullRingThicknessColor.Insert(j, Color.Red);
                    else
                        FullRingThicknessColor.Insert(j, Color.Green);
                    i++;
                    j++;
                }
                MR.thirty_txt.ForeColor = FullRingThicknessColor[0];
                MR.sixty_txt.ForeColor = FullRingThicknessColor[1];
                MR.ninty_txt.ForeColor = FullRingThicknessColor[2];
                MR.onetwenty_txt.ForeColor = FullRingThicknessColor[3];
                MR.onefifty_txt.ForeColor = FullRingThicknessColor[4];
                MR.oneeighty_txt.ForeColor = FullRingThicknessColor[5];
                MR.twoten_txt.ForeColor = FullRingThicknessColor[6];
                MR.twoforty_txt.ForeColor = FullRingThicknessColor[7];
                MR.twoseventy_txt.ForeColor = FullRingThicknessColor[8];
                MR.threehundered_txt.ForeColor = FullRingThicknessColor[9];
                MR.threethirty_txt.ForeColor = FullRingThicknessColor[10];
                MR.threesixty_txt.ForeColor = FullRingThicknessColor[11];

                int a=0;
                int b=0;
                foreach (double  intensity in FullRingIntensity)
                {
                    if (FullRingIntensity[a] < ringIntensityMin || FullRingIntensity[a] > ringIntensityMax)
                        FullRingIntensityColor.Insert(b,Color.Red);
                    else
                        FullRingIntensityColor.Insert(b, Color.Green);
                    a++;
                    b++;
                }
                MR.Ithirty_txt.ForeColor = FullRingIntensityColor[0];
                MR.Isixty_txt.ForeColor = FullRingIntensityColor[1];
                MR.Ininty_txt.ForeColor = FullRingIntensityColor[2];
                MR.Ionetwenty_txt.ForeColor = FullRingIntensityColor[3];
                MR.Ionefifty_txt.ForeColor = FullRingIntensityColor[4];
                MR.Ioneeighty_txt.ForeColor = FullRingIntensityColor[5];
                MR.Itwoten_txt.ForeColor = FullRingIntensityColor[6];
                MR.Itwoforty_txt.ForeColor = FullRingIntensityColor[7];
                MR.Itwoseventy_txt.ForeColor = FullRingIntensityColor[8];
                MR.Ithreehundered_txt.ForeColor = FullRingIntensityColor[9];
                MR.Ithreethirty_txt.ForeColor = FullRingIntensityColor[10];
                MR.Ithreesixty_txt.ForeColor = FullRingIntensityColor[11];

                MR.thirty_txt.Text = FullRingThickness[0].ToString("0.00");
                MR.sixty_txt.Text = FullRingThickness[1].ToString("0.00");
                MR.ninty_txt.Text = FullRingThickness[2].ToString("0.00");
                MR.onetwenty_txt.Text = FullRingThickness[3].ToString("0.00");
                MR.onefifty_txt.Text = FullRingThickness[4].ToString("0.00");
                MR.oneeighty_txt.Text = FullRingThickness[5].ToString("0.00");
                MR.twoten_txt.Text = FullRingThickness[6].ToString("0.00");
                MR.twoforty_txt.Text = FullRingThickness[7].ToString("0.00");
                MR.twoseventy_txt.Text = FullRingThickness[8].ToString("0.00");
                MR.threehundered_txt.Text = FullRingThickness[9].ToString("0.00");
                MR.threethirty_txt.Text = FullRingThickness[10].ToString("0.00");
                MR.threesixty_txt.Text = FullRingThickness[11].ToString("0.00");
                //MR.RingThickness_txt.Text = rc.rc.ComputeFullRingThickness().ToString("0.00");
                //MR.RingInt_txt.Text = rc.rc.ComputeIntensity().ToString("0.00");
                MR.Ithirty_txt.Text = FullRingIntensity[0].ToString("0.00");
                MR.Isixty_txt.Text = FullRingIntensity[1].ToString("0.00");
                MR.Ininty_txt.Text = FullRingIntensity[2].ToString("0.00");
                MR.Ionetwenty_txt.Text = FullRingIntensity[3].ToString("0.00");
                MR.Ionefifty_txt.Text = FullRingIntensity[4].ToString("0.00");
                MR.Ioneeighty_txt.Text = FullRingIntensity[5].ToString("0.00");
                MR.Itwoten_txt.Text = FullRingIntensity[6].ToString("0.00");
                MR.Itwoforty_txt.Text = FullRingIntensity[7].ToString("0.00");
                MR.Itwoseventy_txt.Text = FullRingIntensity[8].ToString("0.00");
                MR.Ithreehundered_txt.Text = FullRingIntensity[9].ToString("0.00");
                MR.Ithreethirty_txt.Text = FullRingIntensity[10].ToString("0.00");
                MR.Ithreesixty_txt.Text = FullRingIntensity[11].ToString("0.00");
                //
                if (rc.rc.R1 > radiusMax || rc.rc.R1 < radiusMin)
                    MR.Radius1_txt.BackColor = Color.Red;
                else
                    MR.Radius1_txt.BackColor = Color.Green;
                if (rc.rc.R2 > radiusMax || rc.rc.R2 < radiusMin)
                    MR.Radius2_txt.BackColor = Color.Red;
                else
                    MR.Radius2_txt.BackColor = Color.Green;
                if ((rc.rc.R1 - rc.rc.R2) > radiusDiff)
                    MR.RadiusDiff_txt.BackColor = Color.Red;
                else
                    MR.RadiusDiff_txt.BackColor = Color.Green;

                MR.Radius1_txt.Text = rc.rc.R1.ToString("0.00");
                MR.Radius2_txt.Text = rc.rc.R2.ToString("0.00");
                MR.RadiusDiff_txt.Text = (rc.rc.R1 - rc.rc.R2).ToString("0.00");
                double AxisMod = (((rc.rc.Axis)) % 180);
//                double AxisMod = (rc.rc.Axis % 180) - 90;
                if (AxisMod > axis_Seventy && AxisMod < axis_oneTen)
                    MR.Axis.BackColor = Color.Green;
                else
                    MR.Axis.BackColor = Color.Red;
                MR.Axis.Text = AxisMod.ToString("0.00");
                if (rc.rc.ellipse_centerX > ringcenterMax || rc.rc.ellipse_centerX < ringcenterMin)
                    MR.RingCentre_txt.BackColor = Color.Red;
                else
                    MR.RingCentre_txt.BackColor = Color.Green;
                MR.RingCentre_txt.Text = rc.rc.ellipse_centerX.ToString("0.00");
                
            }
            private bool focused = false;

            private void onFocusKnobChanged(String n, Args arg)
            {
                //Graphics g = Graphics.FromImage(maskOverlay);
                focused = (bool)arg["focusknobstatus"];
                DrawFocusStatus();
            }

            private void DrawFocusStatus()
            {
                if (!focused)
                {
                    Proximity_status.Text = "Not Focussed";
                    Proximity_status.Font = new Font("Arial",12, FontStyle.Bold);
                    Proximity_status.ForeColor = Color.FromArgb(254, 0, 0); 
                }
                else
                {
                    Proximity_status.Text = "Focussed";
                    Proximity_status.Font = new Font("Arial", 12, FontStyle.Bold);
                    Proximity_status.ForeColor = Color.Green;
                }
            }
       }
}

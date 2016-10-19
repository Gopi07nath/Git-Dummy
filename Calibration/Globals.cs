// $URL: http://forus_server:81/svn/CalibrationFundusImages/trunk/Calibration/calibrationUI.cs $
// $LastChangedBy: vinay $ on $LastChangedDate: 2013-10-29 15:03:19 +0530 (Tue, 29 Oct 2013) $  
//$Rev: 96 $
/* **********************************************************************************
 * File name: calibrationUI.cs
 *This class contains all the global variables that are commonly used at many places.
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
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using Emgu.Util;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV;
using System.IO;
using Nvram;
namespace Calibration
{
   public class Globals
    {
       public static bool isIlluminationGrid = false;
       public static int LuxMin = 6500;
       public static int LuxMax = 8000;
       public static string cameraID ;
       public static bool isCameraConnected = false;

       public static void initSettings()
       {
           currentSettings = new Settings();
       }

       private static string logFilePath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "LogFile.txt";
       public static string LogFilePath
       {
           get { return logFilePath; }
           set { logFilePath = value; }
       }

       public static Bitmap whiteBmp;
       public static Bitmap IrBmp;
       public static Bitmap Bmp = new Bitmap(2048, 1536);
       public static int WlWidth = 2048;
       public static int Wlheight = 1536;
       public static int IRWidth = 1024;
       public static int IRheight = 768;

       public static Boolean isHighResolution = false;
       public static Boolean GradingAfterCorrection = false;

       public static int digitalGain=0;
       public static int exposure=0;

       public static Bitmap mask1mp;
       public static Bitmap mask3mp;

       public static Image<Gray, byte> ThreshImg;
       public static Image<Gray, byte> ChannelImg;
       public static Image<Gray, byte> RedChannelImg;
       public static Image<Bgr, byte> ToSegmentImage;

       public static Boolean IsZeroPoint = false;
       public static Boolean IsR1R2Mode = true;
       
       public static string DeviceId = "";
       public static Boolean isRoyal = false;

       public static Boolean CameraInitialized = false;
       public static Boolean isCaptureFailure = false;
       public static Boolean isPowerFailure = false;

       public static Boolean isRefractoCalibration = false;
       public static Boolean isNvramFailure = false;
       public static Boolean isSetLACord = false;
       public static Boolean isTopCord = false;
       public static Boolean isBottomCord = false;
       public static string SaveImagePath = "";
       public static  bool browseBtnClicked = false;

       public static Rectangle WLRoiRect = new Rectangle(950, 450, 300, 500); // new Rectangle(425, 225, 150, 250);
       public static Rectangle IRroiRect = new Rectangle(425, 225, 150, 250);
       public static Rectangle WLRoiRectLAcoordinates = new Rectangle(950, 450, 200, 200);

       public static string path = "C:";
       public static string calibrationPath = path + Path.DirectorySeparatorChar + "CalibrationImages";
       public static string refractoReadingsFile=Globals.calibrationPath + Path.DirectorySeparatorChar 
                + "Refracto"+ Path.DirectorySeparatorChar + Globals.DeviceId+"readings.txt";
       
       public static bool CalibStart = false;
       public static bool IsLiveCalib = false;
       public static NvramHelper nvramHelper;
       public static bool Retake = false;
       public static bool RingRetake = false;
       public static bool CalibOver = false;
       public static bool FileNotFound = false;
       public static double[] LensID = new double[1];

       #region ********** Modes/Flow **********
       public static mode currentMode;
       public enum mode { classicRoyalPage, DeviceIdPage, isMemoryTest, isCameraAlignment, isFundusAlignment, isRefractoCalibration, isLensArtifactMeasurement, isRefractoAlignment, NVRAM };
       public enum ApplicationFlow { production, service, qa, distributor };
       #endregion

    
       #region ********** Settings **********
       public static Settings currentSettings;
       [Serializable()]
       [StructLayout(LayoutKind.Sequential)]
       public class Settings
       {
           public String deviceId ="";
           public CameraAlignment cameraAlignment = new CameraAlignment();
           public ImageMetrics imageMetrics = new ImageMetrics();
           public RoyalSettings royalSettings = new RoyalSettings();
           public LAMetrics laMetrics = new LAMetrics();
           public LACoordinate laCoordinates = new LACoordinate();
           public MemoryTestStructure memoryTestStruct=new MemoryTestStructure();
       }

       [Serializable()]
       [StructLayout(LayoutKind.Sequential)]
       public class CameraAlignment
       {
           public bool isCameraAlignmentSaved;
           public string firstValue;
           public string secondValue;
           public string thirdValue;
           public string fourthValue;
           public string fifthValue;
           public string sixthValue;
           public string seventhValue;
           public string eighthValue;
       }

       [Serializable()]
       [StructLayout(LayoutKind.Sequential)]
       public class RoyalSettings
       {
           public  bool isRoyalSettingsSaved ;
           public byte LLValue_Current;
           public int AOI_X;
           public int AOI_Y;
           public int Refracto_Gain;
           public int DisplayCenter_X;
           public int DisplayCenter_Y;
           public byte LLValue_Reset;
           public byte LLValue_Min;
           public byte LLValue_Max;
       }
       [Serializable()]
       [StructLayout(LayoutKind.Sequential)]
       public  class ImageMetrics
       {
           public  bool isImageMetricsSaved;
           public  int Average_IR_Light_Intensity;
           public  double Periphery_To_Inner_IR_Light_Intensity_Variation;
           public  double Top_To_Bottom_IR_Light_Intensity_Variation;
           public  int Average_White_Light_Intensity;
           public  double Periphery_To_Inner_White_Light_Intensity_Variation;
           public  double Top_To_Bottom_White_Light_Intensity_Variation;

           public  Color averageIntensityLabelColor_IR;
           public  Color peripheryToInnerIntensityVarLabelColor_IR;
           public  Color topToBottomIntensityVarLabelColor_IR;

           public  Color averageIntensityLabelColor_WL;
           public  Color peripheryToInnerIntensityVarLabelColor_WL;
           public  Color topToBottomIntensityVarLabelColor_WL;

           public int firstValue;
           public int secondValue;
           public int thirdValue;
           public int fourthValue;
           public int fifthValue;
           public int sixthValue;
           public int seventhValue;
           public int eighthValue;
       }
       [Serializable()]
       [StructLayout(LayoutKind.Sequential)]
       public class LAMetrics
       {
           public  bool isLAMetricsSaved ;

           public  double Average_Peak_Before_Correction_IR;
           public  double Percentage_Affected_Pixels_Before_Correction_IR;
           public  double Average_Peak_After_Correction_IR;
           public  double Percentage_Affected_Pixels_After_Correction_IR;

           public  double Average_Peak_Before_Correction_WL;
           public  double Percentage_Affected_Pixels_Before_Correction_WL;
           public  double Average_Peak_After_Correction_WL;
           public  double Percentage_Affected_Pixels_After_Correction_WL;

           public  Color Average_Peak_Before_Correction_Color_WL;
           public  Color Percentage_Affected_Pixels_Before_Correction_Color_WL;
       }
       [Serializable()]
       [StructLayout(LayoutKind.Sequential)]
       public class LACoordinate
       {
           public  int TopX;
           public  int TopY;
           public  int BottomX;
           public  int BottomY;
       }
       [Serializable()]
       [StructLayout(LayoutKind.Sequential)]
       public class MemoryTestStructure
       {
           public bool   isMemoryTestStructureSaved;
           public string FlashLight ="NOK"  ;
           public int LUXvalue = 0;
           public string IRLight ="NOK" ;
           public string WhiteLightRing="NOK" ;
           public string IRRing="NOK" ;
           public string BaloonLight="NOK" ;
           public string RefractoIR="NOK" ;
           public string ProximitySensor="NOK" ;
           public string LeftRight ="NOK" ;
           public string Trigger="NOK" ;
           public string MemoryTest ="NOK" ;
       }
       [Serializable()]
       [StructLayout(LayoutKind.Sequential)]
       public class NVRAMValues
       {
           public bool isNvramValueSaved;
       }
       #endregion
    }
}

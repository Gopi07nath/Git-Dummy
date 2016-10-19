using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
namespace Nvram
{
    internal class NVRAM
    {
        public NVRAM_FIELDS DeviceId = new NVRAM_FIELDS();
        public NVRAM_FIELDS OptoMechanicalID = new NVRAM_FIELDS();
        public NVRAM_FIELDS BomVersion_Minor = new NVRAM_FIELDS();
        public NVRAM_FIELDS BomVersion_Major = new NVRAM_FIELDS();

        public NVRAM_FIELDS LensArtifactSettings = new NVRAM_FIELDS();

        public NVRAM_FIELDS EnableLensArtifact = new NVRAM_FIELDS();
        public NVRAM_FIELDS LensArtifactTopX = new NVRAM_FIELDS();
        public NVRAM_FIELDS LensArtifactTopY = new NVRAM_FIELDS();
        public NVRAM_FIELDS LensArtifactBottomX = new NVRAM_FIELDS();
        public NVRAM_FIELDS LensArtifactBottomY = new NVRAM_FIELDS();

        public NVRAM_FIELDS HeightRefPoint = new NVRAM_FIELDS();
        public NVRAM_FIELDS AOIRectangleX = new NVRAM_FIELDS();
        public NVRAM_FIELDS AOIRectangleY = new NVRAM_FIELDS();
        public NVRAM_FIELDS AOIRectangleWidth = new NVRAM_FIELDS();
        public NVRAM_FIELDS AOIRectangleHeight = new NVRAM_FIELDS();

        public NVRAM_FIELDS DisplayCentreX = new NVRAM_FIELDS();
        public NVRAM_FIELDS DisplayCentreY = new NVRAM_FIELDS();

        public NVRAM_FIELDS LEDThreshold = new NVRAM_FIELDS();

        public NVRAM_FIELDS LiquidLensSettings = new NVRAM_FIELDS();

        public NVRAM_FIELDS LiquidLensResetValue = new NVRAM_FIELDS();
        public NVRAM_FIELDS LiquidLensSteps = new NVRAM_FIELDS();
        public NVRAM_FIELDS LiquidLensCurrentValue = new NVRAM_FIELDS();
        public NVRAM_FIELDS LiquidLensMinValue = new NVRAM_FIELDS();
        public NVRAM_FIELDS LiquidLensMaxValue = new NVRAM_FIELDS();
        public NVRAM_FIELDS LiquidLensSweepStepSize = new NVRAM_FIELDS();
        public NVRAM_FIELDS RefractoGain = new NVRAM_FIELDS();

        public NVRAM_FIELDS ActivationCode = new NVRAM_FIELDS();
        public NVRAM_FIELDS IsPayPerUse = new NVRAM_FIELDS();//bool
        public NVRAM_FIELDS IsFirstTime = new NVRAM_FIELDS();//bool
        public NVRAM_FIELDS ExpiryDate = new NVRAM_FIELDS();
        public NVRAM_FIELDS LicenseMode = new NVRAM_FIELDS();//string
        public NVRAM_FIELDS NoOfVisits = new NVRAM_FIELDS();
        public NVRAM_FIELDS NoOfLicensesElapsed = new NVRAM_FIELDS();

        public NVRAM_FIELDS PatientCount_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS LeftEyeRefraction_count_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS RightEyeRefraction_count_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS ReportsGenerated_count_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS ReportPrintouts_count_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS WhiteLightLED_ON_Duration_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS IR_LED_ON_Duration_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS RingIR_LED_ON_Duration_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS Film_LED_ON_Duration_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS CorneaFixtureIR_ON_Duration_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS CorneaLED_ON_Duration_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS NewVisit_count_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS LeftAnteriorImage_count_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS RightAnteriorImage_count_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS LeftPosteriorImage_count_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS RightPosteriorImage_count_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS DeletedImages_count_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS ReportsMailed_counts_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS MacAddress_NV = new NVRAM_FIELDS();
        public NVRAM_FIELDS latestdatetime_NV = new NVRAM_FIELDS();

        public NVRAM_FIELDS LicensingSettings = new NVRAM_FIELDS();
        public NVRAM_FIELDS StatisticsSettings = new NVRAM_FIELDS();
        public NVRAM_FIELDS RoyalSettings = new NVRAM_FIELDS();
        public NVRAM_FIELDS ClassicSettings = new NVRAM_FIELDS();
        public NVRAM_FIELDS NvramSettings = new NVRAM_FIELDS();


        public NVRAM(bool isRoyal)
        {
            ClassicSettings.enable = 1;
            RoyalSettings.enable = 1;
            LensArtifactSettings.enable = 1;
            LiquidLensSettings.enable = 1;
            LicensingSettings.enable = 1;
            NvramSettings.enable = 1;
            DeviceId = new NVRAM_FIELDS();
            DeviceId.enable = 1;
            DeviceId.Length = 12;
            DeviceId.Name = "Device-ID";
            if (isRoyal)
                DeviceId.value = "3RXXXXXXXXXX";
            else
                DeviceId.value = "3CXXXXXXXXXX";


            OptoMechanicalID.enable = 1;
            OptoMechanicalID.Length = 6;
            OptoMechanicalID.Name = "Opto-Mechanical ID";
            if (isRoyal)
                OptoMechanicalID.value = "TRXXXX";
            else
                OptoMechanicalID.value = "TCXXXX";

            EnableLensArtifact.enable = 1;
            EnableLensArtifact.Length = 1;
            EnableLensArtifact.Name = "Enable-Lens-Artifact Removal";
            EnableLensArtifact.value = (byte)0;
            EnableLensArtifact.MaxValue = (int)1;
            EnableLensArtifact.MinValue = (int)0;

            LensArtifactTopX.enable = 1;
            LensArtifactTopX.Length = 4;
            LensArtifactTopX.Name = "Lens-Artifact Top X";
            LensArtifactTopX.value = 1024;
            LensArtifactTopX.MaxValue = 2048;
            LensArtifactTopX.MinValue = 0;

            LensArtifactTopY.enable = 1;
            LensArtifactTopY.Length = 4;
            LensArtifactTopY.Name = "Lens-Artifact Top Y";
            LensArtifactTopY.value = 768;
            LensArtifactTopY.MaxValue = 2048;
            LensArtifactTopY.MinValue = 0;


            LensArtifactBottomX.enable = 1;
            LensArtifactBottomX.Length = 4;
            LensArtifactBottomX.Name = "Lens-Artifact Bottom X";
            LensArtifactBottomX.value = 1024;
            LensArtifactBottomX.MaxValue = 2048; ;
            LensArtifactBottomX.MinValue = 0;

            LensArtifactBottomY.enable = 1;
            LensArtifactBottomY.Length = 4;
            LensArtifactBottomY.Name = "Lens-Artifact Bottom Y";
            LensArtifactBottomY.value = 768;
            LensArtifactBottomY.MaxValue = 2048;
            LensArtifactBottomY.MinValue = 0;

            HeightRefPoint.enable = 1;
            HeightRefPoint.Length = 4;
            HeightRefPoint.Name = "Height Referece Point";
            HeightRefPoint.value = 285;
            HeightRefPoint.MaxValue = (int)HeightRefPoint.value + 100;
            HeightRefPoint.MinValue = (int)HeightRefPoint.value - 100;

            AOIRectangleX.enable = 1;
            AOIRectangleX.Length = 4;
            AOIRectangleX.Name = "AOI Rectangle X";
            AOIRectangleX.value = 544;
            AOIRectangleX.MaxValue = 2048;
            AOIRectangleX.MinValue = 0;

            AOIRectangleY.enable = 1;
            AOIRectangleY.Length = 4;
            AOIRectangleY.Name = "AOI Rectangle Y";
            AOIRectangleY.value = 288;
            AOIRectangleY.MaxValue = 2048;
            AOIRectangleY.MinValue = 0;

            AOIRectangleWidth.enable = 1;
            AOIRectangleWidth.Length = 4;
            AOIRectangleWidth.Name = "AOI Rectangle Width";
            AOIRectangleWidth.value = 960;
            AOIRectangleWidth.MaxValue = 2048;
            AOIRectangleWidth.MinValue = 0;

            AOIRectangleHeight.enable = 1;
            AOIRectangleHeight.Length = 4;
            AOIRectangleHeight.Name = "AOI Rectangle Height";
            AOIRectangleHeight.value = 960;
            AOIRectangleHeight.MaxValue = 2048;
            AOIRectangleHeight.MinValue = 0;

            DisplayCentreX.enable = 1;
            DisplayCentreX.Length = 4;
            DisplayCentreX.Name = "Display Centre X";
            DisplayCentreX.value = 512;
            DisplayCentreX.MaxValue = 2048;
            DisplayCentreX.MinValue = 0;

            DisplayCentreY.enable = 1;
            DisplayCentreY.Length = 4;
            DisplayCentreY.Name = "Display Centre Y";
            DisplayCentreY.value = 384;
            DisplayCentreY.MaxValue = 2048;
            DisplayCentreY.MinValue = 0;

            LEDThreshold.enable = 1;
            LEDThreshold.Length = 4;
            LEDThreshold.Name = "LED Threshold";
            LEDThreshold.value = 200;
            LEDThreshold.MaxValue = (int)LEDThreshold.value + 100;
            LEDThreshold.MinValue = (int)LEDThreshold.value - 100;

            LiquidLensResetValue.enable = 1;
            LiquidLensResetValue.Length = 1;
            LiquidLensResetValue.Name = "Liquid Lens Reset Value";
            LiquidLensResetValue.value = (byte)170;
            LiquidLensResetValue.MaxValue = byte.MaxValue;
            LiquidLensResetValue.MinValue = byte.MinValue;

            LiquidLensSteps.enable = 1;
            LiquidLensSteps.Length = 1;
            LiquidLensSteps.Name = "Liquid Lens Steps";
            LiquidLensSteps.value = (byte)10;
            LiquidLensSteps.MaxValue = byte.MaxValue;
            LiquidLensSteps.MinValue = byte.MinValue;

            LiquidLensCurrentValue.enable = 1;
            LiquidLensCurrentValue.Length = 1;
            LiquidLensCurrentValue.Name = "Liquid Lens current Value";
            LiquidLensCurrentValue.value = (byte)170;
            LiquidLensCurrentValue.MaxValue = byte.MaxValue;
            LiquidLensCurrentValue.MinValue = byte.MinValue;

            LiquidLensMinValue.enable = 1;
            LiquidLensMinValue.Length = 1;
            LiquidLensMinValue.Name = "Liquid Lens Max Value";
            LiquidLensMinValue.value = (byte)190;
            LiquidLensMinValue.MaxValue = byte.MaxValue;
            LiquidLensMinValue.MinValue = byte.MinValue;

            LiquidLensMaxValue.enable = 1;
            LiquidLensMaxValue.Length = 1;
            LiquidLensMaxValue.Name = "Liquid Lens Min Value";
            LiquidLensMaxValue.value = (byte)170;
            LiquidLensMaxValue.MaxValue = byte.MaxValue;
            LiquidLensMaxValue.MinValue = byte.MinValue;

            LiquidLensSweepStepSize.enable = 1;
            LiquidLensSweepStepSize.Length = 1;
            LiquidLensSweepStepSize.Name = "Liquid Lens Sweep Step Size";
            LiquidLensSweepStepSize.value = (byte)2;
            LiquidLensSweepStepSize.MaxValue = byte.MaxValue;
            LiquidLensSweepStepSize.MinValue = byte.MinValue;

            BomVersion_Major.enable = 1;
            BomVersion_Major.Length = 1;
            BomVersion_Major.Name = "BOM Version_Major";
            BomVersion_Major.MaxValue = byte.MaxValue;
            BomVersion_Major.MinValue = byte.MinValue;
            if (isRoyal)
            {
                BomVersion_Major.value = (byte)1;

            }
            else
            {
                BomVersion_Major.value = (byte)2;

            }
            BomVersion_Minor.enable = 1;
            BomVersion_Minor.Length = 1;
            BomVersion_Minor.Name = "BOM Version_Minor";
            BomVersion_Minor.MaxValue = byte.MaxValue;
            BomVersion_Minor.MinValue = byte.MinValue;
            if (isRoyal)
            {
                BomVersion_Minor.value = (byte)1;

            }
            else
            {
                BomVersion_Minor.value = (byte)0;

            }
            RefractoGain.enable = 1;
            RefractoGain.Length = 1;
            RefractoGain.Name = "Refracto Gain";
            RefractoGain.value = (byte)5;
            RefractoGain.MaxValue = byte.MaxValue;
            RefractoGain.MinValue = byte.MinValue;

            PatientCount_NV.enable = 0;
            PatientCount_NV.Length = sizeof(int);
            PatientCount_NV.Name = "Patient Count";
            PatientCount_NV.value = 0;
            PatientCount_NV.MaxValue = int.MaxValue;
            PatientCount_NV.MinValue = int.MinValue;

            LeftEyeRefraction_count_NV.enable = 0;
            LeftEyeRefraction_count_NV.Length = sizeof(int);
            LeftEyeRefraction_count_NV.Name = "Left Eye Refraction Count";
            LeftEyeRefraction_count_NV.value = 0;
            LeftEyeRefraction_count_NV.MaxValue = int.MaxValue;
            LeftEyeRefraction_count_NV.MinValue = int.MinValue;

            RightEyeRefraction_count_NV.enable = 0;
            RightEyeRefraction_count_NV.Length = sizeof(int);
            RightEyeRefraction_count_NV.Name = "Right Eye Refraction Count";
            RightEyeRefraction_count_NV.value = 0;
            RightEyeRefraction_count_NV.MaxValue = int.MaxValue;
            RightEyeRefraction_count_NV.MinValue = int.MinValue;

            ReportsGenerated_count_NV.enable = 0;
            ReportsGenerated_count_NV.Length = sizeof(int);
            ReportsGenerated_count_NV.Name = "Reports Generated Count";
            ReportsGenerated_count_NV.value = 0;
            ReportsGenerated_count_NV.MaxValue = int.MaxValue;
            ReportsGenerated_count_NV.MinValue = int.MinValue;

            ReportPrintouts_count_NV.enable = 0;
            ReportPrintouts_count_NV.Length = sizeof(int);
            ReportPrintouts_count_NV.Name = "Report Printouts Count";
            ReportPrintouts_count_NV.value = 0;
            ReportPrintouts_count_NV.MaxValue = int.MaxValue;
            ReportPrintouts_count_NV.MinValue = int.MinValue;

            WhiteLightLED_ON_Duration_NV.enable = 0;
            WhiteLightLED_ON_Duration_NV.Length = sizeof(int);
            WhiteLightLED_ON_Duration_NV.Name = "White Light LED ON Duration";
            WhiteLightLED_ON_Duration_NV.value = 0;
            WhiteLightLED_ON_Duration_NV.MaxValue = int.MaxValue;
            WhiteLightLED_ON_Duration_NV.MinValue = int.MinValue;

            IR_LED_ON_Duration_NV.enable = 0;
            IR_LED_ON_Duration_NV.Length = sizeof(int);
            IR_LED_ON_Duration_NV.Name = "IR Light LED ON Duration";
            IR_LED_ON_Duration_NV.value = 0;
            IR_LED_ON_Duration_NV.MaxValue = int.MaxValue;
            IR_LED_ON_Duration_NV.MinValue = int.MinValue;

            RingIR_LED_ON_Duration_NV.enable = 0;
            RingIR_LED_ON_Duration_NV.Length = sizeof(int);
            RingIR_LED_ON_Duration_NV.Name = "Ring IR Light LED ON Duration";
            RingIR_LED_ON_Duration_NV.value = 0;
            RingIR_LED_ON_Duration_NV.MaxValue = int.MaxValue;
            RingIR_LED_ON_Duration_NV.MinValue = int.MinValue;

            Film_LED_ON_Duration_NV.enable = 0;
            Film_LED_ON_Duration_NV.Length = sizeof(int);
            Film_LED_ON_Duration_NV.Name = "Film LED Light LED ON Duration";
            Film_LED_ON_Duration_NV.value = 0;
            Film_LED_ON_Duration_NV.MaxValue = int.MaxValue;
            Film_LED_ON_Duration_NV.MinValue = int.MinValue;

            CorneaFixtureIR_ON_Duration_NV.enable = 0;
            CorneaFixtureIR_ON_Duration_NV.Length = sizeof(int);
            CorneaFixtureIR_ON_Duration_NV.Name = "Cornea Fixture IR ON Duration";
            CorneaFixtureIR_ON_Duration_NV.value = 0;
            CorneaFixtureIR_ON_Duration_NV.MaxValue = int.MaxValue;
            CorneaFixtureIR_ON_Duration_NV.MinValue = int.MinValue;

            CorneaLED_ON_Duration_NV.enable = 0;
            CorneaLED_ON_Duration_NV.Length = sizeof(int);
            CorneaLED_ON_Duration_NV.Name = "Cornea LED ON Duration";
            CorneaLED_ON_Duration_NV.value = 0;
            CorneaLED_ON_Duration_NV.MaxValue = int.MaxValue;
            CorneaLED_ON_Duration_NV.MinValue = int.MinValue;

            NewVisit_count_NV.enable = 0;
            NewVisit_count_NV.Length = sizeof(int);
            NewVisit_count_NV.Name = "New visit count";
            NewVisit_count_NV.value = 0;
            NewVisit_count_NV.MaxValue = int.MaxValue;
            NewVisit_count_NV.MinValue = int.MinValue;

            LeftAnteriorImage_count_NV.enable = 0;
            LeftAnteriorImage_count_NV.Length = sizeof(int);
            LeftAnteriorImage_count_NV.Name = "Left anterior image count";
            LeftAnteriorImage_count_NV.value = 0;
            LeftAnteriorImage_count_NV.MaxValue = int.MaxValue;
            LeftAnteriorImage_count_NV.MinValue = int.MinValue;

            RightAnteriorImage_count_NV.enable = 0;
            RightAnteriorImage_count_NV.Length = sizeof(int);
            RightAnteriorImage_count_NV.Name = "Right anterior image count";
            RightAnteriorImage_count_NV.value = 0;
            RightAnteriorImage_count_NV.MaxValue = int.MaxValue;
            RightAnteriorImage_count_NV.MinValue = int.MinValue;

            LeftPosteriorImage_count_NV.enable = 0;
            LeftPosteriorImage_count_NV.Length = sizeof(int);
            LeftPosteriorImage_count_NV.Name = "Left posterior image count";
            LeftPosteriorImage_count_NV.value = 0;
            LeftPosteriorImage_count_NV.MaxValue = int.MaxValue;
            LeftPosteriorImage_count_NV.MinValue = int.MinValue;

            RightPosteriorImage_count_NV.enable = 0;
            RightPosteriorImage_count_NV.Length = sizeof(int);
            RightPosteriorImage_count_NV.Name = "Right posterior image count";
            RightPosteriorImage_count_NV.value = 0;
            RightPosteriorImage_count_NV.MaxValue = int.MaxValue;
            RightPosteriorImage_count_NV.MinValue = int.MinValue;

            DeletedImages_count_NV.enable = 0;
            DeletedImages_count_NV.Length = sizeof(int);
            DeletedImages_count_NV.Name = "Deleted images count";
            DeletedImages_count_NV.value = 0;
            DeletedImages_count_NV.MaxValue = int.MaxValue;
            DeletedImages_count_NV.MinValue = int.MinValue;

            ReportsMailed_counts_NV.enable = 0;
            ReportsMailed_counts_NV.Length = sizeof(int);
            ReportsMailed_counts_NV.Name = "Reports mailed count";
            ReportsMailed_counts_NV.value = 0;
            ReportsMailed_counts_NV.MaxValue = int.MaxValue;
            ReportsMailed_counts_NV.MinValue = int.MinValue;

            MacAddress_NV.enable = 0;
            MacAddress_NV.Length = 12;
            MacAddress_NV.Name = "MAC address";
            MacAddress_NV.value = "";

            latestdatetime_NV.enable = 0;
            latestdatetime_NV.Length = 14;
            latestdatetime_NV.Name = "Latest datetime";
            latestdatetime_NV.value = "";

            ActivationCode.enable = 0;
            ActivationCode.Length = sizeof(int);
            ActivationCode.Name = "Activation Code";
            ActivationCode.value = 0;
            ActivationCode.MaxValue = int.MaxValue;
            ActivationCode.MinValue = int.MinValue;

            IsPayPerUse.enable = 1;
            IsPayPerUse.Length = sizeof(byte);
            IsPayPerUse.Name = "Is Pay Per Use";
            IsPayPerUse.value = (byte)0;
            IsPayPerUse.MaxValue = (int)1;
            IsPayPerUse.MinValue = (int)0;

            IsFirstTime.enable = 0;
            IsFirstTime.Length = sizeof(byte);
            IsFirstTime.Name = "Is First Time";
            IsFirstTime.value = (byte)1;
            IsFirstTime.MaxValue = (int)1;
            IsFirstTime.MinValue = (int)0;

            ExpiryDate.enable = 0;
            ExpiryDate.Length = 8;
            ExpiryDate.Name = "Expiry Date";
            ExpiryDate.value = "";

            LicenseMode.enable = 0;
            LicenseMode.Length = sizeof(byte);
            LicenseMode.Name = "License Mode";
            LicenseMode.value = (byte)0;
            LicenseMode.MaxValue = (int)1;
            LicenseMode.MinValue = (int)0;

            NoOfVisits.enable = 0;
            NoOfVisits.Length = sizeof(int);
            NoOfVisits.Name = "No Of Visits";
            NoOfVisits.value = 0;
            NoOfVisits.MaxValue = int.MaxValue;
            NoOfVisits.MinValue = int.MinValue;

            NoOfLicensesElapsed.enable = 0;
            NoOfLicensesElapsed.Length = sizeof(int);
            NoOfLicensesElapsed.Name = "NoOfLicensesElapsed";
            NoOfLicensesElapsed.value = 0;
            NoOfLicensesElapsed.MaxValue = int.MaxValue;
            NoOfLicensesElapsed.MinValue = int.MinValue;

        }
    }
    internal class NVRAM_FIELDS
    {
        public byte enable;
        public byte Length;
        public string Name;
        public int MinValue;
        public int MaxValue;
        public object value;
        
    }

}

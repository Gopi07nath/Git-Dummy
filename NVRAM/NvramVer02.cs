using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Reflection;

namespace Nvram
{
    internal class NVRAM_VERSION_0_2
    {
      public  NvramSettings nvramSettings;
        public struct NvramSettings
        {
            public NVRAM_FIELDS NvramSetting;
            public NVRAM_FIELDS DeviceId;
            public NVRAM_FIELDS OptoMechanicalID;
            public NVRAM_FIELDS BomVersion_Major;
            public NVRAM_FIELDS BomVersion_Minor;

            public NVRAM_FIELDS ClassicSettings;

            public NVRAM_FIELDS EnableLensArtifact;
            public NVRAM_FIELDS LensArtifactTopX;
            public NVRAM_FIELDS LensArtifactTopY;
            public NVRAM_FIELDS LensArtifactBottomX;
            public NVRAM_FIELDS LensArtifactBottomY;

            public NVRAM_FIELDS RoyalSettings;
            public NVRAM_FIELDS HeightRefPoint;
            public NVRAM_FIELDS AOIRectangleX;
            public NVRAM_FIELDS AOIRectangleY;
            public NVRAM_FIELDS AOIRectangleWidth;
            public NVRAM_FIELDS AOIRectangleHeight;
            public NVRAM_FIELDS DisplayCentreX;
            public NVRAM_FIELDS DisplayCentreY;
            public NVRAM_FIELDS LEDThreshold;

            public NVRAM_FIELDS LiquidLensSettings;
            public NVRAM_FIELDS LiquidLensResetValue;
            public NVRAM_FIELDS LiquidLensSteps;
            public NVRAM_FIELDS LiquidLensCurrentValue;
            public NVRAM_FIELDS LiquidLensMaxValue;
            public NVRAM_FIELDS LiquidLensMinValue;
            public NVRAM_FIELDS LiquidLensSweepStepSize;
            public NVRAM_FIELDS RefractoGain;

            public NVRAM_FIELDS StatisticsSettings;
            public NVRAM_FIELDS PatientCount;
            public NVRAM_FIELDS TotalAnteriorImage_Count;
            public NVRAM_FIELDS TotalPosteriorImage_Count;
            public NVRAM_FIELDS LeftEyeRefraction_count;
            public NVRAM_FIELDS RightEyeRefraction_count;
            public NVRAM_FIELDS ReportsGenerated_count;
            public NVRAM_FIELDS ReportPrintouts_count;
            public NVRAM_FIELDS WhiteLightLED_ON_Duration;
            public NVRAM_FIELDS IR_LED_ON_Duration;
            public NVRAM_FIELDS RingIR_LED_ON_Duration;
            public NVRAM_FIELDS Film_LED_ON_Duration;
            public NVRAM_FIELDS CorneaFixtureIR_ON_Duration;
            public NVRAM_FIELDS CorneaLED_ON_Duration;

            public NVRAM_FIELDS LicensingSettings;
            public NVRAM_FIELDS ActivationCode;
            public NVRAM_FIELDS IsPayPerUse;//bool
            public NVRAM_FIELDS IsFirstTime;//bool
            public NVRAM_FIELDS ExpiryDate;
            public NVRAM_FIELDS LicenseMode;//string
            public NVRAM_FIELDS NoOfVisits;
            public NVRAM_FIELDS NoOfLicensesElapsed;


            public void initNvramSettings()
            {
                NvramSetting = new NVRAM_FIELDS();
                DeviceId = new NVRAM_FIELDS();
                OptoMechanicalID = new NVRAM_FIELDS();
                BomVersion_Major = new NVRAM_FIELDS();
                BomVersion_Minor = new NVRAM_FIELDS();

                ClassicSettings = new NVRAM_FIELDS();
                EnableLensArtifact = new NVRAM_FIELDS();
                LensArtifactTopX = new NVRAM_FIELDS();
                LensArtifactTopY = new NVRAM_FIELDS();
                LensArtifactBottomX = new NVRAM_FIELDS();
                LensArtifactBottomY = new NVRAM_FIELDS();

                RoyalSettings = new NVRAM_FIELDS();
                HeightRefPoint = new NVRAM_FIELDS();
                AOIRectangleX = new NVRAM_FIELDS();
                AOIRectangleY = new NVRAM_FIELDS();
                AOIRectangleWidth = new NVRAM_FIELDS();
                AOIRectangleHeight = new NVRAM_FIELDS();
                DisplayCentreX = new NVRAM_FIELDS();
                DisplayCentreY = new NVRAM_FIELDS(); ;
                LEDThreshold = new NVRAM_FIELDS();

                LiquidLensSettings = new NVRAM_FIELDS();
                LiquidLensResetValue = new NVRAM_FIELDS();
                LiquidLensSteps = new NVRAM_FIELDS();
                LiquidLensCurrentValue = new NVRAM_FIELDS();
                LiquidLensMaxValue = new NVRAM_FIELDS();
                LiquidLensMinValue = new NVRAM_FIELDS();
                LiquidLensSweepStepSize = new NVRAM_FIELDS();
                RefractoGain = new NVRAM_FIELDS();

                StatisticsSettings = new NVRAM_FIELDS();
                PatientCount = new NVRAM_FIELDS();
                TotalAnteriorImage_Count = new NVRAM_FIELDS();
                TotalPosteriorImage_Count = new NVRAM_FIELDS();
                LeftEyeRefraction_count = new NVRAM_FIELDS();
                RightEyeRefraction_count = new NVRAM_FIELDS();
                ReportsGenerated_count = new NVRAM_FIELDS();
                ReportPrintouts_count = new NVRAM_FIELDS();
                WhiteLightLED_ON_Duration = new NVRAM_FIELDS();
                IR_LED_ON_Duration = new NVRAM_FIELDS();
                RingIR_LED_ON_Duration = new NVRAM_FIELDS();
                Film_LED_ON_Duration = new NVRAM_FIELDS();
                CorneaFixtureIR_ON_Duration = new NVRAM_FIELDS();
                CorneaLED_ON_Duration = new NVRAM_FIELDS();

                LicensingSettings = new NVRAM_FIELDS();
               
                ActivationCode = new NVRAM_FIELDS();
                IsPayPerUse = new NVRAM_FIELDS();//bool
                IsFirstTime = new NVRAM_FIELDS();//bool
                ExpiryDate = new NVRAM_FIELDS();
                LicenseMode = new NVRAM_FIELDS();//string
                NoOfVisits = new NVRAM_FIELDS();
                NoOfLicensesElapsed = new NVRAM_FIELDS();


            }
        }

        public void initNvramSettings()
        {
           // FieldInfo[] finf = this.nvramSettings.GetType().GetFields();
            PropertyInfo[] pinf = this.nvramSettings.GetType().GetProperties();
            foreach (var item in pinf)
            {
                //if(item.GetType()==typeof(NVRAM_FIELDS))
                NVRAM_FIELDS nvf = new NVRAM_FIELDS();
                item.SetValue(this.nvramSettings,nvf, null);
               // NVRAM_FIELDS nvf = item.GetValue(this.nvramSettings) as NVRAM_FIELDS;
                //NVRAM_FIELDS nvf = item as NVRAM_FIELDS;
                //nvf = new NVRAM_FIELDS();
            }

        }

        public NVRAM_VERSION_0_2(bool isRoyal)
        {
            nvramSettings = new NvramSettings();
            nvramSettings.initNvramSettings();
           // initNvramSettings();
        }





    }
   
}

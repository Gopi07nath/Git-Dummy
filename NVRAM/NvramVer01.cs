using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Nvram
{
    internal class NVRAM_VERSION_0_1
        
        {
            public NvramSettings nvramSettings;
            bool isRoyaldevice = false;
           
            public struct NvramSettings
            {
                public NVRAM_FIELDS DeviceId;
                public NVRAM_FIELDS OptoMechanicalID;

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

                public void initNvramSettings()
                {
                    DeviceId = new NVRAM_FIELDS(); ;
                    DeviceId.Length = 16;
                    OptoMechanicalID = new NVRAM_FIELDS(); ;
                    OptoMechanicalID.Length = 7;
                    ClassicSettings = new NVRAM_FIELDS(); ;
                    EnableLensArtifact = new NVRAM_FIELDS(); ;
                    LensArtifactTopX = new NVRAM_FIELDS(); ;
                    LensArtifactTopY = new NVRAM_FIELDS(); ;
                    LensArtifactBottomX = new NVRAM_FIELDS(); ;
                    LensArtifactBottomY = new NVRAM_FIELDS(); ;

                    RoyalSettings = new NVRAM_FIELDS(); ;
                    HeightRefPoint = new NVRAM_FIELDS(); ;
                    AOIRectangleX = new NVRAM_FIELDS(); ;
                    AOIRectangleY = new NVRAM_FIELDS(); ;
                    AOIRectangleWidth = new NVRAM_FIELDS(); ;
                    AOIRectangleHeight = new NVRAM_FIELDS(); ;
                    DisplayCentreX = new NVRAM_FIELDS(); ;
                    DisplayCentreY = new NVRAM_FIELDS(); ;
                    LEDThreshold = new NVRAM_FIELDS(); ;

                }
            };
            public void initNvramSettings()
            {
               FieldInfo[] finf =  this.nvramSettings.GetType().GetFields();
               foreach (FieldInfo item in finf)
               {
                   //if(item.GetType()==typeof(NVRAM_FIELDS))
                   NVRAM_FIELDS nvf = item.GetValue(this.nvramSettings) as NVRAM_FIELDS;
                   //NVRAM_FIELDS nvf = item as NVRAM_FIELDS;
                   nvf = new NVRAM_FIELDS();
               }

            }


            public NVRAM_VERSION_0_1(bool isRoyal)
            {

                nvramSettings = new NvramSettings();
                nvramSettings.initNvramSettings();
               // initNvramSettings();
            }

        }
 }

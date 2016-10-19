using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Globalization;
namespace Nvram
{
    public class NvramHelper
    {
        private NVRAM nvram;
        private NVRAM_VERSION_0_1 Version01;
        private NVRAM_VERSION_0_2 Version02;
        private NVRAM_VERSION_0_3 Version03;
        #region Enum for Set and Get NVRAM Values to be changed for every version of nvram
        public enum CommonSettings
        {
            DeviceId, OptoMechanicalID, BomVersion_Major, BomVersion_Minor
        }
        public enum RoyalSettings
        {
            //HighRefPoint,
            AOIRectangleX, AOIRectangleY, AOIRectangleWidth, AOIRectangleHeight, DisplayCentreX,
            DisplayCentreY, LEDThreshold, LiquidLensResetValue, LiquidLensSteps, LiquidLensCurrentValue, LiquidLensMinValue,
            LiquidLensMaxValue, LiquidLensSweepStepSize, RefractoGain
        }
        public enum ClassicSettings
        {
            EnableLensArtifact,
            LensArtifactTopX, LensArtifactTopY, LensArtifactBottomX, LensArtifactBottomY,
        }
        public enum StatisticsSettings
        {
            PatientCount_NV,
            LeftEyeRefraction_count_NV, RightEyeRefraction_count_NV, ReportsGenerated_count_NV, ReportPrintouts_count_NV, WhiteLightLED_ON_Duration_NV,
            IR_LED_ON_Duration_NV, RingIR_LED_ON_Duration_NV, Film_LED_ON_Duration_NV, CorneaFixtureIR_ON_Duration_NV, CorneaLED_ON_Duration_NV,
            NewVisit_count_NV, LeftAnteriorImage_count_NV, RightAnteriorImage_count_NV, LeftPosteriorImage_count_NV, RightPosteriorImage_count_NV,
            DeletedImages_count_NV, ReportsMailed_counts_NV, MacAddress_NV, latestdatetime_NV,
        }
        public enum LicensingSettings
        {
            ActivationCode, IsPayPerUse, IsFirstTime, ExpiryDate, LicenseMode, NoOfVisits, NoOfLicensesElapsed
        }
        #endregion
        private byte[] nvram_buf;
        //private string currentNvramVersion = "0.2";
        private string newNvramVersion = "0.3";
        public const Double CURRENT_NVRAM_VERSION = 0.3;
        private string existingNvramVersion = " ";
        public static Dictionary<string, object> nvramDic;
        private FieldInfo[] fieldInfo;
        private int fieldInfoIndx = 0;
        private byte[] byteArray;
        private object obj;
        private bool isRoyalDevice = false;
        public bool WriteNvram = false;
        public string NewLine = Environment.NewLine;
        public NvramHelper(bool isRoyal, byte[] Nvram_Arr)
        {
            nvramDic = new Dictionary<string, object>();
            nvram = new NVRAM(isRoyal);
            isRoyalDevice = isRoyal;
            Type tdat = typeof(NVRAM);
            fieldInfo = tdat.GetFields();

            getVersionFromNvram(Nvram_Arr);
            
        }

        public void CreateDumpFile(string path)
        {
            string str = GetNvram();
           string devID =  ((NVRAM_FIELDS)(nvramDic["DeviceId"])).value.ToString();
            StreamWriter stWriter = new StreamWriter("Nvram_" + devID + ".txt");
            stWriter.WriteLine(str);
            stWriter.Close();
            stWriter.Dispose();
            if (!string.IsNullOrEmpty(path))
                File.Copy("Nvram_" + devID + ".txt", path + Path.DirectorySeparatorChar + "Nvram_" + devID + ".txt", true);

        }

        public string GetNvram()
        {
            string retStr ="";
            
            foreach (KeyValuePair<string, object> keyVal in nvramDic)
            {
               // if (keyVal.Value != null)
                NVRAM_FIELDS nvf = keyVal.Value as NVRAM_FIELDS;
                if(nvf.value != null)
                    retStr +=nvf.Name + " = " + nvf.value.ToString() +NewLine;
            }
            return retStr;
        }
        public bool ShowNvramDialog()
        {
            NVRAMForm nvramF = new NVRAMForm(nvramDic);
            nvramF.ShowDialog();
            return (WriteNvram = nvramF.saveBtnclicked);

        }
        public object GetNvramValue(object nvramDataID)
        {
            switch (newNvramVersion)
            {
                case "0.1":
                    {
                        getValue(Version01.nvramSettings, nvramDataID);
                        break;
                    }
                case "0.2":
                    {
                        getValue(Version02.nvramSettings, nvramDataID);
                        break;
                    }
                case "0.3":
                    {
                        getValue(Version03.nvramSettings, nvramDataID);
                        break;
                    }
            }
            return obj;
        }

        /// <summary>
        /// sets the value to the nvram.
        /// </summary>
        /// <param name="nvramEnums"> is obtained by typing nvramhelper.currentNvram.NvramValueID</param>
        /// <param name="value"></param>
        public void SetNvramValue(object nvramDataID, object value)
        {
            switch (newNvramVersion)
            {
                case "0.1":
                    {
                        setValue(Version01.nvramSettings, nvramDataID, value);
                        break;
                    }
                case "0.2":
                    {
                        setValue(Version02.nvramSettings, nvramDataID, value);
                        break;
                    }
                case "0.3":
                    {
                        setValue(Version03.nvramSettings, nvramDataID, value);
                        break;
                    }
            }
        }

        public byte[] GetNvramByteArray()
        {

            switch (newNvramVersion)
            {
                case "0.1":
                    {
                        getByteArray(Version01.nvramSettings);
                        break;
                    }
                case "0.2":
                    {
                        getByteArray(Version02.nvramSettings);
                        break;
                    }
                case "0.3":
                    {
                        getByteArray(Version03.nvramSettings);
                        break;
                    }
            }
            return nvram_buf;
        }

        #region Private Methods
        private void VersionInitialize(string nvramVersion)
        {
            switch (nvramVersion)
            {
                case "0.1":
                    {
                        Version01 = new NVRAM_VERSION_0_1(isRoyalDevice);
                        initializeNvram(Version01.nvramSettings);
                        Version01.nvramSettings.DeviceId.Length = 16;
                        Version01.nvramSettings.OptoMechanicalID.Length = 7;
                        Version01.nvramSettings.EnableLensArtifact.Length = 5;
                        break;
                    }
                case "0.2":
                    {
                        Version02 = new NVRAM_VERSION_0_2(isRoyalDevice);
                        initializeNvram(Version02.nvramSettings);
                        break;
                    }
                case "0.3":
                    {
                        Version03 = new NVRAM_VERSION_0_3(isRoyalDevice);
                        initializeNvram(Version03.nvramSettings);
                        break;
                    }
            }

        }
        private void getByteArray(object nvramVersionSettings)
        {
            nvram_buf = new byte[256];
            int byteIndx = 2;
            string[] strArr = newNvramVersion.Split('.');
            byte temp1 = Convert.ToByte(strArr[0]);
            byte temp2 = Convert.ToByte(strArr[1]);

            nvram_buf[0] = temp1;
            nvram_buf[1] = temp2;
            Type t = nvramVersionSettings.GetType();
            FieldInfo[] fieldInfoArr = t.GetFields();
            int intRes=0;
            byte byteRes=0;
            foreach (FieldInfo item in fieldInfoArr)
            {
                object obj = item.GetValue(nvramVersionSettings);
                if (obj.GetType() == typeof(NVRAM_FIELDS))
                {
                    NVRAM_FIELDS nvF = obj as NVRAM_FIELDS;
                    byte[] bytes = new byte[nvF.Length];
                    if (nvF.Name != null)
                    {
                        Console.WriteLine(nvF.Name + "  " + nvF.value.GetType() + "  " +nvF.value + "  " + byteIndx);

                        if (nvF.value.GetType() == typeof(string))
                        {
                            string val = nvF.value as string;
                            bytes = Encoding.UTF8.GetBytes(val);
                            Array.Copy(bytes, 0, nvram_buf, byteIndx, bytes.Length);
                            byteIndx += nvF.Length;
                        }
                        else if (int.TryParse( nvF.value.ToString(),out intRes) && nvF.Length == sizeof(int))
                        {
                           // int val = Convert.ToInt32(nvF.value);
                            bytes = BitConverter.GetBytes(intRes);
                            Array.Copy(bytes, 0, nvram_buf, byteIndx, bytes.Length);
                            byteIndx += sizeof(int);

                        }
                        else if(byte.TryParse(nvF.value.ToString(),out byteRes) && nvF.Length == sizeof(byte))
                        {
                            bytes[0] = byteRes;
                            Array.Copy(bytes, 0, nvram_buf, byteIndx, bytes.Length);
                            byteIndx += sizeof(byte);

                        }
                    }
                }

            }
        }
        private void setValue(object nvramVersionSettings, object nvramDataID, object value)
        {
            Type t = nvramVersionSettings.GetType();
            FieldInfo[] f = t.GetFields();
            foreach (var field in f)
            {
                obj = field.GetValue(nvramVersionSettings);

                if (obj.GetType() == typeof(NVRAM_FIELDS))
                {
                    if (Enum.GetName(nvramDataID.GetType(), nvramDataID) == field.Name)
                    {
                        NVRAM_FIELDS nvF = obj as NVRAM_FIELDS;
                        nvF.value = value;
                        break;
                    }
                }
            }
        }
        private void setValue(ref NVRAM_FIELDS val)
        {
            if (fieldInfoIndx != -1)
            {
                NVRAM_FIELDS tempField = fieldInfo[fieldInfoIndx].GetValue(nvram) as NVRAM_FIELDS;
                val.Length = tempField.Length;
                val.enable = tempField.enable;
                val.MaxValue = tempField.MaxValue;
                val.MinValue = tempField.MinValue;
                val.Name = tempField.Name;
                val.value = tempField.value;
            }
        }
        private void getValue(object nvramVersionSettings, object nvramDataID)
        {

            Type t = nvramVersionSettings.GetType();
            FieldInfo[] f = t.GetFields();
            foreach (var field in f)
            {
                obj = field.GetValue(nvramVersionSettings);

                if (obj.GetType() == typeof(NVRAM_FIELDS))
                {
                    if (Enum.GetName(nvramDataID.GetType(), nvramDataID) == field.Name)
                    {
                        NVRAM_FIELDS nvF = obj as NVRAM_FIELDS;
                        if (nvF.Length == 1)
                            obj = Convert.ToByte(nvF.value);
                        else
                            obj = nvF.value;
                        Type typ = obj.GetType();
                        break;
                    }
                }
            }
        }
        private void SetNvramByteArray2Struct(byte[] nvramValues, object nvramSettingVersion)
        {
            int indx = 0;
            int count = 0;
            byteArray = new byte[nvramValues.Length];
            Array.Copy(nvramValues, byteArray, nvramValues.Length);


            foreach (var field in (nvramSettingVersion).GetType().GetFields(BindingFlags.Instance |
                                                 BindingFlags.NonPublic |
                                                 BindingFlags.Public))
            {
                object obj = field.GetValue(nvramSettingVersion);
                if (obj.GetType() == typeof(NVRAM_FIELDS))
                {
                    NVRAM_FIELDS nvF = obj as NVRAM_FIELDS;
                    setValue(ref nvF, ref indx);
                }

            }
        }
        private void initializeNvram(object NvramStructure)
        {

            Type t = NvramStructure.GetType();
            FieldInfo[] f = t.GetFields();
            foreach (var field in f)
            {
                obj = field.GetValue(NvramStructure);

                if (obj.GetType() == typeof(NVRAM_FIELDS))
                {
                    string str = field.Name;
                    GetFieldInfoIndx(str);
                    NVRAM_FIELDS nvF = obj as NVRAM_FIELDS;
                    setValue(ref nvF);

                }
            }
        }



        private void getVersionFromNvram(byte[] NvramArr)
        {
            existingNvramVersion = NvramArr[0].ToString() + "." + NvramArr[1].ToString();
            nvram_buf = new byte[NvramArr.Length - 2];
            Array.Copy(NvramArr, 2, nvram_buf, 0, nvram_buf.Length - 2);
            if (newNvramVersion != existingNvramVersion)
            {
                double currentVersion = Convert.ToDouble(newNvramVersion, CultureInfo.InvariantCulture);
                double existingVersion = Convert.ToDouble(existingNvramVersion, CultureInfo.InvariantCulture);


                if (existingVersion > currentVersion)
                {
                    StreamWriter st = new StreamWriter(existingNvramVersion + "_settings.txt");
                    foreach (byte b in NvramArr)
                        st.WriteLine(b);
                    short val  =(short) NvramArr.Sum(x=>x);
                  st.WriteLine( val);
                    st.Dispose();
                }
                VersionInitialize(existingNvramVersion);
                VersionInitialize(newNvramVersion);
                SetNvram(existingNvramVersion);
                NvramToDictionary(existingNvramVersion);
                SetNvramFromDic();
                NvramToDictionary(newNvramVersion);
                WriteNvram = true;

            }
            else
            {
                VersionInitialize(newNvramVersion);
                SetNvram(existingNvramVersion);
                NvramToDictionary(newNvramVersion);
                WriteNvram = false;
            }

        }
        private void saveNvram2Xml(object version,string versionNum)
        {
            XmlDocument xml = new XmlDocument();
            MemoryStream memStream = new MemoryStream();
            XmlWriter xmlStreamWriter = XmlWriter.Create(memStream);
            switch (versionNum)
            {
                case "0.1":
                    {
                        XmlSerializer xmlSerialization = new XmlSerializer((Version01.nvramSettings).GetType());
                        xmlSerialization.Serialize(xmlStreamWriter, Version01.nvramSettings);
                        break;
                    }
                case "0.2":
                    {
                        XmlSerializer xmlSerialization = new XmlSerializer((Version02.nvramSettings).GetType());
                        xmlSerialization.Serialize(xmlStreamWriter, Version02.nvramSettings);
                        break;
                    }
                case "0.3":
                    {
                        XmlSerializer xmlSerialization = new XmlSerializer((Version03.nvramSettings).GetType());
                        xmlSerialization.Serialize(xmlStreamWriter, Version03.nvramSettings);
                        break;
                    }
            }
          
            memStream.Position = 0;
            xml.Load(memStream);
            xml.Save(versionNum + "_settings.xml");
            memStream.Dispose();
        }
        private void SetNvramToDic(object VersionSettings)
        {
            nvramDic.Clear();
            nvramDic = new Dictionary<string, object>();

            foreach (var field in VersionSettings.GetType().GetFields(BindingFlags.Instance |
                                                 BindingFlags.NonPublic |
                                                 BindingFlags.Public))
            {
                object obj = field.GetValue(VersionSettings);
                if (obj.GetType() == typeof(NVRAM_FIELDS))
                {
                    NVRAM_FIELDS nvF = obj as NVRAM_FIELDS;
                    nvramDic.Add(field.Name, nvF);
                }
            }

        }

        private void setValue(ref NVRAM_FIELDS val, ref int indx)
        {
            byte[] bytes = new byte[val.Length];
            Array.Copy(byteArray, indx, bytes, 0, val.Length);

            if (val.Name != null)
            {
                if (val.value.GetType() == typeof(string))
                {
                    val.value = Encoding.UTF8.GetString(bytes);
                    string str = val.value.ToString().Replace("\0", string.Empty).Trim();
                    val.value = str;
                }
                else if (val.value.GetType() == typeof(int))
                {
                    Array.Copy(byteArray, indx, bytes, 0, bytes.Length);
                    val.value = BitConverter.ToInt32(bytes, 0);
                }
                else
                {
                    val.value = (byte)bytes[0];

                }
                Console.WriteLine(val.Name + "  " + val.GetType() + "   " + val.value + "  " + indx);

                indx += val.Length;
            }
        }

        private void GetFieldInfoIndx(string fieldInfoName)
        {
            fieldInfoIndx = -1;
            for (int i = 0; i < fieldInfo.Length; i++)
            {
                if (fieldInfoName == fieldInfo[i].Name)
                {
                    fieldInfoIndx = i;
                    break;
                }
            }

        }

        private void SetNvram(string versionNum)
        {
            switch (versionNum)
            {
                case "0.1":
                    {
                        SetNvramByteArray2Struct(nvram_buf, Version01.nvramSettings);
                        break;
                    }
                case "0.2":
                    {

                        SetNvramByteArray2Struct(nvram_buf, Version02.nvramSettings);
                        break;
                    }
                case "0.3":
                    {
                        SetNvramByteArray2Struct(nvram_buf, Version03.nvramSettings);
                        break;
                    }
                default:
                    {
                        
                        break;
                    }

            }
        }

        private void SetNvramFromDic()
        {
            switch (newNvramVersion)
            {
                case "0.1":
                    {
                        GetNvramFromDic(nvramDic, Version01.nvramSettings);

                        break;
                    }
                case "0.2":
                    {
                        GetNvramFromDic(nvramDic, Version02.nvramSettings);
                        break;
                    }
                case "0.3":
                    {
                        GetNvramFromDic(nvramDic, Version03.nvramSettings);
                        break;
                    }
            }
        }

        private void GetNvramFromDic(Dictionary<string, object> nvramDic, object nvramSettings)
        {
            Type t = nvramSettings.GetType();
            FieldInfo[] fields = t.GetFields();
            foreach (FieldInfo fieldVal in fields)
            {
                obj = fieldVal.GetValue(nvramSettings);

                if (obj.GetType() == typeof(NVRAM_FIELDS))
                {
                    NVRAM_FIELDS nvF = obj as NVRAM_FIELDS;
                    object tempObj = new object();
                    nvramDic.TryGetValue(fieldVal.Name, out tempObj);
                    if (tempObj != null)
                    {
                        NVRAM_FIELDS tempFields = tempObj as NVRAM_FIELDS;
                        nvF.value = tempFields.value;
                    }
                }
            }
        }


        private void NvramToDictionary(string versionNum)
        {
            switch (versionNum)
            {
                case "0.1":
                    {
                        SetNvramToDic(Version01.nvramSettings);
                        break;
                    }
                case "0.2":
                    {
                        SetNvramToDic(Version02.nvramSettings);

                        break;
                    }
                case "0.3":
                    {
                        SetNvramToDic(Version03.nvramSettings);

                        break;
                    }
                default:
                    {
                       NvramToDictionary(newNvramVersion);
                       break;

                    }
            }

        }

        #endregion
    }

}
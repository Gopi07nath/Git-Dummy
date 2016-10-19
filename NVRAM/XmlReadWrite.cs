using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Nvram
{

    internal class XmlReadWrite
    {
        public String fileName = null;
        MemoryStream memStream ;
        XmlSerializer xmlSerialization;
        public XmlReadWrite()
        {
        }
        public XmlReadWrite(String fname)
        {
            fileName = fname;
        }
        private static Dictionary<String, Object> dic;

        public void ReadXml(Dictionary<string,object> nvramDic)
        {
                dic = NvramHelper.nvramDic;
        }

        public KeyValuePair<String, Object> GetRoot()
        {
            if (dic.Count > 0)
            {
                KeyValuePair<String, Object> root = dic.First();
                return root;
            }
            else
            {
                return new KeyValuePair<string, object>();
            }
        }


        private static void objectConvert(string val, ref object destVal )
        {
            if (destVal.GetType() == typeof(int))
            {
                destVal = Convert.ToInt32(val);
            }
            else if (destVal.GetType() == typeof(byte))
            {
                destVal = Convert.ToByte(val);
            }
            else
                destVal = val;

        }
        public static  void SetValue(String node, String val)
        {
            string[] parts = node.Split('.');
            Dictionary<String, Object> tmp = dic;

                foreach (KeyValuePair<string,object> item in dic)
                {
                    NVRAM_FIELDS nvF = item.Value as NVRAM_FIELDS;

                    if (nvF.Name != null)
                    {
                        if (nvF.Name == parts[1])
                        {
                            objectConvert(val, ref nvF.value);
                            break;
                        }
                    }
                    else
                        continue;
                }
 

        }

    }
}

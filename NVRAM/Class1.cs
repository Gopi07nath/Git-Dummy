using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace NVRAM
{
    public class NVRAMVERSION_00
    {
         NVRAM version00 = new NVRAM();
        List<NVRAM_FIELDS> versionFields = new List<NVRAM_FIELDS>();
        private void setVersion00()
        {
            version00.field1.enable = 0;
            versionFields.Add(version00.field1);

            version00.field2.enable = 0;
            versionFields.Add(version00.field2);

            version00.field3.enable = 0;
            versionFields.Add(version00.field3);

            version00.field4.enable = 0;
            versionFields.Add(version00.field4);

            version01.field5.enable = 0;
            versionFields.Add(version00.field5);

            version00.field6.enable = 0;
            versionFields.Add(version00.field6);

            version00.field7.enable = 0;
            versionFields.Add(version00.field7);

            version00.field8.enable = 0;
            versionFields.Add(version00.field8);

            version00.field9.enable = 0;
            versionFields.Add(version00.field9);

            version00.field10.enable = 0;
            versionFields.Add(version00.field10);

            version00.field11.enable = 0;
            versionFields.Add(version00.field11);

            version00.field12.enable = 0;
            versionFields.Add(version00.field12);

            version00.field13.enable = 0;
            versionFields.Add(version00.field13);

            version00.field14.enable = 0;
            versionFields.Add(version00.field14);

            version00.field15.enable = 0;
            versionFields.Add(version00.field15);

            version00.field16.enable = 0;
            versionFields.Add(version00.field16);

            version00.field17.enable = 0;
            versionFields.Add(version00.field17);
        }
        public List<NVRAM_FIELDS> getVersion00()
        {
            setVersion00();
            return versionFields;
        }
    }

    public class NVRAMVERSION_01
    {
         NVRAM version01 = new NVRAM();
        List<NVRAM_FIELDS> versionFields = new List<NVRAM_FIELDS>();
        private void setVersion01()
        {
            version01.field1.enable = 1;
            versionFields.Add(version01.field1);

            version01.field2.enable = 1;
            versionFields.Add(version01.field2);

            version01.field3.enable = 1;
            versionFields.Add(version01.field3);

            version01.field4.enable = 1;
            versionFields.Add(version01.field4);

            version01.field5.enable = 1;
            versionFields.Add(version01.field5);

            version01.field6.enable = 1;
            versionFields.Add(version01.field6);

            version01.field7.enable = 1;
            versionFields.Add(version01.field7);

            version01.field8.enable = 1;
            versionFields.Add(version01.field8);

            version01.field9.enable = 1;
            versionFields.Add(version01.field9);

            version01.field10.enable = 1;
            versionFields.Add(version01.field10);

            version01.field11.enable = 1;
            versionFields.Add(version01.field11);

            version01.field12.enable = 1;
            versionFields.Add(version01.field12);

            version01.field13.enable = 1;
            versionFields.Add(version01.field13);

            version01.field14.enable = 1;
            versionFields.Add(version01.field14);

            version01.field15.enable = 1;
            versionFields.Add(version01.field15);

            version01.field16.enable = 1;
            versionFields.Add(version01.field16);

            version01.field17.enable = 0;
            versionFields.Add(version01.field17);
        }
        public List<NVRAM_FIELDS> getVersion01()
        {
            setVersion01();
            return versionFields;
        }
        public void setDefaultValues()
        {
            

        }

    }

    public class NVRAMVERSION_02
    {
         NVRAM version02 = new NVRAM();
        List<NVRAM_FIELDS> versionFields = new List<NVRAM_FIELDS>();
        private void setVersion02()
        {
            version02.field1.enable = 1;
            versionFields.Add(version02.field1);

            version02.field2.enable = 1;
            versionFields.Add(version02.field2);

            version02.field3.enable = 1;
            versionFields.Add(version02.field3);

            version02.field4.enable = 1;
            versionFields.Add(version02.field4);

            version02.field5.enable = 1;
            versionFields.Add(version02.field5);

            version02.field6.enable = 1;
            versionFields.Add(version02.field6);

            version02.field7.enable = 1;
            versionFields.Add(version02.field7);

            version02.field8.enable = 1;
            versionFields.Add(version02.field8);

            version02.field9.enable = 1;
            versionFields.Add(version02.field9);

            version02.field10.enable = 1;
            versionFields.Add(version02.field10);

            version02.field11.enable = 1;
            versionFields.Add(version02.field11);

            version02.field12.enable = 1;
            versionFields.Add(version02.field12);

            version02.field13.enable = 1;
            versionFields.Add(version02.field13);

            version02.field14.enable = 1;
            versionFields.Add(version02.field14);

            version02.field15.enable = 1;
            versionFields.Add(version02.field15);

            version02.field16.enable = 1;
            versionFields.Add(version02.field16);

            version02.field17.enable = 1;
            versionFields.Add(version02.field17);
        }

        public List<NVRAM_FIELDS> getVersion02()
        {
            setVersion02();
            return versionFields;
        }
    }


}

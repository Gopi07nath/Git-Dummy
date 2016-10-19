using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Resources;
using System.Xml;
namespace Nvram
{
    internal delegate void SaveBtnClicked(object sender, EventArgs e);
    internal partial class Config_UC : UserControl
    {
        public event SaveBtnClicked saveClicked;
        private string password = null;
        public Dictionary<string, object> nvramDic;
        //Multilang:This function initializes the string ID's in this file
        private void initializeLang()
        {           
            
            //label2.Text = Globals.languageResource.GetString("STRING_Config_UC_label2");
            //save_btn.Text = Globals.languageResource.GetString("STRING_Config_UC_save_btn");
        }

        public Config_UC()
        {
            InitializeComponent();
            initializeLang();
        }
        public void Setup(String fileName,Dictionary<string,object> nvramDic)
        {
                xmlRW.ReadXml(nvramDic);
                foreach (KeyValuePair<string, object> k in nvramDic)
                    addLeaf(k, 1, null, 0, 0);

            string date = DateTime.Now.Day.ToString();
            string month = DateTime.Now.Month.ToString();
            if (date.Length == 1)
            {
                date = "0" + DateTime.Now.Day.ToString();
            }

            if (month.Length == 1)
            {
                month = "0" + DateTime.Now.Month.ToString();
            }

            password = date + month;
        }

        XmlReadWrite xmlRW = new XmlReadWrite();

        KeyValuePair<String, Object> rootDict;
        int vIndent = 10;
        int hIndent = 10;
        

        private void addLeaf(KeyValuePair<String, Object> leaf, int lvl, String parentName, int minVal, int maxVal)
        {
            NVRAM_FIELDS nvF = leaf.Value as NVRAM_FIELDS;
            XmlLeaf_UC node;
            {
                if (nvF.Name != null)
                {
                    if (nvF.enable == 1)
                    {
                        node = new XmlLeaf_UC(nvF.Name, nvF.value.ToString(), parentName, lvl, nvF.MinValue, nvF.MaxValue);
                        node.Left = hIndent + 5 * lvl;
                        node.Top = vIndent;
                        this.panel1.Controls.Add(node);
                        vIndent += 30;
                    }
                }
                else
                {
                    if (nvF.enable == 1)
                    {
                        if(leaf.Key.ToLower().Contains("nvram"))
                        node = new XmlLeaf_UC(leaf.Key, nvF.value.ToString(), parentName, 3, 0, 0);
                        else
                        node = new XmlLeaf_UC(leaf.Key, null, parentName, 2, 0, 0);
                        node.Left = hIndent + 5 * lvl;
                        node.Top = vIndent;
                        this.panel1.Controls.Add(node);
                        vIndent += 30;
                    }
                }
              
            }
        }
        private bool checkPasswd(string passwd)
        {
            if (passwd == password) return true;
            else return false;
        }

        private void passwd_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (checkPasswd(passwd_tbx.Text.Trim()))
                {
                    password_pnl.Visible = false;
                    panel1.Visible = true;
                    save_btn.Visible = true;
                   
                }
                else
                {
                    //STRING_Config_UC_046
                    //MessageBox.Show(Globals.languageResource.GetString("STRING_Config_UC_046"), Globals.languageResource.GetString("STRING_3nethra"));
                }
              
            }
        }

		private void save_btn_Click(object sender, EventArgs e)
        {

            saveClicked(null, null);
		}
		void dummy(Object sender, EventArgs e) {
		}
      
    } 
}

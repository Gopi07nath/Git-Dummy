using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;


namespace Nvram
{
    internal partial class NVRAMForm : Form
    {
       public bool saveBtnclicked = false;
        public NVRAMForm(Dictionary<string,object> NvramDic)
        {
            InitializeComponent(); Console.WriteLine("Constructor");
            Config_UC con = new Config_UC();
            con.saveClicked += new SaveBtnClicked(con_saveClicked);

            con.Setup("",NvramHelper.nvramDic);
            this.Controls.Add(con);

        }

        void con_saveClicked(object sender, EventArgs e)
        {
            Dictionary<string, object> dic = NvramHelper.nvramDic;
            saveBtnclicked = true;
            this.Close();
        }

        private void NVRAMForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                saveBtnclicked = false;
                this.Close();
            }
        }

    }
}

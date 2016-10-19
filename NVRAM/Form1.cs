using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;

namespace Nvram
{
    internal partial class Form1 : Form
    {
        byte[] nvram_buf;
        public Form1()
        {
            InitializeComponent();
        }


        private void readNvram()
        {
            
            nvramHelper = new NvramHelper(true, nvram_buf);
            writeNvram();
        }
        NvramHelper nvramHelper;
        private void button1_Click(object sender, EventArgs e)
        {
            readNvram();

        }


        private void writeNvram()
        {
            nvram_buf = nvramHelper.GetNvramByteArray();
            //tricam.WriteNVRAM(nvram_buf);
        }

        private void WriteNvram2File(string nvramString)
        {
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            StreamWriter st = new StreamWriter(fileName);
            st.WriteLine(nvramString);

            st.Flush();
            st.Close();
            st.Dispose();
            System.Diagnostics.Process.Start(fileName);

        }
        private void button2_Click(object sender, EventArgs e)
        {
            writeNvram();
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
        }

        


    }

        private void button3_Click(object sender, EventArgs e)
        {
            NVRAMForm nvramF = new NVRAMForm(NvramHelper.nvramDic);
            if (nvramF.ShowDialog() == DialogResult.Cancel)
            {
               // nvramHelper.nvramDic = Globals.nvramDic;
                writeNvram();
            }
        }
        }

}

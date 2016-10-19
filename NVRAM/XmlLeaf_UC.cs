using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Nvram
{
    internal partial class XmlLeaf_UC : UserControl
    {
        string parent = null;
        RadioButton trb;
        RadioButton frb;

        TextBox textBox,textBox1, textBox2;
        NumericUpDown numericUpDown;
        public XmlLeaf_UC(string lbl, string val, string parents, int lvl,int min,int max)
        {
            InitializeComponent();
            
            label1.Text = lbl;
            this.parent = parents;
            bool b = false;
            int upDownVal = 0;
            string val1 = "";
            if (val == null)
            {
                //textBox.Hide();
                label1.Dock = DockStyle.Left;
                label1.Font = new Font("Arial", 11 - lvl, FontStyle.Bold);
               

                switch (lvl)
                {
                    case 1:
                        label1.ForeColor = Color.RoyalBlue;
                        break;
                    case 2:
                        label1.ForeColor = Color.OrangeRed; ;

                        break;
                    case 3:
                        label1.ForeColor = Color.DarkViolet;
                        break;
                }
                
                return;
            }
            val1=val.ToLower();
            try
            {
                //if (Globals.isNvram)
                {
                    if (lbl.ToLower().Contains("enablelens"))
                    {
                            frb = new RadioButton();
                            frb.Size = new Size(60, 10);
                            frb.Text = "False";
                            frb.Dock = DockStyle.Left;
                            frb.Parent = panel1;

                            trb = new RadioButton();
                            trb.Size = new Size(60, 10);
                            trb.Text = "True";
                            trb.Dock = DockStyle.Left;
                            trb.Parent = panel1;

                            if (val1 == "0")
                            {
                                frb.Checked = true;
                            }
                            else
                            {
                                trb.Checked = true;

                            }
                            trb.CheckedChanged += new EventHandler(trb_CheckedChanged);
                    } 
                   // else
                        if (Int32.TryParse(val, out upDownVal))
                        {
                            numericUpDown = new NumericUpDown();
                            numericUpDown.Parent = this.panel1;
                            numericUpDown.Size = new Size(70, 20);
                            numericUpDown.Minimum = (decimal)min;
                            numericUpDown.Maximum = (decimal)max;
                            numericUpDown.Value = (decimal)upDownVal;
                            numericUpDown.ValueChanged += new EventHandler(numericUpDown_ValueChanged);

                        }
                        else
                        {
                            if (lbl.Contains("Device"))
                            {
                                GenerateDeviceIDTextBoxes(val);
                                return;
                            }

                            if (lbl.ToLower().Contains("opto"))
                            {
                                GenerateOptoMechIDTextBoxes(val);
                                return;
                            }
                        }

                    }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                System.Windows.Forms.SendKeys.Send("{TAB}");
                e.SuppressKeyPress = true;
            }
        }

        void textBox2_Leave(object sender, EventArgs e)
        {
            string str = this.parent + "." + label1.Text;
            if (label1.Text.Contains("Device"))
                XmlReadWrite.SetValue(str, textBox2.Text + textBox1.Text + textBox.Text);
        }

        void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                System.Windows.Forms.SendKeys.Send("{TAB}");
                e.SuppressKeyPress = true;
            }
        }

        void textBox1_Leave(object sender, EventArgs e)
        {
            string str = this.parent + "." + label1.Text;
            if (label1.Text.Contains("Device"))
                XmlReadWrite.SetValue(str, textBox2.Text + textBox1.Text + textBox.Text);
            else if (label1.Text.ToLower().Contains("optomech"))
                XmlReadWrite.SetValue(str, textBox1.Text + textBox.Text);
        }

        void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            string str = this.parent + "." + label1.Text;
            XmlReadWrite.SetValue(str, numericUpDown.Value.ToString());
        }
       

        void trb_CheckedChanged(object sender, EventArgs e)
        {
            string str = this.parent + "." + label1.Text;
            if (label1.Text.ToLower().Contains("enablelensartifact"))
            {
               byte byteVal =0;
                if (trb.Checked)
                {
                    byteVal = 1;
                    XmlReadWrite.SetValue(str,byteVal.ToString());
                }
                else
                {
                    XmlReadWrite.SetValue(str, byteVal.ToString());
                }
            }
            else if (trb.Checked)
            {
                XmlReadWrite.SetValue(str, trb.Text.ToLower());
            }
            else
            {
                XmlReadWrite.SetValue(str, frb.Text.ToLower());
            }
        }

        void textBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                System.Windows.Forms.SendKeys.Send("{TAB}");
                e.SuppressKeyPress = true;
            }
        }

        void textBox_Leave(object sender, EventArgs e)
        {
            
                 string str = this.parent + "." + label1.Text;
                 if (label1.Text.Contains("Device"))
                     XmlReadWrite.SetValue(str, textBox2.Text + textBox1.Text + textBox.Text);
                 else if (label1.Text.ToLower().Contains("opto"))
                     XmlReadWrite.SetValue(str, textBox1.Text + textBox.Text);
                 else
            XmlReadWrite.SetValue(str, textBox.Text);
        }
        private void GenerateDeviceIDTextBoxes(string val )
        {
            textBox = new TextBox();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            textBox.CharacterCasing = CharacterCasing.Upper;
            textBox1.CharacterCasing = CharacterCasing.Upper;
            textBox2.CharacterCasing = CharacterCasing.Upper;
            textBox.Size = new Size(40, 10);
            textBox1.Size = new Size(40, 10);
            textBox2.Size = new Size(40, 10);
            
            textBox.Parent = this.panel1;
            textBox1.Parent = this.panel1;
            textBox2.Parent = this.panel1;

            textBox.TabIndex = 3;
            textBox1.TabIndex = 2;
            textBox2.TabIndex = 1;

            textBox2.Dock = DockStyle.Left;
            textBox1.Dock = DockStyle.Left;
            textBox.Dock = DockStyle.Left;
           
            textBox1.MaxLength = 4;
            textBox2.MaxLength = 4;
            textBox.MaxLength = 4;
            textBox.Leave += new EventHandler(textBox_Leave);
            textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
            textBox1.Leave += new EventHandler(textBox1_Leave);
            textBox1.KeyDown += new KeyEventHandler(textBox1_KeyDown);
            textBox2.Leave += new EventHandler(textBox2_Leave);
            textBox2.KeyDown += new KeyEventHandler(textBox2_KeyDown);
            char[] charArr = val.ToCharArray();
            string str1 = "";
            string str2 = "";
            string str3 = "";
            for (int i = 0; i < charArr.Length; i++)
            {

                if (i < 4)
                    str1 += charArr[i];
                if (i >= 4 && i < 8)
                    str2 += charArr[i];
                if (i >= 8 && i < charArr.Length)
                    str3 += charArr[i];
            }

            textBox.Text = str3;
            textBox1.Text = str2;
            textBox2.Text = str1;
        }

        private void GenerateOptoMechIDTextBoxes(string val)
        {
            textBox1 = new TextBox();
            textBox = new TextBox();
            textBox.Parent = this.panel1;
            textBox1.Parent = this.panel1;
            textBox.CharacterCasing = CharacterCasing.Upper;
            textBox1.CharacterCasing = CharacterCasing.Upper;
            textBox.TabIndex = 2;
            textBox1.TabIndex = 1;
            textBox.Size = new Size(40, 10);
            textBox1.Size = new Size(25, 10);

            textBox1.Dock = DockStyle.Left;
            textBox.Dock = DockStyle.Left;
            textBox1.MaxLength = 2;
            textBox.MaxLength = 4;
            textBox.Leave += new EventHandler(textBox_Leave);
            textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
            textBox1.Leave += new EventHandler(textBox1_Leave);
            textBox1.KeyDown += new KeyEventHandler(textBox1_KeyDown);
            
            char[] charArr = val.ToCharArray();
            string str1 = "";
            string str3 = "";
            for (int i = 0; i < charArr.Length; i++)
            {
                if (i < 2)
                    str1 += charArr[i];
                if (i >= 2 && i < 6)
                    str3 += charArr[i];
            }

            textBox.Text = str3;
            textBox1.Text = str1;
        }

    }
}

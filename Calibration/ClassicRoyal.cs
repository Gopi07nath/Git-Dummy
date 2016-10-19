using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Calibration
{
    public partial class ClassicRoyal : UserControl
    {
        Facade f;
        public ClassicRoyal()
        {
            InitializeComponent();
            f = Facade.getInstance();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
                return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ClassicRoyal_Load(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void Classic_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (Classic_rb.Checked)
                Globals.isRoyal = false;
            else
                Globals.isRoyal = true;
        }
    }
}

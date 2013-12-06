using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IPLab
{
    public partial class PenColor : Form
    {
        public PenColor()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
        }
    }
}

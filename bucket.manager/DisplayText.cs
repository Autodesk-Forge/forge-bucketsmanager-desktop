using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bucket.manager
{
    public partial class DisplayText : Form
    {
        public DisplayText()
        {
            InitializeComponent();
        }

        public DisplayText(string theText)
        {
            InitializeComponent();
            this.tbText.Text = theText;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

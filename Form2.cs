using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TFT_Overlay
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            VersionLab.Text = "Version: " + Properties.Settings.Default.Version;
            TopMost = Properties.Settings.Default.TopMost;
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://ko-fi.com/xcibe95x");
            Close();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.patreon.com/xcibe95x");
            Close();
        }
    }
}

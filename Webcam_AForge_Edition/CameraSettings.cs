using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Webcam_AForge_Edition
{
    public partial class CameraSettings : Form
    {
        private GlobalVars gv;

        public CameraSettings(GlobalVars gv)
        {
            InitializeComponent();
            this.gv = gv;

        }

        private void CameraSettings_Load(object sender, EventArgs e)
        {
            TabPage tabPage;
            int counter = 1;
            VideoCapabilities[] vc = gv.FinalVideo.VideoCapabilities;
            foreach (VideoCapabilities item in vc)
            {

                if (counter > 2)
                {
                    // Creates a new tab page and adds it to the tab control
                    tabPage = new TabPage();
                    tabPage.Name = "tabPage" + counter.ToString();
                    tabPage.Text = counter.ToString();
                    this.tabControl1.TabPages.Add(tabPage);

                }
                else if (counter == 1)
                {
                    tabPage = tabPage1;
                    tabPage.Text = counter.ToString();
                }
                else
                {
                    tabPage = tabPage2;
                    tabPage.Text = counter.ToString();
                }

                Label nl = new Label();
                nl.AutoSize = true;
                nl.Location = new Point(10, 20);
                nl.Text = String.Format($"Resolution : Width = {item.FrameSize.Width.ToString()} Height = {item.FrameSize.Height.ToString()}");

                Label nl2 = new Label();
                nl2.AutoSize = true;
                nl2.Location = new Point(10, 35);
                nl2.Text = String.Format($"MaximumFrameRate : {item.MaximumFrameRate}");

                Label nl3 = new Label();
                nl3.AutoSize = true;
                nl3.Location = new Point(10, 50);
                nl3.Text = String.Format($"Bitcount : {item.BitCount}");

                Label nl4 = new Label();
                nl4.AutoSize = true;
                nl4.Location = new Point(10, 65);
                nl4.Text = String.Format($"Framerate : {item.AverageFrameRate.ToString()}");

                // Stop Layout temporally
                tabPage.SuspendLayout();
                tabPage.Controls.Add(nl); 
                tabPage.Controls.Add(nl2);
                tabPage.Controls.Add(nl3);
                tabPage.Controls.Add(nl4);

                tabPage.ResumeLayout();

                counter++;
            }
            //          gv.FinalVideo.VideoResolution = vc[1];
        }
    }
}

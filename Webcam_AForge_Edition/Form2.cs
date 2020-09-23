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
using AForge.Imaging;
using AForge.Math;

namespace Webcam_AForge_Edition
{
    public partial class Form2 : Form
    {
        public Form2(Bitmap video)
        {
            InitializeComponent();
            pb_Image.Image = video;
        }

        Pen myPen = new Pen(Color.FromArgb(0, 0, 0, 0));
        Graphics g = null;

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            g = panel_Histogram.CreateGraphics();
        }

        public void createHistogram(Bitmap video)
        {
            ImageStatistics stat = new ImageStatistics(video);
            Histogram red = stat.Red;

            g.DrawLine(myPen, 1, 1, red.Max - red.Min, red.Max - red.Min);
        }
    }
}

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
        Bitmap imgLocal;
        string colour = string.Empty;

        // createHistogram():
        int offset = 20;
        float Ycord = 0;
        float Xcord = 0;

        Graphics g = null;

        public Form2(System.Drawing.Image imgVideo, string colourScale)
        {
            InitializeComponent();
            pb_Image.SizeMode = PictureBoxSizeMode.Normal;
            pb_Image.Image = imgVideo;
            imgLocal = (Bitmap)imgVideo;
            colour = colourScale;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            g = panel_Histogram.CreateGraphics();
            createHistogram();
        }

        public void createHistogram()
        {
            ImageStatistics stat = new ImageStatistics(imgLocal);
            Histogram histogram = new Histogram(new int[256]);

            switch (colour)
            {
                case "red":
                    histogram = stat.Red;
                    break;
                case "green":
                    histogram = stat.Green;
                    break;
                case "blue":
                    histogram = stat.Blue;
                    break;
                case "gray":
                    histogram = stat.Gray;
                    break;
                default:
                    break;
            }

            Ycord = (float)(panel_Histogram.Height - (2 * offset)) / histogram.Values.Max();
            Xcord = (float)(panel_Histogram.Width - (2 * offset)) / (histogram.Values.Length - 1);

            Pen myPen = new Pen(new SolidBrush(Color.Black), Xcord);

            for (int i = 0; i < histogram.Values.Length; i++)
            {
                //Draw lines
                g.DrawLine(myPen, 
                    new PointF(offset + (i * Xcord), panel_Histogram.Height - offset),
                    new PointF(offset + (i * Xcord), panel_Histogram.Height - offset - histogram.Values[i] * Ycord)
                );
            }
        }
    }
}

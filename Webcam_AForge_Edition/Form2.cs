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

        public Form2(System.Drawing.Image imgCapture, string colourScale)
        {
            InitializeComponent();
            pb_Image.Image = imgCapture;
            imgLocal = (Bitmap)imgCapture;
            colour = colourScale;
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
            Pen myPen = new Pen(new SolidBrush(Color.Black), Xcord);

            switch (colour)
            {
                case "red":
                    histogram = stat.Red;
                    myPen = new Pen(new SolidBrush(Color.Red), Xcord);
                    break;
                case "green":
                    histogram = stat.Green;
                    myPen = new Pen(new SolidBrush(Color.Green), Xcord);
                    break;
                case "blue":
                    histogram = stat.Blue;
                    myPen = new Pen(new SolidBrush(Color.Blue), Xcord);
                    break;
                case "gray":
                    histogram = stat.Gray;
                    myPen = new Pen(new SolidBrush(Color.Black), Xcord);
                    break;
                default:
                    break;
            }

            Ycord = (float)(panel_Histogram.Height - (2 * offset)) / histogram.Values.Max();
            Xcord = (float)(panel_Histogram.Width - (2 * offset)) / (histogram.Values.Length - 1);

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

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
    public partial class Form1 : Form
    {
        // Variable declaration

        Stack<Bitmap> imageStack;
        internal GlobalVars gv; // 'Instantiate' Global variable

        public Form1()
        {
            InitializeComponent();
            buttonCamStart.Enabled = false;
            gv = new GlobalVars(); // 'Initialize' Global variable
        }

        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void Form1_Load(object sender, EventArgs e)
        {
            imageStack = new Stack<Bitmap>();

            gv.VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo VideoCaptureDevice in gv.VideoCaptureDevices)
            {
                comboBoxCameraList.Items.Add(VideoCaptureDevice.Name);
            }
            if (comboBoxCameraList.Items.Count > 0)
            {
                comboBoxCameraList.SelectedIndex = 0;
                buttonCamStart.Enabled = true;
            }
            buttonStop.Enabled = false;
        }

        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Sure you want to close?", "Are you sure?", MessageBoxButtons.YesNo);
            if (DialogResult.No == dr)
            {
                e.Cancel = true;
            }
            else
            {
                if (gv.FinalVideo != null)
                {
                    gv.FinalVideo.Stop();
                    gv.FinalVideo.WaitForStop();
                }
                gv.FinalVideo = null;
            }
        }

        /**************************************************************************************/
        //
        /**************************************************************************************/
        void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap video = (Bitmap)eventArgs.Frame.Clone(); // (Bitmap) = 'typecast'
            imgVideo.Image = video;

        }

        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void buttonCapture_Click(object sender, EventArgs e)
        {
            try
            {
                imgCapture.Image = (Image)imgVideo.Image.Clone();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("You need to select a resolution first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void buttonCamStart_Click(object sender, EventArgs e)
        {
            gv.FinalVideo = new VideoCaptureDevice(gv.VideoCaptureDevices[comboBoxCameraList.SelectedIndex].MonikerString);

            CameraSettings cs = new CameraSettings(gv);
            DialogResult dr = cs.ShowDialog();

            if (DialogResult.OK == dr)
            {
                // Get videoresolution possibilities
                VideoCapabilities[] vc = gv.FinalVideo.VideoCapabilities;
                // Get selected resolution
                int resolutionSelection = int.Parse(cs.tabControl1.SelectedTab.Text) - 1;  // Minus 1 due to 0 offset
                // Set camera resolution
                gv.FinalVideo.VideoResolution = vc[resolutionSelection];
                // Enable eventhandler
                gv.FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
                gv.FinalVideo.Start();

                buttonCamStart.Enabled = false;
                buttonStop.Enabled = true;
            }
        }


        /**************************************************************************************/
        //
        /**************************************************************************************/
        /// <summary>
        /// Convert captured picture to Grayscale.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGrayscale_Click(object sender, EventArgs e)
        {
            try
            {
                imageStack.Push(new Bitmap(imgCapture.Image));
                //  undoToolStripMenuItem.Enabled = true;
                Bitmap bt = new Bitmap(imgCapture.Image);
                for (int y = 0; y < bt.Height; y++)
                {
                    for (int x = 0; x < bt.Width; x++)
                    {
                        Color c = bt.GetPixel(x, y);

                        int avg = (c.R + c.G + c.B) / 3;
                        bt.SetPixel(x, y, Color.FromArgb(avg, avg, avg));
                    }
                }
                imgCapture.Image = bt;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("You need to capture a picture first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Only if there is still something left on the stack
            if (imageStack.Count > 0)
                imgCapture.Image = imageStack.Pop();
        }

        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void resolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                gv.FinalVideo.SignalToStop();

                gv.FinalVideo.Stop();
                gv.FinalVideo.WaitForStop();
                gv.FinalVideo.NewFrame -= new NewFrameEventHandler(FinalVideo_NewFrame);

                CameraSettings cs = new CameraSettings(gv);
                DialogResult dr = cs.ShowDialog();

                if (DialogResult.OK == dr)
                {
                    VideoCapabilities[] vc = gv.FinalVideo.VideoCapabilities;
                    int resolutionSelection = int.Parse(cs.tabControl1.SelectedTab.Text) - 1;  // Minus 1 due to 0 offset

                    gv.FinalVideo.VideoResolution = vc[resolutionSelection];

                    gv.FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
                    gv.FinalVideo.Start();

                    buttonCamStart.Enabled = false;
                    buttonStop.Enabled = true;
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("You need to select a camera first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (gv.FinalVideo != null)
            {
                gv.FinalVideo.Stop();
                gv.FinalVideo.WaitForStop();
            }
            gv.FinalVideo = null;
            buttonCamStart.Enabled = true;
            buttonStop.Enabled = false;
        }
    }
}

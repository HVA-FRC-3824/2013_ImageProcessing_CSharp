using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using edu.wpi.first.wpilibj.networktables;
using edu.wpi.first.wpilibj.networktables2;
using edu.wpi.first.wpilibj.tables;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.UI;
using System.Threading;



namespace ImageProcessing
{
    public partial class Form1 : Form
    {
        private Thread processingThread;
        private NetworkTable table;

        public delegate int readTrackBarDelegate(TrackBar bar);

        public int readTrackBar(TrackBar bar)
        {
            return bar.Value;
        }

        public Form1()
        {
            InitializeComponent();
            processingThread = new Thread(processImages);
            NetworkTable.setClientMode();
            NetworkTable.setIPAddress("10.78.5.176");

            table = NetworkTable.getTable("SmartDashboard");

        }

        private void processImages()
        {
 
            //Capture capture = new Capture("rtsp://192.168.0.90:554/axis-media/media.amp");
           // Capture capture = new Capture("rtsp://10.0.9.5:554/axis-media/media.amp");

//  This is for testing via webcam image.
Capture capture;  
    capture = new Capture();
    Image<Bgr, Byte> frame = capture.QueryFrame();

            Image<Bgr, byte> image;
            Image<Hsv, byte> hsvImage;
            Image<Gray, byte> mask;

            int hMin, hMax, sMin, sMax, vMin, vMax;
            hMin = hMax = sMin = sMax = vMin = vMax = 0;

            int i = 0;

            while (true)
            {
                i++;
                //capture.Grab();
                //image = capture.RetrieveBgrFrame();
                //image = new Image<Bgr, Byte>("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\ImageProcessing2013CSharp\\retroreflective.png");
                image = new Image<Bgr, Byte>("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\ImageProcessing2013CSharp\\Robot 3 - 26ft.png");

                // The SmoothGaussian makes the image much MUCH smoother.
                image.SmoothGaussian(299, 299, 250.0, 250.0);
                // Convert the image to hsv
                hsvImage = image.Convert<Hsv, byte>();

                // Threshold the HSV Image for targets
                readTrackBarDelegate readBar = new readTrackBarDelegate(readTrackBar);

                hMin = (int)this.Invoke(readBar, bar_hMin);

                hMax = (int)this.Invoke(readBar, bar_hMax);

                sMin = (int)this.Invoke(readBar, bar_sMin);

                sMax = (int)this.Invoke(readBar, bar_sMax);

                vMin = (int)this.Invoke(readBar, bar_vMin);

                vMax = (int)this.Invoke(readBar, bar_vMax);

              //mask = hsvImage.InRange(new Hsv(130.0 / 2, 25.0, 30.0), new Hsv(150.0 / 2, 255.0, 255.0));
                //mask = hsvImage.InRange(new Hsv(hMin, sMin, vMin), new Hsv(hMax, sMax, vMax));
                mask = hsvImage.InRange(new Hsv((int)(103 / 240.0f * 180),
                            (int)(100 / 240.0f * 255),
                            (int)(150 / 240.0f * 255)),
                     new Hsv((int)(118 / 240.0f * 180),
                            (int)(240 / 240.0f * 255),
                            (int)(240 / 240.0f * 255)));
                //Hsv hsv = new Hsv(

                // Filter image
                //mask._Erode(4);
                mask._Dilate(4);
                mask._Erode(4);

                
                
                //      mask._SmoothGaussian(image, 2, 2, 1.5, 1.5);

                // Find the Rectangles
                //contours = mask.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE);

                // List of boxes found
                List<Rectangle> boxList = new List<Rectangle>();

                for (Contour<Point> contours = mask.FindContours(); contours != null; contours = contours.HNext)
                {
                    contours.ApproxPoly(contours.Perimeter * 0.05);
                     boxList.Add(contours.BoundingRectangle);
                }

                const double imageWidth = 320;
                double minCenter = imageWidth; //min distance to center of image
                int centerRectNum = 0;  //number of closest rect
                
                for (int n=0;n<boxList.Count;n++)
                {
                    Rectangle rec = boxList[n];
                    image.Draw(rec, new Bgr(255, 21, 255), 2);
                    double centerx = rec.X + rec.Width / 2.0;
                    if (Math.Abs(centerx - imageWidth / 2) < minCenter)
                    {
                        minCenter = Math.Abs(centerx - imageWidth / 2);
                        centerRectNum = n;
                    }
                }

                //Console.Write(minCenter);
                //MessageBox.Show("Pos:" + (boxList[centerRectNum].X + boxList[centerRectNum].Width/2.0)+"distance: "+minCenter);
                //MessageBox.Show("Off by:" + (boxList[centerRectNum].X + boxList[centerRectNum].Width / 2.0 - imageWidth/2));
                SetText("Off by:" + (boxList[centerRectNum].X + boxList[centerRectNum].Width / 2.0 - imageWidth / 2));

                // Print out the information about the first rect
                if (boxList.Count != 0)
                {
                    NetworkTable.getTable("SmartDashboard").putNumber("x", boxList[0].X);
                    NetworkTable.getTable("SmartDashboard").putNumber("y", boxList[0].Y);
                    NetworkTable.getTable("SmartDashboard").putNumber("h", boxList[0].Height);
                    NetworkTable.getTable("SmartDashboard").putNumber("w", boxList[0].Width);
                    NetworkTable.getTable("SmartDashboard").putNumber("servo", boxList[0].X / 320.0);
                }

                //table.putNumber("index", i);

                im_image.Image = image.ToBitmap();
                im_mask.Image = mask.ToBitmap();

            }
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            processingThread.Start();
        }

        #region TrackBar Value Changed
        private void bar_hMin_ValueChanged(object sender, EventArgs e)
        {
            lb_hMin.Text = ((TrackBar)sender).Value.ToString();
        }

        private void bar_sMin_ValueChanged(object sender, EventArgs e)
        {
            lb_sMin.Text = ((TrackBar)sender).Value.ToString();
        }

        private void bar_vMin_ValueChanged(object sender, EventArgs e)
        {
            lb_vMin.Text = ((TrackBar)sender).Value.ToString();
        }

        private void bar_hMax_ValueChanged(object sender, EventArgs e)
        {
            lb_hMax.Text = ((TrackBar)sender).Value.ToString();
        }

        private void bar_sMax_ValueChanged(object sender, EventArgs e)
        {
            lb_sMax.Text = ((TrackBar)sender).Value.ToString();
        }

        private void bar_vMax_ValueChanged(object sender, EventArgs e)
        {
            lb_vMax.Text = ((TrackBar)sender).Value.ToString();
        }
        #endregion

        delegate void SetTextCallback(string text);
        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.distance.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.distance.Text = text;
            }
        }

        private void bar_servo_ValueChanged(object sender, EventArgs e)
        {
            //table.putNumber("servo", bar_servo.Value / 100.0);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            processingThread.Interrupt();
            processingThread.Abort();
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            processingThread.Abort();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

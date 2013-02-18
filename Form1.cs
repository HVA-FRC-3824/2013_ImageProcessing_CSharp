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
        //private Thread processingThread;
        private NetworkTable table;
        private             Capture capture;
        public delegate int readTrackBarDelegate(TrackBar bar);

        public int readTrackBar(TrackBar bar)
        {
            return bar.Value;
        }

        public Form1()
        {
            InitializeComponent();
            //processingThread = new Thread(processImages);
            NetworkTable.setClientMode();
            NetworkTable.setIPAddress("10.0.1.2");

            table = NetworkTable.getTable("SmartDashboard");

            // capture = new Capture("rtsp://192.168.0.90:554/axis-media/media.amp");
            capture = new Capture("rtsp://10.0.1.11:554/axis-media/media.amp");

            //  This is for testing via webcam image.
            //capture = new Capture();
            Application.Idle += processImage;
        }

        private void processImage(object sender, EventArgs arg)
        {
 
            Image<Bgr, Byte> image = capture.QueryFrame();

            
            Image<Hsv, byte> hsvImage;
            Image<Gray, byte> mask;

                
            //image = new Image<Bgr, Byte>("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\ImageProcessing2013CSharp\\retroreflective.png");
            //image = new Image<Bgr, Byte>("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\ImageProcessing2013CSharp\\Robot 3 - 26ft.png");

            // The SmoothGaussian makes the image much MUCH smoother.
            //image.SmoothGaussian(299, 299, 250.0, 250.0);
            // Convert the image to hsv
            hsvImage = image.Convert<Hsv, byte>();

            //mask = hsvImage.InRange(new Hsv(130.0 / 2, 25.0, 30.0), new Hsv(150.0 / 2, 255.0, 255.0));
                            // Threshold the HSV Image for targets
                //readTrackBarDelegate readBar = new readTrackBarDelegate(readTrackBar);

                //int hMin = (int)this.Invoke(readBar, bar_hMin);

                //int hMax = (int)this.Invoke(readBar, bar_hMax);

                //int sMin = (int)this.Invoke(readBar, bar_sMin);

                //int sMax = (int)this.Invoke(readBar, bar_sMax);

                //int vMin = (int)this.Invoke(readBar, bar_vMin);

                //int vMax = (int)this.Invoke(readBar, bar_vMax);

                int hMin = 0, hMax = 180, sMin = 0, sMax = 22, vMin = 172, vMax = 255;
            mask = hsvImage.InRange(new Hsv(hMin, sMin, vMin), new Hsv(hMax, sMax, vMax));
            //mask = hsvImage.InRange(new Hsv((int)(103 / 240.0f * 180),
            //            (int)(100 / 240.0f * 255),
            //            (int)(150 / 240.0f * 255)),
            //        new Hsv((int)(118 / 240.0f * 180),
            //            (int)(240 / 240.0f * 255),
            //            (int)(240 / 240.0f * 255)));
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
            List<Ellipse> ellList = new List<Ellipse>();

            for (Contour<Point> contours = mask.FindContours(); contours != null; contours = contours.HNext)
            {
                    contours.ApproxPoly(contours.Perimeter * 0.05);
                    Rectangle rect = contours.BoundingRectangle;
                    if (rect.Width > 80 && rect.Height > 10 && contours.Area>1000)
                    {
                        boxList.Add(rect);
                        image.Draw(contours, new Bgr(21, 255, 255), 2);
                    }
                    //ellipse not used other than for drawing - could be removed
                    if (contours.Total >= 5) //required for ellipse fitting
                    {
                        PointF[] pts = new PointF[contours.Total];
                        for (int i = 0; i < contours.Total; i++)
                            pts[i] = new PointF(contours[i].X, contours[i].Y);
                        Ellipse ell = PointCollection.EllipseLeastSquareFitting(pts);
                        if (ell.MCvBox2D.size.Height > 10 && ell.MCvBox2D.size.Width > 80)
                            ellList.Add(ell);
                    }
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
            for (int n = 0; n < ellList.Count; n++)
            {
                image.Draw(ellList[n], new Bgr(255, 21, 255), 2);
            }
            //Console.Write(minCenter);
            //MessageBox.Show("Pos:" + (boxList[centerRectNum].X + boxList[centerRectNum].Width/2.0)+"distance: "+minCenter);
            //MessageBox.Show("Off by:" + (boxList[centerRectNum].X + boxList[centerRectNum].Width / 2.0 - imageWidth/2));
                

            // Print out the information about the first rect
            if (boxList.Count != 0)
            {
                double offset = (boxList[centerRectNum].X + boxList[centerRectNum].Width / 2.0 - imageWidth / 2);
                //SetText("Off by:" + offset);
                NetworkTable.getTable("SmartDashboard").putNumber("frisbee offset", offset);
                NetworkTable.getTable("SmartDashboard").putNumber("frisbee size", boxList[centerRectNum].Width);
                //NetworkTable.getTable("SmartDashboard").putNumber("x", boxList[0].X);
                //NetworkTable.getTable("SmartDashboard").putNumber("y", boxList[0].Y);
                //NetworkTable.getTable("SmartDashboard").putNumber("h", boxList[0].Height);
                //NetworkTable.getTable("SmartDashboard").putNumber("w", boxList[0].Width);
                //NetworkTable.getTable("SmartDashboard").putNumber("servo", boxList[0].X / 320.0);
            }
            else
            {
                NetworkTable.getTable("SmartDashboard").putNumber("frisbee offset", 0);
                NetworkTable.getTable("SmartDashboard").putNumber("frisbee size", 320);  //assume it is too big to see
            }

            //table.putNumber("index", i);
            im_mask.Image = mask.ToBitmap();
                
            im_image.Image = image.ToBitmap();
            //SetImage(image.ToBitmap());
            

            
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
//            processingThread.Start();
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

        delegate void SetImageCallback(Image image);
        private void SetImage(Image image)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.im_image.InvokeRequired)
            {
                SetImageCallback d = new SetImageCallback(SetImage);
                this.Invoke(d, new object[] { image });
            }
            else
            {
                this.im_image.Image = image;
            }
        }

        private void bar_servo_ValueChanged(object sender, EventArgs e)
        {
            //table.putNumber("servo", bar_servo.Value / 100.0);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //processingThread.Interrupt();
            //processingThread.Abort();
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            //processingThread.Abort();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

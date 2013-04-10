#region Using Statements
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
#endregion

namespace ImageProcessing
{
    public partial class ImageProcessing : Form
    {
        #region *** Declaracations
        #region Constants
        private const int H_MIN = 0;
        private const int H_MAX = 180;
        private const int S_MIN = 0;
        private const int S_MAX = 22;
        private const int V_MIN = 172;
        private const int V_MAX = 255;
        #endregion

        #region delegates
        // delegate to support writing to the Windows text box through another thread
        public delegate void WriteMessageDelegate(string text, bool linefeed);

        public delegate int readTrackBarDelegate(TrackBar bar);
        #endregion

        #region Variables
        private WriteMessageDelegate m_writeMessageToTextBox_Delegate;
        private NetworkTable table;
        private Capture capture;
        #endregion
        #endregion

        #region Constructor
        public ImageProcessing()
        {
            // inistialzie the dialog windows components
            InitializeComponent();

            m_writeMessageToTextBox_Delegate = writeMessageToTextBox;

            // setup access to the network table
            NetworkTable.setClientMode();
            NetworkTable.setIPAddress("10.38.24.2");
            table = NetworkTable.getTable("SmartDashboard");

            // setup a link to the camera videl
            //capture = new Capture("rtsp://192.168.0.90:554/axis-media/media.amp");
            capture = new Capture("rtsp://10.38.24.11:554/axis-media/media.amp");

            // This is for testing via webcam image.
            //capture = new Capture();

#if USE_SLIDERS
            // show the controls
            lb_hMin.Visible = true;
            lb_hMax.Visible = true;
            lb_sMin.Visible = true;
            lb_sMax.Visible = true;
            lb_vMin.Visible = true;
            lb_vMax.Visible = true;

            bar_hMin.Visible = true;
            bar_hMax.Visible = true;
            bar_sMin.Visible = true;
            bar_sMax.Visible = true;
            bar_vMin.Visible = true;
            bar_vMax.Visible = true;

            lb_hMinValue.Visible = true;
            lb_hMaxValue.Visible = true;
            lb_sMinValue.Visible = true;
            lb_sMaxValue.Visible = true;
            lb_vMinValue.Visible = true;
            lb_vMaxValue.Visible = true;

            // set the scroll bars
            bar_hMin.Value = H_MIN;
            bar_hMax.Value = H_MAX;
            bar_sMin.Value = S_MIN;
            bar_sMax.Value = S_MAX;
            bar_vMin.Value = V_MIN;
            bar_vMax.Value = V_MAX;

            bar_Scroll((object)bar_hMin, (EventArgs)null);
            bar_Scroll((object)bar_hMax, (EventArgs)null);
            bar_Scroll((object)bar_sMin, (EventArgs)null);
            bar_Scroll((object)bar_sMax, (EventArgs)null);
            bar_Scroll((object)bar_vMin, (EventArgs)null);
            bar_Scroll((object)bar_vMax, (EventArgs)null);
#endif

            // run the image processing when the operating system is idle
            Application.Idle += processImageFrisbee;
        }
        #endregion

        #region *** Windows Events
        #region Bar Scroll
        private void bar_Scroll(object sender, EventArgs e)
        {
            TrackBar trackbar = (TrackBar)sender;

            // determine the track bar that changed and update the label with the value
            switch (trackbar.Name)
            {
                case "bar_hMin":
                    lb_hMinValue.Text = trackbar.Value.ToString();
                    break;

                case "bar_sMin":
                    lb_sMinValue.Text = trackbar.Value.ToString();
                    break;

                case "bar_vMin":
                    lb_vMinValue.Text = trackbar.Value.ToString();
                    break;

                case "bar_hMax":
                    lb_hMaxValue.Text = trackbar.Value.ToString();
                    break;

                case "bar_sMax":
                    lb_hMaxValue.Text = trackbar.Value.ToString();
                    break;

                case "bar_vMax":
                    lb_vMaxValue.Text = trackbar.Value.ToString();
                    break;
            }
        }
        #endregion

        #region Image Processing Form Closing
        private void ImageProcessing_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        #endregion
        #endregion

        #region Porcess Image Frisbee
        private void processImageFrisbee(object sender, EventArgs arg)
        {
            const double imageWidth = 320;
            int maxBoundingBoxWidth = 80;
            int maxBoundingBoxHeight = 10;
            int maxBoundingBoxArea = 1000;
            int hMin = H_MIN;
            int hMax = H_MAX;
            int sMin = S_MIN;
            int sMax = S_MAX;
            int vMin = V_MIN;
            int vMax = V_MAX;
            Image<Bgr, Byte> image = capture.QueryFrame();
            Image<Hsv, byte> hsvImage;
            Image<Gray, byte> mask;

            try
            {
                // Convert the image to hsv
                hsvImage = image.Convert<Hsv, byte>();

#if USE_SLIDERS
                // Threshold the HSV Image for targets
                readTrackBarDelegate readBar = new readTrackBarDelegate(readTrackBar);

                // read the track bar positions
                hMin = (int)this.Invoke(readBar, bar_hMin);
                hMax = (int)this.Invoke(readBar, bar_hMax);
                sMin = (int)this.Invoke(readBar, bar_sMin);
                sMax = (int)this.Invoke(readBar, bar_sMax);
                vMin = (int)this.Invoke(readBar, bar_vMin);
                vMax = (int)this.Invoke(readBar, bar_vMax);
#endif

                // mask the image for the Frisbees
                mask = hsvImage.InRange(new Hsv(hMin, sMin, vMin), new Hsv(hMax, sMax, vMax));

                // Filter image
                mask._Dilate(4);
                //mask._Erode(4);

                // List of boxes found
                List<Rectangle> boxList     = new List<Rectangle>();
                List<Ellipse>   ellipseList = new List<Ellipse>();


                for (Contour<Point> contours = mask.FindContours(); contours != null; contours = contours.HNext)
                {
                    contours.ApproxPoly(contours.Perimeter * 0.05);

                    Rectangle rect = contours.BoundingRectangle;

                    if (rect.Width  > maxBoundingBoxWidth  &&
                        rect.Height > maxBoundingBoxHeight &&
                        contours.Area > maxBoundingBoxArea)
                    {
                        boxList.Add(rect);
                        image.Draw(contours, new Bgr(21, 255, 255), 2);
                    }

                    // ellipse not used other than for drawing - could be removed
                    if (contours.Total >= 5) // required for ellipse fitting
                    {
                        PointF[] pts = new PointF[contours.Total];

                        for (int i = 0; i < contours.Total; i++)
                            pts[i] = new PointF(contours[i].X, contours[i].Y);

                        Ellipse ellipse = PointCollection.EllipseLeastSquareFitting(pts);

                        if ((ellipse.MCvBox2D.size.Height > 10) && 
                            (ellipse.MCvBox2D.size.Width  > 10))
                            ellipseList.Add(ellipse);
                    }
                }

                double minCenter = imageWidth;   // min distance to center of image
                int centerRectNum = 0;           // number of closest rect

                for (int n = 0; n < boxList.Count; n++)
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

                for (int n = 0; n < ellipseList.Count; n++)
                {
                    image.Draw(ellipseList[n], new Bgr(255, 21, 255), 2);
                }

                //Console.Write(minCenter);
                //MessageBox.Show("Pos:"    + (boxList[centerRectNum].X + boxList[centerRectNum].Width/2.0) + "distance: "+ minCenter);
                //MessageBox.Show("Off by:" + (boxList[centerRectNum].X + boxList[centerRectNum].Width / 2.0 - imageWidth / 2));

                // Print out the information about the first rect
                if (boxList.Count != 0)
                {
                    double offset = (boxList[centerRectNum].X + boxList[centerRectNum].Width / 2.0 - imageWidth / 2);
                    NetworkTable.getTable("SmartDashboard").putNumber("Frisbee Offset", offset);
                    NetworkTable.getTable("SmartDashboard").putNumber("Frisbee Size", boxList[centerRectNum].Width);
                }
                else
                {
                    NetworkTable.getTable("SmartDashboard").putNumber("Frisbee Offset", 0);
                    NetworkTable.getTable("SmartDashboard").putNumber("Frisbee Size", 320);  //assume it is too big to see
                }

                im_mask.Image = mask.ToBitmap();
                im_image.Image = image.ToBitmap();
            }
            catch
            {
            }
        }
        #endregion

        #region Read TrackBar
        public int readTrackBar(TrackBar bar)
        {
            return bar.Value;
        }
        #endregion


        #region Write Message To TextBox
        public void writeMessageToTextBox(string message, bool linefeed)
        {
            // InvokeRequired required compares the thread ID of the calling thread
            // to the thread ID of the creating thread. If these threads are
            // different, it returns true.
            if (tb_message.InvokeRequired)
            {
                WriteMessageDelegate d = new WriteMessageDelegate(writeMessageToTextBox);
                Invoke(d, new object[] { message, linefeed });
            }
            else
            {
                // determine if the text box should be cleared
                if (message == "CLEAR")
                    tb_message.Text = "";
                else
                    tb_message.Text += message;

                // check for adding a carriage return
                if (linefeed)
                    tb_message.Text += Environment.NewLine;

                // move the horizontal bar to the end of text
                tb_message.SelectionStart = tb_message.TextLength;
                tb_message.ScrollToCaret();

                // ensure the text box is updated
                this.Update();
            }
        }
        #endregion
    }
}

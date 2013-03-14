using System;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
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
       // FileStream shotInformation = File.Create("C:\\WindRiver\\workspace\\ImageProcessing2013CSharp\\Image Saves\\shotData.txt");
        private NetworkTable table;
        private             Capture pickup_Capture;
        private             Capture target_Capture;
        public delegate int readTrackBarDelegate(TrackBar bar);
        private double[,] capturedData = new double[250,2];
        string str_shot;
        double shotRPM = 0;
        double shotAngle = 0;
        int lastShotCounter;
        int shotCounter = -1;
        int matchNumber = 1;

        Image<Bgr, Byte> target_image;
        Image<Bgr, Byte> pickup_image;

        StringBuilder sb = new StringBuilder();

        public int readTrackBar(TrackBar bar)
        {
            return bar.Value;
        }

        public Form1()
        {
            InitializeComponent();
            //processingThread = new Thread(processImages);
            NetworkTable.setClientMode();
           NetworkTable.setIPAddress("10.38.24.2");

            table = NetworkTable.getTable("SmartDashboard");

            // The next three lines are for testing purposes only!
            //NetworkTable.getTable("SmartDashboard").putNumber("Last Shot Shooter RPM", 0);
            //NetworkTable.getTable("SmartDashboard").putNumber("Last Shot Shooter Angle", 0);
           // NetworkTable.getTable("SmartDashboard").putNumber("Shot Counter", 0);
            //NetworkTable.getTable("SmartDashboard").putNumber("Match Number", 1);

             //capture = new Capture("rtsp://192.168.0.90:554/axis-media/media.amp");

            // Setup Cameras to these IP addresses
            // Using two cameras gets rid of having to swap between both types of image processing
            pickup_Capture = new Capture("rtsp://10.38.24.11:554/axis-media/media.amp");
            target_Capture = new Capture("rtsp://10.38.24.11:554/axis-media/media.amp");

            //  This is for testing via webcam image.
            //pickup_Capture = new Capture();
            //target_Capture = new Capture();
            Application.Idle += processImage;

        }

        private void processImage(object sender, EventArgs arg)
        {

            processImageTarget();
            processImageFrisbee();
            captureImageData();
        }

        private void processImageFrisbee()
        {
 
            Image<Bgr, Byte> image = pickup_Capture.QueryFrame();

            int maxBoundingBoxWidth = 80;
            int maxBoundingBoxHeight = 10;
            int maxBoundingBoxArea = 1000;
            
            Image<Hsv, byte> hsvImage;
            Image<Gray, byte> mask;

            // Convert the image to hsv
            hsvImage = image.Convert<Hsv, byte>();
                int hMin = 0, hMax = 180, sMin = 0, sMax = 22, vMin = 172, vMax = 255;
            mask = hsvImage.InRange(new Hsv(hMin, sMin, vMin), new Hsv(hMax, sMax, vMax));

            // Filter image
            mask._Dilate(4);
            mask._Erode(4);

            // List of boxes found
            List<Rectangle> boxList = new List<Rectangle>();
            List<Ellipse> ellList = new List<Ellipse>();

            for (Contour<Point> contours = mask.FindContours(); contours != null; contours = contours.HNext)
            {
                    contours.ApproxPoly(contours.Perimeter * 0.05);
                    Rectangle rect = contours.BoundingRectangle;
                    if (rect.Width > maxBoundingBoxWidth 
                       && rect.Height > maxBoundingBoxHeight 
                         && contours.Area > maxBoundingBoxArea)
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
                NetworkTable.getTable("SmartDashboard").putNumber("frisbee offset", offset);
                NetworkTable.getTable("SmartDashboard").putNumber("frisbee size", boxList[centerRectNum].Width);

            }
            else
            {
                NetworkTable.getTable("SmartDashboard").putNumber("frisbee offset", 0);
                NetworkTable.getTable("SmartDashboard").putNumber("frisbee size", 320);  //assume it is too big to see
            }

            im_mask_frisbee.Image = mask.ToBitmap();
                
            im_image_frisbee.Image = image.ToBitmap();

            pickup_image = image;

            
        }

        private void processImageTarget()
        {

            Image<Bgr, Byte> image = target_Capture.QueryFrame();


            Image<Hsv, byte> hsvImage;
            Image<Gray, byte> mask;

            // Convert the image to hsv
            hsvImage = image.Convert<Hsv, byte>();

            //mask = hsvImage.InRange(new Hsv(130.0 / 2, 25.0, 30.0), new Hsv(150.0 / 2, 255.0, 255.0));
            //mask = hsvImage.InRange(new Hsv(hMin, sMin, vMin), new Hsv(hMax, sMax, vMax));
            mask = hsvImage.InRange(new Hsv((int)(103 / 240.0f * 180),
                        (int)(100 / 240.0f * 255),
                        (int)(150 / 240.0f * 255)),
                    new Hsv((int)(118 / 240.0f * 180),
                        (int)(240 / 240.0f * 255),
                        (int)(240 / 240.0f * 255)));
            // Filter image
            //mask._Erode(4);
            mask._Dilate(4);
            mask._Erode(4);

            // Find the Rectangles
            //contours = mask.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE);

            // List of boxes found
            List<Rectangle> boxList = new List<Rectangle>();

            for (Contour<Point> contours = mask.FindContours(); contours != null; contours = contours.HNext)
            {
                contours.ApproxPoly(contours.Perimeter * 0.05);
                Rectangle rect = contours.BoundingRectangle;
                if (rect.Width > 40 && rect.Height > 10 && contours.Area > 400)
                {
                    boxList.Add(rect);
                }
            }

            const double imageWidth = 320;
            double minCenter = imageWidth; //min distance to center of image
            int centerRectNum = 0;  //number of closest rect

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

            //Console.Write(minCenter);
            //MessageBox.Show("Pos:" + (boxList[centerRectNum].X + boxList[centerRectNum].Width/2.0)+"distance: "+minCenter);
            //MessageBox.Show("Off by:" + (boxList[centerRectNum].X + boxList[centerRectNum].Width / 2.0 - imageWidth/2));


            // Print out the information about the first rect
            if (boxList.Count != 0)
            {
                double offset = (boxList[centerRectNum].X + boxList[centerRectNum].Width / 2.0 - imageWidth / 2);
                NetworkTable.getTable("SmartDashboard").putNumber("camera offset", offset);
            }
            else
            {
                NetworkTable.getTable("SmartDashboard").putNumber("camera offset", 0);
            }

            im_mask_target.Image = mask.ToBitmap();

            im_image_target.Image = image.ToBitmap();

            target_image = image;

        }

        private void captureImageData()
        {
            shotRPM = NetworkTable.getTable("SmartDashboard").getNumber("Shooter Speed GIT");
            shotAngle = NetworkTable.getTable("SmartDashboard").getNumber("Shooter Angle GIT");

            // the next two lines are in the correct order. DO NOT CHANGE!
            lastShotCounter = shotCounter;
            shotCounter = (int)NetworkTable.getTable("SmartDashboard").getNumber("Shot Counter");

            // The next three lines are for testing purposes only!
            /*if (shotCounter < 5)
            {
                shotCounter++;
            }
            
            if (lastShotCounter < shotCounter)
            {
                shotRPM += 1000;
                shotAngle += 2;
            }*/
            if (lastShotCounter < shotCounter)
            {
                capturedData[shotCounter, 0] = shotRPM;
                capturedData[shotCounter, 1] = shotAngle;

                str_shot = "Shot:\t" + shotCounter + "\tRPM:\t" + capturedData[shotCounter, 0] + "\tAngle:\t" + capturedData[shotCounter, 1] + "\n";

                  // System.IO.File.WriteAllText(@"C:\\WindRiver\\workspace\\ImageProcessing2013CSharp\\Image Saves\\shotData" + shotCounter + ".txt", str_shot);
                if (Directory.Exists("C:\\WindRiver\\workspace\\ImageProcessing2013CSharp\\Match " + matchNumber) == false)
                {
                    Directory.CreateDirectory("C:\\WindRiver\\workspace\\ImageProcessing2013CSharp\\Match " + matchNumber);
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\\WindRiver\\workspace\\ImageProcessing2013CSharp\\Match " + matchNumber + "\\Shot Data.txt", true))
                {
                    file.WriteLine(str_shot);
                }
                if (Directory.Exists("C:\\WindRiver\\workspace\\ImageProcessing2013CSharp\\Match " + matchNumber + "\\Shot Image") == false)
                {
                    Directory.CreateDirectory("C:\\WindRiver\\workspace\\ImageProcessing2013CSharp\\Match " + matchNumber + "\\Shot Image");
                }
                if (Directory.Exists("C:\\WindRiver\\workspace\\ImageProcessing2013CSharp\\Match " + matchNumber + "\\Shot Image\\Target") == false)
                {
                    Directory.CreateDirectory("C:\\WindRiver\\workspace\\ImageProcessing2013CSharp\\Match " + matchNumber + "\\Shot Image\\Target");
                }
                if (Directory.Exists("C:\\WindRiver\\workspace\\ImageProcessing2013CSharp\\Match " + matchNumber + "\\Shot Image\\Pickup") == false)
                {
                    Directory.CreateDirectory("C:\\WindRiver\\workspace\\ImageProcessing2013CSharp\\Match " + matchNumber + "\\Shot Image\\Pickup");
                }
                saveJpeg("C:\\WindRiver\\workspace\\ImageProcessing2013CSharp\\Match " + matchNumber + "\\Shot Image\\Target\\Shot " + shotCounter + " .jpeg", target_image.ToBitmap(), 100);
                saveJpeg("C:\\WindRiver\\workspace\\ImageProcessing2013CSharp\\Match " + matchNumber + "\\Shot Image\\Pickup\\Shot " + shotCounter + " .jpeg", pickup_image.ToBitmap(), 100);
            }

            // Next Comment should work for matches instead of the if conditional.
            
            if (shotCounter >= 100)            
            //if (lastShotCounter > shotCounter)
            {
                matchNumber++;
                shotCounter = 0;
                shotRPM = 0;
                shotAngle = 0;
            }

            
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
        //private void SetText(string text)
        //{
        //    // InvokeRequired required compares the thread ID of the
        //    // calling thread to the thread ID of the creating thread.
        //    // If these threads are different, it returns true.
        //    if (this.distance.InvokeRequired)
        //    {
        //        SetTextCallback d = new SetTextCallback(SetText);
        //        this.Invoke(d, new object[] { text });
        //    }
        //    else
        //    {
        //        this.distance.Text = text;
        //    }
        //}

        delegate void SetImageCallback(Image image);
        private void SetImage(Image image)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.im_image_target.InvokeRequired)
            {
                SetImageCallback d = new SetImageCallback(SetImage);
                this.Invoke(d, new object[] { image });
            }
            else
            {
                this.im_image_target.Image = image;
            }
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void saveJpeg(string path, Bitmap img, long quality)
        {
            // Encoder parameter for image quality

            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            // Jpeg image codec
            ImageCodecInfo jpegCodec = this.getEncoderInfo("image/jpeg");

            if (jpegCodec == null)
                return;

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);
        }

        private ImageCodecInfo getEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }
    }

}


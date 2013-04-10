#region Using Statements
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using edu.wpi.first.wpilibj.networktables;
using Emgu.CV;
using Emgu.CV.Structure;
using ZedGraph;
#endregion

namespace ImageProcessing
{
   public partial class ImageProcessing : Form
   {
      #region *** Declaracations
      #region Constants
      private const float FRISBEE_THRESHOLD = 0.65f;

      private const int H_MIN =   0;
      private const int H_MAX =  60;  //180;

      private const int S_MIN =   0;
      private const int S_MAX = 255;  // 22;

      private const int V_MIN =  90;  //170;
      private const int V_MAX = 255;

      private const int HEIGHT = 240;
      private const int WIDTH  = 320;
      #endregion

      #region delegates
      // delegate to support writing to the Windows text box through another thread
      public delegate void WriteMessageDelegate(string text, bool linefeed);

      public delegate int readTrackBarDelegate(TrackBar bar);

      private delegate void graphFloatArray_Callback(ZedGraphControl zgc_graph, string title, long[] data,
                                              int numberOfDataPoints, bool clearData);
      #endregion

      #region Variables
      private NetworkTable table;
      private Capture capture;

      long[] m_sum_row = new long[HEIGHT];
      long [] m_sum_col = new long[WIDTH];
      #endregion
      #endregion

      #region Constructor
      public ImageProcessing()
      {
         // inistialzie the dialog windows components
         InitializeComponent();

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

         bar_Scroll((object) bar_hMin, (EventArgs) null);
         bar_Scroll((object) bar_hMax, (EventArgs) null);
         bar_Scroll((object) bar_sMin, (EventArgs) null);
         bar_Scroll((object) bar_sMax, (EventArgs) null);
         bar_Scroll((object) bar_vMin, (EventArgs) null);
         bar_Scroll((object) bar_vMax, (EventArgs) null);
#endif

         // run the image processing when the operating system is idle
         //Application.Idle += processImageFrisbeeOriginal; 
         Application.Idle += processImageFrisbee;
      }
      #endregion

      #region *** Windows Events
      #region Bar Scroll
      private void bar_Scroll(object sender, EventArgs e)
      {
         TrackBar trackbar = (TrackBar) sender;

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
               lb_sMaxValue.Text = trackbar.Value.ToString();
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

      #region Process Image Frisbee Original
      private void processImageFrisbeeOriginal(object sender, EventArgs arg)
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
            hMin = (int) this.Invoke(readBar, bar_hMin);
            hMax = (int) this.Invoke(readBar, bar_hMax);
            sMin = (int) this.Invoke(readBar, bar_sMin);
            sMax = (int) this.Invoke(readBar, bar_sMax);
            vMin = (int) this.Invoke(readBar, bar_vMin);
            vMax = (int) this.Invoke(readBar, bar_vMax);
#endif

            // mask the image for the Frisbees
            mask = hsvImage.InRange(new Hsv(hMin, sMin, vMin), new Hsv(hMax, sMax, vMax));

            // Filter image
            mask._Dilate(4);
            mask._Erode(4);

            //writeMessageToTextBox("Width: " + hsvImage.Size.Width.ToString(), true);
            //writeMessageToTextBox("Height: " + hsvImage.Size.Height.ToString(), true);

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

      #region Process Image Frisbee
      private void processImageFrisbee(object sender, EventArgs arg)
      {

         int FrisbeeLeft_X = 0;
         int FrisbeeLeft_Y = 0;
         int FrisbeeRight_X = 0;
         int FrisbeeRight_Y = 0;
         int hMin = H_MIN;
         int hMax = H_MAX;
         int sMin = S_MIN;
         int sMax = S_MAX;
         int vMin = V_MIN;
         int vMax = V_MAX;
         Image<Bgr, Byte> image = capture.QueryFrame();
         Image<Hsv, byte> hsvImage;
         Image<Gray, byte> mask;
         Image<Gray, byte> leftFrisbee;
         Image<Gray, byte> rightFrisbee;

         try
         {
            // Convert the image to hsv
            hsvImage = image.Convert<Hsv, byte>();

#if USE_SLIDERS
            // Threshold the HSV Image for targets
            readTrackBarDelegate readBar = new readTrackBarDelegate(readTrackBar);

            // read the track bar positions
            hMin = (int) this.Invoke(readBar, bar_hMin);
            hMax = (int) this.Invoke(readBar, bar_hMax);
            sMin = (int) this.Invoke(readBar, bar_sMin);
            sMax = (int) this.Invoke(readBar, bar_sMax);
            vMin = (int) this.Invoke(readBar, bar_vMin);
            vMax = (int) this.Invoke(readBar, bar_vMax);
#endif

            // mask the image for the Frisbees
            mask = hsvImage.InRange(new Hsv(hMin, sMin, vMin), new Hsv(hMax, sMax, vMax));

            // Filter image
            mask._Dilate(4);
            mask._Erode(4);
            
            Matrix<float> imgMatHorizontal = new Matrix<float>(HEIGHT, 1, 1);

            int FrisbeeMidPoint = findMidPointBetweenFrisbees(mask);

            // find the left Frisbee X position
            FrisbeeLeft_X = findMeanIndex(0, FrisbeeMidPoint, m_sum_col);

            // find the left Frisbee X position
            FrisbeeRight_X = findMeanIndex(FrisbeeMidPoint, WIDTH, m_sum_col);

            // get an image of the left Frisbee
            Rectangle leftfrisbeeRect = new Rectangle(0, 0, FrisbeeMidPoint, HEIGHT);
            leftFrisbee = mask.Copy(leftfrisbeeRect);

            // sum in the horizontal direction for Y postion
            leftFrisbee.Reduce<float>(imgMatHorizontal, Emgu.CV.CvEnum.REDUCE_DIMENSION.SINGLE_COL, Emgu.CV.CvEnum.REDUCE_TYPE.CV_REDUCE_SUM);

            // read the horizontal sum
            for (int horIndex = 0; horIndex < HEIGHT; horIndex++)
               m_sum_row[horIndex] = (long) imgMatHorizontal[horIndex, 0];

            // find the Y position
            FrisbeeLeft_Y = findMeanIndex(0, HEIGHT, m_sum_row);

            // get an image of the right Frisbee
            Rectangle rightfrisbeeRect = new Rectangle(FrisbeeMidPoint, 0, WIDTH - FrisbeeMidPoint, HEIGHT);
            rightFrisbee = mask.Copy(rightfrisbeeRect);

            // sum in the horizontal direction for Y postion
            rightFrisbee.Reduce<float>(imgMatHorizontal, Emgu.CV.CvEnum.REDUCE_DIMENSION.SINGLE_COL, Emgu.CV.CvEnum.REDUCE_TYPE.CV_REDUCE_SUM);

            // read the horizontal sum
            for (int horIndex = 0; horIndex < HEIGHT; horIndex++)
               m_sum_row[horIndex] = (long) imgMatHorizontal[horIndex, 0];

            // find the Y position
            FrisbeeRight_Y = findMeanIndex(0, HEIGHT, m_sum_row);

            graphFloatArray(zgc_col, "Sum Row", m_sum_row, m_sum_row.Length, true);
            graphFloatArray(zgc_row, "Sum Column", m_sum_col, m_sum_col.Length, true);

            // show the Frisbee in the image (X, Y)
            Bgr bgr = new Bgr(Color.Red);

            PointF pointA = new PointF(FrisbeeLeft_X, FrisbeeLeft_Y);
            Cross2DF cross2DFA = new Cross2DF(pointA, 10, 10);
            image.Draw(cross2DFA, bgr, 2);

            PointF pointB = new PointF(FrisbeeRight_X, FrisbeeRight_Y);
            Cross2DF cross2DFB = new Cross2DF(pointB, 10, 10);
            image.Draw(cross2DFB, bgr, 2);

            // Print out the information about the first rect
            //if (boxList.Count != 0)
            //{
            //   double offset = (boxList[centerRectNum].X + boxList[centerRectNum].Width / 2.0 - imageWidth / 2);
            //   NetworkTable.getTable("SmartDashboard").putNumber("Frisbee Offset", offset);
            //   NetworkTable.getTable("SmartDashboard").putNumber("Frisbee Size", boxList[centerRectNum].Width);
            //}
            //else
            //{
            //   NetworkTable.getTable("SmartDashboard").putNumber("Frisbee Offset", 0);
            //   NetworkTable.getTable("SmartDashboard").putNumber("Frisbee Size", 320);  //assume it is too big to see
            //}

            im_mask.Image = mask.ToBitmap();
            im_image.Image = image.ToBitmap();

            //Thread.Sleep(500);
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

      #region Graph Float Array
      /// <summary>
      /// Routine Summary:
      ///
      ///    private void graphFloatArray(string title, float[] data,
      ///                                 int numberOfDataPoints, bool clearData);
      ///
      ///       title              - Title of the graph.
      ///       data               - Data to be plotted.
      ///       numberOfDataPoints - Number of points to be plotted.
      ///       clearData          - Flag to indicate if the graph should be cleared.
      ///
      /// Description:
      ///
      ///    Routine to graph an array of data or clear the graph.
      ///
      /// Return Value:
      ///
      ///    None.
      ///
      /// Modification History:
      ///
      ///     8/24/08 JWY - First version of routine.
      /// </summary>
      private void graphFloatArray(ZedGraphControl zgc_graph, string title, long[] data, int numberOfDataPoints, bool clearData)
      {
         // InvokeRequired required compares the thread ID of the calling thread
         // to the thread ID of the creating thread. If these threads are
         // different, it returns true.
         if (zgc_graph.InvokeRequired)
         {
            graphFloatArray_Callback d = new graphFloatArray_Callback(graphFloatArray);
            Invoke(d, new object[] { zgc_graph, title, data, numberOfDataPoints, clearData });
         }
         else
         {
            GraphPane myPane = zgc_graph.GraphPane;

            // determine if the graph should be cleared
            if (clearData)
            {
               // clear the graph data and reset the color index
               myPane.CurveList.Clear();
            }

            // get the graph color
            Color color = Color.Red;

            // create four sets of point pair lists for the graph
            PointPairList graphData = new PointPairList();

            // load the tube energy plots
            for (int index = 0; index < numberOfDataPoints; index++)
               graphData.Add(index, data[index]);

            // add the curve to the plot
            myPane.AddCurve("", graphData, color, SymbolType.None);

            // calculate the axis scale ranges
            myPane.Title.Text = title;
            myPane.XAxis.Scale.Max = numberOfDataPoints;
            zgc_graph.AxisChange();
            zgc_graph.Invalidate();
         }
      }
      #endregion

      #region findPointBetweenFrisbees
      private int findPointBetweenFrisbees(long [] data)
      {
         bool aboveLocalMinimum = false;
         bool searchingForMinmum = false;
         long maximum = 0;
         long minimum = 999999999;
         int minimumIndex = 0;
         long [] filteredData = new long[data.Length];

         // filter the data (5 point box car) and find the maximum value
         for (int index = 2; index < data.Length - 2; index++)
         {
            // 5 point box car filter
            filteredData[index] = data[index - 2] + 
                                  data[index - 1] + 
                                  data[index + 0] +
                                  data[index + 1] + 
                                  data[index + 2];

            // find the maximum value of the filtered data
            if (filteredData[index] > maximum)
               maximum = filteredData[index];
         }

         graphFloatArray(zgc_row, "Filtered", filteredData, filteredData.Length, true);

         // find the local minimum
         for (int index = 10; index < data.Length - 10; index++)
         {
            // find the first point that is "above" the local minimum
            if ((aboveLocalMinimum == false) &&
               (filteredData[index] > maximum * FRISBEE_THRESHOLD))
            {
               // start looking for the first "hump"
               aboveLocalMinimum = true;

               // increase the index to jump over noise
               index += 5;

               // ensure the range of index
               if (index >= data.Length)
                  index = data.Length - 1;
            }
            // search for coming down the first "hump"
            else if ((aboveLocalMinimum == true) &&
                     (searchingForMinmum == false) &&
                     (filteredData[index] < maximum * FRISBEE_THRESHOLD))
            {
               // start looking for the local minimum
               searchingForMinmum = true;

               // increase the index to jump over noise
               index += 5;

               // ensure the range of index
               if (index >= data.Length)
                  index = data.Length - 1;
            }
            // search for the local minimum
            else if ((searchingForMinmum == true) &&
                     (filteredData[index] < minimum))
            {
               // remember the local minimum
               minimum      = filteredData[index];
               minimumIndex = index;
            }
            // find the point where the curve starts back up
            else if ((searchingForMinmum == true) &&
                     (filteredData[index] > maximum * FRISBEE_THRESHOLD))
               return (minimumIndex);
         }

         return 0;
      }
      #endregion

      #region Find Mean Index
      private int findMeanIndex(int startIndex, int stopIndex, long[] data)
      {
         long sum = 0;
         long mean = 0;

         // determine the sum of the array
         for (int index = startIndex; index < stopIndex; index++)
            sum += data[index];

         // find the array mean
         for (int index = startIndex; index < stopIndex; index++)
         {
            // count events
            mean += data[index];

            // the mean index is where the counts are half the total
            if (mean >= sum / 2)
               return (index);
         }

         return 0;
      }
      #endregion

      private int findMidPointBetweenFrisbees(Image<Gray, byte> mask)
      {
         Matrix<float> imgMatVertical = new Matrix<float>(1, WIDTH, 1);

         // histogram in the vertical direction to find the point between the Frisbees
         mask.Reduce<float>(imgMatVertical, Emgu.CV.CvEnum.REDUCE_DIMENSION.SINGLE_ROW, Emgu.CV.CvEnum.REDUCE_TYPE.CV_REDUCE_SUM);

         // read the vertical sum
         for (int verIndex = 0; verIndex < WIDTH; verIndex++)
            m_sum_col[verIndex] = (long) imgMatVertical[0, verIndex];

         // find the point between Frisbees
         return(findPointBetweenFrisbees(m_sum_col));
      }
   }
}

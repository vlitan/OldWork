using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System.IO;
using System.IO.Ports;


namespace DataProcTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region headeru`
        const int VTRACK_MAX_FEATURE_COUNT = 480;
        const int AGE_HIST_COUNT = 16;
        const int AGE_HIST_STEP = 2;

        public struct tvXYLoc
        {
            float x;      // x location
            float y;      // y location
        };

        public struct t_vTrackOutputMetadata
        {
            public uint[] prevId;// = new uint[VTRACK_MAX_FEATURE_COUNT];
            public uint[] currId;// = new uint[VTRACK_MAX_FEATURE_COUNT];
            public tvXYLoc[] prevFeatures;// = new tvXYLoc[VTRACK_MAX_FEATURE_COUNT];
            public tvXYLoc[] currFeatures;// = new tvXYLoc[VTRACK_MAX_FEATURE_COUNT];
        };

        public struct t_vTrackOutputHeader
        {
            public ulong timestamp;
            public ulong frameId;
            public uint prevFeaturesCount;
            public uint currFeaturesCount;
        }

        public struct t_vTrackDebug
        {
            public uint ppTime;     // Pixel Pipeline's time
            public uint ofTime;     // Optical Flow's time
            public uint fmTime;     // Feature Maintenance's time
            public uint vTrackFrameCount;
            public uint ppKeypoints;
            public uint ofInputKeypoints;
            public uint fmInputKeypoints;
            public uint avgThreshold;
            public uint[] ageHist;// = new uint[AGE_HIST_COUNT];
            public uint miscTime;   // Scheduling time (initialization, memcpy etc)
            public uint totalTime;   // Sum of the above three + the scheduling time
            public uint maxPreStart; // The pixel pipe can be started earlier with this amount of clock cycles
            public uint[] info ;//= new uint[37];   // Extra debug & profiling data
        } ;

        public struct FeatureMetadata
        {
            public uint id;              // feature ID
            public uint age;             // feature age in frames
            public float harris_score;    // feature harris score
        } ;

        public struct tvTrackResultSF
        {
            public t_vTrackOutputHeader header;
            public t_vTrackOutputMetadata meta;
            public t_vTrackDebug debug;
        } ;

        #endregion


        private Capture capture;       
        private bool captureInProgress;

        int k = 0;
        int framesCount = 0;
        int servosCount = 0;
        const int deltaCount = 7;
        const int servoCount = 5;
        float xAvg = 0;
        float yAvg = 0;
        float xAvgPrev = 0;
        float yAvgPrev = 0;
        float[] deltaX = new float[deltaCount];
        float[] deltaY = new float[deltaCount];
        int[] servoX = new int[servoCount];
        int[] servoY = new int[servoCount];
        int servoXAvg;
        int servoYAvg;
        float deltaXAvg = 0;
        float deltaYAvg = 0;


        SerialPort port;
        
        const int xAngleCorrection=2;
        const int yAngleCorrection=-13;
        int xAngle = 90 + xAngleCorrection;
        int yAngle = 90 + yAngleCorrection;

        int xAnglePrev;
        int yAnglePrev;
        void Track(ref Bitmap bm)
        {

            k = 0;
            xAvg = 0;
            yAvg = 0;
            for (int i = 1; i < bm.Height - 23; i+=1)
            {
                for (int j = 1; j < bm.Width - 4; j+=2)
                {
                    
                    if(bm.GetPixel(j, i).R ==0)
                    {
                        if ((bm.GetPixel(j, i).G > 190) && (bm.GetPixel(j, i).B < 95) )
                        {
                        
                            k++;
                            xAvg += j;
                            yAvg += i;
                        }
                    }

                }
            }
            if ((k > 18) || ((Math.Abs(deltaX[deltaCount - 1]) > 7) && (Math.Abs(deltaY[deltaCount - 1]) > 7)))
            {

                xAvg /= k;
                yAvg /= k;
                int Amplif=k/10;
                #region comentat
                /*
                if (Math.Abs(xAvg - xAvgPrev) > 10)
                if (xAvg - xAvgPrev > 0)
                    richTextBox2.Text += "dreapta " + (xAvg - xAvgPrev) + "\n";
                else
                    richTextBox2.Text += "stanga " + (xAvg - xAvgPrev) + "\n";
            else
                richTextBox2.Text += "0oriz" + "\n";
            if (Math.Abs(yAvg - yAvgPrev) > 10)
                if (yAvg - yAvgPrev > 0)
                    richTextBox2.Text += "jos " + (yAvg - yAvgPrev) + "\n";
                else
                    richTextBox2.Text += "sus " + (yAvg - yAvgPrev) + "\n";
            else
                richTextBox2.Text += "0vert" + "\n";
            richTextBox2.Text += "" + k + '\t' + xAvg + '\t' + yAvg + "\n";*/
                #endregion


                //shift delta array
                ShiftDelta();


                if (framesCount < deltaCount)//daca sunt destule frameuri
                {
                    framesCount++;
                }
                else
                {
                    #region incercare de eliminare a park
                    /*
                    int xOrientation = 0;
                    int yOrientation = 0;
                    //deltaX avg
                    
                    //verificam semnul majoritar
                    for (byte i = 0; i < deltaCount; i++)
                    {
                        if (deltaX[i] > 0)
                            xOrientation++;
                        else
                            xOrientation--;
                    }
                    for (byte i = 0; i < deltaCount; i++)
                    {
                        if (deltaY[i] > 0)
                            xOrientation++;
                        else
                            xOrientation--;
                    }

                    deltaXAvg = 0;
                    int deltaXAvgCount = 0;
                    for (byte i = 0; i < deltaCount; i++)
                    {
                        if (xOrientation * deltaX[i] > 0)
                        {
                            deltaXAvg += deltaX[i];
                            deltaXAvgCount++;
                        }
                    }
                    //deltaXAvg /= deltaXAvgCount;

                    //deltaY avg
                    deltaYAvg = 0;
                    int deltaYAvgCount = 0;
                    for (byte i = 0; i < deltaCount; i++)
                    {
                        if (yOrientation * deltaY[i] > 0)
                        {
                            deltaYAvg += deltaX[i];
                            deltaYAvgCount++;
                        }
                    }
                   // deltaYAvg /= deltaYAvgCount;
                    */
                    #endregion

                    //deltaX avg
                    deltaXAvg = 0;
                    for (byte i = 0; i < deltaCount; i++)
                    {
                        deltaXAvg += deltaX[i];
                    }
                    deltaXAvg /= deltaCount;

                    //deltaY avg
                    deltaYAvg = 0;
                    for (byte i = 0; i < deltaCount; i++)
                    {
                        deltaYAvg += deltaY[i];
                    }
                    deltaYAvg /= deltaCount;

                    //debug
                    richTextBox2.Text += "variatie oriz " + deltaXAvg + "   " + k + '\n';
                    richTextBox2.Text += "variatie vert " + deltaYAvg + "   " + k + '\n';
                    #region comm
                    /*
                    ShiftServo();

                    //servoX avg
                    
                    for (byte i = 0; i < servoCount; i++)
                        xOrientation += Normalizare(deltaX[i]);
                    

                    //servoY avg
              
                    for (byte i = 0; i < servoCount; i++)
                        yOrientation += Normalizare(deltaY[i]);
                    */
                    #endregion
                    try
                    {
                        /*
                        if (deltaXAvg > 0)
                        {
                            xAngle += 3; 
                        }
                        else
                        {
                            xAngle -= 3;                    
                        }

                        if (deltaYAvg > 0)
                        {
                            yAngle += 3; 
                        }
                        else
                        {
                            yAngle -= 3; 
                        }*/
                         

                         yAngle += Convert.ToInt32(deltaYAvg * 2 / 3);
                         xAngle += Convert.ToInt32(deltaXAvg * 2 / 3);
                    }
                    catch
                    {
                        //MessageBox.Show("Eroare retardata!");
                    }


                    if ((xAngle > 0) && (xAngle < 180) && (yAngle < 180) && (yAngle > 0))
                    {
                        try
                        {
                            port.Write("D" + GetDigit(xAngle, 3) + GetDigit(xAngle, 2) + GetDigit(xAngle, 1) + GetDigit(yAngle, 3) + GetDigit(yAngle, 2) + GetDigit(yAngle, 1));
                        }
                        catch
                        {
                            MessageBox.Show("Nu exista port");
                        }
                    }

                }

                xAvgPrev = xAvg;
                yAvgPrev = yAvg;
            }
                /*
            else
            {
                for (int i = 0; i < deltaCount; i++)
                {
                    deltaX[i] = deltaY[i] = 0;
                }
                xAngle=90;
                yAngle=90;
                port.Write("D" + GetDigit(xAngle, 3) + GetDigit(xAngle, 2) + GetDigit(xAngle, 1) + GetDigit(yAngle, 3) + GetDigit(yAngle, 2) + GetDigit(yAngle, 1));

            }*/
        }

        private int Normalizare(float nr)
        {
            return (int)nr / (int)(Math.Abs(nr));
        }

        private void ShiftDelta()
        {
            for (int i = 0; i < deltaCount - 2; i++)
            {
                deltaX[i] = deltaX[i + 1];
                deltaY[i] = deltaY[i + 1];
            }
            deltaX[deltaCount - 1] = xAvg - xAvgPrev;
            deltaY[deltaCount - 1] = yAvg - yAvgPrev;
        }

        private void ShiftServo()
        {
            for (int i = 0; i < servoCount - 2; i++)
            {
                servoX[i] = servoX[i + 1];
                servoY[i] = servoY[i + 1];
            }
            servoX[servoCount - 1] = xAngle;
            servoY[servoCount - 1] = yAngle;
        }

        int GetDigit(int number, int index)
        {
            int pz = (int)Math.Pow(10, index);
            return (number % pz) / (pz / 10);
        }
        

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string portName = comboBox1.SelectedItem.ToString();
            try
            {
                /////HARDCODAAAT!!!!!!!!!!
                port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
                port.Open();
                if ((xAngle > 0) && (xAngle < 180) && (yAngle < 180) && (yAngle > 0))
                {
                    try
                    {
                        port.Write("D" + GetDigit(xAngle, 3) + GetDigit(xAngle, 2) + GetDigit(xAngle, 1) + GetDigit(yAngle, 3) + GetDigit(yAngle, 2) + GetDigit(yAngle, 1));
                    }
                    catch
                    {
                        MessageBox.Show("Nu exista port");
                    }
                }
            }
            catch (Exception E)
            {
                MessageBox.Show("Portul ales nu exista, sau nu se deschide");
            }
        }

        void SaveSnapshot()
        {
            Image<Bgr, Byte> ImageFrame = capture.QueryFrame();

            SaveFileDialog svd = new SaveFileDialog();
            svd.ShowDialog();
            ImageFrame.Save(svd.FileName+".jpg");
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            bool okArrow = false;

            try
            {
                switch (e.KeyData)
                {
                    case Keys.Up: yAngle++; okArrow = true; break;
                    case Keys.Down: yAngle--; okArrow = true; break;
                    case Keys.Left: xAngle--; okArrow = true; break;
                    case Keys.Right: xAngle++; okArrow = true; break;
                }
                if (okArrow)
                {
                    if ((xAngle > 0) && (xAngle < 180) && (yAngle < 180) && (yAngle > 0))
                    {
                        try
                        {
                            port.Write("D" + GetDigit(xAngle, 3) + GetDigit(xAngle, 2) + GetDigit(xAngle, 1) + GetDigit(yAngle, 3) + GetDigit(yAngle, 2) + GetDigit(yAngle, 1));
                        }
                        catch
                        {
                            MessageBox.Show("Nu exista port");
                        }
                    }
                }

            }
            catch (Exception E)
            {

            }
        }

       

        

        private void ProcessFrame(object sender, EventArgs arg)
        {
            Image<Bgr, Byte> ImageFrame = capture.QueryFrame();
            imageBox2.Image = ImageFrame;

            if (!checkBox1.Checked)
            {
                Bitmap bm = ImageFrame.Bitmap;
                Track(ref bm);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            #region if capture is not created, create it now
            if (capture == null)
            {
                try
                {
                    capture = new Capture();
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
            }
            #endregion

            if (capture != null)
            {
                if (captureInProgress)
                {               
                    Application.Idle -= ProcessFrame;
                }
                else
                {
                    Application.Idle += ProcessFrame;
                }
                captureInProgress = !captureInProgress;
            }
        }
        

        private void Form1_Load_1(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
            for (int i = 0; i < servoCount; i++)
            {
                servoX[i] = 90;
                servoY[i] = 90;
            }
            /*
            try
            {
                /////HARDCODAAAT!!!!!!!!!!
                port = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
                port.Open();
            }
            catch (Exception E)
            {
                MessageBox.Show("Portul ales nu exista, sau nu se deschide");
            }*/

            

            int index = -1;
            string comPortName = null;
            string[] portNames = null;
            portNames = SerialPort.GetPortNames();

            do
            {
                index++;
                comboBox1.Items.Add(portNames[index]);

            }
            while (!((portNames[index] == comPortName) || (index == portNames.GetUpperBound(0))));
             
           
        }

        private void ReleaseData()
        {
            if (capture != null)
                capture.Dispose();
        }
        //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\//////////////////////////////////////
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        /////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        private void button2_Click(object sender, EventArgs e)
        {
            /*
            byte[] frameBytes;
            Image<Bgr, Byte> ImageFrame = capture.QueryFrame();
            imageBox1.Image = ImageFrame;
            frameBytes = ImageFrame.Bytes;
            MessageBox.Show("" + frameBytes.Length);
            Bitmap bm = new Bitmap(ImageFrame.ToBitmap(640,500));
            frameBytes = ImageToByte(bm);
            MessageBox.Show("" + frameBytes.Length);
             
            //Bitmap meta = new Bitmap();
            */
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveSnapshot();
        }

        /*
        private void button1_Click(object sender, EventArgs e)
        {/*
            #region if capture is not created, create it now
            if (capture == null)
            {
                try
                {
                    capture = new Capture();
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
            }
            #endregion

            if (capture != null)
            {
                //capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_MODE, 19);
                if (captureInProgress)
                {  
                    button1.Text = "Start!"; 
                    Application.Idle -= ProcessFrame;
                }
                else
                {
                  
                    button1.Text = "Stop";
                    Application.Idle += ProcessFrame;
                }

                captureInProgress = !captureInProgress;
            }

        }*/

        

        
    }
}

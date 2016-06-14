using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.IO.Ports;

namespace Spec1
{
    public delegate void UpdatedEventHandler(object sender, EventArgs e);

    public partial class Form1 : Form
    {



        public Form1()
        {
            InitializeComponent();
            LoadLastConfiguration();
        }

        
        int lines = 40;         //number of lines retrieved from the analyzer
        int beatLines = 3;
        int analyserFreq = 40;   //in Hz
        int tunelRows = 6;
        int tunelColumns = 6;
        int curveRows = 5;
        int curveColumns = 3;
        float beatConstant = 1.26f;
        bool useInternalTimer = false;
        int maxAudioLevel=Int16.MaxValue;
 

        int Right;              
        int Left;

        Graphics g;            
        Graphics b;
        Graphics l;
        Graphics r;
        SolidBrush solidBlack = new SolidBrush(Color.Black);
        SolidBrush solidColors = new SolidBrush(Color.Blue);
        SolidBrush solidRed = new SolidBrush(Color.Red);
        
        Interpreter I;

        SerialPort port;
        bool dataReceived;

        bool Enabledd = false;


        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            
        }

        bool firstTime = true;

        private bool CollectConfiguration() //from interface &validate data
        {
            try
            {
                int aux = Convert.ToInt32(SpectrumBands.Text);
                if(aux>0)
                {
                    lines = aux;
                }
                else
                {
                    MessageBox.Show("The number of spectrum lines must be > 0");
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("Conversion error at SpectrumLines");
                return false;
            }

            try
            {
                int aux = Convert.ToInt32(BeatBands.Text);
                if (aux > 0)
                {
                    beatLines = aux;
                }
                else
                {
                    MessageBox.Show("The number of beat lines must be > 0");
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("Conversion error at BeatLines");
                return false;
            }
            try
            {
                int aux = Convert.ToInt32(AnFreq.Text);
                if (aux > 0)
                {
                    analyserFreq = aux;
                }
                else
                {
                    MessageBox.Show("The freq must be > 0");
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("Conversion error at AnalyserFreq");
                return false;
            }
            try
            {
                float aux = (float)Convert.ToDouble(BeatConst.Text);
                if (aux >= 0)
                {
                    beatConstant = aux;
                }
                else
                {
                    MessageBox.Show("The beatConstant must be >= 0");
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("Conversion error at Beat Constant");
                return false;
            }
            return true;
        }
        private void SaveConfiguration()
        {
            StreamWriter sw = new StreamWriter("ConfigurationBuffer.txt");
            sw.WriteLine("" + lines);
            sw.WriteLine("" + beatLines);
            sw.WriteLine("" + analyserFreq);
            sw.WriteLine("" + beatConstant);
            sw.Close();
        }
        private bool LoadLastConfiguration()
        {
            try
            {
                StreamReader sr = new StreamReader("ConfigurationBuffer.txt");
                
                SpectrumBands.Text = sr.ReadLine();
                BeatBands.Text = sr.ReadLine();
                AnFreq.Text = sr.ReadLine();
                BeatConst.Text = sr.ReadLine(); 

                sr.Close();

                return true;
            }
            catch
            {
                MessageBox.Show("Error while reading configuration");
                return false;
            }
        }
        private void DisplayCurrentConfiguration()
        {
            AnFreq.Text = "" + analyserFreq;
            SpectrumBands.Text = "" + lines;
            BeatBands.Text = "" + beatLines;
            BeatConst.Text = "" + beatConstant;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(firstTime)
            {
                
                if (!CollectConfiguration())
                    return;
                //else continue
                Initialize();
                button2.Text = "Restart";
                firstTime = false;
                DisplayCurrentConfiguration();
                button1.Enabled = true;
                SetState(true);
                button3.Enabled = false;
                string[] methods = I.GetMethods();
                
                foreach (string method in methods)
	            {
                    comboBox1.Items.Add(method);
	            }
                
            }
            else
            {
                Application.Restart();
            }

            
        }
        
        private void Initialize()
        {
            g = panel1.CreateGraphics();            //create the graphics on a panel
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;// smooth out the drawing procces
            b = panel2.CreateGraphics();
            b.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            l = panel3.CreateGraphics();
            l.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            r = panel4.CreateGraphics();
            r.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            timer1.Enabled = false;                 //disable the timer
            timer1.Interval = Convert.ToInt32(1000 / analyserFreq);

            progressBar1.Enabled = true;
            progressBar2.Enabled = true;
            progressBar1.Maximum = maxAudioLevel;
            progressBar2.Maximum = maxAudioLevel;

            

            

            dataReceived = false;

            // asta e esenta
            I = new Interpreter(lines,maxAudioLevel, beatLines, tunelRows, tunelColumns,curveRows,curveColumns, analyserFreq, useInternalTimer, beatConstant);
            foreach(string s in I.Devices)
            {
                comboBox3.Items.Add(s);
            }
            I.Update();
            
            
            trackBar1.Maximum = maxAudioLevel;
            trackBar1.Minimum = 10;
            trackBar1.Value = maxAudioLevel - 1000;

            InitPort();
        }
       


        #region  Display procedures

        public void DisplayTunelLights(bool[,] lights, int N, int M)
        {
            l.Clear(Color.White);
            if(I.Beats[1]>0)
            {
                solidColors.Color = Color.BlueViolet;
            }
            else
            {
                solidColors.Color = Color.Blue;
            }
            float width = panel3.Width / M; //get the width of a line
            float height = panel3.Height / N; //get the width of a line

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    if(lights[i,j])
                    {
                        l.FillRectangle(solidColors,j*width,i*height, width-1, height-1);
                    }
                }
                
            }

        }

        public void DisplayCurveLights(bool[,] lights, int N, int M)
        {
            r.Clear(Color.White);
            if (I.Beats[1] > 0)
            {
                solidColors.Color = Color.BlueViolet;
            }
            else
            {
                solidColors.Color = Color.Blue;
            }
            float width = panel4.Width / N; //get the width of a line
            float height = panel4.Height / M; //get the width of a line

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    if (lights[i, j])
                    {
                        r.FillRectangle(solidColors, i * width, (M - j-1) * height, width - 1, height - 1);
                    }
                }

            }
        }


        //this function displays the hole array of data
        public void DisplaySpectrum(float[] spectrumData)
        {

            g.Clear(Color.White);               //clear the panel
            float width = panel1.Width / (lines);//get the width of a line
            
            float proportionalValue;
            
            for (int x = 0; x<lines; x++)
            {
                proportionalValue =  spectrumData[x] * panel1.Height / 255;// see DisplayOne
                g.FillRectangle(solidBlack, x * width, panel1.Height-proportionalValue, width - 1, proportionalValue);
            }
            
            
        }

        void DisplayBeats(float[] Beats)
        {
            b.Clear(Color.White);                   //clear the panel
            float width = panel2.Width / beatLines; //get the width of a line
            
            for (int i = 0; i < beatLines; i++)
            {
                if (Beats[i] > 0)
                    b.FillRectangle(solidRed, i * width, 0, width - 1, panel2.Height);

            }
        }

        //Display the Right and Left Valuein progressBars
        private void DisplayRightLeft(int r, int l)
        {
            progressBar1.Value = (r > progressBar1.Maximum) ? progressBar1.Maximum : r;
            progressBar2.Value = (l > progressBar2.Maximum) ? progressBar2.Maximum : l;
        }

        #endregion

       

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            I.Update();                            //update the interpreter
            DisplaySpectrum(I.Spectrum);           //display spectrum
            DisplayBeats(I.Beats);                 //display beats
            DisplayRightLeft(I.Right, I.Left);     //display right&left
            DisplayTunelLights(I.Tunel.lights,I.Tunel.RowsCount, I.Tunel.ColumnsCount);  //dispay lighs
            DisplayCurveLights(I.Curve.lights, I.Curve.RowsCount, I.Curve.ColumnsCount);
            
            //GetBytes();
        }

        private byte[] GetBytes()
        {

            byte cntr=1;
            int answCntr=1;
            byte[] answer = new byte[255];
            bool[] lghts = I.GetLights();
            I.ComputeTrimLights();
            BytePack bp=new BytePack();
            bp.Value = 0;
            MessageBox.Show("" + I.TrimBounds[0]+" "+I.TrimBounds[1]);
            bp.Value = I.TrimBounds[0];
            bp[0] = true;
            answer[0] = bp.Value;
            for (int i = (I.TrimBounds[0]-1)*7; i <= Math.Min(lghts.Length,(I.TrimBounds[1]-1)*7) ; i++)
            //MessageBox.Show("" + lghts.Length);
          //  for (int i = 0; i < lghts.Length; i++)
            {
                
                if (cntr > 7)
                {
                    cntr = 1;
                    answer[answCntr] = bp.Value;
                    answCntr++;
                    bp.Value = 0;
                }
                
                bp[cntr] = lghts[i];
                cntr++;
                
            }
            answer[answCntr] = bp.Value;
            //MessageBox.Show("" + answCntr);

            Array.Resize(ref answer, answCntr+1);
           // MessageBox.Show("" + answer[0]);
            
            
            return answer;
        }

        //enable and disable button
        private void button1_Click(object sender, EventArgs e)
        {
            SetState(!Enabledd);
        }



        private void SetState(bool state)
        {
           // timer1.Enabled = state;
            I.Enabled = state;
            button1.Text = "disable";
            Enabled = state;
            if (state)
            {
                button1.Text = "disable";
            }
            else
            {
                button1.Text = "enable";
            }
        }

        // send data w/ sockets
        private void Send(string msg)
        {
            Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1994);

            sck.Connect(endPoint);
           
            byte[] msgBuffer = Encoding.Default.GetBytes(msg);
            sck.Send(msgBuffer, 0, msgBuffer.Length, 0);

            sck.Close();
        }

        private void InitPort()
        {
            int index = -1; //trebe sa fie -1..trustme
            string comPortName = null;
            string[] portNames = null;
            portNames = SerialPort.GetPortNames();
            if (portNames.Length > 0)
            {
                do
                {
                    index++;
                    comboBox2.Items.Add(portNames[index]);
                }
                while (!((portNames[index] == comPortName) || (index == portNames.GetUpperBound(0))));
            }
        }

        
        #region debuging&testing
        int kapa = 0;
        private void button4_Click(object sender, EventArgs e)
        {
          
            richTextBox1.Text = "";
           /* I.Tunel.Light();
            I.Curve.Light();*/
            switch (kapa)
            {
                    /*
                case 0: I.Tunel.Light(); break;
                case 1: I.Tunel.Shut(2,0,I.Tunel.ColumnsCount,true); break;
                case 2: I.Tunel.Shut(3,0,I.Tunel.RowsCount,false); break;
                case 3: I.Tunel.Light(2, 3); break;
                case 4: I.Curve.Light(); break;
                case 5: I.Curve.Shut(1, 1); break;*
                     */
                case 0: I.All.Light(); break;
                case 1: I.All.Shut(2+2,0,I.All.ColumnsCount,true); break;
                case 2: I.All.Shut(3,0,I.All.RowsCount,false); break;
                case 3: I.All.Light(2+2, 3); break;
                case 4: I.All.Light(0,0,I.All.ColumnsCount,true);I.All.Light(1,0,I.All.ColumnsCount,true); break;
                case 5: I.All.Shut(1, 1); break;
                case 6: I.All.Shut(0, 0); break;
                case 7: I.All.Shut(4, 2); break;
            }
            I.SplitToTwo();
            /*
            for (int i = 0; i < I.Tunel.RowsCount; i++)
            {
                for (int j = 0; j < I.Tunel.ColumnsCount; j++)
                {
                    richTextBox1.Text += (I.Tunel.lights[i, j]) ? "1" : "0";
                }
                richTextBox1.Text += "\r\n";
            }

            richTextBox1.Text += "\r\n";

            for (int i = 0; i < I.Curve.RowsCount; i++)
            {
                for (int j = 0; j < I.Curve.ColumnsCount; j++)
                {
                    richTextBox1.Text += (I.Curve.lights[i, j]) ? "1" : "0";
                }
                richTextBox1.Text += "\r\n";
            }
            */

            
            kapa++;
            DisplayAsMatrix();
            //DisplayAsBinary();
            DisplayAsByte();
            DisplayTunelLights(I.Tunel.lights, I.Tunel.RowsCount, I.Tunel.ColumnsCount);  //dispay lighs
            DisplayCurveLights(I.Curve.lights, I.Curve.RowsCount, I.Curve.ColumnsCount);
           
            /*
            I.ComputeTrimLights();
            BeatConst.Text = "";
            for (int i = 1; i < full.Length; i++)
            {
                BeatConst.Text += ((full[i]) ? "1" : "0") + ((i % 5 == 0) ? " " : "");
            }
            textBox2.Text = "" + I.TrimBounds[0] + " " + I.TrimBounds[1];
            */
            


            
        }

        

        private void DisplayAsByte()
        {
            byte[] bts = GetBytes();
            
            for (int i = 0; i < bts.Length; i++)
            {
                richTextBox1.Text += "\r\n";
                richTextBox1.Text += ""+bts[i];
               // port.Write("" + Convert.ToChar(bts[i]));
            }

        }

        private void DisplayAsBinary()
        {
            bool[] lts = I.GetLights();
            byte[] bts = GetBytes();
            string b = "0";
            
            for (int i = 0; i < bts.Length; i++)
            {
                

                if (i % 7 == 0)
                {
                    MessageBox.Show("" + i);
                    byte sa = Convert.ToByte(b, 2);
                   // richTextBox1.Text += sa;
                    richTextBox1.Text += "\r\n";
                    richTextBox1.Text += bts[i/7];
                    richTextBox1.Text += "\r\n";
                    b = "0";

                }
               // b  += (lts[i]) ? "1" : "0";

            }
        }

        private void DisplayAsMatrix()
        {
            bool[] lts = I.GetLights();
            for (int i = 0; i < lts.Length; i++)
            {
                if(i<I.Tunel.lights.Length)
                {
                    if ( i%I.Tunel.ColumnsCount == 0)
                        richTextBox1.Text += "\r\n";
                    richTextBox1.Text += (lts[i]) ? "1" : "0";
                }
                else
                {
                    
                    if ((i) % I.Curve.ColumnsCount == 0)
                        richTextBox1.Text += "\r\n";
                    richTextBox1.Text += (lts[i]) ? "1" : "0";
                }
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
           // SendViaSerial();
           
        }

        private void button8_Click(object sender, EventArgs e)
        {
            TestSendShutAll();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            TestSend1();
        }

        private void TestSendLightAll()
        {
            string toSend = "";
           // toSend += "" + Convert.ToChar(129);
           port.Write("c");
           /* for (int i = 1; i <= 10; i++)
            {
                toSend+="" + Convert.ToChar(127);
            }
            port.Write(toSend);*/
        }

        private void TestSendShutAll()
        {
            string toSend = "";
            toSend += "" + Convert.ToChar(129);
            // port.Write("" + Convert.ToChar(129));
            for (int i = 1; i <= 10; i++)
            {
                toSend += "" + Convert.ToChar(0);
            }
            port.Write(toSend);
        }

        private void TestSend1()
        {
            string toSend = "";
            toSend += "" + Convert.ToChar(131);
            // port.Write("" + Convert.ToChar(129));
            for (int i = 3; i < 6; i++)
            {
                toSend += "" + Convert.ToChar(127);
            }
            toSend += "" + Convert.ToChar(98);
            port.Write(toSend);
        }

        private void SendViaSerial()
        {
            byte[] bytesToSend = GetBytes();

            for (int i = 0; i < bytesToSend.Length; i++)
            {
                port.Write(""+Convert.ToChar(bytesToSend[i]));
            }
            dataReceived = false;
        }

        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfiguration();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadLastConfiguration();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            I.MethodName = comboBox1.SelectedItem.ToString();
        }

        private void comboBox1_StyleChanged(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

           
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            KILL("WavPlayer");
        }

        private void KILL(string procName)
        {
            System.Diagnostics.Process[] procs = null;

            try
            {
                procs = Process.GetProcessesByName(procName);

                Process mspaintProc = procs[0];

                if (!mspaintProc.HasExited)
                {
                    mspaintProc.Kill();
                }
            }
            catch { }
            finally
            {
                if (procs != null)
                {
                    foreach (Process p in procs)
                    {
                        p.Dispose();
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Process.Start(@"WaveDebug\WavPlayer.exe");
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            maxAudioLevel = trackBar1.Value;
            progressBar1.Maximum = maxAudioLevel;
            progressBar2.Maximum = maxAudioLevel;
            I.MaxAudioLevel = maxAudioLevel;
        }

        

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string portName = comboBox2.SelectedItem.ToString();
           try
            {
                port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
                port.Open();
                port.DataReceived += port_DataReceived;
            }
            catch
            {
                MessageBox.Show("Portul ales nu exista, sau nu se deschide: " + portName);
            }
        }

        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            string indata = sp.ReadLine();
            int asciicode;
           MessageBox.Show(indata);
            Int32.TryParse(indata, out asciicode);

            char answer = (char)asciicode;

            MessageBox.Show(""+indata +"  "+ answer+" w/ code "+asciicode+"  ");
        }



        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        //nimic
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            I.Enabled = false;
            I.SelectedDeviceIndex = Convert.ToInt32((comboBox3.SelectedItem as string).Split(' ')[0]);
            I.Enabled = true;
        }

        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            
            int i;
            int j;
            int X = e.X+1;
            int Y = e.Y+1;
            
            float width = panel4.Width / I.Curve.RowsCount;      //get the width of a line
            float height = panel4.Height / I.Curve.ColumnsCount; //get the width of a line
            j = I.Curve.ColumnsCount - (int)Math.Truncate(Y / height)-1;
            i = (int)Math.Truncate(X / width);
            
            if(I.Curve.lights[i,j])
            {
                I.Curve.Shut(i, j);
            }
            else
            {
                I.Curve.Light(i, j);
            }

            DisplayCurveLights(I.Curve.lights, I.Curve.RowsCount, I.Curve.ColumnsCount);

        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            int i;
            int j;
            int X = e.X+1;
            int Y = e.Y+1;
            float width = panel3.Width / I.Tunel.RowsCount; //get the width of a line
            float height = panel3.Height / I.Tunel.ColumnsCount; //get the width of a line
            i = (int)(Y / height);
            j = (int)(X / width);

            if (I.Tunel.lights[i, j])
            {
                I.Tunel.Shut(i, j);
            }
            else
            {
                I.Tunel.Light(i, j);
            }

            DisplayTunelLights(I.Tunel.lights, I.Tunel.RowsCount, I.Tunel.ColumnsCount);
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TestSendLightAll();


        }

        private void button10_Click(object sender, EventArgs e)
        {


            //SerialPort sp = (SerialPort)sender;

            string indata = port.ReadLine();
            int asciicode;
            // MessageBox.Show(indata);
            Int32.TryParse(indata, out asciicode);

            char answer = (char)asciicode;

            MessageBox.Show("" + indata + "  " + answer + " w/ code " + asciicode + "  ");
        }

        

       

        


    }
}

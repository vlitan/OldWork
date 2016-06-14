using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spec1
{
    public delegate void UpdatedEventHandler(object sender, EventArgs e);

    public partial class Form1 : Form
    {



        public Form1()
        {
            InitializeComponent();
            g = panel1.CreateGraphics();            //create the graphics on a panel
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;// smooth out the drawing procces
            b = panel2.CreateGraphics();
            b.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            timer1.Enabled = false;                 //disable the timer
            timer1.Interval = Convert.ToInt32(1000 / analyserFreq);

            /*
            analyzer = new Analyzer(lines);         //new analyzer
            analyzer.Updated += analyzer_Updated;   //overload the event
            SpectrumData = new float[lines + 1];        //initialize SpectrumData
            beatDetector = new Beat(lines, beatLines, analyserFreq);
            */

            interpreter = new Interpreter(lines, beatLines, 3, 3, analyserFreq, false, 1.27f);
            button1.Enabled = true;
            progressBar1.Enabled = true;            //and the progressBars
            progressBar2.Enabled = true;
            progressBar1.Maximum = Int16.MaxValue;
            progressBar2.Maximum = Int16.MaxValue;
            AnFreq.Text = "" + analyserFreq;
            SpectrumBands.Text = "" + lines;
            BeatBands.Text = "" + beatLines;

            dinamicDisplay = false;


            lights = new Lights(3, 5);
            MessageBox.Show("FormLoaded");

        }

        
        private PipeClient pipeClient;
        private int ctr = 1;

        int lines = 40;         //number of lines retrieved from the analyzer
        int beatLines = 3;
        int analyserFreq = 55;   //in Hz
        Analyzer analyzer;          //the anayzer, never use more than 1
        float[] SpectrumData;       //the array where the SpectrumData is stored in the main program
        int Right;              
        int Left;
        bool dinamicDisplay;    //true if the display is done one by one, as the analyzer gets the data

        Graphics g;             //..
        Graphics b;
        bool Enabled = false;
        Beat beatDetector;

        Lights lights;
        Interpreter interpreter;
        
        private void Form1_Load(object sender, EventArgs e)
        {
           

        }
        //TODO: reinit , currently not working
        

        private void button3_Click(object sender, EventArgs e)
        {
            
            try
            {
                analyserFreq = Convert.ToInt32(AnFreq.Text);
                lines = Convert.ToInt32(SpectrumBands.Text);
                beatLines = Convert.ToInt32(BeatBands.Text);
            }
            catch
            {
                MessageBox.Show("Error converting input to Ints");
            }

            analyzer.Free();
            analyzer = new Analyzer(lines);
            analyzer.Updated += analyzer_Updated;
            SpectrumData = new float[lines + 1];
            timer1.Enabled = false;                 //disable the timer
            timer1.Interval = Convert.ToInt32(1000 / analyserFreq);
            b.Clear(Color.White);
            g.Clear(Color.White);
            beatDetector = new Beat(lines, beatLines, analyserFreq,0);
        }

        //this function is called every time the Updated event occurs
        //this way every time a new value is found, it is displayed
        void analyzer_Updated(object sender, EventArgs e)
        {
           
            bool dataIsValid=!((analyzer.CurrentIndex==0)&&(analyzer.CurrentValue==-1));
            if (!dataIsValid)
                MessageBox.Show("Invalid data in dimamic display");
            if (dinamicDisplay && dataIsValid)
                DisplayOne(analyzer.CurrentIndex, analyzer.CurrentValue);
           
        }

        //basicaly draws a white rectangle over that strip of the panel
        //then draws a black one, depending of the value
        void DisplayOne(int index, float value)
        {
           
            //the width of a line depends of the number of lines an the width of the panel
            float width = panel1.Width / (lines);
            SolidBrush solidBlack = new SolidBrush(Color.Black);
            SolidBrush solidWhite = new SolidBrush(Color.White);
            float proportionalValue;
            //the max value is 255. the height of the rectangle is made proportional with the height of the panel
            //(regula de trei simpla)
            proportionalValue = value * panel1.Height / 255;
            
            g.FillRectangle(solidWhite, index * width, 0, width -1, panel1.Height);//clear the column
            g.FillRectangle(solidBlack, index * width, panel1.Height - proportionalValue, width -1, proportionalValue);//draw the black recangle

        }

        

        //this function displays the hole array of data
        public void DisplayAll(float[] spectrumData)
        {

            g.Clear(Color.White);               //clear the panel
            float width = panel1.Width / (lines);//get the width of a line
            SolidBrush solidBlack = new SolidBrush(Color.Black);
            float proportionalValue;
            
            for (int x = 0; x<lines; x++)
            {
                proportionalValue =  spectrumData[x] * panel1.Height / 255;// see DisplayOne
                g.FillRectangle(solidBlack, x * width, panel1.Height-proportionalValue, width - 1, proportionalValue);
            }
            
            
        }

        //Display the Right and Left Valuein progressBars
        private void DisplayRightLeft()
        {
            progressBar1.Value = (Right > progressBar1.Maximum) ? progressBar1.Maximum : Right;
            progressBar2.Value = (Left > progressBar2.Maximum) ? progressBar2.Maximum : Left;
        }

        //enable and disable button
        private void button1_Click(object sender, EventArgs e)
        {
            if(Enabled)
            {
                timer1.Enabled = false;
               // analyzer.Enable = false;
                interpreter.Enabled = false;
                button1.Text = "disabled";
                Enabled = false;
            }
            else
            {
                timer1.Enabled = true;
                //analyzer.Enable = true;
                interpreter.Enabled = true;
                button1.Text = "enabled";
                Enabled = true;
            }
        }

        void DisplayBeats(float[] Beats)
        {
            b.Clear(Color.White);               //clear the panel
            float width = panel2.Width / beatLines;//get the width of a line
            SolidBrush solidRed = new SolidBrush(Color.Red);
            for (int i = 0; i < beatLines; i++)
            {
               if(Beats[i]>0)
                    b.FillRectangle(solidRed, i * width,0 , width - 1, panel2.Height);
                
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*
            //get the data in SpectrumData
            SpectrumData = analyzer.GetSpectrum(lines,ref Right,ref Left);
            beatDetector.PushData(SpectrumData);
            //if it is ok display it
            if((SpectrumData[0]!=-1)&&(!dinamicDisplay))
            {
                DisplayAll(SpectrumData);
            }
            DisplayBeats(beatDetector.Beats);
            DisplayRightLeft();
            */
            interpreter.Update();

            SpectrumData = interpreter.Spectrum;
            DisplayAll(SpectrumData);
            DisplayBeats(interpreter.Beats);

            
            interpreter.GetLights();
            string dat;
            dat = "";
            for (int i = 0; i < beatLines; i++)
            {
                dat += ""+ ((interpreter.Beats[i] > 0) ? 1 : 0) ;
            }
            

            Right = interpreter.Right;
            Left = interpreter.Left;
            DisplayRightLeft();
            


            
        }

        private void SendData(string Data)
        {
            pipeClient = new PipeClient();
            pipeClient.Send(Data + " - " + ctr.ToString(), "TestPipe", 1000);
            ctr++;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            dinamicDisplay = !dinamicDisplay;
            button2.Text = (dinamicDisplay) ? "dinamic" : "complet";
        }
        int kapa = 0;
        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            switch (kapa)
            {
                case 0: lights.Light(); break;
                case 1: lights.Shut(2); break;
                case 2: lights.Shut(-3); break;
                case 3: lights.Light(2, 3); break;
            }
            for (int i = 1; i <= lights.RowsCount; i++)
            {
                for (int j = 1; j <=lights.ColumnsCount; j++)
                {
                    richTextBox1.Text += (lights.lights[i, j]) ? "1" : "0";
                }
                richTextBox1.Text+="\r\n";
            }
            kapa++;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            pipeClient = null;
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            
        }

        

 
    }
}

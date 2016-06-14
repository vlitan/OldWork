using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Spec1
{
    class Interpreter
    {
        public event UpdatedEventHandler Updated;
        protected virtual void OnUpdated(EventArgs e)
        {
            if (Updated != null)
            {
                Updated(this, e);
            }
        }

        private Analyzer spectrumAnalyzer;
        private Beat beatDetector;
        private Lights tunel;
        private Lights curve;
        private int freq; //frequency in Hz
        public Timer timer;
        private int spectrumLines;


        private Lights all;
        public Lights All
        {
            get { return all; }
            set { all = value; }
        }

        private float[] spectrum;
        private float[] beats;
        private int right;
        private int left;
        private bool enabled;
        private bool[] LastLghts;
        private byte[] trimBounds;

        private int maxAudioLevel;

        private string methodName;

        public string MethodName
        {
            get
            { return methodName; }
            set
            { methodName = value; }

        }

        public byte[] TrimBounds
        {
            get { return trimBounds; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                spectrumAnalyzer.Enable = value;

            }
        }

        public List<string> Devices
        {
            get { return spectrumAnalyzer.Devices; }
        }

        public int SelectedDeviceIndex
        {
            get { return spectrumAnalyzer.SelectedIndex; }
            set { spectrumAnalyzer.SelectedIndex = value; }
        }

        public int Frequency
        {
            get { return freq; }
        }

        public float[] Spectrum
        {
            get { return spectrum; }
        }

        public float[] Beats
        {
            get { return beats; }
        }

        public int Left
        {
            get { return left; }
        }
        public int Right
        {
            get { return right; }
        }

        public Lights Tunel
        {
            get { return tunel; }
        }

        public Lights Curve
        {
            get { return curve; }
        }

        public int MaxAudioLevel
        {
            get { return maxAudioLevel; }
            set
            { maxAudioLevel = value; }
        }

        private delegate void Updater();

        //citeste si in Lights;

        /*obiectu asta Interpreter face cam toata treaba.
         * in constructor trebe specificat pe cate segmente analizeaza spectrul, 
         * pe cate segmente de frecventa analizeaza beatul.
         * cate linii si cate coloane o sa aiba matricea in care se retin luminile
         * frecventa la care se lucreaza (40-50 hz ii foarte ok, de preferat 40)
         * si o booleana care indica daca obiectul va folosi un timer intern sau nu
         * de preferat sa nu se foloseasca unul, si sa se apeleze din exterior metoda Update(),
         * la frecventa specificata
         * ultilul parametru este constanta de sensibilitate pentru BeatDetector explicata in clasa respectiva
         *
         * toate datele (luminile, spectrumul, beaturile, etc sunt disponibile, ele fiind proprietati readOnly)
         */
        public Interpreter(int SpectrumLines,int MaxAudioLevel, int BeatLines, int TunelRows, int TunelColumns,int CurveRows, int CurveColumns, int Frequency, bool UseTimer, float BeatConstant)
        {
            spectrumAnalyzer = new Analyzer(SpectrumLines);
            beatDetector = new Beat(SpectrumLines, BeatLines, Frequency, BeatConstant);
            tunel = new Lights(TunelRows, TunelColumns);
            curve = new Lights(CurveRows, CurveColumns);
            enabled = false;
            freq = Frequency;
            timer = new Timer();
            timer.Interval = Convert.ToInt32(1000 / freq);
            timer.Tick += timer_Tick;
            trimBounds = new byte[2];
            maxAudioLevel = MaxAudioLevel;

         //   GetInClockWiseSpiralArray(ref spiralIs,ref spiralJs);

            all = new Lights(8, 6);
            
            if(UseTimer)
            {
                timer.Enabled = true;
            }
            else
            {
                timer.Enabled =false;
            }

            LastLghts = new bool[tunel.lights.Length+curve.lights.Length];
            spectrumLines = SpectrumLines;

            methodName = "";
            
        }

        void timer_Tick(object sender, EventArgs e)
        {
            Update();
        }
        private int contB = 0;
        private int contH = 0;

        /*in functia asta trebuie interactionat cu luminile si modificat
        ii foarte importanta si aici o sa avem de lucru in principal
        acuma ii doar un exemplu
         
         de citit si in Lights!@*/



        #region UpdateMethods
        // rules for adding a new method
        //#1 : make a description
        //#2 : every method has to be  in mtds array below
        //#3 : every method has to be in the switch, as donw below


        

        public string[] GetMethods()
        {
            string[] mtds = new string[] { 
            "Experimental1",
            "ShowVolume",
            "InClockWiseSpiral"};
            return mtds;
        }

        private void UpdateLights()
        {
            switch(methodName)
            {
                case "Experimental1": Experimental1(); break;
                case "ShowVolume": ShowVolume(); break;
                case "InClockWiseSpiral": InClockWiseSpiral(); break;
            }
        }

        public void Experimental1() //0
        {
            if (IsBeat(0))
            {
                tunel.Shut(contB,0,tunel.ColumnsCount,true);
                if (contB+1 >= tunel.RowsCount)
                {
                    contB = 0;
                }
                else
                {
                    contB++;
                }
                tunel.Light(contB, 0, tunel.ColumnsCount, true);
            }
            if (IsBeat(2))
            {
                tunel.Shut(contH,0,tunel.RowsCount,false);
                if (contH +1>= tunel.ColumnsCount)
                {
                    contH = 0;
                }
                else
                {
                    contH++;
                }
                tunel.Light(contH, 0, tunel.RowsCount, false);
            }
        }

        private int[] spiralIs;
        private int[] spiralJs;

        private void InClockWiseSpiral()
        { }

        public void GetInClockWiseSpiralArray(ref int[] Is,ref int[] Js)
        {
             int top = 0;
             int down = all.RowsCount - 1;
             int left = 0;
             int right = all.ColumnsCount-1;
            
 
             while(true)
             {
                 // Print top row
                 for(int j = left; j <= right; ++j) 
                 {
                     all.Light(top,j);
                 }
                 top++;
                 if(top > down || left > right) break;
                 //Print the rightmost column
                 for(int i = top; i <= down; ++i)
                 {
                     all.Light(i,right);
                 }
                 right--;
                 if(top > down || left > right) break;
                 //Print the bottom row
                 for(int j = right; j >= left; --j) 
                 {
                     all.Light(down,j);
                 }
                 down--;
                 if(top > down || left > right) break;
                 //Print the leftmost column
                 for(int i = down; i >= top; --i)
                 {
                     all.Light(i, left);
                 }
                 left++;
                 if(top > down || left > right) break;
             }
             SplitToTwo();
        }

        //display Volume from Curve to back
        //using right and left.
        
        int maxLights = 9;
        int propValueRight;
        int propValueLeft;

        public void SplitToTwo()
        {
            for (int i = 0; i < curve.ColumnsCount; i++)
            {
                curve.lights[0, i] = all.lights[0, i];
            }
            for (int i= curve.ColumnsCount-1; i>=0; i--)
            {
                curve.lights[4, i] = all.lights[0, 3 + curve.ColumnsCount - i-1];
            }

            for (int i = 0; i < curve.ColumnsCount; i++)
            {
                curve.lights[1, i] = all.lights[1, i];
            }
            for (int i = curve.ColumnsCount - 1; i >= 0; i--)
            {
                curve.lights[3, i] = all.lights[1, 3 + curve.ColumnsCount - i-1];
            }

            for (int i = 0; i < tunel.RowsCount; i++)
            {
                for (int j = 0; j < tunel.ColumnsCount; j++)
                {
                    tunel.lights[i, j] = all.lights[i + 2,j];
                }
            }
        }

        public void ShowVolume() //1
        {
            Shut();

            propValueRight = (int)Math.Round((double)(right * maxLights / maxAudioLevel));
            propValueLeft = (int)Math.Round((double)(left * maxLights / maxAudioLevel));

            Console.WriteLine(""+propValueLeft+"  "+propValueRight);
            LightLeft(propValueLeft);
            LightRight(propValueRight);  
        }
        #region AuxFunctions for "ShowVolume"


        
        private void LightLeft(int level)
        {
            int middle = 2 ;
            for (int i = middle; i >= middle - Math.Min(level, middle); i--)
            {
                curve.Light(i, 0, curve.ColumnsCount, true);
            }
            if (level > 3)
            {
                level -= 3;
                int mid = tunel.ColumnsCount / 2;
                for (int i = 0; i <mid; i++)
                {
                    tunel.Light(i, 0, level, false);
                }

            }
        }
        private void LightRight(int level)
        {
            int middle = 2;
            for (int i = middle; i <= middle+Math.Min(level,middle); i++)
            {
                curve.Light(i, 0, curve.ColumnsCount, true);
            }
            if(level>3)

            {
                int mid = tunel.ColumnsCount / 2;
                level -= 3;
         
                for (int i = mid; i < tunel.ColumnsCount; i++)
                {
                    tunel.Light(i, 0, level, false);
                }
                
            }
            
        }
        #endregion
     

        public void GetNextInSpiral(ref int i, ref int j,ref int Orientation, ref int Turn, bool ClockWise)
        {
            if(ClockWise)
            { 
                switch (Orientation)
                {
                  

                }
            }
            else
            {

            }
        }

        private int GetJ(int index, int columnsNumber)
        {
            return index % columnsNumber;
        }
        private int GetI(int index, int rowsNumber)
        {
            return index / rowsNumber;
        }
        

        public bool IsBeat(int index)
        {
            return (beats[index] > 0) && (beatDetector.LastBeats[index]<0);
        }
        #endregion 



        //transforma matricea intr-un sir de booleene care ulterior este codificat si trimis pe serial
        
        public bool[] GetLights()
        {
            bool[] lghts=new bool[tunel.lights.Length+curve.lights.Length];
            for (int i = 0; i < tunel.RowsCount; i++)
            {
                for (int j = 0; j < tunel.ColumnsCount; j++)
                {
                    lghts[i * tunel.ColumnsCount + j] = tunel.lights[i, j];
                }
            }
            for (int i = 0; i < curve.RowsCount; i++)
            {
                for (int j = 0; j < curve.ColumnsCount; j++)
                {
                    lghts[tunel.RowsCount * tunel.ColumnsCount + i * curve.ColumnsCount + j] = curve.lights[i, j];
                } 
            }
            return lghts;
        }

        //functia este publica deci poate fi apelata dintr-un timer extern sau in cel extern
        //updateaza toate datele
        public void Update()
        {
            spectrum = spectrumAnalyzer.GetSpectrum(spectrumLines, ref right, ref left);
            right = Math.Min(maxAudioLevel, right);
            left = Math.Min(maxAudioLevel, left);
            beatDetector.PushData(spectrum);
            beats = beatDetector.Beats;
            UpdateLights();
            ComputeTrimLights();
            OnUpdated(EventArgs.Empty);
        }

        private bool firstTimeHere=true;
        public void ComputeTrimLights()
        {
            
            bool[] Lghts = GetLights();
            int StartIndex = 0;
            int EndIndex = 0;
            if(firstTimeHere)
            {
                firstTimeHere = false;
                StartIndex = 1;
                EndIndex = Lghts.Length+1;
            }
            else
            {
                for (int i = 0; i < Lghts.Length; i++)
                {
                    if((Lghts[i]!=LastLghts[i])&&(StartIndex==0))
                    {
                        StartIndex = i;
                    }
                    if(Lghts[i]!=LastLghts[i])
                    {
                        EndIndex = i;
                    }
                }
            }
            LastLghts = Lghts;
            trimBounds[0]=(byte)(StartIndex/7+1);
            trimBounds[1]=(byte)(EndIndex/7+1);

        }
        public void Shut()
        {
            tunel.Shut();
            curve.Shut();
        }
        
    }

    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public string dta;
        private Analyzer spectrumAnalyzer;
        private Beat beatDetector;
        private Lights lights;
        private int freq; //frequency in Hz
        public Timer timer;
        private int spectrumLines;

        private float[] spectrum;
        private float[] beats;
        private int right;
        private int left;

        private bool enabled;

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

        public Lights Lights
        {
            get { return lights; }
        }

        public bool Enabled
        {
            get 
            {
                return enabled;
            }
            set
            {
                spectrumAnalyzer.Enable = value;
                enabled = value;
                timer.Enabled = value;
            }
        }

        public Interpreter(int SpectrumLines, int BeatLines, int LightRows, int LightColumns, int Frequency, bool UseTimer, float BeatConstant)
        {
            spectrumAnalyzer = new Analyzer(SpectrumLines);
           
            beatDetector = new Beat(SpectrumLines, BeatLines, Frequency,BeatConstant);
            lights = new Lights(LightRows, LightColumns);
            
            freq = Frequency;
            timer = new Timer();
            timer.Interval = Convert.ToInt32(1000 / freq);
            timer.Tick += timer_Tick;
            if(UseTimer)
            {
                timer.Enabled = true;
            }
            else
            {
                timer.Enabled =false;
            }
            

            spectrumLines = SpectrumLines;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            Update();
        }
        private int contB = 0;
        private int contH = 0;
        private void UpdateLights()
        {
            if(beats[0]>0)
            {
                lights.Shut(contB);
                if(contB>lights.RowsCount)
                {
                    contB = 1;
                }
                else
                {
                    contB++;
                }
                lights.Light(contB);
            }
            if(beats[1]>0)
            {
                lights.Shut(contH);
                if (contH > lights.RowsCount)
                {
                    contH = 1;
                }
                else
                {
                    contH++;
                }
                lights.Light(-contH);
            }
        }

        public bool[] GetLights()
        {
            bool[] lghts=new bool[lights.RowsCount*lights.ColumnsCount];
            for (int i = 0; i < lights.RowsCount; i++)
            {
                for (int j = 0; j < lights.ColumnsCount; j++)
                {
                    lghts[i * lights.ColumnsCount + j] = lights.lights[i, j];
                    dta += (lights.lights[i, j]) ? 1 : 0;
                }
            }
            return lghts;
        }

        public void Update()
        {
            spectrum = spectrumAnalyzer.GetSpectrum(spectrumLines, ref right, ref left);
            beatDetector.PushData(spectrum);
            beats = beatDetector.Beats;
            OnUpdated(EventArgs.Empty);
        }
    }
}

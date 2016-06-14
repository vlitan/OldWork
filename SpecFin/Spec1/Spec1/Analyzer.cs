using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

//those are used to analyze the data
//the dlls have to be in the same folder with the exe.
using Un4seen.Bass;         
using Un4seen.BassWasapi;   

  

namespace Spec1
{
    
    /// <summary>
    ///Inspired by webmaster442(on codeproject.com)
    
    /// The main program has to make requests about the current data
    /// this way de frequency of the requests, the processing/display data
    /// can be done without having to modify anithing at all in the Analyzer class
    /// also, the event Udated occurs when a value is modified in SpectrumData
    /// in order to smooth-out the drawing and processing 
    /// </summary>

    

    class Analyzer
    {

        public event UpdatedEventHandler Updated;

        //is occurs when a value is modified in the array of data
        protected virtual void OnUpdated(EventArgs e)
        {
            if (Updated != null)
            {
                Updated(this, e);
            }
        }

        private bool enable;               //enabled status
        private float[] fftBuffer;         //buffer for fft data
        private float left, right;         //progressbars for left and right channel intensity
        private float[] SpectrumData;       //duh..

        private int currentIndex;           //the current index in SpectrumData
        private float currentValue;         //the current value in SpectrumData at currentIndex

        private WASAPIPROC process;        //callback function to obtain data
        private int lastLevel;             //last output level
        private int hanCtr;                //last output level counter

        private List<string> devices;      //list containing output devices (for future development)
        private int selectedIndex;         //the selected index in the devices list
        private bool initialized;          //initialized flag
        private int devIndex;              //used device index

        
        //CurrentIndex and CurrentValue are ReadOnly
        //are used when the display is done dinamicaly
        public int CurrentIndex
        {
            get
            {
                return currentIndex;
            }
        }

        public float CurrentValue
        {
            get
            {
                return currentValue;
            }
        }

        public float[] Spectrum
        {
            get { return SpectrumData; }
        }
        
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                //verify if the index is found in the list
                if (value < devices.Count)
                    selectedIndex = value;
                else
                    MessageBox.Show("There is no " + value + " device in the list");
            }
        }
        //is readonly
        public List<string> Devices
        {
            get { return devices; }
        }
        //the enable statement
        //stops or starts the Analyze
        //in case re-initialize it
        public bool Enable
        {
            get { return enable; }
            set
            {
                enable = value;
                if(value)
                {
                    if (!initialized)
                    {
                        //used for selecting an output device
                        string str = (devices[selectedIndex]);
                        string[] array = str.Split(' ');
                        devIndex = Convert.ToInt32(array[0]);
                        bool result = BassWasapi.BASS_WASAPI_Init(devIndex, 0, 0, BASSWASAPIInit.BASS_WASAPI_BUFFER, 1f, 0.05f, process, IntPtr.Zero);
                        if (!result)
                        {
                            var error = Bass.BASS_ErrorGetCode();
                            MessageBox.Show(error.ToString()+"()");
                        }
                        else
                        {
                            initialized = true;
                        }
                        
                    }
                    BassWasapi.BASS_WASAPI_Start();
                    
                }
                else
                    BassWasapi.BASS_WASAPI_Stop(true);
                //System.Threading.Thread.Sleep(500);
            }
        }

        public Analyzer(int lines)
        {
            fftBuffer = new float[1024];
            lastLevel = 0;
            hanCtr = 0;
            right = left = 0;
            initialized = false;
            currentIndex = 0;
            currentValue = 0;
            devIndex = 0;
            selectedIndex = 0;
            devIndex = 0;
            process = new WASAPIPROC(Process);
            devices = new List<string>();
            this.Init();
            SpectrumData=new float[lines];
        }

       

        public float[] GetSpectrum(int lines, ref int R, ref int L)
        {
            int ret = BassWasapi.BASS_WASAPI_GetData(fftBuffer, (int)BASSData.BASS_DATA_FFT2048); //get channel fft data
            //if the Data was successfully retrieved
            if (ret != -1)
            {
                int x, y;
                int b0 = 0;

                //computes the spectrum data, the code is taken from a bass_wasapi sample.
                for (x = 0; x < lines; x++)
                {
                    float peak = 0;
                    int b1 = (int)Math.Pow(2, x * 10.0 / (lines - 1));
                    if (b1 > 1023) b1 = 1023;
                    if (b1 <= b0) b1 = b0 + 1;
                    for (; b0 < b1; b0++)
                    {
                        if (peak < fftBuffer[1 + b0]) peak = fftBuffer[1 + b0];
                    }
                    y = (int)(Math.Sqrt(peak) * 3 * 255 - 4);
                    if (y > 255) y = 255;
                    if (y < 0) y = 0;
                    SpectrumData[x] = y;
                    currentIndex = x;
                    currentValue = y;
                    OnUpdated(EventArgs.Empty);

                }

                //right left volume 
                int level = BassWasapi.BASS_WASAPI_GetLevel();
                left = Utils.LowWord32(level);
                right = Utils.HighWord32(level);
                R = (int)right;
                L = (int)left;

                //Required, because some programs hang the output. If the output hangs for a 75ms
                //this piece of code re initializes the output so it doesn't make a gliched sound for long.
                if (level == lastLevel && level != 0)
                    hanCtr++;

                lastLevel = level;


                if (hanCtr > 3)
                {
                    hanCtr = 0;
                    left = 0;
                    right = 0;
                    Free();
                    Bass.BASS_Init(0, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
                    initialized = false;
                    Enable = true;
                }
                
            }
            else
            {
                //otherwise the fail is market in the SpectrumData, at the first position
                SpectrumData[0] = -1;
                currentIndex = 0;
                currentValue = -1;
                OnUpdated(EventArgs.Empty);
            }

            return SpectrumData;
            
        }

        //cleanup
        public void Free()
        {
            BassWasapi.BASS_WASAPI_Free();
            Bass.BASS_Free();
        }
        //object destroyer
        ~Analyzer()
        {
            SpectrumData = null;
            right = left = 0;
            Free();
        }

        //Callback
        private int Process(IntPtr buffer, int length, IntPtr user)
        {
            return length;
        }

        //initialisation of BassWasapi stuff
        private void Init()
        {
            bool result = false;
            //this part is used when there are several output devices
            //the devices list is created, every devise is numbered
            for (int i = 0; i < BassWasapi.BASS_WASAPI_GetDeviceCount(); i++)
            {
                var device = BassWasapi.BASS_WASAPI_GetDeviceInfo(i);
                if (device.IsEnabled && device.IsLoopback)
                {
                    devices.Add(string.Format("{0} - {1}", i, device.name));
                }
            }
            selectedIndex = 0;

            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATETHREADS, false);

            result = Bass.BASS_Init(0, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            if (!result) throw new Exception("Init Error");
        }

        

       
    }
}

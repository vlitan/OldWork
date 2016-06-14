using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spec1
{

    /* the algorithm used is explained at http://article.programmersheaven.com/Patin/BeatDetectionAlgorithms.pdf
     */
    class Beat
    {
        private int bufferSize;

        private int inLines;
        private int outLines;
        private float[,] localEnergyBuffer;
        private bool bufferIsFull;
        private float[] beat;
        private int k = 0;
        private int freq;
        private float[] C;
        private float[] V;
        private bool useConstant;
        private float constant;

        public float[] Beats
        {
            get { return beat; }
        }

        public bool UseConstant
        {
            get { return useConstant; }
            set { useConstant = value; }
        }

        public float Constant
        { get
            { return constant; }
          set
            { constant = value; }
        }

        public Beat(int InLines,int OutLines,int Freq,float DeltaConstant)
        {
            inLines=InLines;
            outLines=OutLines;
            bufferSize = Freq;
            if(DeltaConstant==0)
            {
                useConstant = false;
            }
            else
            {
                constant = DeltaConstant;
            }

            bufferIsFull = false;
            localEnergyBuffer = new float[bufferSize,OutLines];
            beat = new float[OutLines];
            C = new float[outLines];
            V = new float[outLines];
            for (int i = 0; i < OutLines; i++)
            {
                beat[i] = 0;
                C[i] = 1.26f;
                
            }
        }

        private float[] GetSpectrumInstant(float[] Spectrum)
        {
            float[] outPut = new float[outLines];
            for (int i = 0; i < inLines; i++)
            {
                outPut[i*outLines/inLines] += Spectrum[i];
            }
            return outPut;
        }

        public void PushData(float[] Spectrum)
        {
            float[] Instant = new float[outLines];
            Instant = GetSpectrumInstant(Spectrum);
            //fill the buffer
            if(!bufferIsFull)
            {
                
                for (int i = 0; i < outLines; i++)
                {
                    localEnergyBuffer[k, i] = Instant[i];
                }
                k++;
                if (k >= bufferSize)
                    bufferIsFull = true;
            }
            else
            {
                //sum up
                float[] localAverage=new float[outLines];
                for (int i = 0; i < outLines; i++)
                {
                    for (int j = 0; j < bufferSize; j++)
                    {
                        localAverage[i] += localEnergyBuffer[j, i];
                    }
                    localAverage[i] /= bufferSize;
                    
                }
                if (!useConstant)
                {
                    //compute Variance

                    for (int i = 0; i < outLines; i++)
                    {
                        for (int j = 0; j < bufferSize; j++)
                        {
                            V[i] += (float)(Math.Abs(localEnergyBuffer[j, i] - localAverage[i]));
                        }
                        V[i] /= bufferSize;

                    }

                    //compute C

                    for (int i = 0; i < outLines; i++)
                    {
                        C[i] = (-0.0025714f * V[i]) + 1.5142857f;
                    }
                }
                
                //compare
                for (int i = 0; i < outLines; i++)
                {
                    beat[i] = Instant[i] -( (useConstant)?constant:C[i]) * localAverage[i];
                }

                //shift to right 1 position
                for (int i = 0; i < outLines; i++)
                {
                    for (int j = bufferSize-1; j > 0; j--)
                    {
                        localEnergyBuffer[j, i] = localEnergyBuffer[j-1, i];
                    }
                }

                //pile
                for (int i = 0; i < outLines; i++)
                {
                    localEnergyBuffer[0, i] = Instant[i];
                }
                
            }
        }

        //destructor
        ~Beat()
        {
            C = null;
            V = null;
            localEnergyBuffer = null;
        }

        



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Spec1
{
    
    class Lights
    {
        public event UpdatedEventHandler Updated;
        protected virtual void OnUpdated(EventArgs e)
        {
            if (Updated != null)
            {
                Updated(this, e);
            }
        }

        public bool[,] lights;
        private int n;
        private int m;

        public int RowsCount
        {
            get { return n; }
        }

        public int ColumnsCount
        {
            get { return m; }
        }



        /*ideea de baza al obiectului este sa usureze controlul
         metodele Light/Shut sunt overloaded
         apelate fara parametru au efect pe toate luminile
         cu un parametru au efect asupra unei linii(daca parametrul e pozitiv) 
         sau asupra unei coloane, daca e negativ
         apelata cu 2 parametrii are efect asupra unui singur punct
         */


         //!!!! important: matricea are coltul stanga sus in [1,1] si dreapte jos in [n,m]*/

        public Lights(int rows, int columns)
        {
            n = rows;
            m = columns;

            lights = new bool[n, m];
        }


        ///*******Light/Shut all the lights******\\\\\
        public void Light()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    lights[i, j] = true;
                }
            }
        }

        public void Shut()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    lights[i, j] = false;
                }
            }
            
        }
        ///***********************************\\\
    


        ///*****Light/shut on 1 dimension\\\\\\\\
        //for x>0 on rows, for x<0 on columns
        public void Light(int I, int StartIndex, int EndIndex, bool OnRow) // x>0 on rows
        {
            if (OnRow)
            {
               
                if (InBounds(I, 1))
                {
                    for (int j = StartIndex; j < EndIndex; j++)
                    {
                        lights[I, j] = true;
                    }
                }
            }
            else
            {
                int J = I;
                if (InBounds(1, J))
                {
                    for (int i = StartIndex; i < EndIndex; i++)
                    {
                        lights[i, J] = true;
                    }
                }
            }
        }

        public void Shut(int I, int StartIndex, int EndIndex, bool OnRow) 
        {
            if (OnRow)
            {

                if (InBounds(I, 1))
                {
                    for (int j = StartIndex; j < EndIndex; j++)
                    {
                        lights[I, j] = false;
                    }
                }
            }
            else
            {
                int J = I;
                if (InBounds(1, J))
                {
                    for (int i = StartIndex; i < EndIndex; i++)
                    {
                        lights[i, J] = false;
                    }
                }
            }
        }
        ////******************************\\\\\\\\\


        ///****Light/Shut one****\\\\\\\\
        public void Light(int i, int j)
        {
            if (InBounds(i, j))
                lights[i, j] = true;
            else
                MessageBox.Show("Indexes out of bounds" + i + " " + j);
        }
        public void Shut(int i, int j)
        {
            if (InBounds(i, j))
                lights[i, j] = false;
            else
                MessageBox.Show("Indexes out of bounds" + i + " " + j);
        }


        ///******\\\\
        ///
       
        ///**********************\\\\\\\\\
        
        private bool InBounds(int i, int j)
        {
            return (i >= 0) && (j >= 0) && (i < n) && (j < m);
        }

    }
}

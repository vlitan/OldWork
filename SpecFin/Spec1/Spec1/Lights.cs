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

        public Lights(int rows, int columns)
        {
            n = rows;
            m = columns;

            lights = new bool[n+1, m+1];
        }


        ///*******Light/Shut all the lights******\\\\\
        public void Light()
        {
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    lights[i, j] = true;
                }
            }
        }

        public void Shut()
        {
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    lights[i, j] = false;
                }
            }
        }
        ///***********************************\\\
    


        ///*****Light/shut on 1 dimension\\\\\\\\
        //for x>0 on rows, for x<0 on columns
        public void Light(int x) // x>0 on rows
        {
            if (x > 0)
            {
                if (InBounds(x, 1))
                {
                    for (int i = 1; i <= m; i++)
                    {
                        lights[x, i] = true;
                    }
                }
            }
            else if(x<0)
            {
                x = -x;
                if (InBounds(1, x))
                {
                    for (int i = 1; i <= n; i++)
                    {
                        lights[i, x] = true;
                    }
                }
            }
        }

        public void Shut(int x)
        {
            if (x > 0)
            {
                if (InBounds(x, 1))
                {
                    for (int i = 1; i <= m; i++)
                    {
                        lights[x, i] = false;
                    }
                }
            }
            else
            {
                x = -x;
                if (InBounds(1, x))
                {
                    for (int i = 1; i <= n; i++)
                    {
                        lights[i, x] = false;
                    }
                }
            }

        }
        ////******************************\\\\\\\\\


        ///****Light/Shut one****\\\\\\\\
        public void Light(int x, int y)
        {
            if (InBounds(x, y))
                lights[x, y] = true;
            else
                MessageBox.Show("Indexes out of bounds"+x+" "+y);
        }
        public void Shut(int x, int y)
        {
            if (InBounds(x, y))
                lights[x, y] = false;
            else
                MessageBox.Show("Indexes out of bounds" + x + " " + y);
        }

        ///**********************\\\\\\\\\
        
        private bool InBounds(int x, int y)
        {
            return (x >= 1) && (y >= 1) && (x <= n) && (y <= m);
        }

    }
}

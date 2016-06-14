using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Parser
{
    class AritmeticEvaluator
    {
        private char[] Expresion;
        private int p;

        public AritmeticEvaluator()
        { 
            Expresion=new char[1000];
        }
        public AritmeticEvaluator(char[] expresion)
        {
            Expresion = new char[1000];
            Expresion = expresion;
        }

        public float Evaluate(char[] expresion)
        {
            Expresion = expresion;
            p = 0;
            return bituire();

        }

        public float bituire(int pp)
        {
            p=pp;
            int a = adunare();

            while (Expresion[p] == '&' || Expresion[p] == '|')
            {

                if (Expresion[p] == '&')
                {
                    p++;
                    a &= adunare();
                }
                else if (Expresion[p] == '|')
                {
                    p++;
                    a |= adunare();
                }

            }
            return a;

        }

        public float bituire()
        {

            int a = adunare();

            while (Expresion[p] == '&' || Expresion[p] == '|')
            {

                if (Expresion[p] == '&')
                {
                    p++;
                    a &= adunare();
                }
                else if (Expresion[p] == '|')
                {
                    p++;
                    a |= adunare();
                }

            }
            return a;

        }



        int adunare()
        {
            float a = inmultire();

            while (Expresion[p] == '+' || Expresion[p] == '-')
            {

                if (Expresion[p] == '+')
                {
                    p++;
                    a += inmultire();
                }
                else if (Expresion[p] == '-')
                {
                    p++;
                    a -= inmultire();
                }

            }
            return (int)a;
        }

        float inmultire()
        {
            float a = term();

            while (Expresion[p] == '*' || Expresion[p] == '/')
            {
                if (Expresion[p] == '*')
                {
                    p++;
                    a *= term();
                }
                else if (Expresion[p] == '/')
                {
                    p++;
                    a /= term();
                }


            }
            return a;
        }

        float term()
        {
            float a = 0;
            if (Expresion[p] == '(')
            {
                p++;
                a = bituire();
                p++;
            }
            else if (Expresion[p] <= '9' && Expresion[p] >= '0')

                a = extract();

            else if (Expresion[p] == 'm' || Expresion[p] == 'a' || Expresion[p] == 'p')
            {

                a = funct();
            }

            return a;
        }

        float funct()
        {
            float a = 0;

            p++;
            switch (Expresion[p])
            {
                case 'o': { p++; a = pow(); break; }
                case 'b': { p++; a = abso(); break; }
                case 'i': { p++; a = min(); break; }
                case 'a': { p++; a = max(); break; }
            }
            return a;

        }

        float max()
        {
            p++;
            float max = -float.MaxValue, a;

            while (Expresion[p] != ')')
            {
                a = 0;
                p++;
                a = bituire();
                if (max <= a) max = a;
            }
            p++;
            return max;

        }

        float min()
        {
            p++;
            float max = float.MaxValue, a;

            while (Expresion[p] != ')')
            {
                a = 0;
                p++;
                a = bituire();
                if (max > a) max = a;
            }
            p++;
            return max;

        }

        float abso()
        {
            float a = 0;
            p++;
            a = (int)bituire();
            return Math.Abs(a);
        }

        float pow()
        {
            p++;
            float r = 1;
            float a = bituire();
            float b = bituire();

            r = (float)Math.Pow(a, b);
            p++;
            return r;
        }


        float extract()
        {
            float a = 0;
            while (Expresion[p] <= '9' && Expresion[p] >= '0')
            {
                a = a * 10 + Expresion[p] - '0';
                p++;
            }
            return a;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Parser
{
    class BooleanEvaluator
    {
        private char[] Expresion;
        private int p;

        public const string EQUAL = "=";
        public const string NOT = "not";
        public const string TRUE="true";
        public const string FALSE="false";
        public const string NEQUAL = "<>";
        public const string GT = ">";
        public const string GTE = ">=";
        public const string LE = "<";
        public const string LEE = "<=";

        public BooleanEvaluator()
        { 
            
        }

        public bool Evaluate(char[] expresion)
        {
            Expresion = expresion;
            p = 0;
            return LogicLevel();
        }

        bool LogicLevel()
        {
            bool answ;
            if(Expresion[p]=='t')
            {
                p+=4;
                answ=true;
            }
            else if(Expresion[p]=='f')
            {
                p += 5;
                answ = false;
            }
            else if(Expresion[p]=='n')
            {
                p+=4;
                answ= !( LogicLevel() );
            }
            else
                answ = ComparationsLevel();
            //TODO: error cases
            while (Expresion[p] == 'a' || Expresion[p] == 'o' || Expresion[p] == 'x')
            {
                switch (Expresion[p])
                {
                    case 'a': p += 3; answ = answ && LogicLevel(); break;
                    case 'o': p += 2; answ = answ || LogicLevel(); break;
                    case 'x': p += 3; answ = answ ^ LogicLevel(); break;
                    default: MessageBox.Show("Error at logic level"); break;
                }
            }
            

            return answ;
        }

        bool ComparationsLevel()
        { 
            AritmeticEvaluator AE=new AritmeticEvaluator();
            string Operator = "";
            float left = AE.bituire(p);
            Operator += Expresion[p];
            if ((Expresion[p+1]=='>')||(Expresion[p+1]=='='))
            { 
                p++;
                Operator += Expresion[p];
            }
            float right = AE.bituire(p);
            //TODO: error cases
            switch (Operator)
            {
                case EQUAL: return left == right;
                case NEQUAL: return left != right;
                case GT: return left > right;
                case GTE: return left >= right;
                case LE: return left < right;
                case LEE: return left <= right;
                default: MessageBox.Show("Error at COmparationsLEvel"); return false;
            }

        }
    }
}

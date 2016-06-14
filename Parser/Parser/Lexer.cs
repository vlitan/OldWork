using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    class Lexer
    {
        
        private HashSet<string> KeyWords=new HashSet<string>();
        private HashSet<string> RobotKeyWords=new HashSet<string>();
        private HashSet<string> OneCharacterLongSymbols=new HashSet<string>();
        private HashSet<string> FirstCharacterLongSymbols = new HashSet<string>();
        private HashSet<string> SecondCharacterLongSymbols = new HashSet<string>();
        private HashSet<string> TwoCharacterLongSymbols = new HashSet<string>();
        private HashSet<string> WhiteSpaces = new HashSet<string>();
        private HashSet<string> Numbers = new HashSet<string>();
        private HashSet<char> Identifier = new HashSet<char>();


        private string[] aux;
        
        private string SourceText;
        private Scaner Scaner;
        private Character C1, C2;
        private Token Tkn=new Token();
        

        public Lexer(string _SourceText)
        {
            //define some rules && keywords

            aux = ("begin end if then for from to do while return write read and or xor").Split(' ');
            AddTo(aux, KeyWords);

            aux = (" Position Explode MoveUp MoveDown MoveLeft MoveRight").Split(' ');
            AddTo(aux, RobotKeyWords);

            aux = (" * + - ; = ( ) & | { } . < >").Split(' ');
            AddTo(aux, OneCharacterLongSymbols);

            aux = ("== <= >= <> != ++ ** -- += -= || <-").Split(' ');
            AddTo(aux, TwoCharacterLongSymbols);

            aux = ("= < > ! + * - | ").Split(' ');
            AddTo(aux, FirstCharacterLongSymbols);

         /*   aux = ("= > + * - | ").Split(' ');
            AddTo(aux, SecondCharacterLongSymbols);*/

            aux = ("\t \n \r \n\r ").Split(' ');
            AddTo(aux, WhiteSpaces);
            WhiteSpaces.Add(" ");


            SourceText = _SourceText;
            Scaner = new Scaner(SourceText);
            C1 = Scaner.GetNextCharacter();
        }

        private void AddTo(string[] Source, HashSet<string> Dest)
        {

            for (int i = 0; i < Source.Length; i++)
                Dest.Add(Source[i]);
        }

        public Token GetNextToken()
        {
            int k = 0;
        x:
            k++;
            if (C1.Char=='☺')
            {
                C1 = Scaner.GetNextCharacter();
                if (C1.Char == '☺')
                    Tkn.Type = "ENDMARK";
                Tkn.Content = ""+C1.Char;
                return Tkn;
            }
            /*
             * Step over comments and whitespaces 
             */

            #region
            while (WhiteSpaces.Contains("" + C1.Char) ||  C1.Char == '?') 
            {
                while (WhiteSpaces.Contains("" + C1.Char))
                {
                    C2 = C1;
                    C1=Scaner.GetNextCharacter();
                }

                while ( C1.Char == '?') // ? commnet content ? <== this is how you comment
                {
                    C1 = Scaner.GetNextCharacter();
                    while ( C1.Char != '?')
                    {
                        if(C1.SourceIndex>Scaner.LastIndex)
                        {
                            Tkn.Type="Error";
                            Tkn.ColumnIndex=C1.ColumnIndex;
                            Tkn.LineIndex=C1.LineIndex;
                            Tkn.Content="The end of code was found before the end of the comment.";
                            return Tkn;
                        }
                        C1 = Scaner.GetNextCharacter();
                    }
                    C1 = Scaner.GetNextCharacter();                            
                }


            }
            #endregion 

            /*
             * Step over comments and whitespaces 
             */


            if (Char.IsLetter(C1.Char)) //Is begining of the identifier or keyword
            {
                Tkn.Content = "";
                Tkn.LineIndex = C1.LineIndex;
                Tkn.ColumnIndex = C1.ColumnIndex;
                Tkn.Type = "Identifier";
                while (Char.IsLetterOrDigit(C1.Char))
                {
                    Tkn.Content += C1.Char;
                    C1 = Scaner.GetNextCharacter();
                }
                if (KeyWords.Contains(Tkn.Content))
                    Tkn.Type = "KeyWord";
                if (RobotKeyWords.Contains(Tkn.Content))
                    Tkn.Type = "RobotKeyWord";
                return Tkn;
            }

            if (Char.IsDigit(C1.Char)) // Is begining of a number
            {
                Tkn.Content = "";
                Tkn.LineIndex = C1.LineIndex;
                Tkn.ColumnIndex = C1.ColumnIndex;
                Tkn.Type = "Number";
                while (Char.IsDigit(C1.Char))
                {
                    Tkn.Content += C1.Char;
                    C1 = Scaner.GetNextCharacter();
                }
                return Tkn;
            }

            //get the simbols
            if (OneCharacterLongSymbols.Contains(""  + C1.Char)||FirstCharacterLongSymbols.Contains(""+C1.Char))
            {
                Tkn.Content = "";
                Tkn.LineIndex = C1.LineIndex;
                Tkn.Type="Symbol";
                Tkn.ColumnIndex = C1.ColumnIndex;
                C2=C1;
                C1=Scaner.GetNextCharacter();
                if(TwoCharacterLongSymbols.Contains(""+C2.Char+C1.Char))
                {
                    Tkn.Content = ""+ C2.Char + C1.Char;
                    C1 = Scaner.GetNextCharacter();
                }
                else if (OneCharacterLongSymbols.Contains("" + C2.Char))
                {
                    Tkn.Content = "" + C2.Char;
                }
                else
                {
                    Tkn.Type = "Error";
                    Tkn.Content = "Unrecognized stuff" + C1.Char;
                }
                return Tkn;
            }

            //Get the strings
            if (C1.Char == '\"')
            {
                Tkn.ColumnIndex = C1.ColumnIndex;
                Tkn.LineIndex = C1.LineIndex;
                Tkn.Type = "String";
                Tkn.Content="";
                C1=Scaner.GetNextCharacter();
                while(C1.Char!='\"')
                {
                    Tkn.Content +=""+C1.Char;
                    C1 = Scaner.GetNextCharacter();
                    if (C1.SourceIndex > Scaner.LastIndex)
                    {
                        Tkn.Type = "Error";
                        Tkn.ColumnIndex = C1.ColumnIndex;
                        Tkn.LineIndex = C1.LineIndex;
                        Tkn.Content = "The end of code was found before the end of the string.";
                        return Tkn;
                    }
                }
                C1 = Scaner.GetNextCharacter();
                return Tkn;
            }

            
           
            /* the main idea with this region is that
            * it`s basicaly all the code above repeated
            * just to process the last value of the SorceText
            */

                
            if(k==1)
                goto x;

            Tkn.Type ="Error";
            Tkn.Content ="Unrecognized stuff"+C1.Char;
            return Tkn;

       }

        private bool IfIn(string S, string[] De)
        {
            for (int i = 0; i < De.Length; i++)
            {
                if (S == De[i])
                    return true;
            }
            return false;
        }

        private bool IfIn(char C, string[] De)
        {
            for (int i = 0; i < De.Length; i++)
            {
                if (C == De[i][0])
                    return true;
            }
            return false;
        }

    }
}
/*
 * begin;
? test text ?
if(43<=12) alfa<-re{}
nard**(MoveUp();+3)
 */
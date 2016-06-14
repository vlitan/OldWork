using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    class Character
    {
        public int LineIndex { set; get; }
        public int ColumnIndex { set; get; }
        public int SourceIndex { get; set; }
        public char Char { set; get; }

        public Character(char _Char, int _LineIndex, int _ColumnIndex, int _SourceIndex)
        {
            this.Char = _Char;
            this.LineIndex = _LineIndex;
            this.ColumnIndex = _ColumnIndex;
            this.SourceIndex = _SourceIndex;
        }

        public override string ToString()
        {
            string str;
            string chr;
            if (this.Char == ' ') chr = " space";
            else if (this.Char == '\t') chr = " tab";
            else if(this.Char=='\r') chr=@" \r-enter";
            else if (this.Char == '\n') chr = @" \n-enter";
            else if (this.Char == '☺') chr = "ENDMARK";
            else chr = "" + this.Char;
            str = "" + this.LineIndex + " " + this.ColumnIndex + " " + this.SourceIndex + " " + chr;
            return str;

        }
    }
}

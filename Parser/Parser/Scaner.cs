using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    class Scaner
    {
        private string SourceText;
        public int SourceIndex = -1;
        private int ColumnIndex=-1;
        private int LineIndex=0;
        public int LastIndex;

        public Scaner(string _SourceText)
        {
            SourceText = _SourceText;
            LastIndex = SourceText.Length;
        }

        public Character GetNextCharacter()
        {
            SourceIndex++;
            if( (SourceIndex<LastIndex)&&(SourceIndex>0)&&(SourceText[SourceIndex - 1] == '\n'))
            {
                ColumnIndex = -1;
                LineIndex++;
            }

            ColumnIndex++;
            Character c;
            if(SourceIndex>=LastIndex)
                    c = new Character('☺', LineIndex, ColumnIndex, SourceIndex);
            else
                    c = new Character(SourceText[SourceIndex], LineIndex, ColumnIndex, SourceIndex);
            return c;
        }
    }
}

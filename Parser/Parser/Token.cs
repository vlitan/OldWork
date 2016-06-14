using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    class Token
    {
        public int ColumnIndex{get;set;}
        public int LineIndex { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }

        public Token()
        {
            Content = "";
        }
        
        public Token(string _Content)
        {
            Content = _Content;
        }

        public override string ToString()
        {
            return this.LineIndex + " " + this.ColumnIndex + " " + this.Type + " " + this.Content;
        }
    }
}

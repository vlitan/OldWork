using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    class ParserC
    {

        #region
        //KeyWords
        public const string MAINBEGIN = "MainBegin";
        public const string MAINEND = "MainEnd";
        public const string BEGIN = "begin";
        public const string END = "end";
        public const string FOR = "for";
        public const string FROM = "from";
        public const string TO = "to";
        public const string DO = "do";
        public const string IF = "if";
        public const string THEN = "then";
        public const string WHILE = "while";
        public const string REPEAT = "repeat";
        public const string UNTIL = "until";

        //Operators
        public const string ASSIGMENT = "<-";
        public const string PLUS = "+";
        public const string MINUS = "-";
        public const string MULTIPLY = "*";
        public const string SLASH = "/";
        public const string EQUAL = "=";
        public const string NEQUAL = "<>";
        public const string GT = ">";
        public const string GTE = ">=";
        public const string LE = "<";
        public const string LEE = "<=";
        public const string INTEGER = "integer";
        public const string SEMICOL = ";";
        public const string QUOTES = "\"";
        public const string OPENSQB = "[";
        public const string CLOSESQB = "]";
        public const string OPENRDB = "(";
        public const string CLOSERDB = ")";
        public const string POINT = ".";
        public const string COMMA = ",";

        public const string IDENTIFIER = "Identifier";
        public const string NUMBER = "Number";
        #endregion

        Lexer Lxr;
        public ParserC(string SourceText)
        {
            Lxr = new Lexer(SourceText);

        }
    }
}

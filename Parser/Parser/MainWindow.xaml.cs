using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Parser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void TestScanner()
        {
            TextRange txtrng = new TextRange(Input.Document.ContentStart, Input.Document.ContentEnd);
            string text = txtrng.Text;
            Scaner Scnr = new Scaner(text);
            bool ok = true;
            string outp = "line  column  text  char \n";
            while (ok)
            {  
                Character c = Scnr.GetNextCharacter();
                outp += c.ToString()+"\n";  
                if (c.Char == '☺')
                    ok = false;
                if (Scnr.SourceIndex == 5)
                    MessageBox.Show(""+c);
                
            }
            Output.Text = outp;

        }

        private void TestLexer()
        {
            TextRange txtrng = new TextRange(Input.Document.ContentStart, Input.Document.ContentEnd);
            string text = txtrng.Text;
            Lexer Lxr = new Lexer(text);
            bool ok = true;
            string outp = "line  column  Type Content \n";
            while (ok)
            {
                Token Tkn = Lxr.GetNextToken();
                outp += Tkn.ToString() + "\n";
                if (Tkn.Type=="ENDMARK")
                    ok = false;
            }
            Output.Text = outp;

        }

        private void TestAritmeticEvaluator()
        {
            TextRange txtrng = new TextRange(Input.Document.ContentStart, Input.Document.ContentEnd);
            string text = txtrng.Text;
            AritmeticEvaluator AE=new AritmeticEvaluator();
            Output.Text = "" + AE.Evaluate(text.ToCharArray()); 
        }

        private void TestBooleanEvaluator()
        {
            TextRange txtrng = new TextRange(Input.Document.ContentStart, Input.Document.ContentEnd);
            string text = txtrng.Text;
            BooleanEvaluator BE = new BooleanEvaluator();
            Output.Text =(BE.Evaluate(text.ToCharArray()) ) ? "true":"false";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TestBooleanEvaluator();
        }
    }
}
/*
 
 
 */
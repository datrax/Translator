using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class Parser
    {
        
        List<Skaner.Lexems> Lexems;
        Skaner.Lexems CurrentLexem;
        int count;
        public Parser(List<Skaner.Lexems> Lexems)
        {
            this.Lexems = new List<Skaner.Lexems>(Lexems);
            count = 0;
        }
        Skaner.Lexems GetNextLexem()
        {
            if (count > Lexems.Count)
                return null;
            else
            {
                CurrentLexem = Lexems[count++];      
                return CurrentLexem;
            }
        }
        public bool check(out int c, out string text)
        {

            if (GetNextLexem().LexemCode == 1)
                if (GetNextLexem().LexemCode == 30)
                    if (GetNextLexem().LexemCode == 50)
                        if (GetNextLexem().LexemCode == 31)
                        { }
                        else
                        {
                            c = 1;
                            text = "make sure you made new line";
                            return false;
                        }
                    else
                    {
                        c = 1;
                        text = "program name expected";
                        return false;
                    }

                else
                {
                    c = 1;
                    text = "\":\" expected";
                    return false;
                }

            else
            {
                c = 1;
                text = "\"init\" expected";
                return false;

            }
            c = 0;
            text = "Success";
            return true;

        }


    }
}

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
        int count, last;
        public Parser(List<Skaner.Lexems> Lexems)
        {
            this.Lexems = new List<Skaner.Lexems>(Lexems);
            count = 0;
            last = 0;
        }
        public Parser(Parser t)
        {
            Lexems = t.Lexems;
            CurrentLexem = t.CurrentLexem;
            count = t.count;
        }
        List<Skaner.Lexems> GetLine()
        {
            List<Skaner.Lexems> t = new List<Skaner.Lexems>();
            while (GetNextLexem().LexemCode != 31)
                t.Add(CurrentLexem);
            return t;
        }
        private void SavePos()
        {
            last = count;
        }
        private void LoadLastPos()
        {
            count = last;
        }

        Skaner.Lexems TryNextLexem()
        {
            if (count > Lexems.Count)
                return null;
            else return Lexems[count];
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
                            if (GetNextLexem().LexemCode == 26)
                                if (GetNextLexem().LexemCode == 31)
                                    if (IsElemList(out c, out text))
                                    { }
                                    else
                                    {
                                        return false;
                                    }
                                else
                                {
                                    c = 2;
                                    text = "make sure you made new line";
                                    return false;
                                }
                            else
                            {
                                c = 2;
                                text = "\"/\" expected";
                                return false;
                            }
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

        private bool IsElemList(out int c, out string text)
        {
            IsElem(out c, out text);
           System.Diagnostics.Debug.WriteLine(GetNextLexem().LexemCode);
            if (IsElem(out c, out text))
            {
                
                c = 0;
                text = "Success";
                return true;
            }
            else return false;

        }

        private bool IsElem(out int c, out string text)
        {

            if ( IsLabel(out c, out text)||IsOgol(out  c, out  text) || IsOper(out c, out text) )
            {
                c = 0;
                text = "Success";
                return true;
            }
            else return false;
        }

        private bool IsLabel(out int c, out string text)
        {

            SavePos();
            if (GetNextLexem().type == 3 && GetNextLexem().LexemCode == 30)
            {

                c = 0;
                text = "Success";
                return true;
            }
            else
            {
                LoadLastPos();
                c = CurrentLexem.RowNumber;
                text = "label expected";
                return false;
            };
        }


        private bool IsOper(out int c, out string text)
        {
            
            if (IsPrysv(out c, out text) || IsInput(out c, out text) || IsOutput(out c, out text) || IsGoto(out c, out text)||IsCondition(out c,out text))
            {
                c = 0;
                text = "Success";
                return true;
            }
            else
            {
                
               // c = CurrentLexem.RowNumber;
                //text = "Wrong syntaxis";
               
                return false;
            };
        }

        private bool IsGoto(out int c, out string text)
        {
            SavePos();
            if (GetNextLexem().LexemCode == 12 && GetNextLexem().type == 5)
            {
                c = 0;
                text = "Success";
                return true;
            }
            else
            {
                c = CurrentLexem.RowNumber;
                text = "Variable name expected";
                LoadLastPos();
                return false;
            };
        }

        private bool IsPrysv(out int c, out string text)
        {
            SavePos();
            if (GetNextLexem().LexemCode == 50 && GetNextLexem().LexemCode == 13&&IsArythm(out  c, out  text))
            {
                c = 0;
                text = "Success";
                return true;
            }
            else
            {
                c = CurrentLexem.RowNumber;
                text = "Make sure you use syntaxis x=t";
                LoadLastPos();
                return false;
            };
        }

        private bool IsArythm(out int c, out string text)
        {
            c = 0;
            text = "Success";
            return true;
        }

        private bool IsOutput(out int c, out string text)
        {
            SavePos();
           
            if (GetNextLexem().LexemCode == 4 && GetNextLexem().LexemCode==28&&IsSpysId(out c, out text) && GetNextLexem().LexemCode==29)
            {
                c = 0;
                text = "Success";
                return true;
            }
            else
            {
                c = CurrentLexem.RowNumber;
                text = "Make sure you use syntaxis scan(a,..h)";
                LoadLastPos();
                return false;
            };
        }

        private bool IsInput(out int c, out string text)
        {
            SavePos();

            if (GetNextLexem().LexemCode == 5 && GetNextLexem().LexemCode == 28 && IsSpysId(out c, out text) && GetNextLexem().LexemCode == 29)
            {
                c = 0;
                text = "Success";
                return true;
            }
            else
            {
                c = CurrentLexem.RowNumber;
                text = "Make sure you use syntaxis scan(a,..h)";
                LoadLastPos();
                return false;
            };
        }
        private bool IsCondition(out int c, out string text)
        {
            SavePos();

            if (GetNextLexem().LexemCode == 8 && GetNextLexem().LexemCode == 28 && IsLogic(out c, out text) && GetNextLexem().LexemCode == 29&&
                GetNextLexem().LexemCode == 9&&GetNextLexem().LexemCode==31&&IsElem(out c,out text)&&
                GetNextLexem().LexemCode == 10&&GetNextLexem().LexemCode==31&&IsElem(out c,out text)&&GetNextLexem().LexemNumber==11)
            {
                c = 0;
                text = "Success";
                return true;
            }
            else
            {
                c = CurrentLexem.RowNumber;
                text = "Make sure you use syntaxis if";
                LoadLastPos();
                return false;
            };
        }

        private bool IsLogic(out int c, out string text)
        {
            c = 0;
            text = "Success";
            return true;
        }

        private bool IsOgol(out int c, out string text)
        {
            c = CurrentLexem.RowNumber;
            text = "Variables expected";
            SavePos();
            if ((GetNextLexem().LexemCode == 2 || CurrentLexem.LexemCode == 3) && IsSpysId(out c, out text))
            {
                c = 0;
                text = "Success";
                return true;
            }
            else
            {
                LoadLastPos();
               
                return false;
            };
        }

        private bool IsSpysId(out int c, out string text)
        {

            if (GetNextLexem().LexemCode == 50)
            {
                while (TryNextLexem().LexemCode ==32)
                {
                
                    if (!(GetNextLexem().LexemCode == 32 && GetNextLexem().LexemCode == 50))

                    {
                        c = CurrentLexem.RowNumber;
                        text = "Variable name expected";
                        return false;
                    }
                }
                c = 0;
                text = "Success";
                return true;
            }
            else
            {
                c = CurrentLexem.RowNumber;
                text = "Variable name expected";
                return false;

            }

        }

    }
}

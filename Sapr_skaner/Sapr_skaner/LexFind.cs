using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Sapr_skaner
{
    class LexFind
    {
        int RowNumber { get; set; }
        int LexemNumber { get; set; }
        int LexemCode { get; set; }
        int IdCode { get; set; }
        int ConstCode { get; set; }
        int type;

        List<string> TypeInt;
        List<string> TypeReal;

        StreamReader re;
        char[] special_lex_collect = {'-','+','/','\\',':','(',')',',' };
        string[] lex_collect = { "init", "int", "real", "print", "scan", "while", "do", "if", "then", "else", "endif", "goto", "=", "!=", "<=", ">=", "<", ">", "==", "AND", "OR", "NOT", "+", "-", "*", "/", "\\", "(", ")", ":", "¶", "," };

        public LexFind()
        {
            RowNumber = 1;
            LexemNumber = 0;
            LexemCode = 0;
            IdCode = 0;
            ConstCode = 0;
            type = 0;
            TypeInt = new List<string>();
            TypeReal = new List<string>();
        }
        public LexFind(StreamReader re)
            : this()
        {

            this.re = re;
        }
        public bool IsSpecialLex(char s)
        {
            for (int i = 0; i < special_lex_collect.Length; i++)
                if (s==special_lex_collect[i]) return true;
            return false;
        }
        public Int32 GetCode(String Lex)
        {
            int code = -1;
            if (Lex.Equals("_Error")) return -1;
            for (int i = 0; i < lex_collect.Length; i++)
            {
                if (Lex.Equals(lex_collect[i]))
                {
                    code = i + 1;
                    break;
                }
            }
            if (code == -1)
                if (Char.IsDigit(Lex[0]) || Lex[0].Equals('-'))
                    code = 51;
                else
                    code = 50;
            return code;
        }
        public string ChooseLexema()
        {
            string lex = "";
            bool LexNotFound = true;
            int mode = 1;
            while (re.Peek() == ' ')//пропуск пробелов
            {
                re.Read();
            };
            while (LexNotFound)
            {
                switch (mode)
                {
                    case 1:
                        {
                            char s = Convert.ToChar(re.Read());
                            lex += s;
                            if (Char.IsLetter(s)) mode = 2;
                            else if (s == '.') mode = 9;
                            else if (Char.IsDigit(s)) mode = 4;
                            else if (s == '!') mode = 10;
                            else if (s == '=') mode = 11;
                            else if (s == '>') mode = 12;
                            else if (s == '<') mode = 13;
                            else if (s == 13) { mode = 14; }//конец строки
                            else if (IsSpecialLex(s)) { lex = s.ToString(); LexNotFound = false; }
                            else{lex = "_Error"; LexNotFound = false;}
                        } break;
                    case 2:
                        {
                            if (Char.IsLetter(Convert.ToChar(re.Peek())) || Char.IsDigit(Convert.ToChar(re.Peek())))
                                lex += Convert.ToChar(re.Read());
                            else if (GetCode(lex) < 52)
                            {
                                LexNotFound = false;
                            }
                                else
                            {
                                lex = "_Error";
                                LexNotFound = false;
                            }
                           
                        } break;
                    case 3:
                        {
                            if (Char.IsDigit(Convert.ToChar(re.Peek())))
                            {
                                lex += Convert.ToChar(re.Read());
                            }
                            else if (re.Peek().Equals('E'))
                            {
                                lex += Convert.ToChar(re.Read());
                                mode = 6;
                            }
                            else
                            {
                                LexNotFound = false;
                            }
                        } break;
                    case 4:
                        {

                            if (Char.IsDigit(Convert.ToChar(re.Peek())))
                            {
                                lex += Convert.ToChar(re.Read());
                            }

                            else if (re.Peek().Equals('.'))
                            {
                                lex += Convert.ToChar(re.Read());
                                mode = 5;
                            }

                            else if (re.Peek().Equals('E'))
                            {
                                lex += Convert.ToChar(re.Read());
                                mode = 6;
                            }
                            else LexNotFound = false;
                        }
                        break;
                    case 5:
                        {
                            if (Char.IsDigit(Convert.ToChar(re.Peek())))
                            {
                                lex += Convert.ToChar(re.Read());
                                mode = 3;
                            }
                          
                            else
                            {
                                lex = "_Error";
                                LexNotFound = false;
                            }
                        } break;
                    case 6:
                        {
                            if (Char.IsDigit(Convert.ToChar(re.Peek())))
                            {
                                lex += Convert.ToChar(re.Read());
                                mode = 8;
                            }
                            else if (re.Peek().Equals('-'))
                            {
                                lex += Convert.ToChar(re.Read());
                                mode = 7;
                            }
                            else
                            {
                                lex = "_Error";
                                LexNotFound = false;
                            }
                        } break;
                    case 7:
                        {
                            if (Char.IsDigit(Convert.ToChar(re.Peek())))
                            {
                                lex += Convert.ToChar(re.Read());
                                mode = 8;
                            }
                            else
                            {
                                lex = "_Error";
                                LexNotFound = false;
                            }
                        } break;
                    case 8:
                        {
                            if (Char.IsDigit(Convert.ToChar(re.Peek())))
                                lex += Convert.ToChar(re.Read());
                            else
                                LexNotFound = false;
                        } break;
                    case 9:
                        {
                            if (Char.IsDigit(Convert.ToChar(re.Peek())))
                            {

                                mode = 3;
                            }
                            else
                            {
                                lex = "_Error";
                                LexNotFound = false;
                            }
                        } break;
                    case 10:
                        {
                            if (re.Peek() == ' ')
                            {
                                lex = "!=";
                                re.Read();
                            }
                            else lex = "_Error";
                        }
                        break;
                    case 11:
                        {
                            if (re.Peek() == '=')
                            {
                                lex = "==";
                                re.Read();
                                LexNotFound = false;
                            }
                            else { lex = "="; LexNotFound = false; }
                        }
                        break;
                    case 12:
                        {
                            if (re.Peek() == '=')
                            {
                                lex = ">=";
                                re.Read();
                                LexNotFound = false;
                            }
                            else
                            {
                                lex = ">";
                                LexNotFound = false;
                            }
                        }
                        break;
                    case 13:
                        {
                            if (re.Peek() == '=')
                            {
                                lex = "<=";
                                re.Read();
                                LexNotFound = false;
                            }
                            else
                            {
                                lex = "<";
                                LexNotFound = false;
                            }
                        }
                        break;
                    case 14:
                        {
                            if (re.Peek() == 10)
                            {
                                lex = "¶";
                                re.Read();
                                LexNotFound = false;

                            }
                            else { lex = "_Error"; LexNotFound = false; }
                        } break;
                    default: lex = "_Error"; break;
                }
            }
            return lex;
        }
        public bool next()
        {
            if (re.Peek() > -1) return true;
            else return false;
        }

        public Lexems NextLex()
        {
            //1-int 2-real 3-label 4-name of program
            int IdConstCode = 0;
            string t = ChooseLexema();
            LexemCode = GetCode(t);
            LexemNumber++;
            if (LexemCode == 1) type = 4;
            if (LexemCode == 2) type = 1;
            if (LexemCode == 3) type = 2;
            if (LexemCode == 12) type = 3;
            if (LexemCode == 31) type = 0;
            Lexems ans = new Lexems(LexemNumber, RowNumber, t, LexemCode, IdConstCode, type);
            if (LexemCode == 31) RowNumber++;
            return ans;
        }
    }
}
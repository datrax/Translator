using System.IO;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Skaner;

namespace RPN
{
    public class PolishNotation
    {
        Dictionary<string,int>labels=new Dictionary<string, int>();
        List<Skaner.Lexems> Lexems;
        Dictionary<string, int> priority = new Dictionary<string, int>();
        int count;
        Skaner.Lexems CurrentLexem;
        List<String> rpn = new List<string>();
        List<string> stack = new List<string>();
        Output table = new Output();
        private Skaner.Lexems TryNextLexem()
        {
            if (count >= Lexems.Count)
            { return new Skaner.Lexems() { LexemCode = -1 }; }
            else return Lexems[count];
        }
        private Skaner.Lexems GetNextLexem()
        {
            if (count >= Lexems.Count)
                return null;
            else
            {
                CurrentLexem = Lexems[count++];
                return CurrentLexem;
            }
        }

        public PolishNotation(List<Skaner.Lexems> Lexems)
        {
            this.Lexems = new List<Skaner.Lexems>(Lexems);
            List<Skaner.Lexems> l = new List<Skaner.Lexems>();
            bool copy = false;
            foreach (Lexems t in Lexems)
            {
                if (copy)
                    l.Add(t);
                if (t.LexemCode == 31)
                    copy = true;
                /*  if (t.LexemCode == 31)
                      copy = false;*/
            }
            //  l.RemoveRange(l.Count - 2, 2);
            this.Lexems = l;

            count = 0;

            priority.Add("(", 0);
            priority.Add("if", 0);
            priority.Add("while", 0);
            priority.Add("do", 1);
            priority.Add("then", 1);
            priority.Add("else", 1);
            priority.Add("endif", 1);
            priority.Add(")", 1);
            priority.Add("=", 2);
            priority.Add("OR", 3);
            priority.Add("AND", 4);
            priority.Add("<", 6);
            priority.Add(">", 6);
            priority.Add("==", 6);
            priority.Add("!=", 6);
            priority.Add("<=", 6);
            priority.Add(">=", 6);
            priority.Add("+", 7);
            priority.Add("-", 7);
            priority.Add("*", 8);
            priority.Add("/", 8);
            priority.Add("@", 8);

        }

        private void MakeRpn()
        {
            while (true)
            {

                switch (TryNextLexem().LexemCode)
                {
                    case 4:
                        Output(); break;
                    case 5: Input();
                        break;
                    case 6:
                    {
                        string label = "m" + labels.Count.ToString();
                        labels.Add(label, count);
                        stack.Add("while");
                        stack.Add(label);
                        rpn.Add(label + ":");
                        table.labelgrid.Rows.Add(label, rpn.Count + 1);
                        GetNextLexem();
                    }break;
                    case 7:
                    {
                     

                        while (stack.Count > 0 && (stack[stack.Count - 1])[0] != 'm' && priority[TryNextLexem().LexemName] <= priority[stack[stack.Count - 1]])
                        {
                            rpn.Add(stack[stack.Count - 1]);
                            stack.RemoveAt(stack.Count - 1);

                        }
                        string label = "m" + labels.Count.ToString();
                        labels.Add(label, count);
                        rpn.Add(label);
                        rpn.Add("УПХ");
                        GetNextLexem();

                    }break;
                    case 8:
                        {
                            stack.Add(GetNextLexem().LexemName);
                            break;
                        }
                    case 9:
                    {
                        while (stack.Count > 0 && (stack[stack.Count - 1])[0] != 'm' && priority[TryNextLexem().LexemName] <= priority[stack[stack.Count - 1]])
                        {
                            rpn.Add(stack[stack.Count - 1]);                            
                            stack.RemoveAt(stack.Count - 1);

                        }
                        string label = "m"+labels.Count.ToString();
                        labels.Add(label,count);
                        stack.Add(label);
                        rpn.Add(label);
                        rpn.Add("УПХ");
                        GetNextLexem();
                    }
                    break;
                    case 10:
                    {
                        while (stack.Count > 0 && (stack[stack.Count - 1])[0]!='m'&&priority[TryNextLexem().LexemName] <= priority[stack[stack.Count - 1]])
                        {
                            rpn.Add(stack[stack.Count - 1]);
                            stack.RemoveAt(stack.Count - 1);

                        }
                        string label = "m" + labels.Count.ToString();
                        labels.Add(label, count);
                        stack.Add(label);
                        rpn.Add(label);
                        rpn.Add("БП");
                        rpn.Add(stack[stack.Count-2]+":");
                        table.labelgrid.Rows.Add(stack[stack.Count - 2], rpn.Count + 1);
                        GetNextLexem();

                    }break;
                    case 11:
                    {
                        while (stack.Count > 0 && (stack[stack.Count - 1])[0] != 'm' && priority[TryNextLexem().LexemName] <= priority[stack[stack.Count - 1]])
                        {
                            rpn.Add(stack[stack.Count - 1]);
                            stack.RemoveAt(stack.Count - 1);
                           
                        }
                        rpn.Add(stack[stack.Count - 1] + ":");
                        table.labelgrid.Rows.Add(stack[stack.Count - 1], rpn.Count+1);
                        stack.RemoveRange((stack.Count - 3), 3);
                        GetNextLexem();
                    }break;
                    case 12:
                    {
                        GetNextLexem();
                        rpn.Add(GetNextLexem().LexemName); 
                        rpn.Add("БП");
                    }
                        break;
                    case 51:
                    case 50:
                    {
                        if (TryNextLexem().type == 3)
                        {
                         
                            rpn.Add(TryNextLexem().LexemName+":");
                            table.labelgrid.Rows.Add(GetNextLexem().LexemName, rpn.Count + 1);
                            GetNextLexem();
                        }
                        else
                        rpn.Add(GetNextLexem().LexemName);
                    }
                        break;
                    case 24:
                        if (count == 0)
                            stack.Add("@");
                        else
                        {
                            if (Lexems[count - 1].LexemCode == 50 || Lexems[count - 1].LexemCode == 51 || Lexems[count - 1].LexemCode == 29)
                                stack.Add("-");
                            else
                                stack.Add("@");

                        }
                        GetNextLexem();
                        break;
                    case 28:
                        stack.Add(GetNextLexem().LexemName);
                        break;
                    case 29:
                        {
                            int t = stack.Count - 1;
                            while (stack[t] != "(")
                            {
                                rpn.Add(stack[t]);
                                if (stack[t] == "(")
                                    throw new Exceptions.MyException(") expected");
                                stack.RemoveAt(t);
                                t--;
                                if (t < 0) throw new Exceptions.MyException("( expected");
                            }
                            stack.RemoveAt(stack.Count - 1);
                            GetNextLexem();
                        }
                        break;
                    case 31:
                    {
                        for (int i = stack.Count - 1; stack.Count>0&&stack[i][0]!='m'; i--)
                        {
                            rpn.Add(stack[i]);
                            stack.RemoveAt(i);
                
                        }
                                         
                        GetNextLexem();
                        break;
                        
                    }
                    case 33:
                        GetNextLexem();
                        break;
                    case 34:
                    {
                        if (count == Lexems.Count-1)
                        {
                            table.Show();
                            return;
                        }
                        rpn.Add(stack[stack.Count-1]);
                        rpn.Add("БП");
                        string num=(stack[stack.Count - 1]);
                        num=num.Substring(1, num.Length - 1);
                        int number = int.Parse(num);
                        rpn.Add("m"+(number+1).ToString()+":");
                        table.labelgrid.Rows.Add("m" + (number + 1).ToString(), rpn.Count + 1);
                        stack.RemoveRange((stack.Count - 2), 2);
                        GetNextLexem();
                        
                        
                        break;
                    }
                    default:

                        
                        if (stack.Count == 0)
                        {
                            stack.Add(GetNextLexem().LexemName);

                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine(TryNextLexem().LexemName[0]);

                            while (stack.Count > 0 && (stack[stack.Count - 1])[0] != 'm' && priority[TryNextLexem().LexemName] <= priority[stack[stack.Count - 1]])
                            {
                                rpn.Add(stack[stack.Count - 1]);
                                if (stack[stack.Count - 1] == "(")
                                    throw new Exceptions.MyException(") expected");
                                stack.RemoveAt(stack.Count - 1);

                            }

                            stack.Add(GetNextLexem().LexemName);

                        }
                        break;
                }

                FillTable();
            }

        }




        private void Input()
        {
            GetNextLexem();
            GetNextLexem();
            while (TryNextLexem().LexemCode != 31)
            {
                if (TryNextLexem().LexemCode == 51 || TryNextLexem().LexemCode == 50)
                {
                    rpn.Add(GetNextLexem().LexemName);
                }
                else
                {
                    rpn.Add("SC");
                    GetNextLexem();
                }
                FillTable();
            }
            GetNextLexem();
        }
        private void Output()
        {
            GetNextLexem();
            GetNextLexem();
            while (TryNextLexem().LexemCode != 31)
            {
                if (TryNextLexem().LexemCode == 51 || TryNextLexem().LexemCode == 50)
                {
                    rpn.Add(GetNextLexem().LexemName);
                }
                else
                {
                    rpn.Add("PR");
                    GetNextLexem();
                }
                FillTable();
            }
            GetNextLexem();
        }
        public void build()
        {
         
            MakeRpn();
            MessageBox.Show(rpn.Aggregate("", (current, t) => current + (" " + t)));
          

        }

        private void FillTable()
        {

            string str = "";
            for (int i = 0; i < stack.Count; i++)
            {
                str += " " + stack.ToArray()[i];
            }
            string str1 = "";
            for (int i = 0; i < rpn.Count; i++)
            {
                str1 += " " + rpn[i];
            }
            table.dataGridView1.Rows.Add(CurrentLexem.LexemName, str, str1);
        }
    }
}

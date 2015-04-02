using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interpreters
{
    public class Interpreter
    {
        private List<string> rpn = new List<string>();
        private DataGridView idValues;
        private DataGridView labelValues;
        private TextBox output;
        private Stack<string> stack = new Stack<string>();
        private Form form;
        
        public Interpreter(List<string> rpn, DataGridView idValues, DataGridView labelValues, TextBox output,Form form)
        {
            this.rpn = rpn;
            this.idValues = idValues;
            this.labelValues = labelValues;
            this.output = output;
            this.form = form;
        }


        double GetLabelValue(string name)
        {
            double answer = 0;
            for (int i = 0; i < labelValues.Rows.Count - 1; i++)
                if (labelValues[0, i].Value.ToString().Equals(name))
                {
                    try
                    {

                        return answer = Convert.ToInt32(labelValues[1, i].Value);


                    }
                    catch (NullReferenceException)
                    {
                        throw new Exceptions.MyException("label " + name + " isn't initialized");
                    }

                }
            throw new Exceptions.MyException("label " + name + " isn't initialized");
        }
        double GetVarValue(string name)
        {
            double answer = 0;
            for (int i = 0; i < idValues.Rows.Count - 1; i++)
                if (idValues[0, i].Value.ToString().Equals(name))
                {
                    try
                    {
                        if (GetVarType(name))
                            answer = Convert.ToInt32(idValues[3, i].Value);
                        else
                        {
                            answer = Convert.ToDouble(idValues[3, i].Value);    
                        }
                        return answer;


                    }
                    catch (NullReferenceException)
                    {
                        throw new Exceptions.MyException("Variable " + name + " isn't initialized");
                    }

                }
            throw new Exceptions.MyException("Variable " + name + " isn't initialized");
        }
        void SetVarValue(string name, double value)
        {
            for (int i = 0; i < idValues.Rows.Count - 1; i++)
                if (idValues[0, i].Value.ToString().Equals(name))
                {
                    idValues[3, i].Value = value.ToString();
                }

        }
        bool GetVarType(string name)
        {
            for (int i = 0; i < idValues.Rows.Count - 1; i++)
                if (idValues[0, i].Value.ToString().Equals(name))
                {
                    if (idValues[2, i].Value.ToString() == "int") return true;
                    return false;
                }
            throw new Exceptions.MyException("Variable vasn't found initialized");
        }
        public void Execute()
        {


            for (int i = 0; i < rpn.Count; i++)
            {
                switch (rpn[i])
                {
                    case "+":
                        {
                            string sval1 = stack.Pop();
                            string sval2 = stack.Pop();
                            double val2, val1;
                            if (Char.IsDigit(sval1[0]) || sval1[0] == '-')
                                val1 = Double.Parse(sval1);
                            else
                                val1 = GetVarValue(sval1);

                            if (Char.IsDigit(sval2[0]) || sval2[0] == '-')
                                val2 = Double.Parse(sval2);
                            else
                                val2 = GetVarValue(sval2);

                            stack.Push((val1 + val2).ToString());


                        } break;
                    case "-":
                        {

                            string sval1 = stack.Pop();
                            string sval2 = stack.Pop();
                            double val2, val1;
                            if (Char.IsDigit(sval1[0]) || sval1[0] == '-')
                                val1 = Double.Parse(sval1);
                            else
                                val1 = GetVarValue(sval1);

                            if (Char.IsDigit(sval2[0]) || sval2[0] == '-')
                                val2 = Double.Parse(sval2);
                            else
                                val2 = GetVarValue(sval2);

                            stack.Push((val2 - val1).ToString());
                        } break;
                    case "*":
                        {

                            string sval1 = stack.Pop();
                            string sval2 = stack.Pop();
                            double val2, val1;
                            if (Char.IsDigit(sval1[0]) || sval1[0] == '-')
                                val1 = Double.Parse(sval1);
                            else
                                val1 = GetVarValue(sval1);

                            if (Char.IsDigit(sval2[0]) || sval2[0] == '-')
                                val2 = Double.Parse(sval2);
                            else
                                val2 = GetVarValue(sval2);

                            stack.Push((val2 * val1).ToString());

                        } break;
                    case "/":
                        {

                            string sval1 = stack.Pop();
                            string sval2 = stack.Pop();
                            double val2, val1;
                            if (Char.IsDigit(sval1[0]) || sval1[0] == '-')
                                val1 = Double.Parse(sval1);
                            else
                                val1 = GetVarValue(sval1);

                            if (Char.IsDigit(sval2[0]) || sval2[0] == '-')
                                val2 = Double.Parse(sval2);
                            else
                                val2 = GetVarValue(sval2);

                            stack.Push((val2 / val1).ToString());
                        } break;
                    case "@":
                    {
                        string sval1 = stack.Pop();

                        double val1;
                        if (Char.IsDigit(sval1[0]) || sval1[0] == '-')
                            val1 = Double.Parse(sval1);
                        else
                            val1 = GetVarValue(sval1);
                        stack.Push((-1* val1).ToString());

                    }break;
                    case "=":
                        {
                            string val1 = stack.Pop();
                            string val = stack.Pop();
                            double val2;
                            if (Char.IsDigit(val1[0]) || val1[0] == '-')
                                val2 = Double.Parse(val1);
                            else
                                val2 = GetVarValue(val1);
                            if (GetVarType(val))
                                SetVarValue(val, (int)val2);
                            else
                                SetVarValue(val, val2);

                        } break;
                    case "PR":
                    {
                        form.Invoke(new ThreadStart(delegate
                        {
                            output.Text += GetVarValue(stack.Pop()) + Environment.NewLine;
                            output.SelectionStart = output.Text.Length;
                        }));
                      
                    }break;
                    case "SC":
                    {
                        int size=0,k;
                        form.Invoke(new ThreadStart(delegate
                        {
                            size = output.TextLength;
                           
                        }));
                        k = size;
                        while (k - size == 0)
                        {
                            System.Threading.Thread.Sleep(40);
                            form.Invoke(new ThreadStart(delegate
                            {
                                k = output.TextLength;

                            }));
                        }
                        string str = output.Text;
                        string buf = "";
                        for (int j = str.Length-1; str[j] != 13; j--)
                        {
                            buf += str[j];
                        }
                        string reversebuf="";                       
                        for (int j = buf.Length-1;j>=0 ;j--)
                        {
                            reversebuf += buf[j];
                        }
                        buf = reversebuf;
                        string val = stack.Pop();
                        if(GetVarType(val))
                            SetVarValue(val,(int)Double.Parse(buf));
                        else
                            SetVarValue(val, Double.Parse(buf));

                        form.Invoke(new ThreadStart(delegate
                        {
                            output.Text += Environment.NewLine;
                            output.SelectionStart = output.Text.Length;
                        }));
                    }break;
                    case "БП":
                    {
                        i=(int)GetLabelValue(stack.Pop())-2;
                    }break;
                    case "<":
                    {
                        string sval1 = stack.Pop();
                        string sval2 = stack.Pop();
                        double val2, val1;
                        if (Char.IsDigit(sval1[0]) || sval1[0] == '-')
                            val1 = Double.Parse(sval1);
                        else
                            val1 = GetVarValue(sval1);

                        if (Char.IsDigit(sval2[0]) || sval2[0] == '-')
                            val2 = Double.Parse(sval2);
                        else
                            val2 = GetVarValue(sval2);
                        stack.Push((val2 < val1).ToString());
                    }
                        break;
                    case ">":
                        {
                            string sval1 = stack.Pop();
                            string sval2 = stack.Pop();
                            double val2, val1;
                            if (Char.IsDigit(sval1[0]) || sval1[0] == '-')
                                val1 = Double.Parse(sval1);
                            else
                                val1 = GetVarValue(sval1);

                            if (Char.IsDigit(sval2[0]) || sval2[0] == '-')
                                val2 = Double.Parse(sval2);
                            else
                                val2 = GetVarValue(sval2);
                            stack.Push((val2 > val1).ToString());
                        }
                        break;
                    case "==":
                        {
                            string sval1 = stack.Pop();
                            string sval2 = stack.Pop();
                            double val2, val1;
                            if (Char.IsDigit(sval1[0]) || sval1[0] == '-')
                                val1 = Double.Parse(sval1);
                            else
                                val1 = GetVarValue(sval1);

                            if (Char.IsDigit(sval2[0]) || sval2[0] == '-')
                                val2 = Double.Parse(sval2);
                            else
                                val2 = GetVarValue(sval2);
                            stack.Push((val2 == val1).ToString());
                        }
                        break;
                    case "<=":
                        {
                            string sval1 = stack.Pop();
                            string sval2 = stack.Pop();
                            double val2, val1;
                            if (Char.IsDigit(sval1[0]) || sval1[0] == '-')
                                val1 = Double.Parse(sval1);
                            else
                                val1 = GetVarValue(sval1);

                            if (Char.IsDigit(sval2[0]) || sval2[0] == '-')
                                val2 = Double.Parse(sval2);
                            else
                                val2 = GetVarValue(sval2);
                            stack.Push((val2 <= val1).ToString());
                        }
                        break;
                    case ">=":
                        {
                            string sval1 = stack.Pop();
                            string sval2 = stack.Pop();
                            double val2, val1;
                            if (Char.IsDigit(sval1[0]) || sval1[0] == '-')
                                val1 = Double.Parse(sval1);
                            else
                                val1 = GetVarValue(sval1);

                            if (Char.IsDigit(sval2[0]) || sval2[0] == '-')
                                val2 = Double.Parse(sval2);
                            else
                                val2 = GetVarValue(sval2);
                            stack.Push((val2 >= val1).ToString());
                        }
                        break;
                    case "!=":
                        {
                            string sval1 = stack.Pop();
                            string sval2 = stack.Pop();
                            double val2, val1;
                            if (Char.IsDigit(sval1[0]) || sval1[0] == '-')
                                val1 = Double.Parse(sval1);
                            else
                                val1 = GetVarValue(sval1);

                            if (Char.IsDigit(sval2[0]) || sval2[0] == '-')
                                val2 = Double.Parse(sval2);
                            else
                                val2 = GetVarValue(sval2);
                            stack.Push((val2 != val1).ToString());
                        }
                        break;
                    case "AND":
                        {
                            bool val1 = Convert.ToBoolean(stack.Pop());
                            bool val2 =  Convert.ToBoolean(stack.Pop());
                           
                            stack.Push((val2 && val1).ToString());
                        }
                        break;
                    case "OR":
                        {
                            bool val1 = Convert.ToBoolean(stack.Pop());
                            bool val2 = Convert.ToBoolean(stack.Pop());

                            stack.Push((val2 || val1).ToString());
                        }
                        break;
                    case "NOT":
                        {
                            bool val1 = Convert.ToBoolean(stack.Pop());
                            stack.Push((!val1).ToString());
                        }
                        break;
                    case "УПХ":
                    {
                       string label =stack.Pop();
                       bool val1 = Convert.ToBoolean(stack.Pop());
                    if(val1==false)
                        i = (int)GetLabelValue(label) - 2;
                    }
                        break;
                    default:
                        {
                            if(rpn[i][rpn[i].Count()-1]!=':')
                            stack.Push(rpn[i]);
                        } break;

                }
            }
            form.Invoke(new ThreadStart(delegate
            {
                output.Text += "_______________"+Environment.NewLine;
                output.Text += "Done";

                //output.SelectionStart = output.Text.Length;

            }));
        }
    }
}

using System.Collections.Generic;
using System.IO;

namespace AutoParser
{
    public class keeper
    {
        public List<int> labels { get; set; }
        public List<int> state { get; set; }
        public int additionalstack { get; set; }
        public List<int> stack { get; set; }
        public object equals { get; set; }
        public object notequals { get; set; }

        public keeper(List<int> labels, List<int> state, List<int> stack, int additionalstack, object equals, object notequals)
        {
            this.equals = equals;
            this.notequals = notequals;
            this.labels = labels;
            this.stack = stack;
            this.state = state;
            this.additionalstack = additionalstack;
        }

    }
    public class Parser
    {
        List<Skaner.Lexems> Lexems;
        Skaner.Lexems CurrentLexem;
        Dictionary<int, keeper> States;
        int state;
        Stack<int> stack = new Stack<int>();
        int count;
        public Parser(List<Skaner.Lexems> Lexems)
        {
            this.Lexems = new List<Skaner.Lexems>(Lexems);
            count = 0;
            state = 1;
            States = new Dictionary<int, keeper>();
            States.Add(1, new keeper(new List<int> { 1 }, new List<int> { 2 }, new List<int> { 0 }, 0, null, "Init expected"));
            States.Add(2, new keeper(new List<int> { 30 }, new List<int> { 3 }, new List<int> { 0 }, 0, null, ": expected"));
            States.Add(3, new keeper(new List<int> { 50 }, new List<int> { 4 }, new List<int> { 0 }, 0, null, "name of program expected"));
            States.Add(4, new keeper(new List<int> { 31 }, new List<int> { 5 }, new List<int> { 0 }, 0, null, "new line expected"));
            States.Add(5, new keeper(new List<int> { 26 }, new List<int> { 6 }, new List<int> { 0 }, 0, null, "/ expected"));
            States.Add(6, new keeper(new List<int> { 31 }, new List<int> { 7 }, new List<int> { 0 }, 0, null, "new line expected"));
            States.Add(7, new keeper(new List<int> { 27 }, new List<int> { 0 }, new List<int> { 0 }, 8, "exit", 9));
            States.Add(8, new keeper(new List<int> { 31 }, new List<int> { 7 }, new List<int> { 0 }, 0, null, "new line expected or you used wrong operator"));
            States.Add(9, new keeper(new List<int> { 2, 3, 26 }, new List<int> { 10, 10, 14 }, new List<int> { 0, 0, 0 }, 13, null, 17));
            States.Add(10, new keeper(new List<int> { 30 }, new List<int> { 11 }, new List<int> { 0 }, 0, null, ": expected"));
            States.Add(11, new keeper(new List<int> { 50 }, new List<int> { 12 }, new List<int> { 0 }, 0, null, "id expected"));
            States.Add(12, new keeper(new List<int> { 32 }, new List<int> { 11 }, new List<int> { 0 }, 0, null, 0));
            States.Add(13, new keeper(new List<int> { 0 }, new List<int> { 0 }, new List<int> { 0 }, 0, null, 0));
            States.Add(14, new keeper(new List<int> { 31 }, new List<int> { 15 }, new List<int> { 0 }, 0, null, "new line or \\ expected "));
            States.Add(15, new keeper(new List<int> { 27 }, new List<int> { 0 }, new List<int> { 0 }, 16, 0, 9));
            States.Add(16, new keeper(new List<int> { 31 }, new List<int> { 15 }, new List<int> { 0 }, 0, null, 0));
            States.Add(17, new keeper(new List<int> { 12, 50, 4, 5, 8, 6 }, new List<int> { 18, 19, 21, 23, 27, 35 }, new List<int> { 0, 0, 0, 0, 0, 0 }, 0, null, 0));
            States.Add(18, new keeper(new List<int> { 50 }, new List<int> { 0 }, new List<int> { 0 }, 0, null, "id expected"));
            States.Add(19, new keeper(new List<int> { 13, 30 }, new List<int> { 40, 0 }, new List<int> { 20, 0 }, 0, null, "= or : expected"));
            States.Add(20, new keeper(new List<int> { 0 }, new List<int> { 0 }, new List<int> { 0 }, 0, null, 0));
            States.Add(21, new keeper(new List<int> { 28 }, new List<int> { 40 }, new List<int> { 22 }, 0, null, "( expected"));
            States.Add(22, new keeper(new List<int> { 29 }, new List<int> { 0 }, new List<int> { 0 }, 0, null, ") expected"));
            States.Add(23, new keeper(new List<int> { 28 }, new List<int> { 24 }, new List<int> { 0 }, 0, null, "( expected"));
            States.Add(24, new keeper(new List<int> { 29, 50 }, new List<int> { 0, 25 }, new List<int> { 0, 0 }, 0, null, ") or id expected"));
            States.Add(25, new keeper(new List<int> { 32, 29 }, new List<int> { 26, 0 }, new List<int> { 0, 0 }, 0, null, ") or , expected"));
            States.Add(26, new keeper(new List<int> { 50 }, new List<int> { 25 }, new List<int> { 0 }, 0, null, "id expected"));
            States.Add(27, new keeper(new List<int> { 28 }, new List<int> { 43 }, new List<int> { 47 }, 0, null, "( expected"));
            States.Add(47, new keeper(new List<int> { 29 }, new List<int> { 28 }, new List<int> { 0 }, 0, null, ") expected"));
            States.Add(28, new keeper(new List<int> { 9 }, new List<int> { 29 }, new List<int> { 0 }, 0, null, "then expected"));
            States.Add(29, new keeper(new List<int> { 31 }, new List<int> { 30 }, new List<int> { 0 }, 0, null, "new line   expected"));
            States.Add(30, new keeper(new List<int> { 10 }, new List<int> { 32 }, new List<int> { 0 }, 31, null, 9));
            States.Add(31, new keeper(new List<int> { 31 }, new List<int> { 30 }, new List<int> { 0 }, 0, null, "new line or \\ expected"));
            States.Add(32, new keeper(new List<int> { 31 }, new List<int> { 33 }, new List<int> { 0 }, 0, null, "new line expected"));
            States.Add(33, new keeper(new List<int> { 11 }, new List<int> { 0 }, new List<int> { 0 }, 34, null, 9));
            States.Add(34, new keeper(new List<int> { 31 }, new List<int> { 33 }, new List<int> { 0 }, 0, null, "new line or \\ expected"));
            States.Add(35, new keeper(new List<int> { 28 }, new List<int> { 43 }, new List<int> { 36 }, 0, null, "( expected"));
            States.Add(36, new keeper(new List<int> { 29 }, new List<int> { 37 }, new List<int> { 0 }, 0, null, ") expected"));
            States.Add(37, new keeper(new List<int> { 7 }, new List<int> { 38 }, new List<int> { 0 }, 0, null, "do expected"));
            States.Add(38, new keeper(new List<int> { 31 }, new List<int> { 9 }, new List<int> { 0 }, 39, null, "new line expected"));
            States.Add(39, new keeper(new List<int> { 31 }, new List<int> { 0 }, new List<int> { 0 }, 0, null, "new line expected"));
            States.Add(40, new keeper(new List<int> { 24, 28, 50, 51 }, new List<int> { 40, 40, 42, 42 }, new List<int> { 0, 41, 0, 0 }, 0, null, "id, number or ( expected"));
            States.Add(41, new keeper(new List<int> { 29 }, new List<int> { 42 }, new List<int> { 0 }, 0, null, ") expected"));
            States.Add(42, new keeper(new List<int> { 23, 24, 25, 26 }, new List<int> { 40, 40, 40, 40 }, new List<int> { 0, 0, 0, 0 }, 0, null, 0));
            States.Add(43, new keeper(new List<int> { 22, 28 }, new List<int> { 43, 43 }, new List<int> { 0, 46 }, 44, null, 40));
            States.Add(44, new keeper(new List<int> { 14, 15, 16, 17, 18, 19 }, new List<int> { 40, 40, 40, 40, 40, 40 }, new List<int> { 45, 45, 45, 45, 45, 45 }, 0, null, "<= or >= or != or or == or < or > expected"));
            States.Add(45, new keeper(new List<int> { 20, 21 }, new List<int> { 43, 43 }, new List<int> { 0, 0 }, 0, null, 0));
            States.Add(46, new keeper(new List<int> { 29 }, new List<int> { 45 }, new List<int> { 0 }, 0, null, ") expected"));
        }
        Skaner.Lexems GetNextLexem()
        {
            if (count >= Lexems.Count)
                return null;
            else
            {
                CurrentLexem = Lexems[count++];
                return CurrentLexem;
            }
        }

        public void Table()
        {
            File.WriteAllText("t1", "");
            for (int i = 1; i < 47; i++)
            {
                for (int j = 0; j < States[i].labels.Count; j++)
                {
                    File.AppendAllText("t1", string.Format("{0,-4} {1,-4} {2,-4} {3,-4}\n", i, States[i].labels[j].ToString() == "0" ? "" : States[i].labels[j].ToString(), States[i].state[j], States[i].stack[j]));

                }
            }
        }
        public void check()
        {
            File.WriteAllText("t2", "");
            while (true)
            {
                bool found = false;
                Skaner.Lexems Lexem = GetNextLexem();
                if (Lexem == null)
                    throw new Exceptions.MyException("\\ expected Line:" + Lexems[count - 1].RowNumber);
                File.AppendAllText("t2", string.Format("{0,-30} {1,-68}    ", Lexem.LexemName, state));
                for (int i = 0; i < stack.Count; i++)
                {
                    File.AppendAllText("t2", string.Format("{0}, ", stack.ToArray()[i]));
                }
                File.AppendAllText("t2", "\n");
                for (int i = 0; i < States[state].labels.Count; i++)
                    if (Lexem.LexemCode == States[state].labels[i])
                    {
                        if (States[state].state[i] == 0)
                        {
                            if (stack.Count > 0)
                            {
                                state = stack.Pop();
                                found = true;
                                break;
                            }
                            else
                            {
                                if (Lexems.Count - count > 0)
                                    throw new Exceptions.MyException("After the last scope cannot be any text Line:" + Lexem.RowNumber);
                                return;
                            }
                        }
                        else
                        {
                            if (States[state].stack[i] != 0)
                            {
                                stack.Push(States[state].stack[i]);
                                state = (int)States[state].state[i];
                                found = true;
                                break;
                            }
                            else
                            {
                                state = States[state].state[i];
                                found = true;
                                break;

                            }
                        }
                    }

                if (!found)
                {
                    found = false;
                    if (States[state].notequals is int)
                    {
                        if ((int)States[state].notequals == 0)
                        {
                            state = stack.Pop();
                            count--;
                            found = true;
                        }
                        else
                        {
                            stack.Push(States[state].additionalstack);
                            state = (int)States[state].notequals;

                            count--;
                            found = true;
                        }
                    }
                }
                if (!found)
                {
                    throw new Exceptions.MyException(States[state].notequals as string + "Line:" + Lexem.RowNumber);
                }
            }
        }
    }
}

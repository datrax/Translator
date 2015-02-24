using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AscendingParser
{
    public class Parser
    {
        public DataGridView datagrid;
        List<Skaner.Lexems> Lexems;
        Skaner.Lexems CurrentLexem;

        string[] NotTerminals;
        DataGridView RelationTable;
        ArrayList grammar;
        int count;
        List<string> stack = new List<string>();
        public Parser(List<Skaner.Lexems> Lexems)
        {
            this.Lexems = new List<Skaner.Lexems>(Lexems);
            count = 0;
            САПР4.Form1 relationform = new САПР4.Form1();
          //  relationform.Show();
            relationform.FillTheTable();
            RelationTable = relationform.GetTable();
            grammar = relationform.GetGrammar();
            NotTerminals = relationform.GetNotTerminals();
        }

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
        public void check()
        {


            stack.Add("");
            while (true)
            {
                
                string Lex = "";
                if (TryNextLexem().LexemCode == 50)
                    Lex = "id";
                else
                    if (TryNextLexem().LexemCode == 51)
                        Lex = "const";
                    else
                        Lex = TryNextLexem().LexemName;
                string Relation = GetRelation(stack[stack.Count - 1], Lex);
                FillTable(Relation);
                if (Relation == "<" || Relation == "=")
                {
                    stack.Add(Lex);
                    GetNextLexem();
                }
                else
                {

                    List<string> temp = stack.ToList();

                    string answer = "";
                    int j = 0;
                    for (int i = 0; i < stack.Count; i++)
                    {
                        if (temp.Count > 0)
                        {
                            answer = FindGram(temp);
                            if (FindGram(temp) == "progr")
                            {
                                datagrid.Rows.Add("progr",">");
                                return;
                            }
                            if (FindGram(temp) != "") break;
                            temp.RemoveAt(0);
                            j++;
                        }
                    }
                    j = stack.Count - temp.Count;
                    if (answer != "")
                    {
                        stack.RemoveRange(j, stack.Count - (j));
                        if (answer == "spys_id" && !(Lexems[count -2].LexemCode == 30)&& !(Lexems[count - 2].LexemCode == 32)&&!(Lexems[count -3].LexemCode==5))
                            answer = "mnozh";
                        stack.Add("<" + answer + ">");
                    }
                    else
                    {
                        throw new Exceptions.MyException("Wrong  " +stack[stack.Count-1]+": "+ Lexems[count-1].RowNumber);
                    }

                }                  
            }
        }

        private void FillTable(string Relation)
        {
            string str = "";
            for (int i = 0; i < stack.Count; i++)
            {
                str += " " + stack.ToArray()[i];
            }
            string str1 = "";
            for (int i = count; i < Lexems.Count; i++)
            {
                str1 += " " + Lexems[i].LexemName;
            }
            datagrid.Rows.Add(str, Relation, str1);
        }

     

        private string FindGram(List<string> temp)
        {
            for (int i = 0; i < grammar.Count; i++)
            {
                if (grammar[i] is string[][])
                {
                    string[][] t = (string[][])grammar[i];
                    for (int j = 0; j < t.Length; j++)
                    {
                        if (CompareStringArrays(temp.ToArray(), t[j])) return NotTerminals[i];
                    }

                }
                else
                {
                    if (CompareStringArrays(temp.ToArray(), (string[])grammar[i])) return NotTerminals[i];
                }
            }
            return "";
        }
        private bool CompareStringArrays(string[] first, string[] second)
        {
            if (first.Length != second.Length) return false;
            for (int i = 0; i < second.Length; i++)
                if (first[i] != second[i]) return false;
            return true;
        }
        public string GetRelation(String Lex1, String Lex2)
        {
            if (Lex1 == "") return "<";
            if (Lex2 == "") return ">";
            int i = 0, j = 0;
            for (; i < RelationTable.RowCount; i++)
            {
                if ((RelationTable[0, i].Value == null ? "" : RelationTable[0, i].Value.ToString()) == Lex1) break;
            };
            for (; i < RelationTable.ColumnCount; j++)
            {
                if ((RelationTable[j, 0].Value == null ? "" : RelationTable[j, 0].Value.ToString()) == Lex2) break;
            };
            if (RelationTable[j, i].Value == null) throw new Exceptions.MyException("Wrong operator: " + Lexems[count].RowNumber);
            else return RelationTable[j, i].Value.ToString();
        }
    }
}
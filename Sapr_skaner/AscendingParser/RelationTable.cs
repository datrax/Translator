using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace САПР4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string[] progr = { "init", "/" ,"<spys_elem_1>", "\\" };
        string[] spys_elem_1 = { "<spys_elem>" };
        string[][] spys_elem = { new string[] { "<elem1>", "<rozd>", "<spys_elem>" }, new string[] { "<elem1>", "<rozd>" } };
        string[][] elem = { new string[] { "<ogol>" }, new string[] { "<oper>" }, new string[] { "<mitka>" }, new string[] { "/", "<spys_elem_1>", "\\" } };
        string[] elem1 = { "<elem>" };
        string[] rozd = { "¶" };
        string[] ogol = { "<type>", ":", "<spys_id_1>" };
        string[] spys_id_1 = { "<spys_id>" };
        string[][] spys_id = { new string[] { "id" },new string[] { "id", ",", "<spys_id>" }  };
        string[][] type = { new string[] { "int" }, new string[] { "real" } };
        string[][] oper = { new string[] { "<prysv>" }, new string[] { "<vved>" }, new string[] { "<vyved>" }, new string[] { "<tsykl>" }, new string[] { "<umov_oper>" }, new string[] { "<perehid>" } };
        string[] prysv = { "id", "=", "<ar_vyr>" };
        string[][] vved = { new string[] { "scan", "(", "<spys_id_1>", ")" }, new string[] { "scan", "(", ")" } };
        string[] vyved = { "print", "(", "<ar_vyr_1>", ")" };
        string[] tsykl = { "while", "(", "<log_vyr_1>", ")", "do", "<elem>" };
        string[] umov_oper = { "if", "(", "<log_vyr_1>", ")", "then",  "<spys_elem_1>", "else", "<spys_elem_1>", "endif" };
        string[] mitka = { "id", ":" };
        string[] perehid = { "goto", "id" };
        string[] ar_vyr_1 = { "<ar_vyr>" };
        string[][] ar_vyr = { new string[] { "-", "<ar_vyr>" }, new string[] { "<dod_1>" }, new string[] { "<dod_1>", "+", "<ar_vyr>" }, new string[] { "<dod_1>", "-", "<ar_vyr>" } };
        string[] dod_1 = { "<dod>" };
        string[][] dod = { new string[] { "<mnozh>" }, new string[] { "<mnozh>", "*", "<dod>" }, new string[] { "<mnozh>", "/", "<dod>" } };
        string[][] mnozh = { new string[] { "(", "<ar_vyr_1>", ")" }, new string[] { "id" }, new string[] { "const" } };
        string[] log_vyr_1 = { "<log_vyr>" };
        string[][] log_vyr = { new string[] { "<log_dod_1>", "OR", "<log_vyr>" }, new string[] { "<log_dod_1>" } };
        string[] log_dod_1 = { "<log_dod>" };
        string[][] log_dod = { new string[] { "<log_mnozh>" }, new string[] { "<log_mnozh>", "AND", "<log_dod>" } };
        string[][] log_mnozh = { new string[] { "(", "<log_vyr_1>", ")" }, new string[] { "<ar_vyr_1>", "<log_znak>", "<ar_vyr>" } };
        string[][] log_znak = { new string[] { "<" }, new string[] { ">" }, new string[] { "==" }, new string[] { ">=" }, new string[] { "<=" }, new string[] { "!=" } };
        ArrayList gram = new ArrayList();
        String[] masneterm;

        public void FillTheTable(){
             gram.Add(progr);
            gram.Add(spys_elem_1);
            gram.Add(spys_elem);
            gram.Add(elem1);
            gram.Add(elem);
            gram.Add(rozd);
            gram.Add(ogol);
            gram.Add(spys_id_1);
            gram.Add(spys_id);
            gram.Add(type);
            gram.Add(oper);
            gram.Add(prysv);
            gram.Add(vved);
            gram.Add(vyved);
            gram.Add(tsykl);
            gram.Add(umov_oper);
            gram.Add(mitka);
            gram.Add(perehid);
            gram.Add(ar_vyr_1);
            gram.Add(ar_vyr);
            gram.Add(dod_1);
            gram.Add(dod);
            gram.Add(mnozh);
            gram.Add(log_vyr_1);
            gram.Add(log_vyr);
            gram.Add(log_dod_1);
            gram.Add(log_dod);
            gram.Add(log_mnozh);
            gram.Add(log_znak);

            masneterm =new string[] { "progr", "spys_elem_1", "spys_elem", "elem1","elem","rozd","ogol", "spys_id_1", "spys_id", "type", "oper", "prysv", "vved", "vyved", "tsykl", "umov_oper", "mitka", "perehid", "ar_vyr_1", "ar_vyr", "dod_1", "dod", "mnozh", "log_vyr_1", "log_vyr", "log_dod_1", "log_dod", "log_mnozh", "log_znak" };
            string[] neterm1;
            string[][] neterm2;
            Stack stack = new Stack();
            //Fill table
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add(progr[0]);
            dataGridView1.Columns.Add("Col", "");
            dataGridView1.Rows[0].Cells[1].Value = progr[0];
            for (int i = 0; i < gram.Count; i++)
            {
                if (gram[i] is string[][])
                {
                    neterm2 = (string[][])gram[i];
                    bool notfound = false;
                    int j;
                    for (int mas = 0; mas < neterm2.Length; mas++)
                    {
                        for (int mas2 = 0; mas2 < neterm2[mas].Length; mas2++)
                        {
                            for (j = 1; j < dataGridView1.ColumnCount; j++)
                            {
                                if ((string)dataGridView1.Rows[0].Cells[j].Value == neterm2[mas][mas2]) { notfound = false; break; }
                                notfound = true;
                            }
                            if (notfound)
                            {
                                dataGridView1.Columns.Add("Col" + j, "");
                                dataGridView1.Rows[0].Cells["Col" + j].Value = neterm2[mas][mas2];
                                dataGridView1.Rows.Add(neterm2[mas][mas2]);
                            }
                        }
                    }
                }
                else
                {
                    neterm1 = (string[])gram[i];
                    bool ispres = false;
                    int j;
                    for (int mas = 0; mas < neterm1.Count(); mas++)
                    {
                        for (j = 1; j < dataGridView1.ColumnCount; j++)
                        {
                            if ((string)dataGridView1.Rows[0].Cells[j].Value == neterm1[mas]) { ispres = false; break; }
                            ispres = true;
                        }
                        if (ispres)
                        {
                            dataGridView1.Columns.Add("Col" + j, "");
                            dataGridView1.Rows[0].Cells["Col" + j].Value = neterm1[mas];
                            dataGridView1.Rows.Add(neterm1[mas]);
                        }
                    }
                }
            }
            //Fill =
            for (int i = 0; i < gram.Count; i++)
            {
                if (gram[i] is string[][])
                {
                    neterm2 = (string[][])gram[i];
                    int Row = 0, Col = 0;
                    for (int mas = 0; mas < neterm2.Length; mas++)
                    {
                        for (int mas2 = 0; mas2 < neterm2[mas].Length; mas2++)
                        {
                            if (mas2 == neterm2[mas].Length - 1) break;
                            for (int j = 1; j < dataGridView1.ColumnCount; j++)
                            {
                                if ((string)dataGridView1.Rows[0].Cells[j].Value == neterm2[mas][mas2 + 1])
                                {
                                    Col = j;
                                    break;
                                }
                            }
                            for (int j = 1; j < dataGridView1.RowCount; j++)
                            {
                                if ((string)dataGridView1.Rows[j].Cells[0].Value == neterm2[mas][mas2])
                                {
                                    Row = j;
                                    break;
                                }
                            }
                            dataGridView1.Rows[Row].Cells[Col].Value = "=";
                        }
                    }
                }
                else
                {
                    neterm1 = (string[])gram[i];
                    int Row = 0, Col = 0;
                    for (int mas = 0; mas < neterm1.Count(); mas++)
                    {
                        if (mas == neterm1.Count() - 1) break;
                        for (int j = 1; j < dataGridView1.ColumnCount; j++)
                        {
                            if ((string)dataGridView1.Rows[0].Cells[j].Value == neterm1[mas + 1])
                            {
                                Col = j;
                                break;
                            }
                        }
                        for (int j = 1; j < dataGridView1.RowCount; j++)
                        {
                            if ((string)dataGridView1.Rows[j].Cells[0].Value == neterm1[mas])
                            {
                                Row = j;
                                break;
                            }
                        }
                        dataGridView1.Rows[Row].Cells[Col].Value = "=";
                    }
                }
            }
            //Fill <
            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                string noterm = (string)dataGridView1.Rows[0].Cells[i].Value;
                if (noterm[0] == '<' && noterm.Length > 2)
                {
                    noterm = noterm.Substring(0, noterm.Length - 1);
                    noterm = noterm.Remove(0, 1);
                    for (int j = 1; j < dataGridView1.RowCount; j++)
                        if ((string)dataGridView1.Rows[j].Cells[i].Value == "=")
                        {
                            List<string> mas = new List<string>();
                            bool ind = false;
                            string noterm1 = noterm;
                            while (!ind)
                                for (int k = 0; k < masneterm.Length; k++)
                                    if (masneterm[k] == noterm1)
                                    {
                                        if (gram[k] is string[][])
                                        {
                                            neterm2 = (string[][])gram[k];
                                            for (int mas1 = 0; mas1 < neterm2.Length; mas1++)
                                                if (!mas.Contains(neterm2[mas1][0]))
                                                {
                                                    mas.Add(neterm2[mas1][0]);
                                                    if (neterm2[mas1][0][0] == '<' && neterm2[mas1][0].Length > 2)
                                                    {
                                                        string rstr = neterm2[mas1][0].Substring(0, neterm2[mas1][0].Length - 1);
                                                        rstr = rstr.Remove(0, 1);
                                                        stack.Push(rstr);
                                                    }
                                                    else ind = true;
                                                }
                                        }
                                        else
                                        {
                                            neterm1 = (string[])gram[k];
                                            if (!mas.Contains(neterm1[0])) mas.Add(neterm1[0]);
                                            if (neterm1[0][0] == '<' && neterm1[0].Length > 2)
                                            {
                                                noterm1 = neterm1[0].Substring(0, neterm1[0].Length - 1);
                                                noterm1 = noterm1.Remove(0, 1);
                                                stack.Push(noterm1);
                                            }
                                            else ind = true;
                                        }
                                        if (stack.Count != 0) { noterm1 = (string)stack.Pop(); ind = false; }
                                        break;
                                    }
                            for (int l1 = 0; l1 < mas.Count; l1++)
                                for (int l2 = 1; l2 < dataGridView1.ColumnCount; l2++)
                                    if ((string)dataGridView1.Rows[0].Cells[l2].Value == mas[l1])
                                    {
                                        if ((string)dataGridView1.Rows[j].Cells[l2].Value == "=")
                                        {
                                            //MessageBox.Show("Конфлікт!\n <" + noterm + "> " + mas[l1] + " При < на =");
                                            dataGridView1.Rows[j].Cells[l2].Value += "<";
                                            //ShowDialog();
                                        }
                                        else dataGridView1.Rows[j].Cells[l2].Value = "<";
                                        break;
                                    }
                        }
                }
            }
            //Fill >
            for (int i = 1; i < dataGridView1.RowCount; i++)
            {
                string noterm = (string)dataGridView1.Rows[i].Cells[0].Value;
                if (noterm[0] == '<' && noterm.Length > 2)
                {
                    noterm = noterm.Substring(0, noterm.Length - 1);
                    noterm = noterm.Remove(0, 1);
                    for (int j = 1; j < dataGridView1.ColumnCount; j++)
                        if ((string)dataGridView1.Rows[i].Cells[j].Value == "=")
                        {
                            List<string> mas = new List<string>();
                            bool ind = false;
                            string noterm1 = noterm;
                            while (!ind)
                                for (int k = 0; k < masneterm.Length; k++)
                                    if (masneterm[k] == noterm1)
                                    {
                                        if (gram[k] is string[][])
                                        {
                                            neterm2 = (string[][])gram[k];
                                            for (int mas1 = 0; mas1 < neterm2.Length; mas1++)
                                            {
                                                if (!mas.Contains(neterm2[mas1][neterm2[mas1].Length - 1]))
                                                {
                                                    mas.Add(neterm2[mas1][neterm2[mas1].Length - 1]);
                                                    if (neterm2[mas1][neterm2[mas1].Length - 1][0] == '<' && neterm2[mas1][neterm2[mas1].Length - 1].Length > 2)
                                                    {
                                                        string rstr = neterm2[mas1][neterm2[mas1].Length - 1].Substring(0, neterm2[mas1][neterm2[mas1].Length - 1].Length - 1);
                                                        rstr = rstr.Remove(0, 1);
                                                        stack.Push(rstr);
                                                    }
                                                    else ind = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            neterm1 = (string[])gram[k];
                                            if (!mas.Contains(neterm1[neterm1.Length - 1])) mas.Add(neterm1[neterm1.Length - 1]);
                                            if (neterm1[neterm1.Length - 1][0] == '<' && neterm1[neterm1.Length - 1].Length > 2)
                                            {
                                                noterm1 = neterm1[neterm1.Length - 1].Substring(0, neterm1[neterm1.Length - 1].Length - 1);
                                                noterm1 = noterm1.Remove(0, 1);
                                                stack.Push(noterm1);
                                            }
                                            else ind = true;
                                        }
                                        if (stack.Count != 0) { noterm1 = (string)stack.Pop(); ind = false; }
                                        else ind = true;
                                        break;
                                    }
                            if (Convert.ToString(dataGridView1.Rows[0].Cells[j].Value)[0] == '<' && Convert.ToString(dataGridView1.Rows[0].Cells[j].Value).Length > 2)
                            {
                                List<string> mas2 = new List<string>();
                                bool ind2 = false;
                                noterm = (string)dataGridView1.Rows[0].Cells[j].Value;
                                noterm = noterm.Substring(0, noterm.Length - 1);
                                noterm = noterm.Remove(0, 1);
                                string noterm2 = noterm;
                                while (!ind2)
                                    for (int k = 0; k < masneterm.Length; k++)
                                        if (masneterm[k] == noterm2)
                                        {
                                            if (gram[k] is string[][])
                                            {
                                                neterm2 = (string[][])gram[k];
                                                for (int mas1 = 0; mas1 < neterm2.Length; mas1++)
                                                    if (!mas2.Contains(neterm2[mas1][0]))
                                                    {
                                                        mas2.Add(neterm2[mas1][0]);
                                                        if (neterm2[mas1][0][0] == '<' && neterm2[mas1][0].Length > 2)
                                                        {
                                                            string rstr = neterm2[mas1][0].Substring(0, neterm2[mas1][0].Length - 1);
                                                            rstr = rstr.Remove(0, 1);
                                                            stack.Push(rstr);
                                                        }
                                                        else ind2 = true;
                                                    }
                                            }
                                            else
                                            {
                                                neterm1 = (string[])gram[k];
                                                if (!mas2.Contains(neterm1[0])) mas2.Add(neterm1[0]);
                                                if (neterm1[0][0] == '<' && neterm1[0].Length > 2)
                                                {
                                                    noterm2 = neterm1[0].Substring(0, neterm1[0].Length - 1);
                                                    noterm2 = noterm2.Remove(0, 1);
                                                    stack.Push(noterm2);
                                                }
                                                else ind2 = true;
                                            }
                                            if (stack.Count != 0) { noterm2 = (string)stack.Pop(); ind2 = false; }
                                            break;
                                        }
                                for (int l1 = 0; l1 < mas.Count; l1++)
                                    for (int l2 = 1; l2 < dataGridView1.RowCount; l2++)
                                        if ((string)dataGridView1.Rows[l2].Cells[0].Value == mas[l1])
                                        {
                                            if ((string)dataGridView1.Rows[l2].Cells[j].Value == "=" || (string)dataGridView1.Rows[l2].Cells[j].Value == "<")
                                            {
                                               // MessageBox.Show("Конфлікт!\n <" + noterm + "> " + mas[l1] + " При > на " + dataGridView1.Rows[l2].Cells[j].Value);
                                                dataGridView1.Rows[l2].Cells[j].Value += ">";
                                                //ShowDialog();
                                            }
                                            else dataGridView1.Rows[l2].Cells[j].Value = ">";
                                            for (int l3 = 0; l3 < mas2.Count; l3++)
                                                for (int l4 = 1; l4 < dataGridView1.ColumnCount; l4++)
                                                    if ((string)dataGridView1.Rows[0].Cells[l4].Value == mas2[l3])
                                                        if ((string)dataGridView1.Rows[l2].Cells[l4].Value == "=" || (string)dataGridView1.Rows[l2].Cells[l4].Value == "<")
                                                        {
                                                         //   MessageBox.Show("Конфлікт!\n <" + noterm + "> " + mas2[l3] + " При > на " + dataGridView1.Rows[l2].Cells[l4].Value);
                                                            dataGridView1.Rows[l2].Cells[l4].Value += ">";
                                                            //ShowDialog();
                                                        }
                                                        else dataGridView1.Rows[l2].Cells[l4].Value = ">";
                                            break;
                                        }
                            }
                            else
                            {
                                for (int l1 = 0; l1 < mas.Count; l1++)
                                    for (int l2 = 1; l2 < dataGridView1.RowCount; l2++)
                                        if ((string)dataGridView1.Rows[l2].Cells[0].Value == mas[l1])
                                        {
                                            if ((string)dataGridView1.Rows[l2].Cells[j].Value == "=" || (string)dataGridView1.Rows[l2].Cells[j].Value == "<")
                                            {
                                             // MessageBox.Show("Конфлікт!\n <" + noterm + "> " + mas[l1] + " При > на " + dataGridView1.Rows[l2].Cells[j].Value);
                                                dataGridView1.Rows[l2].Cells[j].Value += ">";
                                                //ShowDialog();
                                            }
                                            else dataGridView1.Rows[l2].Cells[j].Value = ">";
                                            break;
                                        }
                            }
                        }
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            FillTheTable();
        }

        internal DataGridView GetTable()
        {
            return dataGridView1;
        }

        internal ArrayList GetGrammar()
        {
            return gram;
        }

        internal string[] GetNotTerminals()
        {
            return masneterm;
        }
    }
}
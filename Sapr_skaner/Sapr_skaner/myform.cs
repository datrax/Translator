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
using Interpreters;
using System.Threading;


namespace Skaner
{
    public delegate void del();
    public class myform : Form
    {
        private Button button1;
        private Button button2;
        private Button button3;
        private List<Lexems> MainItems;
        private List<Lexems> ConstItems;
        private List<Lexems> IdItems;
        private List<Lexems> GotoItems;
        private List<Lexems> LabelItems;

        private TextBox textBox11;
        private ComboBox ParsingMethod;
        private TextBox OutputScreen;
        private Button button4;
        private TextBox inputBox;

        string path;
        public myform()
        {
            InitializeComponent();
            ConstItems = new List<Lexems>();
            MainItems = new List<Lexems>();
            IdItems = new List<Lexems>();
            GotoItems = new List<Lexems>();
            LabelItems = new List<Lexems>();
            ParsingMethod.Items.Add("Рекурсивный спуск");
            ParsingMethod.Items.Add("Высходящий разбор");
            ParsingMethod.SelectedItem = ParsingMethod.Items[1];
        }
        #region Initform
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.ParsingMethod = new System.Windows.Forms.ComboBox();
            this.OutputScreen = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.inputBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(671, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Сохранить как";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(784, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "сохранить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(732, 100);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Выполнить";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox11
            // 
            this.textBox11.BackColor = System.Drawing.SystemColors.Window;
            this.textBox11.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox11.Location = new System.Drawing.Point(-3, 0);
            this.textBox11.Multiline = true;
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(668, 454);
            this.textBox11.TabIndex = 0;
            this.textBox11.WordWrap = false;
            this.textBox11.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox11_KeyDown);
            // 
            // ParsingMethod
            // 
            this.ParsingMethod.FormattingEnabled = true;
            this.ParsingMethod.Location = new System.Drawing.Point(671, 58);
            this.ParsingMethod.Name = "ParsingMethod";
            this.ParsingMethod.Size = new System.Drawing.Size(188, 21);
            this.ParsingMethod.TabIndex = 5;
            // 
            // OutputScreen
            // 
            this.OutputScreen.Location = new System.Drawing.Point(670, 142);
            this.OutputScreen.Multiline = true;
            this.OutputScreen.Name = "OutputScreen";
            this.OutputScreen.ReadOnly = true;
            this.OutputScreen.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.OutputScreen.Size = new System.Drawing.Size(189, 160);
            this.OutputScreen.TabIndex = 6;
            this.OutputScreen.TextChanged += new System.EventHandler(this.StopThred);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(801, 308);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(58, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "Ввод";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.InputNumber);
            // 
            // inputBox
            // 
            this.inputBox.Location = new System.Drawing.Point(672, 310);
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(123, 20);
            this.inputBox.TabIndex = 8;
            // 
            // myform
            // 
            this.ClientSize = new System.Drawing.Size(871, 455);
            this.Controls.Add(this.inputBox);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.OutputScreen);
            this.Controls.Add(this.ParsingMethod);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox11);
            this.Name = "myform";
            this.Text = "Datrax editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        public void load_text(StreamReader str, string path)
        {
            textBox11.Text = str.ReadToEnd();
            textBox11.ScrollBars = ScrollBars.Both;
            this.path = path;
            textBox11.Select(0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sf.Filter = "Файлы dt|*.dt|Файлы txt|*.txt";
            sf.InitialDirectory = path;
            if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllText(sf.FileName, textBox11.Text, Encoding.Default);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.WriteAllText(path, textBox11.Text, Encoding.Default);
        }
        bool Validate(LexemsTable tableform)
        {
            for (int i = 0; i < MainItems.LongCount(); i++)
            {

                if (MainItems[i].LexemCode == -1)
                {

                    throw new Exceptions.MyException(("Error! Undefined lexem Line:" + MainItems[i].RowNumber));
                }
                if (MainItems[i].LexemCode == 50)
                {

                    bool notfound = true;
                    for (int j = 0; j < IdItems.LongCount(); j++)
                    {
                        if (MainItems[i].LexemName.Equals(IdItems[j].LexemName))
                        {
                            if (MainItems[i].type.Equals(IdItems[j].type)||(MainItems[i].type!=0))
                            {
                        
                            }
                            MainItems[i].IdConstCode = IdItems[j].IdConstCode;
                            MainItems[i].type = IdItems[j].type;
                            notfound = false;

                        }
                    }
                    if (notfound && MainItems[i].type > 0)
                    {
                        if (IdItems.LongCount() == 0) MainItems[i].IdConstCode = 1;
                        else
                            MainItems[i].IdConstCode = IdItems[Convert.ToInt16(IdItems.LongCount()) - 1].IdConstCode + 1;
                        IdItems.Add(MainItems[i]);
                    }
                    if (i < MainItems.LongCount() - 1 && MainItems[i + 1].LexemCode == 30 && i > 2)//label
                    {
                        MainItems[i].type = 3;
                        LabelItems.Add(MainItems[i]);
                    }
                    if (MainItems[i].type == 5) GotoItems.Add(MainItems[i]);
                    if (MainItems[i].type == 0)
                    {

                        throw new Exceptions.MyException(("Error! Undefined variable Line:" + MainItems[i].RowNumber));
                    }
                }
                else
                    MainItems[i].type = 0;//Убираем тип с запятых и т.д.
                if (MainItems[i].LexemCode == 51)
                {
                    bool notfound = true;
                    for (int j = 0; j < ConstItems.LongCount(); j++)
                    {
                        if (MainItems[i].LexemName.Equals(ConstItems[j].LexemName))
                        {
                            MainItems[i].IdConstCode = ConstItems[j].IdConstCode;
                            MainItems[i].type = ConstItems[j].type;
                            notfound = false;
                        }
                    }

                    if (notfound)
                    {
                        if (ConstItems.LongCount() == 0) MainItems[i].IdConstCode = 1;
                        else
                            MainItems[i].IdConstCode = ConstItems[Convert.ToInt16(ConstItems.LongCount()) - 1].IdConstCode + 1;
                        ConstItems.Add(MainItems[i]);
                    }


                }

                tableform.LexemTable.Rows.Add(MainItems[i].LexemNumber, MainItems[i].RowNumber, MainItems[i].LexemName, MainItems[i].LexemCode, MainItems[i].IdConstCode.ToString() == "0" ? " " : MainItems[i].IdConstCode.ToString());
            }

            CheckingLabels();

            foreach (Lexems g in IdItems)
            {
                string type = "";
                if (g.type == 1) type = "int";
                if (g.type == 2) type = "real";
                if (g.type == 5) type = "label";
                tableform.IdTable.Rows.Add(g.LexemName, g.IdConstCode, type);///////////////
            }
            foreach (Lexems g in ConstItems)
            {
                tableform.ConstTable.Rows.Add(g.LexemName, g.IdConstCode, g.type);///////////////
            }

            return true;
        }
       void CheckingLabels()
        {
           for (int i = 0; i < GotoItems.LongCount(); i++)
           {
               bool found = false;
               for (int j = 0; j < LabelItems.LongCount(); j++)
               {
                   if (LabelItems[j].LexemName.Equals(GotoItems[i].LexemName))
                       found = true;
               }
               if (found==false)
               {
                   throw new Exceptions.MyException(("Error! Label's not found. Line:" + GotoItems[i].RowNumber));
               }
           }                       
        }

        private Thread InterpreterThread;
        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "Выполнить")
            {
                button3.Text = "Стоп";
                start_lexem_determination();
              
            }
            else
            {
                if(InterpreterThread!=null)InterpreterThread.Abort();
                button3.Text = "Выполнить";
            }
            
        }

        void displa_error_message(Exceptions.MyException ex)
        {
            OutputScreen.Text += "There are errors:"+Environment.NewLine;
            int row;
            int.TryParse(ex.Message.Substring(ex.Message.LastIndexOf(":")+1), out row);
            textBox11.Select(find_line_position(row - 1) + 1, find_line_position(row) - find_line_position(row - 1) - 2);//-2 and +1 to avoid catching special symbols
            textBox11.Focus();
            MessageBox.Show(ex.Message);
            OutputScreen.Text += ex.Message;
        }
        void start_lexem_determination()
        {
            OutputScreen.Clear();
            IdItems.Clear();
            ConstItems.Clear();
            GotoItems.Clear();
            LabelItems.Clear();
            MainItems.Clear();
            LexemsTable tableform = new LexemsTable();
            StreamReader str;
            File.WriteAllText("Temp", textBox11.Text, Encoding.Default);
            str = File.OpenText("Temp");
            LexFind ob = new LexFind(str);
            while (ob.can_read())
                MainItems.Add(ob.NextLex());
            try
            {
                Validate(tableform);
             //    tableform.Show();//shows lexem table
                  OutputScreen.Text += "Lexem analiz - sucess"+Environment.NewLine;
                dynamic parser = null; 
                if (ParsingMethod.SelectedIndex == 0)
                {
                    parser = new Parser.Parser(MainItems);
                   
                }
                if (ParsingMethod.SelectedIndex == 1)
                {
                    parser = new AscendingParser.Parser(MainItems);
                    AscendingParser.StackOutput stackoutput = new AscendingParser.StackOutput(MainItems);
                    parser.datagrid = stackoutput.dataGridView1;
                    //parser.ShowRelations();
                    //  stackoutput.Show();
                    
                }
                if (ParsingMethod.SelectedIndex == 2)
                {
                  parser = new AutoParser.Parser(MainItems);//Auto parser
               // parser.Table();
                }               
                parser.check();
                OutputScreen.Text += "Grammar analiz - sucess"+Environment.NewLine;

                RPN.PolishNotation polobj = new RPN.PolishNotation(MainItems);
                polobj.build();
                OutputScreen.Text += "Polish Notation built"+Environment.NewLine+"Execution:"+Environment.NewLine+"_________________"+Environment.NewLine+Environment.NewLine;
                Interpreters.Interpreter interpreter = new Interpreters.Interpreter(polobj.rpn,tableform.IdTable,polobj.labelgrid,OutputScreen,this);

                InterpreterThread = new Thread(new ThreadStart(interpreter.Execute));

                
                InterpreterThread.Start();


            }
            catch (Exceptions.MyException ex)
            {
               
                displa_error_message(ex);
              
            }
            finally
            {
                str.Close();
            }
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5) button3_Click(null,null);

        }
        private int find_line_position(int row)
        {
            StreamReader str;
            str = File.OpenText("Temp");
            int pos = 0, count = 0;
            while (pos != row)
            {

                if (str.Peek() == -1) { str.Close(); return ++count; }

                if (str.Read() == 13) pos++;
                count++;
            }
            str.Close();
            return count;
        }

        private void InputNumber(object sender, EventArgs e)
        {
            double an;
            if (Double.TryParse(inputBox.Text, out an))
                OutputScreen.Text += an.ToString();
            else
            {
                MessageBox.Show("Only numbers!!");
            }
            inputBox.Text = "";
        }

        private void StopThred(object sender, EventArgs e)
        {
            if (OutputScreen.TextLength>0&&OutputScreen.Text[OutputScreen.TextLength - 1] == 'e')
            {
                if (InterpreterThread != null) InterpreterThread.Abort();
                button3.Text = "Выполнить";
            }
        }
         ~myform()

        {
            if (InterpreterThread != null) InterpreterThread.Abort();
            InterpreterThread.Join();
        }


    }
}
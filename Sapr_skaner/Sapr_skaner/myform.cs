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
    public class myform : Form
    {
        private Button button1;
        private Button button2;
        private Button button3;
        private List<Lexems> Items;
        private List<String> IdCode;
        private List<String> ConstCode;
        private List<int> IdType;
        private TextBox textBox11;
      //  LexemsTable tableform;
        string path;
        public myform(){
            InitializeComponent();
            Items = new List<Lexems>();
           // tableform  = new LexemsTable();
            IdCode=new List<string>();
            ConstCode = new List<string>();
            IdType = new List<int>();
        }
        #region Initform
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 460);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Сохранить как";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(117, 460);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "сохранить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(515, 460);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Лексемы";
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
            // myform
            // 
            this.ClientSize = new System.Drawing.Size(665, 497);
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

        public void load_text(StreamReader str,string path)
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

        int Validate(LexemsTable tableform)
        {
            
           
            int IdConstCode = 0;
            int CoCode = 0;
            for (int i = 0; i < Items.LongCount(); i++)
            {

                if (Items[i].LexemCode == -1) return Items[i].RowNumber;
                if (Items[i].LexemCode == 50)
                {
                    bool notfound = true;
                    for (int j = 0; j < IdCode.LongCount(); j++)
                    {
                        if (Items[i].LexemName.Equals(IdCode[j]))
                        {
                            IdConstCode = j + 1; notfound = false;
                            if (Items[i].type != 0) return Items[i].RowNumber;//проверка повтора в одном блоке обьявл
                        }
                    }
                    if (notfound)
                    {
                        if (Items[i].type == 0) return Items[i].RowNumber;//проверка инициализации переменной  
                        
                            IdConstCode++; IdCode.Add(Items[i].LexemName);
                            IdType.Add(Items[i].type);
                        
                      
                      
                    }

                    Items[i].IdConstCode = IdConstCode;
                }
                else Items[0].type = 0;//обнуляем лишний тип
                if (Items[i].LexemCode == 51)
                {
                    bool notfound = true;
                    for (int j = 0; j < ConstCode.LongCount(); j++)
                    {
                        if (Items[i].LexemName.Equals(IdCode[j]))
                        { CoCode = j + 1; notfound = false; }
                    }
                    if (notfound)
                    {
                        CoCode++; ConstCode.Add(Items[i].LexemName);
                    }

                    Items[i].IdConstCode = CoCode; 
                }

                tableform.LexemTable.Rows.Add(Items[i].LexemNumber, Items[i].RowNumber, Items[i].LexemName, Items[i].LexemCode, Items[i].IdConstCode.ToString() == "0" ? " " : Items[i].IdConstCode.ToString());/////////////
          
            }
            for (int j = 0; j < IdCode.LongCount(); j++)
            {
                string type = "";
                if (IdType[j] == 1) type = "int";
                if (IdType[j] == 2) type = "real";
                if (IdType[j] == 3) type = "label";
                tableform.IdTable.Rows.Add(IdCode[j],j + 1,type);//////////////////
            }
            for (int j = 0; j < ConstCode.LongCount(); j++)
            {
                tableform.ConstTable.Rows.Add(ConstCode[j], j + 1);///////////////
            }
            return -1;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            start_lexem_determination();
     
        }

      
        void start_lexem_determination()
        {
            IdCode = new List<string>();
            ConstCode = new List<string>();
            IdType = new List<int>();
            Items.Clear();
            LexemsTable tableform = new LexemsTable();
            StreamReader str;
            // str = File.OpenText(path);
            File.WriteAllText("Temp", textBox11.Text, Encoding.Default);
            str = File.OpenText("Temp");
            LexFind ob = new LexFind(str);
            while (ob.next())
                Items.Add(ob.NextLex());
            int c = Validate(tableform);
            if (c == -1)
            {
                tableform.Show();
                str.Close();
            }
            else
            {
                str.Close();               
                textBox11.Select(find_line_position(c - 1), find_line_position(c) - find_line_position(c-1));
              //  textBox11.Focus();
               // textBox11.SelectedText = new Font(textBox11.Font.Bold, 10);
                MessageBox.Show("Error in the line: " + c);
            };
   
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5) start_lexem_determination();
        }
        private int find_line_position(int row)
        {
            StreamReader str;
            str = File.OpenText("Temp");
            int pos = 0,count=0;
            while (pos != row)
            {

                if(str.Read()==13)pos++;
                count++;
            }
            str.Close();
            return count;
        }
    }
}
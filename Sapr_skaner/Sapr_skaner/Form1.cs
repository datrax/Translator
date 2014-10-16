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
    
    public partial class Form1 : Form
    {
        OpenFileDialog FileDialog;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "Для начала выберите ваш файл формата *.dt";
            button2.Enabled=true;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileDialog = new OpenFileDialog();
            FileDialog.Filter = "Файлы dt|*.dt|Файлы txt|*.txt";
            if (FileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FilePath.Text = FileDialog.FileName;
                FilePath.SelectionStart = FilePath.Text.Length;
                
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((FilePath.Text == "") || !FilePath.Text.Substring(FilePath.Text.Length - 3).Equals(".dt")&&
                !FilePath.Text.Substring(FilePath.Text.Length - 4).Equals(".txt"))
                MessageBox.Show("Ошибка не был выбран файл формата .dt");

            else
            {
                if(!File.Exists(FilePath.Text))
                    File.WriteAllText(FilePath.Text, "", Encoding.Default); 
                StreamReader re = File.OpenText(FilePath.Text);
                myform form2=new myform();
                form2.Show();
                form2.load_text(re, FilePath.Text);
                re.Close();
            }
        }
    }
}
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

namespace AscendingParser
{
    public partial class StackOutput : Form
    {
        List<Skaner.Lexems> MainItems;
        public StackOutput()
        {
            //InitializeComponent();
        }
      public   StackOutput(List<Skaner.Lexems> MainItems)
        {
            InitializeComponent();
            this.MainItems = MainItems;
        }

        private void Loading(object sender, EventArgs e)
        {
           
            try
            {
            
            }
            catch
            {
                throw;
            }
        }
    }
   
}

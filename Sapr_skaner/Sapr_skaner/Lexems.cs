using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sapr_skaner
{
    class Lexems
    {
        List<Lexems> fg;
        public Lexems(int a,int b,string c,int d,int e,int f){
            RowNumber = b;
            LexemNumber = a;
            LexemName = c;
            LexemCode = d;
            IdConstCode = e;
            type = f;
        }
        public int RowNumber { get; private set; }
        public int LexemNumber { get; private set; }
        public int LexemCode { get;set; }
        public string LexemName { get; private set; }
        public int IdConstCode { get;  set; }
        public int type{ get;  set; }
    }

}

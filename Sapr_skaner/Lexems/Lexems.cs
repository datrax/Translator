﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skaner
{
    public class Lexems
    {
        public Lexems()
        {
            RowNumber = 0;
            LexemNumber = 0;
            LexemName = "";
            LexemCode = 0;
            IdConstCode = 0;
            type = 0;
        }
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

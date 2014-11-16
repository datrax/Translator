using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class MyException : Exception
    {
        public string Message;
        public MyException(string Message)
        {
            this.Message = Message;
        }
    }
}

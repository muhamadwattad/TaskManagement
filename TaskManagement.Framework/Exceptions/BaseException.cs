using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Framework.Exceptions
{
    public class BaseException : Exception
    {
        public string Key { get; private set; }

        public string[] Args { get; private set; }
        public BaseException(string key, string message = "", params string[] args) : base(message)
        {
            Key = key;
            Args = args;
        }
    }
}

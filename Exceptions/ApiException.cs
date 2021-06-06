using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooManager.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(string code, string message) : base(message)
        {
            Code = code;
        }

        public string Code { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain.Core.Exceptions
{

    public class CommonException : Exception
    {
        public CommonException() { }

        public CommonException(String message)
            : base(message)
        {
        }

        public CommonException(String message, Exception inner)
            : base(message, inner)
        {

        }

        public CommonException(String message, Exception inner, EnumEx enumException)
            : base(message, inner)
        {
            EnumException = enumException;
        }
        public string BusinessMessage { get; set; }
        public EnumEx EnumException { get; set; }

        public enum EnumEx
        {
            Error = 0,
            Warning = 1
        }
    }
}

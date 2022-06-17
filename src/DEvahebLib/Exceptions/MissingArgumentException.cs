using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEvahebLib.Exceptions
{
    public class MissingArgumentException : Exception
    {
        public MissingArgumentException(string? message)
            : this(message, innerException: null)
        {
        }

        public MissingArgumentException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}

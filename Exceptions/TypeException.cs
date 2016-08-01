using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportantClasses.Exceptions
{
    public class TypeException : Exception
    {
        public TypeException(string message = "", Exception innerException = null) : base(message, innerException)
        {
            
        }
    }
}

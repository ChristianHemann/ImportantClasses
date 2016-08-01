using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportantClasses.Exceptions
{
    /// <summary>
    /// This Exception is calles when a type do not match another type
    /// </summary>
    public class TypeException : Exception
    {
        /// <summary>
        /// This Exception is calles when a type do not match another type
        /// </summary>
        /// <param name="message">The Message of the exception</param>
        /// <param name="innerException">the inner exception if present</param>
        public TypeException(string message = "", Exception innerException = null) : base(message, innerException) { }
    }
}

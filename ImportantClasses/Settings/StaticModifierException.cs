using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportantClasses.Exceptions
{
    /// <summary>
    /// This exception is thrown when an object should be static but isnt or vise versa.
    /// </summary>
    public class StaticModifierException : Exception
    {
        /// <summary>
        /// This exception is thrown when an object should be static but isnt or vise versa.
        /// </summary>
        /// <param name="message">The Message of the exception</param>
        /// <param name="innerException">the inner exception if present</param>
        public StaticModifierException(string message, Exception innerException = null) : base(message, innerException) { }
    }
}

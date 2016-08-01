using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportantClasses.Exceptions
{
    /// <summary>
    /// thrown when a key was not found
    /// </summary>
    public class KeyNotFoundException : Exception 
    {
        /// <summary>
        /// used when a key was nor found
        /// </summary>
        /// <param name="message">the description of the exception</param>
        /// <param name="innerException">the inner exception if present</param>
        public KeyNotFoundException(string message, Exception innerException = null) :base(message, innerException) { }
    }
}

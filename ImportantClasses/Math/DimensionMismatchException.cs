using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportantClasses.Math
{
    /// <summary>
    /// occurs when the dimensions of a vector does not match the expected number of dimensions
    /// </summary>
    public class DimensionMismatchException : Exception
    {
        /// <summary>
        /// occurs when the dimensions of a vector does not match the expected number of dimensions
        /// </summary>
        /// <param name="message">the error message</param>
        /// <param name="innerException">the possible exception thrown earlier</param>
        public DimensionMismatchException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}

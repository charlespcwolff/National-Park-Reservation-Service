using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Exceptions
{
    /// <summary>
    /// Specifies that passwords do not match each other
    /// </summary>
    public class PasswordMatchException : Exception
    {
        /// <summary>
        /// Constructor needed to create custom exception
        /// </summary>
        /// <param name="message">Custom error message for the exception</param>
        public PasswordMatchException(string message = "") : base(message)
        {

        }
    }
}

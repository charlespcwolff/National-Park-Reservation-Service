using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Exceptions
{
    /// <summary>
    /// Specifies that the desired user does exist in the system
    /// </summary>
    public class UserExistsException : Exception
    {
        /// <summary>
        /// Constructor needed to create custom exception
        /// </summary>
        /// <param name="message">Custom error message for the exception</param>
        public UserExistsException(string message = "") : base(message)
        {

        }
    }
}

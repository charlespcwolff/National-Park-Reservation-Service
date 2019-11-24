using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Exceptions
{
    /// <summary>
    /// Specifies that a user does not exist in the system
    /// </summary>
    public class UserDoesNotExistException : Exception
    {
        /// <summary>
        /// Constructor needed to create custom exception
        /// </summary>
        /// <param name="message">Custom error message for the exception</param>
        public UserDoesNotExistException(string message = "") : base(message)
        {

        }
    }
}

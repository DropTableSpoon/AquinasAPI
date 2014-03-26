using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aquinas
{
    /// <summary>
    /// Represents errors that occur due to received data being of the incorrect format.
    /// </summary>
    public class MalformedDataException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the MalformedDataException class.
        /// </summary>
        public MalformedDataException() : base() { }

        /// <summary>
        /// Initializes a new instance of the MalformedDataException class.
        /// </summary>
        public MalformedDataException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the MalformedDataException class.
        /// </summary>
        public MalformedDataException(string message, Exception inner) : base(message, inner) { }
    }
}

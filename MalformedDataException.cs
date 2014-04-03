using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aquinas
{
    /// <summary>
    /// Represents errors that occur due to received data being of the incorrect format.
    /// </summary>
    public class MalformedDataException : ApiException
    {
        /// <summary>
        /// Initializes a new instance of the MalformedDataException class.
        /// </summary>
        public MalformedDataException() : base(ApiExceptionDetails.MalformedData) { }

        /// <summary>
        /// Initializes a new instance of the MalformedDataException class.
        /// </summary>
        public MalformedDataException(string message) : base(message, ApiExceptionDetails.MalformedData) { }

        /// <summary>
        /// Initializes a new instance of the MalformedDataException class.
        /// </summary>
        public MalformedDataException(string message, Exception inner) : base(message, ApiExceptionDetails.MalformedData, inner) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aquinas
{
    /// <summary>
    /// Represents an error thrown due to data provided to the Aquinas API.
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the ApiException class.
        /// </summary>
        public ApiException(ApiExceptionDetails details)
        {
            Details = details;
        }

        /// <summary>
        /// Initialises a new instance of the ApiException class.
        /// </summary>
        public ApiException(string message, ApiExceptionDetails details) : base(message)
        {
            Details = details;
        }

        /// <summary>
        /// Initialises a new instance of the ApiException class.
        /// </summary>
        public ApiException(string message, ApiExceptionDetails details, Exception inner) : base(message, inner)
        {
            Details = details;
        }

        /// <summary>
        /// Represents the specific problem represented by this ApiException.
        /// </summary>
        public ApiExceptionDetails Details
        {
            get;
            private set;
        }
    }

    /// <summary>
    /// Represents the specific problem represented by the ApiException.
    /// </summary>
    public enum ApiExceptionDetails
    {
        BadLogin,
        MalformedData,
        BadHttpStatus,
        NotAuthenticated
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aquinas
{
    /// <summary>
    /// Contains event state for when an API exception is thrown.
    /// </summary>
    public class ApiExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the details of the event.
        /// </summary>
        public ApiExceptionDetails Details
        {
            get;
            private set;
        }

        /// <summary>
        /// Create a new instance of the ApiExceptionEventArgs class with the given exception details.
        /// </summary>
        /// <param name="details">The details of the API exception thrown.</param>
        public ApiExceptionEventArgs(ApiExceptionDetails details)
        {
            Details = details;
        }
    }
}

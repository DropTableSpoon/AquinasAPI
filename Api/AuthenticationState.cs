using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Aquinas.Api
{
    /// <summary>
    /// Represents the asynchronous state of user authentication.
    /// </summary>
    public struct AuthenticationState
    {
        /// <summary>
        /// The asynchronous callback for once the authentication process is complete.
        /// </summary>
        public AsyncCallback Callback;

        /// <summary>
        /// The HttpWebRequest used for sending the authentication request.
        /// </summary>
        public HttpWebRequest Request;

        /// <summary>
        /// Create a new AuthenticationState object.
        /// </summary>
        /// <param name="callback">The asynchronous callback for once the authentication process is complete.</param>
        /// <param name="request">The HttpWebRequest used for sending the authentication request.</param>
        public AuthenticationState(AsyncCallback callback, HttpWebRequest request)
        {
            Callback = callback;
            Request = request;
        }
    }
}

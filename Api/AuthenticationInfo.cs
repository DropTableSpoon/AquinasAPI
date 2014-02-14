using System;
using System.IO;
using System.Net;
using System.Xml.Linq;

namespace Aquinas.Api
{
    /// <summary>
    /// Represents authentication info used to connect to the server.
    /// </summary>
    public struct AuthenticationInfo
    {
        /// <summary>
        /// The long-form admission number.
        /// </summary>
        public string AdmissionNumber
        {
            get;
            private set;
        }

        /// <summary>
        /// The user's password.
        /// </summary>
        public string Password
        {
            get;
            private set;
        }

        /// <summary>
        /// The token GUID used to validate requests.
        /// </summary>
        public Guid Token
        {
            get
            {
                if (Authenticated)
                    return _Token;
                else
                    throw new InvalidOperationException("This AuthenticationInfo object has not yet been authenticated." +
                        "Use the BeginAuthenticate method to authenticate.");
            }
            private set
            {
                _Token = value;
            }
        }

        private Guid _Token;
        private bool Authenticated;

        /// <summary>
        /// Create a new AuthenticationInfo object.
        /// </summary>
        /// <param name="admissionNumber">The long-form admission number.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="token">The token GUID used to validate requests.</param>
        public AuthenticationInfo(string admissionNumber, string password, Guid token)
            : this()
        {
            AdmissionNumber = admissionNumber;
            Password = password;
            Token = token;
        }

        /// <summary>
        /// Create a new AuthenticationInfo object.
        /// </summary>
        /// <param name="admissionNumber">The long-form admission number.</param>
        /// <param name="password">The user's password.</param>
        public AuthenticationInfo(string admissionNumber, string password)
            : this()
        {
            AdmissionNumber = admissionNumber;
            Password = password;
        }

        /// <summary>
        /// Builds the request body in XML form.
        /// </summary>
        /// <returns>The request body to send to the server.</returns>
        private XDocument BuildRequestBody()
        {
            XDocument document = new XDocument();
            XElement rootElement = new XElement("AuthDetails",
                new XElement("AdmissionNo", AdmissionNumber),
                new XElement("Password", Password));
            document.Add(rootElement);
            return document;
        }

        /// <summary>
        /// Begins an asynchronous API authentication request.
        /// </summary>
        /// <param name="callback">The asynchronous callback for the web request</param>
        /// <returns>An XDocument containing the result of the request.</returns>
        public IAsyncResult BeginAuthenticate(AsyncCallback callback)
        {
            HttpWebRequest request = WebRequest.CreateHttp(
                Properties.Resources.AuthenticationUrl);
            request.Method = "POST";
            request.ContentType = "application/xml";
            AuthenticationState state = new AuthenticationState(callback, request); // anonymous class for storing state
            return request.BeginGetRequestStream(AuthenticateWriteAndSend, state);
        }

        /// <summary>
        /// Writes the authentication data to the stream and sends it asynchronously.
        /// </summary>
        /// <param name="result">The IAsyncResult object representing the status of the asynchronous operation.</param>
        private void AuthenticateWriteAndSend(IAsyncResult result)
        {
            AuthenticationState state = ((AuthenticationState)result.AsyncState);
            HttpWebRequest request = state.Request;
            Stream requestStream = request.EndGetRequestStream(result); // Should this be closed or does HttpWebRequest do that itself?
            XDocument requestBody = BuildRequestBody();
            requestBody.Save(requestStream);
            request.BeginGetResponse(state.Callback, request);
        }

        /// <summary>
        /// Ends an asynchronous API request and sets this object's value to the result of the operation.
        /// </summary>
        /// <param name="result">The IAsyncResult object representing the status of the asynchronous operation.</param>
        /// <returns>Returns this object.</returns>
        public AuthenticationInfo EndAuthenticate(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);

            XDocument document = XDocument.Load(response.GetResponseStream());

            XElement tokenElement = document.Element("Token");
            if (tokenElement != null)
            {
                Token = new Guid(tokenElement.Value);
                Authenticated = true;
            }
            else
            {
                throw new NullReferenceException("A token was not received from the server.");
            }
            return this;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
        /// <param name="token">The token GUID used to validate requests.</param>
        public AuthenticationInfo(string admissionNumber, Guid token)
            : this()
        {
            AdmissionNumber = admissionNumber;
            Token = token;
        }

        /// <summary>
        /// Create a new AuthenticationInfo object.
        /// </summary>
        /// <param name="admissionNumber">The long-form admission number.</param>
        public AuthenticationInfo(string admissionNumber)
            : this()
        {
            AdmissionNumber = admissionNumber;
        }

        /// <summary>
        /// Begins an asynchronous API authentication request.
        /// </summary>
        /// <param name="admissionNumber"></param>
        /// <param name="result">The IAsyncResult object representing the status of the asynchronous operation.</param>
        /// <returns>An XDocument containing the result of the request.</returns>
        public IAsyncResult BeginAuthenticate(string admissionNumber, AsyncCallback callback)
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(
                String.Format("https://www.my.aquinas.ac.uk/MobileAPI/api/Student/GetStudentName/{0}",
                            admissionNumber.ToUpper()));
            request.ContentType = "application/xml";

            return request.BeginGetResponse(callback, request);
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

            Token = new Guid(document.Element("Token").Value);
            return this;
        }
    }
}

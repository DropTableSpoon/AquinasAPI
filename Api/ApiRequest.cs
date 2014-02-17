using System;
using System.IO;
using System.Net;
using System.Xml.Linq;

namespace Aquinas.Api
{
    /// <summary>
    /// Represents a request to the MyAquinas API.
    /// </summary>
    public class ApiRequest
    {
        /// <summary>
        /// A web request to be sent to the server.
        /// </summary>
        private HttpWebRequest WebRequest;

        /// <summary>
        /// Create a new API request.
        /// </summary>
        /// <param name="authInfo">The authentication info to send to the server.</param>
        /// <param name="requestPath">The API request path. You can use the constants given in <see cref="ApiRequest"/>.</param>
        public ApiRequest(AuthenticationInfo authInfo, string requestPath)
        {
            if (!authInfo.Authenticated)
            {
                throw new InvalidOperationException("The AuthenticationInfo object must have been authenticated before usage in an API request.");
            }

            WebRequest = System.Net.WebRequest.CreateHttp(
                CreateApiUrl(requestPath));
            WebRequest.ContentType = "application/xml";
            WebRequest.Headers["AuthToken"] = authInfo.Token.ToString();
        }

        /// <summary>
        /// Begins an asynchronous API request.
        /// </summary>
        /// <param name="callback">The asynchronous callback to call upon completion of the request.</param>
        /// <returns>Returns the status of the asynchronous operation.</returns>
        public IAsyncResult BeginApiRequest(AsyncCallback callback)
        {
            return WebRequest.BeginGetResponse(callback, this);
        }

        /*
        /// <summary>
        /// Writes the API request data to the stream and sends it asynchronously.
        /// </summary>
        /// <param name="result">The IAsyncResult object representing the status of the asynchronous operation.</param>
        private void ApiRequestWriteAndSend(IAsyncResult result)
        {
            AuthenticationState state = ((AuthenticationState)result.AsyncState);
            HttpWebRequest request = state.Request;
            Stream requestStream = request.EndGetRequestStream(result); // Should this be closed or does HttpWebRequest do that itself?
            XDocument requestBody = BuildRequestBody();
            requestBody.Save(requestStream);
            request.BeginGetResponse(state.Callback, this);
        }
        */

        /// <summary>
        /// Ends an asynchronous API request.
        /// </summary>
        /// <param name="result">The IAsyncResult object representing the status of the asynchronous operation.</param>
        /// <returns>An XDocument containing the result of the request.</returns>
        public XDocument EndApiRequest(IAsyncResult result)
        {
            ApiRequest apiRequest = (ApiRequest)result.AsyncState;
            HttpWebResponse response = (HttpWebResponse)apiRequest.WebRequest.EndGetResponse(result);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                XDocument document = XDocument.Load(response.GetResponseStream());
                return document;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Invalid authentication token.");
            }
            else
            {
                throw new Exception("Unexpected status: " + response.StatusDescription);
            }
        }

        /// <summary>
        /// Creates a valid API URL.
        /// </summary>
        /// <param name="requestPath">The API request path. You can use the constants given in <see cref="ApiRequest"/>.</param>
        /// <returns></returns>
        private string CreateApiUrl(string requestPath)
        {
            return String.Format(Properties.Resources.ApiTemplateUrl,
                requestPath);
        }

        /// <summary>
        /// An API request to get basic student details.
        /// </summary>
        public const string GetStudentDetails = "Student/GetStudentDetails";
    }
}

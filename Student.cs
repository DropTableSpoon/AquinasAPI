using System;
using System.Linq;
using System.Xml.Linq;
using Aquinas.Api;

namespace Aquinas
{
    /// <summary>
    /// Represents an Aquinas student.
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Raised when this student is authenticated with the Aquinas server.
        /// </summary>
        public event EventHandler<StudentUpdateEventArgs> Authenticated;

        /// <summary>
        /// Raised when this student's name is loaded.
        /// </summary>
        public event EventHandler<StudentUpdateEventArgs> StudentNameLoaded;

        /// <summary>
        /// Raised when this student's timetable data is loaded.
        /// </summary>
        public event EventHandler<StudentUpdateEventArgs> TimetableDataLoaded;

        /// <summary>
        /// The first (chosen) name of the student.
        /// </summary>
        public string FirstName
        {
            get;
            private set;
        }

        /// <summary>
        /// The full name of the student.
        /// </summary>
        public string FullName
        {
            get;
            private set;
        }

        /// <summary>
        /// The short-form Aquinas number (AQ######) of the student.
        /// </summary>
        public string AquinasNumber
        {
            get;
            private set;
        }

        /// <summary>
        /// The long-form Aquinas number (AQ##0000####) of the student.
        /// </summary>
        public string AdmissionNumber
        {
            get;
            private set;
        }

        /// <summary>
        /// The authentication info for this student.
        /// </summary>
        private AuthenticationInfo AuthInfo;

        public XElement[] Timetable;

        /// <summary>
        /// Initialises a new Student object.
        /// </summary>
        /// <param name="aquinasNumber">The short-form Aquinas number of the student.</param>
        public Student(string aquinasNumber)
        {
            aquinasNumber = aquinasNumber.ToUpper();
            AdmissionNumber = GetAdmissionNumber(aquinasNumber);
        }

        /// <summary>
        /// Authenticates the user with the given password.
        /// This will start an asynchronous authentication request.
        /// </summary>
        /// <param name="password">The user's login password.</param>
        public void Authenticate(string password)
        {
            AuthInfo = new AuthenticationInfo(AdmissionNumber, password);
            AuthInfo.BeginAuthenticate(AuthenticationCallback);
        }

        /// <summary>
        /// The asynchronous callback method for authentication.
        /// </summary>
        /// <param name="result">The status of the asynchronous operation.</param>
        private void AuthenticationCallback(IAsyncResult result)
        {
            AuthInfo.EndAuthenticate(result);
            Authenticated.Raise(this, new StudentUpdateEventArgs(this));

            // Get student name
            ApiRequest basicInfoRequest = new ApiRequest(AuthInfo, ApiRequest.GetStudentName);
            basicInfoRequest.BeginApiRequest(BasicInfoCallback);

            // Get timetable data
            ApiRequest timetableRequest = new ApiRequest(AuthInfo, ApiRequest.GetTimetableData);
            timetableRequest.BeginApiRequest(TimetableCallback);
        }

        /// <summary>
        /// The asynchronous callback method for getting student timetable data.
        /// </summary>
        /// <param name="result">The status of the asynchronous operation.</param>
        private void TimetableCallback(IAsyncResult result)
        {
            XDocument basicInfoDocument = ((ApiRequest)result.AsyncState).EndApiRequest(result);
            XElement timetableInfo = basicInfoDocument.Element(XName.Get("ArrayOfTimetableSession", Properties.Resources.XmlNamespace));
            if (timetableInfo != null)
            {
                Timetable = timetableInfo.Elements(XName.Get("TimetableSession", Properties.Resources.XmlNamespace)).ToArray();
                TimetableDataLoaded.Raise(this, new StudentUpdateEventArgs(this));
            }
            else
            {
                // badly formed XML data
                throw new NullReferenceException(Properties.Resources.ExceptionMalformedXml);
            }
        }

        /// <summary>
        /// The asynchronous callback method for getting basic student info.
        /// </summary>
        /// <param name="result">The status of the asynchronous operation.</param>
        private void BasicInfoCallback(IAsyncResult result)
        {
            XDocument basicInfoDocument = ((ApiRequest)result.AsyncState).EndApiRequest(result);
            XElement basicInfo = basicInfoDocument.Element(XName.Get("StudentName", Properties.Resources.XmlNamespace));
            if (basicInfo != null)
            {
                XElement chosenName = basicInfo.Element(XName.Get("ChosenName", Properties.Resources.XmlNamespace));
                if (chosenName != null)
                {
                    FirstName = chosenName.Value;
                    StudentNameLoaded.Raise(this, new StudentUpdateEventArgs(this));
                }
                else
                {
                    // student name not present in data
                    throw new NullReferenceException(Properties.Resources.ExceptionStudentNameNotPresent);
                }
            }
            else
            {
                // badly formed XML data
                throw new NullReferenceException(Properties.Resources.ExceptionMalformedXml);
            }
        }

        /// <summary>
        /// Convert a short-form Aquinas number into a long-form admission number.
        /// </summary>
        /// <param name="aquinasNumber">The short-form Aquinas number to convert.</param>
        /// <returns>The equivalent long-form Aquinas number.</returns>
        private static string GetAdmissionNumber(string aquinasNumber)
        {
            return aquinasNumber.Insert(4, "0000").ToUpper();
        }
    }
}

using System;
using System.Linq;
using System.Xml.Linq;
using Aquinas.Api;
using Aquinas.Timetable;

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
        /// Raised when this student's basic details is loaded.
        /// </summary>
        public event EventHandler<StudentUpdateEventArgs> StudentDetailsLoaded;

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
        /// The surname of the student.
        /// </summary>
        public string Surname
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

        public string Forenames
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
        /// The timetable of the student.
        /// </summary>
        public CollegeTimetable<StudentCollegeDay> Timetable
        {
            get;
            private set;
        }

        /// <summary>
        /// The authentication info for this student.
        /// </summary>
        public AuthenticationInfo AuthInfo;

        /// <summary>
        /// Initialises a new Student object.
        /// </summary>
        /// <param name="aquinasNumber">The short-form Aquinas number of the student.</param>
        public Student(string aquinasNumber)
        {
            AquinasNumber = aquinasNumber.ToUpper();
            AdmissionNumber = GetAdmissionNumber(AquinasNumber);
        }

        /// <summary>
        /// Initialises a new Student object with the given token
        /// </summary>
        /// <param name="aquinasNumber">The short-form Aquinas number of the student.</param>
        /// <param name="token">The student's Authentication Token.</param>
        public Student(string aquinasNumber, Guid token)
        {
            AquinasNumber = aquinasNumber.ToUpper();
            AdmissionNumber = GetAdmissionNumber(AquinasNumber);
            AuthInfo = new AuthenticationInfo(AdmissionNumber, null, token);
        }

        /// <summary>
        /// Begins an Async request to get the student's details
        /// </summary>
        public void GetDetails()
        {
            ApiRequest basicInfoRequest = new ApiRequest(AuthInfo, ApiRequest.GetStudentDetails);
            basicInfoRequest.BeginApiRequest(BasicInfoCallback);
        }

        /// <summary>
        /// Begins an Async request to get the student's Timetable
        /// </summary>
        public void GetTimetable()
        {
            ApiRequest timetableRequest = new ApiRequest(AuthInfo, ApiRequest.GetTimetableData);
            timetableRequest.BeginApiRequest(TimetableCallback);
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

            // Get student details
            GetDetails();

            // Get timetable data
            GetTimetable();
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
                XElement[] timetableSessions = timetableInfo
                    .Elements(XName.Get("TimetableSession", Properties.Resources.XmlNamespace))
                    .ToArray();
                Timetable = new CollegeTimetable<StudentCollegeDay>();
                Timetable.AddLessons(timetableSessions);
                TimetableDataLoaded.Raise(this, new StudentUpdateEventArgs(this));
            }
            else
            {
                // badly formed XML data
                throw new MalformedDataException(Properties.Resources.ExceptionMalformedXml);
            }
        }

        /// <summary>
        /// The asynchronous callback method for getting basic student info.
        /// </summary>
        /// <param name="result">The status of the asynchronous operation.</param>
        private void BasicInfoCallback(IAsyncResult result)
        {
            XDocument basicInfoDocument = ((ApiRequest)result.AsyncState).EndApiRequest(result);
            XElement basicInfo = basicInfoDocument.Element(XName.Get("StudentDetails", Properties.Resources.XmlNamespace));
            if (basicInfo != null)
            {
                try
                {
                    XElement chosenName = basicInfo.Element(XName.Get("ChosenName", Properties.Resources.XmlNamespace));
                    FirstName = chosenName.Value.Trim();

                    XElement forenames = basicInfo.Element(XName.Get("Forename", Properties.Resources.XmlNamespace));
                    Forenames = forenames.Value.Trim();

                    XElement lastName = basicInfo.Element(XName.Get("Surname", Properties.Resources.XmlNamespace));
                    Surname = lastName.Value.Trim();

                    FullName = String.Format("{0} {1}", Forenames, Surname);
                    StudentDetailsLoaded.Raise(this, new StudentUpdateEventArgs(this));
                }
                catch (NullReferenceException ex)
                {
                    throw new MalformedDataException(Properties.Resources.ExceptionDataNotPresent, ex);
                    /* Merged the error handling into one try-catch block because if one part of the data
                     * is missing, the rest most likely will be too (unless the API changes in which case
                     * we know what the issue is anyway) so it should not cause any problems */
                }
            }
            else
            {
                // badly formed XML data
                throw new MalformedDataException(Properties.Resources.ExceptionMalformedXml);
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

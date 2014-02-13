using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        /// <summary>
        /// Initialises a new Student object and authenticates.
        /// </summary>
        /// <param name="aquinasNumber">The short-form Aquinas number of the student.</param>
        public Student(string aquinasNumber)
        {
            aquinasNumber = aquinasNumber.ToUpper();
            AdmissionNumber = GetAdmissionNumber(aquinasNumber);
            AuthInfo.BeginAuthenticate(AdmissionNumber, AuthenticationCallback);
        }

        /// <summary>
        /// The asynchronous callback method for authentication.
        /// </summary>
        /// <param name="result">The status of the asynchronous operation.</param>
        private void AuthenticationCallback(IAsyncResult result)
        {
            AuthInfo.EndAuthenticate(result);
            ApiRequest basicInfoRequest = new ApiRequest(AuthInfo, ApiRequest.GetStudentDetails);
            basicInfoRequest.BeginApiRequest(BasicInfoCallback);
        }

        /// <summary>
        /// The asynchronous callback method for getting basic student info.
        /// </summary>
        /// <param name="result">The status of the asynchronous operation.</param>
        private void BasicInfoCallback(IAsyncResult result)
        {
            XDocument basicInfoDocument = ((ApiRequest)result.AsyncState).EndApiRequest(result);
            XElement basicInfo = basicInfoDocument.Root;
            FirstName = basicInfo.Element("stu_chosen_name").Value;
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

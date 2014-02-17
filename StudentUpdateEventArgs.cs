using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aquinas
{
    /// <summary>
    /// Contains event state for when student details are updated.
    /// </summary>
    public class StudentUpdateEventArgs : EventArgs
    {
        /// <summary>
        /// The Student for which the update took place.
        /// </summary>
        public Student Student
        {
            get;
            private set;
        }

        /// <summary>
        /// Create a new StudentUpdateEventArgs object with the given student.
        /// </summary>
        /// <param name="student">The Student for which the update took place.</param>
        public StudentUpdateEventArgs(Student student)
        {
            Student = student;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aquinas.Timetable
{
    /// <summary>
    /// Contains the lessons to be attended by a student.
    /// </summary>
    public class StudentCollegeDay : CollegeDay
    {
        /// <summary>
        /// Gets the lesson in first period.
        /// </summary>
        public Lesson Period1
        {
            get
            {
                return StudentLessons.ContainsKey("1") ?
                    StudentLessons["1"] : Lesson.Empty;
            }
        }

        /// <summary>
        /// Gets the lesson in second period.
        /// </summary>
        public Lesson Period2
        {
            get
            {
                return StudentLessons.ContainsKey("2") ?
                    StudentLessons["2"] : Lesson.Empty;
            }
        }

        /// <summary>
        /// Gets the registration time information.
        /// </summary>
        public Lesson PeriodRegistration
        {
            get
            {
                return StudentLessons.ContainsKey("Registration") ?
                    StudentLessons["Registration"] : Lesson.Empty;
            }
        }

        /// <summary>
        /// Gets the lesson in third period.
        /// </summary>
        public Lesson Period3
        {
            get
            {
                return StudentLessons.ContainsKey("3") ?
                    StudentLessons["3"] : Lesson.Empty;
            }
        }

        /// <summary>
        /// Gets the lesson in fourth period.
        /// </summary>
        public Lesson Period4
        {
            get
            {
                return StudentLessons.ContainsKey("4") ?
                    StudentLessons["4"] : Lesson.Empty;
            }
        }

        /// <summary>
        /// Gets the lesson in fifth period.
        /// </summary>
        public Lesson Period5
        {
            get
            {
                return StudentLessons.ContainsKey("5") ?
                    StudentLessons["5"] : Lesson.Empty;
            }
        }

        /// <summary>
        /// Gets the lesson in sixth period.
        /// </summary>
        public Lesson Period6
        {
            get
            {
                return StudentLessons.ContainsKey("6") ?
                    StudentLessons["6"] : Lesson.Empty;
            }
        }

        /// <summary>
        /// Gets the lesson in seventh period.
        /// </summary>
        public Lesson Period7
        {
            get
            {
                return StudentLessons.ContainsKey("7") ?
                    StudentLessons["7"] : Lesson.Empty;
            }
        }

        /// <summary>
        /// Gets the lesson in eighth period.
        /// </summary>
        public Lesson Period8
        {
            get
            {
                return StudentLessons.ContainsKey("8") ?
                    StudentLessons["8"] : Lesson.Empty;
            }
        }
    }
}

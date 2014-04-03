using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aquinas.Timetable
{
    /// <summary>
    /// Contains the lessons to be attended by a user.
    /// </summary>
    public class CollegeDay : IEnumerable<Lesson>
    {
        /// <summary>
        /// Holds all of the user's lessons for this day.
        /// </summary>
        protected Dictionary<string, Lesson> StudentLessons = new Dictionary<string,Lesson>();

        /// <summary>
        /// Adds a lesson to this day.
        /// </summary>
        /// <param name="period">The period the lesson occurs in.</param>
        /// <param name="lesson">The lesson data.</param>
        public void AddLesson(string period, Lesson lesson)
        {
            lesson.Period = period;
            StudentLessons.Add(period == "2R" ? "Registration" : period, lesson);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return StudentLessons.Values.GetEnumerator();
        }

        IEnumerator<Lesson> IEnumerable<Lesson>.GetEnumerator()
        {
            return StudentLessons.Values.GetEnumerator();
        }
    }
}

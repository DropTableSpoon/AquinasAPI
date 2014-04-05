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

        /// <summary>
        /// Gets a lesson, by period, from this timetable.
        /// If the lesson is not present in the timetable, an Empty (free) lesson is returned.
        /// </summary>
        /// <param name="lesson">The string representing the period of the lesson.</param>
        /// <returns>The lesson in the timetable with the given period.</returns>
        public Lesson this[string lesson]
        {
            get
            {
                return StudentLessons.ContainsKey(lesson) ? StudentLessons[lesson] : Lesson.Empty;
            }
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

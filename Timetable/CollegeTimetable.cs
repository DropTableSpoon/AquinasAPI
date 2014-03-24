using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Aquinas.Timetable
{
    /// <summary>
    /// Represents a college timetable.
    /// </summary>
    /// <typeparam name="TDay">The type of day the user has - usually StudentCollegeDay.</typeparam>
    public class CollegeTimetable<TDay> where TDay : CollegeDay, new()
    {
        /// <summary>
        /// All days in the college week.
        /// </summary>
        private Dictionary<DayOfWeek, TDay> Days;

        /// <summary>
        /// Create a new instance of the CollegeTimetable class.
        /// </summary>
        public CollegeTimetable()
        {
            Days = new Dictionary<DayOfWeek, TDay>();
            Days.Add(DayOfWeek.Monday, new TDay());
            Days.Add(DayOfWeek.Tuesday, new TDay());
            Days.Add(DayOfWeek.Wednesday, new TDay());
            Days.Add(DayOfWeek.Thursday, new TDay());
            Days.Add(DayOfWeek.Friday, new TDay());
        }

        /// <summary>
        /// Gets or sets the timetable for a given day.
        /// </summary>
        /// <param name="day">Which day to get or set the timetable for.</param>
        public TDay this[DayOfWeek day]
        {
            get
            {
                return Days.ContainsKey(day) ? Days[day] : null;
            }
            set
            {
                Days[day] = value;
            }
        }

        /// <summary>
        /// Adds a lesson to a timetable.
        /// </summary>
        /// <param name="session">The timetable session data.</param>
        public void AddLesson(XElement session)
        {
            DayOfWeek dayOfWeek = (DayOfWeek)Int32.Parse(session.Element(session.Name.Namespace + "Day").Value);
            string period = session.Element(session.Name.Namespace + "Period").Value;
            Lesson lesson = new Lesson(
                session.Element(session.Name.Namespace + "ClassCode").Value,
                session.Element(session.Name.Namespace + "Description").Value,
                session.Element(session.Name.Namespace + "Room").Value,
                session.Element(session.Name.Namespace + "Tutor").Value);
            this[dayOfWeek].AddLesson(period, lesson);
        }

        /// <summary>
        /// Adds several lessons to a timetable.
        /// </summary>
        /// <param name="sessions">The data for the timetable sessions.</param>
        public void AddLessons(IEnumerable<XElement> sessions)
        {
            foreach (XElement session in sessions)
            {
                AddLesson(session);
            }
        }
    }
}

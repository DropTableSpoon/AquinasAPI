using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aquinas.Timetable
{
    /// <summary>
    /// Represents a lesson to be attended by a student.
    /// </summary>
    public struct Lesson
    {
        /// <summary>
        /// Gets the class code of this lesson, in the format XX-XX-XX-XX.
        /// </summary>
        public string ClassCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the description of the lesson.
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the room that the lesson is in.
        /// </summary>
        public string Room
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the tutor of the lesson.
        /// </summary>
        public string Tutor
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets whether this is a free period or not.
        /// </summary>
        public bool Free
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets which period this lesson is in.
        /// </summary>
        public string Period
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new instance of the Lesson structure.
        /// </summary>
        /// <param name="classCode">The class code of this lesson, in the format XX-XX-XX-XX.</param>
        /// <param name="description">The description of the lesson.</param>
        /// <param name="room">The room that the lesson is in.</param>
        /// <param name="tutor">The name of the tutor of the lesson.</param>
        public Lesson(string classCode, string description, string room, string tutor)
            : this()
        {
            ClassCode = classCode;
            Description = description.Split(';').First();
            Room = room;
            Tutor = tutor;
        }
        
        public override string ToString()
        {
            return Free ?
                "Free" :
                String.Format("{0} with {1} in {2}", Description, Tutor, Room);
        }

        /// <summary>
        /// An empty lesson (lunch, study, etc.)
        /// </summary>
        public static Lesson Empty
        {
            get
            {
                return new Lesson("-", "Free", "-", "-") { Free = true, Period = "-" };
            }
        }
    }
}

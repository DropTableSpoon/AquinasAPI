using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aquinas
{
    /// <summary>
    /// A static container class for extension methods for the EventHandler&lt;T&gt; class.
    /// </summary>
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Raises the event in a thread-safe manner.
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="argument">The argument to send to the event handler.</param>
        public static void Raise<T>(this EventHandler<T> theEvent, object sender, T argument) where T : EventArgs
        {
            if (theEvent != null)
            {
                theEvent(sender, argument);
            }
        }
    }
}

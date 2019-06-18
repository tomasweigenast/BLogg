using BLogg.Core.Events;

namespace BLogg.Core.Formatters
{
    /// <summary>
    /// Base class that defines an event formatter
    /// </summary>
    public interface IEventFormatter
    {
        /// <summary>
        /// Applies format to the event
        /// </summary>
        /// <param name="logEvent">The log event</param>
        string Format(LogEvent logEvent);
    }
}

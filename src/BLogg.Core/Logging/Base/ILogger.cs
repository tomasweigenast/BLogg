using BLogg.Core.Events;

namespace BLogg.Core.Logging.Base
{
    /// <summary>
    /// Base class for each logger implementation
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Process a log event
        /// </summary>
        /// <param name="logEvent">The log event to process</param>
        void ProcessEvent(LogEvent logEvent);
    }
}

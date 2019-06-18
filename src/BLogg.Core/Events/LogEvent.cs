using BLogg.Core.Attributes;
using BLogg.Core.Diagnostics;
using System;

namespace BLogg.Core.Events
{
    /// <summary>
    /// Defines an event that happened an it must be logged
    /// </summary>
    public class LogEvent
    {
        #region Public Properties

        /// <summary>
        /// The level of the event
        /// </summary>
        [Placeholder("LogLevel")]
        public LogLevel Level { get; }

        /// <summary>
        /// The message of the event
        /// </summary>
        [Placeholder("Message")]
        public string Message { get; }

        /// <summary>
        /// The date and time when the event was logged
        /// </summary>
        [Placeholder("Timestamp")]
        public DateTime FireDate { get; }

        /// <summary>
        /// Details of the current call
        /// </summary>
        [Placeholder("CallDiag")]
        public CallDiagnostic CallDiagnostics { get; }

        /// <summary>
        /// Any exception to log
        /// </summary>
        [Placeholder("Ex")]
        public Exception Exception { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LogEvent(LogLevel level, string message, DateTime fireDate, CallDiagnostic diagnostics, Exception exception)
        {
            Level = level;
            Message = message;
            FireDate = fireDate;
            CallDiagnostics = diagnostics;
            Exception = exception;
        }

        #endregion
    }
}

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
        public LogLevel Level { get; }

        /// <summary>
        /// The message of the event
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// The date and time when the event was logged
        /// </summary>
        public DateTime FireDate { get; }

        /// <summary>
        /// Details of the current call
        /// </summary>
        public CallDiagnostic CallDiagnostics { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LogEvent(LogLevel level, string message, DateTime fireDate, CallDiagnostic diagnostics)
        {
            Level = level;
            Message = message;
            FireDate = fireDate;
            CallDiagnostics = diagnostics; 
        }

        #endregion
    }
}

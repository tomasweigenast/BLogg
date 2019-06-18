using BLogg.Core.Diagnostics;
using BLogg.Core.Events;
using BLogg.Core.Logging.Base;
using BLogg.Core.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace BLogg.Core.Logging
{
    /// <summary>
    /// Provides methods for logging messages in the entire application
    /// </summary>
    public sealed class Logger : ILogger
    {
        #region Private Members

        private IList<ILogProcessor> mLogProcessors;
        private LogLevel mDefaultLogLevel;

        #endregion

        #region Constructors

        /// <summary>
        /// Make lazy
        /// </summary>
        static Logger() { }

        /// <summary>
        /// Default internal constructor
        /// </summary>
        internal Logger(ILogProcessor[] processors, LogLevel logLevel)
        {
            mLogProcessors = new List<ILogProcessor>(processors);
            mDefaultLogLevel = logLevel;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Logs a message with the default log level
        /// </summary>
        /// <param name="message">The message to log</param>
        public void Log(string message)
            => Log(message, mDefaultLogLevel);

        /// <summary>
        /// Logs a message with a defined log level
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="level">The level to use</param>
        public void Log(string message, LogLevel level)
            => Log(message, level, null);

        /// <summary>
        /// Logs a message with a defined log level and an exception
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="level">The level to use</param>
        public void Log(string message, LogLevel level, Exception exception)
        {
            // Get event
            var logEvent = GetLogEvent(message, level, exception);

            // TODO: Apply format to the message

            // Process the log event
            foreach (var processor in mLogProcessors)
                processor.Process(logEvent);
        }

        #endregion

        #region Private Helper Methods

        // Gets a log event
        LogEvent GetLogEvent(string message, LogLevel level, Exception exception)
        {
            // Get current stackframe
            StackFrame stackFrame = null;
            StackTrace stackTrace = new StackTrace();
            for (int i = 0; i < stackTrace.GetFrames().Length; i++)
            {
                var frameBase = stackTrace.GetFrame(i).GetMethod();
                var name = MethodBase.GetCurrentMethod().Name;

                if (frameBase.Name != "Log" && frameBase.Name != name)
                    stackFrame = new StackFrame(i, true);
            }

            // Create call diagnostic
            CallDiagnostic diagnostic = new CallDiagnostic(Thread.CurrentThread.ManagedThreadId, Process.GetCurrentProcess().Id, stackFrame);

            // Create log event
            LogEvent logEvent = new LogEvent(level, message, DateTime.Now, diagnostic, exception);

            // Return the log event
            return logEvent;
        }

        /// <summary>
        /// Process a log event
        /// </summary>
        /// <param name="logEvent">The log event to process</param>
        void ILogger.ProcessEvent(LogEvent logEvent)
        {
            // Process the message for each processor
            foreach(var processor in mLogProcessors)
                processor.Process(logEvent);
        }

        #endregion
    }
}

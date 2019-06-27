using BLogg.Core.Diagnostics;
using BLogg.Core.Events;
using BLogg.Core.Logging.Base;
using BLogg.Core.Processing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
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
            mLogProcessors = processors;
            mDefaultLogLevel = logLevel;
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// A global instance of a logger
        /// </summary>
        public static Logger Glob { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Logs a message with the default log level
        /// </summary>
        /// <param name="message">The message to log</param>
        public void Log(string message) => Log(message, mDefaultLogLevel);

        /// <summary>
        /// Logs a message formatting properties
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="message">The message</param>
        /// <param name="properties">The properties</param>
        public void Log(LogLevel level, string message, params object[] properties)
        {
            // Get properties of the message using regex
            var messagePropertiesMatches = Regex.Matches(message, @"(?<=\{)[^}]*(?=\})");
            List<string> messageProperties = new List<string>(); // List to store properties

            // Get each property
            foreach (Match msgPropMatch in messagePropertiesMatches)
                messageProperties.Add(msgPropMatch.Value);

            string finalMessage = message;

            // Foreach property
            for (int i = 0; i < messageProperties.Count; i++)
            {
                string messageProperty = messageProperties[i];

                // If the property starts with a "!", use json to serialize the content of the class
                if (messageProperty.StartsWith("!"))
                    finalMessage = finalMessage.Replace($"{{{messageProperties[i]}}}", JsonConvert.SerializeObject(properties[i], Formatting.None));

                // Otherwise, call ToString method of the class
                else
                    finalMessage = finalMessage.Replace($"{{{messageProperties[i]}}}", messageProperty.ToString());
            }

            // Log the message
            Log(finalMessage, level);
        }

        /// <summary>
        /// Logs a message with a defined log level
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="level">The level to use</param>
        public void Log(string message, LogLevel level) => Log(message, level, null);

        /// <summary>
        /// Logs a message with the log level Information
        /// </summary>
        /// <param name="message">The message to log</param>
        public void LogInformation(string message) => Log(message, LogLevel.Info);

        /// <summary>
        /// Logs a message with the log level Information
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="properties">The properties to format</param>
        public void LogInformation(string message, params object[] properties) => Log(LogLevel.Info, message, properties);

        /// <summary>
        /// Logs a message with the log level Debug
        /// </summary>
        /// <param name="message">The message to log</param>
        public void LogDebug(string message) => Log(message, LogLevel.Debug);

        /// <summary>
        /// Logs a message with the log level Debug
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        public void LogDebug(string message, Exception exception) => Log(message, LogLevel.Debug, exception);

        /// <summary>
        /// Logs a message with the log level Debug
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="properties">The properties to format</param>
        public void LogDebug(string message, params object[] properties) => Log(LogLevel.Debug, message, properties);

        /// <summary>
        /// Logs a message with the log level Error
        /// </summary>
        /// <param name="message">The message to log</param>
        public void LogError(string message) => Log(message, LogLevel.Error);

        /// <summary>
        /// Logs a message with the log level Error
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="properties">The properties to format</param>
        public void LogError(string message, params object[] properties) => Log(LogLevel.Error, message, properties);

        /// <summary>
        /// Logs a message with the log level Error
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        public void LogError(string message, Exception exception) => Log(message, LogLevel.Error, exception);

        /// <summary>
        /// Logs a message with the log level Fatal
        /// </summary>
        /// <param name="message">The message to log</param>
        public void LogFatal(string message) => Log(message, LogLevel.Fatal);

        /// <summary>
        /// Logs a message with the log level Fatal
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="properties">The properties to format</param>
        public void LogFatal(string message, params object[] properties) => Log(LogLevel.Fatal, message, properties);

        /// <summary>
        /// Logs a message with the log level Fatal
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        public void LogFatal(string message, Exception exception) => Log(message, LogLevel.Fatal, exception);

        /// <summary>
        /// Logs a message with the log level Warning
        /// </summary>
        /// <param name="message">The message to log</param>
        public void LogWarning(string message) => Log(message, LogLevel.Warning);

        /// <summary>
        /// Logs a message with the log level Warning
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="properties">The properties to format</param>
        public void LogWarning(string message, params object[] properties) => Log(LogLevel.Warning, message, properties);

        /// <summary>
        /// Logs a message with the log level Warning
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        public void LogWarning(string message, Exception exception) => Log(message, LogLevel.Warning, exception);

        /// <summary>
        /// Logs a message with a defined log level and an exception
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="level">The level to use</param>
        internal void Log(string message, LogLevel level, Exception exception)
        {
            // Get event
            var logEvent = GetLogEvent(message, level, exception);

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

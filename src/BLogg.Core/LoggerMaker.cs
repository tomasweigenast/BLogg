using BLogg.Core.Events;
using BLogg.Core.Logging;
using BLogg.Core.Logging.Base;
using BLogg.Core.Logging.Configuration;

namespace BLogg.Core
{
    /// <summary>
    /// Provides methods to create a new <see cref="ILogger"/>
    /// </summary>
    public sealed class LoggerMaker
    {
        #region Private Members

        private LogLevel mDefaultLogLevel = LogLevel.Info; // The default log level to use

        #endregion

        #region Constructors

        /// <summary>
        /// Lazy constructor
        /// </summary>
        static LoggerMaker() { }

        /// <summary>
        /// Default internal constructor
        /// </summary>
        internal LoggerMaker()
        {
            WithProcessor = new LoggerProcessorsConfiguration(this);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Provides methods to implement new logger processors
        /// </summary>
        public LoggerProcessorsConfiguration WithProcessor { get; }

        #endregion

        #region Methods

        #region Static

        /// <summary>
        /// Creates a new logger
        /// </summary>
        public static LoggerMaker MakeNew() => new LoggerMaker();

        #endregion

        /// <summary>
        /// Specifies the logger to use a default log level when the logging function is called.
        /// If this is not set, the default log level is Info
        /// </summary>
        /// <param name="logLevel">The default log level to set</param>
        public LoggerMaker WithDefaultLogLevel(LogLevel logLevel)
        {
            mDefaultLogLevel = logLevel;

            return this;
        }

        /// <summary>
        /// Builds a logger with the custom settings
        /// </summary>
        public Logger Build()
        {
            Logger logger = new Logger(WithProcessor.GetProcessors(), mDefaultLogLevel);

            return logger;
        }

        #endregion
    }
}

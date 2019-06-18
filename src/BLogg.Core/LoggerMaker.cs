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
            Processors = new LoggerProcessorsConfiguration(this);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Provides methods to implement new logger processors
        /// </summary>
        public LoggerProcessorsConfiguration Processors { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new logger
        /// </summary>
        public static LoggerMaker MakeNew()
            => new LoggerMaker();

        #endregion
    }
}

using BLogg.Core.Events;
using BLogg.Core.Logging.Configuration;
using System;
using System.IO;

namespace BLogg.Core.Processing.BuiltIn
{
    /// <summary>
    /// Processor that handles log events and writes them to the default Console output
    /// </summary>
    public class ConsoleProcessor : ILogProcessor<ConsoleProcessorSettings>
    {
        #region Constructors

        /// <summary>
        /// Creates a new <see cref="ConsoleProcessor"/>
        /// </summary>
        public ConsoleProcessor() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// The configuration for the processor if needed
        /// </summary>
        public ConsoleProcessorSettings Configuration { get; set; }

        #endregion

        #region Methods

        public void OnAdd()
        {
        }

        public void OnRevoke()
        {
            throw new System.NotImplementedException();
        }

        public void Process(LogEvent logEvent)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// The settings for the <see cref="ConsoleProcessor"/>
    /// </summary>
    public class ConsoleProcessorSettings : IProcessorConfiguration
    {
        /// <summary>
        /// A custom writer flush to write the log events to
        /// </summary>
        public TextWriter Writer { get; set; }
    }

    /// <summary>
    /// Extensions methods to add implement <see cref="ConsoleProcessor"/> easily
    /// </summary>
    public static class ConsoleProcessorExtensions
    {
        /// <summary>
        /// Adds a new instance of the <see cref="ConsoleProcessor"/> to the processors
        /// </summary>
        public static LoggerMaker Console(this LoggerProcessorsConfiguration configuration)
        {
            // Add the processor
            configuration.AddNew<ConsoleProcessor>();

            // Return the configuration
            return configuration.Maker;
        }

        /// <summary>
        /// Adds a new instance of the <see cref="ConsoleProcessor"/> to the processors
        /// </summary>
        public static LoggerMaker Console(this LoggerProcessorsConfiguration configuration, Action<ConsoleProcessorSettings> settings)
        {
            // Add the processor
            configuration.AddNew<ConsoleProcessor, ConsoleProcessorSettings>(settings);

            // Return the configuration
            return configuration.Maker;
        }
    }
}

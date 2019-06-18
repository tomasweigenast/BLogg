using BLogg.Core.Events;
using BLogg.Core.Logging.Configuration;
using System;

namespace BLogg.Core.Processing.BuiltIn
{
    /// <summary>
    /// Processor that handles log events and writes them to files
    /// </summary>
    public class FileProcessor : ILogProcessor<FileProcessorSettings>
    {
        #region Constructors

        /// <summary>
        /// Creates a new <see cref="ConsoleProcessor"/>
        /// </summary>
        public FileProcessor() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// The configuration for the processor if needed
        /// </summary>
        public FileProcessorSettings Configuration { get; set; }

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
    /// The configurations for the <see cref="FileProcessor"/>
    /// </summary>
    public class FileProcessorSettings : IProcessorConfiguration
    {
        /// <summary>
        /// The folder path where the logs files will be saved
        /// </summary>
        public string Path { get; set; }
    }

    /// <summary>
    /// Extensions methods to add implement <see cref="FileProcessor"/> easily
    /// </summary>
    public static class FileProcessorExtensions
    {
        /// <summary>
        /// Adds a new instance of the <see cref="ConsoleProcessor"/> to the processors
        /// </summary>
        public static LoggerMaker File(this LoggerProcessorsConfiguration configuration, Action<FileProcessorSettings> settings)
        {
            // Add the processor
            bool success = configuration.AddNew<FileProcessor, FileProcessorSettings>(settings);

            if (!success)
                throw new Exception("Processor duplicated.");

            // Return the configuration
            return configuration.Maker;
        }
    }
}

using BLogg.Core.Attributes;
using BLogg.Core.Events;
using BLogg.Core.Formatters;
using BLogg.Core.Logging.Configuration;
using System;
using System.IO;

namespace BLogg.Core.Processing.BuiltIn.File
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
            // Check if folder exists
            if (!new DirectoryInfo(Configuration.Path).Attributes.HasFlag(FileAttributes.Directory))
                throw new ArgumentException($"The path '{Configuration.Path}' is not a valid directory.");

            // Create dir if not exists
            Directory.CreateDirectory(Configuration.Path);
            LogFileManager.LoadLogFiles(Configuration.Path, Configuration.FileSizeLimit <= 0 ? true : false);
        }

        public void OnRevoke() { }

        public void Process(LogEvent logEvent)
        {
            // If the log level is not enabled, return
            if (!Configuration.LogLevels.HasFlag(logEvent.Level))
                return;

            // Get a log and write
            using(var logFileWriter = LogFileManager.GetLogFile(Configuration.FileSizeLimit <= 0 ? $"Period:{Configuration.ChangePeriod}" : $"SizeLimit:{Configuration.FileSizeLimit}"))
                logFileWriter.WriteLine(new DefaultLogFormatter(false).Format(logEvent));

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
        [RequiredProperty]
        public string Path { get; set; }

        /// <summary>
        /// Indicates the log levels that will be logged to the file.
        /// By default, it will log <see cref="LogLevel.Debug"/> and <see cref="LogLevel.Info"/> levels.
        /// </summary>
        public LogLevel LogLevels { get; set; } = LogLevel.Debug | LogLevel.Info;

        /// <summary>
        /// The limit of the file size until create new file
        /// </summary>
        public long FileSizeLimit { get; set; } = -1;

        /// <summary>
        /// The period to roll the log file and create a new one
        /// </summary>
        public FileChangePeriod ChangePeriod { get; set; } = FileChangePeriod.PerWeek;
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

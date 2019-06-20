using BLogg.Core.Events;
using BLogg.Core.Formatters;
using BLogg.Core.Formatters.Event;
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
            
        }

        /// <summary>
        /// Process the log event
        /// </summary>
        public void Process(LogEvent logEvent)
        {
            // If a unique level was provided, check if the event contains it
            if(Configuration?.Level != null)
                if (logEvent.Level != Configuration?.Level.Value) return; // Skip it

            // Get the text writer to use
            TextWriter writerToUse;
            IEventFormatter formatter;

            // Get the writer to use
            if (Configuration?.Writer == null) writerToUse = Console.Out;
            else writerToUse = Configuration?.Writer;

            // Detect a custom formatter
            if (Configuration?.CustomOutputFormat != null && !string.IsNullOrWhiteSpace(Configuration?.CustomOutputFormat))
                formatter = new PlaceholderLogFormatter(Configuration?.CustomOutputFormat); // Use the custom format
            else
                formatter = new DefaultLogFormatter(Configuration == null ? false : Configuration.UseColors); // Use default log formatter

            // Write the log
            writerToUse.WriteLine(formatter.Format(logEvent));
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

        /// <summary>
        /// The unique level to log to the console. If its null, all the levels will be logged
        /// </summary>
        public LogLevel? Level { get; set; } = null;

        /// <summary>
        /// A custom output format for the processor
        /// </summary>
        public string CustomOutputFormat { get; set; }

        /// <summary>
        /// If true, the log will be written with colors
        /// </summary>
        public bool UseColors { get; set; }
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

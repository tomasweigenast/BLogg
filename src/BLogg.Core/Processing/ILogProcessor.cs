using BLogg.Core.Events;

namespace BLogg.Core.Processing
{
    /// <summary>
    /// Interface used to create logging processors
    /// </summary>
    public interface ILogProcessor
    {
        #region Methods

        /// <summary>
        /// Process an event
        /// </summary>
        /// <param name="logEvent">The event to process</param>
        void Process(LogEvent logEvent);

        /// <summary>
        /// Method invoked when the processor is revoked
        /// </summary>
        void OnRevoke();

        /// <summary>
        /// Method invoked when the processor is added
        /// </summary>
        void OnAdd();

        #endregion
    }

    /// <summary>
    /// Interface used to create logging processors which implements custom configuration
    /// </summary>
    public interface ILogProcessor<TProcessConfiguration> : ILogProcessor
        where TProcessConfiguration : IProcessorConfiguration
    {
        #region Properties

        /// <summary>
        /// The configuration for this processor
        /// </summary>
        TProcessConfiguration Configuration { get; set; }

        #endregion
    }

    /// <summary>
    /// Interface used to create logging processors which implementsa custom configuration and an optional configuration
    /// </summary>
    public interface ILogProcessor<TProcessConfiguration, TProcessOptionalConfiguration> : ILogProcessor
        where TProcessConfiguration : IProcessorConfiguration
        where TProcessOptionalConfiguration : IProcessorConfiguration
    {
        #region Properties

        /// <summary>
        /// The configuration for this processor
        /// </summary>
        TProcessConfiguration Configuration { get; set; }

        /// <summary>
        /// The optional configuration for the processor
        /// </summary>
        TProcessOptionalConfiguration OptionalConfiguration { get; set; }

        #endregion
    }
}

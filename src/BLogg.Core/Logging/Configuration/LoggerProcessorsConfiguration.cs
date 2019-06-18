using BLogg.Core.Processing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLogg.Core.Logging.Configuration
{
    /// <summary>
    /// Used to configure the logger
    /// </summary>
    public class LoggerProcessorsConfiguration
    {
        #region Private Members

        private LoggerMaker mMaker; // The logger maker
        private List<ILogProcessor> mAddedProcessors; // A list containing all the added processors

        #endregion

        #region Constructors

        internal LoggerProcessorsConfiguration(LoggerMaker maker)
        {
            mMaker = maker;
            mAddedProcessors = new List<ILogProcessor>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new processor
        /// </summary>
        public bool AddNew<TProcessor>() where TProcessor : ILogProcessor
        {
            // Create instance
            var processorInstance = (ILogProcessor)Activator.CreateInstance<TProcessor>();

            // Return false if something fails
            if (mAddedProcessors == null) return false;
            if (mAddedProcessors.Contains(processorInstance)) return false;

            // Add the processor
            mAddedProcessors.Add(processorInstance);

            // Execute on add event
            processorInstance.OnAdd();

            return true;
        }

        /// <summary>
        /// Adds a new processor with custom configuration
        /// </summary>
        /// <typeparam name="TProcessor">The type of the processor to add</typeparam>
        /// <typeparam name="TProcessorSettings">The settings of the processor</typeparam>
        /// <param name="settings">The settings of the processor</param>
        public bool AddNew<TProcessor, TProcessorSettings>(Action<TProcessorSettings> settings) 
            where TProcessorSettings : IProcessorConfiguration
            where TProcessor : ILogProcessor<TProcessorSettings>
        {
            // Create instances
            TProcessor processorInstance = Activator.CreateInstance<TProcessor>(); // Processor instance
            TProcessorSettings processorSettingsInstance = Activator.CreateInstance<TProcessorSettings>(); // Processor settings instance
            settings.Invoke(processorSettingsInstance); // Invoke settings

            // Return false if something fails
            if (mAddedProcessors == null) return false;
            if (mAddedProcessors.Contains(processorInstance)) return false;

            // Apply configuration
            processorInstance.Configuration = processorSettingsInstance;

            // Add processor
            mAddedProcessors.Add(processorInstance);

            return true;
        }

        /// <summary>
        /// Removes a processor
        /// </summary>
        public bool Remove(ILogProcessor processor)
        {
            // Return false if something fails
            if (mAddedProcessors == null) return false;
            if (!mAddedProcessors.Contains(processor)) return false;

            // Revoke method
            processor.OnRevoke();

            // Remove the processor
            mAddedProcessors.Remove(processor);

            return true;
        }

        #region Internal

        /// <summary>
        /// Tries to get a processor
        /// </summary>
        /// <typeparam name="TProcessor">The type of processor to search</typeparam>
        internal ILogProcessor GetProcessor<TProcessor>()
        {
            // Try to get a processor
            var possibleProcessor = mAddedProcessors.FirstOrDefault(x => x.GetType() == typeof(TProcessor));

            // Throw exception if not found
            if (possibleProcessor == null)
                throw new Exception($"Processor of type {typeof(TProcessor).ToString()} not found.");

            // Return
            return possibleProcessor;
        }

        /// <summary>
        /// Gets all the processors
        /// </summary>
        internal ILogProcessor[] GetProcessors()
            => mAddedProcessors.ToArray();

        #endregion

        #endregion
    }
}

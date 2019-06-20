using BLogg.Core.Attributes;
using BLogg.Core.Exceptions;
using BLogg.Core.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        #region Public Properties

        /// <summary>
        /// The current logger maker instance
        /// </summary>
        public LoggerMaker Maker => mMaker;

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
        /// Adds a new processor with custom required configuration
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
            if (mAddedProcessors.Any(x => x.GetType() == typeof(TProcessor))) return false;

            // Check null properties
            if (processorSettingsInstance.GetType().GetCustomAttributes().Any(x => x.GetType() == typeof(RequiredPropertyAttribute)))
                CheckNullProperties(processorSettingsInstance);

            // Apply configuration
            processorInstance.Configuration = processorSettingsInstance;

            // Add processor
            mAddedProcessors.Add(processorInstance);

            return true;
        }

        /// <summary>
        /// Adds a new processor with custom required configuration and an optional one
        /// </summary>
        /// <typeparam name="TProcessor">The type of the processor to add</typeparam>
        /// <typeparam name="TProcessorSettings">The settings type of the processor</typeparam>
        /// <typeparam name="TProcessorOptionalSettings">The optional settings type of the processor</typeparam>
        /// <param name="settings">The settings of the processor</param>
        /// <param name="optionalSettings">The optional settings of the processor</param>
        public bool AddNew<TProcessor, TProcessorSettings, TProcessorOptionalSettings>(Action<TProcessorSettings> settings, Action<TProcessorOptionalSettings> optionalSettings)
            where TProcessorSettings : IProcessorConfiguration
            where TProcessorOptionalSettings : IProcessorConfiguration
            where TProcessor : ILogProcessor<TProcessorSettings, TProcessorOptionalSettings>
        {
            // Create instances
            TProcessor processorInstance = Activator.CreateInstance<TProcessor>(); // Processor instance
            TProcessorSettings processorSettingsInstance = Activator.CreateInstance<TProcessorSettings>(); // Processor settings instance
            TProcessorOptionalSettings processorOptionalSettingsInstance = Activator.CreateInstance<TProcessorOptionalSettings>(); // The optional settings
            settings.Invoke(processorSettingsInstance); // Invoke settings
            optionalSettings.Invoke(processorOptionalSettingsInstance); // Invoke optional settings

            // Return false if something fails
            if (mAddedProcessors == null) return false;
            if (mAddedProcessors.Any(x => x.GetType() == typeof(TProcessor))) return false;

            // Check null properties of the required properties
            CheckNullProperties(processorSettingsInstance);

            // Apply configuration
            processorInstance.Configuration = processorSettingsInstance;
            processorInstance.OptionalConfiguration = processorOptionalSettingsInstance;

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

        #region Private Method Helpers

        /// <summary>
        /// Checks for each property of a type and if it founds a null property, it throws an exception
        /// </summary>
        public void CheckNullProperties(object instance)
        {
            // Get public properties
            var properties = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Foreach property
            foreach(var property in properties)
            {
                // Try to get its value
                object value = property.GetValue(instance);

                // If the value if null, throw exception
                if (value == null)
                    throw new RequiredPropertyException(property.Name);
            }
        }

        #endregion
    }
}

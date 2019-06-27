using BLogg.Core.Events;
using BLogg.Core.Formatters.Placeholders;

namespace BLogg.Core.Formatters.Event
{
    /// <summary>
    /// Applies format to the an event using the placeholder formatter
    /// </summary>
    public class PlaceholderLogFormatter : IEventFormatter
    {
        #region Private Members

        private string mTemplate; // The output template

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="template">The format template</param>
        public PlaceholderLogFormatter(string template)
        {
            // TODO: Check if template contains at least one property to parse
            // TODO: Add color parser

            mTemplate = template;
        }

        #endregion

        /// <summary>
        /// Format the event
        /// </summary>
        public string Format(LogEvent logEvent) => PlaceholderFormatter.ApplyFormat(mTemplate, logEvent);
    }
}

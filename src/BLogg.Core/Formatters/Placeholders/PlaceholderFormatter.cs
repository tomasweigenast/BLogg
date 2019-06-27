using BLogg.Core.Attributes;
using BLogg.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLogg.Core.Formatters.Placeholders
{
    /// <summary>
    /// Applies format to a string
    /// </summary>
    public static class PlaceholderFormatter
    {
        /// <summary>
        /// Apply formats
        /// </summary>
        public static string ApplyFormat(string str, LogEvent logEvent)
        {
            // Avoid bugs
            if (string.IsNullOrWhiteSpace(str)) throw new ArgumentNullException(nameof(str));
            if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));

            var validPlaceholders = new Dictionary<string, string>();
            string finalStr = str;

            // Get matches
            string[] matches = str.Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            // Check if the placeholder in the strings exists
            foreach(var match in matches) // Foreach match
            {
                string matchValue = "";
                string[] matchParameters = match.Split(':');
                Placeholder placeholder = null;

                // If the match contains any parameter
                if(match.Contains(":"))
                {
                    // Get values
                    string placeholderStr = matchParameters[0];
                    string parameter = match.Replace($"{placeholderStr}:", "");

                    placeholder = new Placeholder(placeholderStr, parameter);

                    matchValue = GetPlaceholderValue(placeholder, logEvent);
                }
                // Otherwise, get the value
                else
                {
                    placeholder = new Placeholder(match);
                    matchValue = GetPlaceholderValue(placeholder, logEvent);
                }

                // Add the formatted value to the list
                if (matchValue != null)
                    validPlaceholders.Add(placeholder.ToString(), matchValue);
            }

            // Foreach entry in the dictionary
            foreach(var entry in validPlaceholders)
                finalStr = finalStr.Replace(entry.Key, entry.Value);

            return finalStr;
        }

        #region Private Helper Methods

        // Returns the placeholder's value based on a log event
        private static string GetPlaceholderValue(Placeholder placeholder, LogEvent logEvent)
        {
            // The placeholder contains a parameter
            if (placeholder.Parameter != null)
            {
                // Search properties in logEvent with the same placeholder attribute that placeholder property
                var properties = logEvent.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(PlaceholderAttribute), true).Length > 0).ToArray();

                // Foreach found property...
                foreach (var property in properties)
                {
                    string propertyPlaceholder = ((PlaceholderAttribute)property.GetCustomAttributes(typeof(PlaceholderAttribute), true)[0]).Name;

                    // If the placeholder name is not the samea as the property name, skip
                    if (propertyPlaceholder != placeholder.Name)
                        continue;

                    // If the property type is DateTime
                    if (property.PropertyType == typeof(DateTime))
                    {
                        DateTime propValue = (DateTime)property.GetValue(logEvent); // Get datetime value
                        return propValue.ToString(placeholder.Parameter); // Apply format and return
                    }

                    // Otherwise, we will get properties of another property
                    else
                    {
                        // Get first level property value
                        object propValue = property.GetValue(logEvent);

                        // Check if there are more properties
                        if (property.PropertyType.Assembly == logEvent.GetType().Assembly && !property.PropertyType.IsEnum)
                        {
                            Type propValueType = propValue.GetType();
                            var secondProperties = propValue.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(PlaceholderAttribute), true).Length > 0).ToArray();

                            // Foreach found property
                            foreach (var secondProp in secondProperties)
                            {
                                // Get placeholder name
                                string secondPropPlaceholder = ((PlaceholderAttribute)secondProp.GetCustomAttributes(typeof(PlaceholderAttribute), true)[0]).Name;

                                // If the placeholder is not the same as the parameter, skip
                                if (secondPropPlaceholder != placeholder.Parameter)
                                    continue;

                                // Get final value
                                object finalValue = secondProp.GetValue(propValue);
                                return finalValue.ToString();
                            }
                        }
                    }
                }
            }

            // Placeholder does not have any parameter
            else
            {
                // Search properties in logEvent with the same placeholder attribute that placeholder property
                var properties = logEvent.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(PlaceholderAttribute), true).Length > 0).ToArray();

                // Foreach found property...
                foreach (var property in properties)
                {
                    string propertyPlaceholder = ((PlaceholderAttribute)property.GetCustomAttributes(typeof(PlaceholderAttribute), true)[0]).Name;

                    // If the placeholder name is not the samea as the property name, skip
                    if (propertyPlaceholder != placeholder.Name)
                        continue;

                    // Get property value an parse it as string
                    return property.GetValue(logEvent).ToString();
                }
            }

            return null;
        }

        #endregion
    }
}

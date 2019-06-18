using System;

namespace BLogg.Core.Attributes
{
    /// <summary>
    /// Mark a property has a placeholder
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PlaceholderAttribute : Attribute
    {
        /// <summary>
        /// The placeholder name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PlaceholderAttribute(string name)
        {
            Name = name;
        }
    }
}

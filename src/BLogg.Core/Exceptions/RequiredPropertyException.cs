using System;

namespace BLogg.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when there is a null property
    /// </summary>
    public class RequiredPropertyException : ArgumentException
    {
        /// <summary>
        /// Creates a new <see cref="RequiredPropertyException"/> with a the property name
        /// </summary>
        /// <param name="propertyName">The name of the property that is null</param>
        public RequiredPropertyException(string propertyName) : base($"Property '{propertyName}' cannot be null because it is required configuration.") { }
    }
}

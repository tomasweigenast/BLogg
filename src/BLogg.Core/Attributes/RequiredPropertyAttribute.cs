using System;

namespace BLogg.Core.Attributes
{
    /// <summary>
    /// Marks a property from a configuration class as required
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredPropertyAttribute : Attribute
    {
        
    }
}

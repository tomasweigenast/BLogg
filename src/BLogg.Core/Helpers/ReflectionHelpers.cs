using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BLogg.Core.Helpers
{
    internal static class ReflectionHelpers
    {
        /// <summary>
        /// Gets all constants from a class
        /// </summary>
        public static FieldInfo[] GetConstants(this Type type)
            => type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Where(x => x.IsLiteral && !x.IsInitOnly).ToArray();

        /// <summary>
        /// Gets all constants value from a class
        /// </summary>
        public static List<string> GetConstantsValue(this Type type)
            => type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Where(x => x.IsLiteral && !x.IsInitOnly).Select(y => (string)y.GetRawConstantValue()).ToList();

        /// <summary>
        /// Gets all constants with its value
        /// </summary>
        public static Dictionary<string, string> GetConstantsWithValue(this Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Where(x => x.IsLiteral && !x.IsInitOnly).ToArray();
            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (var field in fields)
                dic.Add(field.Name, (string)field.GetRawConstantValue());

            return dic;
        }
    }
}

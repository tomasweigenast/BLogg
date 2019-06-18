namespace BLogg.Core.Formatters.Placeholders
{
    /// <summary>
    /// A placeholder that its converted to its value
    /// </summary>
    public class Placeholder
    {
        #region Public Properties
        /// <summary>
        /// The name of the placeholder
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Any valid parameter
        /// </summary>
        public string Parameter { get; }

        #endregion

        #region Constructor

        public Placeholder(string name, string parameter = null)
        {
            Name = name;
            Parameter = parameter;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            string finalStr = "";
            if (!string.IsNullOrWhiteSpace(Parameter))
                finalStr += "{" + Name + ":" + Parameter + "}";
            else
                finalStr += "{" + Name + "}";

            return finalStr;
        }

        #endregion
    }
}

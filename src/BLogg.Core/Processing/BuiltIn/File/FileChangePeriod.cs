namespace BLogg.Core.Processing.BuiltIn.File
{
    /// <summary>
    /// The available periods to change the log file
    /// </summary>
    public enum FileChangePeriod
    {
        /// <summary>
        /// Roll the file per day
        /// </summary>
        PerDay,

        /// <summary>
        /// Roll the file per week
        /// </summary>
        PerWeek,

        /// <summary>
        /// Roll the file per month
        /// </summary>
        PerMonth,

        /// <summary>
        /// Roll the file per year
        /// </summary>
        PerYear
    }
}

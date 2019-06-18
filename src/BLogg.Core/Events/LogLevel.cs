namespace BLogg.Core.Events
{
    /// <summary>
    /// All the possible levels of an event
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Relatively detailed tracing used by application developers. The exact meaning of the three debug levels varies amoung subsystems.
        /// </summary>
        Debug = 10000,

        /// <summary>
        /// Informational messages that might make sense to end users and system administrators, and highlight the process of the application.
        /// </summary>
        Info = 20000,

        /// <summary>
        /// Potentially harmful situations of interest to end users or system managers that indicate potential problems.
        /// </summary>
        Warning = 30000,

        /// <summary>
        /// Error events of considerable importance that will prevent normal program execution, but might still allow the application to continue running.
        /// </summary>
        Error = 40000,

        /// <summary>
        /// Very severe error events that might cause the application to terminate
        /// </summary>
        Fatal = 50000
    }
}

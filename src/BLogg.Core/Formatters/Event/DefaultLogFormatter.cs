using BLogg.Core.Events;

namespace BLogg.Core.Formatters
{
    /// <summary>
    /// The base log formatter 
    /// </summary>
    public class DefaultLogFormatter : IEventFormatter
    {
        /// <summary>
        /// Applies the format to the event
        /// </summary>
        /// <param name="logEvent"></param>
        public string Format(LogEvent logEvent)
        {
            // TODO: Apply formatters to the log message

            return string.Format("[{0:dd/MM/yy HH:mm:ss}] [{1}] [Thread: {2} Process: {3}] <{4}: {5} line: {6}>",
                logEvent.FireDate,
                logEvent.Level,
                logEvent.CallDiagnostics.ThreadId,
                logEvent.CallDiagnostics.ProcessId,
                logEvent.CallDiagnostics.CallingClass,
                logEvent.CallDiagnostics.CallingMethod,
                logEvent.CallDiagnostics.LineNumber);
        }
    }
}

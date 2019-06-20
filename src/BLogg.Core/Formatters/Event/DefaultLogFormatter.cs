using BLogg.Core.Events;
using BLogg.Core.Extensions;
using System;
using System.Drawing;

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

            if (logEvent.Exception == null)
                return
                    $"[{logEvent.FireDate.ToString("dd/MM/yy HH:mm:ss")}]" +
                    $" [{logEvent.Level}]" +
                    $" <{logEvent.CallDiagnostics.CallingClass}: {logEvent.CallDiagnostics.CallingMethod}" +
                    $" line: {logEvent.CallDiagnostics.LineNumber}>" +
                    $" -> {logEvent.Message}".Pastel(Color.Bisque);
            else
                return
                    $"[{logEvent.FireDate.ToString("dd/MM/yy HH:mm:ss")}]" +
                    $" [{logEvent.Level}]" +
                    $" <{logEvent.CallDiagnostics.CallingClass}: {logEvent.CallDiagnostics.CallingMethod}" +
                    $" line: {logEvent.CallDiagnostics.LineNumber}>" +
                    $" -> {logEvent.Message}".Pastel(Color.Bisque) +
                    Environment.NewLine +
                    $"{logEvent.Exception.ToString().Pastel(Color.OrangeRed)}";
        }
    }
}

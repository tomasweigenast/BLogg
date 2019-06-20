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
        #region Private Members

        private bool mIsColored;

        #endregion

        #region Constructor

        public DefaultLogFormatter(bool colored)
        {
            mIsColored = colored;
        }

        #endregion

        /// <summary>
        /// Applies the format to the event
        /// </summary>
        /// <param name="logEvent"></param>
        public string Format(LogEvent logEvent)
        {
            // TODO: Apply formatters to the log message
            string returnString = null;

            if(!mIsColored)
            {
                returnString += $"[{logEvent.FireDate.ToString("dd/MM/yy HH:mm:ss")}]" +
                                $" [{logEvent.Level}]" +
                                $" <{logEvent.CallDiagnostics.CallingClass}: {logEvent.CallDiagnostics.CallingMethod}" +
                                $" line: {logEvent.CallDiagnostics.LineNumber}>" +
                                $" -> {logEvent.Message}";

                if (logEvent.Exception != null)
                    returnString += Environment.NewLine + logEvent.Exception.ToString();
            }
            else
            {
                returnString += $"[{logEvent.FireDate.ToString("dd/MM/yy HH:mm:ss")}]".Pastel(Color.Aqua) +
                                $" [{logEvent.Level}]".Pastel(logEvent.Level.GetColor()) +
                                $" <{logEvent.CallDiagnostics.CallingClass}: {logEvent.CallDiagnostics.CallingMethod}".Pastel(Color.AliceBlue) +
                                $" line: {logEvent.CallDiagnostics.LineNumber}>".Pastel(Color.AliceBlue) +
                                " -> " +
                                $"{logEvent.Message}".Pastel(logEvent.Level.GetColor());

                if (logEvent.Exception != null)
                    returnString += Environment.NewLine + logEvent.Exception.ToString().Pastel(Color.Red);
            }

            return returnString;
        }
    }
}

using BLogg.Core.Attributes;
using System.Diagnostics;
using System.Reflection;

namespace BLogg.Core.Diagnostics
{
    /// <summary>
    /// Used to diagnostic method calls and classes
    /// </summary>
    public class CallDiagnostic
    {
        #region Public Properties

        /// <summary>
        /// The id of the thread where the call was made
        /// </summary>
        [Placeholder("ThreadId")]
        public int ThreadId { get; }

        /// <summary>
        /// The id of the process where the call was made
        /// </summary>
        [Placeholder("ProcessId")]
        public int ProcessId { get; }

        /// <summary>
        /// The number of the line where the call was made
        /// </summary>
        [Placeholder("LineNumber")]
        public int LineNumber { get; }

        /// <summary>
        /// Gets the file where the code that gets executed is written
        /// </summary>
        [Placeholder("FileName")]
        public string FileName { get; }

        /// <summary>
        /// The name of the calling class
        /// </summary>
        [Placeholder("Class")]
        public string CallingClass { get; }

        /// <summary>
        /// The name of the calling method
        /// </summary>
        [Placeholder("Method")]
        public string CallingMethod { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CallDiagnostic(int threadId, int processId, StackFrame stackFrame)
        {
            ThreadId = threadId;
            ProcessId = processId;

            // Get stackframe properties
            var methodBase = stackFrame == null ? MethodBase.GetCurrentMethod() : stackFrame.GetMethod();
            CallingMethod = methodBase.Name;
            CallingClass = methodBase.ReflectedType.Name;
            LineNumber = stackFrame.GetFileLineNumber();
            FileName = stackFrame.GetFileName();
        }

        #endregion

        #region Methods

        public override string ToString() => $"[Thr: {ThreadId} Pr: {ProcessId}] <{CallingClass}: {CallingMethod} line: {LineNumber}>";

        #endregion
    }
}

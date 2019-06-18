using System.Diagnostics;

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
        public int ThreadId { get; }

        /// <summary>
        /// The id of the process where the call was made
        /// </summary>
        public int ProcessId { get; }

        /// <summary>
        /// The stack frame of the current call
        /// </summary>
        public StackFrame StackFrame { get; }

        /// <summary>
        /// The number of the line where the call was made
        /// </summary>
        public int LineNumber { get; }

        /// <summary>
        /// The name of the calling class
        /// </summary>
        public string CallingClass { get; }

        /// <summary>
        /// The name of the calling method
        /// </summary>
        public string CallingMethod { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CallDiagnostic(int threadId, int processId, StackFrame stackFrame, int lineNumber, string callingClass, string callingMethod)
        {
            ThreadId = threadId;
            ProcessId = processId;
            StackFrame = stackFrame;
            LineNumber = lineNumber;
            CallingClass = callingClass;
            CallingMethod = callingMethod;
        }

        #endregion
    }
}

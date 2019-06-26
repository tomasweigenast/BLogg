using System;
using System.IO;

namespace BLogg.Core.Processing.BuiltIn.File
{
    internal abstract class BaseLogFile
    {
        /// <summary>
        /// The path to the file
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the file size
        /// </summary>
        public long FileSize => new FileInfo(Path).Length;
    }

    internal class DateTimeLogFile : BaseLogFile
    {
        /// <summary>
        /// The date when the log file was 
        /// </summary>
        public DateTime LogDate { get; set; }
    }

    internal class EnumeratedLogFile : BaseLogFile
    {
        /// <summary>
        /// The log id
        /// </summary>
        public int Id { get; set; }
    }
}

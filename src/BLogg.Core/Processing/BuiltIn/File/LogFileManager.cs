using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace BLogg.Core.Processing.BuiltIn.File
{
    internal static class LogFileManager
    {
        #region Private Members

        private static readonly SortedList<object, BaseLogFile> mLogFiles = new SortedList<object, BaseLogFile>();
        private static readonly List<string> mInvalidLogFiles = new List<string>();
        private static string mWorkingDir;
        private static bool mDateTimeNameFormat;

        #endregion

        /// <summary>
        /// Gets the logs files
        /// </summary>
        public static void LoadLogFiles(string directory, bool dateTimeNameFormat)
        {
            mDateTimeNameFormat = dateTimeNameFormat;
            mWorkingDir = directory;

            // Foreach found log file..
            foreach(var logFilePath in Directory.EnumerateFiles(directory, "*.log"))
            {
                string fileName = Path.GetFileNameWithoutExtension(logFilePath); // Get filename

                // Format the log file with datetime
                if(dateTimeNameFormat)
                {
                    bool parseResult = DateTime.TryParseExact(fileName, "ddMMyy", null, DateTimeStyles.None, out DateTime dateTime); // Parse its name as a date time

                    // If the parse was failed, add to invalid log files
                    if (!parseResult)
                    {
                        mInvalidLogFiles.Add(logFilePath);
                        continue;
                    }

                    // Create a log file
                    DateTimeLogFile logFile = new DateTimeLogFile
                    {
                        Path = logFilePath,
                        LogDate = dateTime
                    };

                    // Add to log files
                    mLogFiles.Add(mLogFiles.Count +1, logFile);
                }

                // Format the log file with numbers
                else
                {
                    // Format: 001.log 002.log
                    // Get log id
                    bool parseResult = int.TryParse(fileName, out int id);

                    // If the parse was failed, add to invalid log files
                    if (!parseResult)
                        mInvalidLogFiles.Add(logFilePath);

                    // Create log file
                    EnumeratedLogFile logFile = new EnumeratedLogFile
                    {
                        Path = logFilePath,
                        Id = id
                    };

                    // Add to log files
                    mLogFiles.Add(id, logFile);
                }
            }
        }

        /// <summary>
        /// Gets a log file to write to
        /// </summary>
        public static StreamWriter GetLogFile(string settings)
        {
            string[] settingParameters = settings.Split(':');

            // If the setting indicates that the file must be changed on a size limit
            if(settingParameters[0] == "SizeLimit")
            {
                // Get parameters
                long sizeLimit = long.Parse(settingParameters[1]);
                EnumeratedLogFile lastLogFile = (EnumeratedLogFile)mLogFiles.LastOrDefault().Value;

                // If last log file is null, create new file
                if (lastLogFile == null)
                {
                    // Get filename
                    string newFileName = $"{mWorkingDir}\\{1.ToString("000")}.log";

                    // Create a new log file
                    EnumeratedLogFile logFile = new EnumeratedLogFile
                    {
                        Id = 1,
                        Path = newFileName
                    };

                    // Add log
                    mLogFiles.Add(1, logFile);

                    return new StreamWriter(new FileStream(newFileName, FileMode.Create, FileAccess.Write, FileShare.Write), Encoding.UTF8);
                }

                // Create new file because the last one reached its limit
                if (lastLogFile.FileSize >= sizeLimit)
                {
                    string newFileName = $"{mWorkingDir}{Path.DirectorySeparatorChar}{(lastLogFile.Id + 1).ToString("000")}.log";

                    // Create a new log file
                    EnumeratedLogFile logFile = new EnumeratedLogFile
                    {
                        Id = lastLogFile.Id + 1,
                        Path = newFileName
                    };

                    // Add log
                    mLogFiles.Add(logFile.Id, logFile);

                    return new StreamWriter(new FileStream(newFileName, FileMode.Create, FileAccess.Write, FileShare.Write), Encoding.UTF8);
                }

                // Return the file
                else
                    return new StreamWriter(lastLogFile.Path, true, Encoding.UTF8);
                
            }

            // Otherwise, get change period
            else if(settingParameters[0] == "Period")
            {
                FileChangePeriod changePeriod = (FileChangePeriod)Enum.Parse(typeof(FileChangePeriod), settingParameters[1]);
                DateTimeLogFile lastLogFile = (DateTimeLogFile)mLogFiles.LastOrDefault().Value;

                // If there are not a last file, create new
                if(lastLogFile == null)
                {
                    // Get filename
                    string newFileName = $"{mWorkingDir}\\{DateTime.Today.ToString("ddMMyy")}.log";

                    // Create a new log file
                    DateTimeLogFile logFile = new DateTimeLogFile
                    {
                        LogDate = DateTime.Today,
                        Path = newFileName
                    };

                    // Add log
                    mLogFiles.Add(1, logFile);

                    return new StreamWriter(new FileStream(newFileName, FileMode.Create, FileAccess.Write, FileShare.Write), Encoding.UTF8);
                }

                // If the last log file is not null...
                if(changePeriod == FileChangePeriod.PerDay) // The change period is per day
                {
                    // Change the file
                    if ((lastLogFile.LogDate.Date - DateTime.Today).TotalDays != 0)
                    {
                        // Get filename
                        string newFileName = $"{mWorkingDir}\\{DateTime.Today.ToString("ddMMyy")}.log";

                        // Create a new log file
                        DateTimeLogFile logFile = new DateTimeLogFile
                        {
                            LogDate = DateTime.Today,
                            Path = newFileName
                        };

                        // Add log
                        mLogFiles.Add(mLogFiles.Count + 1, logFile);

                        return new StreamWriter(new FileStream(newFileName, FileMode.Create, FileAccess.Write, FileShare.Write), Encoding.UTF8);
                    }

                    // Return the same file
                    else return new StreamWriter(lastLogFile.Path, true, Encoding.UTF8);
                }

                else if (changePeriod == FileChangePeriod.PerMonth) // The change period is per month
                {
                    // Needs to change the file // -76
                    if ((lastLogFile.LogDate.Date - DateTime.Today).TotalDays != 0)
                    {
                        // Get filename
                        string newFileName = $"{mWorkingDir}\\{DateTime.Today.ToString("ddMMyy")}.log";

                        // Create a new log file
                        DateTimeLogFile logFile = new DateTimeLogFile
                        {
                            LogDate = DateTime.Today,
                            Path = newFileName
                        };

                        // Add log
                        mLogFiles.Add(mLogFiles.Count + 1, logFile);

                        return new StreamWriter(new FileStream(newFileName, FileMode.Create, FileAccess.Write, FileShare.Write), Encoding.UTF8);
                        
                    }

                    // Returns the same file
                    else return new StreamWriter(lastLogFile.Path, true, Encoding.UTF8);
                }

                else if (changePeriod == FileChangePeriod.PerWeek) // The change period is per week
                {
                    // Change the file
                    if ((lastLogFile.LogDate.Date - DateTime.Today).TotalDays != 0)
                    {
                        // Get filename
                        string newFileName = $"{mWorkingDir}\\{DateTime.Today.ToString("ddMMyy")}.log";

                        // Create a new log file
                        DateTimeLogFile logFile = new DateTimeLogFile
                        {
                            LogDate = DateTime.Today,
                            Path = newFileName
                        };

                        // Add log
                        mLogFiles.Add(mLogFiles.Count + 1, logFile);

                        return new StreamWriter(new FileStream(newFileName, FileMode.Create, FileAccess.Write, FileShare.Write), Encoding.UTF8);
                    }

                    // Return the same file
                    else return new StreamWriter(lastLogFile.Path, true, Encoding.UTF8);
                }

                else if (changePeriod == FileChangePeriod.PerYear) // The change period is per year
                {
                    // Change the file
                    if ((lastLogFile.LogDate.Date - DateTime.Today).TotalDays != 0)
                    {
                        // Get filename
                        string newFileName = $"{mWorkingDir}\\{DateTime.Today.ToString("ddMMyy")}.log";

                        // Create a new log file
                        DateTimeLogFile logFile = new DateTimeLogFile
                        {
                            LogDate = DateTime.Today,
                            Path = newFileName
                        };

                        // Add log
                        mLogFiles.Add(mLogFiles.Count + 1, logFile);

                        return new StreamWriter(new FileStream(newFileName, FileMode.Create, FileAccess.Write, FileShare.Write), Encoding.UTF8);
                    }

                    // Return the same file
                    else return new StreamWriter(lastLogFile.Path, true, Encoding.UTF8);
                }
            }

            return null;

        }

        /// <summary>
        /// Gets all the loaded log files
        /// </summary>
        public static BaseLogFile[] GetLogFiles()
        {
            BaseLogFile[] logs = new BaseLogFile[mLogFiles.Count];
            var keys = mLogFiles.Keys;
            foreach (object key in keys)
                logs[keys.IndexOf(key)] = mLogFiles[key];

            return logs;
        }

        /// <summary>
        /// Gets all the invalid log files
        /// </summary>
        public static string[] GetInvalidLogFiles() => mInvalidLogFiles.ToArray();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MikeDev.Debug
{
    /// <summary>
    /// MLogger class is used for debugging and logging procedures.
    /// </summary>
    public class MLogger
    {
        private bool _DefaultCulture = true;

        public string CapturedFilename { get; private set; }

        /// <summary>
        /// Capture a file and write future log to it.
        /// </summary>
        /// <param name="filename">Path to captured file.</param>
        public MLogger(string filename)
        {
            CapturedFilename = filename;
        }

        /// <summary>
        /// Capture a file and write future log (using custom culture) to it.
        /// </summary>
        /// <param name="filename">Path to captured file.</param>
        /// <param name="customCulture">The custom template for log.</param>
        public MLogger(string filename, string customCulture)
        {

        }

        /// <summary>
        /// Write message to captured file.
        /// </summary>
        /// <param name="level">The level of severity.</param>
        /// <param name="message">The message to write.</param>
        /// <param name="includeTime">Whether include time or not.</param>
        /// <returns>True if log is written successfully. Otherwise, false.</returns>
        public bool Log(string message, bool includeTime = true, LogLevel level = LogLevel.Info)
        {
            string Data = _InternalPrepareMessage(message, includeTime, level);
            throw new NotImplementedException();
        }

        /// <summary>
        /// LogLevel enum indicates the severity of the message.
        /// </summary>
        public enum LogLevel
        {
            Info,
            Warning,
            Error
        }

        /// <summary>
        /// Create message based on default template.
        /// </summary>
        private string _InternalPrepareMessage(string message, bool includeTime, LogLevel level)
        {
            if (_DefaultCulture)
            {
                // Add timestamp
                string Message = includeTime ? $"[{DateTime.Now}]" : "";

                // Add severity
                Message += $"{level.ToString()} : ";

                // Add message
                Message += message;

                return Message;
            }

            throw new NotImplementedException();
        }
    }
}

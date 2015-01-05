using Whitelog.Interface;

namespace Whitelog.Core.Loggers.String
{
    public interface IStringAppender
    {
        /// <summary>
        /// Indicate if the current LogEntry pass filter
        /// </summary>
        /// <returns>True to filter the log, other wise false</returns>
        bool Filter(LogEntry logEntry);

        void Append(string value,LogEntry logEntry);
    }
}
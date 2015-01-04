using Whitelog.Core.Binary;
using Whitelog.Core.Binary.FileLog;
using Whitelog.Interface;

namespace Whitelog.Core.Loggers
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

    public interface IBinaryAppender
    {
        /// <summary>
        /// Indicate if the current LogEntry pass filter
        /// </summary>
        /// <returns>True to filter the log, other wise false</returns>
        bool Filter(LogEntry logEntry);

        void AppendLogEntry(IRawData data, LogEntry logEntry);
        void AppendPackage(IRawData data,RegisteredPackageDefinition definition);
        void AppendStringChached(IRawData data,CacheString cacheString);
    }
}
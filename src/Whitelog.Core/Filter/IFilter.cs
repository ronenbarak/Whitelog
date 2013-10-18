using Whitelog.Interface;

namespace Whitelog.Core.Filter
{

    public interface IFilter
    {
        /// <summary>
        /// Indicate if the current LogEntry pass filter
        /// </summary>
        /// <returns>True to filter the log, other wise false</returns>
        bool Filter(LogEntry logEntry);
    }
}

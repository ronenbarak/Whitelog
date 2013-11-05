using System;
using Whitelog.Barak.Common.SystemTime;

namespace Whitelog.Barak.SystemDateTime
{
    /// <summary>
    /// Wrapper for DateTime.UtcNow
    /// </summary>
    class SystemUtcTime : ISystemTime
    {
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }
    }
}
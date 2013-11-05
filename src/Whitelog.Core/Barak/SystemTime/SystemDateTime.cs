using System;
using Whitelog.Barak.Common.SystemTime;

namespace Whitelog.Barak.SystemDateTime
{
    /// <summary>
    /// Wrapper for DateTime.Now
    /// </summary>
    public class SystemDateTime : ISystemTime
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}
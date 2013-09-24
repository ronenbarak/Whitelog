using System;
using Whitelog.Barak.Common.SystemTime;

namespace Whitelog.Core
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
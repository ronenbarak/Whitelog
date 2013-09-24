using System;

namespace Whitelog.Core.Reader
{
    public class UnkownLogFileException : Exception
    {
        public UnkownLogFileException()
            : base("The Log is in invalid format")
        {
        }
    }
}
using System;

namespace Whitelog.Core
{
    public class CorruptedDataException : Exception
    {
        public CorruptedDataException()
            : base("The stream containing invalid format")
        {

        }
    }
}
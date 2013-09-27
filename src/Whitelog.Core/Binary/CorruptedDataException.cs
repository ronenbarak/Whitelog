using System;

namespace Whitelog.Core.Binary
{
    public class CorruptedDataException : Exception
    {
        public CorruptedDataException()
            : base("The stream containing invalid format")
        {

        }
    }
}
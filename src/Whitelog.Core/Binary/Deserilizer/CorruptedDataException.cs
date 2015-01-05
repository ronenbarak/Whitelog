using System;

namespace Whitelog.Core.Binary.Deserilizer
{
    public class CorruptedDataException : Exception
    {
        public CorruptedDataException()
            : base("The stream containing invalid format")
        {

        }
    }
}
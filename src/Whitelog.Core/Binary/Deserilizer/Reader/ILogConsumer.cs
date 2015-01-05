using Whitelog.Core.Binary.Deserilizer.Reader.Generic;

namespace Whitelog.Core.Binary.Deserilizer.Reader
{
    public interface ILogConsumer
    {
        void Consume(IEntryData entryData);
    }
}
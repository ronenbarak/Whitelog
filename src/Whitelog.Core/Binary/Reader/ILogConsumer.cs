namespace Whitelog.Core.Binary.Reader
{
    public interface ILogConsumer
    {
        void Consume(ILogEntryData entryData);
    }
}
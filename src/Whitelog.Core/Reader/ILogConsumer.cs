namespace Whitelog.Core.Reader
{
    public interface ILogConsumer
    {
        void Consume(ILogEntryData entryData);
    }
}
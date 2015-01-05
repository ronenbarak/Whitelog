using System;
using System.IO;

namespace Whitelog.Core.Binary.Deserilizer.Reader
{
    public interface ILogReaderFactory
    {
        Guid Id { get; }
        ILogReader Create(Stream data, ILogConsumer consumer);
    }
}
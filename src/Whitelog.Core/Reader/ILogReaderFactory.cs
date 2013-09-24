using System;
using System.IO;

namespace Whitelog.Core.Reader
{
    public interface ILogReaderFactory
    {
        Guid Id { get; }
        ILogReader Create(Stream data, ILogConsumer consumer);
    }
}
using System;
using Whitelog.Core.Binary.ListWriter;

namespace Whitelog.Core.Binary
{
    public static class BufferDefaults
    {
        public const int Size = 1024;
    }

    public interface IRawData
    {
        int Length { get; }
        byte[] Buffer { get; }
        DateTime DateTime { get; set; }
    }

    public interface IBuffer : IRawData,IDisposable
    {
        ISerializer AttachedSerializer { get; }
        void SetLength(int length);
        byte[] IncressSize(int length,int required);
    }

    public interface ISubmitLogEntryFactory
    {
        ISubmitEntry<IRawData> CreateSubmitLogEntry(IListWriter listWriter);
    }
}
using System;
using Whitelog.Interface;
using Whitelog.Core.ListWriter;

namespace Whitelog.Core.FileLog
{
    public static class BufferDefaults
    {
        public const int Size = 1024;
    }

    public interface IRawData
    {
        int Length { get; }
        byte[] Buffer { get; set; }
    }

    public interface IBuffer : IRawData,IDisposable
    {
        ISerializer AttachedSerializer { get; }
        void SetLength(int length);
        byte[] IncressSize(int length,int required);
    }

    public interface ISubmitLogEntryFactory
    {
        ISubmitLogEntry CreateSubmitLogEntry(IListWriter listWriter);
    }

    public interface ISubmitLogEntry
    {
        void WaitForIdle();
        void AddLogEntry(IRawData buffer);
    }
}
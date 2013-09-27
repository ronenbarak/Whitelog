using System.Collections.Generic;
using Whitelog.Core.Binary.FileLog;

namespace Whitelog.Core.Binary.ListWriter
{
    public interface IBufferConsumer
    {
        void Consume(IRawData buffer);
    }

    public interface IListWriter
    {
        byte[] GetListWriterSignature();
        void WriteData(IRawData buffer);
        void WriteData(IEnumerable<IRawData> buffer);
        
        bool Read(IBufferConsumer bufferConsumer);
        bool ReadAll(IBufferConsumer objectObserver);

        object LockObject { get; }
        void Flush();
    }
}
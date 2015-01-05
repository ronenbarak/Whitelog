using System.Collections.Generic;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;

namespace Whitelog.Core.Binary.Serializer
{
    public interface IListWriter
    {
        byte[] GetListWriterSignature();

        void WriteData(IRawData buffer);
        void WriteData(IEnumerable<IRawData> buffer);

        void Flush();
    }
}
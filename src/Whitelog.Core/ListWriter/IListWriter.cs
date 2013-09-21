using System.Collections.Generic;
using Whitelog.Core.FileLog;

namespace Whitelog.Core.ListWriter
{
    public interface IObjectObserver
    {
        void Add(byte[] bytes);
        ICollection<object> GetCollection();
    }

    public interface IListWriter
    {
        byte[] GetListWriterSignature();
        void WriteData(IRawData buffer);
        void WriteData(IEnumerable<IRawData> buffer);
        byte[] Read();
        void ReadAll(IObjectObserver objectObserver);
        object LockObject { get; }
        void Flush();
    }
}
using System.Collections.Generic;

namespace Whitelog.Barak.Common.DataStructures.BulkCollector
{
    public interface IBulkCollector<T>
    {
        void Add(T data);
        IEnumerable<T> GetBulk();
    }
}

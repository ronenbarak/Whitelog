using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Whitelog.Barak.Common.DataStructures.Ring
{
    [StructLayout(LayoutKind.Explicit, Size = 2 * CacheLine.Size)]
    internal struct CacheLineStorageLong
    {
        [FieldOffset(CacheLine.Size)]
        public long _data;
    }
    
    internal static class CacheLine
    {
        ///<summary>
        /// Size of a cache line in bytes
        ///</summary>
        public const int Size = 128;
    }

    public class RingBuffer<T>
    {

        private int _ringModMask;
        private Entry<T>[] _entries;
        private CacheLineStorageLong _sequence = new CacheLineStorageLong();

        public RingBuffer(int size,Func<RingBuffer<T>,T> entryFactory)
        {
            _sequence._data = -1;
            var sizeAsPowerOfTwo = Util.CeilingNextPowerOfTwo(size);
            _ringModMask = sizeAsPowerOfTwo - 1;
            _entries = new Entry<T>[sizeAsPowerOfTwo];

            Fill(entryFactory);
        }

        public long GetNextEntry(out T data)
        {
            var seq = Interlocked.Increment(ref _sequence._data);
            Entry<T> entry = this[seq];
            // Check if i can release the entry
            long prevSeq = seq - _entries.Length;
            SpinWait spinWait = new SpinWait();
            while (!(entry.Sequence == prevSeq && entry.State == Entry<T>.StateOptions.Consumed))
            {
                spinWait.SpinOnce();
            }
            
            entry.State = Entry<T>.StateOptions.Allocated;
            entry.Sequence = seq;
            data = entry.Data;
            return seq;
        }

        public void Commit(long seq)
        {
            this[seq].State = Entry<T>.StateOptions.Commited;
        }

        ///<summary>
        /// Get the <see cref="Entry{T}"/> for a given sequence in the RingBuffer.
        ///</summary>
        ///<param name="sequence">sequence for the <see cref="Entry{T}"/></param>
        public Entry<T> this[long sequence]
        {
            get
            {
                return _entries[(int)sequence & _ringModMask];
            }
        }

        private void Fill(Func<RingBuffer<T>, T> entryFactory)
        {
            for (var i = 0; i < _entries.Length; i++)
            {
                var data = entryFactory(this);
                var entry = new Entry<T>(data);
                entry.State = Entry<T>.StateOptions.Consumed;
                entry.Sequence = i - _entries.Length;
                _entries[i] = entry;
            }   
        }
    }
}

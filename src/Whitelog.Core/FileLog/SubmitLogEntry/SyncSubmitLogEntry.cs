using Whitelog.Core.ListWriter;
using Whitelog.Core.Serializer.MemoryBuffer;

namespace Whitelog.Core.FileLog.SubmitLogEntry
{
    public class SyncSubmitLogEntry : ISubmitLogEntry
    {
        private IListWriter m_listWriter;
        private IBufferAllocator m_bufferAllocator;

        public SyncSubmitLogEntry(IListWriter listWriter, IBufferAllocator bufferAllocator)
        {
            m_bufferAllocator = bufferAllocator;
            m_listWriter = listWriter;
        }

        public IBuffer Allocate()
        {
            return m_bufferAllocator.Allocate();
        }

        public void AddLogEntry(IRawData buffer)
        {
            lock (m_listWriter.LockObject)
            {
                m_listWriter.WriteData(buffer);
                m_listWriter.Flush();
            }
        }

        public void WaitForIdle()
        {
            lock (m_listWriter.LockObject)
            {
            }
        }
    }
}
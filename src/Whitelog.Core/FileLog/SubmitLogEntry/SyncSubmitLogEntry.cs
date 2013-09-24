using Whitelog.Core.ListWriter;
using Whitelog.Core.Serializer.MemoryBuffer;

namespace Whitelog.Core.FileLog.SubmitLogEntry
{
    public class SyncSubmitLogEntry : ISubmitLogEntry
    {
        private IListWriter m_listWriter;        

        public SyncSubmitLogEntry(IListWriter listWriter)
        {
            m_listWriter = listWriter;
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
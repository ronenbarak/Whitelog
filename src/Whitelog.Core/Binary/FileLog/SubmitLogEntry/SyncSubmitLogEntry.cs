using Whitelog.Core.Binary.ListWriter;

namespace Whitelog.Core.Binary.FileLog.SubmitLogEntry
{
    public class SyncSubmitLogEntry : ISubmitLogEntry
    {
        private IListWriter m_listWriter;
        private object m_lockObject = new object();
        public SyncSubmitLogEntry(IListWriter listWriter)
        {
            m_listWriter = listWriter;
        }

        public void AddLogEntry(IRawData buffer)
        {
            lock (m_lockObject)
            {
                m_listWriter.WriteData(buffer);
                m_listWriter.Flush();
            }
        }

        public void WaitForIdle()
        {
        }
    }
}
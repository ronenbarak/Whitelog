using Whitelog.Core.Binary;
using Whitelog.Core.Binary.Serializer;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;

namespace Whitelog.Core.Loggers.Binary.SubmitLogEntry
{
    public class SyncSubmitEntry : ISubmitEntry<IRawData>
    {
        private IListWriter m_listWriter;
        private object m_lockObject = new object();
        public SyncSubmitEntry(IListWriter listWriter)
        {
            m_listWriter = listWriter;
        }

        public void AddEntry(IRawData buffer)
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
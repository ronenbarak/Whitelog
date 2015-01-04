using Whitelog.Core.Binary.ListWriter;

namespace Whitelog.Core.Binary
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
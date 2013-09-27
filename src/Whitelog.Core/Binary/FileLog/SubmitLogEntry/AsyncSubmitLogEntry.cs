using System;
using System.Threading;
using Whitelog.Barak.Common.DataStructures.BulkCollector;
using Whitelog.Core.Binary.ListWriter;

namespace Whitelog.Core.Binary.FileLog.SubmitLogEntry
{
    public class CloneRawData : IRawData
    {
        private byte[] m_newBuffer;

        public CloneRawData(byte[] buffer)
            : this(buffer, buffer.Length)
        {
        }

        public CloneRawData(byte[] buffer, int legnth)
        {
            var newBuffer = new byte[legnth];
            System.Buffer.BlockCopy(buffer, 0, newBuffer, 0, legnth);
            m_newBuffer = newBuffer;
        }

        public int Length
        {
            get { return m_newBuffer.Length; }
        }

        public byte[] Buffer
        {
            get { return m_newBuffer; }
            set
            {
                throw new NotImplementedException("Not Implemented CloneRawData.set_Buffer");
            }
        }
    }

    class AsyncSubmitLogEntry : ISubmitLogEntry
    {
        public AsyncSubmitLogEntry(IListWriter listWriter)
        {
            m_listWriter = listWriter;
        }

        private QueuedBulkCollector<IRawData> m_queuedBulk = new QueuedBulkCollector<IRawData>();
        private int m_pendingBulksToCollect = 0;
        private int m_useCounter = 0;
        private IListWriter m_listWriter;

        public void AddLogEntry(IRawData buffer)
        {
            bool isFirstInQueue =false;
            m_queuedBulk.Add(new CloneRawData(buffer.Buffer,buffer.Length), out isFirstInQueue);
            if (isFirstInQueue)
            {
                int threadCount = Interlocked.Increment(ref m_pendingBulksToCollect);
                if (threadCount == 1)
                {
                    // the application might terminate befor the thread run and we will lose logs
                    // for that resean we have WaitForIdle to insure the log has written into disk.
                    ThreadPool.QueueUserWorkItem(WriteDataToFile);
                }
            }
        }

        private void WriteDataToFile(object state)
        {
            Interlocked.Increment(ref m_useCounter); // This is thread safe without the Interlocked.Increment but its looks better this way
            
            bool bContinue = true;
            while (bContinue)
            {
                var logEntries = m_queuedBulk.GetBulk();
                lock (m_listWriter.LockObject) // we lock the file in case some other thred is trying to use it
                {
                    m_listWriter.WriteData(logEntries);
                }
                bContinue = Interlocked.Decrement(ref m_pendingBulksToCollect) != 0;
            }

            lock (m_listWriter.LockObject)
            {
                m_listWriter.Flush();
            }
            Interlocked.Decrement(ref m_useCounter); // This line should be thread safe.
        }

        public void WaitForIdle()
        {
            while (m_pendingBulksToCollect !=0 || m_useCounter != 0)
            {
                System.Threading.Thread.Sleep(1);
            }
        }
    }
}
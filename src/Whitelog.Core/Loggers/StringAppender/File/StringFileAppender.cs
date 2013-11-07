using System;
using System.Text;
using Whitelog.Core.Binary.FileLog;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;
using Whitelog.Core.Filter;
using Whitelog.Interface;

namespace Whitelog.Core.Loggers.StringAppender.File
{
    public class StringFileAppender : IStringAppender
    {
        private static readonly byte[] newLive = UTF8Encoding.Default.GetBytes(Environment.NewLine);
        private readonly IFilter m_filter;
        private IBufferAllocator m_bufferAllocator;
        private ISubmitLogEntry m_submitLogEntry;
        
        public StringFileAppender(IBufferAllocator bufferAllocator,ISubmitLogEntry submitLogEntry,IFilter filter = null)
        {
            m_submitLogEntry = submitLogEntry;
            m_filter = filter;
            m_bufferAllocator = bufferAllocator;
        }

        public bool Filter(LogEntry logEntry)
        {
            if (m_filter != null)
            {
                return m_filter.Filter(logEntry);
            }
            return false;
        }

        public void Append(string value, LogEntry logEntry)
        {
            // it is extremly inefficent to get a string and than convert it to byte array and than transfer to the submitter
            // but if you want high performance use the binary serilizer.
            using (var buffer = m_bufferAllocator.Allocate())
            {
                buffer.DateTime = logEntry.Time;
                buffer.AttachedSerializer.Serialize(value);
                buffer.AttachedSerializer.Serialize(newLive, 0, newLive.Length);
                buffer.AttachedSerializer.Flush();
                m_submitLogEntry.AddLogEntry(buffer);
            }
        }
    }
}

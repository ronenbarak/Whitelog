using System;
using System.Collections.Concurrent;
using System.IO;

namespace Whitelog.Core.Binary.Deserilizer.Reader
{
    public class WhitelogBinaryReaderFactory
    {
        class DisposeRegistration : IDisposable
        {
            private bool m_isDisposed = false;
            private Action m_action;

            public DisposeRegistration(Action action)
            {
                m_action = action;
            }

            public void Dispose()
            {
                if (!m_isDisposed)
                {
                    m_action.Invoke();
                    m_isDisposed = true;   
                }
            }
        }

        private ConcurrentDictionary<Guid, ILogReaderFactory> m_logReaderFactories = new ConcurrentDictionary<Guid, ILogReaderFactory>();

        public IDisposable RegisterReaderFactory(ILogReaderFactory logReaderFactory)
        {
            m_logReaderFactories.TryAdd(logReaderFactory.Id, logReaderFactory);
            return new DisposeRegistration(() =>
                                           {
                                               ILogReaderFactory temp;
                                               m_logReaderFactories.TryRemove(logReaderFactory.Id, out temp);
                                           });
        }

        public ILogReader GetLogReader(Stream data,ILogConsumer logConsumer,Stream otherData = null)
        {
            data.Position = 0;
            byte[] guidBuffer = new byte[16];
            data.Read(guidBuffer, 0, guidBuffer.Length);
            ILogReaderFactory logreader;
            if (!m_logReaderFactories.TryGetValue(new Guid(guidBuffer), out logreader))
            {
                throw new UnkownLogFileException();
            }
            else
            {
                return logreader.Create(data, logConsumer);
            }
        }
    }
}
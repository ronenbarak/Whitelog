using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Whitelog.Core.Filter;
using Whitelog.Core.String;
using Whitelog.Core.String.Layout;
using Whitelog.Core.String.StringBuffer;

namespace Whitelog.Core.Loggers
{
    public class LayoutLogger : IStringLogger
    {
        private StringLayoutRenderer m_stringLayoutRenderer;
        private readonly IStringBuffer m_stringBuffer;
        private object m_lockObject = new object();
        private IStringAppender[] m_stringAppenders = new IStringAppender[0];
        private readonly IFilter m_filter;
        public string Layout { get; private set; }


        public LayoutLogger(string layout, IStringBuffer stringBuffer)
            : this(layout, stringBuffer, null)
        {
        }

        public LayoutLogger(IStringBuffer stringBuffer):this("${longdate} ${title} ${message}",stringBuffer,null)
        {
        }

        public LayoutLogger(IStringBuffer stringBuffer,IFilter filter)
            : this("${longdate} ${title} ${message}", stringBuffer, filter)
        {
        }

        public LayoutLogger(string layout, IStringBuffer stringBuffer,IFilter filter)
        {
            Layout = layout;
            m_filter = filter;
            m_stringBuffer = stringBuffer;
            m_stringLayoutRenderer = new StringLayoutRenderer(layout);
        }

        public void RegisterLayoutExtensions(IStringLayoutFactory layoutFactory)
        {
            m_stringLayoutRenderer.RegisterLayoutExtensions(layoutFactory);
        }

        public void RegisterLayoutExtensions(IEnumerable<IStringLayoutFactory> layoutFactory)
        {
            foreach (var stringLayoutFactory in layoutFactory)
            {
                m_stringLayoutRenderer.RegisterLayoutExtensions(stringLayoutFactory);   
            }
        }

        public void AttachToTunnelLog(LogTunnel logTunnel)
        {
            logTunnel.LogEntry += logTunnel_LogEntry;
        }

        public void DetachTunnelLog(LogTunnel logTunnel)
        {
            logTunnel.LogEntry -= logTunnel_LogEntry;
        }

        public void AddStringAppender(IStringAppender stringAppender)
        {
            lock (m_lockObject)
            {
                var lst = m_stringAppenders.ToList();
                lst.Add(stringAppender);
                m_stringAppenders = lst.ToArray();
            }
        }

        public void RemoveStringAppender(IStringAppender stringAppender)
        {
            lock (m_lockObject)
            {
                var lst = m_stringAppenders.ToList();
                lst.Remove(stringAppender);
                m_stringAppenders = lst.ToArray();
            }
        }

        void logTunnel_LogEntry(Interface.LogEntry entry)
        {
            if (m_filter != null)
            {
                if (m_filter.Filter(entry))
                {
                    return;
                }
            }

            // We dont render if no appender exsist
            // we dont render if no appender pass filter
            var tempAppender = m_stringAppenders;
            for (int i = 0; i < tempAppender.Length; i++)
            {
                if (!tempAppender[i].Filter(entry))
                {
                    string renderedString = null;
                    using (var buffer = m_stringBuffer.Allocate())
                    {
                        m_stringLayoutRenderer.Render(entry, buffer.StringBuilder);
                        renderedString = buffer.StringBuilder.ToString();
                    }
                    tempAppender[i].Append(renderedString,entry);
                    for (int j = i +1; j < tempAppender.Length; j++)
                    {
                        if (!tempAppender[j].Filter(entry))
                        {
                            tempAppender[j].Append(renderedString,entry);
                        }
                    }
                    break;
                }
            }
        }

        public void RegisterDefinition(IStringPackageDefinition packageDefinition)
        {
            m_stringLayoutRenderer.RegisterDefinition(packageDefinition);
        }
    }
}

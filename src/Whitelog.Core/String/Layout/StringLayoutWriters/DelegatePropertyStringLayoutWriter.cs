using System;
using System.Text;
using Whitelog.Interface;

namespace Whitelog.Core.String.Layout.StringLayoutWriters
{
    public class DelegatePrimitivePropertyStringLayoutWriter : IStringLayoutWriter
    {
         private readonly Delegate m_parmExtractor;
        private readonly string m_property;

        public DelegatePrimitivePropertyStringLayoutWriter(string property, Delegate parmExtractor)
        {
            m_property = property + "=";
            m_parmExtractor = parmExtractor;
        }

        public void Render(StringBuilder stringBuilder, IStringRenderer stringRenderer, LogEntry logEntry)
        {
            stringBuilder.Append(m_property);
            if (logEntry.Paramaeter != null)
            {
                stringBuilder.Append(m_parmExtractor.DynamicInvoke(logEntry.Paramaeter));
            }
        }   
    }

    public class DelegatePropertyStringLayoutWriter : IStringLayoutWriter
    {
        private readonly Delegate m_parmExtractor;
        private readonly string m_property;

        public DelegatePropertyStringLayoutWriter(string property, Delegate parmExtractor)
        {
            m_property = property + "=";
            m_parmExtractor = parmExtractor;
        }

        public void Render(StringBuilder stringBuilder, IStringRenderer stringRenderer, LogEntry logEntry)
        {
            stringBuilder.Append(m_property);
            if (logEntry.Paramaeter != null)
            {
                stringRenderer.Render(m_parmExtractor.DynamicInvoke(logEntry.Paramaeter), stringBuilder);
            }
        }
    }
}
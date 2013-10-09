using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whitelog.Interface;

namespace Whitelog.Core.String.Layout.StringLayoutWriters
{
    public class CompositeStringLayoutWriter : IStringLayoutWriter
    {
        private IStringLayoutWriter[] m_writers = new IStringLayoutWriter[0];
        public void Add(IStringLayoutWriter layoutWriter)
        {
            Array.Resize(ref m_writers, m_writers.Length + 1);
            m_writers[m_writers.Length - 1] = layoutWriter;
        }

        public void Render(StringBuilder stringBuilder,IStringRenderer stringRenderer, LogEntry logEntry)
        {
            for (int i = 0; i < m_writers.Length; i++)
            {
                m_writers[i].Render(stringBuilder, stringRenderer, logEntry);
            }
        }
    }
}

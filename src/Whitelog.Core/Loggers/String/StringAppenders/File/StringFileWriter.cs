using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Whitelog.Core.File;

namespace Whitelog.Core.Loggers.String.StringAppenders.File
{
    public class TextEntry
    {
        public TextEntry(DateTime timestamp,string text)
        {
            TimeStamp = timestamp;
            Text = text;
        }
        public DateTime TimeStamp { get;private set; }
        public string Text { get; private set; }
    }

    public class StringFileWriter
    {        
        private Stream m_stream;
        
        private readonly IStreamProvider m_streamProvider;
        private readonly FileConfiguration m_fileConfiguration;
        private byte[] EOL = Encoding.Unicode.GetBytes(Environment.NewLine);
        private byte[] m_buffer = new byte[1024];

        public StringFileWriter(IStreamProvider streamProvider)
        {
            m_streamProvider = streamProvider;
            m_stream = m_streamProvider.GetStream();
        }

        public void WriteData(TextEntry entry)
        {
            if (m_buffer.Length < entry.Text.Length * 6)
            {
                m_buffer = new byte[entry.Text.Length * 6];
            }

            var size = Encoding.Unicode.GetBytes(entry.Text, 0, entry.Text.Length, m_buffer, 0);

            if (m_streamProvider.ShouldArchive(size, size, entry.TimeStamp))
            {
                m_stream.Close();
                m_stream.Dispose();
                m_streamProvider.Archive();
                m_stream = m_streamProvider.GetStream();
            }
            
            m_stream.Write(m_buffer, 0, size);
            m_stream.Write(EOL, 0, EOL.Length);// End of line
        }

        public void WriteData(IEnumerable<TextEntry> entries)
        {
            foreach (var entry in entries)
            {
                WriteData(entry);
            }
        }
        
        public void Flush()
        {
            m_stream.Flush();
        }
    }
}
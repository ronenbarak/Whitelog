using System;
using System.IO;

namespace Whitelog.Core.File
{
    public class InMemoryStreamProvider : IStreamProvider
    {
        private Stream m_stream;

        public InMemoryStreamProvider():this(new MemoryStream())
        {
            
        }
        public InMemoryStreamProvider(Stream stream)
        {
            m_stream = stream;
        }

        public Stream GetStream()
        {
            return m_stream;
        }

        public bool ShouldArchive(long currSize, int bytesToAdd, DateTime now)
        {
            return false;
        }

        public void Archive()
        {
            throw new NotImplementedException();
        }
    }
}
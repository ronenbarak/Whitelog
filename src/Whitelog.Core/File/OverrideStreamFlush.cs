using System;
using System.IO;
using System.Runtime.Remoting;
using System.Threading;

namespace Whitelog.Core.File
{
    class OverrideStreamFlush : Stream
    {
        private FileStream m_fileStream;

        public OverrideStreamFlush(FileStream fileStream)
        {
            m_fileStream = fileStream;
        }

        public override void Flush()
        {
            m_fileStream.Flush(true);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return m_fileStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            m_fileStream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return m_fileStream.Read(buffer,offset,count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            m_fileStream.Write(buffer, offset, count);
        }

        public override bool CanRead
        {
            get { return m_fileStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return m_fileStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return m_fileStream.CanWrite; }
        }

        public override long Length
        {
            get { return m_fileStream.Length; }
        }

        public override long Position
        {
            get { return m_fileStream.Position; }
            set { m_fileStream.Position = value; }
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return m_fileStream.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return m_fileStream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override bool CanTimeout
        {
            get { return m_fileStream.CanTimeout; }
        }

        public override void Close()
        {
            m_fileStream.Close();
        }

        public override ObjRef CreateObjRef(Type requestedType)
        {
            return m_fileStream.CreateObjRef(requestedType);
        }

        protected override void Dispose(bool disposing)
        {
            m_fileStream.Dispose();
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return m_fileStream.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            m_fileStream.EndWrite(asyncResult);
        }

        protected override WaitHandle CreateWaitHandle()
        {
            return new ManualResetEvent(false);
        }

        public override bool Equals(object obj)
        {
            return m_fileStream.Equals(obj);
        }

        public override int GetHashCode()
        {
            return m_fileStream.GetHashCode();
        }

        public override object InitializeLifetimeService()
        {
            return m_fileStream.InitializeLifetimeService();
        }

        protected override void ObjectInvariant()
        {            
        }

        public override int ReadByte()
        {
            return m_fileStream.ReadByte();
        }

        public override int ReadTimeout { get { return m_fileStream.ReadTimeout; } set {m_fileStream.ReadTimeout =value;} }
        public override string ToString()
        {
            return m_fileStream.ToString();
        }

        public override void WriteByte(byte value)
        {
            m_fileStream.WriteByte(value);
        }

        public override int WriteTimeout { get { return m_fileStream.WriteTimeout; } set { m_fileStream.WriteTimeout = value; } }
    }

}
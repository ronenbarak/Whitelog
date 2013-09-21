using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using Whitelog.Core.FileLog;

namespace Whitelog.Core.ListWriter
{
    public class FixSizeList : IListWriter
    {
        public static readonly Guid ID = new Guid("D1251307-1097-4AD6-A553-F04D7B388580");
        private static byte[] EndOfList = BitConverter.GetBytes(-1);
        private Stream m_stream;
        private long m_endPosition;
        private long m_currentPosition;
        private long m_startPosition;

        public FixSizeList(Stream stream,long startPosition,long endPosition,bool newFile)
        {
            m_currentPosition = m_startPosition = startPosition;
            m_endPosition = endPosition;
            m_stream = stream;

            if (m_stream.Length < endPosition)
            {
                m_stream.SetLength(endPosition);
            }

            if (newFile)
            {
                MoveToLastPosition();
                m_stream.Write(EndOfList, 0, sizeof(int));
            }

        }

        public void WriteData(IRawData buffer)
        {
            if (m_currentPosition + buffer.Length + sizeof(int) * 2 > m_endPosition)
            {
                throw  new Exception("Out of space");
            }

            MoveToLastPosition();
            m_stream.Write(BitConverter.GetBytes(buffer.Length), 0, sizeof(int));
            m_stream.Write(buffer.Buffer, 0, buffer.Length);
            m_currentPosition = m_stream.Position;
            m_stream.Write(EndOfList, 0, sizeof(int));
        }

        public void WriteData(IEnumerable<IRawData> buffer)
        {
            MoveToLastPosition();
            foreach (var currBuffer in buffer)
            {
                if (m_stream.Position + currBuffer.Length + sizeof(int) * 2 > m_endPosition)
                {
                    throw new Exception("Out of space");
                }
                m_stream.Write(BitConverter.GetBytes(currBuffer.Length), 0, sizeof(int));
                m_stream.Write(currBuffer.Buffer, 0, currBuffer.Length);
            }
            m_currentPosition = m_stream.Position;
            m_stream.Write(EndOfList, 0, sizeof(int));
        }

        private void MoveToLastPosition()
        {
            m_stream.Position = m_currentPosition;
        }

        public byte[] Read()
        {
            lock (LockObject)
            {
                MoveToLastPosition();
                // Check if there is some thing to read
                //byte[] mainBuffer = m_bufferAllocator.Allocate();
                byte[] intSizeBuffer = new byte[sizeof (int)];
                if (m_stream.Read(intSizeBuffer, 0, sizeof(int)) == sizeof(int))
                {
                    int sizeBuffer = BitConverter.ToInt32(intSizeBuffer, 0);
                    if (sizeBuffer != -1)
                    {
                        //byte[] dataBuffer = mainBuffer;
                        //if (mainBuffer.Length >= sizeBuffer)
                        //{
                          var dataBuffer = new byte[sizeBuffer];
                        //}
                        if (m_stream.Read(dataBuffer, 0, sizeBuffer) != sizeBuffer)
                        {
                            throw new CorruptedDataException();
                        }
                        else
                        {
                            m_currentPosition = m_stream.Position;
                            return dataBuffer;
                        }
                    }
                }
                return null;
            }
        }

        public void ReadAll(IObjectObserver objectObserver)
        {
            byte[] buffer = null;
            while ((buffer = Read()) != null)
            {
                objectObserver.Add(buffer);
            }
        }


        public object LockObject
        {
            get { return m_stream; }
        }


        public void Flush()
        {
            m_stream.Flush();
        }

        public byte[] GetListWriterSignature()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(ID.ToByteArray(), 0, ID.ToByteArray().Length);
                ms.Write(BitConverter.GetBytes(m_startPosition),0,sizeof(long));
                ms.Write(BitConverter.GetBytes(m_endPosition), 0, sizeof(long));
                return ms.ToArray();
            }
        }
    }
}
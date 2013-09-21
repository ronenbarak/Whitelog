using System.Collections.Generic;
using Whitelog.Core.ListWriter;
using Whitelog.Core.Serializer;

namespace Whitelog.Core
{
    public class LogReaderObjectObserver : IObjectObserver
    {
        private Unpacker m_unpacker;
        private List<object> objectList = new List<object>();
        public LogReaderObjectObserver(Unpacker unpacker)
        {
            m_unpacker = unpacker;
        }

        public void Add(byte[] bytes)
        {
            using (StreamDeserilizer ms = new StreamDeserilizer(bytes))
            {
                var data = m_unpacker.Unpack<ILogEntryData>(ms);
                if (data != null)
                {
                    objectList.Add(data);
                }
            }
        }

        public ICollection<object> GetCollection()
        {
            return objectList;
        }
    }
}
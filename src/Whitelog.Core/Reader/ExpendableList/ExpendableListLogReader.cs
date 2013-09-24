using System;
using System.Collections.Generic;
using Whitelog.Barak.Common.Events;
using Whitelog.Core.FileLog;
using Whitelog.Core.FileLog.SubmitLogEntry;
using Whitelog.Core.Generic;
using Whitelog.Core.ListWriter;
using Whitelog.Core.PakageDefinitions.Unpack;
using Whitelog.Core.Serializer;

namespace Whitelog.Core.Reader.ExpendableList
{
    public class ExpendableLogReader : IBufferConsumer
    {
        public void Consume(IRawData buffer)
        {
        }
    }

    public class ExpendableListLogReader : ILogReader, IBufferConsumer
    {        
        private readonly ILogConsumer m_consumer;
        private IListWriter m_listWriter;
        private Unpacker m_dataUnpacker = new Unpacker();
        private StreamDeserilizer m_deserilizer = new StreamDeserilizer();
        private int m_currentPendingIndex = 0;
        private Queue<Tuple<int,int, IRawData>> m_pendingDefinitonQueue = new Queue<Tuple<int,int, IRawData>>();
        private Queue<Tuple<int, ILogEntryData>> m_pendingUnpackedObjects = new Queue<Tuple<int, ILogEntryData>>();
        private bool m_tryToDequeuPendingItems = false;
        public ExpendableListLogReader(ILogConsumer consumer,IListWriter listWriter)
        {
            m_listWriter = listWriter;
            m_consumer = consumer;
            BinaryPackageDefinitionToGenericUnpackageDefinition packageDefinitionToGenericUnpackageDefinition = new BinaryPackageDefinitionToGenericUnpackageDefinition();
            m_dataUnpacker.AddPackageDefinition(packageDefinitionToGenericUnpackageDefinition);
            m_dataUnpacker.AddPackageDefinition(new GenericPropertyUnpackageDefinition());
            m_dataUnpacker.AddPackageDefinition(new ConstGenericPropertyUnpackageDefinition());
            var cacheStringUnpackageDefinition = new CacheStringUnpackageDefinition();
            m_dataUnpacker.AddPackageDefinition(cacheStringUnpackageDefinition);

            packageDefinitionToGenericUnpackageDefinition.PackageDefinitionRegistred += OnPackageDefinitionRegistred;
            cacheStringUnpackageDefinition.CacheStringDeserializer += OnCacheStringDeserializer;
        }

        private void OnCacheStringDeserializer(object sender, EventArgs<CacheString> e)
        {
            m_dataUnpacker.SetCachedString(e.Data.Id, e.Data.Value);
            if (m_currentPendingIndex != 0)
            {
                if (m_pendingDefinitonQueue.Peek().Item2 == e.Data.Id)
                {
                    m_tryToDequeuPendingItems = true;
                }
            }
        }

        private void OnPackageDefinitionRegistred(object sender, EventArgs<GenericUnpackageDefinition> eventArgs)
        {
            m_dataUnpacker.AddPackageDefinition(eventArgs.Data);
            if (m_currentPendingIndex != 0)
            {
                if (m_pendingDefinitonQueue.Peek().Item2 == eventArgs.Data.DefinitionId)
                {
                    m_tryToDequeuPendingItems = true;
                }
            }
        }

        public bool TryRead()
        {
            return m_listWriter.ReadAll(this);
        }

        void IBufferConsumer.Consume(IRawData buffer)
        {
            m_deserilizer.Init(buffer.Buffer);
            try
            {
                var data = m_dataUnpacker.Unpack<ILogEntryData>(m_deserilizer);
                if (m_tryToDequeuPendingItems)
                {
                    TryDequeuPendingItems();
                }
                if (data != null)
                {
                    if (m_currentPendingIndex != 0)
                    {
                        m_pendingUnpackedObjects.Enqueue(new Tuple<int, ILogEntryData>(m_currentPendingIndex, data));
                        m_currentPendingIndex++;
                    }
                    else
                    {
                        m_consumer.Consume(data);
                    }
                }
            }
            catch (UnkownPackageException unkownPackageException)
            {
                m_pendingDefinitonQueue.Enqueue(new Tuple<int, int, IRawData>(m_currentPendingIndex,
                    unkownPackageException.PackageId, new CloneRawData(buffer.Buffer, buffer.Length)));
                m_currentPendingIndex++;
            }
            catch (CacheStringNotFoundException cacheStringNotFoundException)
            {
                m_pendingDefinitonQueue.Enqueue(new Tuple<int, int, IRawData>(m_currentPendingIndex,cacheStringNotFoundException.Id, new CloneRawData(buffer.Buffer, buffer.Length)));
                m_currentPendingIndex++;
            }
        }

        private void TryDequeuPendingItems()
        {
            m_tryToDequeuPendingItems = false;
            while (m_pendingDefinitonQueue.Count != 0)
            {
                var currentDequeueItem = m_pendingDefinitonQueue.Peek();
                if (m_pendingUnpackedObjects.Count != 0 && m_pendingUnpackedObjects.Peek().Item1 < currentDequeueItem.Item1)
                {
                    m_consumer.Consume(m_pendingUnpackedObjects.Dequeue().Item2);
                }
                else
                {
                    m_deserilizer.Init(currentDequeueItem.Item3.Buffer);
                    try
                    {
                        var data = m_dataUnpacker.Unpack<ILogEntryData>(m_deserilizer);
                        if (data != null)
                        {
                            m_consumer.Consume(data);
                        }

                        m_pendingDefinitonQueue.Dequeue();
                    }
                    catch (UnkownPackageException unkownPackageException)
                    {
                        break; // stop try to desilize
                    }
                    catch (CacheStringNotFoundException cacheStringNotFoundException)
                    {
                        break; // stop try to desilize
                    }
                }
            }
            
            if (m_pendingDefinitonQueue.Count == 0)
            {
                m_currentPendingIndex = 0;
            }
        }
    }
}
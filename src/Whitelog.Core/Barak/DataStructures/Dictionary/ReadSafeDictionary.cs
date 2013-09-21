using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Whitelog.Barak.Common.DataStructures.Dictionary
{
    /// <summary>
    /// Dictionary With TryGetValue Safe for multi thread.
    /// </summary>
    public class ReadSafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        public delegate TValue UpdateFactory(TKey key, TValue oldValue, TValue newValue);
        private class Entry
        {
            public Entry(TKey key,TValue value )
            {
                Key = key;
                Value = value;
            }
            public TKey Key;
            public TValue Value;
            public Entry Next;
        }

        private class DictionaryBuffer
        {
            public readonly Entry[] array;
            public readonly int m_arrayModMask;
            public DictionaryBuffer(Entry[] keyValuePairs, int arrayModMask)
            {
                m_arrayModMask = arrayModMask;
                array = keyValuePairs;
            }
        }

        private static readonly IMaintenanceMode DefaultMaintenanceMode = new IncressSizeOnlyMaintenanceMode(4096, 0.2, 2);
        private readonly IEqualityComparer<TKey> m_comparer;
        private readonly HashSet<Entry> m_items = new HashSet<Entry>();
        private readonly IMaintenanceMode m_maintenanceMode;
        
        private DictionaryBuffer m_dictionaryBuffer;
        private int m_maxDuplicateHashCount;

        public ReadSafeDictionary() : this(EqualityComparer<TKey>.Default, DefaultMaintenanceMode)
        {
        }

        public ReadSafeDictionary(IEqualityComparer<TKey> comparer) : this(comparer, DefaultMaintenanceMode)
        {
        }
        public ReadSafeDictionary(IMaintenanceMode maintenanceMode) : this(EqualityComparer<TKey>.Default, maintenanceMode)
        {
        }
        
        public ReadSafeDictionary(IEqualityComparer<TKey> comparer, IMaintenanceMode maintenanceMode)
        {
            m_maintenanceMode = maintenanceMode;
            m_comparer = comparer;
            Rebuild(m_maintenanceMode.ExpectedSize(0, 0, 0));
        }

        private static int CeilingNextPowerOfTwo(int x)
        {
            var result = 2;
            while (result < x)
            {
                result *= 2;
            }
            return result;
        }

        /// <summary>
        /// This is the only safe method
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            DictionaryBuffer dictionaryBuffer = m_dictionaryBuffer;
            int keySequence = m_comparer.GetHashCode(key);
            var node = dictionaryBuffer.array[keySequence & dictionaryBuffer.m_arrayModMask];
            while(node != null)
            {
                if (m_comparer.Equals(key, node.Key))
                {
                    value = node.Value;
                    return true;
                }
                node = node.Next;
            }
            value = default(TValue);
            return false;
        }


        public bool TryAdd(TKey key, TValue value)
        {
            DictionaryBuffer buffer = m_dictionaryBuffer;
            return TryAdd(new KeyValuePair<TKey, TValue>(key, value), buffer, true);
        }

        public void AddOrUpdate(TKey key, TValue value, UpdateFactory updateFactory)
        {
            DictionaryBuffer dictionaryBuffer = m_dictionaryBuffer;
            int keySequence = m_comparer.GetHashCode(key);
            Entry node = dictionaryBuffer.array[keySequence & dictionaryBuffer.m_arrayModMask];
            int level = 1;
            if (node == null)
            {
                node = new Entry(key, value);
                dictionaryBuffer.array[keySequence & dictionaryBuffer.m_arrayModMask] = node;
            }
            else
            {
                Entry prevNode = null;
                // Check if the key already exsist
                // And move to the last item
                while (node != null)
                {
                    level++;
                    if (m_comparer.Equals(key, node.Key))
                    {
                        // Replate the current node
                        node.Value = updateFactory.Invoke(key, node.Value, value);
                        return; // Exit the funciton we are done here
                    }

                    prevNode = node;
                    node = node.Next;
                }
                level++;
                prevNode.Next = node = new Entry(key, value);
            }

            m_maxDuplicateHashCount = Math.Max(m_maxDuplicateHashCount, level);

            m_items.Add(node);

            int expectedSize = m_maintenanceMode.ExpectedSize(dictionaryBuffer.array.Length, m_items.Count, m_maxDuplicateHashCount);
            if (expectedSize != dictionaryBuffer.array.Length)
            {
                Rebuild(expectedSize);
            }
        }

        public bool TryRemove(TKey key)
        {
            DictionaryBuffer dictionaryBuffer = m_dictionaryBuffer;
            int keySequence = m_comparer.GetHashCode(key);
            Entry node = dictionaryBuffer.array[keySequence & dictionaryBuffer.m_arrayModMask];
            Entry prevnode = null;
            while (node != null)
            {
                if (m_comparer.Equals(key, node.Key))
                {
                    m_items.Remove(node);
                    int expectedSize = m_maintenanceMode.ExpectedSize(dictionaryBuffer.array.Length, m_items.Count, m_maxDuplicateHashCount);
                    if (expectedSize != dictionaryBuffer.array.Length)
                    {
                        Rebuild(expectedSize);
                    }
                    else
                    {
                        if (prevnode == null)
                        {
                            dictionaryBuffer.array[keySequence & dictionaryBuffer.m_arrayModMask] = node.Next;
                        }
                        else
                        {
                            prevnode.Next = node.Next;
                        }
                    }
                    return true;
                }
                prevnode = node;
                node = node.Next;
            }
            return false;
        }

        private bool TryAdd(KeyValuePair<TKey, TValue> newItem, DictionaryBuffer dictionaryBuffer, bool addItem)
        {
            int keySequence = m_comparer.GetHashCode(newItem.Key);
            Entry node = dictionaryBuffer.array[keySequence & dictionaryBuffer.m_arrayModMask];
            int level = 1;
            if (node == null)
            {
                node = new Entry(newItem.Key, newItem.Value);
                dictionaryBuffer.array[keySequence & dictionaryBuffer.m_arrayModMask] = node;
            }
            else
            {
                Entry prevNode = null;
                // Check if the key already exsist
                // And move to the last item
                while (node != null)
                {
                    level++;
                    if (m_comparer.Equals(newItem.Key,node.Key))
                    {
                        return false;
                    }
                    
                    prevNode = node;
                    node = node.Next;
                }
                level++;
                prevNode.Next = node =new Entry(newItem.Key, newItem.Value);
            }
            m_maxDuplicateHashCount = Math.Max(m_maxDuplicateHashCount, level);

            if (addItem)
            {
                m_items.Add(node);
            }

            int expectedSize = m_maintenanceMode.ExpectedSize(dictionaryBuffer.array.Length, m_items.Count, m_maxDuplicateHashCount);
            if (expectedSize != dictionaryBuffer.array.Length)
            {
                Rebuild(expectedSize);
            }
            return true;
        }

        private void Rebuild(int size)
        {
            int sizeAsPowerOfTwo = CeilingNextPowerOfTwo(size);
            int arrayModMask = sizeAsPowerOfTwo - 1;
            Entry[] array = new Entry[sizeAsPowerOfTwo];
            m_maxDuplicateHashCount = 0;
            DictionaryBuffer dicBuffer = new DictionaryBuffer(array, arrayModMask);
            foreach (var currItem in m_items)
            {
                TryAdd(new KeyValuePair<TKey, TValue>(currItem.Key, currItem.Value), dicBuffer, false);
            }
            m_dictionaryBuffer = dicBuffer;
        }

        public void Clear()
        {
            DictionaryBuffer buff = m_dictionaryBuffer;
            for (int i = 0; i < m_dictionaryBuffer.array.Length; i++)
            {
                buff.array[i] = null;
            }
            m_maxDuplicateHashCount = 0;
            m_items.Clear();
        }

        #region IDictionary

        public void Add(TKey key, TValue value)
        {
            if (!TryAdd(key, value))
            {
                throw new ArgumentException("An element with the same key already exists in the Dictionary");
            }
        }

        public bool ContainsKey(TKey key)
        {
            TValue value;
            return TryGetValue(key, out value);
        }

        public ICollection<TKey> Keys
        {
            get { return new List<TKey>(System.Linq.Enumerable.Select(m_items, x => x.Key)); }
        }

        public bool Remove(TKey key)
        {
            return TryRemove(key);
        }

        public ICollection<TValue> Values
        {
            get { return new List<TValue>(System.Linq.Enumerable.Select(m_items, x => x.Value)); }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (TryGetValue(key, out value))
                {
                    return value;
                }
                throw new KeyNotFoundException();
            }
            set
            {
                AddOrUpdate(key, value, (key1, oldValue, newValue) => newValue );
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            TryAdd(item.Key, item.Value);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            return TryGetValue(item.Key, out value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            int count = arrayIndex; 
            foreach (var entry in m_items)
            {
                array[count] = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
                count++;
            }
        }

        public int Count
        {
            get { return m_items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return TryRemove(item.Key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return m_items.Select(p => new KeyValuePair<TKey, TValue>(p.Key,p.Value)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_items.Select(p => new KeyValuePair<TKey, TValue>(p.Key, p.Value)).GetEnumerator();
        }

        #endregion #region IDictionary
    }
}
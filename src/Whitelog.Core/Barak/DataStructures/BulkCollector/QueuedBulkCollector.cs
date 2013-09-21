using System.Collections.Generic;
using System.Threading;

namespace Whitelog.Barak.Common.DataStructures.BulkCollector
{
    public class QueuedBulkCollector<T> : IBulkCollector<T>
    {
        class Node
        {
            public readonly T value;
            // http://www.codeproject.com/Articles/31283/Volatile-fields-in-NET-A-look-inside
            public Node next; // Will not work on Itanium Hardware
            public bool IsHead;
            public Node(T data)
            {
                value = data;
            }

            public Node()
            {
            }


        }



        private Node m_last;
        private Node m_head;
        private object m_lockObject = new object();

        public QueuedBulkCollector()
        {
            m_head = new Node()
                         {
                             IsHead = true,
                         };
            m_last = m_head;
        }

        public void Add(T data)
        {
            var node = new Node(data);
            var prev = Interlocked.Exchange(ref m_last, node);
            prev.next = node;
        }

        public void Add(T data,out bool isHead)
        {
            var node = new Node(data);
            var prev = Interlocked.Exchange(ref m_last, node);
            prev.next = node;
            isHead = prev.IsHead;
        }

        class Enumerable : IEnumerable<T>
        {
            private Node m_last;
            private Node m_head;

            public Enumerable(Node head, Node last)
            {
                m_head = head;
                m_last = last;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return new Enumerator(m_head, m_last);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return new Enumerator(m_head, m_last);
            }
        }

        class Enumerator : IEnumerator<T>
        {
            private readonly Node m_last;
            private Node m_current;
            private readonly Node m_head;

            public Enumerator(Node head, Node last)
            {
                m_last = last;
                m_head = head;
                m_current = m_head;
            }

            public T Current
            {
                get { return m_current.value; }
            }

            public void Dispose()
            {
            }

            object System.Collections.IEnumerator.Current
            {
                get { return this; }
            }

            public bool MoveNext()
            {
                if (m_current == m_last)
                {
                    return false;
                }
                else
                {
                    if (m_current.next == null)
                    {
                        SpinWait s = new SpinWait();
                        while (m_current.next == null)
                        {
                            s.SpinOnce();
                        }
                    }
                    m_current = m_current.next;
                    return true;
                }
            }

            public void Reset()
            {
                m_current = m_head;
            }
        }

        public IEnumerable<T> GetBulk()
        {
            Node lastNode = null;
            Node lastHead = null;

            var head = new Node()
                           {
                               IsHead = true,
                           };
            lock (m_lockObject)
            {
                lastHead = m_head;
                m_head = head;
                lastNode = Interlocked.Exchange(ref m_last, head);
            }

            return new Enumerable(lastHead, lastNode);
        }
    }
}
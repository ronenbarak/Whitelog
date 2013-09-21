using System.Collections.Generic;
using System.Threading;

namespace Whitelog.Barak.Common.DataStructures.BulkCollector
{
    public class StackBulkCollector<T> : IBulkCollector<T>
    {
        class Node
        {
            public readonly T value;
            
            private bool m_isSet;
            private Node m_prev;

            public Node(T data)
            {
                value = data;
            }

            public Node()
            {
            }

            public Node Prev
            {
                set
                {
                    m_prev = value;
                    m_isSet = true;
                }

                get
                {
                    if (!m_isSet)
                    {
                        SpinWait s = new SpinWait();
                        while (!m_isSet)
                        {
                            s.SpinOnce();
                        }
                    }

                    return m_prev;
                }
            }
        }

        class Enumerable : IEnumerable<T>
        {
            private Node m_last;

            public Enumerable(Node last)
            {
                m_last = last;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return new Enumerator(m_last);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return new Enumerator(m_last);
            }
        }

        class Enumerator : IEnumerator<T>
        {
            private readonly Node m_last;
            private Node m_current;

            public Enumerator(Node last)
            {
                var node = m_current = m_last = new Node();
                node.Prev = last;
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
                if (m_current == null)
                {
                    return false;
                }

                m_current = m_current.Prev;
                return m_current != null;
            }

            public void Reset()
            {
                m_current = m_last;
            }
        }

        private Node m_last;

        public void Add(T data)
        {
            var node = new Node(data);
            node.Prev = Interlocked.Exchange(ref m_last, node);
        }

        public IEnumerable<T> GetBulk()
        {
            var last = Interlocked.Exchange(ref m_last, null);
            return new Enumerable(last);
        }
    }
}
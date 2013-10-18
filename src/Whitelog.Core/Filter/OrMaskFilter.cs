using Whitelog.Interface;

namespace Whitelog.Core.Filter
{
    public class OrMaskFilter : IFilter
    {
        private readonly IFilter[] m_filters;

        public OrMaskFilter(params IFilter[] filters)
        {
            m_filters = filters;
        }

        public bool Filter(LogEntry logEntry)
        {
            for (int i = 0; i < m_filters.Length; i++)
            {
                if (m_filters[i].Filter(logEntry))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
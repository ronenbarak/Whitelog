using Whitelog.Interface;

namespace Whitelog.Core.Filter
{
    public class AndMaskFilter : IFilter
    {
        private readonly IFilter[] m_filters;

        public AndMaskFilter(params IFilter[] filters)
        {
            m_filters = filters;
        }

        public bool Filter(LogEntry logEntry)
        {
            for (int i = 0; i < m_filters.Length; i++)
            {
                if (!m_filters[i].Filter(logEntry))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
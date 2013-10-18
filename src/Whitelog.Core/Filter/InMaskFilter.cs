using Whitelog.Interface;
using Whitelog.Interface.LogTitles;

namespace Whitelog.Core.Filter
{
    public class InMaskFilter : IFilter
    {
        private long m_mask;

        public InMaskFilter(params long[] values)
        {
            m_mask = 0;

            foreach (var value in values)
            {
                if ((value & ReservedLogTitleIds.ReservedLogTitle) == ReservedLogTitleIds.ReservedLogTitle)
                {
                    m_mask |= (value ^ ReservedLogTitleIds.ReservedLogTitle);
                }
            }
        }

        public bool Filter(LogEntry logEntry)
        {
            if ((logEntry.Title.Id & ReservedLogTitleIds.ReservedLogTitle) == ReservedLogTitleIds.ReservedLogTitle)
            {
                return (logEntry.Title.Id & m_mask) == 0;   
            }
            return true;
        }
    }
}
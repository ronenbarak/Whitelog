using System.Collections.Generic;
using System.Linq;
using Whitelog.Core.Filter;
using Whitelog.Interface.LogTitles;

namespace Whitelog.Core.Configuration.Fluent
{
    public interface IFilterBuilder<T>
    {
        T Include(params LogTitles[] logTitles);
        T Exclude(params LogTitles[] logTitles);
        T Custom(IFilter filter);
    }

    class FilterBuilder<T> : IFilterBuilder<T>
    {
        private readonly T m_returnObject;
        private IFilter m_filter = null;
        private static readonly Dictionary<LogTitles, long> m_titleToId = new Dictionary<LogTitles, long>()
                                                                         {
                                                                             {LogTitles.Close, ReservedLogTitleIds.Close},
                                                                             {LogTitles.Debug, ReservedLogTitleIds.Debug},
                                                                             {LogTitles.Error, ReservedLogTitleIds.Error},
                                                                             {LogTitles.Fatal, ReservedLogTitleIds.Fatal},
                                                                             {LogTitles.Info, ReservedLogTitleIds.Info},
                                                                             {LogTitles.Open, ReservedLogTitleIds.Open},
                                                                             {LogTitles.Trace,ReservedLogTitleIds.Trace},
                                                                             {LogTitles.Warning, ReservedLogTitleIds.Warning},
                                                                         };

        public FilterBuilder(T returnObject)
        {
            m_returnObject = returnObject;
        }

        public IFilter Build()
        {
            return m_filter;
        }

        public T Include(params LogTitles[] logTitles)
        {
            m_filter = new InclusiveMaskFilter(logTitles.Select(p => m_titleToId[p]).ToArray());
            return m_returnObject;
        }

        public T Exclude(params LogTitles[] logTitles)
        {
            m_filter = new ExclusiveMaskFilter(logTitles.Select(p => m_titleToId[p]).ToArray());
            return m_returnObject;
        }

        public T Custom(IFilter filter)
        {
            m_filter = filter;
            return m_returnObject;
        }
    }
}

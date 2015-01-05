using System;
using System.Collections.Generic;
using Whitelog.Core.Loggers;
using Whitelog.Core.Loggers.String;
using Whitelog.Core.String.Layout;
using Whitelog.Core.String.Layout.StringLayoutFactory;

namespace Whitelog.Core.Configuration.Fluent.StringLayout
{
    class LayoutExtensions<T> : ILayoutExtensions<T>
    {
        private static readonly ObjectStringLayoutFactory m_astrix = new ObjectStringLayoutFactory();
        private static readonly TitleStringLayoutFactory m_title = new TitleStringLayoutFactory();
        private static readonly DateStringLayoutFactory m_date = new DateStringLayoutFactory();
        private static readonly ThreadIdStringLayoutFactory m_threadId = new ThreadIdStringLayoutFactory();

        private HashSet<IStringLayoutFactory> m_extensions = new HashSet<IStringLayoutFactory>();
        private T m_source;

        public LayoutExtensions(T source)
        {
            m_source = source;
        }

        public void Initilze(StringLayoutLogger stringLayoutLogger)
        {
            foreach (var stringLayoutFactory in m_extensions)
            {
                stringLayoutLogger.RegisterLayoutExtensions(stringLayoutFactory);
            }
        }

        public T All
        {
            get
            {
                var x = this.Astrix;
                x = this.Title;
                x = this.LongDate;
                x = this.ThreadId;

                return m_source;
            }
        }

        public T Astrix
        {
            get
            {
                m_extensions.Add(m_astrix);
                return m_source;
            }
        }

        public T Title
        {
            get
            {
                m_extensions.Add(m_title);
                return m_source;
            }
        }
        
        public T LongDate
        {
            get
            {
                m_extensions.Add(m_date);
                return m_source;
            }
        }
        public T ThreadId
        {
            get
            {
                m_extensions.Add(m_threadId);
                return m_source;
            }
        }

        public T Custom(IStringLayoutFactory layoutFactory)
        {
            m_extensions.Add(layoutFactory);
            return m_source;
        }

        public T AddCustom(IStringLayoutFactory layoutFactory)
        {
            m_extensions.Add(layoutFactory);
            return m_source;
        }
    }
}
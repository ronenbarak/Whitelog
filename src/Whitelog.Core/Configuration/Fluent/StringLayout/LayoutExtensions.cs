using System;
using System.Collections.Generic;
using Whitelog.Core.Loggers;
using Whitelog.Core.String.Layout;
using Whitelog.Core.String.Layout.StringLayoutFactory;

namespace Whitelog.Core.Configuration.Fluent.StringLayout
{
    class LayoutExtensions : ILayoutExtensions
    {
        private static readonly ObjectStringLayoutFactory m_astrix = new ObjectStringLayoutFactory();
        private static readonly TitleStringLayoutFactory m_title = new TitleStringLayoutFactory();
        private static readonly DateStringLayoutFactory m_date = new DateStringLayoutFactory();
        private static readonly ThreadIdStringLayoutFactory m_threadId = new ThreadIdStringLayoutFactory();

        private HashSet<IStringLayoutFactory> m_extensions = new HashSet<IStringLayoutFactory>();

        public LayoutExtensions()
        {
        }

        public void Initilze(LayoutLogger layoutLogger)
        {
            foreach (var stringLayoutFactory in m_extensions)
            {
                layoutLogger.RegisterLayoutExtensions(stringLayoutFactory);
            }
        }

        public ILayoutExtensions All
        {
            get
            {
                return Astrix.Title.LongDate.ThreadId;
            }
        }

        public ILayoutExtensions Astrix
        {
            get
            {
                m_extensions.Add(m_astrix);
                return this;
            }
        }

        public ILayoutExtensions Title
        {
            get
            {
                m_extensions.Add(m_title);
                return this;
            }
        }
        
        public ILayoutExtensions LongDate
        {
            get
            {
                m_extensions.Add(m_date);
                return this;
            }
        }
        public ILayoutExtensions ThreadId
        {
            get
            {
                m_extensions.Add(m_threadId);
                return this;
            }
        }
        public ILayoutExtensions AddCustom(IStringLayoutFactory layoutFactory)
        {
            m_extensions.Add(layoutFactory);
            return this;
        }
    }
}
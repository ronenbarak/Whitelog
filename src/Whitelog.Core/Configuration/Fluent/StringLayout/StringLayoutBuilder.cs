using System;
using System.Collections.Generic;
using Whitelog.Core.Configuration.Fluent.StringLayout.File;
using Whitelog.Core.Filter;
using Whitelog.Core.Loggers;
using Whitelog.Core.Loggers.String;
using Whitelog.Core.PackageDefinitions;
using Whitelog.Core.String;
using Whitelog.Core.String.StringBuffer;

namespace Whitelog.Core.Configuration.Fluent.StringLayout
{
    class StringLayoutBuilder : IStringLayoutBuilder,IStringOutputTo, ILoggerBuilder, IFilterBuilder<IStringLayoutBuilder>
    {
        class CustomStringAppenderBuilder : IStringAppenderBuilder
        {
            private IStringAppender m_stringAppender;

            public CustomStringAppenderBuilder(IStringAppender stringAppender)
            {
                m_stringAppender = stringAppender;
            }

            public IStringAppender Build()
            {
                return m_stringAppender;
            }
        }

        private string m_layout = null;

        private List<IStringAppenderBuilder> m_stringAppenders = new List<IStringAppenderBuilder>();
        public List<IStringPackageDefinition> m_definitions = new List<IStringPackageDefinition>();
        private LayoutExtensions<IStringLayoutBuilder> m_layoutExtensions;
        private FilterBuilder<StringLayoutBuilder> m_filterBuilder;

        public StringLayoutBuilder()
        {
            m_filterBuilder = new FilterBuilder<StringLayoutBuilder>(this);
        }

        public ILogger Build()
        {
            string layout = m_layout;
            if (m_layout == null)
            {
                layout = "${longdate} ${title} ${message}";
            }
            var filter = m_filterBuilder.Build();
            var layoutLogger = new StringLayoutLogger(layout, StringBufferPool.Instance, filter);
            if (m_layoutExtensions == null)
            {
                m_layoutExtensions = new LayoutExtensions<IStringLayoutBuilder>(this);
                var t = m_layoutExtensions.All;
            }
            m_layoutExtensions.Initilze(layoutLogger);

            foreach (var stringPackageDefinition in m_definitions)
            {
                layoutLogger.RegisterDefinition(stringPackageDefinition);
            }

            foreach (var stringAppender in m_stringAppenders)
            {
                layoutLogger.AddStringAppender(stringAppender.Build());
            }
            return layoutLogger;
        }


        public IFilterBuilder<IStringLayoutBuilder> Filter { get { return this; }}

        public IStringLayoutBuilder SetLayout(string layout)
        {
            m_layout = layout;
            return this;
        }

        public ILayoutExtensions<IStringLayoutBuilder> Extensions {
            get
            {
                if (m_layoutExtensions == null)
                {
                    m_layoutExtensions = new LayoutExtensions<IStringLayoutBuilder>(this);   
                }
                return m_layoutExtensions;
            }
        }

        public IStringLayoutBuilder Map<T>(Func<PackageDefinition<T>, object> define)
        {
            var packageDefinition = new PackageDefinition<T>();
            define.Invoke(packageDefinition);
            m_definitions.Add(packageDefinition);
            return this;
        }

        public IStringLayoutBuilder Map(IStringPackageDefinition definition)
        {
            m_definitions.Add(definition);
            return this;
        }

        public IStringOutputTo OutputTo { get { return this; } }
        
        #region Filter
        
        public IStringLayoutBuilder Include(params LogTitles[] logTitles)
        {
            return m_filterBuilder.Include(logTitles);
        }

        public IStringLayoutBuilder Exclude(params LogTitles[] logTitles)
        {
            return m_filterBuilder.Exclude(logTitles);
        }

        public IStringLayoutBuilder Custom(IFilter filter)
        {
            return m_filterBuilder.Custom(filter);
        }

        #endregion

        #region IStringOutputTo

        public StringOutputer Async { get { return new StringOutputer(this,builder => m_stringAppenders.Add(builder), ExecutionMode.Async); } }
        public StringOutputer Sync  { get { return new StringOutputer(this, builder => m_stringAppenders.Add(builder), ExecutionMode.Sync); } }

        #endregion
    }
}
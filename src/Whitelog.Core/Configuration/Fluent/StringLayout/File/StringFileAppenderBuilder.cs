using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whitelog.Core.Binary.FileLog;
using Whitelog.Core.Binary.FileLog.SubmitLogEntry;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;
using Whitelog.Core.Configuration.Fluent.StringLayout;
using Whitelog.Core.Configuration.Fluent.StringLayout.File;
using Whitelog.Core.File;
using Whitelog.Core.Loggers;
using Whitelog.Core.Loggers.StringAppender.File;

namespace Whitelog.Core.Configuration.Fluent.StringLayout.File
{
    class StringFileAppenderBuilder : IStringFileAppenderBuilder , IStringAppenderBuilder
    {
        private FilterBuilder<IStringFileAppenderBuilder> m_filterBuilder;
        private Buffers m_buffers = Buffers.ThreadStatic;
        private ExecutionMode m_executionMode;
        private FileConfiguration m_fileConfiguration = new FileConfiguration()
                                                        {
                                                            FilePath = @"{basepath}\Log.txt",
                                                        };

        public StringFileAppenderBuilder()
        {
            m_filterBuilder = new FilterBuilder<IStringFileAppenderBuilder>(this);
        }
        public IStringFileAppenderBuilder Config(Func<IFileConfigurationBuilder, object> file)
        {
            var fcb = new FileConfigurationBuilder(@"{basepath}\Log.txt");
            file.Invoke(fcb);
            m_fileConfiguration = fcb.GetFileConfiguration();
            return this;
        }

        public IFilterBuilder<IStringFileAppenderBuilder> Filter { get { return m_filterBuilder; } }

        public IStringFileAppenderBuilder ExecutionMode(ExecutionMode executionMode)
        {
            m_executionMode = executionMode;
            return this;
        }

        public IStringFileAppenderBuilder Buffer(Buffers buffers)
        {
            m_buffers = buffers;
            return this;
        }

        public IStringAppender Build()
        {
            var listWriter = new StringListWriter(new FileStreamProvider(m_fileConfiguration));
            ISubmitLogEntry subbmiter = null;
            if (m_executionMode == Fluent.ExecutionMode.Async)
            {
                subbmiter = new AsyncSubmitLogEntry(listWriter);
            }
            else if (m_executionMode == Fluent.ExecutionMode.Sync)
            {
                subbmiter = new SyncSubmitLogEntry(listWriter);
            }

            IBufferAllocator bufferAllocator = null;
            if (m_buffers == Buffers.BufferPool)
            {
                bufferAllocator = BufferPoolProxy.Instance;
            }
            else if (m_buffers == Buffers.ThreadStatic)
            {
                bufferAllocator = ThreadStaticBufferFactory.Instance.CreateBufferAllocator(listWriter);
            }
            var filter = m_filterBuilder.Build();
            return new StringFileAppender(bufferAllocator, subbmiter, filter);
        }
    }
}

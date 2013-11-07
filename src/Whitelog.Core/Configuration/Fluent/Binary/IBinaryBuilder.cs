using System;
using Whitelog.Core.Binary.FileLog;
using Whitelog.Core.Binary.FileLog.SubmitLogEntry;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;
using Whitelog.Core.File;
using Whitelog.Core.Loggers;

namespace Whitelog.Core.Configuration.Fluent.Binary
{
    public interface IBinaryBuilder
    {
        IBinaryBuilder Config(Func<IFileConfigurationBuilder, object> file);
        IBinaryBuilder Buffer(Buffers buffers);
        IBinaryBuilder ExecutionMode(ExecutionMode executionMode);
    }

    public class BinaryBuilder : IBinaryBuilder , ILoggerBuilder
    {
        private FileConfigurationBuilder m_fileConfigurationBuilder = new FileConfigurationBuilder(@"{basepath}\Log.dat");
        private Buffers m_buffers = Buffers.ThreadStatic;
        private ExecutionMode m_executionMode = Fluent.ExecutionMode.Async;

        public IBinaryBuilder Config(Func<IFileConfigurationBuilder, object> file)
        {
            m_fileConfigurationBuilder = new FileConfigurationBuilder(@"{basepath}\Log.dat");
            file.Invoke(m_fileConfigurationBuilder);
            return this;
        }

        public IBinaryBuilder Buffer(Buffers buffers)
        {
            m_buffers = buffers;
            return this;
        }

        public IBinaryBuilder ExecutionMode(ExecutionMode executionMode)
        {
            m_executionMode = executionMode;
            return this;
        }

        public ILogger Build()
        {
            var fileStreamProvider = new FileStreamProvider(m_fileConfigurationBuilder.GetFileConfiguration());
            ISubmitLogEntryFactory subbmiter = null;
            if (m_executionMode == Fluent.ExecutionMode.Async)
            {
                subbmiter = new AsyncSubmitLogEntryFactory();
            }
            else if (m_executionMode == Fluent.ExecutionMode.Sync)
            {
                subbmiter = new SyncSubmitLogEntryFactory();
            }

            IBufferAllocatorFactory bufferAllocator = null;
            if (m_buffers == Buffers.BufferPool)
            {
                bufferAllocator = BufferPoolFactory.Instance;
            }
            else if (m_buffers == Buffers.ThreadStatic)
            {
                bufferAllocator = ThreadStaticBufferFactory.Instance;
            }

            return new ContinuesBinaryFileLogger(fileStreamProvider, subbmiter, bufferAllocator);
        }
    }
}
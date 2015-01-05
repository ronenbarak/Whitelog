using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Whitelog.Core.Binary;
using Whitelog.Core.Binary.Serializer;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;
using Whitelog.Core.File;
using Whitelog.Core.Loggers;
using Whitelog.Core.Loggers.Binary;
using Whitelog.Core.Loggers.Binary.SubmitLogEntry;
using Whitelog.Core.PackageDefinitions;

namespace Whitelog.Core.Configuration.Fluent.Binary
{
    public interface IBuffers
    {
        IBinaryBuilder ThreadStatic { get; }
        IBinaryBuilder BufferPool { get; }
        IBinaryBuilder MemoryAllocation { get; }
        IBinaryBuilder Custom(IBufferAllocatorFactory allocatorFactory);
    }

    public interface IExecutionMode
    {
        IBinaryBuilder Async { get; }
        IBinaryBuilder Sync { get; } 
    }

    public interface IBinaryBuilder
    {
        IBuffers Buffer { get; }
        IExecutionMode Mode { get; }
        IBinaryBuilder Map<T>(Func<PackageDefinition<T>, object> define);
        IBinaryBuilder Map(IBinaryPackageDefinition definition);
        IBinaryBuilder File(Func<IFileConfigurationBuilder, object> func);
        IFilterBuilder<IBinaryBuilder> Filter { get; }
    }

    public class BinaryBuilder : IBinaryBuilder, ILoggerBuilder, IBuffers, IExecutionMode
    {
        public List<IBinaryPackageDefinition> m_definitions = new List<IBinaryPackageDefinition>();

        private IBufferAllocatorFactory m_bufferAllocatorFactory = ThreadStaticBufferFactory.Instance;
        private ExecutionMode m_executionMode = Fluent.ExecutionMode.Async;
        private FileConfigurationBuilder m_fileConfigurationBuilder = new FileConfigurationBuilder(@"{basepath}\Log.log");
        public IBuffers Buffer { get { return this; } }
        private FilterBuilder<IBinaryBuilder> m_filter;
        public IExecutionMode Mode { get { return this; } }

        public BinaryBuilder()
        {
            m_filter = new FilterBuilder<IBinaryBuilder>(this);
        }

        public IBinaryBuilder Map<T>(Func<PackageDefinition<T>, object> define)
        {
            var packageDefinition = new PackageDefinition<T>();
            define.Invoke(packageDefinition);
            m_definitions.Add(packageDefinition);
            return this;
        }

        public IBinaryBuilder Map(IBinaryPackageDefinition definition)
        {
            m_definitions.Add(definition);
            return this;
        }

        public IFilterBuilder<IBinaryBuilder> Filter { get { return m_filter; } }

        public IBinaryBuilder File(Func<IFileConfigurationBuilder, object> func)
        {
            func.Invoke(m_fileConfigurationBuilder);
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

            var logger = new ContinuesBinaryFileLogger(fileStreamProvider, subbmiter, m_bufferAllocatorFactory);

            foreach (var binaryPackageDefinition in m_definitions)
            {
                logger.RegisterDefinition(binaryPackageDefinition);
            }

            return logger;
        }

        public IBinaryBuilder ThreadStatic { get { m_bufferAllocatorFactory = ThreadStaticBufferFactory.Instance; return this; } }
        public IBinaryBuilder BufferPool { get { m_bufferAllocatorFactory = BufferPoolFactory.Instance; return this; } }
        public IBinaryBuilder MemoryAllocation { get { m_bufferAllocatorFactory = BufferPoolFactory.Instance; return this; } }
        public IBinaryBuilder Custom(IBufferAllocatorFactory allocatorFactory)
        {
            m_bufferAllocatorFactory = allocatorFactory;
            return this;
        }

        IBinaryBuilder IExecutionMode.Sync { get { m_executionMode = ExecutionMode.Sync; return this; } }
        IBinaryBuilder IExecutionMode.Async { get { m_executionMode = ExecutionMode.Async; return this; } }
    }
}
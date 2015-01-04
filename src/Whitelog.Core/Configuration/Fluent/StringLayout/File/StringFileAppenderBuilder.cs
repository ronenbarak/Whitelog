using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Whitelog.Core.Binary.FileLog;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;
using Whitelog.Core.File;
using Whitelog.Core.Loggers;
using Whitelog.Core.Loggers.StringAppender.File;

namespace Whitelog.Core.Configuration.Fluent.StringLayout.File
{
	class StringFileAppenderBuilder : IStringAppenderBuilder
	{
        private FilterBuilder<StringFileAppenderBuilder> m_filterBuilder;
	    private FileConfigurationBuilder m_fileConfiguration;
	    private ExecutionMode m_executionMode;

	    public StringFileAppenderBuilder(ExecutionMode executionMode, FileConfigurationBuilder fileConfiguration)
	    {
	        m_executionMode = executionMode;
	        m_fileConfiguration = fileConfiguration;
            m_filterBuilder = new FilterBuilder<StringFileAppenderBuilder>(this);
	    }

		public IStringAppender Build()
		{
			var fileWriter = new StringFileWriter(new FileStreamProvider(m_fileConfiguration.GetFileConfiguration()));

		    IStringAppenderSubbmiter subbmiter = null;
            if (m_executionMode == ExecutionMode.Sync)
            {
                subbmiter = new SyncStringFileSubbmiter(fileWriter);
            }
            else if (m_executionMode == ExecutionMode.Async)
            {
                subbmiter = new AsyncStringFile(fileWriter);
            }

			var filter = m_filterBuilder.Build();
            return new StringFileAppender(subbmiter, filter);
		}
	}
}

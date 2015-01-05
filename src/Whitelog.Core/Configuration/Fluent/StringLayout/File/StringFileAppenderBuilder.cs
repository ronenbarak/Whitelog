using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;
using Whitelog.Core.File;
using Whitelog.Core.Loggers;
using Whitelog.Core.Loggers.String;
using Whitelog.Core.Loggers.String.StringAppenders.File;
using Whitelog.Core.Loggers.String.StringAppenders.File.Submitter;

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

		    IStringAppenderSubmitter submitter = null;
            if (m_executionMode == ExecutionMode.Sync)
            {
                submitter = new SyncStringFileSubmitter(fileWriter);
            }
            else if (m_executionMode == ExecutionMode.Async)
            {
                submitter = new AsyncStringFile(fileWriter);
            }

			var filter = m_filterBuilder.Build();
            return new StringFileAppender(submitter, filter);
		}
	}
}

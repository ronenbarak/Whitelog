using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;
using Whitelog.Barak.SystemDateTime;
using Whitelog.Core;
using Whitelog.Core.Loggers;
using Whitelog.Core.LogScopeSyncImplementation;
using Whitelog.Interface;

namespace Whitelog.Sample
{
    public partial class WhitelogSample : Form
    {
        private LogTunnel m_log = new LogTunnel(new SystemDateTime(), LogScopeSyncFactory.Create());
        
        public WhitelogSample()
        {
            InitializeComponent();
            BindingList<LayoutLogger> layoutLoggers = new BindingList<LayoutLogger>();
            
            m_binaryTabPage.Controls.Add(new BinaryLoggerBuilder(){LogTunnel = m_log, PreviewTabControl = m_previewTabControl});
            m_stringLayoutTabPage.Controls.Add(new StringLayoutLoggerBuilder() { LogTunnel = m_log, LayoutLoggers = layoutLoggers});
            m_consoleAppanderTabPage.Controls.Add(new ColorLoggerBuilder(){LogTunnel = m_log,LayoutLoggers =  layoutLoggers, PreviewTabControl =  m_previewTabControl});
            
            m_samplePanel.Controls.Add(new LoggingSamples() { Logger = m_log });
        }
    }
}

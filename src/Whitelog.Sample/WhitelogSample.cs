using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Whitelog.Core;
using Whitelog.Core.FileLog;
using Whitelog.Core.FileLog.SubmitLogEntry;
using Whitelog.Core.LogScopeSyncImplementation;
using Whitelog.Core.Reader;
using Whitelog.Core.Serializer.MemoryBuffer;
using Whitelog.Interface;

namespace Whitelog.Sample
{
    public partial class WhitelogSample : Form
    {
        private LogTunnel m_log = new LogTunnel(new SystemDateTime(), new SingleLogPerApplicationScopeSync());
        private Stack<ILogScope> m_logScopes = new Stack<ILogScope>();
        public WhitelogSample()
        {
            InitializeComponent();
            m_txtPath.Text = System.IO.Path.Combine(Environment.CurrentDirectory, "Sample.Log");
            m_loggerTypes.Items.Add(typeof (ContinuesBinaryFileLogger).Name);

            var ringBufferLogger = new RingSubmitLogEntryFactory(RingConsumeOption.SpinWait, 1000);
            m_submitterType.Items.Add(new SubmiterOption(new AsyncSubmitLogEntryFactory(), "Async - Eventually consistent"));
            m_submitterType.Items.Add(new SubmiterOption(new SyncSubmitLogEntryFactory(), "Sync - ACID"));
            m_submitterType.Items.Add(new SubmiterOption(ringBufferLogger, "RingBuffer - Zero Memory Allocation(Eventually consistent)"));

            m_bufferTypes.Items.Add(new BufferOption(ThreadStaticBufferFactory.Instance, "ThreadStatic"));
            m_bufferTypes.Items.Add(new BufferOption(BufferPoolFactory.Instance, "BufferPool"));
            m_bufferTypes.Items.Add(new BufferOption(ringBufferLogger, "RingBuffer"));

            m_bufferTypes.SelectedIndex = 0;
            m_submitterType.SelectedIndex = 0;
            m_loggerTypes.SelectedIndex = 0;

            m_cboLogTitles.SelectedIndex = 0;
        }

        private void m_btnLogBuilder_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_loggerTypes.SelectedItem.ToString() == typeof(ContinuesBinaryFileLogger).Name)
                {
                    ContinuesBinaryFileLogger fileLogger = new ContinuesBinaryFileLogger(m_txtPath.Text, ((SubmiterOption)m_submitterType.SelectedItem).SubmitLogEntryFactory, ((BufferOption)m_bufferTypes.SelectedItem).BufferAllocatorFactory);
                    fileLogger.AttachToTunnelLog(m_log);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void m_btnSimpleLog_Click(object sender, EventArgs e)
        {
            switch (m_cboLogTitles.SelectedItem.ToString().ToUpper())
            {
                case "DEBUG":
                    m_log.LogDebug(m_txtSimpleLogMessage.Text);
                    break;
                case "ERROR":
                    m_log.LogError(m_txtSimpleLogMessage.Text);
                    break;
                case "FATAL":
                    m_log.LogFatal(m_txtSimpleLogMessage.Text);
                    break;
                case "INFO":
                    m_log.LogInfo(m_txtSimpleLogMessage.Text);
                    break;
                case "WARNING":
                    m_log.LogWarning(m_txtSimpleLogMessage.Text);
                    break;
            }
        }

        private void m_btnOpenScope_Click(object sender, EventArgs e)
        {
            m_logScopes.Push(m_log.CreateScope(m_txtScopeMessage.Text));
            m_btnClose.Enabled = true;
        }

        private void m_btnClose_Click(object sender, EventArgs e)
        {
            var scope = m_logScopes.Pop();
            scope.Dispose();
            m_btnClose.Enabled = m_logScopes.Count != 0;
        }
    }
}

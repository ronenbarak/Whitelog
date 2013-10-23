using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Whitelog.Interface;

namespace Whitelog.Sample
{
    public partial class LoggingSamples : UserControl
    {
        private Stack<ILogScope> m_logScopes = new Stack<ILogScope>();

        public ILog Logger { get; set; }

        public LoggingSamples()
        {
            InitializeComponent();
            m_cboLogTitles.SelectedIndex = 0;
        }

        private void m_btnSimpleLog_Click(object sender, EventArgs e)
        {
            switch (m_cboLogTitles.SelectedItem.ToString().ToUpper())
            {
                case "DEBUG":
                    Logger.LogDebug(m_txtSimpleLogMessage.Text);
                    break;
                case "ERROR":
                    Logger.LogError(m_txtSimpleLogMessage.Text);
                    break;
                case "FATAL":
                    Logger.LogFatal(m_txtSimpleLogMessage.Text);
                    break;
                case "INFO":
                    Logger.LogInfo(m_txtSimpleLogMessage.Text);
                    break;
                case "WARNING":
                    Logger.LogWarning(m_txtSimpleLogMessage.Text);
                    break;
            }
        }

        private void m_btnOpenScope_Click(object sender, EventArgs e)
        {
            m_logScopes.Push(Logger.CreateScope(m_txtScopeMessage.Text));
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Whitelog.Core;
using Whitelog.Core.Loggers;
using Whitelog.Core.Loggers.String;
using Whitelog.Core.String.Layout.StringLayoutFactory;
using Whitelog.Core.String.StringBuffer;

namespace Whitelog.Sample
{
    public partial class StringLayoutLoggerBuilder : UserControl
    {
        class ExtensionOption
        {
            public string Extension { get; private set; }
            public string Description { get; private set; }

            public ExtensionOption(string extension, string description)
            {
                Extension = extension;
                Description = description;
            }

            public override string ToString()
            {
                return string.Format("{0} - {1}", Extension, Description);
            }
        }

        private IList<StringLayoutLogger> m_layouyLoggers;
        public LogTunnel LogTunnel { get; set; }
        public IList<StringLayoutLogger> LayoutLoggers {
            get
            {
                return m_layouyLoggers;
            }
            set
            {
                m_layouyLoggers = value;
                m_lstBuildLayouts.DataSource = value;
                m_btnBuild_Click(null, EventArgs.Empty);
            }}
        
        public StringLayoutLoggerBuilder()
        {
            InitializeComponent();
            this.Dock =DockStyle.Fill;

            m_lstBuildLayouts.DisplayMember = "Layout";

            m_extensions.Items.Add(new ExtensionOption("Title", "AppendLogEntry the title Level"));
            m_extensions.Items.Add(new ExtensionOption("Newline", "AppendLogEntry new Line"));
            m_extensions.Items.Add(new ExtensionOption("ScopeId", "AppendLogEntry the current scope id"));
            m_extensions.Items.Add(new ExtensionOption("ThreadId", "AppendLogEntry the current threadId"));
            m_extensions.Items.Add(new ExtensionOption("Longdate", "AppendLogEntry the date"));
            m_extensions.Items.Add(new ExtensionOption("Message", "AppendLogEntry the message"));

            m_extensions.SelectedIndex = 0;
        }

        private void m_btnAddExtension_Click(object sender, EventArgs e)
        {
            m_txtLayout.Text = m_txtLayout.Text + string.Format("${{{0}}}", (m_extensions.SelectedItem as ExtensionOption).Extension);
        }

        private void m_btnBuild_Click(object sender, EventArgs e)
        {
            var logger = new StringLayoutLogger(m_txtLayout.Text,StringBufferPool.Instance);
            logger.RegisterLayoutExtensions(AllLayoutFactories.Factories);
            LayoutLoggers.Add(logger);
            logger.AttachToTunnelLog(LogTunnel);
        }
    }
}

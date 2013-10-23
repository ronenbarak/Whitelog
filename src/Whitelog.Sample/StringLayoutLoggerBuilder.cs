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

        private IList<LayoutLogger> m_layouyLoggers;
        public LogTunnel LogTunnel { get; set; }
        public IList<LayoutLogger> LayoutLoggers {
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

            m_extensions.Items.Add(new ExtensionOption("Title", "Append the title Level"));
            m_extensions.Items.Add(new ExtensionOption("Newline", "Append new Line"));
            m_extensions.Items.Add(new ExtensionOption("ScopeId", "Append the current scope id"));
            m_extensions.Items.Add(new ExtensionOption("ThreadId", "Append the current threadId"));
            m_extensions.Items.Add(new ExtensionOption("Longdate", "Append the date"));
            m_extensions.Items.Add(new ExtensionOption("Message", "Append the message"));

            m_extensions.SelectedIndex = 0;
        }

        private void m_btnAddExtension_Click(object sender, EventArgs e)
        {
            m_txtLayout.Text = m_txtLayout.Text + string.Format("${{{0}}}", (m_extensions.SelectedItem as ExtensionOption).Extension);
        }

        private void m_btnBuild_Click(object sender, EventArgs e)
        {
            var logger = new LayoutLogger(m_txtLayout.Text,StringBufferPool.Instance);
            LayoutLoggers.Add(logger);
            logger.AttachToTunnelLog(LogTunnel);
        }
    }
}

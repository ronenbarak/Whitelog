﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Whitelog.Core;
using Whitelog.Core.Binary;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;
using Whitelog.Core.File;
using Whitelog.Core.Loggers;
using Whitelog.Core.Loggers.Binary;
using Whitelog.Core.Loggers.Binary.SubmitLogEntry;
using Whitelog.Viewer;
using System.IO;

namespace Whitelog.Sample
{
    public partial class BinaryLoggerBuilder : UserControl
    {
        public LogTunnel LogTunnel { get; set; }
        public TabControl PreviewTabControl { get; set; }

        public BinaryLoggerBuilder()
        {
            Dock =DockStyle.Fill;
            InitializeComponent();
			m_txtPath.Text = string.Format(@"{{BaseDir}}{0}Sample.Log",Path.DirectorySeparatorChar);
            m_loggerTypes.Items.Add(typeof(ContinuesBinaryFileLogger).Name);
            m_loggerTypes.Items.Add("In Memory");

            m_submitterType.Items.Add(new SubmiterOption(new AsyncSubmitLogEntryFactory(), "Async - Eventually consistent"));
            m_submitterType.Items.Add(new SubmiterOption(new SyncSubmitLogEntryFactory(), "Sync - ACID"));

            m_bufferTypes.Items.Add(new BufferOption(ThreadStaticBufferFactory.Instance, "ThreadStatic"));
            m_bufferTypes.Items.Add(new BufferOption(BufferPoolFactory.Instance, "BufferPool"));

            m_submitterType.SelectedIndexChanged += m_submitterType_SelectedIndexChanged;
            m_bufferTypes.SelectedIndexChanged += m_submitterType_SelectedIndexChanged;
            m_bufferTypes.SelectedIndex = 0;
            m_submitterType.SelectedIndex = 0;
            m_loggerTypes.SelectedIndex = 0;
        }

        void m_submitterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_submitterType.SelectedIndex == 2)
            {
                m_bufferTypes.SelectedIndex = 2;
            }
            
            if (m_bufferTypes.SelectedIndex == 2 && m_submitterType.SelectedIndex != 2)
            {
                m_bufferTypes.SelectedIndex = 0;
            }
        }

        private void m_btnLogBuilder_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_loggerTypes.SelectedItem.ToString() == typeof(ContinuesBinaryFileLogger).Name)
                {
                    var fileStremProvider = new FileStreamProvider(new FileConfiguration()
                                                                   {
                                                                       FilePath = m_txtPath.Text,
                                                                   });
                    var fileLogger = new ContinuesBinaryFileLogger(fileStremProvider, ((SubmiterOption)m_submitterType.SelectedItem).SubmitLogEntryFactory, ((BufferOption)m_bufferTypes.SelectedItem).BufferAllocatorFactory);
                    fileLogger.AttachToTunnelLog(LogTunnel);

                    var tabPage = new TabPage("BinaryFile " + System.IO.Path.GetFileName(m_txtPath.Text));
                    TreeListView dataTreeListView = new TreeListView();
                    dataTreeListView.Dock = DockStyle.Fill;
                    tabPage.Controls.Add(dataTreeListView);
                    var logAppender = new TreeViewLogAppender(dataTreeListView);
                    PreviewTabControl.TabPages.Add(tabPage);
                    logAppender.OpenFile(fileStremProvider.FileName);
                }
                else if (m_loggerTypes.SelectedItem.ToString() == "In Memory")
                {
                    var fileLogger = new ContinuesBinaryFileLogger(new InMemoryStreamProvider(), ((SubmiterOption)m_submitterType.SelectedItem).SubmitLogEntryFactory, ((BufferOption)m_bufferTypes.SelectedItem).BufferAllocatorFactory);
                    fileLogger.AttachToTunnelLog(LogTunnel);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}

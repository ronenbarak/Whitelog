using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Whitelog.Core;
using Whitelog.Core.Filter;
using Whitelog.Core.Loggers;
using Whitelog.Core.Loggers.StringAppender.Console;
using Whitelog.Interface.LogTitles;

namespace Whitelog.Sample
{
    public partial class ColorLoggerBuilder : UserControl
    {
        private IList<LayoutLogger> m_layouyLoggers;
        private int m_index = 0;

        public LogTunnel LogTunnel { get; set; }
        public TabControl PreviewTabControl { get; set; }

        public IList<LayoutLogger> LayoutLoggers
        {
            get
            {
                return m_layouyLoggers;
            }
            set
            {
                m_layouyLoggers = value;
                m_cboLayout.DataSource = value;
            }
        }

        public ColorLoggerBuilder()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            m_cboInfoForeground.SelectedIndex  = 0;
            m_cboErrorForeground.SelectedIndex = 0;
            m_cboErrorBackground.SelectedIndex = 0;
            
            m_chkDefaultColors_CheckedChanged(null,EventArgs.Empty);
        }

        private void m_chkDefaultColors_CheckedChanged(object sender, EventArgs e)
        {
            m_cboInfoForeground.Enabled = m_cboErrorForeground.Enabled = m_cboErrorBackground.Enabled = !m_chkDefaultColors.Checked;
        }

        private void m_btnBuild_Click(object sender, EventArgs e)
        {
            if (m_cboLayout.SelectedIndex >= 0)
            {
                var tabPage = new TabPage("Console " + m_index++ + "#");
                RichTextBox richTextBox = new RichTextBox();
                richTextBox.ReadOnly = true;
                richTextBox.BackColor = Color.Black;
                richTextBox.ForeColor = Color.White;
                richTextBox.Dock = DockStyle.Fill;
                tabPage.Controls.Add(richTextBox);
                PreviewTabControl.TabPages.Add(tabPage);
                if (m_chkDefaultColors.Checked)
                {
                    var defualtColorSchema = new DefaultColorSchema();
                    (m_cboLayout.SelectedItem as LayoutLogger).AddStringAppender(new ConsoleAppender(new RichTextBoxConsoleLogEntry(richTextBox),defualtColorSchema));   
                }
                else
                {
                    var colorSchema = new ConditionColorSchema();
                    ConsoleColor? infoForeground = null;
                    ConsoleColor? errorForeground = null;
                    ConsoleColor? errorBackground = null;
                    if (m_cboInfoForeground.SelectedItem.ToString() != "None")
                    {
                        ConsoleColor s;
                        ConsoleColor.TryParse<ConsoleColor>(m_cboInfoForeground.SelectedItem.ToString(), true, out s);
                        infoForeground = s;
                    }
                    if (m_cboErrorForeground.SelectedItem.ToString() != "None")
                    {
                        ConsoleColor s;
                        ConsoleColor.TryParse<ConsoleColor>(m_cboErrorForeground.SelectedItem.ToString(), true, out s);
                        errorForeground = s;
                    }
                    if (m_cboErrorBackground.SelectedItem.ToString() != "None")
                    {
                        ConsoleColor s;
                        ConsoleColor.TryParse<ConsoleColor>(m_cboErrorBackground.SelectedItem.ToString(), true, out s);
                        errorBackground = s;
                    }

                    colorSchema.AddCondition(entry => entry.Title.Id == ReservedLogTitleIds.Info,new ColorLine(null,infoForeground));
                    colorSchema.AddCondition(entry => entry.Title.Id == ReservedLogTitleIds.Error, new ColorLine(errorBackground, errorForeground));

                    (m_cboLayout.SelectedItem as LayoutLogger).AddStringAppender(new ConsoleAppender(new RichTextBoxConsoleLogEntry(richTextBox),colorSchema));   
                }
            }
            
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Whitelog.Core.Binary.Reader;
using Whitelog.Core.Binary.Reader.ExpendableList;

namespace Whitelog.Viewer
{
    public partial class WhiteLogViewerForm : Form
    {
        private FileStream m_strem;

        public WhiteLogViewerForm()
        {
            InitializeComponent();
            m_logTreeView.CanExpandGetter = CanExpandGetter;
            m_logTreeView.ChildrenGetter = ChildrenGetter;
        }

        private IEnumerable ChildrenGetter(object model)
        {
            return (model as LogNode).Children;
        }

        private bool CanExpandGetter(object model)
        {
            return (model as LogNode).Children != null && (model as LogNode).Children.Count != 0;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog(this);

            try
            {
                if (!string.IsNullOrEmpty(fileDialog.FileName))
                {
                    var readerFactory = new WhitelogBinaryReaderFactory();
                    readerFactory.RegisterReaderFactory(new ExpandableLogReaderFactory());
                    readerFactory.RegisterReaderFactory(new InMemoryLogReaderFactory());

                    if (m_strem != null)
                    {
                        m_strem.Dispose();
                        m_strem = null;
                    }

                    m_strem =  System.IO.File.Open(fileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    var whitelogConsumer = new WhiteLogConsumer();
                    var logReader = readerFactory.GetLogReader(m_strem, whitelogConsumer);

                    logReader.TryRead();
                    m_logTreeView.Roots = whitelogConsumer.LogNodes;
                    m_logTreeView.ExpandAll();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}

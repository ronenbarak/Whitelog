using System;
using System.Windows.Forms;

namespace Whitelog.Viewer
{
    public partial class WhiteLogViewerForm : Form
    {
        private TreeViewLogAppender m_treeViewLogAppender;

        public WhiteLogViewerForm()
        {
            InitializeComponent();
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
                    if (m_treeViewLogAppender != null)
                    {
                        m_treeViewLogAppender.Dispose();
                    }
                    m_treeViewLogAppender = new TreeViewLogAppender(m_logTreeView);
                    m_treeViewLogAppender.OpenFile(fileDialog.FileName);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}

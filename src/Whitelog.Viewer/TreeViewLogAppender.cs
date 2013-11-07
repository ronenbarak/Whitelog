using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Whitelog.Core.Binary.Reader;
using Whitelog.Core.Binary.Reader.ExpendableList;

namespace Whitelog.Viewer
{
    public class TreeViewLogAppender : IDisposable
    {
        private DataTreeListView m_logTreeView;
        private FileStream m_strem;
        private Timer m_timer = null;
        private bool m_disposed = false;

        public TreeViewLogAppender(DataTreeListView logTreeView)
        {
            m_logTreeView = logTreeView;
            InitDataTreeView();
            m_logTreeView.CanExpandGetter = CanExpandGetter;
            m_logTreeView.ChildrenGetter = ChildrenGetter;
        }

        private void InitDataTreeView()
        {
            var olvTimeColumn = ((BrightIdeasSoftware.OLVColumn) (new BrightIdeasSoftware.OLVColumn()));
            var olvTitleColumn = ((BrightIdeasSoftware.OLVColumn) (new BrightIdeasSoftware.OLVColumn()));
            var olvMessageColumn = ((BrightIdeasSoftware.OLVColumn) (new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.m_logTreeView)).BeginInit();
            m_logTreeView.AllColumns.Clear();
            m_logTreeView.AllColumns.Add(olvTimeColumn);
            m_logTreeView.AllColumns.Add(olvTitleColumn);
            m_logTreeView.AllColumns.Add(olvMessageColumn);
            
            m_logTreeView.DataSource = null;
            m_logTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            m_logTreeView.FullRowSelect = true;
            m_logTreeView.HeaderUsesThemes = false;
            m_logTreeView.HideSelection = false;
            m_logTreeView.Location = new System.Drawing.Point(0, 24);
            m_logTreeView.Name = "m_logTreeView";
            m_logTreeView.OwnerDraw = true;
            m_logTreeView.RootKeyValueString = "";
            m_logTreeView.ShowGroups = false;
            m_logTreeView.Size = new System.Drawing.Size(673, 511);
            m_logTreeView.TabIndex = 2;
            m_logTreeView.UseCompatibleStateImageBehavior = false;
            m_logTreeView.UseFilterIndicator = true;
            m_logTreeView.UseFiltering = true;
            m_logTreeView.View = System.Windows.Forms.View.Details;
            m_logTreeView.VirtualMode = true;
            // 
            // olvTimeColumn
            // 
            olvTimeColumn.AspectName = "Time";
            olvTimeColumn.CellPadding = null;
            olvTimeColumn.Groupable = false;
            olvTimeColumn.Sortable = false;
            olvTimeColumn.Text = "Time";
            olvTimeColumn.Width = 200;
            // 
            // olvTitleColumn
            // 
            olvTitleColumn.AspectName = "Title";
            olvTitleColumn.CellPadding = null;
            olvTitleColumn.Sortable = false;
            olvTitleColumn.Text = "Title";
            // 
            // olvMessageColumn
            // 
            olvMessageColumn.AspectName = "Message";
            olvMessageColumn.CellPadding = null;
            olvMessageColumn.FillsFreeSpace = true;
            olvMessageColumn.Groupable = false;
            olvMessageColumn.Sortable = false;
            olvMessageColumn.Text = "Message";

            ((System.ComponentModel.ISupportInitialize)(this.m_logTreeView)).EndInit();
        }

        private IEnumerable ChildrenGetter(object model)
        {
            return (model as LogNode).Children;
        }

        private bool CanExpandGetter(object model)
        {
            return (model as LogNode).Children != null && (model as LogNode).Children.Count != 0;
        }

        public void OpenFile(string filePath)
        {
            var readerFactory = new WhitelogBinaryReaderFactory();
            readerFactory.RegisterReaderFactory(new ExpandableLogReaderFactory());

            m_strem = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var whitelogConsumer = new WhiteLogConsumer();
            var logReader = readerFactory.GetLogReader(m_strem, whitelogConsumer);

            m_timer = new Timer();
            m_timer.Interval = (int)TimeSpan.FromMilliseconds(100).TotalMilliseconds;
            m_timer.Tick += (o, args) =>
                            {
                                if (!m_disposed)
                                {
                                    logReader.TryRead();
                                }
                            };

            whitelogConsumer.ItemAdded += (o, eventData) =>
                                          {
                                              if (eventData.IsRootItem)
                                              {
                                                  m_logTreeView.AddObject(eventData.LogNode);
                                              }
                                              else
                                              {
                                                  m_logTreeView.RefreshObject(eventData.LogNode);
                                                  m_logTreeView.Expand(eventData.LogNode);
                                              }
                                          };
            m_logTreeView.BeginUpdate();
            logReader.TryRead();
            m_logTreeView.EndUpdate();
            m_timer.Start();
        }

        public void Dispose()
        {
            m_disposed = true;
            if (m_strem != null)
            {
                m_strem.Dispose();
                m_strem = null;
            }

            if (m_timer != null)
            {
                m_timer.Stop();
                m_timer.Dispose();
            }

            var objects = m_logTreeView.Objects.Cast<Object>().ToList();
            foreach (var o in objects)
            {
                m_logTreeView.RemoveObject(o);
            }
        }
    }
}
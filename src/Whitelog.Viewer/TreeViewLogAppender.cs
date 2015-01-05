using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Whitelog.Core.Binary.Deserilizer.Reader;
using Whitelog.Core.Binary.Deserilizer.Reader.ExpendableList;

namespace Whitelog.Viewer
{
    public class TreeViewLogAppender : IDisposable
    {
        private TreeListView m_logTreeView;
        private FileStream m_strem;
        private Timer m_timer = null;
        private bool m_disposed = false;

        public TreeViewLogAppender(TreeListView logTreeView)
        {
            m_logTreeView = logTreeView;
            InitDataTreeView();
            m_logTreeView.CanExpandGetter = CanExpandGetter;
            m_logTreeView.ChildrenGetter = ChildrenGetter;

            m_logTreeView.CheckBoxes = false;
        }

        private void InitDataTreeView()
        {
            var olvTimeColumn = ((BrightIdeasSoftware.OLVColumn) (new BrightIdeasSoftware.OLVColumn()));
            olvTimeColumn.AspectGetter = rowObject => ((LogNode) rowObject).Time;
            olvTimeColumn.AspectToStringConverter = rowObject => rowObject.ToString();
            var olvTitleColumn = ((BrightIdeasSoftware.OLVColumn) (new BrightIdeasSoftware.OLVColumn()));
            olvTitleColumn.AspectGetter = rowObject => ((LogNode)rowObject).Title;
            olvTitleColumn.AspectToStringConverter = rowObject => rowObject == null ? "": rowObject.ToString();
            var olvMessageColumn = ((BrightIdeasSoftware.OLVColumn) (new BrightIdeasSoftware.OLVColumn()));
            olvMessageColumn.AspectGetter = rowObject => ((LogNode)rowObject).Message;
            olvMessageColumn.AspectToStringConverter = rowObject => rowObject == null ? "" : rowObject.ToString();
            ((System.ComponentModel.ISupportInitialize)(this.m_logTreeView)).BeginInit();
            m_logTreeView.AllColumns.Clear();
            m_logTreeView.AllColumns.Add(olvTimeColumn);
            m_logTreeView.AllColumns.Add(olvTitleColumn);
            m_logTreeView.AllColumns.Add(olvMessageColumn);
            m_logTreeView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
                                           {
                                               olvTimeColumn,
                                               olvTitleColumn,
                                               olvMessageColumn
                                           });

            this.m_logTreeView.Location = new System.Drawing.Point(0, 27);
            this.m_logTreeView.Name = "m_treeListView";
            this.m_logTreeView.OwnerDraw = true;
            this.m_logTreeView.ShowGroups = false;
            this.m_logTreeView.Size = new System.Drawing.Size(673, 505);
            this.m_logTreeView.TabIndex = 2;
            this.m_logTreeView.UseCompatibleStateImageBehavior = false;
            this.m_logTreeView.View = System.Windows.Forms.View.Details;
            this.m_logTreeView.VirtualMode = true;
            // 
            // olvTimeColumn
            // 
            olvTimeColumn.AspectName = "Time";
            olvTimeColumn.Text = "Time";
            olvTimeColumn.Width = 200;
            // 
            // olvTitleColumn
            // 
            olvTitleColumn.AspectName = "Title";
            olvTitleColumn.Text = "Title";
            // 
            // olvMessageColumn
            // 
            olvMessageColumn.AspectName = "Message";
            olvMessageColumn.FillsFreeSpace = true;
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
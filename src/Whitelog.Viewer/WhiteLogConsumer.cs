using System;
using System.Collections.Generic;
using System.Linq;
using Whitelog.Core.Binary;
using Whitelog.Core.Binary.Generic;
using Whitelog.Core.Binary.Reader;

namespace Whitelog.Viewer
{
    public class LogNodeEvent : EventArgs
    {
        public LogNode LogNode { get; private set; }
        public bool IsRootItem { get; private set; }
        public LogNodeEvent(LogNode logNode,bool isRootItem)
        {
            IsRootItem = isRootItem;
            LogNode = logNode;
        }
    }

    public class WhiteLogConsumer : ILogConsumer
    {
        public List<LogNode> LogNodes { get; private set; }
        public event EventHandler<LogNodeEvent> ItemAdded;

        private Dictionary<int, LogNode> m_openNodes = new Dictionary<int, LogNode>();
        public WhiteLogConsumer()
        {
            LogNodes = new List<LogNode>();
        }

        private void RaiseEvent(LogNode logNode,bool isRootItem)
        {
            if (ItemAdded != null)
            {
                ItemAdded.Invoke(this, new LogNodeEvent(logNode, isRootItem));
            }
        }

        public void Consume(ILogEntryData entryData)
        {
            if (entryData.GetEntryType().FullName == "Whitelog.Interface.LogEntry")
            {
                var titleInfo = (entryData.GetProperties().ElementAt(2).GetValue(entryData) as GenericPackageData);
                int scopeId = (int) (entryData.GetProperties().ElementAt(1).GetValue(entryData));
                if (titleInfo.Type == "Whitelog.Core.OpenLogScopeTitle")
                {
                    int parentScopeId = (int) titleInfo.GetValue(1);
                    var node= new LogNode()
                    {
                        Time = (DateTime) entryData.GetProperties().ElementAt(0).GetValue(entryData),
                        Title = null,
                        Message = titleInfo.GetValue(0).ToString(),
                        Children = new List<LogNode>(),
                    };
                    
                    m_openNodes.Add(scopeId,node);

                    LogNode parentNode;
                    if (m_openNodes.TryGetValue(parentScopeId, out parentNode))
                    {
                        parentNode.Children.Add(node);
                        RaiseEvent(parentNode,false);

                    }
                    else
                    {
                        LogNodes.Add(node);
                        RaiseEvent(node,true);
                    }
                }
                else if (titleInfo.Type == "Whitelog.Core.CloseLogScopeTitle")
                {
                    if (m_openNodes.ContainsKey(scopeId))
                    {
                        LogNode parentNode;
                        if (m_openNodes.TryGetValue(scopeId, out parentNode))
                        {
                            parentNode.Children.Add(new LogNode()
                                                    {
                                                        Message =  "<-",
                                                        Time = (DateTime)entryData.GetProperties().ElementAt(0).GetValue(entryData),
                                                        Title = null,
                                                    });
                            RaiseEvent(parentNode, false);
                        }
                        m_openNodes.Remove(scopeId);
                    }
                }
                else
                {
                    var node = new LogNode()
                               {
                                   Time = (DateTime) entryData.GetProperties().ElementAt(0).GetValue(entryData),
                                   Title = titleInfo.GetValue(1).ToString(),
                                   Message = titleInfo.GetValue(0).ToString(),
                               };
                    LogNode parentNode;
                    if (m_openNodes.TryGetValue(scopeId, out parentNode))
                    {
                        parentNode.Children.Add(node);
                        RaiseEvent(parentNode,false);
                    }
                    else
                    {
                        LogNodes.Add(node);
                        RaiseEvent(node,true);
                    }
                }
            }
        }
    }
}
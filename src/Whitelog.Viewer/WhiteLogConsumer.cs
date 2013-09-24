using System;
using System.Collections.Generic;
using System.Linq;
using BrightIdeasSoftware;
using Whitelog.Core;
using Whitelog.Core.Generic;
using Whitelog.Core.Reader;

namespace Whitelog.Viewer
{
    public class WhiteLogConsumer : ILogConsumer
    {
        public List<LogNode> LogNodes { get; private set; }
        private Dictionary<int, LogNode> m_openNodes = new Dictionary<int, LogNode>();
        public WhiteLogConsumer()
        {
            LogNodes = new List<LogNode>();
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
                    }
                    else
                    {
                        LogNodes.Add(node);   
                    }
                }
                else if (titleInfo.Type == "Whitelog.Core.CloseLogScopeTitle")
                {
                    m_openNodes.Remove(scopeId);
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
                    }
                    else
                    {
                        LogNodes.Add(node);   
                    }
                }
            }
        }
    }
}
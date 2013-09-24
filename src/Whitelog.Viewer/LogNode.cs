using System;
using System.Collections.Generic;

namespace Whitelog.Viewer
{
    public class LogNode
    {
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }

        public List<LogNode> Children { get; set; }
    }
}
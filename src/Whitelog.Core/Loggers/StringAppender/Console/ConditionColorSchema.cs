using System;
using System.Linq;
using Whitelog.Interface;

namespace Whitelog.Core.Loggers.StringAppender.Console
{
    public class ConditionColorSchema : IColorSchema
    {
        public ColorLine DefaultColor { get; set; }
        private Tuple<Func<LogEntry, bool>,ColorLine>[] m_conditions = new Tuple<Func<LogEntry, bool>,ColorLine>[0];
        public ConditionColorSchema()
        {
            DefaultColor = new ColorLine(null, null);
        }

        public void AddCondition(Func<LogEntry, bool> condition, ColorLine color)
        {
            var conditions = m_conditions.ToList();
            conditions.Add(Tuple.Create(condition,color));
            m_conditions = conditions.ToArray();
        }

        public ColorLine GetColor(LogEntry logEntry)
        {
            var temp = m_conditions;
            for (int i = 0; i < temp.Length; i++)
            {
                var currCondition = temp[i];
                if (currCondition.Item1.Invoke(logEntry))
                {
                    return currCondition.Item2;
                }
            }
            return DefaultColor;
        }
    }
}
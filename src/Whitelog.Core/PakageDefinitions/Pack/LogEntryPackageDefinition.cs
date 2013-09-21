using System;
using Whitelog.Interface;
using System.Linq;

namespace Whitelog.Core.PakageDefinitions.Pack
{
    public sealed class LogEntryPackageDefinition : PackageDefinition<LogEntry>
    {
        private Type m_type;

        private LogEntryPackageDefinition(Type type)
        {
            m_type = type;
            Define(x => x.Time, entry => entry.Time)
            .Define(x => x.LogScopeId, entry => entry.LogScopeId)
            .Define(x => x.Title, entry => entry.Title)
            .DefineArray(x => x.Paramaeters, entry => entry.Paramaeters);
        }

        public LogEntryPackageDefinition():this(typeof(LogEntry))
        {
        }

        public override IBinaryPackageDefinition Clone(System.Type type, object instance)
        {
            return new LogEntryPackageDefinition(type);
        }

        public override System.Type GetTypeDefinition()
        {
            return m_type;
        }

        public override void PackData(IBinaryPackager packager, ISerializer serializer, object data)
        {
            LogEntry t = (LogEntry) data;
            serializer.Serialize(t.Time.Ticks);
            serializer.Serialize(t.LogScopeId);
            packager.Pack(t.Title,serializer);
            var parms = t.Paramaeters;
            if (parms == null)
            {
                serializer.SerializeVariant(0);
            }
            else
            {
                serializer.SerializeVariant(parms.Length + 1); // we add one more to tell it is not null

                for (int i = 0; i < parms.Length;i++)
                {
                    packager.Pack(parms[i], serializer);
                }
            }
        }
    }
}
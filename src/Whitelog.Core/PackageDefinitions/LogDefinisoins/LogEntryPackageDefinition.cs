using System;
using Whitelog.Core.Binary;
using Whitelog.Core.Binary.Serializer;
using Whitelog.Interface;

namespace Whitelog.Core.PackageDefinitions.LogDefinisoins
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
            .Define(x => x.Paramaeter, entry => entry.Paramaeter);
        }

        public LogEntryPackageDefinition():this(typeof(LogEntry))
        {
        }

        public override IPackageDefinition Clone(System.Type type, object instance)
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
            packager.Pack(t.Paramaeter,serializer);
        }
    }
}
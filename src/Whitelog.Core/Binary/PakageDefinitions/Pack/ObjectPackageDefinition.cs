using System;

namespace Whitelog.Core.Binary.PakageDefinitions.Pack
{
    public sealed class ObjectPackageDefinition : PackageDefinition<object>
    {
        private Type m_type;

        private ObjectPackageDefinition(Type type)
        {
            m_type = type;
            Define("ToString", o => o.ToString());
        }

        public ObjectPackageDefinition():this(typeof(Object))
        {
        }
        
        public override IBinaryPackageDefinition Clone(System.Type type, object instance)
        {
            return new ObjectPackageDefinition(type);
        }
        
        public override System.Type GetTypeDefinition()
        {
            return m_type;
        }

        public override void PackData(IBinaryPackager packager, ISerializer serializer, object data)
        {
            serializer.Serialize(data.ToString());
        }
    }
}
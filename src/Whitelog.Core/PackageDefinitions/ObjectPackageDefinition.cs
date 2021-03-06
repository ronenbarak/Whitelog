using System;
using System.Text;
using Whitelog.Core.Binary;
using Whitelog.Core.Binary.Serializer;
using Whitelog.Core.String;

namespace Whitelog.Core.PackageDefinitions
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

        public override IPackageDefinition Clone(System.Type type, object instance)
        {
            if (!type.IsClass || type == typeof(string))
            {
                return new ObjectPackageDefinition(type);
            }
            else
            {
                return PackageDefinitionHelper.CreateInstatnce(type);
            }
        }
        
        public override System.Type GetTypeDefinition()
        {
            return m_type;
        }

        public override void JsonPackData(object data, IStringRenderer stringRenderer, StringBuilder stringBuilder)
        {
            // data connot be null here
            stringBuilder.Append(stringRenderer.ToString());
        }

        public override void Render(object data, IStringRenderer stringRenderer, StringBuilder stringBuilder)
        {
            // data connot be null here
            stringBuilder.Append(data.ToString());   
        }

        public override void PackData(IBinaryPackager packager, ISerializer serializer, object data)
        {
            serializer.Serialize(data.ToString());
        }
    }
}
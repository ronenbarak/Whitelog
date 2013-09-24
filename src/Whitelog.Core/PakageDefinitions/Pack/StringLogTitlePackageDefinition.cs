using System;
using Whitelog.Interface;
using Whitelog.Interface.LogTitles;

namespace Whitelog.Core.PakageDefinitions.Pack
{
    public sealed class StringLogTitlePackageDefinition : PackageDefinition<StringLogTitle>
    {
        private Type m_type;

        public StringLogTitlePackageDefinition():this(typeof(StringLogTitle),null)
        {
        }

        private StringLogTitlePackageDefinition(Type type, StringLogTitle instance)
        {
            m_type = type;
            if (instance == null)
            {
                DefineConstString(x => x.Title, title => title.Title, null);   
            }
            else
            {
                DefineConstString(x => x.Title, title => title.Title, instance.Title);
            }
            Define(x => x.Message, x => x.Message);
        }

        public override IBinaryPackageDefinition Clone(Type type, object instance)
        {
            return new StringLogTitlePackageDefinition(type, (StringLogTitle)instance);
        }

        public override Type GetTypeDefinition()
        {
            return m_type;
        }

        public override void PackData(IBinaryPackager packager, ISerializer serializer, object data)
        {
            StringLogTitle t = (StringLogTitle)data;
            serializer.Serialize(t.Message);
        }
    }

    public sealed class CustomStringLogTitlePackageDefinition : PackageDefinition<CustomStringLogTitle>
    {
        private Type m_type;

        public CustomStringLogTitlePackageDefinition(): this(typeof(CustomStringLogTitle))
        {
        }

        private CustomStringLogTitlePackageDefinition(Type type)
        {
            m_type = type;
            DefineCacheString(x => x.Title, title => title.Title);
            Define(x => x.Message, x => x.Message);
        }

        public override Type GetTypeDefinition()
        {
            return m_type;
        }

        public override IBinaryPackageDefinition Clone(System.Type type, object instance)
        {
            return new CustomStringLogTitlePackageDefinition(type);
        }

        public override void PackData(IBinaryPackager packager, ISerializer serializer, object data)
        {
            CustomStringLogTitle t = (CustomStringLogTitle) data;
            serializer.SerializeVariant(packager.GetCacheStringId(t.Title));
        }
    }
}
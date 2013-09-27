using System;

namespace Whitelog.Core.Binary.PakageDefinitions.Pack
{
    public class BasePropertyDefinition<T> : IPropertyDefinition
    {
        private readonly Action<T, IBinaryPackager, ISerializer> m_serilizer;

        public string Name { get; protected set; }
        public SerilizeType SerilizeType { get; protected set; }

        public BasePropertyDefinition(string property, SerilizeType serilizeType, Action<T, IBinaryPackager, ISerializer> serilizer)
        {
            m_serilizer = serilizer;
            Name = property;
            SerilizeType = serilizeType;
        }

        public void Serilize(T data, IBinaryPackager packager, ISerializer serializer)
        {
            m_serilizer.Invoke(data, packager, serializer);
        }
    }

    public class PropertyDefinition<T> : BasePropertyDefinition<T>,ISimplePropertyDefinition
    {
        public PropertyDefinition(string property, SerilizeType serilizeType, Action<T, IBinaryPackager, ISerializer> serilizer):base(property, serilizeType, serilizer)
        {
        }
    }
}
using System;
using Whitelog.Core.PackageDefinitions;

namespace Whitelog.Core.Binary.PakageDefinitions.Pack
{
    public class BaseBinaryPropertyDefinition<T> : ISimpleBinaryPropertyDefinition
    {
        private readonly Action<T, IBinaryPackager, ISerializer> m_serilizer;

        public string Name { get; protected set; }
        public SerilizeType SerilizeType { get; protected set; }

        public BaseBinaryPropertyDefinition(string property, SerilizeType serilizeType, Action<T, IBinaryPackager, ISerializer> serilizer)
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

    public class BinaryBinaryPropertyDefinition<T> : BaseBinaryPropertyDefinition<T>
    {
        public BinaryBinaryPropertyDefinition(string property, SerilizeType serilizeType, Action<T, IBinaryPackager, ISerializer> serilizer):base(property, serilizeType, serilizer)
        {
        }
    }
}
using System;

namespace Whitelog.Core.Binary.PakageDefinitions.Unpack
{
    public class UnpackPropertyDefinition<T> : IPropertyDefinition
    {
        private Action<T, IUnpacker, IDeserializer> m_unpacker;
        public string Name { get; protected set; }
        public SerilizeType SerilizeType { get; protected set; }

        public UnpackPropertyDefinition(string property, SerilizeType serilizeType, Action<T, IUnpacker, IDeserializer> unpacker)
        {
            m_unpacker = unpacker;
            Name = property;
            SerilizeType = serilizeType;
        }

        public void Unpack(T data, IUnpacker packager, IDeserializer stream)
        {
            m_unpacker.Invoke(data, packager, stream);
        }
    }
}
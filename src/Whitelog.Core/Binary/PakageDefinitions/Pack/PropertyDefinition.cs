using System;

namespace Whitelog.Core.Binary.PakageDefinitions.Pack
{
    public class BinaryPropertyDefinition<T> : ISimpleBinaryPropertyDefinition
    {
        public readonly Action<T, IBinaryPackager, ISerializer> Serilize;

        public string Name { get; protected set; }
        public SerilizeType SerilizeType { get; protected set; }

        public BinaryPropertyDefinition(string property, SerilizeType serilizeType, Action<T, IBinaryPackager, ISerializer> serilizer)
        {
            Serilize = serilizer;
            Name = property;
            SerilizeType = serilizeType;
        }
    }
}
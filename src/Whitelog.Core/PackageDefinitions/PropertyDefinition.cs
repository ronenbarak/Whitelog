using System;
using Whitelog.Core.Binary;
using Whitelog.Core.Binary.Serializer;

namespace Whitelog.Core.PackageDefinitions
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
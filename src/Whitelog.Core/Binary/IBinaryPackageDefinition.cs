using System;
using Whitelog.Core.PackageDefinitions;

namespace Whitelog.Core.Binary
{
    public interface ISimpleBinaryPropertyDefinition : IPropertyDefinition
    {
        SerilizeType SerilizeType { get; }
    }

    public interface IUnpackageDefinition : IPackageDefinition
    {
        int DefinitionId { get; set; }
        
        /// <summary>
        /// Unpack the data
        /// </summary>
        /// <returns>True if this is data object, False if interanl data definition</returns>
        object Unpack(IDeserializer deserializer, IUnpacker unpacker);
    }

    public interface IBinaryPackageDefinition : IPackageDefinition
    {
        IBinaryPackageDefinition Clone(Type type, object instance);
        Type GetTypeDefinition();
        void PackData(IBinaryPackager packager, ISerializer serializer, object data);
    }
}
using System;
using Whitelog.Core.PackageDefinitions;

namespace Whitelog.Core.Binary.Serializer
{
    public interface ISimpleBinaryPropertyDefinition : IPropertyDefinition
    {
        SerilizeType SerilizeType { get; }
    }

    public interface IBinaryPackageDefinition : IPackageDefinition
    {
        IBinaryPackageDefinition Clone(Type type, object instance);
        Type GetTypeDefinition();
        void PackData(IBinaryPackager packager, ISerializer serializer, object data);
    }
}
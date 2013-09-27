using System;
using System.Collections.Generic;

namespace Whitelog.Core.Binary
{
    public interface IPackageDefinition
    {
        IEnumerable<IPropertyDefinition> GetPropertyDefinition();
    }

    public interface IStringPackageDefinition : IPackageDefinition
    {
        string Render(IBinaryPackager packager, object data);
    }

    public interface IPropertyDefinition
    {
        string Name { get; }
        SerilizeType SerilizeType { get; }
    }

    public interface ISimplePropertyDefinition : IPropertyDefinition
    {
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
        void PackData(IBinaryPackager packager, ISerializer serializer, object data);
        Type GetTypeDefinition();
    }
}
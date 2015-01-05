using Whitelog.Core.PackageDefinitions;

namespace Whitelog.Core.Binary.Deserilizer
{
    public interface IUnpackageDefinition : IPackageDefinition
    {
        int DefinitionId { get; set; }
        
        /// <summary>
        /// Unpack the data
        /// </summary>
        /// <returns>True if this is data object, False if interanl data definition</returns>
        object Unpack(IDeserializer deserializer, IUnpacker unpacker);
    }
}
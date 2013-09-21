using System.Linq;
using Whitelog.Interface;

namespace Whitelog.Core.PakageDefinitions.Unpack
{
    public class CacheStringUnpackageDefinition : IUnpackageDefinition
    {
        public bool Unpack(IDeserializer deserializer, IUnpacker unpacker,out object data)
        {
            unpacker.SetCachedString(deserializer.DeserializeVariantInt(), deserializer.DeserializeString());
            data = null;
            return false;
        }

        public string GetTypeDefinition()
        {
            return typeof(string).FullName;
        }

        public int DefinitionId
        {
            get { return (int)KnownPackageDefinition.ChachePackageDefinition; }
            set { }
        }

        public System.Collections.Generic.IEnumerable<IPropertyDefinition> GetPropertyDefinition()
        {
            return Enumerable.Empty<IPropertyDefinition>();
        }
    }
}
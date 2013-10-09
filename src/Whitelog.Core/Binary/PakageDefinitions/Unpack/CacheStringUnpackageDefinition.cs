using System;
using System.Linq;
using Whitelog.Barak.Common.Events;
using Whitelog.Core.PackageDefinitions;

namespace Whitelog.Core.Binary.PakageDefinitions.Unpack
{
    public class CacheStringUnpackageDefinition : IUnpackageDefinition
    {
        public event EventHandler<EventArgs<CacheString>> CacheStringDeserializer;
        public object Unpack(IDeserializer deserializer, IUnpacker unpacker)
        {
            CacheString cacheString = new CacheString()
                                      {
                                          Id = deserializer.DeserializeVariantInt(),
                                          Value = deserializer.DeserializeString()
                                      };
            this.RaiseEvent(CacheStringDeserializer, cacheString);
            return cacheString;
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
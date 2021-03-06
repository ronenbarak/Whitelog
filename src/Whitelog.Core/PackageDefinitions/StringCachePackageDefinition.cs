using Whitelog.Core.Binary;

namespace Whitelog.Core.PackageDefinitions
{
    public class StringCachePackageDefinition : PackageDefinition<CacheString>
    {
        public static int ConstDefinitionId
        {
            get { return (int)KnownPackageDefinition.ChachePackageDefinition; }
        }

        public StringCachePackageDefinition()
        {
            DefineVariant(s => s.Id, s => s.Id);
            Define(s => s.Value, s => s.Value);
        }
    }
}
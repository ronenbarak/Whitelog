using Whitelog.Core.PackageDefinitions;

namespace Whitelog.Core.Binary.PakageDefinitions.Pack
{
    class ConstPropertyDefinitionDefinition : PackageDefinition<ConstStringPropertyDefinitoin>
    {
        public static int ConstDefinitionId
        {
            get { return (int)KnownPackageDefinition.ConstPropertyDefinitionDefinition; }
        }

        public ConstPropertyDefinitionDefinition()
        {
            Define(x => x.Name, x => x.Name)
            .Define(x=>x.SerilizeType, x => (byte)x.SerilizeType)
            .Define(x=>x.Value, x => x.Value);
        }
    }

    class PropertyDefinitionDefinition : PackageDefinition<ISimpleBinaryPropertyDefinition>
    {
        public static int ConstDefinitionId
        {
            get { return (int)KnownPackageDefinition.PropertyDefinitionDefinition; }
        }

        public PropertyDefinitionDefinition()
        {
            Define(x => x.Name, x => x.Name)
                .Define(x=>x.SerilizeType, x => (byte)x.SerilizeType);
        }
    }
}
using Whitelog.Core.PackageDefinitions;

namespace Whitelog.Core.Binary.PakageDefinitions.Pack
{
    class RegisteredPackageDefinitionDefinition : PackageDefinition<RegisteredPackageDefinition>
    {
        public static int ConstDefinitionId
        {
            get { return (int)KnownPackageDefinition.BinaryPackagedDefinitionDefinition; }
        }

        public RegisteredPackageDefinitionDefinition()
        {
            DefineVariant(x => x.DefinitionId, x => x.DefinitionId)
            .DefineVariant(x => x.BaseDefinitionId, x => x.BaseDefinitionId)
            .Define("FullName", x => x.Definition.GetTypeDefinition().FullName)
            .Define("PropertyDefinitions", x => x.Definition.GetPropertyDefinition());
        }
    }
}
using Whitelog.Core.Binary;
using Whitelog.Core.Binary.Serializer;

namespace Whitelog.Core.PackageDefinitions
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
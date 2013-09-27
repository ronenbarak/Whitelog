namespace Whitelog.Core.Binary
{
    public sealed class RegisteredPackageDefinition
    {
        public int DefinitionId { get; set; }
        public int BaseDefinitionId { get; set; }

        public readonly IBinaryPackageDefinition Definition;

        public RegisteredPackageDefinition(IBinaryPackageDefinition definition,int definitionId,int baseDefinitionId)
        {
            DefinitionId = definitionId;
            BaseDefinitionId = baseDefinitionId;
            Definition = definition;
        }
    }
}
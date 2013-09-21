using Whitelog.Interface;
using Whitelog.Core.PakageDefinitions;
using Whitelog.Core.PakageDefinitions.Unpack;

namespace Whitelog.Core.Generic
{
    public class ConstGenericPropertyUnpackageDefinition : UnpackageDefinition<GenericPackageProperty>
    {
        public ConstGenericPropertyUnpackageDefinition()
        {
            DefineString("Name", (property, s) => property.Property = s);
            DefineByte("SerilizeType", (property, b) => property.SerilizeType = (SerilizeType)b);
            DefineString("Value", (property, s) => property.Value = s);
        }

        public override int DefinitionId
        {
            get { return (int)KnownPackageDefinition.ConstPropertyDefinitionDefinition; }
            set
            {
            }
        }

        protected override GenericPackageProperty CreateInstance()
        {
            return new GenericPackageProperty();
        }
    }

    public class GenericPropertyUnpackageDefinition : UnpackageDefinition<GenericPackageProperty>
    {
        public GenericPropertyUnpackageDefinition()
        {
            DefineString("Name", (property, s) => property.Property = s);
            DefineByte("SerilizeType", (property, b) => property.SerilizeType = (SerilizeType)b);
        }

        public override int DefinitionId
        {
            get { return (int)KnownPackageDefinition .PropertyDefinitionDefinition; }
            set
            {
            }
        }

        protected override GenericPackageProperty CreateInstance()
        {
            return new GenericPackageProperty();
        }
    }
}
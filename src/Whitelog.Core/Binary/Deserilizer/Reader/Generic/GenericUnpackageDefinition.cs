using System.Collections.Generic;
using Whitelog.Core.Binary.Deserilizer.Unpack;

namespace Whitelog.Core.Binary.Deserilizer.Reader.Generic
{
    public class GenericUnpackageDefinition : UnpackageDefinition<IEntryData>
    {
        private readonly List<IPropertyInfo> m_propertyDefinitions = new List<IPropertyInfo>();
        public GenericComponentType Type { get; set; }

        public void AddProperty(IPropertyInfo propertyInfo)
        {
            m_propertyDefinitions.Add(propertyInfo);
        }
        
        protected override IEntryData CreateInstance()
        {
            return new GenericPackageData(Type, m_propertyDefinitions);
        }
    }
}
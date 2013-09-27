using System.Collections.Generic;
using Whitelog.Core.Binary.PakageDefinitions.Unpack;

namespace Whitelog.Core.Binary.Generic
{
    public class GenericUnpackageDefinition : UnpackageDefinition<ILogEntryData>
    {
        private readonly List<IPropertyInfo> m_propertyDefinitions = new List<IPropertyInfo>();
        public GenericComponentType Type { get; set; }

        public void AddProperty(IPropertyInfo propertyInfo)
        {
            m_propertyDefinitions.Add(propertyInfo);
        }
        
        protected override ILogEntryData CreateInstance()
        {
            return new GenericPackageData(Type, m_propertyDefinitions);
        }
    }
}
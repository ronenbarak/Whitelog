using System.Collections.Generic;

namespace Whitelog.Core.Binary.Deserilizer.Reader.Generic
{
    public class GenericPackageData : IEntryData
    {

        private readonly object[] m_propertiesValue;
        private readonly IComponentType m_type;
        private readonly IList<IPropertyInfo> m_propertyDefinitions;

        public GenericPackageData(IComponentType type, IList<IPropertyInfo> propertyDefinitions)
        {
            m_propertyDefinitions = propertyDefinitions;
            m_propertiesValue = new object[propertyDefinitions.Count];
            m_type = type;
        }

        public string Type { get { return m_type.FullName; }}

        public object GetValue(int index)
        {
            return m_propertiesValue[index];
        }

        public void SetValue(int index, object value)
        {
            m_propertiesValue[index] = value;
        }


        public IComponentType GetEntryType()
        {
            return m_type;
        }

        public IEnumerable<IPropertyInfo> GetProperties()
        {
            return m_propertyDefinitions;
        }
    }
}
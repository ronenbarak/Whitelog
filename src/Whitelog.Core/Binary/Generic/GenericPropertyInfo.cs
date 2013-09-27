using System;

namespace Whitelog.Core.Binary.Generic
{
    public class GenericPropertyInfo : IPropertyInfo
    {
        private IComponentType m_componentType;
        private string m_name;
        private int m_index;

        public GenericPropertyInfo(IComponentType componentType, string name, int index)
        {
            m_index = index;
            m_name = name;
            m_componentType = componentType;
        }

        public IComponentType ComponentType
        {
            get { return m_componentType; }
        }

        public Type Type { get; set; }

        public string Name
        {
            get { return m_name; }
        }

        public void SetValue(ILogEntryData instance , object data)
        {
            var obj = instance as GenericPackageData;
            obj.SetValue(m_index,data);
        }

        public object GetValue(ILogEntryData instance)
        {
            var obj = instance as GenericPackageData;
            return obj.GetValue(m_index);
        }
    }
}
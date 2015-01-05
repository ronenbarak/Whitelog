using System;
using Whitelog.Core.Binary;

namespace Whitelog.Core.PackageDefinitions
{
    public class ConstStringPropertyDefinitoin : IPropertyDefinition
    {
        private Func<object, string> m_dataExtractort;
        private readonly string m_propertyName;

        public ConstStringPropertyDefinitoin(string propertyName, Func<object, string> dataExtractort,string value)
        {
            m_propertyName = propertyName;
            m_dataExtractort = dataExtractort;
            Value = value;
        }

        public ConstStringPropertyDefinitoin Clone(object instance)
        {
            var value = m_dataExtractort.Invoke(instance);
            return new ConstStringPropertyDefinitoin(m_propertyName, m_dataExtractort,value);
        }

        public string Value { get; protected set; }

        public string Name
        {
            get { return m_propertyName; }
        }

        public SerilizeType SerilizeType
        {
            get { return SerilizeType.ConstString; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Whitelog.Core.Binary;
using Whitelog.Core.PackageDefinitions;

namespace Whitelog.Core.String
{
    public class StringPropertyDefinition<T> : IPropertyDefinition
    {
        private Action<T, IStringRenderer, StringBuilder> m_valueExtractor;
        public string Name { get; private set; }

        public StringPropertyDefinition(string name, Action<T, IStringRenderer, StringBuilder> valueExtractor)
        {
            Name = name;
            m_valueExtractor = valueExtractor;
        }

        public void Render(T instance,IStringRenderer stringRenderer, StringBuilder stringBuilder)
        {
            stringBuilder.Append(Name);
            stringBuilder.Append("=");
            m_valueExtractor.Invoke(instance, stringRenderer, stringBuilder);
        }
    }
}

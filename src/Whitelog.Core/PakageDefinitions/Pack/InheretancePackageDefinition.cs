using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Whitelog.Barak.Common.ExtensionMethods;
using Whitelog.Interface;
using Whitelog.Interface.LogTitles;

namespace Whitelog.Core.PakageDefinitions.Pack
{
    internal class InheritancePackageDefinition<T> : PackageDefinition<T>
    {
        private readonly Type m_type;

        public InheritancePackageDefinition(List<BasePropertyDefinition<T>> propertyDefinitions,List<ConstStringPropertyDefinitoin> constStringPropertyDefinitoins, Type type)
        {
            m_type = type;
            m_definitions = propertyDefinitions.ToArray();
            m_constDefinitions = constStringPropertyDefinitoins;
        }

        public override Type GetTypeDefinition()
        {
            return m_type;
        }
    }
}
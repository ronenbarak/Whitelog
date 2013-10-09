using System;
using System.Collections.Generic;
using Whitelog.Core.PackageDefinitions;

namespace Whitelog.Core.Binary.PakageDefinitions.Pack
{
    internal class InheritancePackageDefinition<T> : PackageDefinition<T>
    {
        private readonly Type m_type;

        public InheritancePackageDefinition(List<BaseBinaryPropertyDefinition<T>> propertyDefinitions,List<ConstStringPropertyDefinitoin> constStringPropertyDefinitoins, Type type)
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
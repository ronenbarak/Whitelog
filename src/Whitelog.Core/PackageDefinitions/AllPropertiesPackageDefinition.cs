using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Whitelog.Core.Binary;
using Whitelog.Core.Binary.PakageDefinitions.Pack;

namespace Whitelog.Core.PackageDefinitions
{
    public static class PackageDefinitionHelper
    {
        public static IPackageDefinition CreateInstatnce(Type type)
        {
            var baseType = typeof(AllPropertiesPackageDefinition<>);
            var packageDefinitionType = baseType.MakeGenericType(new[] { type });
            var packageDefinitionInstance = Activator.CreateInstance(packageDefinitionType) as IPackageDefinition;
            return packageDefinitionInstance;
        }
    }

    public class AllPropertiesPackageDefinition<T> : InheritancePackageDefinition<T,object>
    {
    }
}
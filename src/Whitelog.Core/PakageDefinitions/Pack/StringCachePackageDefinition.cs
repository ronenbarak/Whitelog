using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Whitelog.Interface;

namespace Whitelog.Core.PakageDefinitions.Pack
{
    public class StringCachePackageDefinition : PackageDefinition<CacheString>
    {
        public static int ConstDefinitionId
        {
            get { return (int)KnownPackageDefinition.ChachePackageDefinition; }
        }

        public StringCachePackageDefinition()
        {
            DefineVariant(s => s.Id, s => s.Id);
            Define(s => s.Value, s => s.Value);
        }
    }
}
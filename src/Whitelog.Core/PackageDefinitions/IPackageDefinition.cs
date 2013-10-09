using System;
using System.Collections.Generic;
using Whitelog.Core.Binary;

namespace Whitelog.Core.PackageDefinitions
{
    public interface IPackageDefinition
    {
        IEnumerable<IPropertyDefinition> GetPropertyDefinition();
    }
}
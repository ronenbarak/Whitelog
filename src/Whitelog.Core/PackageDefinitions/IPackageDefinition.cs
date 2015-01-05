using System.Collections.Generic;

namespace Whitelog.Core.PackageDefinitions
{
    public interface IPackageDefinition
    {
        IEnumerable<IPropertyDefinition> GetPropertyDefinition();
    }
}
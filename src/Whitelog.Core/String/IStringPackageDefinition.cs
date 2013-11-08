using System;
using System.Text;
using Whitelog.Core.PackageDefinitions;

namespace Whitelog.Core.String
{
    public interface IStringPackageDefinition : IPackageDefinition
    {
        IStringPackageDefinition Clone(Type type, object instance);
        void Render(object data, IStringRenderer stringRenderer, StringBuilder stringBuilder);
        Type GetTypeDefinition();
    }
}
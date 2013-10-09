using System;
using System.Text;
using Whitelog.Core.PackageDefinitions;

namespace Whitelog.Core.String
{
    public interface IStringPackageDefinition : IPackageDefinition
    {
        void Render(object data, IStringRenderer stringRenderer, StringBuilder stringBuilder);
        Type GetTypeDefinition();
    }
}
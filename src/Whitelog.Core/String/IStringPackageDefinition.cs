using System;
using System.Text;
using Whitelog.Core.PackageDefinitions;

namespace Whitelog.Core.String
{
    public interface IJsonPackageDefinition : IPackageDefinition
    {
        IJsonPackageDefinition Clone(Type type, object instance);
        void JsonPackData(object data, IStringRenderer stringRenderer, StringBuilder stringBuilder);
        Type GetTypeDefinition();
    }

    public interface IStringPackageDefinition : IPackageDefinition
    {
        IStringPackageDefinition Clone(Type type, object instance);
        void Render(object data, IStringRenderer stringRenderer, StringBuilder stringBuilder);
        Type GetTypeDefinition();
    }
}
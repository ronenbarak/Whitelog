using System;
using System.Collections.Generic;

namespace Whitelog.Core.Binary.Deserilizer.Reader.Generic
{
    public interface IComponentType
    {
        string FullName { get; }
    }

    public interface IPropertyInfo
    {
        IComponentType ComponentType { get; }
        Type Type { get; }
        string Name { get; }
        void SetValue(IEntryData instance, object data);
        object GetValue(IEntryData instance);
    }

    public interface IEntryData
    {
        IComponentType GetEntryType();
        IEnumerable<IPropertyInfo> GetProperties();
    }
}
using System;
using System.Collections.Generic;

namespace Whitelog.Core
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
        void SetValue(ILogEntryData instance, object data);
        object GetValue(ILogEntryData instance);
    }

    public interface ILogEntryData
    {
        IComponentType GetEntryType();
        IEnumerable<IPropertyInfo> GetProperties();
    }
}
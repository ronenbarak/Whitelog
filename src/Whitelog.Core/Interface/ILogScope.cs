using System;

namespace Whitelog.Interface
{
    public interface ILogScope : IDisposable
    {
        int LogScopeId { get; }
    }
}
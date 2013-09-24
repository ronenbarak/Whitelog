namespace Whitelog.Core
{
    public interface ILogScopeSyncImplementation
    {
        LogScope LogScope { get; set; }
        int GetScopeId();
    }
}
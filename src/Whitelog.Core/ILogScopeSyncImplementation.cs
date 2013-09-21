namespace Whitelog.Interface
{
    public interface ILogScopeSyncImplementation
    {
        LogScope LogScope { get; set; }
        int GetScopeId();
    }
}
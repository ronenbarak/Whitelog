namespace Whitelog.Core.Loggers
{
    public interface ILogger
    {
        void AttachToTunnelLog(LogTunnel logTunnel);
        void DetachTunnelLog(LogTunnel logTunnel);
    }
}
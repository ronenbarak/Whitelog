using System;

namespace Whitelog.Core.Binary
{
    public interface IBinaryLogger : IDisposable
    {
        void AttachToTunnelLog(LogTunnel logTunnel);
        void DetachTunnelLog(LogTunnel logTunnel);

        void RegisterDefinition(IBinaryPackageDefinition packageDefinition);
    }
}
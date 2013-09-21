using System;
using Whitelog.Interface;

namespace Whitelog.Core
{
    public interface IBinaryLogger : IDisposable
    {
        void AttachToTunnelLog(LogTunnel logTunnel);
        void DetachTunnelLog(LogTunnel logTunnel);

        void RegisterDefinition(IBinaryPackageDefinition packageDefinition);
    }
}
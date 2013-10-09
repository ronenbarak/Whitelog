using System;
using Whitelog.Core.Loggers;

namespace Whitelog.Core.Binary
{
    public interface IBinaryLogger : ILogger, IDisposable
    {
        void RegisterDefinition(IBinaryPackageDefinition packageDefinition);
    }
}
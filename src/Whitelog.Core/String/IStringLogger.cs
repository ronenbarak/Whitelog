using Whitelog.Core.Loggers;

namespace Whitelog.Core.String
{
    public interface IStringLogger : ILogger
    {
        void RegisterDefinition(IStringPackageDefinition packageDefinition);
    }
}
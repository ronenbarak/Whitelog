using Whitelog.Interface;

namespace Whitelog.Core
{
    public interface IBinaryPackager
    {
        int GetCacheStringId(string value);
        void Pack(object data, ISerializer serializer);
    }
}
namespace Whitelog.Core.Binary
{
    public interface IBinaryPackager
    {
        int GetCacheStringId(string value);
        void Pack(object data, ISerializer serializer);
    }
}
using System;
using System.IO;

namespace Whitelog.Interface
{
    public interface IBinaryPackager
    {
        int GetCacheStringId(string value);
        void Pack(object data, ISerializer serializer);
    }
}
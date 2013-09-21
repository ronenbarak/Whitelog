namespace Whitelog.Interface
{
    public enum SerilizeType:byte
    {
        Int32 = 0,
        Double,
        Bool,
        DateTime,
        Int64,
        Byte,
        String,
        Enumerable,
        Guid,
        VariantUInt32,
        CacheString,

        ConstString = System.Byte.MaxValue -1,
        Object = System.Byte.MaxValue,
    }
}
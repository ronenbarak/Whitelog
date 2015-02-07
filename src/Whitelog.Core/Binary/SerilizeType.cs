namespace Whitelog.Core.Binary
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
        UInt32,
        SByte,
        Short,
        UShort,
        UInt64,
        Float,
        Char,
        Decimal,

        ConstString = System.Byte.MaxValue -1,
        Object = System.Byte.MaxValue,
    }
}
using System;

namespace Whitelog.Core.Binary.Deserilizer.Unpack
{
    public class UnkwonSerilizeTypeException : Exception
    {
        protected SerilizeType SerilizeType { get; set; }

        public UnkwonSerilizeTypeException(SerilizeType serilizeType)
            : base(string.Format("Unkwon SerilizeType detected {0}", serilizeType))
        {
            SerilizeType = serilizeType;
        }
    }
}
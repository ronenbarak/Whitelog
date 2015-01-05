using System;

namespace Whitelog.Core.Binary.Deserilizer
{
    public class UnkownPackageException : Exception
    {
        public int PackageId { get; private set; }

        public UnkownPackageException(int packageId)
            : base(string.Format("Unknwon package defintion {0}", packageId))
        {
            PackageId = packageId;
        }
    }
}
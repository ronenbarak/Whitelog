using System;

namespace Whitelog.Core
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
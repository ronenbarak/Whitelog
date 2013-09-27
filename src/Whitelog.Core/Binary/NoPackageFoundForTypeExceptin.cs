using System;

namespace Whitelog.Core.Binary
{
    class NoPackageFoundForTypeExceptin : Exception
    {
        public Type MissingType { get; set; }

        public NoPackageFoundForTypeExceptin(Type type)
            : base(string.Format("No package definition found for type {0}",type))
        {
            MissingType = type;
        }
    }
}
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Whitelog.Core
{
    public class ObjectReferanceEquals : IEqualityComparer<object>
    {
        bool IEqualityComparer<object>.Equals(object x, object y)
        {
            return object.ReferenceEquals(x, y);
        }

        int IEqualityComparer<object>.GetHashCode(object obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }
    }
}
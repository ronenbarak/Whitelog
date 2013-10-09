using System;
using System.Collections.Generic;

namespace Whitelog.Core
{
    class TypeComparer : IEqualityComparer<Type>
    {
        public bool Equals(Type x, Type y)
        {
            return x == y;
        }

        public int GetHashCode(Type obj)
        {
            return obj.GetHashCode();
        }
    }
}
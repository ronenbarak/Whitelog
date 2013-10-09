using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Whitelog.Core
{
    public class AnonymousTypesHelper
    {
        public static bool IsAnonymousType(Type type)
        {
            return Attribute.IsDefined(type, typeof (CompilerGeneratedAttribute)) &&
                  type.IsGenericType && type.Name.Contains("AnonymousType") &&
                  type.GetGenericArguments().Length == type.GetProperties().Count();
        }
    }
}

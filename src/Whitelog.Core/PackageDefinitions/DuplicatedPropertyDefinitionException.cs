using System;

namespace Whitelog.Core.PackageDefinitions
{
    class DuplicatedPropertyDefinitionException : Exception
    {
        public string Property { get; protected set; }

        public DuplicatedPropertyDefinitionException(string property):base(string.Format("The property '{0}' is already defined in package",property))
        {
            Property = property;
        }
    }
}
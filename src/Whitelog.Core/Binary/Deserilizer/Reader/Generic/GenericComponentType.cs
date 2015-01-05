using System;

namespace Whitelog.Core.Binary.Deserilizer.Reader.Generic
{
    public class GenericComponentType : IComponentType, IEquatable<GenericComponentType>
    {
        private readonly string m_fullName;
        private readonly int m_id;

        public GenericComponentType(string fullName, int Id)
        {
            m_id = Id;
            m_fullName = fullName;
        }

        public string FullName
        {
            get { return m_fullName; }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GenericComponentType);
        }

        public override int GetHashCode()
        {
            return m_id;
        }

        public bool Equals(GenericComponentType other)
        {
            if (other == null)
            {
                return false;
            }

            return m_id == other.m_id;
        }
    }
}
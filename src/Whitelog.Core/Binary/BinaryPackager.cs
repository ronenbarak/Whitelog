using System;
using System.Collections.Generic;
using Whitelog.Barak.Common.DataStructures.Dictionary;
using Whitelog.Barak.Common.Events;
using Whitelog.Core.Binary.PakageDefinitions;

namespace Whitelog.Core.Binary
{
    public class TypeComparer : IEqualityComparer<Type>
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

    public class StringComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return x == y;
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }

    public class BinaryPackager : IBinaryPackager
    {
        public event EventHandler<EventArgs<RegisteredPackageDefinition>> PackageRegistered;
        public event EventHandler<EventArgs<RegisteredPackageDefinition>> PackageUnregistered;
        public event EventHandler<EventArgs<CacheString>> StringChached;

        protected readonly ReadSafeDictionary<Type, RegisteredPackageDefinition> m_PackageDefinitions = new ReadSafeDictionary<Type, RegisteredPackageDefinition>(new TypeComparer());

        protected readonly ReadSafeDictionary<object, int> m_stringCacheByObjectReferance = new ReadSafeDictionary<object, int>(new ObjectReferanceEquals());
        protected readonly ReadSafeDictionary<string, int> m_stringCacheByString = new ReadSafeDictionary<string, int>(new StringComparer());
        protected readonly object m_definitionSyncObjectLock = new object();
        private int m_cacheStringIdentity = 1;
        private int m_packegeId = 1;

        protected virtual RegisteredPackageDefinition GetInheretancePackageDefinition(Type type, RegisteredPackageDefinition sourcePackageDefinition,object instance)
        {
            if (sourcePackageDefinition.DefinitionId == (int)KnownPackageDefinition.PropertyDefinitionDefinition)
            {
                m_PackageDefinitions.TryAdd(type, sourcePackageDefinition);
                return sourcePackageDefinition;
            }
            else
            {
                IBinaryPackageDefinition packageDefinition = sourcePackageDefinition.Definition.Clone(type, instance);
                RegisteredPackageDefinition registeredPackageDefinition;
                RegisterDefinition(packageDefinition, sourcePackageDefinition.DefinitionId, out registeredPackageDefinition);
                return registeredPackageDefinition;   
            }
        }

        public void Pack(object data, ISerializer serializer)
        {
            if (data == null)
            {
                serializer.SerializeVariant(0);
            }
            else
            {
                Type type = data.GetType();
                RegisteredPackageDefinition packageDefinition;
                if (!m_PackageDefinitions.TryGetValue(type, out packageDefinition))
                {
                    lock (m_PackageDefinitions)
                    {
                        if (!m_PackageDefinitions.TryGetValue(type, out packageDefinition))
                        {
                            // Get the closest packageDefinition
                            packageDefinition = GetClosestPackageDefinition(type);
                            if (packageDefinition != null)
                            {
                                packageDefinition = GetInheretancePackageDefinition(type, packageDefinition, data);
                            }
                        }
                    }
                }

                if (packageDefinition == null)
                {
                    throw new NoPackageFoundForTypeExceptin(type);
                }
                serializer.SerializeVariant(packageDefinition.DefinitionId + 1);
                packageDefinition.Definition.PackData(this, serializer, data);
            }
        }

        public int GetCacheStringId(string value)
        {
            int identityId;
            if (!m_stringCacheByObjectReferance.TryGetValue(value, out identityId))
            {
                if (!m_stringCacheByString.TryGetValue(value, out identityId))
                {
                    lock (m_definitionSyncObjectLock)
                    {
                        if (!m_stringCacheByObjectReferance.TryGetValue(value, out identityId))
                        {
                            if (!m_stringCacheByString.TryGetValue(value, out identityId))
                            {
                                var cacheString = new CacheString
                                                  {
                                                      Id = m_cacheStringIdentity++,
                                                      Value = value
                                                  };

                                this.RaiseEvent(StringChached, cacheString);

                                m_stringCacheByObjectReferance.TryAdd(value, cacheString.Id);
                                m_stringCacheByString.TryAdd(value, cacheString.Id);
                                identityId = cacheString.Id;
                            }
                        }
                    }
                }
            }
            return identityId;
        }

        protected RegisteredPackageDefinition GetClosestPackageDefinition(Type currDataType)
        {
            int distance = int.MaxValue;
            RegisteredPackageDefinition closestDefinition = null;
            foreach (var logPackageDefinition in m_PackageDefinitions)
            {
                if (logPackageDefinition.Key.IsAssignableFrom(currDataType))
                {
                    var currType = currDataType;
                    int currentDistance = 0;
                    while (currType != null && logPackageDefinition.Key.IsAssignableFrom(currType))
                    {
                        currentDistance++;
                        currType = currType.BaseType;
                    }

                    if (currentDistance < distance)
                    {
                        distance = currentDistance;
                        closestDefinition = logPackageDefinition.Value;
                    }
                }
            }
            return closestDefinition;
        }
        
        public bool RegisterDefinition(IBinaryPackageDefinition packageDefinition, int packegeId,int basepackegeId, out RegisteredPackageDefinition registeredPackageDefinition)
        {
            lock (m_definitionSyncObjectLock)
            {
                registeredPackageDefinition = new RegisteredPackageDefinition(packageDefinition, packegeId,
                                                                              basepackegeId);
                if (m_PackageDefinitions.TryAdd(packageDefinition.GetTypeDefinition(), registeredPackageDefinition))
                {
                    this.RaiseEvent(PackageRegistered, registeredPackageDefinition);
                    return true;
                }
                return false;
            }
        }

        public bool RegisterDefinition(IBinaryPackageDefinition packageDefinition)
        {
            RegisteredPackageDefinition temp;
            return RegisterDefinition(packageDefinition, out temp);
        }

        public bool RegisterDefinition(IBinaryPackageDefinition packageDefinition, out RegisteredPackageDefinition registeredPackageDefinition)
        {
            return RegisterDefinition(packageDefinition, m_packegeId++,(int)KnownPackageDefinition.NoDefinition, out registeredPackageDefinition);
        }

        protected bool RegisterDefinition(IBinaryPackageDefinition packageDefinition,int baseDefinitionId, out RegisteredPackageDefinition registeredPackageDefinition)
        {
            return RegisterDefinition(packageDefinition, m_packegeId++, baseDefinitionId, out registeredPackageDefinition);
        }

        /*public virtual bool UnregisterDefinition(IBinaryPackageDefinition packageDefinition)
        {
            bool removed = m_PackageDefinitions.TryRemove(packageDefinition.GetTypeDefinition(), out packageDefinition);
            this.RaiseEvent(PackageUnregistered, packageDefinition);
            return removed;
        }*/
    }
}
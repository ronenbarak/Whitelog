using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Whitelog.Barak.Common.ExtensionMethods;

namespace Whitelog.Core.Binary.PakageDefinitions.Pack
{
    class DuplicatedPropertyDefinitionException : Exception
    {
        public string Property { get; protected set; }

        public DuplicatedPropertyDefinitionException(string property):base(string.Format("The property '{0}' is already defined in package",property))
        {
            Property = property;
        }
    }

    public class PackageDefinition<T> : IBinaryPackageDefinition
    {
        private static readonly byte[] ZeroByteArray = BitConverter.GetBytes((int)-1);

        protected BasePropertyDefinition<T>[] m_definitions = new BasePropertyDefinition<T>[0];
        protected List<ConstStringPropertyDefinitoin> m_constDefinitions = new List<ConstStringPropertyDefinitoin>();

        public virtual IBinaryPackageDefinition Clone(Type type, object instance)
        {
            return new InheritancePackageDefinition<T>(m_definitions.ToList(), m_constDefinitions.Select(p=>p.Clone(instance)).ToList(), type);
        }

        public virtual Type GetTypeDefinition()
        {
	        return typeof(T);
        }

        public virtual IEnumerable<IPropertyDefinition> GetPropertyDefinition()
        {
            return ((IEnumerable<IPropertyDefinition>)m_definitions).Union(m_constDefinitions).ToList();
        }

        public virtual void PackData(IBinaryPackager packager, ISerializer serializer, object data)
        {
            T instance = (T)data;
            var temp = m_definitions;
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i].Serilize(instance, packager, serializer);
            }
        }

        protected void AddDefinition(string property, SerilizeType serilizeType, Action<T, IBinaryPackager, ISerializer> serilizer)
        {
            ValidateDefinitionUniq(property);
            Array.Resize(ref m_definitions, m_definitions.Length + 1);
            m_definitions[m_definitions.Length -1] = new PropertyDefinition<T>(property, serilizeType, serilizer);
        }

        private void ValidateDefinitionUniq(string property)
        {
            if (((IEnumerable<IPropertyDefinition>) m_definitions).Union(m_constDefinitions).Any(p => p.Name == property))
            {
                throw new DuplicatedPropertyDefinitionException(property);
            }
        }

        public PackageDefinition<T> Define(Expression<Func<T,object>> expression, Func<T, int> dataExtractor)
        {
            return Define(ObjectHelper.GetMemberName(expression), dataExtractor);
        }

        public PackageDefinition<T> DefineVariant(Expression<Func<T, object>> expression, Func<T, int> dataExtractor)
        {
            return DefineVariant(ObjectHelper.GetMemberName(expression), dataExtractor);
        }

        public PackageDefinition<T> Define(Expression<Func<T, object>> expression, Func<T, byte> dataExtractor)
        {
            return Define(ObjectHelper.GetMemberName(expression), dataExtractor);
        }

        public PackageDefinition<T> Define(Expression<Func<T, object>> expression, Func<T, double> dataExtractor)
        {
            return Define(ObjectHelper.GetMemberName(expression), dataExtractor);
        }

        public PackageDefinition<T> Define(Expression<Func<T, object>> expression, Func<T, long> dataExtractor)
        {
            return Define(ObjectHelper.GetMemberName(expression), dataExtractor);
        }

        public PackageDefinition<T> Define(Expression<Func<T, object>> expression, Func<T, bool> dataExtractor)
        {
            return Define(ObjectHelper.GetMemberName(expression), dataExtractor);
        }

        public PackageDefinition<T> Define(Expression<Func<T, object>> expression, Func<T, DateTime> dataExtractor)
        {
            return Define(ObjectHelper.GetMemberName(expression), dataExtractor);
        }

        public PackageDefinition<T> Define(Expression<Func<T, object>> expression, Func<T, string> dataExtractor)
        {
            return Define(ObjectHelper.GetMemberName(expression), dataExtractor);
        }

        public PackageDefinition<T> DefineCacheString(Expression<Func<T, object>> expression, Func<T, string> dataExtractor)
        {
            return DefineCacheString(ObjectHelper.GetMemberName(expression), dataExtractor);
        }

        public PackageDefinition<T> DefineConstString(Expression<Func<T, object>> expression, Func<T, string> dataExtractor, string initialValue)
        {
            return DefineConstString(ObjectHelper.GetMemberName(expression), dataExtractor, initialValue);
        }

        public PackageDefinition<T> Define(Expression<Func<T, object>> expression, Func<T, IEnumerable> dataExtractor)
        {
            return Define(ObjectHelper.GetMemberName(expression), dataExtractor);
        }

        public PackageDefinition<T> DefineCollection(Expression<Func<T, object>> expression, Func<T, ICollection> dataExtractor)
        {
            return DefineCollection(ObjectHelper.GetMemberName(expression), dataExtractor);
        }
        
        public PackageDefinition<T> DefineArray(Expression<Func<T, object>> expression, Func<T, object[]> dataExtractor)
        {
            return DefineArray(ObjectHelper.GetMemberName(expression), dataExtractor);
        }

        public PackageDefinition<T> Define(Expression<Func<T, object>> expression, Func<T, Guid> dataExtractor)
        {
            return Define(ObjectHelper.GetMemberName(expression), dataExtractor);
        }

        public PackageDefinition<T> Define(Expression<Func<T, object>> expression, Func<T, object> dataExtractor)
        {
            return Define(ObjectHelper.GetMemberName(expression), dataExtractor);
        }

        public PackageDefinition<T> Define(string property, Func<T, int> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Int32, (arg, packager, serializer) => serializer.Serialize(dataExtractor.Invoke(arg)));
            return this;
        }

        public PackageDefinition<T> DefineVariant(string property, Func<T, int> dataExtractor)
        {
            AddDefinition(property, SerilizeType.VariantUInt32, (arg, packager, serializer) => serializer.SerializeVariant(dataExtractor.Invoke(arg)));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, double> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Double, (arg, packager, serializer) => serializer.Serialize(dataExtractor.Invoke(arg)));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, bool> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Bool, (arg, packager, serializer) => serializer.Serialize(dataExtractor.Invoke(arg)));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, byte> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Byte, (arg, packager, serializer) => serializer.Serialize(dataExtractor.Invoke(arg)));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, DateTime> dataExtractor)
        {
            AddDefinition(property, SerilizeType.DateTime, (arg, packager, serializer) => serializer.Serialize(dataExtractor.Invoke(arg).Ticks));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, long> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Int64, (arg, packager, serializer) => serializer.Serialize(dataExtractor.Invoke(arg)));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, Guid> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Guid, (arg, packager, serializer) => serializer.Serialize(dataExtractor.Invoke(arg).ToByteArray(),0, 16));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, string> dataExtractor)
        {
            AddDefinition(property, SerilizeType.String, (arg, packager, serializer) => serializer.Serialize(dataExtractor.Invoke(arg)));

            return this;
        }

        public PackageDefinition<T> DefineCacheString(string property, Func<T, string> dataExtractor)
        {
            AddDefinition(property, SerilizeType.CacheString, (arg1, packager, serializer) => serializer.SerializeVariant(packager.GetCacheStringId(dataExtractor.Invoke(arg1))));
            return this;
        }

        public PackageDefinition<T> DefineConstString(string property, Func<T, string> dataExtractor, string initialValue)
        {
            ValidateDefinitionUniq(property);
            m_constDefinitions.Add(new ConstStringPropertyDefinitoin(property,o => dataExtractor.Invoke((T)o),initialValue));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, IEnumerable> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Enumerable, (arg, packager, serializer) =>
                                                             {
                                                                 var enumerable = dataExtractor.Invoke(arg);
                                                                 if (enumerable == null)
                                                                 {
                                                                     serializer.SerializeVariant(0);
                                                                 }
                                                                 else
                                                                 {
                                                                     int itemCount = 1; // we add one more to tell it is not null
                                                                     foreach (var item in enumerable)
                                                                     {
                                                                         itemCount++;
                                                                     }
                                                                     serializer.SerializeVariant(itemCount);

                                                                     foreach (var item in enumerable)
                                                                     {
                                                                         packager.Pack(item, serializer);
                                                                     }
                                                                 }
                                                             });

            return this;
        }

        public PackageDefinition<T> DefineCollection(string property, Func<T, ICollection> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Enumerable, (arg, packager, serializer) =>
            {
                var enumerable = dataExtractor.Invoke(arg);
                if (enumerable == null)
                {
                    serializer.SerializeVariant(0);
                }
                else
                {
                    serializer.SerializeVariant(enumerable.Count + 1); // we add one more to tell it is not null

                    foreach (var item in enumerable)
                    {
                        packager.Pack(item, serializer);
                    }
                }
            });

            return this;
        }

        public PackageDefinition<T> DefineArray(string property, Func<T, object[]> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Enumerable, (arg, packager, serializer) =>
            {
                object[] array = dataExtractor.Invoke(arg);
                if (array == null)
                {
                    serializer.SerializeVariant(0);
                }
                else
                {
                    serializer.SerializeVariant(array.Length + 1); // we add one more to tell it is not null

                    for (int i = 0; i < array.Length; i++)
                    {
                        packager.Pack(array[i], serializer);
                    }
                }
            });

            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, object> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Object, (arg, packager, serializer) => packager.Pack(dataExtractor.Invoke(arg), serializer));
            return this;
        }
    }
}

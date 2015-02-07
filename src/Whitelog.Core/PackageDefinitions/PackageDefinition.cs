using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using Whitelog.Barak.Common.ExtensionMethods;
using Whitelog.Core.Binary;
using Whitelog.Core.Binary.Serializer;
using Whitelog.Core.String;

namespace Whitelog.Core.PackageDefinitions
{

    [StructLayout(LayoutKind.Explicit)]
    struct IntFloat
    {
        [FieldOffset(0)]
        public int IntValue;
        [FieldOffset(0)]
        public float FloatValue;
    }

    public class PackageDefinition<T> : IBinaryPackageDefinition, IStringPackageDefinition, IJsonPackageDefinition
    {
        private static readonly byte[] ZeroByteArray = BitConverter.GetBytes((int)-1);

        protected BinaryPropertyDefinition<T>[] m_definitions = new BinaryPropertyDefinition<T>[0];
        protected List<ConstStringPropertyDefinitoin> m_constDefinitions = new List<ConstStringPropertyDefinitoin>();

        protected StringPropertyDefinition<T>[] m_stringDefinitions = new StringPropertyDefinition<T>[0];
        protected JsonPropertyDefinition<T>[] m_JsonDefinitions = new JsonPropertyDefinition<T>[0];

        public virtual IPackageDefinition Clone(Type type, object instance)
        {
            var baseType = typeof(InheritancePackageDefinition<,>);
            var packageDefinitionType = baseType.MakeGenericType(new[] { type, typeof(T) });
            var packageDefinitionInstance = Activator.CreateInstance(packageDefinitionType,
                                                                        m_definitions.ToList(),
                                                                        m_constDefinitions.Select(p => p.Clone(instance)).ToList(),
                                                                        m_stringDefinitions.ToList(),
                                                                        m_JsonDefinitions.ToList()) as IPackageDefinition;
            return packageDefinitionInstance;
        }

        IBinaryPackageDefinition IBinaryPackageDefinition.Clone(Type type, object instance)
        {
            return Clone(type, instance) as IBinaryPackageDefinition;
        }

        IStringPackageDefinition IStringPackageDefinition.Clone(Type type, object instance)
        {
            return Clone(type, instance) as IStringPackageDefinition;
        }

        IJsonPackageDefinition IJsonPackageDefinition.Clone(Type type, object instance)
        {
            return Clone(type, instance) as IJsonPackageDefinition;
        }

        public virtual void JsonPackData(object data, IStringRenderer stringRenderer, StringBuilder stringBuilder)
        {
            stringBuilder.Append("{");
            T instance = (T)data;
            var temp = m_JsonDefinitions;
            for (int i = 0; i < temp.Length; i++)
            {
                if (i != 0)
                {
                    stringBuilder.Append(",");
                }
                temp[i].Render(instance, stringRenderer, stringBuilder);
            }
            stringBuilder.Append("}");
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

        public virtual void Render(object data, IStringRenderer stringRenderer, StringBuilder stringBuilder)
        {
            stringBuilder.Append("{");
            T instance = (T)data;
            var temp = m_stringDefinitions;
            for (int i = 0; i < temp.Length; i++)
            {
                if (i != 0)
                {
                    stringBuilder.Append(" ");
                }
                temp[i].Render(instance, stringRenderer,stringBuilder);
            }
            stringBuilder.Append("}");
        }

        protected void AddDefinition(string property, SerilizeType serilizeType, Action<T, IBinaryPackager, ISerializer> serilizer)
        {
            ValidateDefinitionUniq(property);
            Array.Resize(ref m_definitions, m_definitions.Length + 1);
            m_definitions[m_definitions.Length - 1] = new BinaryPropertyDefinition<T>(property, serilizeType, serilizer);
        }

        protected void AddStringDefinition(string property, Action<T,IStringRenderer,StringBuilder> valueAppender)
        {
            Array.Resize(ref m_stringDefinitions, m_stringDefinitions.Length + 1);
            m_stringDefinitions[m_stringDefinitions.Length - 1] = new StringPropertyDefinition<T>(property, valueAppender);
        }

        protected void AddJsonDefinition(string property, Action<T, IStringRenderer, StringBuilder> valueAppender)
        {
            Array.Resize(ref m_JsonDefinitions, m_JsonDefinitions.Length + 1);
            m_JsonDefinitions[m_JsonDefinitions.Length - 1] = new JsonPropertyDefinition<T>(property, valueAppender);
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

        public PackageDefinition<T> Define(Expression<Func<T, object>> expression, Func<T, uint> dataExtractor)
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

        public PackageDefinition<T> Define(Expression<Func<T, object>> expression, Func<T, sbyte> dataExtractor)
        {
            return Define(ObjectHelper.GetMemberName(expression), dataExtractor);
        }

        public PackageDefinition<T> Define(Expression<Func<T, object>> expression, Func<T, short> dataExtractor)
        {
            return Define(ObjectHelper.GetMemberName(expression), dataExtractor);
        }

        public PackageDefinition<T> Define(Expression<Func<T, object>> expression, Func<T, ushort> dataExtractor)
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

        public PackageDefinition<T> Define(Expression<Func<T, object>> expression, Func<T, ulong> dataExtractor)
        {
            return Define(ObjectHelper.GetMemberName(expression), dataExtractor);
        }

        public PackageDefinition<T> Define(Expression<Func<T, object>> expression, Func<T, float> dataExtractor)
        {
            return Define(ObjectHelper.GetMemberName(expression), dataExtractor);
        }

        public PackageDefinition<T> Define(Expression<Func<T, object>> expression, Func<T, char> dataExtractor)
        {
            return Define(ObjectHelper.GetMemberName(expression), dataExtractor);
        }

        public PackageDefinition<T> Define(Expression<Func<T, object>> expression, Func<T, decimal> dataExtractor)
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
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, uint> dataExtractor)
        {
            AddDefinition(property, SerilizeType.UInt32, (arg, packager, serializer) => serializer.Serialize((int)dataExtractor.Invoke(arg)));
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            return this;
        }

        public PackageDefinition<T> DefineVariant(string property, Func<T, int> dataExtractor)
        {
            AddDefinition(property, SerilizeType.VariantUInt32, (arg, packager, serializer) => serializer.SerializeVariant(dataExtractor.Invoke(arg)));
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, double> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Double, (arg, packager, serializer) => serializer.Serialize(dataExtractor.Invoke(arg)));
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, bool> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Bool, (arg, packager, serializer) => serializer.Serialize(dataExtractor.Invoke(arg)));
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) => sb.Append(!dataExtractor.Invoke(arg1) ? "false":"true"));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, byte> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Byte, (arg, packager, serializer) => serializer.Serialize(dataExtractor.Invoke(arg)));
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, sbyte> dataExtractor)
        {
            AddDefinition(property, SerilizeType.SByte, (arg, packager, serializer) => serializer.Serialize(dataExtractor.Invoke(arg)));
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            return this;
        }

        public
#if UNSafe
 unsafe
#endif
            PackageDefinition<T> Define(string property, Func<T, float> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Float, (arg, packager, serializer) =>
            {
                float value = dataExtractor.Invoke(arg);
#if UNSafe
                serializer.Serialize(*((int*)&value));
#else
                IntFloat f;
                f.IntValue = 0; // This is requeired for compilation
                f.FloatValue = value;
                serializer.Serialize(f.IntValue);
#endif
            });
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, short> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Short, (arg, packager, serializer) => serializer.Serialize(dataExtractor.Invoke(arg)));
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, ushort> dataExtractor)
        {
            AddDefinition(property, SerilizeType.UShort, (arg, packager, serializer) => serializer.Serialize((short) dataExtractor.Invoke(arg)));
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, char> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Char, (arg, packager, serializer) => serializer.Serialize((short)dataExtractor.Invoke(arg)));
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, decimal> dataExtractor)
        {            
            AddDefinition(property, SerilizeType.Decimal, (arg, packager, serializer) =>
            {
                var value = dataExtractor.Invoke(arg);
                var bits = decimal.GetBits(value);
                serializer.Serialize(bits[0]);
                serializer.Serialize(bits[1]);
                serializer.Serialize(bits[2]);
                serializer.Serialize(bits[3]);
            });
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            return this;
        }

        private static readonly DateTime m_epocTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        public PackageDefinition<T> Define(string property, Func<T, DateTime> dataExtractor)
        {
            AddDefinition(property, SerilizeType.DateTime, (arg, packager, serializer) => serializer.Serialize(dataExtractor.Invoke(arg).Ticks));
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) =>
            {
                sb.Append("\"\\/Date(");
                sb.Append((dataExtractor.Invoke(arg1) - m_epocTime).TotalMilliseconds);
                sb.Append(")\\/\"");
            });
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, long> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Int64, (arg, packager, serializer) => serializer.Serialize(dataExtractor.Invoke(arg)));
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, ulong> dataExtractor)
        {
            AddDefinition(property, SerilizeType.UInt64, (arg, packager, serializer) => serializer.Serialize((long)dataExtractor.Invoke(arg)));
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, Guid> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Guid, (arg, packager, serializer) => serializer.Serialize(dataExtractor.Invoke(arg).ToByteArray(),0, 16));
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) =>
            {
                sb.Append("\"");
                sb.Append(dataExtractor.Invoke(arg1));
                sb.Append("\"");
            });
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, string> dataExtractor)
        {
            AddDefinition(property, SerilizeType.String, (arg, packager, serializer) => serializer.Serialize(dataExtractor.Invoke(arg)));
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) =>
            {
                var value = dataExtractor.Invoke(arg1);
                if (value == null)
                {
                    sb.Append("null");
                }
                else
                {
                    sb.Append("\"");
                    sb.Append(value);
                    sb.Append("\"");   
                }
            });

            return this;
        }

        public PackageDefinition<T> DefineCacheString(string property, Func<T, string> dataExtractor)
        {
            AddDefinition(property, SerilizeType.CacheString, (arg1, packager, serializer) => serializer.SerializeVariant(packager.GetCacheStringId(dataExtractor.Invoke(arg1))));
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) =>
            {
                var value = dataExtractor.Invoke(arg1);
                if (value == null)
                {
                    sb.Append("null");
                }
                else
                {
                    sb.Append("\"");
                    sb.Append(value);
                    sb.Append("\"");
                }
            });
            return this;
        }

        public PackageDefinition<T> DefineConstString(string property, Func<T, string> dataExtractor, string initialValue)
        {
            ValidateDefinitionUniq(property);
            m_constDefinitions.Add(new ConstStringPropertyDefinitoin(property,o => dataExtractor.Invoke((T)o),initialValue));
            
            AddStringDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) => sb.Append(dataExtractor.Invoke(arg1)));
            AddJsonDefinition(property, (arg1, renderer, sb) =>
            {
                var value = dataExtractor.Invoke(arg1);
                if (value == null)
                {
                    sb.Append("null");
                }
                else
                {
                    sb.Append("\"");
                    sb.Append(value);
                    sb.Append("\"");
                }
            });
            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, IEnumerable> dataExtractor)
        {
            // it is possiple to serilize an array more efficiently by reserving space for size of array in advance,
            // but that will require change in the serilizer interface
            // and i dont want to do it.
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


            AddStringDefinition(property, (arg, renderer,sb) => 
                                          {
                                              var enumerable = dataExtractor.Invoke(arg);
                                              if (enumerable == null)
                                              {
                                                  // Do nothing its empty
                                              }
                                              else
                                              {
                                                  sb.Append("[");
                                                  bool isFirst = true;
                                                  foreach (var data in enumerable)
                                                  {
                                                      if (!isFirst)
                                                      {
                                                          sb.Append(",");
                                                      }
                                                      isFirst = false;
                                                      renderer.Render(data,sb);
                                                  }
                                                  sb.Append("]");
                                              }
                                          });

            AddJsonDefinition(property, (arg, renderer, sb) =>
            {
                var enumerable = dataExtractor.Invoke(arg);
                if (enumerable == null)
                {
                    sb.Append("null");                    
                }
                else
                {
                    sb.Append("[");
                    bool isFirst = true;
                    foreach (var data in enumerable)
                    {
                        if (!isFirst)
                        {
                            sb.Append(",");
                        }
                        isFirst = false;
                        renderer.Render(data, sb);
                    }
                    sb.Append("]");
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

            AddStringDefinition(property, (arg, renderer, sb) =>
            {
                var enumerable = dataExtractor.Invoke(arg);
                if (enumerable == null)
                {
                    // Do nothing its empty
                }
                else
                {
                    sb.Append("[");
                    bool isFirst = true;
                    foreach (var data in enumerable)
                    {
                        if (!isFirst)
                        {
                            sb.Append(",");
                        }
                        isFirst = false;
                        
                        renderer.Render(data, sb);
                    }
                    sb.Append("]");
                }
            });

            AddJsonDefinition(property, (arg, renderer, sb) =>
            {
                var enumerable = dataExtractor.Invoke(arg);
                if (enumerable == null)
                {
                    // Do nothing its empty
                    sb.Append("null");
                }
                else
                {
                    sb.Append("[");
                    bool isFirst = true;
                    foreach (var data in enumerable)
                    {
                        if (!isFirst)
                        {
                            sb.Append(",");
                        }
                        isFirst = false;

                        renderer.Render(data, sb);
                    }
                    sb.Append("]");
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

            AddStringDefinition(property, (arg, renderer, sb) =>
            {
                var enumerable = dataExtractor.Invoke(arg);
                if (enumerable == null)
                {
                    // Do nothing its empty
                }
                else
                {
                    sb.Append("[");
                    bool isFirst = true;
                    foreach (var data in enumerable)
                    {
                        if (!isFirst)
                        {
                            sb.Append(",");
                        }
                        isFirst = false;
                        renderer.Render(data, sb);
                    }
                    sb.Append("]");
                }
            });

            return this;
        }

        public PackageDefinition<T> Define(string property, Func<T, object> dataExtractor)
        {
            AddDefinition(property, SerilizeType.Object, (arg, packager, serializer) => packager.Pack(dataExtractor.Invoke(arg), serializer));
            AddStringDefinition(property, (arg1, renderer, sb) =>
                                          {
                                              renderer.Render(dataExtractor.Invoke(arg1), sb);
                                          });
            AddJsonDefinition(property, (arg1, renderer, sb) =>
                                          {
                                              renderer.Render(dataExtractor.Invoke(arg1), sb);
                                          });

            return this;
        }
    }
}

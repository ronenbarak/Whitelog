using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Whitelog.Core.PackageDefinitions;

namespace Whitelog.Core.Binary.Deserilizer.Unpack
{
    public class CacheStringNotFoundException : Exception
    {
        public CacheStringNotFoundException(int id):base(string.Format("Cached string id={0} was not found",id))
        {
            Id = id;
        }
        public int  Id { get; private set; }
    }

    public abstract class UnpackageDefinition<T> : IUnpackageDefinition
    {
        private List<UnpackPropertyDefinition<T>> m_unpackageDefinitions = new List<UnpackPropertyDefinition<T>>();

        public UnpackageDefinition()
        {
        }

        public UnpackageDefinition(int definitionId)
        {
            DefinitionId = definitionId;
        }

        public virtual object Unpack(IDeserializer deserializer, IUnpacker unpacker)
        {
            T instance = CreateInstance();
            foreach (var unpackPropertyDefinition in m_unpackageDefinitions)
            {
                unpackPropertyDefinition.Unpack(instance, unpacker, deserializer);
            }
            return instance;
        }

        protected abstract T CreateInstance();

        public virtual int DefinitionId { get; set; }

        public virtual void AddDefintion(UnpackPropertyDefinition<T> unpackPropertyDefinition)
        {
            m_unpackageDefinitions.Add(unpackPropertyDefinition);
        }

        protected UnpackageDefinition<T> Define(string property, SerilizeType serilizeType, Action<T, IUnpacker, IDeserializer> unpacker)
        {
            AddDefintion(new UnpackPropertyDefinition<T>(property, serilizeType, unpacker));
            return this;
        }

        public UnpackageDefinition<T> DefineInt(string property, Action<T, int> setter)
        {
            Define(property,SerilizeType.Int32,(instance, unpacker, deserializer) => setter.Invoke(instance, deserializer.DeserializeInt()));
            return this;
        }

        public UnpackageDefinition<T> DefineVariantInt(string property, Action<T, int> setter)
        {
            Define(property, SerilizeType.VariantUInt32, (instance, unpacker, deserializer) => setter.Invoke(instance, deserializer.DeserializeVariantInt()));
            return this;
        }

        public UnpackageDefinition<T> DefineDouble(string property, Action<T, double> setter)
        {
            Define(property, SerilizeType.Double, (instance, unpacker, deserializer) => setter.Invoke(instance, deserializer.DeserializeDouble()));
            return this;
        }

        public UnpackageDefinition<T> DefineBool(string property, Action<T, bool> setter)
        {
            Define(property, SerilizeType.Bool, (instance, unpacker, deserializer) => setter.Invoke(instance, deserializer.DeserializeBool()));
            return this;
        }

        public UnpackageDefinition<T> DefineDateTime(string property, Action<T, DateTime> setter)
        {
            Define(property, SerilizeType.DateTime, (instance, unpacker, deserializer) => setter.Invoke(instance, new DateTime(deserializer.DeserializeLong())));
            return this;
        }

        public UnpackageDefinition<T> DefineLong(string property, Action<T, long> setter)
        {
            Define(property, SerilizeType.Int64, (instance, unpacker, deserializer) => setter.Invoke(instance, deserializer.DeserializeLong()));
            return this;
        }

        public UnpackageDefinition<T> DefineByte(string property, Action<T, byte> setter)
        {
            Define(property, SerilizeType.Byte, (instance, unpacker, deserializer) => setter.Invoke(instance, deserializer.DeserializeByte()));
            return this;
        }

        public UnpackageDefinition<T> DefineGuid(string property, Action<T, Guid> setter)
        {
            Define(property, SerilizeType.Byte, (instance, unpacker, deserializer) => setter.Invoke(instance, new Guid(deserializer.DeserializeByteArray(16))));
            return this;
        }

        public UnpackageDefinition<T> DefineString(string property, Action<T, string> setter)
        {
            Define(property, SerilizeType.String, (instance, unpacker, deserializer) => setter.Invoke(instance, deserializer.DeserializeString()));
            return this;
        }

        public UnpackageDefinition<T> DefineCacheString(string property, Action<T, string> setter)
        {
            Define(property, SerilizeType.String, (instance, unpacker, deserializer) =>
            {
                var definitionId = deserializer.DeserializeVariantInt();
                string value;
                if (unpacker.TryGetCachedString(definitionId, out value))
                {
                    setter.Invoke(instance, value);
                }
                else
                {
                    throw new CacheStringNotFoundException(definitionId);
                }
            });
            return this;
        }

        public UnpackageDefinition<T> DefineEnumerable(string property, Action<T, IEnumerable> setter)
        {
            Define(property, SerilizeType.Enumerable, (instance, unpacker, deserializer) =>
                                                        {
                                                            int legnthValue = deserializer.DeserializeVariantInt();
                                                            if (legnthValue == 0)
                                                            {
                                                                setter.Invoke(instance, null);
                                                            }
                                                            else
                                                            {
                                                                if (legnthValue == 1)
                                                                {
                                                                    setter.Invoke(instance, Enumerable.Empty<object>());
                                                                }
                                                                else
                                                                {
                                                                    object[] lstData = new object[legnthValue -1];
                                                                    for (int i = 0; i < lstData.Length; i++)
                                                                    {
                                                                        lstData[i] = unpacker.Unpack<object>(deserializer);
                                                                    }
                                                                    setter.Invoke(instance, lstData);
                                                                }
                                                            }
                                                        });
            return this;
        }

        public UnpackageDefinition<T> DefineObject(string property, Action<T, object> setter)
        {
            Define(property, SerilizeType.Object, (instance, unpacker, deserializer) => setter.Invoke(instance, unpacker.Unpack<object>(deserializer)));
            return this;
        }

        public IEnumerable<IPropertyDefinition> GetPropertyDefinition()
        {
            return m_unpackageDefinitions;
        }
    }
}
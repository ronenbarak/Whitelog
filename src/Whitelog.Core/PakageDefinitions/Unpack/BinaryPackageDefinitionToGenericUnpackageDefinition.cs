using System;
using System.Collections;
using Whitelog.Barak.Common.Events;
using Whitelog.Interface;
using Whitelog.Core.Generic;

namespace Whitelog.Core.PakageDefinitions.Unpack
{
    public class BinaryPackageDefinitionToGenericUnpackageDefinition : UnpackageDefinition<GenericUnpackageDefinition>
    {
        public event EventHandler<EventArgs<GenericUnpackageDefinition>> PackageDefinitionRegistred;

        public override int DefinitionId
        {
            get { return (int)KnownPackageDefinition.BinaryPackagedDefinitionDefinition; }
            set { }
        }

        public override bool Unpack(IDeserializer deserializer, IUnpacker unpacker, out object data)
        {
            var isData = base.Unpack(deserializer, unpacker, out data);
            if (isData)
            {
                this.RaiseEvent(PackageDefinitionRegistred, data as GenericUnpackageDefinition);
            }
            return isData;
        }

        public BinaryPackageDefinitionToGenericUnpackageDefinition()
        {
            DefineVariantInt("DefinitionId", (definition, i) => definition.DefinitionId = i);
            DefineVariantInt("BaseDefinitionId", (definition, i) => {/* we dont use the BaseDefinitionId yet*/ });
            DefineString("FullName", (definition, s) => definition.Type = new GenericComponentType(s,definition.DefinitionId));
            DefineEnumerable("PropertyDefinitions", (definition, enumerable) =>
                                                       {
                                                           if (enumerable != null)
                                                           {
                                                               int index = 0;
                                                               foreach (GenericPackageProperty packageProperty in enumerable)
                                                               {
                                                                   string propertyName = packageProperty.Property;
                                                                   GenericPropertyInfo propertyInfo = new GenericPropertyInfo(definition.Type, packageProperty.Property, index);
                                                                   definition.AddProperty(propertyInfo);
                                                                   switch (packageProperty.SerilizeType)
                                                                   {
                                                                       case SerilizeType.Bool:
                                                                           propertyInfo.Type = typeof(bool);
                                                                           definition.DefineBool(propertyName, (instance, value) => propertyInfo.SetValue(instance,value));
                                                                           break;
                                                                       case SerilizeType.Byte:
                                                                           propertyInfo.Type = typeof(byte);
                                                                           definition.DefineByte(propertyName, (instance, value) => propertyInfo.SetValue(instance,value));
                                                                           break;
                                                                       case SerilizeType.DateTime:
                                                                           propertyInfo.Type = typeof(DateTime);
                                                                           definition.DefineDateTime(propertyName, (instance, value) => propertyInfo.SetValue(instance,value));
                                                                           break;
                                                                       case SerilizeType.Double:
                                                                           propertyInfo.Type = typeof(double);
                                                                           definition.DefineDouble(propertyName, (instance, value) => propertyInfo.SetValue(instance,value));
                                                                           break;
                                                                       case SerilizeType.Enumerable:
                                                                           propertyInfo.Type = typeof(Array);
                                                                           definition.DefineEnumerable(propertyName, (instance, value) => propertyInfo.SetValue(instance,value));
                                                                           break;
                                                                       case SerilizeType.Guid:
                                                                           propertyInfo.Type = typeof(Guid);
                                                                           definition.DefineGuid(propertyName, (instance, value) => propertyInfo.SetValue(instance,value));
                                                                           break;
                                                                       case SerilizeType.Int32:
                                                                           propertyInfo.Type = typeof(int);
                                                                           definition.DefineInt(propertyName, (instance, value) => propertyInfo.SetValue(instance,value));
                                                                           break;
                                                                       case SerilizeType.Int64:
                                                                           propertyInfo.Type = typeof(long);
                                                                           definition.DefineLong(propertyName, (instance, value) => propertyInfo.SetValue(instance,value));
                                                                           break;
                                                                       case SerilizeType.Object:
                                                                           propertyInfo.Type = typeof(object);
                                                                           definition.DefineObject(propertyName, (instance, value) => propertyInfo.SetValue(instance,value));
                                                                           break;
                                                                       case SerilizeType.String:
                                                                           propertyInfo.Type = typeof(string);
                                                                           definition.DefineString(propertyName, (instance, value) => propertyInfo.SetValue(instance,value));
                                                                           break;
                                                                       case SerilizeType.VariantUInt32:
                                                                           propertyInfo.Type = typeof(int);
                                                                           definition.DefineVariantInt(propertyName, (instance, value) => propertyInfo.SetValue(instance,value));
                                                                           break;
                                                                       case SerilizeType.CacheString:
                                                                           propertyInfo.Type = typeof(string);
                                                                           definition.DefineCacheString(propertyName, (instance, value) => propertyInfo.SetValue(instance,value));
                                                                           break;
                                                                       case SerilizeType.ConstString:
                                                                           propertyInfo.Type = typeof(string);
                                                                           GenericPackageProperty property = packageProperty;
                                                                           definition.AddDefintion(new UnpackPropertyDefinition<ILogEntryData>(propertyName, SerilizeType.String, (data, unpacker, arg3) => propertyInfo.SetValue(data, property.Value)));
                                                                           break;
                                                                       default:
                                                                           throw new UnkwonSerilizeTypeException(packageProperty.SerilizeType);
                                                                   }

                                                                   index++;
                                                               }
                                                           }
                                                       });
        }

        protected override GenericUnpackageDefinition CreateInstance()
        {
            return new GenericUnpackageDefinition();
        }
    }
}
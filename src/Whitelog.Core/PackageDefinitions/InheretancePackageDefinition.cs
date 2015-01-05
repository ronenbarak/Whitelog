using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Whitelog.Core.String;

namespace Whitelog.Core.PackageDefinitions
{
    public class InheritancePackageDefinition<T,TBase> : PackageDefinition<T> where T:TBase
    {
        class PropertyEqualityComparer : IEqualityComparer<PropertyInfo>
        {
            public bool Equals(PropertyInfo x, PropertyInfo y)
            {
                return x.Name == y.Name;
            }

            public int GetHashCode(PropertyInfo obj)
            {
                return obj.Name.GetHashCode();
            }
        }

        public InheritancePackageDefinition()
            : this(new List<BinaryPropertyDefinition<TBase>>(), new List<ConstStringPropertyDefinitoin>(), new List<StringPropertyDefinition<TBase>>(), new List<JsonPropertyDefinition<TBase>>())
        {            
        }

        // This class is being called using reflection
        public InheritancePackageDefinition(List<BinaryPropertyDefinition<TBase>> propertyDefinitions,
                                            List<ConstStringPropertyDefinitoin> constStringPropertyDefinitoins,
                                            List<StringPropertyDefinition<TBase>> stringDefinitions,
                                            List<JsonPropertyDefinition<TBase>> jsonDefinitions)
        {
            // We use the Activator.CreateInstance to convert from Action<TBase> tp Action<T> it will work in run time but it doesnt compile...
            m_definitions = propertyDefinitions.Select(p=>Activator.CreateInstance(typeof(BinaryPropertyDefinition<T>),new object[] {p.Name,p.SerilizeType,p.Serilize}) as BinaryPropertyDefinition<T>).ToArray();
            m_stringDefinitions = stringDefinitions.Select(p => Activator.CreateInstance(typeof(StringPropertyDefinition<T>), new object[] { p.Name, p.ValueExtractor }) as StringPropertyDefinition<T>).ToArray();
            m_JsonDefinitions = jsonDefinitions.Select(p => Activator.CreateInstance(typeof(JsonPropertyDefinition<T>), new object[] { p.Name, p.ValueExtractor }) as JsonPropertyDefinition<T>).ToArray();
            m_constDefinitions = constStringPropertyDefinitoins;

            var type = typeof(T);

            var newProperties = type.GetProperties().Except(typeof(TBase).GetProperties(), new PropertyEqualityComparer()).ToList();

            var thisType = this.GetType();
            var methods = thisType.GetMethods();
            var objectDefine = methods.First(p => p.Name == "Define" &&
                                                  p.GetParameters()[0].ParameterType == typeof(string) &&
                                                  p.GetParameters()[1].ParameterType.IsGenericType &&
                                                  p.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>) &&
                                                  p.GetParameters()[1].ParameterType.GetGenericArguments()[1] == typeof(object));

            foreach (var propertyInfo in newProperties)
            {
                if (propertyInfo.CanRead)
                {
                    var getMethod = propertyInfo.GetGetMethod();
                    if (getMethod != null)
                    {
                        var genericTypeForFunc = typeof (Func<,>);

                        var delegateType =  genericTypeForFunc.MakeGenericType(new[] {type, propertyInfo.PropertyType});
                        var parmExtractor = Delegate.CreateDelegate(delegateType, getMethod);

                        if (typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            DefineCollection(propertyInfo.Name, (Func<T, ICollection>)parmExtractor);
                        }
                        else if ((typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType)) && propertyInfo.PropertyType != (typeof(System.String)))
                        {
                            Define(propertyInfo.Name, (Func<T, IEnumerable>)parmExtractor);
                        }
                        else
                        {
                            var foundDirectSerilizetion = methods.FirstOrDefault(p => p.Name == "Define" &&
                                                          p.GetParameters()[0].ParameterType == typeof(string) &&
                                                          p.GetParameters()[1].ParameterType.IsGenericType &&
                                                          p.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>) &&
                                                          p.GetParameters()[1].ParameterType.GetGenericArguments()[1] == propertyInfo.PropertyType); ;
                            if (foundDirectSerilizetion != null)
                            {
                                foundDirectSerilizetion.Invoke(this, new object[] { propertyInfo.Name, parmExtractor });
                            }
                            else
                            {
                                objectDefine.Invoke(this, new object[] { propertyInfo.Name, parmExtractor });
                            }
                        }
                    }
                }
            }


        }
    }
}
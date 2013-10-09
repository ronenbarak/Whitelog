using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using Whitelog.Core.Binary;

namespace Whitelog.Core.PackageDefinitions
{
    public static class AllPropertiesPackageDefinitionHelper
    {
        public static IPackageDefinition CreateInstatnce(Type type)
        {
            var baseType = typeof(AllPropertiesPackageDefinition<>);
            var packageDefinitionType = baseType.MakeGenericType(new[] { type });
            var packageDefinitionInstance = Activator.CreateInstance(packageDefinitionType) as IBinaryPackageDefinition;
            return packageDefinitionInstance;
        }
    }

    public class AllPropertiesPackageDefinition<T> : PackageDefinition<T>
    {
        public AllPropertiesPackageDefinition()
        {
            var type = typeof (T);

            var properties = type.GetProperties();

            var thisType = this.GetType();
            var methods = thisType.GetMethods();
            var objectDefine = methods.First(p => p.Name == "Define" && 
                                                  p.GetParameters()[0].ParameterType == typeof(string) &&
                                                  p.GetParameters()[1].ParameterType.IsGenericType &&
                                                  p.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>) &&
                                                  p.GetParameters()[1].ParameterType.GetGenericArguments()[1] == typeof(object));
            foreach (var propertyInfo in properties)
            {
                var functorParam = Expression.Parameter(type);
                var lambda = Expression.Lambda(Expression.Property(functorParam, propertyInfo), functorParam);
                var parmExtractor = lambda.Compile();

                if (typeof (ICollection).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    DefineCollection(propertyInfo.Name, (Func<T, ICollection>) parmExtractor);
                }
                else if ((typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType)) && propertyInfo.PropertyType != (typeof(System.String)))
                {
                    Define(propertyInfo.Name, (Func<T, IEnumerable>) parmExtractor);
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
                        objectDefine.Invoke(this, new object[] { propertyInfo.Name, parmExtractor});
                    }
                }
            }


        }
    }
}
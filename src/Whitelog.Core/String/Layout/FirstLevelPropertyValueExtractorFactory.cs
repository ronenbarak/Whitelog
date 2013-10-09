using System;
using System.Linq.Expressions;
using Whitelog.Core.String.Layout.StringLayoutFactory;
using Whitelog.Core.String.Layout.StringLayoutWriters;

namespace Whitelog.Core.String.Layout
{
    public class FirstLevelPropertyValueExtractorFactory : IMessageParamaterHanlderFactory
    {
        public IStringLayoutWriter Create(string property, Type type)
        {
            if (property == "*")
            {
                return new ObjectStringLayoutFactory.ObjectStringLayoutWriter();
            }

            if (type == null)
            {
                return new ConstStringLayoutWriter("{" + property + "}");
            }

            var propertyInfo = type.GetProperty(property);
            if (propertyInfo == null)
            {
                return new ConstStringLayoutWriter("{" + property + "}");
            }

            var functorParam = Expression.Parameter(type);
            var lambda = Expression.Lambda(Expression.Property(functorParam, propertyInfo), functorParam);
            Delegate parmExtractor = lambda.Compile();

            if (propertyInfo.PropertyType.IsPrimitive || propertyInfo.PropertyType == typeof (string))
            {
                return new DelegatePrimitivePropertyStringLayoutWriter(property, parmExtractor);
            }
            else
            {
                return new DelegatePropertyStringLayoutWriter(property, parmExtractor);   
            }
        }

    }
}
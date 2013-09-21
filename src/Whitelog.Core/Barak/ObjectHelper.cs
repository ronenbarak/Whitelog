using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;


namespace Whitelog.Barak.Common.ExtensionMethods
{
    public static class ObjectHelper
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Int2Bytes
        {
            public Int2Bytes(Int32 value) { b0 = b1 = b2 = b3 = 0; i32 = value; }
            [FieldOffset(0)]
            public Int32 i32;
            [FieldOffset(0)]
            public byte b0;
            [FieldOffset(1)]
            public byte b1;
            [FieldOffset(2)]
            public byte b2;
            [FieldOffset(3)]
            public byte b3;
        }

        public static void AddToHash(ref int hash, params int[] add)
        {
            foreach (var currAdd in add)
            {
                AddToHash(ref hash, currAdd);
            }
        }

        public static int GetHash<T>(params T[] items) where T : struct
        {
            int hash = 0;
            foreach (var currAdd in items)
            {
                AddToHash(ref hash, currAdd.GetHashCode());
            }
            return hash;
        }

        public static void AddToHash(ref int hash, int add)
        {
            Int2Bytes bytes = new Int2Bytes(add);

            hash += bytes.b0;
            hash += hash << 10;
            hash ^= hash >> 6;

            hash += bytes.b1;
            hash += hash << 10;
            hash ^= hash >> 6;

            hash += bytes.b2;
            hash += hash << 10;
            hash ^= hash >> 6;

            hash += bytes.b3;
            hash += hash << 10;
            hash ^= hash >> 6;
        }

        public static TOut NullGurd<TIn, TOut>(this TIn obj, Func<TIn, TOut> action)
        {
            if (obj == null)
            {
                return default(TOut);
            }
            else
            {
                return action.Invoke(obj);
            }
        }

        public static TOut NullGurd<TIn, TOut>(this TIn obj, Func<TIn, TOut> action, Func<TOut> defaultValue)
        {
            if (obj == null)
            {
                return defaultValue.Invoke();
            }
            else
            {
                return action.Invoke(obj);
            }
        }

        public static TOut NullGurd<TIn, TOut>(this TIn obj, Func<TIn, TOut> action, TOut defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            else
            {
                return action.Invoke(obj);
            }
        }

        private static bool IsConversion(ExpressionType expressionType)
        {
            if (expressionType != ExpressionType.Convert)
                return expressionType == ExpressionType.ConvertChecked;
            else
                return true;
        }

        private static string ClassMember(System.Linq.Expressions.Expression expression)
        {
            if (expression.NodeType == ExpressionType.MemberAccess)
                return FindMemberExpression(expression) + ".class";
            else
                return "class";
        }

        private static bool IsNullableOfT(Type type)
        {
            if (type.IsGenericType)
                return type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
            else
                return false;
        }

        private static string FindMemberExpression(System.Linq.Expressions.Expression expression)
        {
            if (expression is MemberExpression)
            {
                MemberExpression memberExpression = (MemberExpression)expression;
                if (memberExpression.Expression.NodeType == ExpressionType.MemberAccess || memberExpression.Expression.NodeType == ExpressionType.Call)
                {
                    if (IsNullableOfT(memberExpression.Member.DeclaringType) && memberExpression.Member.Name == "Value")
                        return FindMemberExpression(memberExpression.Expression);
                    else
                        return FindMemberExpression(memberExpression.Expression) + "." + memberExpression.Member.Name;
                }
                else
                {
                    if (!IsConversion(memberExpression.Expression.NodeType))
                        return memberExpression.Member.Name;
                    return (FindMemberExpression(memberExpression.Expression) + "." + memberExpression.Member.Name).TrimStart(new char[1]
          {
            '.'
          });
                }
            }
            else if (expression is UnaryExpression)
            {
                UnaryExpression unaryExpression = (UnaryExpression)expression;
                if (!IsConversion(unaryExpression.NodeType))
                    throw new Exception("Cannot interpret member from " + expression.ToString());
                else
                    return FindMemberExpression(unaryExpression.Operand);
            }
            else if (expression is MethodCallExpression)
            {
                MethodCallExpression methodCallExpression = (MethodCallExpression)expression;
                if (methodCallExpression.Method.Name == "GetType")
                    return ClassMember(methodCallExpression.Object);
                if (methodCallExpression.Method.Name == "get_Item" || methodCallExpression.Method.Name == "First")
                    return FindMemberExpression(methodCallExpression.Object);
                else
                    throw new Exception("Unrecognised method call in expression " + expression.ToString());
            }
            else if (expression is ParameterExpression)
                return "";
            else
                throw new Exception("Could not determine member from " + expression.ToString());
        }

        public static string GetFullMemberPath(LambdaExpression expression)
        {
            return FindMemberExpression(expression.Body);
        }

        public static string GetMemberName(LambdaExpression expresstion)
        {
            if ((expresstion.Body is MemberExpression))
            {
                return (expresstion.Body as MemberExpression).Member.Name;
            }
            else if (expresstion.Body is UnaryExpression)
            {
                if ((expresstion.Body as UnaryExpression).Operand is MemberExpression)
                {
                    return ((expresstion.Body as UnaryExpression).Operand as MemberExpression).Member.Name;
                }
            }
            else if (expresstion.Body is System.Linq.Expressions.MethodCallExpression)
            {
                return (expresstion.Body as System.Linq.Expressions.MethodCallExpression).Method.Name;
            }
            return null;
        }

        public static PropertyInfo GetProperty<T>(Expression<Func<T, object>> expresstion)
        {
            return GetProperty((LambdaExpression)expresstion);
        }

        public static PropertyInfo GetProperty(LambdaExpression expresstion)
        {
            if (expresstion.Body is UnaryExpression)
            {
                if ((expresstion.Body as UnaryExpression).Operand is MemberExpression)
                {
                    return ((expresstion.Body as UnaryExpression).Operand as MemberExpression).Member as PropertyInfo;
                }
            }
            if ((expresstion.Body is MemberExpression))
            {
                return (expresstion.Body as MemberExpression).Member as PropertyInfo;
            }
            return null;
        }
    }
}

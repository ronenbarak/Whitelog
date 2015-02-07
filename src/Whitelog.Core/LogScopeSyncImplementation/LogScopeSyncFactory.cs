using System;
using System.Collections.Generic;
using System.Linq;

namespace Whitelog.Core.LogScopeSyncImplementation
{
    public static class LogScopeSyncFactory
    {
        private static Queue<Type> _types;

        class A1 { }
        class A2 { }
        class A3 { }
        class A4 { }
        class A5 { }
        class A6 { }
        class A7 { }
        class A8 { }
        class A9 { }
        class A10 { }
        static LogScopeSyncFactory()
        {
            // We use generic types to create a diffrent ThreadStatic every time a new insatnce of this class is created
            // This is a major hack and will be needed to be fixed some how....
            _types = new Queue<Type>();
            _types.Enqueue(typeof(A1));
            _types.Enqueue(typeof(A2));
            _types.Enqueue(typeof(A3));
            _types.Enqueue(typeof(A4));
            _types.Enqueue(typeof(A5));
            _types.Enqueue(typeof(A6));
            _types.Enqueue(typeof(A7));
            _types.Enqueue(typeof(A8));
            _types.Enqueue(typeof(A9));
            _types.Enqueue(typeof(A10));
        }

        public static ILogScopeSyncImplementation Create()
        {
            lock (_types)
            {
                var currType = _types.Dequeue();
                var logScopeSyncType = typeof (LogScopeSync<>).MakeGenericType(currType);
                return (ILogScopeSyncImplementation) Activator.CreateInstance(logScopeSyncType);
            }
        }

        public static void Free(ILogScopeSyncImplementation implementation)
        {
            lock (_types)
            {
                if (implementation.GetType().IsGenericType &&
                    implementation.GetType().GetGenericTypeDefinition() == typeof (LogScopeSync<>))
                {
                    var type = implementation.GetType().GetGenericArguments()[0];
                    if (!_types.Contains(type))
                    {
                        _types.Enqueue(type);
                    }
                }
            }
        }
    }
}
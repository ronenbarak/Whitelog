using System;
using System.Collections.Generic;
using System.Linq;

namespace Whitelog.Core.LogScopeSyncImplementation
{
    public static class LogScopeSyncFactory
    {
        private static Queue<Type> _types;

        static LogScopeSyncFactory()
        {
            // We use generic types to create a diffrent ThreadStatic every time a new insatnce of this class is created
            // This is a major hack and will be needed to be fixed some how....
            _types = new Queue<Type>(typeof (LogScopeSyncFactory).Assembly.GetTypes().Where(p=>!p.ContainsGenericParameters));
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
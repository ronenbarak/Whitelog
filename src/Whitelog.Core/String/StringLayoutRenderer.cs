using System;
using System.Text;
using Whitelog.Barak.Common.DataStructures.Dictionary;
using Whitelog.Barak.Common.Events;
using Whitelog.Core.PackageDefinitions;
using Whitelog.Core.String.Layout;
using Whitelog.Core.String.StringBuffer;
using Whitelog.Interface;
using Whitelog.Interface.LogTitles;


namespace Whitelog.Core.String
{
    public class JsonSerilizer : IStringRenderer
    {
        public event EventHandler<EventArgs<IJsonPackageDefinition>> PackageRegistered;
        protected readonly object m_definitionSyncObjectLock = new object();
        protected readonly ReadSafeDictionary<Type, IJsonPackageDefinition> m_PackageDefinitions = new ReadSafeDictionary<Type, IJsonPackageDefinition>(new TypeComparer());

        protected IJsonPackageDefinition GetClosestPackageDefinition(Type currDataType)
        {
            int distance = int.MaxValue;
            IJsonPackageDefinition closestDefinition = null;
            foreach (var logPackageDefinition in m_PackageDefinitions)
            {
                if (logPackageDefinition.Key.IsAssignableFrom(currDataType))
                {
                    var currType = currDataType;
                    int currentDistance = 0;
                    while (currType != null && logPackageDefinition.Key.IsAssignableFrom(currType))
                    {
                        currentDistance++;
                        currType = currType.BaseType;
                    }

                    if (currentDistance < distance)
                    {
                        distance = currentDistance;
                        closestDefinition = logPackageDefinition.Value;
                    }
                }
            }
            return closestDefinition;
        }

        public void Render(object data, StringBuilder stringBuilder)
        {
            if (data == null)
            {
                stringBuilder.Append("null");
            }
            else
            {
                Type type = data.GetType();
                IJsonPackageDefinition packageDefinition;
                if (!m_PackageDefinitions.TryGetValue(type, out packageDefinition))
                {
                    lock (m_PackageDefinitions)
                    {
                        if (!m_PackageDefinitions.TryGetValue(type, out packageDefinition))
                        {
                            packageDefinition = GetClosestPackageDefinition(type);
                            packageDefinition = packageDefinition.Clone(type, data);
                            m_PackageDefinitions.Add(type, packageDefinition);
                        }
                    }
                }

                if (packageDefinition == null)
                {
                    throw new NoPackageFoundForTypeExceptin(type);
                }

                packageDefinition.JsonPackData(data, this, stringBuilder);
            }
        }

        public void RegisterDefinition(IJsonPackageDefinition packageDefinition)
        {
            lock (m_definitionSyncObjectLock)
            {
                m_PackageDefinitions[packageDefinition.GetTypeDefinition()] = packageDefinition;
                this.RaiseEvent(PackageRegistered, packageDefinition);
            }
        }
    }

    public class StringLayoutRenderer : IStringRenderer
    {

        class TitleTypeTuple : IEquatable<TitleTypeTuple>
        {
            public TitleTypeTuple Clone()
            {
                return new TitleTypeTuple()
                {
                    Layout =  Layout,
                    Type =  Type,
                };
            }
            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Layout != null ? Layout.GetHashCode() : 0)*397) ^ (Type != null ? Type.GetHashCode() : 0);
                }
            }

            public string Layout { get; set; }
            public Type Type { get; set; }

            public override bool Equals(object obj)
            {
                return Equals((TitleTypeTuple) obj);
            }

            public bool Equals(TitleTypeTuple other)
            {
                return string.Equals(Layout, other.Layout) && object.Equals(Type, other.Type);
            }
        }

        [ThreadStatic]
        private static TitleTypeTuple m_titleTypeTupleCache;

        public event EventHandler<EventArgs<IStringPackageDefinition>> PackageRegistered;

        protected readonly object m_definitionSyncObjectLock = new object();
        protected readonly ReadSafeDictionary<Type, IStringPackageDefinition> m_PackageDefinitions = new ReadSafeDictionary<Type, IStringPackageDefinition>(new TypeComparer());
        ReadSafeDictionary<TitleTypeTuple, IStringLayoutWriter> m_cacheMessageWriter = new ReadSafeDictionary<TitleTypeTuple, IStringLayoutWriter>();
        private StringLayoutParser m_stringLayoutParser;
        private IStringBuffer m_stringBuffer;

        public StringLayoutRenderer(string layout)
        {
            m_stringLayoutParser = new StringLayoutParser(new FirstLevelPropertyValueExtractorFactory(), layout);
            RegisterDefinition(new ObjectPackageDefinition());
        }

        public void RegisterLayoutExtensions(IStringLayoutFactory layoutFactory)
        {
            m_stringLayoutParser.Register(layoutFactory);
        }

        protected IStringPackageDefinition GetClosestPackageDefinition(Type currDataType)
        {
            int distance = int.MaxValue;
            IStringPackageDefinition closestDefinition = null;
            foreach (var logPackageDefinition in m_PackageDefinitions)
            {
                if (logPackageDefinition.Key.IsAssignableFrom(currDataType))
                {
                    var currType = currDataType;
                    int currentDistance = 0;
                    while (currType != null && logPackageDefinition.Key.IsAssignableFrom(currType))
                    {
                        currentDistance++;
                        currType = currType.BaseType;
                    }

                    if (currentDistance < distance)
                    {
                        distance = currentDistance;
                        closestDefinition = logPackageDefinition.Value;
                    }
                }
            }
            return closestDefinition;
        }

        public void Render(LogEntry data, StringBuilder stringBuilder)
        {
            Type type = null;
            if (data.Paramaeter != null)
            {
                type = data.Paramaeter.GetType();
            }
            string titleMessage = null;
            var messageLogTitle = data.Title as IMessageLogTitle;
            if (messageLogTitle != null)
            {
                titleMessage = messageLogTitle.Message;
            }

            if (m_titleTypeTupleCache == null)
            {
                m_titleTypeTupleCache = new TitleTypeTuple();
            }

            m_titleTypeTupleCache.Layout = titleMessage;
            m_titleTypeTupleCache.Type = type;
            IStringLayoutWriter stringLayoutWriter;
            if (!m_cacheMessageWriter.TryGetValue(m_titleTypeTupleCache, out stringLayoutWriter))
            {
                lock (m_definitionSyncObjectLock)
                {
                    if (!m_cacheMessageWriter.TryGetValue(m_titleTypeTupleCache, out stringLayoutWriter))
                    {
                        stringLayoutWriter = m_stringLayoutParser.Parse(titleMessage, type);
                        m_cacheMessageWriter.Add(m_titleTypeTupleCache.Clone(), stringLayoutWriter);
                    }
                }
            }

            stringLayoutWriter.Render(stringBuilder, this,data);
        }

        public void Render(object data, StringBuilder stringBuilder)
        {
            if (data == null)
            {
                // Do nothing its ok
            }
            else
            {
                Type type = data.GetType();
                IStringPackageDefinition packageDefinition;
                if (!m_PackageDefinitions.TryGetValue(type, out packageDefinition))
                {
                    lock (m_PackageDefinitions)
                    {
                        if (!m_PackageDefinitions.TryGetValue(type, out packageDefinition))
                        {
                            packageDefinition = GetClosestPackageDefinition(type);
                            packageDefinition = packageDefinition.Clone(type, data);
                            m_PackageDefinitions.Add(type, packageDefinition);
                        }
                    }
                }

                if (packageDefinition == null)
                {
                    throw new NoPackageFoundForTypeExceptin(type);
                }

                packageDefinition.Render(data, this,stringBuilder);
            }
        }

        public void RegisterDefinition(IStringPackageDefinition packageDefinition)
        {
            lock (m_definitionSyncObjectLock)
            {
                m_PackageDefinitions[packageDefinition.GetTypeDefinition()] = packageDefinition;
                this.RaiseEvent(PackageRegistered, packageDefinition);
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Whitelog.Barak.Common.DataStructures.Dictionary;
using Whitelog.Barak.Common.Events;
using Whitelog.Core.PackageDefinitions;
using Whitelog.Core.String.Layout;
using Whitelog.Core.String.Layout.StringLayoutFactory;
using Whitelog.Core.String.StringBuffer;
using Whitelog.Interface;
using Whitelog.Interface.LogTitles;


namespace Whitelog.Core.String
{
    public class StringLayoutRenderer : IStringRenderer
    {
        public event EventHandler<EventArgs<IStringPackageDefinition>> PackageRegistered;

        protected readonly object m_definitionSyncObjectLock = new object();
        protected readonly ReadSafeDictionary<Type, IStringPackageDefinition> m_PackageDefinitions = new ReadSafeDictionary<Type, IStringPackageDefinition>(new TypeComparer());
        ReadSafeDictionary<Tuple<string,Type>, IStringLayoutWriter> m_cacheMessageWriter = new ReadSafeDictionary<Tuple<string, Type>, IStringLayoutWriter>();
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

            IStringLayoutWriter stringLayoutWriter;
            if (!m_cacheMessageWriter.TryGetValue(new Tuple<string, Type>(titleMessage, type), out stringLayoutWriter))
            {
                lock (m_definitionSyncObjectLock)
                {
                    if (!m_cacheMessageWriter.TryGetValue(new Tuple<string, Type>(titleMessage, type),out stringLayoutWriter))
                    {
                        stringLayoutWriter = m_stringLayoutParser.Parse(titleMessage, type);
                        m_cacheMessageWriter.Add(new Tuple<string, Type>(titleMessage, type),stringLayoutWriter);
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
                            if (AnonymousTypesHelper.IsAnonymousType(type))
                            {
                                   packageDefinition = AllPropertiesPackageDefinitionHelper.CreateInstatnce(type) as IStringPackageDefinition;
                            }
                            else
                            {
                                packageDefinition = GetClosestPackageDefinition(type);   
                            }
                        }
                    }
                }

                if (packageDefinition == null)
                {
                    throw new NoPackageFoundForTypeExceptin(type);
                }
                stringBuilder.Append("{");
                packageDefinition.Render(data, this,stringBuilder);
                stringBuilder.Append("}");
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Whitelog.Barak.Common.Events;
using Whitelog.Interface;
using Whitelog.Core.PakageDefinitions.Unpack;

namespace Whitelog.Core
{
    public static class UnpackerHelper
    {
        public static T Unpack<T>(this IUnpacker unpacker,IDeserializer deserializer) where T:class 
        {
            return unpacker.Unpack(deserializer) as T;
        }
    }

    public interface IUnpacker
    {
        object Unpack(IDeserializer deserializer);
        bool TryGetCachedString(int id, out string value);
        bool TryGetPackageType(Type type, out IUnpackageDefinition packageDefinition);
    }

    public class ResolvePakageDefinition
    {
        public ResolvePakageDefinition (int pakageDefinitionId)
        {
            PakageDefinitionId = pakageDefinitionId;
        }

        public int PakageDefinitionId { get; protected set; }
        public IUnpackageDefinition UnpackageDefinition { get; set; }
    }

    public class Unpacker : IUnpacker
    {
        public event EventHandler<HandledEventArgs<ResolvePakageDefinition>> PakageDefinitionResolve;
        public event EventHandler<EventArgs<int>> PakageDefinitionRegistered;

        private Dictionary<int, IUnpackageDefinition> m_definitionsById = new Dictionary<int, IUnpackageDefinition>();
        private Dictionary<int, string> m_stringCache = new Dictionary<int, string>();
        
        //private Dictionary<string, IUnpackageDefinition> m_definitionsByType = new Dictionary<string, IUnpackageDefinition>();
        
        public void AddPackageDefinition(IUnpackageDefinition packageDefinition)
        {
            if (!m_definitionsById.ContainsKey(packageDefinition.DefinitionId))
            {
                m_definitionsById.Add(packageDefinition.DefinitionId, packageDefinition);
                this.RaiseEvent(PakageDefinitionRegistered, packageDefinition.DefinitionId);
            }
            //m_definitionsByType.Add(packageDefinition.GetTypeDefinition(), packageDefinition);
        }

        public object Unpack(IDeserializer deserializer)
        {
            int packageDefinitionId = deserializer.DeserializeVariantInt();
            IUnpackageDefinition packageDefinition = GetPackageById(packageDefinitionId -1);
            return packageDefinition.Unpack(deserializer, this);
        }

        public IUnpackageDefinition GetPackageById(int packageDefinitionId)
        {
            IUnpackageDefinition packageDefinition;
            if (!m_definitionsById.TryGetValue(packageDefinitionId, out packageDefinition))
            {
                var missingPackageDefinitoin = new ResolvePakageDefinition(packageDefinitionId);
                if (!this.RaiseEvent(PakageDefinitionResolve, missingPackageDefinitoin))
                {
                    throw new UnkownPackageException(packageDefinitionId);
                }
                else
                {
                    packageDefinition = missingPackageDefinitoin.UnpackageDefinition;
                }
            }

            return packageDefinition;
        }

        public bool TryGetPackageType(Type type, out IUnpackageDefinition packageDefinition)
        {
            throw new NotImplementedException("??");
            //return m_definitionsByType.TryGetValue(type.FullName, out packageDefinition);
        }


        public bool SetCachedString(int id, string value)
        {
            m_stringCache.Add(id,value);
            return true;
        }

        public bool TryGetCachedString(int id, out string value)
        {
            return m_stringCache.TryGetValue(id, out value);
        }
    }
}

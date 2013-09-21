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
            object data;
            if (unpacker.Unpack(deserializer,out data))
            {
                return data as T;
            }
            return default(T);
        }
    }

    public interface IUnpacker
    {
        bool Unpack(IDeserializer deserializer,out object data);
        bool SetCachedString(int id, string value);
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
        private Dictionary<int, IUnpackageDefinition> m_definitionsById = new Dictionary<int, IUnpackageDefinition>();
        private Dictionary<int, string> m_stringCache = new Dictionary<int, string>();
        
        //private Dictionary<string, IUnpackageDefinition> m_definitionsByType = new Dictionary<string, IUnpackageDefinition>();
        
        public void AddPackageDefinition(IUnpackageDefinition packageDefinition)
        {
            if (!m_definitionsById.ContainsKey(packageDefinition.DefinitionId))
            {
                m_definitionsById.Add(packageDefinition.DefinitionId, packageDefinition);   
            }
            //m_definitionsByType.Add(packageDefinition.GetTypeDefinition(), packageDefinition);
        }

        public bool Unpack(IDeserializer deserializer,out object data)
        {
            int packageDefinitionId = deserializer.DeserializeVariantInt();
            if (packageDefinitionId == 0)
            {
                data = null;
                return true;
            }
            else
            {
                IUnpackageDefinition packageDefinition = GetPackageById(packageDefinitionId -1);
                return packageDefinition.Unpack(deserializer, this, out data);
            }
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

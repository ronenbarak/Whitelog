using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Whitelog.Core.Generic;
using Whitelog.Core.PakageDefinitions;
using Whitelog.Core.PakageDefinitions.Pack;
using Whitelog.Core.Serializer.MemoryBuffer;

namespace Whitelog.Core
{
    public class StreamLogBinaryPackager : BinaryPackager
    {
        private Unpacker m_unpacker;
        private readonly Dictionary<string, List<GenericUnpackageDefinition>> m_streamDefinition = new Dictionary<string, List<GenericUnpackageDefinition>>();
        private IBufferAllocator m_bufferAllocatorRegistration;

        protected override RegisteredPackageDefinition GetInheretancePackageDefinition(Type type,
                                                                                       RegisteredPackageDefinition sourcePackageDefinition,
                                                                                       object instance)
        {
            var attributes = type.GetCustomAttributes(false);
            if (attributes.Any(p=>p is CompilerGeneratedAttribute && type.IsGenericType && type.GetGenericArguments().Length == type.GetProperties().Count()))
            {
                var baseType = typeof (AllPropertiesPackageDefinition<>);
                var packageDefinitionType = baseType.MakeGenericType(new[] {type});
                var packageDefinitionInstance = Activator.CreateInstance(packageDefinitionType) as IBinaryPackageDefinition;

                RegisteredPackageDefinition regPackageDefinitionInstance;
                RegisterDefinition(packageDefinitionInstance, sourcePackageDefinition.DefinitionId, out regPackageDefinitionInstance);
                return regPackageDefinitionInstance;
            }
            else
            {
                return base.GetInheretancePackageDefinition(type, sourcePackageDefinition, instance);   
            }
        }

        public StreamLogBinaryPackager()
        {
            //m_packageDefinitionListWriter = packageDefinition;

            /*m_definitionsBinaryPackager = new BinaryPackager();
            RegisteredPackageDefinition registeredPackageDefinition;
            m_definitionsBinaryPackager.RegisterDefinition(new RegisteredPackageDefinitionDefinition(), RegisteredPackageDefinitionDefinition.ConstDefinitionId, (int)KnownPackageDefinition.NoDefinition, out registeredPackageDefinition);
            m_definitionsBinaryPackager.RegisterDefinition(new PropertyDefinitionDefinition(), PropertyDefinitionDefinition.ConstDefinitionId, (int)KnownPackageDefinition.NoDefinition, out registeredPackageDefinition);
            m_definitionsBinaryPackager.RegisterDefinition(new StringCachePackageDefinition(), StringCachePackageDefinition.ConstDefinitionId, (int)KnownPackageDefinition.NoDefinition, out registeredPackageDefinition);*/
            
            //m_unpacker = new Unpacker();
            //m_unpacker.AddPackageDefinition(new BinaryPackageDefinitionToGenericUnpackageDefinition());
            //m_unpacker.AddPackageDefinition(new GenericPropertyUnpackageDefinition());

            // Read all definitions
            /*byte[] definition = null;
            while ((definition = m_packageDefinitionListWriter.Read()) != null)
            {
                using (StreamDeserilizer ms = new StreamDeserilizer(definition))
                {
                    var packageData = m_unpacker.Unpack<GenericUnpackageDefinition>(ms);
                    if (packageData != null)
                    {
                        List<GenericUnpackageDefinition> genericPackageData;
                        if (!m_streamDefinition.TryGetValue(packageData.FullName, out genericPackageData))
                        {
                            m_streamDefinition.Add(packageData.FullName,
                                                   genericPackageData = new List<GenericUnpackageDefinition>());
                        }
                        genericPackageData.Add(packageData);
                    }
                }
            }*/

            RegisteredPackageDefinition registeredPackageDefinition;
            RegisterDefinition(new RegisteredPackageDefinitionDefinition(), RegisteredPackageDefinitionDefinition.ConstDefinitionId, (int)KnownPackageDefinition.NoDefinition, out registeredPackageDefinition);
            RegisterDefinition(new PropertyDefinitionDefinition(), PropertyDefinitionDefinition.ConstDefinitionId, (int)KnownPackageDefinition.NoDefinition, out registeredPackageDefinition);
            RegisterDefinition(new StringCachePackageDefinition(), StringCachePackageDefinition.ConstDefinitionId, (int)KnownPackageDefinition.NoDefinition, out registeredPackageDefinition);
            RegisterDefinition(new ConstPropertyDefinitionDefinition(), ConstPropertyDefinitionDefinition.ConstDefinitionId, (int)KnownPackageDefinition.NoDefinition, out registeredPackageDefinition);
        }

        //public bool RegisterDefinition(IBinaryPackageDefinition packageDefinition)
        //{
        //    lock (m_definitionSyncObjectLock)
        //    {
        //        // Check if the difinition is in the read collection


        //        // Set Definiton Id
        //        bool storInStream = true;

        //        RegisteredPackageDefinition registeredPackageDefinition;
        //        bool added = RegisterDefinition(packageDefinition,out registeredPackageDefinition);
        //        if (storInStream && added)
        //        {
        //            byte[] buffer = null;
        //            using (MemoryStreamSerializer ms = new MemoryStreamSerializer(m_bufferAllocatorRegistration))
        //            {
        //                m_definitionsBinaryPackager.Pack(registeredPackageDefinition, ms);
        //                buffer = ms.ToArray();
        //            }

        //            /*lock (m_packageDefinitionListWriter.LockObject)
        //            {
        //                m_packageDefinitionListWriter.WriteData(buffer);
        //            }*/
        //        }
        //        return added;
        //    }
        //}
    }
}
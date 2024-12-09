using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace SparseInject
{
#if UNITY_2019_1_OR_NEWER
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    internal static class ReflectionBakingProviderCache
    {
        private static Dictionary<Assembly, IReflectionBakingProvider> _cache = new Dictionary<Assembly, IReflectionBakingProvider>(32);

        public static bool TryGetInstanceFactory(Type type, out InstanceFactoryBase instanceFactory, out Type[] constructorParameterTypes)
        {
            if (TryGetProvider(type.Assembly, out var provider))
            {
                instanceFactory = provider.GetInstanceFactory(type);
                constructorParameterTypes = provider.ConstructorParametersSpan;
                
                return instanceFactory != null;
            }

            instanceFactory = null;
            constructorParameterTypes = null;
            
            return false;
        }
        
        private static bool TryGetProvider(Assembly assembly, out IReflectionBakingProvider provider)
        {
            if (_cache.TryGetValue(assembly, out provider))
            {
                return provider != null;
            }

            var providerType = assembly
                .GetType("SparseInject_ReflectionBakingProvider", false);
                    
            if (providerType != null)
            {
                provider = (IReflectionBakingProvider) FormatterServices.GetUninitializedObject(providerType);
                
                provider.Initialize();
                        
                _cache.Add(assembly, provider);

                return true;
            }
            
            _cache.Add(assembly, null);
            
            return false;
        }
    }
}
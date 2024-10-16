using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace SparseInject
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Container : IScopeResolver
    {
        private readonly Container _parentContainer;

        private readonly Dependency[] _dense;
        private readonly int[] _sparse;
        private readonly int[] _dependencyReferences;
        
        private readonly object[][] _arrays;

        internal Container(
            Container parentContainer,
            Dependency[] dependenciesDense,
            int[] sparse,
            int[] dependencyReferences,
            int maxConstructorLength)
        {
            _parentContainer = parentContainer;
            _dense = dependenciesDense;
            _sparse = sparse;
            _dependencyReferences = dependencyReferences;

            _arrays = ArrayCache.GetConstructorParametersPool(maxConstructorLength);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Resolve<T>() where T : class
        {
            var dependencyId = TypeCompileInfo<T>.GetRuntimeDependencyId();

            return (T) Resolve(dependencyId);
        }

        public object Resolve(int dependencyId)
        {
            dependencyId = _sparse[dependencyId];
            ref var dependency = ref _dense[dependencyId];
            var instances = dependency.ImplementationsCount == 1
                ? null
                : Array.CreateInstance(dependency.Type, dependency.ImplementationsCount);

            dependencyId++;

            for (var i = 0; i < dependency.ImplementationsCount; i++)
            {
                ref var implementation = ref _dense[dependencyId + i];

                if (implementation.SingletonFlag == SingletonFlag.SingletonWithValue)
                {
                    return implementation.SingletonValue;
                }

                if (implementation.ConstructorDependenciesCount == 0)
                {
                    var instanceOne = default(object);

                    if (implementation.GeneratedInstanceFactory != null)
                    {
                        instanceOne = implementation.GeneratedInstanceFactory.Create(null);
                    }
                    else
                    {
                        instanceOne = implementation.ConstructorInfo.Invoke(BindingFlags.Default, binder: null,
                            parameters: _arrays[0], culture: null);
                    }
                    
                    if (implementation.SingletonFlag == SingletonFlag.Singleton)
                    {
                        implementation.SingletonValue = instanceOne;
                        implementation.SingletonFlag = SingletonFlag.SingletonWithValue;
                    }

                    if (dependency.ImplementationsCount == 1)
                    {
                        return instanceOne;
                    }

                    instances.SetValue(instanceOne, i);
                    
                    continue;
                }

                var reserved = ArrayCache.PullReserved(implementation.ConstructorDependenciesCount);

                if (implementation.ScopeConfigurator != null)
                {
                    var containerBuilder = new ContainerBuilder(64);
                    
                    implementation.ScopeConfigurator.Invoke(containerBuilder, this);
                    
                    var container = containerBuilder.BuildInternal(this);
                    
                    for (var j = 0; j < implementation.ConstructorDependenciesCount; j++)
                    {
                        var constructorDependencyId = _dependencyReferences[j + implementation.ConstructorDependenciesIndex];

                        if (_sparse[constructorDependencyId] < 0)
                        {
                            reserved.Array[j + reserved.StartIndex] = container.Resolve(constructorDependencyId);
                        }
                        else
                        {
                            reserved.Array[j + reserved.StartIndex] = Resolve(constructorDependencyId);
                        }
                    }
                }
                else
                {
                    for (var j = 0; j < implementation.ConstructorDependenciesCount; j++)
                    {
                        var constructorDependencyId = _dependencyReferences[j + implementation.ConstructorDependenciesIndex];

                        if (_sparse[constructorDependencyId] < 0)
                        {
                            reserved.Array[j + reserved.StartIndex] = _parentContainer.Resolve(constructorDependencyId);
                        }
                        else
                        {
                            reserved.Array[j + reserved.StartIndex] = Resolve(constructorDependencyId);
                        }
                    }
                }

                var constructorParameters = _arrays[implementation.ConstructorDependenciesCount];

                for (var j = 0; j < implementation.ConstructorDependenciesCount; j++)
                {
                    constructorParameters[j] = reserved.Array[j + reserved.StartIndex];
                }

                var instance = default(object);
                
                if (implementation.GeneratedInstanceFactory != null)
                {
                    instance = implementation.GeneratedInstanceFactory.Create(constructorParameters);
                }
                else
                {
                    instance = implementation.ConstructorInfo.Invoke(BindingFlags.Default, binder: null,
                        parameters: constructorParameters, culture: null);
                }

                ArrayCache.PushReserved(implementation.ConstructorDependenciesCount);

                if (implementation.SingletonFlag == SingletonFlag.Singleton)
                {
                    implementation.SingletonValue = instance;
                    implementation.SingletonFlag = SingletonFlag.SingletonWithValue;
                }

                if (dependency.ImplementationsCount == 1)
                {
                    return instance;
                }

                instances.SetValue(instance, i);
            }

            return instances;
        }

        internal bool HasDependency(int dependencyId)
        {
            return _dense[dependencyId].Type != null;
        }
    }
}
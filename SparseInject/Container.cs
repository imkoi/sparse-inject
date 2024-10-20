using System;
using System.Collections.Generic;
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
        private readonly Dictionary<Type, int> _contractIds;
        private readonly int[] _contractsSparse;
        private readonly Contract[] _contractsDense;
        private readonly int[] _contractsConcretesIndices;
        private readonly Concrete[] _concretes;
        private readonly int[] _dependencyReferences;
        
        private readonly object[][] _arrays;

        internal Container(
            Container parentContainer,
            Dictionary<Type, int> contractIds,
            int[] contractsSparse,
            Contract[] contractsDense,
            int[] contractsConcretesIndices,
            Concrete[] concretes,
            int[] dependencyReferences,
            int maxConstructorLength)
        {
            _parentContainer = parentContainer;
            _contractIds = contractIds;
            _contractsSparse = contractsSparse;
            _contractsDense = contractsDense;
            _contractsConcretesIndices = contractsConcretesIndices;
            _concretes = concretes;
            _dependencyReferences = dependencyReferences;

            _arrays = ArrayCache.GetConstructorParametersPool(maxConstructorLength);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Resolve<T>() where T : class
        {
            if (_contractIds.TryGetValue(typeof(T), out var id))
            {
                return (T) Resolve(id);
            }

            throw new SparseInjectException("Trying to resolve unknown type");
        }

        public object Resolve(int dependencyId)
        {
            dependencyId = _contractsSparse[dependencyId];
            ref var contract = ref _contractsDense[dependencyId];
            var instances = contract.ConcretesCount == 1
                ? null
                : Array.CreateInstance(contract.Type, contract.ConcretesCount);

            for (var i = 0; i < contract.ConcretesCount; i++)
            {
                var concreteIndex = _contractsConcretesIndices[contract.ConcretesIndex + i];
                ref var concrete = ref _concretes[concreteIndex];
            
                if (concrete.SingletonFlag == SingletonFlag.SingletonWithValue)
                {
                    return concrete.SingletonValue;
                }
            
                if (concrete.ConstructorDependenciesCount == 0)
                {
                    var instanceOne = default(object);
            
                    if (concrete.GeneratedInstanceFactory != null)
                    {
                        instanceOne = concrete.GeneratedInstanceFactory.Create(null);
                    }
                    else
                    {
                        instanceOne = concrete.ConstructorInfo.Invoke(BindingFlags.Default, binder: null,
                            parameters: _arrays[0], culture: null);
                    }
                    
                    if (concrete.SingletonFlag == SingletonFlag.Singleton)
                    {
                        concrete.SingletonValue = instanceOne;
                        concrete.SingletonFlag = SingletonFlag.SingletonWithValue;
                    }
            
                    if (contract.ConcretesCount == 1)
                    {
                        return instanceOne;
                    }
            
                    instances.SetValue(instanceOne, i);
                    
                    continue;
                }
            
                var reserved = ArrayCache.PullReserved(concrete.ConstructorDependenciesCount);
            
                if (concrete.ScopeConfigurator != null)
                {
                    var containerBuilder = new ContainerBuilder(64);
                    
                    concrete.ScopeConfigurator.Invoke(containerBuilder, this);
                    
                    var container = containerBuilder.BuildInternal(this);
                    
                    for (var j = 0; j < concrete.ConstructorDependenciesCount; j++)
                    {
                        var constructorDependencyId = _dependencyReferences[j + concrete.ConstructorDependenciesIndex];
            
                        if (_contractsSparse[constructorDependencyId] < 0)
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
                    for (var j = 0; j < concrete.ConstructorDependenciesCount; j++)
                    {
                        var constructorDependencyId = _dependencyReferences[j + concrete.ConstructorDependenciesIndex];
            
                        if (_contractsSparse[constructorDependencyId] < 0)
                        {
                            reserved.Array[j + reserved.StartIndex] = _parentContainer.Resolve(constructorDependencyId);
                        }
                        else
                        {
                            reserved.Array[j + reserved.StartIndex] = Resolve(constructorDependencyId);
                        }
                    }
                }
            
                var constructorParameters = _arrays[concrete.ConstructorDependenciesCount];
            
                for (var j = 0; j < concrete.ConstructorDependenciesCount; j++)
                {
                    constructorParameters[j] = reserved.Array[j + reserved.StartIndex];
                }
            
                var instance = default(object);
                
                if (concrete.GeneratedInstanceFactory != null)
                {
                    instance = concrete.GeneratedInstanceFactory.Create(constructorParameters);
                }
                else
                {
                    instance = concrete.ConstructorInfo.Invoke(BindingFlags.Default, binder: null,
                        parameters: constructorParameters, culture: null);
                }
            
                ArrayCache.PushReserved(concrete.ConstructorDependenciesCount);
            
                if (concrete.SingletonFlag == SingletonFlag.Singleton)
                {
                    concrete.SingletonValue = instance;
                    concrete.SingletonFlag = SingletonFlag.SingletonWithValue;
                }
            
                if (contract.ConcretesCount == 1)
                {
                    return instance;
                }
            
                instances.SetValue(instance, i);
            }
            
            return instances;
        }
    }
}
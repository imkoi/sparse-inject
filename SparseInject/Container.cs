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
        private readonly Type _containerType;
        private readonly Container _parentContainer;
        private readonly Dictionary<Type, int> _contractIds;
        private readonly int[] _contractsSparse;
        private readonly Contract[] _contractsDense;
        private readonly int[] _contractsConcretesIndices;
        private readonly Concrete[] _concretes;
        private readonly int[] _dependencyReferences;
        private readonly int _concretesCount;

        private readonly object[][] _arrays;
        private readonly object[] _emptyArray;

        internal Container(
            Type containerType,
            Container parentContainer,
            Dictionary<Type, int> contractIds,
            int[] contractsSparse,
            Contract[] contractsDense,
            int[] contractsConcretesIndices,
            Concrete[] concretes,
            int[] dependencyReferences,
            int maxConstructorLength,
            int concretesCount)
        {
            _containerType = containerType;
            _parentContainer = parentContainer;
            _contractIds = contractIds;
            _contractsSparse = contractsSparse;
            _contractsDense = contractsDense;
            _contractsConcretesIndices = contractsConcretesIndices;
            _concretes = concretes;
            _dependencyReferences = dependencyReferences;
            _concretesCount = concretesCount;

            _arrays = ArrayCache.GetConstructorParametersPool(maxConstructorLength);
            _emptyArray = _arrays[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Resolve<T>() where T : class
        {
            var type = typeof(T);
            
            if (_contractIds.TryGetValue(type, out var id))
            {
                if (_contractsSparse[id] < 0)
                {
                    if (_parentContainer != null)
                    {
                        return _parentContainer.Resolve<T>();
                    }
                    
                    throw new SparseInjectException($"Trying to resolve unknown type '{type}'");
                }
                
                return (T) Resolve(id);
            }

            if (type.IsArray)
            {
                var elementType = type.GetElementType()!;

                if (_contractIds.TryGetValue(elementType, out id))
                {
                    var instance = Resolve(id);
                    var array = Array.CreateInstance(elementType, 1);
                    
                    array.SetValue(instance, 0);
                    
                    return (T) (object) array;
                }
                
                return (T) (object) Array.CreateInstance(elementType, 0);
            }

            throw new SparseInjectException($"Trying to resolve unknown type '{type}'");
        }

        public object Resolve(int dependencyId)
        {
            var denseIndex = _contractsSparse[dependencyId];
            
            // if (denseIndex < 0)
            // {
            //     if (_parentContainer != null)
            //     {
            //         reserved.Array[j + reserved.StartIndex] =
            //             _parentContainer.Resolve(constructorDependencyId);
            //     }
            //     else
            //     {
            //         var unknownParameter = concrete.ConstructorInfo.GetParameters()[j].ParameterType;
            //
            //         if (unknownParameter.IsArray)
            //         {
            //             reserved.Array[j + reserved.StartIndex] =
            //                 Array.CreateInstance(unknownParameter.GetElementType(), 0);
            //         }
            //         else
            //         {
            //             throw new SparseInjectException($"Trying to resolve unknown type '{unknownParameter}'");
            //         }
            //     }
            // }

            ref var contract = ref _contractsDense[denseIndex];
            var instances = contract.ConcretesCount == 1
                ? null
                : Array.CreateInstance(contract.Type, contract.ConcretesCount);
            var constructorContractsCount = -1;
            var constructorContractsIndex = -1;
            var reserved = default(ArrayCache.Reserved);

            for (var i = 0; i < contract.ConcretesCount; i++)
            {
                var concreteIndex = _contractsConcretesIndices[contract.ConcretesIndex + i];
                ref var concrete = ref _concretes[concreteIndex];

                var instance = default(object);
                
                if (!(concrete.IsSingleton() && concrete.HasValue()))
                {
                    constructorContractsCount = concrete.GetConstructorContractsCount();
                    constructorContractsIndex = concrete.GetConstructorContractsIndex();

                    if (constructorContractsCount > 0)
                    {
                        reserved = ArrayCache.PullReserved(constructorContractsCount);
                    }

                    if (concrete.IsScope())
                    {
                        var containerBuilder = new ContainerBuilder(this, _contractIds, 32);

                        ((Action<IScopeBuilder, IScopeResolver>)concrete.Value).Invoke(containerBuilder, this);

                        var container = containerBuilder.BuildInternal(concrete.Type, this);

                        for (var j = 0; j < constructorContractsCount; j++)
                        {
                            var constructorDependencyId = _dependencyReferences[j + constructorContractsIndex];

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
                        for (var j = 0; j < constructorContractsCount; j++)
                        {
                            var constructorDependencyId = _dependencyReferences[j + constructorContractsIndex];

                            if (_contractsSparse[constructorDependencyId] < 0)
                            {
                                if (_parentContainer != null)
                                {
                                    reserved.Array[j + reserved.StartIndex] =
                                        _parentContainer.Resolve(constructorDependencyId);
                                }
                                else
                                {
                                    var unknownParameter = concrete.ConstructorInfo.GetParameters()[j].ParameterType;

                                    if (unknownParameter.IsArray)
                                    {
                                        reserved.Array[j + reserved.StartIndex] =
                                            Array.CreateInstance(unknownParameter.GetElementType(), 0);
                                    }
                                    else
                                    {
                                        throw new SparseInjectException($"Trying to resolve unknown type '{unknownParameter}'");
                                    }
                                }
                            }
                            else
                            {
                                reserved.Array[j + reserved.StartIndex] = Resolve(constructorDependencyId);
                            }
                        }
                    }

                    var constructorParameters = default(object[]);

                    if (constructorContractsCount > 0)
                    {
                        constructorParameters = _arrays[constructorContractsCount];

                        for (var j = 0; j < constructorContractsCount; j++)
                        {
                            constructorParameters[j] = reserved.Array[j + reserved.StartIndex];
                        }

                        ArrayCache.PushReserved(constructorContractsCount);
                    }
                    else
                    {
                        constructorParameters = _emptyArray;
                    }

                    if (concrete.GeneratedInstanceFactory != null)
                    {
                        instance = concrete.GeneratedInstanceFactory.Create(constructorParameters);
                    }
                    else
                    {
#if DEBUG
                        try
                        {
                            instance = concrete.ConstructorInfo.Invoke(BindingFlags.Default, binder: null,
                                parameters: constructorParameters, culture: null);
                        }
                        catch(Exception exception)
                        {
                            throw new SparseInjectException($"Failed to create instance of '{concrete.Type}'\n{exception}");
                        }
#else
                        instance = concrete.ConstructorInfo.Invoke(BindingFlags.Default, binder: null,
                            parameters: constructorParameters, culture: null);
#endif
                    }

                    if (concrete.IsSingleton())
                    {
                        concrete.Value = instance;
                        concrete.MarkValue(true);
                    }
                }
                else
                {
                    if (concrete.IsFactory())
                    {
                        var factoryWithResolver = (Func<IScopeResolver, object>) concrete.Value;
                        var factory = factoryWithResolver.Invoke(this);
                        
                        instance = factory;
                        
                        concrete.Value = instance;
                        concrete.MarkFactory(false);
                    }
                    else
                    {
                        instance = concrete.Value;
                    }
                }

                if (contract.ConcretesCount == 1)
                {
                    return instance;
                }

#if DEBUG
                try
                {
                    instances.SetValue(instance, i);
                }
                catch (Exception exception)
                {
                    throw new SparseInjectException($"Failed resolve array of '{instances.GetType().GetElementType()}' because it has instance of type '{instance.GetType()}'\n{exception}");
                }
#else
                instances.SetValue(instance, i);
#endif
            }

            return instances;
        }

        internal bool TryGetConcrete(Type type, out Concrete concrete)
        {
            if (_contractIds.TryGetValue(type, out var contractId))
            {
                var denseIndex = _contractsSparse[contractId];

                if (denseIndex < 0 && _parentContainer != null)
                {
                    return _parentContainer.TryGetConcrete(type, out concrete);
                }

                var contract = _contractsDense[denseIndex];
                var concreteIndex = _contractsConcretesIndices[contract.ConcretesIndex + contract.ConcretesCount - 1];

                concrete = _concretes[concreteIndex];
                
                return true;
            }

            concrete = default(Concrete);
            return false;
        }

        public bool ContractExist(int contractId)
        {
            if (_contractsSparse[contractId] >= 0)
            {
                return true;
            }

            if (_parentContainer != null)
            {
                return _parentContainer.ContractExist(contractId);
            }

            return false;
        }

        public bool TryFindContainerWithContract(int contractId, out Container container)
        {
            if (_contractsSparse[contractId] >= 0)
            {
                container = this;
                return true;
            }
            
            if (_parentContainer != null)
            {
                return TryFindContainerWithContract(contractId, out container);
            }

            container = null;
            
            return false;
        }

        public int GetDependencyContractId(int contractIndex)
        {
            return _dependencyReferences[contractIndex];
        }

        internal ContainerInfo GetContainerInfo()
        {
            return new ContainerInfo(_parentContainer, _contractsSparse,
                _contractsDense, _contractsConcretesIndices, 
                _concretes, _dependencyReferences, _concretesCount);
        }
    }
}
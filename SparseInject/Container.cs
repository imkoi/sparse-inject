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

        private Dictionary<Type, Type> _fallbackElements = new Dictionary<Type, Type>(4);

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

            return (T) Resolve(type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private object Resolve(Type type)
        {
            if (_contractIds.TryGetValue(type, out var id))
            {
                if (_contractsSparse[id] < 0)
                {
                    if (_parentContainer != null)
                    {
                        return _parentContainer.Resolve(type);
                    }
                    
                    throw new SparseInjectException($"Trying to resolve unknown type '{type}'");
                }
                
                return ResolveInternal(id);
            }

            return ResolveFallback(type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private object ResolveFallback(Type type)
        {
            if (!_fallbackElements.TryGetValue(type, out var elementType))
            {
                elementType = type.IsArray ? type.GetElementType() : null;
                
                _fallbackElements.Add(type, elementType);
            }

            if (elementType != null)
            {
                return Array.CreateInstance(elementType, 0);
            }
            
            throw new SparseInjectException($"Trying to resolve unknown type '{type}'");
        }

        // TODO: make request should it resolve single instance or array
        private object ResolveInternal(int dependencyId)
        {
            var denseIndex = _contractsSparse[dependencyId];

            ref var contract = ref _contractsDense[denseIndex];
            var instances = contract.IsCollection()
                ? Array.CreateInstance(contract.Type, contract.GetConcretesCount())
                : null;
            var constructorContractsCount = -1;
            var constructorContractsIndex = -1;
            var reserved = default(ArrayCache.Reserved);

            for (var i = 0; i < contract.GetConcretesCount(); i++)
            {
                var concreteIndex = _contractsConcretesIndices[contract.GetConcretesIndex() + i];
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

                        var container = containerBuilder.BuildInternal(contract.Type, this);

                        for (var j = 0; j < constructorContractsCount; j++)
                        {
                            var constructorDependencyId = _dependencyReferences[j + constructorContractsIndex];

                            if (_contractsSparse[constructorDependencyId] < 0)
                            {
                                reserved.Array[j + reserved.StartIndex] = container.ResolveInternal(constructorDependencyId);
                            }
                            else
                            {
                                reserved.Array[j + reserved.StartIndex] = ResolveInternal(constructorDependencyId);
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
                                        _parentContainer.ResolveInternal(constructorDependencyId);
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
                                reserved.Array[j + reserved.StartIndex] = ResolveInternal(constructorDependencyId);
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

                    if (concrete.HasInstanceFactory())
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
                        concrete.MarkValue();
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

                if (!contract.IsCollection())
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
                var concreteIndex = _contractsConcretesIndices[contract.GetConcretesIndex() + contract.GetConcretesCount() - 1];

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
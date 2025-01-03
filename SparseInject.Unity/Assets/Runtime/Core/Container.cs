using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SparseInject
{
#if UNITY_2017_1_OR_NEWER
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class Container : IScopeResolver
    {
        private Type _containerType;
        private Container _parentContainer;
        private Dictionary<Type, int> _contractIds;
        private int[] _contractsSparse;
        private Contract[] _contractsDense;
        private int[] _contractsConcretesIndices;
        private Concrete[] _concretes;
        private int[] _dependencyReferences;
        private int _concretesCount;

        private object[][] _arrays;
        private object[] _emptyArray;

        private Dictionary<Type, Type> _fallbackElements = new Dictionary<Type, Type>(4);
        
        private bool _isDisposed;

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
                var contractIndex = _contractsSparse[id];
                
                if (contractIndex < 0)
                {
                    if (_parentContainer != null)
                    {
                        // TODO: add test that cover this case
                        return _parentContainer.Resolve(type);
                    }
                    
                    // TODO: add test that cover this case
                    throw new SparseInjectException($"Trying to resolve unknown type '{type}'");
                }
                
                return ResolveInternal(contractIndex);
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
        
        private object ResolveInternal(int contractIndex)
        {
            ref var contract = ref _contractsDense[contractIndex];
            var concretesCount = contract.GetConcretesCount();

            if (contract.IsCollection())
            {
                return ResolveMultipleConcrete(concretesCount, ref contract);
            }
            
            return ResolveSingleConcrete(concretesCount, ref contract);
        }

        private object ResolveSingleConcrete(int concretesCount, ref Contract contract)
        {
            var concreteIndex = _contractsConcretesIndices[contract.GetConcretesIndex() + concretesCount - 1];
            ref var concrete = ref _concretes[concreteIndex];

            var instance = default(object);
            
            if (!(concrete.IsSingleton() && concrete.HasValue()))
            {
                instance = CreateConcrete(ref contract, ref concrete);

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

            return instance;
        }

        private object ResolveMultipleConcrete(int concretesCount, ref Contract contract)
        {
            var instancesIndex = 0;
            
            var concreteIndex = _contractsConcretesIndices[contract.GetConcretesIndex()];
            ref var concrete = ref _concretes[concreteIndex];
                
            if (concretesCount == 1 && concrete.IsSingleton() && concrete.HasValue())
            {
                if (concrete.IsArray())
                {
                    return concrete.Value;
                }
                    
                var result = Array.CreateInstance(contract.Type, 1);
                    
                result.SetValue(concrete.Value, instancesIndex);
                    
                return result;
            }
            
            var instances = Array.CreateInstance(contract.Type, concretesCount);
            
            for (var i = 0; i < concretesCount; i++)
            {
                concreteIndex = _contractsConcretesIndices[contract.GetConcretesIndex() + i];
                concrete = ref _concretes[concreteIndex];

                var instance = default(object);
                
                if (!(concrete.IsSingleton() && concrete.HasValue()))
                {
                    instance = CreateConcrete(ref contract, ref concrete);

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

                if (concrete.IsArray())
                {
                    var array = instance as Array;
                    var arrayLength = array.Length;

                    if (concrete.Type == contract.Type)
                    {
                        instances.SetValue(array, instancesIndex);

                        instancesIndex++;
                    }
                    else if (arrayLength == 0)
                    {
                        var newLength = instances.Length - 1;

                        var newArray = Array.CreateInstance(contract.Type, newLength);
                        Array.Copy(instances, newArray, newLength);
                            
                        instances = newArray;
                    }
                    else if (arrayLength == 1)
                    {
                        instances.SetValue(array.GetValue(0), instancesIndex);
                        
                        instancesIndex++;
                    }
                    else
                    {
                        var oldLength = instances.Length;
                        var newLength = arrayLength - 1 + oldLength;

                        var newArray = Array.CreateInstance(contract.Type, newLength);
                        Array.Copy(instances, newArray, oldLength);
                            
                        instances = newArray;

                        for (var j = 0; j < arrayLength; j++)
                        {
                            instances.SetValue(array.GetValue(j), instancesIndex);

                            instancesIndex++;
                        }
                    }
                }
                else
                {
                    instances.SetValue(instance, instancesIndex);

                    instancesIndex++;
                }
            }

            return instances;
        }

        private object CreateConcrete(ref Contract contract, ref Concrete concrete)
        {
            var reserved = default(ArrayCache.Reserved);
            var contractIndex = -1;
            var createdContainer = default(Container);

            var constructorContractsCount = concrete.GetConstructorContractsCount();
            var constructorContractsIndex = concrete.GetConstructorContractsIndex();

            if (constructorContractsCount > 0)
            {
                reserved = ArrayCache.PullReserved(constructorContractsCount);
            }

            if (concrete.IsScope())
            {
                var containerBuilder = new ContainerBuilder(this, _contractIds, 32);

                ((Action<IScopeBuilder, IScopeResolver>)concrete.Value).Invoke(containerBuilder, this);

                createdContainer = containerBuilder.BuildInternal(contract.Type, this);

                for (var j = 0; j < constructorContractsCount; j++)
                {
                    var constructorDependencyId = _dependencyReferences[j + constructorContractsIndex];
                    contractIndex = _contractsSparse[constructorDependencyId];

                    // not exist in current scope - find in created one
                    if (contractIndex < 0)
                    {
                        contractIndex = createdContainer._contractsSparse[constructorDependencyId];

                        // not exist in current scope - find in parent
                        if (contractIndex < 0)
                        {
                            var parent = _parentContainer;
                            
                            while (parent != null)
                            {
                                contractIndex = parent._contractsSparse[constructorDependencyId];
                                
                                if (contractIndex < 0)
                                {
                                    parent = parent._parentContainer;
                                }
                                else
                                {
                                    reserved.Array[j + reserved.StartIndex] = parent.ResolveInternal(contractIndex);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            reserved.Array[j + reserved.StartIndex] = createdContainer.ResolveInternal(contractIndex);
                        }
                    }
                    else
                    {
                        reserved.Array[j + reserved.StartIndex] = ResolveInternal(contractIndex);
                    }
                }
            }
            else
            {
                for (var j = 0; j < constructorContractsCount; j++)
                {
                    var constructorDependencyId = _dependencyReferences[j + constructorContractsIndex];
                    contractIndex = _contractsSparse[constructorDependencyId];

                    if (contractIndex < 0)
                    {
                        var parent = _parentContainer;
                        
                        while (parent != null)
                        {
                            contractIndex = parent._contractsSparse[constructorDependencyId];
                                
                            if (contractIndex < 0)
                            {
                                parent = parent._parentContainer;
                            }
                            else
                            {
                                break;
                            }
                        }
                        
                        if (contractIndex < 0)
                        {
                            var unknownParameter = concrete.ConstructorInfo.GetParameters()[j].ParameterType;

                            if (unknownParameter.IsArray)
                            {
                                reserved.Array[j + reserved.StartIndex] =
                                    Array.CreateInstance(unknownParameter.GetElementType(), 0);
                            }
                            else
                            {
                                // TODO: Add test that cover this logic
                                throw new SparseInjectException($"Trying to resolve unknown type '{unknownParameter}'");
                            }
                        }
                        else
                        {
                            reserved.Array[j + reserved.StartIndex] = parent.ResolveInternal(contractIndex);
                        }
                    }
                    else
                    {
                        reserved.Array[j + reserved.StartIndex] = ResolveInternal(contractIndex);
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

#if UNITY_2021_2_OR_NEWER || NET
            if (concrete.HasInstanceFactory())
            {
                if (concrete.IsScope())
                {
                    var scope = concrete.GeneratedInstanceFactory.Create(constructorParameters) as Scope;

                    scope._container = createdContainer;

                    return scope;
                }
                
                return concrete.GeneratedInstanceFactory.Create(constructorParameters);
            }
            
            if (concrete.IsScope())
            {
                var scope = concrete.ConstructorInfo.Invoke(BindingFlags.Default, binder: null,
                    parameters: constructorParameters, culture: null) as Scope;

                scope._container = createdContainer;

                return scope;
            }
            
            return concrete.ConstructorInfo.Invoke(BindingFlags.Default, binder: null,
                parameters: constructorParameters, culture: null);
#else
            if (concrete.IsScope())
            {
                var scope = concrete.ConstructorInfo.Invoke(BindingFlags.Default, binder: null,
                    parameters: constructorParameters, culture: null) as Scope;

                scope._container = createdContainer;

                return scope;
            }
            
            return concrete.ConstructorInfo.Invoke(BindingFlags.Default, binder: null,
                parameters: constructorParameters, culture: null);
#endif
        }

        internal bool TryGetConcrete(Type type, out Concrete concrete)
        {
            if (_contractIds.TryGetValue(type, out var contractId))
            {
                var denseIndex = _contractsSparse[contractId];

                if (denseIndex < 0 && _parentContainer != null)
                {
                    // TODO: add test that have recursion case
                    return _parentContainer.TryGetConcrete(type, out concrete);
                }

                var contract = _contractsDense[denseIndex];
                var concreteIndex = _contractsConcretesIndices[contract.GetConcretesIndex() + contract.GetConcretesCount() - 1];

                concrete = _concretes[concreteIndex];
                
                return true;
            }

            // TODO: add test that have concrete not found case
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
                // TODO: add test that have recursion case
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
            
            var parent = _parentContainer;

            while (parent != null)
            {
                if (parent._contractsSparse[contractId] < 0)
                {
                    parent = parent._parentContainer;
                }
                else
                {
                    container = parent;

                    return true;
                }
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

        public void Dispose()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(_containerType?.Name ?? nameof(Container));
            }
            
            _parentContainer = null;
            _contractIds = null;
            _contractsSparse = null;
            _contractsDense = null;
            _contractsConcretesIndices = null;
            _concretes = null;
            _dependencyReferences = null;
            _concretesCount = -1;
    
            _arrays = null;
            _emptyArray = null;
    
            _fallbackElements = null;

            _isDisposed = true;
        }
    }
}
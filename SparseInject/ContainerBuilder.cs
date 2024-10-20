using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Unity.IL2CPP.CompilerServices;

namespace SparseInject
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ContainerBuilder : IScopeBuilder
    {
        private readonly Dictionary<Type, int> _contractIds;
        
        private int[] _contractsSparse;
        private Contract[] _contractsDense;
        private int[] _contractsConcretesIndices;
        private Concrete[] _concretes;

        private int _dependenciesCount;
        private int _implementationsCount;
        
        private int _lastSparseIndex;
        private int _lastContractsConcretesIndex;
        
        public ContainerBuilder(int capacity = 4096)
        {
            _contractIds = new Dictionary<Type, int>(capacity);
            _contractsSparse = new int[capacity];
            _contractsDense = new Contract[capacity];
            _contractsConcretesIndices = new int[capacity];
            _concretes = new Concrete[capacity];

            for (var i = 0; i < capacity; i++)
            {
                _contractsSparse[i] = -1;
            }
        }
        
        public void Register(Action<IScopeBuilder> registerMethod)
        {
            registerMethod.Invoke(this);
        }

        public void Register<TKey>(Lifetime lifetime = Lifetime.Transient)
            where TKey : class
        {
            Register<TKey, TKey>(lifetime);
        }

        public void Register<TKey, TImplementation>(Lifetime lifetime = Lifetime.Transient)
            where TKey : class
            where TImplementation : class, TKey
        {
            ref var concrete = ref AddConcrete(typeof(TImplementation), out var index);
            
            AddContract(typeof(TKey), index);

            concrete.SingletonFlag = lifetime == Lifetime.Singleton 
                ? SingletonFlag.Singleton
                : SingletonFlag.NotSingleton;
        }
        
        public void Register<TKey0, TKey1, TImplementation>(Lifetime lifetime = Lifetime.Transient)
            where TKey0 : class
            where TKey1 : class
            where TImplementation : class, TKey0, TKey1
        {
            Register<TKey0, TImplementation>(lifetime);
            Register<TKey1, TImplementation>(lifetime);
        }
        
        public void Register<TKey0, TKey1, TKey2, TImplementation>(Lifetime lifetime = Lifetime.Transient)
            where TKey0 : class
            where TKey1 : class
            where TKey2 : class
            where TImplementation : class, TKey0, TKey1, TKey2
        {
            Register<TKey0, TImplementation>(lifetime);
            Register<TKey1, TImplementation>(lifetime);
            Register<TKey2, TImplementation>(lifetime);
        }

        public void Register<TKey>(TKey value)
            where TKey : class
        {
            Register<TKey, TKey>(value);
        }

        public void Register<TKey, TImplementation>(TImplementation value)
            where TKey : class
            where TImplementation : class, TKey
        {
            ref var concrete = ref AddConcrete(typeof(TImplementation), out var index);
            
            AddContract(typeof(TKey), index);

            concrete.SingletonFlag = SingletonFlag.SingletonWithValue;
            concrete.SingletonValue = value;
        }
        
        public void RegisterScope<TScope>(Action<IScopeBuilder> install)
            where TScope : Scope
        {
            RegisterScope<TScope, TScope>(install);
        }

        public void RegisterScope<TScope, TScopeImplementation>(Action<IScopeBuilder> install)
            where TScope : class, IDisposable
            where TScopeImplementation : Scope
        {
            RegisterScope<TScope, TScopeImplementation>((builder, parentScope) =>
            {
                install(builder);
            });
        }

        public void RegisterScope<TScope>(Action<IScopeBuilder, IScopeResolver> install)
            where TScope : Scope
        {
            RegisterScope<TScope, TScope>(install);
        }

        public void RegisterScope<TScope, TScopeImplementation>(Action<IScopeBuilder, IScopeResolver> install)
            where TScope : class, IDisposable
            where TScopeImplementation : Scope
        {
            ref var concrete = ref AddConcrete(typeof(TScopeImplementation), out var index);
            
            AddContract(typeof(TScope), index);

            concrete.SingletonFlag = SingletonFlag.NotSingleton;
            concrete.ScopeConfigurator = install;
        }
        
        public Container Build()
        {
            return BuildInternal(null);
        }

        internal Container BuildInternal(Container parentContainer)
        {
            var stats = BuildPrecomputeDependenciesCount();
            var implementationDependencyIds = BuildBakeImplementationDependencyIds(
                stats.implementationDependenciesCount,
                stats.implementationConstructorParameterInfos,
                stats.implementationConstructorParameters,
                stats.maxConstructorLength);
            
            /*
            CircularDependencyValidator.ThrowIfInvalid(_implementationsCount, _contractsDense,
                _contractsSparse, implementationDependencyIds);*/
            
            return new Container(
                parentContainer,
                _contractIds,
                _contractsSparse,
                _contractsDense,
                _contractsConcretesIndices,
                _concretes,
                implementationDependencyIds,
                stats.maxConstructorLength);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref Concrete AddConcrete(Type concreteType, out int index)
        {
            index = _implementationsCount;

            if (index >= _concretes.Length)
            {
                Array.Resize(ref _concretes, index * 2);
            }
            
            ref var concrete = ref _concretes[index];
            
            concrete.Type = concreteType;

            _implementationsCount++;
            
            return ref concrete;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddContract(Type contractType, int concreteIndex)
        {
            if (!_contractIds.TryGetValue(contractType, out var contractId))
            {
                contractId = _contractIds.Count;
                
                _contractIds.Add(contractType, contractId);
            }

            if (contractId >= _contractsSparse.Length)
            {
                var oldSize = _contractsSparse.Length;
                var newSize = contractId * 2;

                Array.Resize(ref _contractsSparse, newSize);
                
                for (var i = oldSize; i < newSize; i++)
                {
                    _contractsSparse[i] = -1;
                }
            }

            ref var contractIndex = ref _contractsSparse[contractId];
            
            if (contractIndex < 0)
            {
                contractIndex = _dependenciesCount++;
            }
            
            if (contractIndex > _contractsDense.Length)
            {
                Array.Resize(ref _contractsDense, contractIndex * 2);
            }
            
            ref var contract = ref _contractsDense[contractIndex];
            contract.Type = contractType;

            if (contract.ConcretesCount == 0)
            {
                var contractsConcretesIndex = _lastContractsConcretesIndex++;
                
                contract.ConcretesIndex = contractsConcretesIndex;
            }

            if (contractId > _lastSparseIndex || _lastSparseIndex == 0)
            {
                _lastSparseIndex = contractId;
                _contractsConcretesIndices[contract.ConcretesIndex + contract.ConcretesCount] = concreteIndex;
            }
            else
            {
                // Array.Copy(_contractsDense, concreteIndex, _contractsDense, concreteIndex + 1, _denseCount - concreteIndex);
                //
                // _contractsDense[concreteIndex].Type = implementationType;
                //
                // for (var i = 0; i < _lastSparseIndex + 1; i++)
                // {
                //     if (_contractsSparse[i] > dependencyId)
                //     {
                //         _contractsSparse[i] += 1;
                //     }
                // }
            }

            contract.ConcretesCount++;
        }

        private (int implementationDependenciesCount, ParameterInfo[][] implementationConstructorParameterInfos, Type[][] implementationConstructorParameters, int maxConstructorLength) BuildPrecomputeDependenciesCount()
        {
            var implementationConstructorParameterInfos = new ParameterInfo[_implementationsCount][];
            var implementationConstructorParameters = new Type[_implementationsCount][];
            var implementationDependenciesCount = 0;
            var maxConstructorLength = int.MinValue;
            var concretesCount = _implementationsCount;

            for (var concreteIndex = 0; concreteIndex < concretesCount; concreteIndex++)
            {
                ref var concrete = ref _concretes[concreteIndex];
                    
                var constructorParametersCount = 0;
                    
                if (ReflectionBakingProviderCache.TryGetInstanceFactory(concrete.Type, out var factory, out var constructorParametersSpan))
                {
                    constructorParametersCount = factory.ConstructorParametersCount;

                    implementationConstructorParameters[concreteIndex] = constructorParametersSpan;
                    concrete.GeneratedInstanceFactory = factory;
                }
                else
                {
                    concrete.ConstructorInfo = concrete.Type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)[0];
                    var constructorParameters = concrete.ConstructorInfo.GetParameters();

                    constructorParametersCount = constructorParameters.Length;
                        
                    implementationConstructorParameterInfos[concreteIndex] = constructorParameters;
                }
                    
                concrete.ConstructorDependenciesCount = constructorParametersCount;

                if (constructorParametersCount > maxConstructorLength)
                {
                    maxConstructorLength = constructorParametersCount;
                }

                implementationDependenciesCount += constructorParametersCount;
            }
            
            return (implementationDependenciesCount, implementationConstructorParameterInfos, implementationConstructorParameters, maxConstructorLength);
        }

        private int[] BuildBakeImplementationDependencyIds(int implementationDependenciesCount,
            ParameterInfo[][] implementationConstructorParameterInfos, Type[][] implementationConstructorParameters,
            int maxConstructorLength)
        {
            var generatedInstanceFactoryDependencyIds = new int[maxConstructorLength];
            var dependencyReferences = new int[implementationDependenciesCount];
            var dependencyReferenceIndex = 0;

            var concretesCount = _implementationsCount;
            var concreteConstructorParametersCount = -1;

            for (var concreteIndex = 0; concreteIndex < concretesCount; concreteIndex++)
            {
                ref var concrete = ref _concretes[concreteIndex];

                concrete.ConstructorDependenciesIndex = dependencyReferenceIndex;

                concreteConstructorParametersCount = concrete.ConstructorDependenciesCount;

                for (var parameterIndex = 0; parameterIndex < concreteConstructorParametersCount; parameterIndex++)
                {
                    var parameterType = default(Type);
                    var contractId = -1;

                    // TODO: optimize
                    if (implementationConstructorParameters[concreteIndex] != null)
                    {
                        parameterType = implementationConstructorParameters[concreteIndex][
                            concrete.GeneratedInstanceFactory.ConstructorParametersIndex + parameterIndex];
                    }
                    else
                    {
                        parameterType = implementationConstructorParameterInfos[concreteIndex][parameterIndex]
                            .ParameterType;
                    }

                    if (parameterType.IsArray)
                    {
                        var elementType = parameterType.GetElementType();

                        if (!_contractIds.TryGetValue(elementType!, out contractId))
                        {
                            contractId = _contractIds.Count;

                            _contractIds.Add(parameterType, contractId);
                        }
                    }
                    else
                    {
                        if (!_contractIds.TryGetValue(parameterType, out contractId))
                        {
                            contractId = _contractIds.Count;

                            _contractIds.Add(parameterType, contractId);
                        }
                    }

                    dependencyReferences[parameterIndex + dependencyReferenceIndex] = contractId;
                    generatedInstanceFactoryDependencyIds[parameterIndex] = contractId;
                }

                dependencyReferenceIndex += concreteConstructorParametersCount;
            }

            return dependencyReferences;
        }
    }

    public struct Contract
    {
        public Type Type;
        public int ConcretesCount;
        public int ConcretesIndex;
    }
    
    public struct Concrete
    {
        public Type Type;
        public int ConstructorDependenciesIndex;
        public int ConstructorDependenciesCount;
        public ConstructorInfo ConstructorInfo;
        public InstanceFactoryBase GeneratedInstanceFactory;
        public int SingletonFlag;
        public object SingletonValue;
        public Action<IScopeBuilder, IScopeResolver> ScopeConfigurator;
    }
}
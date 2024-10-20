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
        
        private int _lastSparseIndex = -1;
        private int _lastContractsConcretesIndex;
        
        public ContainerBuilder(int capacity = 4096) : this(new Dictionary<Type, int>(capacity), capacity)
        {
            
        }

        internal ContainerBuilder(Dictionary<Type, int> contractIds, int capacity = 4096)
        {
            _contractIds = contractIds;
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

        public void Register<TConcreteContract>(Lifetime lifetime = Lifetime.Transient)
            where TConcreteContract : class
        {
            Register<TConcreteContract, TConcreteContract>(lifetime);
        }

        public void Register<TContract, TConcrete>(Lifetime lifetime = Lifetime.Transient)
            where TContract : class
            where TConcrete : class, TContract
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);
            
            var contractId = GetOrAddContractId<TContract>(out var contractType);
            
            AddContract(contractId, contractType, index);

            concrete.SingletonFlag = lifetime == Lifetime.Singleton 
                ? SingletonFlag.Singleton
                : SingletonFlag.NotSingleton;
        }
        
        public void Register<TContract0, TContract1, TConcrete>(Lifetime lifetime = Lifetime.Transient)
            where TContract0 : class
            where TContract1 : class
            where TConcrete : class, TContract0, TContract1
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);
            
            var contractId = GetOrAddContractId<TContract0>(out var contractType);
            
            AddContract(contractId, contractType, index);
            
            contractId = GetOrAddContractId<TContract1>(out contractType);
            
            AddContract(contractId, contractType, index);

            concrete.SingletonFlag = lifetime == Lifetime.Singleton 
                ? SingletonFlag.Singleton
                : SingletonFlag.NotSingleton;
        }
        
        public void Register<TContract0, TContract1, TContract2, TConcrete>(Lifetime lifetime = Lifetime.Transient)
            where TContract0 : class
            where TContract1 : class
            where TContract2 : class
            where TConcrete : class, TContract0, TContract1, TContract2
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);
            
            var contractId = GetOrAddContractId<TContract0>(out var contractType);
            
            AddContract(contractId, contractType, index);
            
            contractId = GetOrAddContractId<TContract1>(out contractType);
            
            AddContract(contractId, contractType, index);
            
            contractId = GetOrAddContractId<TContract2>(out contractType);
            
            AddContract(contractId, contractType, index);

            concrete.SingletonFlag = lifetime == Lifetime.Singleton 
                ? SingletonFlag.Singleton
                : SingletonFlag.NotSingleton;
        }

        public void Register<TConcreteContract>(TConcreteContract value)
            where TConcreteContract : class
        {
            Register<TConcreteContract, TConcreteContract>(value);
        }

        public void Register<TContract, TConcrete>(TConcrete value)
            where TContract : class
            where TConcrete : class, TContract
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);
            
            var contractId = GetOrAddContractId<TContract>(out var contractType);
            
            AddContract(contractId, contractType, index);

            concrete.SingletonFlag = SingletonFlag.SingletonWithValue;
            concrete.SingletonValue = value;
        }
        
        public void Register<TContract0, TContract1, TConcrete>(TConcrete value)
            where TContract0 : class
            where TContract1 : class
            where TConcrete : class, TContract0, TContract1
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);
            
            var contractId = GetOrAddContractId<TContract0>(out var contractType);
            
            AddContract(contractId, contractType, index);
            
            contractId = GetOrAddContractId<TContract1>(out contractType);
            
            AddContract(contractId, contractType, index);

            concrete.SingletonFlag = SingletonFlag.SingletonWithValue;
            concrete.SingletonValue = value;
        }
        
        public void Register<TContract0, TContract1, TContract2, TConcrete>(TConcrete value)
            where TContract0 : class
            where TContract1 : class
            where TContract2 : class
            where TConcrete : class, TContract0, TContract1, TContract2
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);
            
            var contractId = GetOrAddContractId<TContract0>(out var contractType);
            
            AddContract(contractId, contractType, index);
            
            contractId = GetOrAddContractId<TContract1>(out contractType);
            
            AddContract(contractId, contractType, index);
            
            contractId = GetOrAddContractId<TContract2>(out contractType);
            
            AddContract(contractId, contractType, index);

            concrete.SingletonFlag = SingletonFlag.SingletonWithValue;
            concrete.SingletonValue = value;
        }
        
        public void RegisterScope<TScopeConcreteContract>(Action<IScopeBuilder> install)
            where TScopeConcreteContract : Scope
        {
            RegisterScope<TScopeConcreteContract, TScopeConcreteContract>(install);
        }

        public void RegisterScope<TScopeContract, TScopeConcrete>(Action<IScopeBuilder> install)
            where TScopeContract : class, IDisposable
            where TScopeConcrete : Scope
        {
            RegisterScope<TScopeContract, TScopeConcrete>((builder, parentScope) =>
            {
                install(builder);
            });
        }

        public void RegisterScope<TScopeConcreteContract>(Action<IScopeBuilder, IScopeResolver> install)
            where TScopeConcreteContract : Scope
        {
            RegisterScope<TScopeConcreteContract, TScopeConcreteContract>(install);
        }

        public void RegisterScope<TScopeContract, TScopeConcrete>(Action<IScopeBuilder, IScopeResolver> install)
            where TScopeContract : class, IDisposable
            where TScopeConcrete : Scope
        {
            ref var concrete = ref AddConcrete(typeof(TScopeConcrete), out var index);
            
            var contractId = GetOrAddContractId<TScopeContract>(out var contractType);
            
            AddContract(contractId, contractType, index);

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
            var concreteConstructorContractIds = BuildBakeImplementationDependencyIds(
                stats.implementationDependenciesCount,
                stats.implementationConstructorParameterInfos,
                stats.implementationConstructorParameters,
                stats.maxConstructorLength);
            
            CircularDependencyValidator.ThrowIfInvalid(_implementationsCount, _concretes, _contractsDense,
                _contractsSparse, _contractsConcretesIndices, concreteConstructorContractIds);
            
            return new Container(
                parentContainer,
                _contractIds,
                _contractsSparse,
                _contractsDense,
                _contractsConcretesIndices,
                _concretes,
                concreteConstructorContractIds,
                stats.maxConstructorLength);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetOrAddContractId<TContract>(out Type contractType)
        {
            contractType = typeof(TContract);
            
            if (!_contractIds.TryGetValue(contractType, out var contractId))
            {
                contractId = _contractIds.Count;
                
                _contractIds.Add(contractType, contractId);

                return contractId;
            }

            _contractIds.TryAdd(typeof(TContract[]), contractId);

            return contractId;
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
        private void AddContract(int contractId, Type contractType, int concreteIndex)
        {
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
                contract.ConcretesIndex = _lastContractsConcretesIndex;
            }

            if (contractId >= _lastSparseIndex)
            {
                _lastSparseIndex = contractId;
                _contractsConcretesIndices[contract.ConcretesIndex + contract.ConcretesCount] = concreteIndex;
            }
            else
            {
                var index = contract.ConcretesIndex + contract.ConcretesCount;
                Array.Copy(_contractsConcretesIndices, index, _contractsConcretesIndices, index + 1, _lastContractsConcretesIndex - index);

                _contractsConcretesIndices[index] = concreteIndex;

                for (var i = 0; i < _dependenciesCount; i++)
                {
                    ref var contractToProcess = ref _contractsDense[i];
                    if (contractToProcess.ConcretesIndex >= index)
                    {
                        contractToProcess.ConcretesIndex += 1;
                    }
                }
            }
            
            _lastContractsConcretesIndex++;
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
                    
                if (concrete.SingletonFlag != SingletonFlag.SingletonWithValue)
                {
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
                }
                
                concrete.ConstructorContractsCount = constructorParametersCount;

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

                concrete.ConstructorContractsIndex = dependencyReferenceIndex;

                concreteConstructorParametersCount = concrete.ConstructorContractsCount;

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
        public int ConstructorContractsIndex;
        public int ConstructorContractsCount;
        public ConstructorInfo ConstructorInfo;
        public InstanceFactoryBase GeneratedInstanceFactory;
        public int SingletonFlag;
        public object SingletonValue;
        public Action<IScopeBuilder, IScopeResolver> ScopeConfigurator;
    }
}
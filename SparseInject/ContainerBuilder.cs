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
    public partial class ContainerBuilder : IScopeBuilder
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

            var hasContract = _contractIds.TryGetValue(contractType, out var contractId);
            var maxSparseIndex = 0;
            
            if (hasContract)
            {
                maxSparseIndex = _contractIds.Count;
                
                _contractIds.TryAdd(typeof(TContract[]), maxSparseIndex);
            }
            else
            {
                contractId = _contractIds.Count;
                maxSparseIndex = contractId;
                
                _contractIds.Add(contractType, contractId);
            }
            
            if (maxSparseIndex >= _contractsSparse.Length)
            {
                var oldSize = _contractsSparse.Length;
                var newSize = maxSparseIndex * 2;

                Array.Resize(ref _contractsSparse, newSize);
                
                for (var i = oldSize; i < newSize; i++)
                {
                    _contractsSparse[i] = -1;
                }
            }

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
            ref var contract = ref GetContract(contractId);

            if (contract.ConcretesCount == 0)
            {
                contract.Type = contractType;
                contract.ConcretesIndex = _lastContractsConcretesIndex;
            }
            else
            {
                
            }

            var nextContractsConcretesCount = _lastContractsConcretesIndex + 1;
            
            if (nextContractsConcretesCount > _contractsConcretesIndices.Length)
            {
                Array.Resize(ref _contractsConcretesIndices, nextContractsConcretesCount * 2);
            }

            var index = contract.ConcretesIndex + contract.ConcretesCount;
            
            if (contractId >= _lastSparseIndex)
            {
                _lastSparseIndex = contractId;
                _contractsConcretesIndices[index] = concreteIndex;
            }
            else
            {
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref Contract GetContract(int contractId)
        {
            ref var contractIndex = ref _contractsSparse[contractId];
            
            if (contractIndex < 0)
            {
                contractIndex = _dependenciesCount++;
            }
            
            if (contractIndex >= _contractsDense.Length)
            {
                Array.Resize(ref _contractsDense, contractIndex * 2);
            }
            
            return ref _contractsDense[contractIndex];
        }
    }
}
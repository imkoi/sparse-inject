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
        private readonly Container _parentContainer;
        private readonly Dictionary<Type, int> _contractIds;
        
        private int[] _contractsSparse;
        private Contract[] _contractsDense;
        private int[] _contractsConcretesIndices;
        private Concrete[] _concretes;

        private int _dependenciesCount;
        private int _concretesCount;
        
        private int _lastContractsConcretesIndex;
        
        public ContainerBuilder(int capacity = 4096) : this(null, new Dictionary<Type, int>(capacity), capacity)
        {
            
        }

        internal ContainerBuilder(
            Container parentContainer,
            Dictionary<Type, int> contractIds,
            int capacity = 4096)
        {
            _parentContainer = parentContainer;
            _contractIds = contractIds;
            _contractsSparse = new int[capacity];
            _contractsDense = new Contract[capacity];
            _contractsConcretesIndices = new int[capacity];
            _concretes = new Concrete[capacity];

            for (var i = 0; i < capacity; i++)
            {
                _contractsSparse[i] = -1;
            }

            for (var i = 0; i < capacity; i++)
            {
                _contractsConcretesIndices[i] = -1;
            }
        }
        
        public Container Build()
        {
            return BuildInternal(null, null);
        }

        internal Container BuildInternal(Type containerType, Container parentContainer)
        {
            var stats = BuildPrecomputeDependenciesCount();
            var concreteConstructorContractIds = BuildBakeImplementationDependencyIds(
                containerType,
                stats.implementationDependenciesCount,
                stats.implementationConstructorParameters,
                stats.maxConstructorLength);
            
            CircularDependencyValidator.ThrowIfInvalid(new ContainerInfo(
                parentContainer,
                _contractsSparse,
                _contractsDense,
                _contractsConcretesIndices,
                _concretes,
                concreteConstructorContractIds,
                _concretesCount));

            return new Container(
                containerType,
                parentContainer,
                _contractIds,
                _contractsSparse,
                _contractsDense,
                _contractsConcretesIndices,
                _concretes,
                concreteConstructorContractIds,
                stats.maxConstructorLength,
                _concretesCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetOrAddContractId<TContract>(out Type contractType)
        {
            contractType = typeof(TContract);

            return GetOrAddContractId(contractType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetOrAddContractId(Type contractType)
        {
            if (!_contractIds.TryGetValue(contractType, out var contractId))
            {
                contractId = _contractIds.Count;
                
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

                _contractIds.Add(contractType, contractId);
            }

            return contractId;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref Concrete AddConcrete(Type concreteType, out int index)
        {
            index = _concretesCount;

            if (index >= _concretes.Length)
            {
                Array.Resize(ref _concretes, index * 2);
            }
            
            ref var concrete = ref _concretes[index];
            
            concrete.Type = concreteType;

            _concretesCount++;
            
            return ref concrete;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddContract<T>(int concreteIndex)
        {
            var contractId = GetOrAddContractId<T>(out var contractType);
            
            ref var contract = ref GetContract(contractId);

            var concretesIndex = _lastContractsConcretesIndex;
            var concretesCount = contract.GetConcretesCount();
            
            contract.SetConcretesIndex(concretesIndex);
            
            if (concretesCount == 0)
            {
                contract.Type = contractType;
                contract.SetConcretesCount(1);
                
                var collectionContractId = GetOrAddContractId<T[]>(out _);

                contract = ref GetContract(collectionContractId);
                contract.Type = contractType;
                contract.SetConcretesIndex(concretesIndex);
                contract.MarkCollection();
            }
            else
            {
                var collectionContractId = GetOrAddContractId<T[]>(out _);

                contract = ref GetContract(collectionContractId);
                concretesIndex = contract.GetConcretesIndex();
                concretesCount = contract.GetConcretesCount();
            }

            var nextContractsConcretesCount = _lastContractsConcretesIndex + 1;
            
            if (nextContractsConcretesCount > _contractsConcretesIndices.Length)
            {
                var oldSize = _contractsConcretesIndices.Length;
                var newSize = nextContractsConcretesCount * 2;
                
                Array.Resize(ref _contractsConcretesIndices, newSize);

                for (var i = oldSize; i < newSize; i++)
                {
                    _contractsConcretesIndices[i] = -1;
                }
            }

            var index = concretesIndex + concretesCount;

            if (index == _lastContractsConcretesIndex)
            {
                _contractsConcretesIndices[index] = concreteIndex;
            }
            else
            {
                Array.Copy(_contractsConcretesIndices, index, _contractsConcretesIndices, index + 1, _lastContractsConcretesIndex - index);

                _contractsConcretesIndices[index] = concreteIndex;

                for (var i = 0; i < _dependenciesCount; i++)
                {
                    ref var contractToProcess = ref _contractsDense[i];

                    concretesIndex = contractToProcess.GetConcretesIndex();
                    
                    // >= because previous swapped contract not update index yet
                    if (concretesIndex >= index)
                    {
                        concretesIndex++;
                        
                        contractToProcess.SetConcretesIndex(concretesIndex);
                    }
                }
            }
            
            _lastContractsConcretesIndex++;

            concretesCount++;
            contract.SetConcretesCount(concretesCount);
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
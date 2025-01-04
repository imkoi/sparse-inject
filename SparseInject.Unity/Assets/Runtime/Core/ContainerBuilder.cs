using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SparseInject
{
#if UNITY_2017_1_OR_NEWER
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public partial class ContainerBuilder : IScopeBuilder
    {
        private readonly Container _parentContainer;
        private readonly Dictionary<Type, int> _contractIds;
        
        private int[] _contractsSparse;
        private Contract[] _contractsDense;
        private int[] _contractsConcretesIndices;
        private Concrete[] _concretes;

        private int[] _valueDisposablesIndices;
        private int _valueDisposablesCount;

        private int _dependenciesCount;
        private int _concretesCount;
        
        private int _lastContractsConcretesIndex;
        
        public ContainerBuilder(int capacity = 1024) : this(null, new Dictionary<Type, int>(capacity), capacity)
        {
            
        }

        internal ContainerBuilder(
            Container parentContainer,
            Dictionary<Type, int> contractIds,
            int capacity)
        {
            _parentContainer = parentContainer;
            _contractIds = contractIds;
            
            _concretes = new Concrete[capacity];
            _contractsConcretesIndices = new int[capacity];
            
            for (var i = 0; i < capacity; i++)
            {
                _contractsConcretesIndices[i] = -1;
            }

            capacity *= 2;
            _contractsSparse = new int[capacity];
            _contractsDense = new Contract[capacity];
            
            for (var i = 0; i < capacity; i++)
            {
                _contractsSparse[i] = -1;
            }
        }
        
        public Container Build()
        {
            return BuildInternal(null, null);
        }

#if DEBUG
        internal ContainerBuilderInfo GetContainerBuilderInfo()
        {
            return new ContainerBuilderInfo(_parentContainer, _contractsSparse, _contractsDense,
                _contractsConcretesIndices, _concretes, _concretesCount);
        }
#endif

        internal Container BuildInternal(Type containerType, Container parentContainer)
        {
            var stats = BuildPrecomputeDependenciesCount();
            var concreteConstructorContractIds = BuildBakeImplementationDependencyIds(
                containerType,
                stats.implementationDependenciesCount,
                stats.implementationConstructorParameters);
            
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
                _concretesCount,
                _valueDisposablesIndices,
                _valueDisposablesCount);
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
            var contractIndex = GetContractIndex(contractId);
            ref var contract = ref _contractsDense[contractIndex];

            var concretesIndex = contract.GetConcretesIndex();
            var concretesCount = contract.GetConcretesCount();
            
            if (concretesCount == 0)
            {
                concretesIndex = _lastContractsConcretesIndex;
                
                if (contractType.IsArray)
                {
                    contract.Type = contractType.GetElementType();
                    contract.MarkCollection();
                }
                else
                {
                    contract.Type = contractType;
                }
                
                contract.SetConcretesIndex(concretesIndex);
                contract.SetConcretesCount(1);
            }
            else
            {
                contract.SetConcretesCount(++concretesCount);
            }

            // collection contract
            
            var collectionContractId = GetOrAddContractId<T[]>(out _);
            var collectionContractIndex = GetContractIndex(collectionContractId);
            ref var collectionContract = ref _contractsDense[collectionContractIndex];

            var collectionConcretesIndex = collectionContract.GetConcretesIndex();
            var collectionConcretesCount = collectionContract.GetConcretesCount();
            
            var nextIndexGrow = -1;
            
            if (collectionConcretesCount == 0)
            {
                collectionConcretesIndex = contract.IsCollection() ? _lastContractsConcretesIndex : concretesIndex;
                
                collectionContract.Type = contractType;
                collectionContract.SetConcretesIndex(collectionConcretesIndex);
                
                nextIndexGrow = _contractsSparse[contractId] + 2;
                
                collectionContract.MarkCollection();
            }
            else
            {
                nextIndexGrow = _contractsSparse[contractId] + 2;
            }

            var nextContractsConcretesCount = _lastContractsConcretesIndex + 1;
            TryExtendCapacityContractConcreteIndices(nextContractsConcretesCount);

            var index = collectionConcretesIndex + collectionConcretesCount;

            if (index == _lastContractsConcretesIndex)
            {
                _contractsConcretesIndices[index] = concreteIndex;
            }
            else
            {
                // TODO: this functional not work as expected
                Array.Copy(_contractsConcretesIndices, index, _contractsConcretesIndices, index + 1, _lastContractsConcretesIndex - index);

                _contractsConcretesIndices[index] = concreteIndex;

                for (; nextIndexGrow < _dependenciesCount; nextIndexGrow++)
                {
                    ref var contractToProcess = ref _contractsDense[nextIndexGrow];

                    contractToProcess.SetConcretesIndex(contractToProcess.GetConcretesIndex() + 1);
                }
            }
            
            _lastContractsConcretesIndex++;

            collectionContract.SetConcretesCount(++collectionConcretesCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetContractIndex(int contractId)
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

            return contractIndex;
        }

        private void TryExtendCapacityContractConcreteIndices(int targetCount)
        {
            if (targetCount > _contractsConcretesIndices.Length)
            {
                var oldSize = _contractsConcretesIndices.Length;
                var newSize = targetCount * 2;
                
                Array.Resize(ref _contractsConcretesIndices, newSize);

                for (var i = oldSize; i < newSize; i++)
                {
                    _contractsConcretesIndices[i] = -1;
                }
            }
        }

        internal void MarkConcreteDisposable(int concreteIndex)
        {
            ref var concrete = ref _concretes[concreteIndex];

            if (!concrete.IsSingleton())
            {
                throw new SparseInjectException("Only singleton contracts could be marked as disposable.");
            }
            
            concrete.MarkDisposable();

            if (concrete.HasValue())
            {
                if (_valueDisposablesCount == 0)
                {
                    _valueDisposablesIndices = new int[32];
                }
                else if (_valueDisposablesCount + 1 >= _valueDisposablesIndices.Length)
                {
                    Array.Resize(ref _valueDisposablesIndices, _valueDisposablesIndices.Length * 2);
                }
            
                _valueDisposablesIndices[_valueDisposablesCount] = concreteIndex;
                _valueDisposablesCount++;
            }
        }
    }
}
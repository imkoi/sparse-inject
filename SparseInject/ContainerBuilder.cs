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
        private readonly TypeIdProvider _contractIds;
        
        private int[] _contractsSparse;
        private Contract[] _contractsDense;
        private int[] _contractsConcretesIndices;
        private Concrete[] _concretes;

        private int[] _valueDisposablesIndices;
        private int _valueDisposablesCount;

        private int _dependenciesCount;
        private int _concretesCount;
        
        private int _lastContractsConcretesIndex;
        
        public ContainerBuilder(int capacity = 1024) : this(null, new TypeIdProvider(capacity), capacity)
        {
            
        }

        internal ContainerBuilder(
            Container parentContainer,
            TypeIdProvider contractIds,
            int capacity)
        {
            _parentContainer = parentContainer;
            _contractIds = contractIds;
            
            _concretes = new Concrete[capacity];
            _contractsConcretesIndices = new int[capacity];
            
            ArrayUtilities.Fill(_contractsConcretesIndices, -1);

            capacity *= 2;
            _contractsSparse = new int[capacity];
            _contractsDense = new Contract[capacity];

            ArrayUtilities.Fill(_contractsSparse, -1);
        }
        
        public Container Build()
        {
            return BuildInternal(null, null);
        }

        internal ContainerBuilderInfo GetContainerBuilderInfo()
        {
            return new ContainerBuilderInfo(_parentContainer, _contractsSparse, _contractsDense,
                _contractsConcretesIndices, _concretes, _concretesCount);
        }

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

        private void AddContract(Type contractType, Type collectionContractType, int concreteIndex)
        {
            _contractIds.TryAdd(contractType, out var contractId);
            _contractIds.TryAdd(collectionContractType, out var collectionContractId);

            var highestContractId = contractId > collectionContractId 
                ? contractId 
                : collectionContractId;
            
            var oldSize = _contractsSparse.Length;
                
            if (highestContractId >= oldSize)
            {
                var newSize = contractId * 2;

                Array.Resize(ref _contractsSparse, newSize);
                
                ArrayUtilities.Fill(_contractsSparse, -1, oldSize);
            }

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

                ArrayUtilities.Fill(_contractsConcretesIndices, -1, oldSize);
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
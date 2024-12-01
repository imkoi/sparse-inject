using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace SparseInject
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class CircularDependencyValidator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowIfInvalid(ContainerInfo containerInfo)
        {
            var concretesCount = containerInfo.ConcretesCount;
            
            var circularDependencyChecker = new List<Concrete>(concretesCount);

            for (var i = 0; i < containerInfo.ConcretesCount; i++)
            {
                circularDependencyChecker.Clear();
                    
                ThrowIfInvalidRecursive(i, containerInfo, circularDependencyChecker);
            }
        }
        
        private static void ThrowIfInvalidRecursive(int concreteIndex, ContainerInfo containerInfo,
            List<Concrete> stack)
        {
            var _concretes = containerInfo.Concretes;
            var _contractsSparse = containerInfo.ContractsSparse; 
            var _contractsDense = containerInfo.ContractsDense;
            var _contractsConcretesIndices = containerInfo.ContractsConcretesIndices;
            
            ref var concrete = ref _concretes[concreteIndex];
            var stackCount = stack.Count;

            for (var i = 0; i < stackCount; i++)
            {
                if (concrete.Type == stack[i].Type)
                {
                    stack.Add(concrete);

                    var path = string.Join("\n",
                        stack.Take(i + 1)
                            .Reverse()
                            .Select((item, itemIndex) => $"    [{itemIndex + 1}] {item} --> {item.Type.FullName}"));
                    
                    throw new SparseInjectException($"{concrete.Type}: Circular dependency detected!\n{path}");
                }
            }

            stack.Add(concrete);
            
            var constructorContractsCount = concrete.GetConstructorContractsCount();
            var constructorContractsIndex = concrete.GetConstructorContractsIndex();

            for (var i = 0; i < constructorContractsCount; i++)
            {
                var constructorContractId = containerInfo.ConcreteConstructorContractIds[i + constructorContractsIndex];
                var constructorContractIndex = _contractsSparse[constructorContractId];

                if (constructorContractIndex >= 0)
                {
                    ref var constructorContract = ref _contractsDense[constructorContractIndex];

                    for (var j = 0; j < constructorContract.ConcretesCount; j++)
                    {
                        var concreteId = _contractsConcretesIndices[j + constructorContract.ConcretesIndex];
                        
                        ThrowIfInvalidRecursive(concreteId, containerInfo, stack);
                    }
                }
                else if(containerInfo.ParentContainer != null && containerInfo.ParentContainer
                            .TryFindContainerWithContract(constructorContractId, out var targetContainer))
                {
                    var requestedInfo = targetContainer.GetContainerInfo();
                    var denseIndex = requestedInfo.ContractsSparse[constructorContractId];
                    ref var constructorContract = ref requestedInfo.ContractsDense[denseIndex];

                    for (var j = 0; j < constructorContract.ConcretesCount; j++)
                    {
                        var concreteId = requestedInfo.ContractsConcretesIndices[j + constructorContract.ConcretesIndex];
                        
                        ThrowIfInvalidRecursive(concreteId, requestedInfo, stack);
                    }
                }
                else if(!concrete.IsScope())
                {
                    throw new SparseInjectException($"Circular dependency validator failed because of unknown dependency in {concrete.Type}!");
                }
            }

            stack.RemoveAt(stack.Count - 1);
        }
    }
}
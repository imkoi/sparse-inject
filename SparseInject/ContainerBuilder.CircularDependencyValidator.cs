using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace SparseInject
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public partial class ContainerBuilder
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ThrowIfInvalid(Container parentContainer, int[] concreteConstructorContractIds)
        {
            var concretesCount = _implementationsCount;
            
            var circularDependencyChecker = new List<Concrete>(concretesCount);

            for (var i = 0; i < concretesCount; i++)
            {
                circularDependencyChecker.Clear();
                    
                ThrowIfInvalidRecursive(i, parentContainer, concreteConstructorContractIds, circularDependencyChecker);
            }
        }
        
        private void ThrowIfInvalidRecursive(int concreteIndex, Container parentContainer, int[] concreteConstructorContractIds,
            List<Concrete> stack)
        {
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
                var constructorContractId = concreteConstructorContractIds[i + constructorContractsIndex];

                if (concrete.IsScope())
                {
                    continue;
                }

                if (constructorContractId < 0)
                {
                    throw new SparseInjectException($"Circular dependency validator failed because of unknown dependency in {concrete.Type}!");
                }
                
                var constructorContractIndex = _contractsSparse[constructorContractId];

                if (constructorContractIndex >= 0)
                {
                    ref var constructorContract = ref _contractsDense[constructorContractIndex];

                    for (var j = 0; j < constructorContract.ConcretesCount; j++)
                    {
                        var concreteId = _contractsConcretesIndices[j + constructorContract.ConcretesIndex];
                        
                        ThrowIfInvalidRecursive(concreteId, parentContainer, concreteConstructorContractIds, stack);
                    }
                }
            }

            stack.RemoveAt(stack.Count - 1);
        }
    }
}
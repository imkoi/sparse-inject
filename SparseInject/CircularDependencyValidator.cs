using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SparseInject
{
    public static class CircularDependencyValidator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfInvalid(int concretesCount, Concrete[] concretes, Contract[] contractsDense,
            int[] contractsSparse, int[] contractsConcretesIndices, int[] concreteConstructorContractIds)
        {
            var circularDependencyChecker = new List<Concrete>(concretesCount);

            for (var i = 0; i < concretesCount; i++)
            {
                circularDependencyChecker.Clear();
                    
                ThrowIfInvalidRecursive(i, concretes, contractsDense, contractsSparse, contractsConcretesIndices,
                    concreteConstructorContractIds, circularDependencyChecker);
            }
        }
        
        private static void ThrowIfInvalidRecursive(int concreteIndex, Concrete[] concretes, 
            Contract[] contractsDense, int[] contractsSparse, int[] contractsConcretesIndices, int[] concreteConstructorContractIds, List<Concrete> stack)
        {
            ref var concrete = ref concretes[concreteIndex];
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

            for (var i = 0; i < concrete.ConstructorContractsCount; i++)
            {
                var constructorContractId = concreteConstructorContractIds[i + concrete.ConstructorContractsIndex];

                if (concrete.ScopeConfigurator != null)
                {
                    continue;
                }

                var constructorContractIndex = contractsSparse[constructorContractId];

                if (constructorContractIndex >= 0)
                {
                    ref var constructorContract = ref contractsDense[constructorContractIndex];

                    for (var j = 0; j < constructorContract.ConcretesCount; j++)
                    {
                        var concreteId = contractsConcretesIndices[j + constructorContract.ConcretesIndex];
                        
                        ThrowIfInvalidRecursive(concreteId, concretes, contractsDense, contractsSparse,
                            contractsConcretesIndices, concreteConstructorContractIds, stack);
                    }
                }
            }

            stack.RemoveAt(stack.Count - 1);
        }
    }
}
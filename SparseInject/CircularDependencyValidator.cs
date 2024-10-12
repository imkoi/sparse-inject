using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SparseInject
{
    public static class CircularDependencyValidator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfInvalid(int implementationsCount, Dependency[] dense, int[] sparse, int[] implementationDependencyIds)
        {
            var circularDependencyChecker = new List<Dependency>();
            
            var index = 0;
            var implementationIndex = 0;
            
            while (implementationIndex < implementationsCount)
            {
                ref var dependency = ref dense[index];
                index++;
                
                for (var i = 0; i < dependency.ImplementationsCount; i++)
                {
                    circularDependencyChecker.Clear();
                    
                    ThrowIfInvalidRecursive(index + i,
                        circularDependencyChecker, dense, sparse, implementationDependencyIds);

                    implementationIndex++;
                }

                index += dependency.ImplementationsCount;
            }
        }
        
        private static void ThrowIfInvalidRecursive(int implementationId, List<Dependency> stack,
            Dependency[] dense, int[] sparse, int[] implementationDependencyIds)
        {
            ref var implementation = ref dense[implementationId];
            var stackCount = stack.Count;

            for (var i = 0; i < stackCount; i++)
            {
                if (implementation.Type == stack[i].Type)
                {
                    stack.Add(implementation);

                    var path = string.Join("\n",
                        stack.Take(i + 1)
                            .Reverse()
                            .Select((item, itemIndex) => $"    [{itemIndex + 1}] {item} --> {item.Type.FullName}"));
                    
                    throw new SparseInjectException($"{implementation.Type}: Circular dependency detected!\n{path}");
                }
            }

            stack.Add(implementation);

            for (var i = 0; i < implementation.ConstructorDependenciesCount; i++)
            {
                var constructorDependencyId = implementationDependencyIds[i + implementation.ConstructorDependenciesIndex];

                if (implementation.ScopeConfigurator != null)
                {
                    continue;
                }

                var constructorDependencyIndex = sparse[constructorDependencyId];

                if (constructorDependencyIndex >= 0)
                {
                    ref var constructorDependency = ref dense[constructorDependencyIndex];

                    for (var j = 0; j < constructorDependency.ImplementationsCount; j++)
                    {
                        ThrowIfInvalidRecursive(constructorDependencyIndex + 1 + j, stack, dense, sparse, implementationDependencyIds);
                    }
                }
            }

            stack.RemoveAt(stack.Count - 1);
        }
    }
}
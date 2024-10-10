using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanResolver
{
    public static class CircularDependencyValidator
    {
        public static void ThrowIfInvalid(int implementationId, Stack<Dependency> stack,
            Dependency[] dependencies, int[] implementationDependencyIds)
        {
            var i = 0;

            ref var implementation = ref dependencies[implementationId];

            foreach (var dependency in stack)
            {
                if (implementation.Type == dependency.Type)
                {
                    stack.Push(implementation);

                    var path = string.Join("\n",
                        stack.Take(i + 1)
                            .Reverse()
                            .Select((item, itemIndex) => $"    [{itemIndex + 1}] {item} --> {item.Type.FullName}"));
                    
                    throw new Exception($"{implementation.Type}: Circular dependency detected!\n{path}");
                }
                i++;
            }

            stack.Push(implementation);

            for (var j = 0; j < implementation.ConstructorDependenciesCount; j++)
            {
                var constructorDependencyId = implementationDependencyIds[j + implementation.ConstructorDependenciesIndex];

                if (constructorDependencyId < 0 && implementation.ScopeConfigurator != null)
                {
                    continue;
                }
                
                ref var constructorDependency = ref dependencies[constructorDependencyId];

                for (var k = 0; k < constructorDependency.ImplementationsCount; k++)
                {
                    ThrowIfInvalid(k + j + 1, stack, dependencies, implementationDependencyIds);
                }
            }

            stack.Pop();
        }
    }
}
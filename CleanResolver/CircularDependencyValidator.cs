using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanResolver
{
    public static class CircularDependencyValidator
    {
        public static void ThrowIfInvalid(Implementation current, Stack<Implementation> stack,
            Dependency[] dependencies, Implementation[] implementations, int[] dependencyImplementationIds, int[] implementationDependencyIds)
        {
            var i = 0;

            foreach (var dependency in stack)
            {
                if (current.Type == dependency.Type)
                {
                    stack.Push(current);

                    var path = string.Join("\n",
                        stack.Take(i + 1)
                            .Reverse()
                            .Select((item, itemIndex) => $"    [{itemIndex + 1}] {item} --> {item.Type.FullName}"));
                    
                    throw new Exception($"{current.Type}: Circular dependency detected!\n{path}");
                }
                i++;
            }

            stack.Push(current);

            for (var j = 0; j < current.ConstructorDependenciesCount; j++)
            {
                var constructorDependencyId = implementationDependencyIds[current.ConstructorDependenciesIndex + j];

                if (constructorDependencyId < 0 && current.ScopeConfigurator != null)
                {
                    continue;
                }
                
                ref var constructorDependency = ref dependencies[constructorDependencyId];

                for (var k = 0; k < constructorDependency.ImplementationsCount; k++)
                {
                    var implementationId = dependencyImplementationIds[k + constructorDependency.ImplementationsIndex];
                    
                    ThrowIfInvalid(implementations[implementationId], stack, dependencies, implementations, dependencyImplementationIds, implementationDependencyIds);
                }
            }

            stack.Pop();
        }
    }
}
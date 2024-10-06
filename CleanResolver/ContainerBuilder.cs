using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace CleanResolver
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ContainerBuilder : IScopeConfigurator
    {
        private Dependency[] _dependencies;
        private Implementation[] _implementations;
        private int[] _dependencyImplementations;

        private int _dependenciesCount;
        private int _dependencyImplementationIndex;

        public ContainerBuilder(int capacity = 4096)
        {
            _dependencies = new Dependency[capacity];
            _implementations = new Implementation[capacity];
            _dependencyImplementations = new int[capacity];
        }

        public void Register<TKey>(RegisterType registerType = RegisterType.Transient)
            where TKey : class
        {
            Register<TKey, TKey>(registerType);
        }

        public void Register<TKey, TImplementation>(RegisterType registerType = RegisterType.Transient)
            where TKey : class
            where TImplementation : class, TKey
        {
            if (TypeCompileInfo<TImplementation>.RegisterImplementation(out var implementationId, out var implementationType))
            {
                if (implementationId >= _implementations.Length)
                {
                    Array.Resize(ref _implementations, implementationId * 2);
                }

                ref var implementation = ref _implementations[implementationId];
                implementation.Type = implementationType;
                implementation.SingletonFlag = registerType == RegisterType.Singleton ? SingletonFlag.Singleton : SingletonFlag.NotSingleton;
            }

            RegisterDependency<TKey>(implementationId);
        }

        public void Register<TKey>(TKey value)
            where TKey : class
        {
            Register<TKey, TKey>(value);
        }

        public void Register<TKey, TImplementation>(TImplementation value)
            where TKey : class
            where TImplementation : class, TKey
        {
            if (TypeCompileInfo<TImplementation>.RegisterImplementation(out var implementationId, out var implementationType))
            {
                if (implementationId >= _implementations.Length)
                {
                    Array.Resize(ref _implementations, implementationId * 2);
                }

                ref var implementation = ref _implementations[implementationId];
                implementation.Type = implementationType;
                implementation.SingletonFlag = SingletonFlag.SingletonWithValue;
                implementation.SingletonValue = value;
            }

            RegisterDependency<TKey>(implementationId);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RegisterScope<TScope>(Action<IScopeConfigurator> install)
            where TScope : Scope
        {
            RegisterScope<TScope, TScope>(install);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RegisterScope<TScope, TScopeImplementation>(Action<IScopeConfigurator> install)
            where TScope : Scope
        {
            if (TypeCompileInfo<TScopeImplementation>.RegisterImplementation(out var implementationId, out var implementationType))
            {
                if (implementationId >= _implementations.Length)
                {
                    Array.Resize(ref _implementations, implementationId * 2);
                }

                ref var implementation = ref _implementations[implementationId];
                implementation.Type = implementationType;
                implementation.SingletonFlag = SingletonFlag.NotSingleton;
                implementation.ScopeConfigurator = install;
            }

            RegisterDependency<TScope>(implementationId);
        }
        
        public Container Build()
        {
            var implementationsCount = 0;
            var implementationDependenciesCount = 0;
            
            for (var j = 0; j < _implementations.Length; j++)
            {
                ref var implementation = ref _implementations[j];

                if (implementation.Type == null)
                {
                    break;
                }
                
                implementation.ConstructorInfo = implementation.Type.GetConstructors()[0];
                implementation.ConstructorParameters = implementation.ConstructorInfo.GetParameters();
                implementation.ConstructorDependenciesCount = implementation.ConstructorParameters.Length;

                implementationDependenciesCount += implementation.ConstructorDependenciesCount;
                implementationsCount++;
            }

            var dependencyReferences = new int[implementationDependenciesCount];
            var dependencyReferenceIndex = 0;
            
            for (var j = 0; j < implementationsCount; j++)
            {
                ref var implementation = ref _implementations[j];
                
                implementation.ConstructorDependenciesIndex = dependencyReferenceIndex;

                for (var i = 0; i < implementation.ConstructorDependenciesCount; i++)
                {
                    var parameterType = implementation.ConstructorParameters[i].ParameterType;

                    if (parameterType.IsArray)
                    {
                        var elementType = parameterType.GetElementType();

                        var dependencyId = TypeIdLocator.TryGetDependencyId(elementType);

                        if (dependencyId < 0)
                        {
                            dependencyId = TypeIdLocator.AddDependencyId(elementType);
                        }

                        if (dependencyId >= _dependencies.Length)
                        {
                            Array.Resize(ref dependencyReferences, dependencyId * 2);
                        }

                        ref var dependency = ref _dependencies[dependencyId];
                        dependency.Type = elementType;

                        dependencyReferences[i + dependencyReferenceIndex] = dependencyId;
                    }
                    else
                    {
                        dependencyReferences[i + dependencyReferenceIndex] = TypeIdLocator.GetDependencyId(parameterType);
                    }
                }
                
                implementation.ConstructorParameters = null;

                dependencyReferenceIndex += implementation.ConstructorDependenciesCount;
            }
            
            var circularDependencyChecker = new Stack<Implementation>();
            
            for (var i = 0; i < _dependenciesCount; i++)
            {
                ref var dependency = ref _dependencies[i];
            
                for (var j = 0; j < dependency.ImplementationsCount; j++)
                {
                    circularDependencyChecker.Clear();
                    
                    CheckCircularDependencyRecursive(_implementations[j + dependency.ImplementationsIndex], circularDependencyChecker, dependencyReferences);
                }
            }
            
            return new Container(_dependencies, _implementations, _dependencyImplementations, dependencyReferences);
        }

        private void RegisterDependency<TKey>(int implementationId)
        {
            var isNewDependency = false;
            
            if (TypeCompileInfo<TKey>.RegisterDependency(out var dependencyId, out var type))
            {
                if (dependencyId >= _dependencies.Length)
                {
                    Array.Resize(ref _dependencies, dependencyId * 2);
                }

                _dependenciesCount++;
                isNewDependency = true;
            }

            ref var dependency = ref _dependencies[dependencyId];
            dependency.Type = type;

            if (isNewDependency)
            {
                dependency.ImplementationsIndex = _dependencyImplementationIndex;
            }
            
            if (isNewDependency || _dependencyImplementationIndex ==
                     dependency.ImplementationsIndex + dependency.ImplementationsCount)
            {
                if (_dependencyImplementationIndex >= _dependencyImplementations.Length)
                {
                    Array.Resize(ref _dependencyImplementations, _dependencyImplementationIndex * 2);
                }
                
                _dependencyImplementations[_dependencyImplementationIndex] = implementationId;
            }
            else
            {
                ArrayUtility.Insert(ref _dependencyImplementations, _dependencyImplementationIndex, implementationId, dependency.ImplementationsIndex + dependency.ImplementationsCount);
                
                for (var i = dependencyId; i < _dependenciesCount; i++)
                {
                    ref var dependencyToProcess = ref _dependencies[i];
                    
                    if (i != dependencyId)
                    {
                        dependencyToProcess.ImplementationsIndex += 1;
                    }
                }
            }

            dependency.ImplementationsCount++;
            _dependencyImplementationIndex++;
        }

        private void CheckCircularDependencyRecursive(Implementation current, Stack<Implementation> stack, int[] implementationDependencyIds)
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
                ref var constructorDependency = ref _dependencies[constructorDependencyId];

                for (var k = 0; k < constructorDependency.ImplementationsCount; k++)
                {
                    CheckCircularDependencyRecursive(_implementations[k + constructorDependency.ImplementationsIndex], stack, implementationDependencyIds);
                }
            }

            stack.Pop();
        }
    }

    public struct Dependency
    {
        public Type Type;
        public int ImplementationsIndex;
        public int ImplementationsCount;
    }

    public struct Implementation
    {
        public Type Type;
        public byte SingletonFlag;
        public object SingletonValue;
        public ConstructorInfo ConstructorInfo;
        public ParameterInfo[] ConstructorParameters;
        public int ConstructorDependenciesIndex;
        public int ConstructorDependenciesCount;
        public Action<IScopeConfigurator> ScopeConfigurator;
    }
}
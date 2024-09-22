using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace CleanResolver
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ContainerBuilder
    {
        private Dependency[] _dependencies;
        private Implementation[] _implementations;
        private int[] _dependencyImplementations;

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
            if (TypeCompileInfo<TImplementation>.RegisterImplementation(out var implementationId))
            {
                if (implementationId >= _implementations.Length)
                {
                    Array.Resize(ref _implementations, implementationId * 2);
                }

                ref var implementation = ref _implementations[implementationId];
                implementation.Type = TypeCompileInfo<TImplementation>.Type;
                implementation.SingletonFlag = registerType == RegisterType.Singleton ? SingletonFlag.Singleton : SingletonFlag.NotSingleton;
            }

            var isNewDependency = false;

            if (TypeCompileInfo<TKey>.RegisterDependency(out var dependencyId))
            {
                if (dependencyId >= _dependencies.Length)
                {
                    Array.Resize(ref _dependencies, dependencyId * 2);
                }

                isNewDependency = true;
            }

            ref var dependency = ref _dependencies[dependencyId];
            dependency.Type = TypeCompileInfo<TKey>.Type;

            if (isNewDependency)
            {
                dependency.ImplementationsIndex = _dependencyImplementationIndex;

                if (_dependencyImplementationIndex >= _dependencyImplementations.Length)
                {
                    Array.Resize(ref _dependencyImplementations, _dependencyImplementationIndex * 2);
                }
                
                _dependencyImplementations[_dependencyImplementationIndex] = implementationId;
            }
            else if (_dependencyImplementationIndex ==
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
                throw new Exception(); // HANDLE CASE WHEN INSERTING IMPLEMENTATION REF IN THE MIDDLE
            }

            dependency.ImplementationsCount++;
            _dependencyImplementationIndex++;
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
            if (TypeCompileInfo<TImplementation>.RegisterImplementation(out var implementationId))
            {
                if (implementationId >= _implementations.Length)
                {
                    Array.Resize(ref _implementations, implementationId * 2);
                }

                ref var implementation = ref _implementations[implementationId];
                implementation.Type = TypeCompileInfo<TImplementation>.Type;
                implementation.SingletonFlag = SingletonFlag.SingletonWithValue;
                implementation.SingletonValue = value;
            }

            var isNewDependency = false;

            if (TypeCompileInfo<TKey>.RegisterDependency(out var dependencyId))
            {
                if (dependencyId >= _dependencies.Length)
                {
                    Array.Resize(ref _dependencies, dependencyId * 2);
                }

                isNewDependency = true;
            }

            ref var dependency = ref _dependencies[dependencyId];
            dependency.Type = TypeCompileInfo<TKey>.Type;

            if (isNewDependency)
            {
                dependency.ImplementationsIndex = _dependencyImplementationIndex;
                
                if (_dependencyImplementationIndex >= _dependencyImplementations.Length)
                {
                    Array.Resize(ref _dependencyImplementations, _dependencyImplementationIndex * 2);
                }
                
                _dependencyImplementations[_dependencyImplementationIndex] = implementationId;
            }
            else if (_dependencyImplementationIndex ==
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
                throw new Exception(); // HANDLE CASE WHEN INSERTING IMPLEMENTATION REF IN THE MIDDLE
            }

            dependency.ImplementationsCount++;
            _dependencyImplementationIndex++;
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
            
            return new Container(_dependencies, _implementations, _dependencyImplementations, dependencyReferences);
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
    }
}
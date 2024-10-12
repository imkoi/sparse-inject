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
    public class ContainerBuilder : IScopeConfigurator
    {
        private int[] _dependenciesSparse;
        private Dependency[] _dependenciesDense;

        private int _dependenciesCount;
        private int _implementationsCount;

        private int _denseCount;
        
        public ContainerBuilder(int capacity = 4096)
        {
            _dependenciesSparse = new int[capacity];
            _dependenciesDense = new Dependency[capacity];

            for (var i = 0; i < capacity; i++)
            {
                _dependenciesSparse[i] = -1;
            }
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
            RegisterDependency<TKey, TImplementation>(out var implementationIndex);

            ref var implementation = ref _dependenciesDense[implementationIndex];
            implementation.SingletonFlag = registerType == RegisterType.Singleton 
                ? SingletonFlag.Singleton 
                : SingletonFlag.NotSingleton;
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
            RegisterDependency<TKey, TImplementation>(out var implementationIndex);
            
            ref var implementation = ref _dependenciesDense[implementationIndex];
            implementation.SingletonFlag = SingletonFlag.SingletonWithValue;
            implementation.SingletonValue = value;
        }

        public void RegisterScope<TScope>(Action<IScopeConfigurator> install)
            where TScope : Scope
        {
            RegisterScope<TScope, TScope>(install);
        }

        public void RegisterScope<TScope, TScopeImplementation>(Action<IScopeConfigurator> install)
            where TScope : Scope
            where TScopeImplementation : class, TScope
        {
            RegisterDependency<TScope, TScopeImplementation>(out var implementationIndex);
            
            ref var implementation = ref _dependenciesDense[implementationIndex];
            implementation.SingletonFlag = SingletonFlag.NotSingleton;
            implementation.ScopeConfigurator = install;
        }
        
        public Container Build()
        {
            return BuildInternal(null);
        }

        internal Container BuildInternal(Container parentContainer)
        {
            var stats = BuildPrecomputeDependenciesCount();
            var implementationDependencyIds = BuildBakeImplementationDependencyIds(
                stats.implementationDependenciesCount,
                stats.implementationConstructorParameters);
            
            CircularDependencyValidator.ThrowIfInvalid(_implementationsCount, _dependenciesDense,
                _dependenciesSparse, implementationDependencyIds);
            
            return new Container(
                parentContainer,
                _dependenciesDense, 
                _dependenciesSparse,
                implementationDependencyIds,
                stats.maxConstructorLength);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RegisterDependency<TKey, TImplementation>(out int implementationIndex)
            where TKey : class
            where TImplementation : class
        {
            var dependencyId = TypeCompileInfo<TKey>.GetId(out var dependencyType);
            var implementationType = TypeCompileInfo<TImplementation>.Type;

            if (dependencyId >= _dependenciesSparse.Length)
            {
                var oldSize = _dependenciesSparse.Length;
                var newSize = dependencyId * 2;

                Array.Resize(ref _dependenciesSparse, newSize);
                
                for (var i = oldSize; i < newSize; i++)
                {
                    _dependenciesSparse[i] = -1;
                }
            }

            ref var dependencyIndex = ref _dependenciesSparse[dependencyId];

            if (dependencyIndex < 0)
            {
                var sizeWithImplementation = _denseCount + 2;
                
                if (sizeWithImplementation >= _dependenciesDense.Length)
                {
                    Array.Resize(ref _dependenciesDense, sizeWithImplementation * 2);
                }

                _dependenciesCount++;

                dependencyIndex = _denseCount;

                _denseCount += 2;
            }
            else
            {
                if (dependencyIndex >= _dependenciesDense.Length)
                {
                    Array.Resize(ref _dependenciesDense, dependencyIndex * 2);
                }
                
                _denseCount += 1;
            }
            
            ref var dependency = ref _dependenciesDense[dependencyIndex];
            dependency.Type = dependencyType;

            implementationIndex = dependencyIndex + dependency.ImplementationsCount + 1;
            ref var implementation = ref _dependenciesDense[implementationIndex];

            if (implementation.Type == null)
            {
                implementation.Type = implementationType;
            }
            else
            {
                // for (var i = dependencyId; i < _dependenciesCount; i++)
                // {
                //     ref var dependencyToProcess = ref _dependenciesDense[i];
                //     
                //     if (i != dependencyId)
                //     {
                //         dependencyToProcess.ImplementationsIndex += 1;
                //     }
                // }
                
                Array.Copy(_dependenciesDense, implementationIndex, _dependenciesDense, implementationIndex + 1, _denseCount - implementationIndex);

                _dependenciesDense[implementationIndex].Type = implementationType;
            }

            dependency.ImplementationsCount++;
            _implementationsCount++;
        }

        private (int implementationDependenciesCount, ParameterInfo[][] implementationConstructorParameters, int maxConstructorLength) BuildPrecomputeDependenciesCount()
        {
            var implementationConstructorParameters = new ParameterInfo[_implementationsCount][];
            var implementationDependenciesCount = 0;
            var index = 0;
            var implementationIndex = 0;

            var maxConstructorLength = int.MinValue;
            
            while (implementationIndex < _implementationsCount)
            {
                ref var dependency = ref _dependenciesDense[index];
                index++;
                
                for (var i = 0; i < dependency.ImplementationsCount; i++)
                {
                    ref var implementation = ref _dependenciesDense[index + i];

                    implementation.ConstructorInfo = implementation.Type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)[0];
                    var constructorParameters = implementation.ConstructorInfo.GetParameters();
                    implementationConstructorParameters[implementationIndex] = constructorParameters;
                    
                    implementation.ConstructorDependenciesCount = constructorParameters.Length;

                    if (implementation.ConstructorDependenciesCount > maxConstructorLength)
                    {
                        maxConstructorLength = implementation.ConstructorDependenciesCount;
                    }

                    implementationDependenciesCount += implementation.ConstructorDependenciesCount;
                    implementationIndex++;
                }

                index += dependency.ImplementationsCount;
            }

            return (implementationDependenciesCount, implementationConstructorParameters, maxConstructorLength);
        }

        private int[] BuildBakeImplementationDependencyIds(int implementationDependenciesCount, ParameterInfo[][] implementationConstructorParameters)
        {
            var dependencyReferences = new int[implementationDependenciesCount];
            var dependencyReferenceIndex = 0;
            
            var index = 0;
            var implementationIndex = 0;
            
            while (implementationIndex < _implementationsCount)
            {
                ref var dependency = ref _dependenciesDense[index];
                index++;
                
                for (var i = 0; i < dependency.ImplementationsCount; i++)
                {
                    ref var implementation = ref _dependenciesDense[index + i];

                    implementation.ConstructorDependenciesIndex = dependencyReferenceIndex;
                    
                    for (var j = 0; j < implementation.ConstructorDependenciesCount; j++)
                    {
                        var parameterType = implementationConstructorParameters[implementationIndex][j].ParameterType;

                        if (parameterType.IsArray)
                        {
                            var elementType = parameterType.GetElementType();

                            var constructorDependencyId = TypeIdLocator.TryGetDependencyId(elementType);
                            
                            if (constructorDependencyId < 0)
                            {
                                constructorDependencyId = TypeIdLocator.AddDependencyId(parameterType);
                            }
                            
                            dependencyReferences[j + dependencyReferenceIndex] = constructorDependencyId;
                        }
                        else
                        {
                            var constructorDependencyId = TypeIdLocator.TryGetDependencyId(parameterType);

                            if (constructorDependencyId < 0)
                            {
                                constructorDependencyId = TypeIdLocator.AddDependencyId(parameterType);
                            }
                            
                            dependencyReferences[j + dependencyReferenceIndex] = constructorDependencyId;
                        }
                    }
                    
                    dependencyReferenceIndex += implementation.ConstructorDependenciesCount;
                    implementationIndex++;
                }

                index += dependency.ImplementationsCount;
            }
            
            return dependencyReferences;
        }
    }

    public struct Dependency
    {
        public Type Type;
        public int ImplementationsCount;
        public int ConstructorDependenciesIndex;
        public int ConstructorDependenciesCount;
        public ConstructorInfo ConstructorInfo;
        public int SingletonFlag;
        public object SingletonValue;
        public Action<IScopeConfigurator> ScopeConfigurator;
    }
}
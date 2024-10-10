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
            where TScopeImplementation : TScope
        {
            install += configurator =>
            {
                configurator.Register<TScope, TScopeImplementation>();
            };

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
            var (implementationDependenciesCount, implementationConstructorParameters) = BuildPrecomputeDependenciesCount();
            var implementationDependencyIds = BuildBakeImplementationDependencyIds(implementationDependenciesCount, implementationConstructorParameters);
            
            BuildThrowIfCircularDependencyExist(implementationDependencyIds);
            
            return new Container(
                parentContainer,
                _dependenciesDense, 
                _dependenciesSparse,
                implementationDependencyIds,
                _dependenciesCount,
                _implementationsCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RegisterDependency<TKey, TImplementation>(out int implementationIndex)
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

            implementationIndex = dependencyIndex + 1;

            if (dependency.ImplementationsCount == 0 || false)
            {
                _dependenciesDense[implementationIndex].Type = implementationType;
            }
            else
            {
                // ArrayUtility.Insert(ref _dependencyImplementationIds, _dependencyImplementationIndex, implementationId, dependency.ImplementationsIndex + dependency.ImplementationsCount);
                //
                // for (var i = dependencyId; i < _dependenciesCount; i++)
                // {
                //     ref var dependencyToProcess = ref _dependenciesDense[i];
                //     
                //     if (i != dependencyId)
                //     {
                //         dependencyToProcess.ImplementationsIndex += 1;
                //     }
                // }

                throw new NotImplementedException();
            }

            dependency.ImplementationsCount++;
            _implementationsCount++;
        }

        private (int implementationDependenciesCount, ParameterInfo[][] implementationConstructorParameters) BuildPrecomputeDependenciesCount()
        {
            var implementationConstructorParameters = new ParameterInfo[_implementationsCount][];
            var implementationDependenciesCount = 0;
            var index = 0;
            var implementationIndex = 0;
            
            while (implementationIndex < _implementationsCount)
            {
                ref var dependency = ref _dependenciesDense[index];
                index++;
                
                for (var i = 0; i < dependency.ImplementationsCount; i++)
                {
                    ref var implementation = ref _dependenciesDense[index + i];

                    implementation.ConstructorInfo = implementation.Type.GetConstructors()[0];
                    
                    var constructorParameters = implementation.ConstructorInfo.GetParameters();
                    implementationConstructorParameters[implementationIndex] = constructorParameters;
                    
                    implementation.ConstructorDependenciesCount = constructorParameters.Length;

                    implementationDependenciesCount += implementation.ConstructorDependenciesCount;
                    implementationIndex++;
                }

                index += dependency.ImplementationsCount;
            }

            return (implementationDependenciesCount, implementationConstructorParameters);
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
                                constructorDependencyId = TypeIdLocator.AddDependencyId(elementType);
                            }

                            if (constructorDependencyId >= _dependenciesDense.Length)
                            {
                                Array.Resize(ref dependencyReferences, constructorDependencyId * 2);
                            }

                            ref var constructorDependency = ref _dependenciesDense[constructorDependencyId]; 
                            constructorDependency.Type = elementType;

                            dependencyReferences[j + dependencyReferenceIndex] = constructorDependencyId;
                        }
                        else
                        {
                            dependencyReferences[j + dependencyReferenceIndex] = TypeIdLocator.TryGetDependencyId(parameterType);
                        }
                    }
                    
                    dependencyReferenceIndex += implementation.ConstructorDependenciesCount;
                    implementationIndex++;
                }

                index += dependency.ImplementationsCount;
            }
            
            return dependencyReferences;
        }

        private void BuildThrowIfCircularDependencyExist(int[] implementationDependencyIds)
        {
            var circularDependencyChecker = new Stack<Dependency>();
            
            var index = 0;
            var implementationIndex = 0;
            
            while (implementationIndex < _implementationsCount)
            {
                ref var dependency = ref _dependenciesDense[index];
                index++;
                
                for (var i = 0; i < dependency.ImplementationsCount; i++)
                {
                    circularDependencyChecker.Clear();
                    
                    CircularDependencyValidator.ThrowIfInvalid(index + i + 1,
                        circularDependencyChecker, _dependenciesDense, implementationDependencyIds);

                    implementationIndex++;
                }

                index += dependency.ImplementationsCount;
            }
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
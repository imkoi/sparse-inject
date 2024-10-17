using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Unity.IL2CPP.CompilerServices;

namespace SparseInject
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ContainerBuilder : IScopeBuilder
    {
        private int[] _sparse;
        private Dependency[] _dense;

        private int _dependenciesCount;
        private int _implementationsCount;

        private int _denseCount;
        private int _lastSparseIndex;
        
        public ContainerBuilder(int capacity = 4096)
        {
            _sparse = new int[capacity];
            _dense = new Dependency[capacity];

            for (var i = 0; i < capacity; i++)
            {
                _sparse[i] = -1;
            }
        }
        
        public void Register(Action<IScopeBuilder> registerMethod)
        {
            registerMethod.Invoke(this);
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

            ref var implementation = ref _dense[implementationIndex];
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
            
            ref var implementation = ref _dense[implementationIndex];
            implementation.SingletonFlag = SingletonFlag.SingletonWithValue;
            implementation.SingletonValue = value;
        }
        
        public void RegisterScope<TScope>(Action<IScopeBuilder> install)
            where TScope : Scope
        {
            RegisterScope<TScope, TScope>(install);
        }

        public void RegisterScope<TScope, TScopeImplementation>(Action<IScopeBuilder> install)
            where TScope : class, IDisposable
            where TScopeImplementation : Scope
        {
            RegisterScope<TScope, TScopeImplementation>((builder, parentScope) =>
            {
                install(builder);
            });
        }

        public void RegisterScope<TScope>(Action<IScopeBuilder, IScopeResolver> install)
            where TScope : Scope
        {
            RegisterScope<TScope, TScope>(install);
        }

        public void RegisterScope<TScope, TScopeImplementation>(Action<IScopeBuilder, IScopeResolver> install)
            where TScope : class, IDisposable
            where TScopeImplementation : Scope
        {
            RegisterDependency<TScope, TScopeImplementation>(out var implementationIndex);
            
            ref var implementation = ref _dense[implementationIndex];
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
                stats.implementationConstructorParameterInfos,
                stats.implementationConstructorParameters,
                stats.maxConstructorLength);
            
            CircularDependencyValidator.ThrowIfInvalid(_implementationsCount, _dense,
                _sparse, implementationDependencyIds);
            
            return new Container(
                parentContainer,
                _dense, 
                _sparse,
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

            if (dependencyId > _lastSparseIndex)
            {
                _lastSparseIndex = dependencyId;
            }

            if (dependencyId >= _sparse.Length)
            {
                var oldSize = _sparse.Length;
                var newSize = dependencyId * 2;

                Array.Resize(ref _sparse, newSize);
                
                for (var i = oldSize; i < newSize; i++)
                {
                    _sparse[i] = -1;
                }
            }

            ref var dependencyIndex = ref _sparse[dependencyId];

            if (dependencyIndex < 0)
            {
                var sizeWithImplementation = _denseCount + 2;
                
                if (sizeWithImplementation > _dense.Length)
                {
                    Array.Resize(ref _dense, sizeWithImplementation * 2);
                }

                _dependenciesCount++;

                dependencyIndex = _denseCount;

                _denseCount += 2;
            }
            else
            {
                var requiredIndex = dependencyIndex + 1;
                
                if (requiredIndex >= _dense.Length)
                {
                    Array.Resize(ref _dense, requiredIndex * 2);
                }
                
                _denseCount += 1;
            }
            
            ref var dependency = ref _dense[dependencyIndex];
            dependency.Type = dependencyType;

            implementationIndex = dependencyIndex + dependency.ImplementationsCount + 1;
            ref var implementation = ref _dense[implementationIndex];

            if (implementation.Type == null)
            {
                implementation.Type = implementationType;
            }
            else
            {
                Array.Copy(_dense, implementationIndex, _dense, implementationIndex + 1, _denseCount - implementationIndex);

                _dense[implementationIndex].Type = implementationType;

                for (var i = 0; i < _lastSparseIndex + 1; i++)
                {
                    if (_sparse[i] > dependencyId)
                    {
                        _sparse[i] += 1;
                    }
                }
            }

            dependency.ImplementationsCount++;
            _implementationsCount++;
        }

        private (int implementationDependenciesCount, ParameterInfo[][] implementationConstructorParameterInfos, Type[][] implementationConstructorParameters, int maxConstructorLength) BuildPrecomputeDependenciesCount()
        {
            var implementationConstructorParameterInfos = new ParameterInfo[_implementationsCount][];
            var implementationConstructorParameters = new Type[_implementationsCount][];
            var implementationDependenciesCount = 0;
            var index = 0;
            var implementationIndex = 0;

            var maxConstructorLength = int.MinValue;
            
            while (implementationIndex < _implementationsCount)
            {
                ref var dependency = ref _dense[index];
                index++;
                
                for (var i = 0; i < dependency.ImplementationsCount; i++)
                {
                    ref var implementation = ref _dense[index + i];
                    
                    var constructorParametersCount = 0;
                    
                    if (ReflectionBakingProviderCache.TryGetInstanceFactory(implementation.Type, out var factory, out var constructorParametersSpan))
                    {
                        constructorParametersCount = factory.ConstructorParametersCount;

                        implementationConstructorParameters[implementationIndex] = constructorParametersSpan;
                        implementation.GeneratedInstanceFactory = factory;
                    }
                    else
                    {
                        implementation.ConstructorInfo = implementation.Type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)[0];
                        var constructorParameters = implementation.ConstructorInfo.GetParameters();

                        constructorParametersCount = constructorParameters.Length;
                        
                        implementationConstructorParameterInfos[implementationIndex] = constructorParameters;
                    }
                    
                    implementation.ConstructorDependenciesCount = constructorParametersCount;

                    if (constructorParametersCount > maxConstructorLength)
                    {
                        maxConstructorLength = constructorParametersCount;
                    }

                    implementationDependenciesCount += constructorParametersCount;
                    implementationIndex++;
                }

                index += dependency.ImplementationsCount;
            }

            return (implementationDependenciesCount, implementationConstructorParameterInfos, implementationConstructorParameters, maxConstructorLength);
        }

        private int[] BuildBakeImplementationDependencyIds(int implementationDependenciesCount, ParameterInfo[][] implementationConstructorParameterInfos, Type[][] implementationConstructorParameters, int maxConstructorLength)
        {
            var generatedInstanceFactoryDependencyIds = new int[maxConstructorLength];
            
            var dependencyReferences = new int[implementationDependenciesCount];
            var dependencyReferenceIndex = 0;
            
            var index = 0;
            var implementationIndex = 0;
            
            while (implementationIndex < _implementationsCount)
            {
                ref var dependency = ref _dense[index];
                index++;
                
                for (var i = 0; i < dependency.ImplementationsCount; i++)
                {
                    ref var implementation = ref _dense[index + i];
                    
                    implementation.ConstructorDependenciesIndex = dependencyReferenceIndex;
                    
                    for (var j = 0; j < implementation.ConstructorDependenciesCount; j++)
                    {
                        var parameterType = default(Type);
                        
                        if (implementationConstructorParameters[implementationIndex] != null)
                        {
                            parameterType = implementationConstructorParameters[implementationIndex][implementation.GeneratedInstanceFactory.ConstructorParametersIndex + j];
                        }
                        else
                        {
                            parameterType = implementationConstructorParameterInfos[implementationIndex][j]
                                .ParameterType;
                        }

                        if (parameterType.IsArray)
                        {
                            var elementType = parameterType.GetElementType();

                            var constructorDependencyId = TypeIdLocator.TryGetDependencyId(elementType);
                            
                            if (constructorDependencyId < 0)
                            {
                                constructorDependencyId = TypeIdLocator.AddDependencyId(parameterType);
                            }
                            
                            dependencyReferences[j + dependencyReferenceIndex] = constructorDependencyId;
                            generatedInstanceFactoryDependencyIds[j] = constructorDependencyId;
                        }
                        else
                        {
                            var constructorDependencyId = TypeIdLocator.TryGetDependencyId(parameterType);

                            if (constructorDependencyId < 0)
                            {
                                constructorDependencyId = TypeIdLocator.AddDependencyId(parameterType);
                            }
                            
                            dependencyReferences[j + dependencyReferenceIndex] = constructorDependencyId;
                            generatedInstanceFactoryDependencyIds[j] = constructorDependencyId;
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
        public InstanceFactoryBase GeneratedInstanceFactory;
        public int SingletonFlag;
        public object SingletonValue;
        public Action<IScopeBuilder, IScopeResolver> ScopeConfigurator;
    }
}
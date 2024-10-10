using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace CleanResolver
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Container
    {
        private readonly Container _parentContainer;

        private readonly Dependency[] _dependencies;
        private readonly int[] _dependencySparse;
        private readonly int[] _dependencyReferences;
        private readonly int _dependenciesOffset;
        private readonly int _implementationsOffset;

        internal Container(
            Container parentContainer,
            Dependency[] dependenciesDense,
            int[] dependencySparse,
            int[] dependencyReferences,
            int dependenciesOffset,
            int implementationsOffset)
        {
            _parentContainer = parentContainer;
            _dependencies = dependenciesDense;
            _dependencySparse = dependencySparse;
            _dependencyReferences = dependencyReferences;
            _dependenciesOffset = dependenciesOffset;
            _implementationsOffset = implementationsOffset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Resolve<T>() where T : class
        {
            var dependencyId = TypeCompileInfo<T>.GetRuntimeDependencyId();

            return (T)Resolve(dependencyId);
        }

        private object Resolve(int dependencyId)
        {
            dependencyId = _dependencySparse[dependencyId];
            ref var dependency = ref _dependencies[dependencyId];
            var instances = dependency.ImplementationsCount == 1
                ? null
                : Array.CreateInstance(dependency.Type, dependency.ImplementationsCount);

            dependencyId++;

            for (var i = 0; i < dependency.ImplementationsCount; i++)
            {
                ref var implementation = ref _dependencies[dependencyId + i];

                if (implementation.SingletonFlag == SingletonFlag.SingletonWithValue)
                {
                    return implementation.SingletonValue;
                }

                var reserved = ArrayCache<object>.PullReserved(implementation.ConstructorDependenciesCount);

                if (implementation.ScopeConfigurator != null)
                {
                    // var containerBuilder = new ContainerBuilder(64, _dependenciesOffset, _implementationsOffset);
                    //
                    // implementation.ScopeConfigurator.Invoke(containerBuilder);
                    //
                    // var container = containerBuilder.BuildInternal(this);
                    //
                    // for (var j = 0; j < implementation.ConstructorDependenciesCount; j++)
                    // {
                    //     var constructorDependencyId = _dependencyReferences[j + implementation.ConstructorDependenciesIndex];
                    //
                    //     if (constructorDependencyId < 0)
                    //     {
                    //         constructorDependencyId = container._dependenciesOffset;
                    //         _dependencyReferences[j + implementation.ConstructorDependenciesIndex] = constructorDependencyId;
                    //         
                    //         reserved.Array[j + reserved.StartIndex] = container.Resolve(constructorDependencyId);
                    //     }
                    //     
                    //     reserved.Array[j + reserved.StartIndex] = Resolve(constructorDependencyId);
                    // }
                }
                else
                {
                    for (var j = 0; j < implementation.ConstructorDependenciesCount; j++)
                    {
                        reserved.Array[j + reserved.StartIndex] = Resolve(_dependencyReferences[j + implementation.ConstructorDependenciesIndex]);
                    }
                }

                var constructorParameters = ArrayCache<object>.Pull(implementation.ConstructorDependenciesCount);

                for (var j = 0; j < implementation.ConstructorDependenciesCount; j++)
                {
                    constructorParameters[j] = reserved.Array[j + reserved.StartIndex];
                }
                // TODO: compare raw copy vs Array.Copy
                // Array.Copy(reserved.Array, reserved.StartIndex, constructorParameters, 0,
                //     implementation.ConstructorDependenciesCount);

                var instance = implementation.ConstructorInfo.Invoke(BindingFlags.Default, binder: null,
                    parameters: constructorParameters, culture: null);

                ArrayCache<object>.PushReserved(ref reserved);
                ArrayCache<object>.Push(constructorParameters);

                if (implementation.SingletonFlag == SingletonFlag.Singleton)
                {
                    implementation.SingletonValue = instance;
                    implementation.SingletonFlag = SingletonFlag.SingletonWithValue;
                }

                if (dependency.ImplementationsCount == 1)
                {
                    return instance;
                }

                instances.SetValue(instance, i);
            }

            return instances;
        }
        
        private object ResolveOptimized(int temp)
        {
            temp = _dependencySparse[temp];
            var deps = _dependencies;
            ref var dependency = ref deps[temp];
            var instances = dependency.ImplementationsCount == 1
                ? null
                : Array.CreateInstance(dependency.Type, dependency.ImplementationsCount);

            temp++;

            for (var i = 0; i < dependency.ImplementationsCount; i++)
            {
                ref var implementation = ref deps[temp + i];
                temp = implementation.ConstructorDependenciesCount;

                if (implementation.SingletonFlag == SingletonFlag.SingletonWithValue)
                {
                    return implementation.SingletonValue;
                }

                var reserved = ArrayCache<object>.PullReserved(temp);

                if (implementation.ScopeConfigurator != null)
                {
                    // var containerBuilder = new ContainerBuilder(64, _dependenciesOffset, _implementationsOffset);
                    //
                    // implementation.ScopeConfigurator.Invoke(containerBuilder);
                    //
                    // var container = containerBuilder.BuildInternal(this);
                    //
                    // for (var j = 0; j < implementation.ConstructorDependenciesCount; j++)
                    // {
                    //     var constructorDependencyId = _dependencyReferences[j + implementation.ConstructorDependenciesIndex];
                    //
                    //     if (constructorDependencyId < 0)
                    //     {
                    //         constructorDependencyId = container._dependenciesOffset;
                    //         _dependencyReferences[j + implementation.ConstructorDependenciesIndex] = constructorDependencyId;
                    //         
                    //         reserved.Array[j + reserved.StartIndex] = container.Resolve(constructorDependencyId);
                    //     }
                    //     
                    //     reserved.Array[j + reserved.StartIndex] = Resolve(constructorDependencyId);
                    // }
                }
                else
                {
                    var depRefs = _dependencyReferences;
                    
                    for (var j = 0; j < temp; j++)
                    {
                        reserved.Array[j + reserved.StartIndex] = ResolveOptimized(depRefs[j + implementation.ConstructorDependenciesIndex]);
                    }
                }

                var constructorParameters = ArrayCache<object>.Pull(temp);

                for (var j = 0; j < temp; j++)
                {
                    constructorParameters[j] = reserved.Array[j + reserved.StartIndex];
                }

                var instance = implementation.ConstructorInfo.Invoke(BindingFlags.Default, binder: null,
                    parameters: constructorParameters, culture: null);

                ArrayCache<object>.PushReserved(ref reserved);
                ArrayCache<object>.Push(constructorParameters);

                if (implementation.SingletonFlag == SingletonFlag.Singleton)
                {
                    implementation.SingletonValue = instance;
                    implementation.SingletonFlag = SingletonFlag.SingletonWithValue;
                }

                if (dependency.ImplementationsCount == 1)
                {
                    return instance;
                }

                instances.SetValue(instance, i);
            }

            return instances;
        }

        internal bool HasDependency(int dependencyId)
        {
            return _dependencies[dependencyId].Type != null;
        }
    }
}
using System;
using Unity.IL2CPP.CompilerServices;

namespace SparseInject
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public partial class ContainerBuilder
    {
        public void Register(Action<IScopeBuilder> registerMethod)
        {
            registerMethod.Invoke(this);
        }

        public void Register<TConcreteContract>(Lifetime lifetime = Lifetime.Transient)
            where TConcreteContract : class
        {
            Register<TConcreteContract, TConcreteContract>(lifetime);
        }

        public void Register<TContract, TConcrete>(Lifetime lifetime = Lifetime.Transient)
            where TContract : class
            where TConcrete : class, TContract
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);

            AddContract<TContract>(index);

            if (lifetime == Lifetime.Singleton)
            {
                concrete.MarkSingleton();
            }
        }
        
        public void Register<TContract0, TContract1, TConcrete>(Lifetime lifetime = Lifetime.Transient)
            where TContract0 : class
            where TContract1 : class
            where TConcrete : class, TContract0, TContract1
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);

            AddContract<TContract0>(index);
            AddContract<TContract1>(index);

            if (lifetime == Lifetime.Singleton)
            {
                concrete.MarkSingleton();
            }
        }
        
        public void Register<TContract0, TContract1, TContract2, TConcrete>(Lifetime lifetime = Lifetime.Transient)
            where TContract0 : class
            where TContract1 : class
            where TContract2 : class
            where TConcrete : class, TContract0, TContract1, TContract2
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);
            
            AddContract<TContract0>(index);
            AddContract<TContract1>(index);
            AddContract<TContract2>(index);
            
            if (lifetime == Lifetime.Singleton)
            {
                concrete.MarkSingleton();
            }
        }

        public void RegisterValue<TConcreteContract>(TConcreteContract value)
            where TConcreteContract : class
        {
            ref var concrete = ref AddConcrete(typeof(TConcreteContract), out var index);
  
            AddContract<TConcreteContract>(index);
            
            concrete.MarkSingleton();
            concrete.MarkValue();
            concrete.Value = value;
        }

        public void RegisterValue<TContract, TConcrete>(TConcrete value)
            where TContract : class
            where TConcrete : class, TContract
        {
#if DEBUG
            if (typeof(TConcrete) != value.GetType())
            {
                throw new SparseInjectException($"To register value of type '{value.GetType().Name}' " +
                                                $"use 'RegisterValue<{typeof(TContract).Name}, {typeof(TConcrete).Name}, {value.GetType().Name}>(value)' method signature");
            }
#endif
            
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);

            AddContract<TContract>(index);
            
            concrete.MarkSingleton();
            concrete.MarkValue();
            concrete.Value = value;
        }
        
        public void RegisterValue<TContract0, TContract1, TConcrete>(TConcrete value)
            where TContract0 : class
            where TContract1 : class
            where TConcrete : class, TContract0, TContract1
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);

            AddContract<TContract0>(index);
            AddContract<TContract1>(index);

            concrete.MarkSingleton();
            concrete.MarkValue();
            concrete.Value = value;
        }
        
        public void RegisterValue<TContract0, TContract1, TContract2, TConcrete>(TConcrete value)
            where TContract0 : class
            where TContract1 : class
            where TContract2 : class
            where TConcrete : class, TContract0, TContract1, TContract2
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);

            AddContract<TContract0>(index);
            AddContract<TContract1>(index);
            AddContract<TContract2>(index);

            concrete.MarkSingleton();
            concrete.MarkValue();
            concrete.Value = value;
        }
        
        public void RegisterFactory<T>(Func<T> factory)
            where T : class
        {
            RegisterFactory<T, T>(factory);
        }
        
        public void RegisterFactory<TContract, TConcrete>(Func<TContract> factory)
            where TConcrete : class, TContract
        {
            RegisterFactory<TContract, TConcrete>(container => factory);
        }
        
        public void RegisterFactory<T>(Func<IScopeResolver, T> factory)
            where T : class
        {
            RegisterFactory<T, T>(container =>
            {
                return () => factory.Invoke(container);
            });
        }
        
        public void RegisterFactory<TContract, TConcrete>(Func<IScopeResolver, TConcrete> factory)
            where TConcrete : class, TContract
        {
            RegisterFactory<TContract, TConcrete>(container =>
            {
                return () => factory.Invoke(container);
            });
        }
        
        public void RegisterFactory<TContract, TConcrete>(Func<IScopeResolver, Func<TContract>> factory)
            where TConcrete : class, TContract
        {
            ref var concrete = ref AddConcrete(typeof(Func<TConcrete>), out var index);

            AddContract<Func<TContract>>(index);

            concrete.MarkFactory(true);
            concrete.MarkSingleton();
            concrete.MarkValue();
            concrete.Value = factory;
        }
        
        public void RegisterFactory<TParameter, T>(Func<TParameter, T> factory) where T : class
        {
            RegisterFactory<TParameter, T, T>(resolver => factory);
        }
        
        public void RegisterFactory<TParameter, T>(Func<IScopeResolver, Func<TParameter, T>> factory) where T : class
        {
            RegisterFactory<TParameter, T, T>(factory);
        }
        
        public void RegisterFactory<TParameter, TContract, TConcrete>(Func<TParameter, TContract> factory) where TConcrete : class, TContract
        {
            RegisterFactory<TParameter, TContract, TConcrete>(resolver => factory);
        }
        
        public void RegisterFactory<TParameter, TContract, TConcrete>(Func<IScopeResolver, Func<TParameter, TContract>> factory) where TConcrete : class, TContract
        {
            ref var concrete = ref AddConcrete(typeof(Func<TParameter, TConcrete>), out var index);

            AddContract<Func<TParameter, TContract>>(index);

            concrete.MarkFactory(true);
            concrete.MarkSingleton();
            concrete.MarkValue();
            concrete.Value = factory;
        }

        public void RegisterScope<TScopeConcreteContract>(Action<IScopeBuilder> install)
            where TScopeConcreteContract : Scope
        {
            RegisterScope<TScopeConcreteContract, TScopeConcreteContract>(install);
        }

        public void RegisterScope<TScopeContract, TScopeConcrete>(Action<IScopeBuilder> install)
            where TScopeContract : class, IDisposable
            where TScopeConcrete : Scope, TScopeContract
        {
            RegisterScope<TScopeContract, TScopeConcrete>((builder, parentScope) =>
            {
                install(builder);
            });
        }

        public void RegisterScope<TScopeConcreteContract>(Action<IScopeBuilder, IScopeResolver> install)
            where TScopeConcreteContract : Scope
        {
            RegisterScope<TScopeConcreteContract, TScopeConcreteContract>(install);
        }

        public void RegisterScope<TScopeContract, TScopeConcrete>(Action<IScopeBuilder, IScopeResolver> install)
            where TScopeContract : class, IDisposable
            where TScopeConcrete : Scope, TScopeContract
        {
            ref var concrete = ref AddConcrete(typeof(TScopeConcrete), out var index);

            AddContract<TScopeContract>(index);

            concrete.MarkScope();
            concrete.Value = install;
        }
    }
}
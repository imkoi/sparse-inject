using System;

namespace SparseInject
{
#if UNITY_2017_1_OR_NEWER
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public partial class ContainerBuilder
    {
        public void RegisterFactory<T>(Func<T> factory)
            where T : class
        {
            RegisterFactory<T, T>(factory);
        }
        
        public void RegisterFactory<TContract, TConcrete>(Func<TContract> factory)
            where TConcrete : class, TContract
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            
            RegisterFactory<TContract, TConcrete>(container => factory);
        }
        
        public void RegisterFactory<T>(Func<IScopeResolver, T> factory)
            where T : class
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            
            RegisterFactory<T, T>(container =>
            {
                return () => factory.Invoke(container);
            });
        }
        
        public void RegisterFactory<TContract, TConcrete>(Func<IScopeResolver, TConcrete> factory)
            where TConcrete : class, TContract
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            
            RegisterFactory<TContract, TConcrete>(container =>
            {
                return () => factory.Invoke(container);
            });
        }
        
        public void RegisterFactory<TContract, TConcrete>(Func<IScopeResolver, Func<TContract>> factory)
            where TConcrete : class, TContract
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            
            ref var concrete = ref AddConcrete(typeof(Func<TConcrete>), out var index);

            AddContract(typeof(Func<TContract>), typeof(Func<TContract>[]), index);

            concrete.MarkFactory(true);
            concrete.MarkSingleton();
            concrete.MarkValue();
            concrete.Value = factory;
        }
        
        public void RegisterFactory<TParameter, T>(Func<TParameter, T> factory) where T : class
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            
            RegisterFactory<TParameter, T, T>(resolver => factory);
        }
        
        public void RegisterFactory<TParameter, T>(Func<IScopeResolver, Func<TParameter, T>> factory) where T : class
        {
            RegisterFactory<TParameter, T, T>(factory);
        }
        
        public void RegisterFactory<TParameter, TContract, TConcrete>(Func<TParameter, TContract> factory) where TConcrete : class, TContract
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            
            RegisterFactory<TParameter, TContract, TConcrete>(resolver => factory);
        }
        
        public void RegisterFactory<TParameter, TContract, TConcrete>(Func<IScopeResolver, Func<TParameter, TContract>> factory) where TConcrete : class, TContract
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            
            ref var concrete = ref AddConcrete(typeof(Func<TParameter, TConcrete>), out var index);

            AddContract(typeof(Func<TParameter, TContract>), typeof(Func<TParameter, TContract>[]), index);

            concrete.MarkFactory(true);
            concrete.MarkSingleton();
            concrete.MarkValue();
            concrete.Value = factory;
        }
    }
}
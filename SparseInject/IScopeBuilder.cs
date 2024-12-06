using System;

namespace SparseInject
{
    public interface IScopeBuilder
    {
        void Register(Action<IScopeBuilder> registerMethod);
        
        public void Register<TKey>(Lifetime lifetime = Lifetime.Transient)
            where TKey : class;
        public void Register<TKey, TImplementation>(Lifetime lifetime = Lifetime.Transient)
            where TKey : class
            where TImplementation : class, TKey;
        public void Register<TContract0, TContract1, TConcrete>(Lifetime lifetime = Lifetime.Transient)
            where TContract0 : class
            where TContract1 : class
            where TConcrete : class, TContract0, TContract1;
        public void Register<TContract0, TContract1, TContract2, TConcrete>(Lifetime lifetime = Lifetime.Transient)
            where TContract0 : class
            where TContract1 : class
            where TContract2 : class
            where TConcrete : class, TContract0, TContract1, TContract2;
        
        public void RegisterValue<TContract>(TContract value)
            where TContract : class;
        public void RegisterValue<TContract, TImplementation>(TImplementation value)
            where TContract : class
            where TImplementation : class, TContract;
        public void RegisterValue<TContract0, TContract1, TConcrete>(TConcrete value)
            where TContract0 : class
            where TContract1 : class
            where TConcrete : class, TContract0, TContract1;
        public void RegisterValue<TContract0, TContract1, TContract2, TConcrete>(TConcrete value)
            where TContract0 : class
            where TContract1 : class
            where TContract2 : class
            where TConcrete : class, TContract0, TContract1, TContract2;

        public void RegisterFactory<T>(Func<T> factory)
            where T : class;
        public void RegisterFactory<T>(Func<IScopeResolver, T> factory)
            where T : class;
        public void RegisterFactory<TParameter, T>(Func<IScopeResolver, Func<TParameter, T>> factory)
            where T : class;
        public void RegisterFactory<TContract, TConcrete>(Func<TContract> factory)
            where TConcrete : class, TContract;
        public void RegisterFactory<TContract, TConcrete>(Func<IScopeResolver, Func<TContract>> factory)
            where TConcrete : class, TContract;
        public void RegisterFactory<TParameter, TContract, TConcrete>(Func<IScopeResolver, Func<TParameter, TContract>> factory)
            where TConcrete : class, TContract;
        
        public void RegisterScope<TScope>(Action<IScopeBuilder> install)
            where TScope : Scope;
        public void RegisterScope<TScope, TScopeImplementation>(Action<IScopeBuilder> install)
            where TScope : class, IDisposable
            where TScopeImplementation : Scope, TScope;
        void RegisterScope<TScope>(Action<IScopeBuilder, IScopeResolver> install)
            where TScope : Scope;
        public void RegisterScope<TScope, TScopeImplementation>(Action<IScopeBuilder, IScopeResolver> install)
            where TScope : class, IDisposable
            where TScopeImplementation : Scope, TScope;
    }
}
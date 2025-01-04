using System;

namespace SparseInject
{
    public interface IScopeBuilder
    {
        void Register(Action<IScopeBuilder> registerMethod);
        
        RegistrationOptions Register<TKey>(Lifetime lifetime = Lifetime.Transient)
            where TKey : class;
        RegistrationOptions Register<TKey, TImplementation>(Lifetime lifetime = Lifetime.Transient)
            where TKey : class
            where TImplementation : class, TKey;
        RegistrationOptions Register<TContract0, TContract1, TConcrete>(Lifetime lifetime = Lifetime.Transient)
            where TContract0 : class
            where TContract1 : class
            where TConcrete : class, TContract0, TContract1;
        RegistrationOptions Register<TContract0, TContract1, TContract2, TConcrete>(Lifetime lifetime = Lifetime.Transient)
            where TContract0 : class
            where TContract1 : class
            where TContract2 : class
            where TConcrete : class, TContract0, TContract1, TContract2;
        
        RegistrationOptions RegisterValue<TContract>(TContract value)
            where TContract : class;
        RegistrationOptions RegisterValue<TContract, TImplementation>(TImplementation value)
            where TContract : class
            where TImplementation : class, TContract;
        RegistrationOptions RegisterValue<TContract0, TContract1, TConcrete>(TConcrete value)
            where TContract0 : class
            where TContract1 : class
            where TConcrete : class, TContract0, TContract1;
        RegistrationOptions RegisterValue<TContract0, TContract1, TContract2, TConcrete>(TConcrete value)
            where TContract0 : class
            where TContract1 : class
            where TContract2 : class
            where TConcrete : class, TContract0, TContract1, TContract2;

        void RegisterFactory<T>(Func<T> factory)
            where T : class;
        void RegisterFactory<T>(Func<IScopeResolver, T> factory)
            where T : class;
        void RegisterFactory<TParameter, T>(Func<IScopeResolver, Func<TParameter, T>> factory)
            where T : class;
        void RegisterFactory<TContract, TConcrete>(Func<TContract> factory)
            where TConcrete : class, TContract;
        void RegisterFactory<TContract, TConcrete>(Func<IScopeResolver, Func<TContract>> factory)
            where TConcrete : class, TContract;
        void RegisterFactory<TParameter, TContract, TConcrete>(Func<IScopeResolver, Func<TParameter, TContract>> factory)
            where TConcrete : class, TContract;
        
        void RegisterScope<TScope>(Action<IScopeBuilder> install)
            where TScope : Scope;
        void RegisterScope<TScope, TScopeImplementation>(Action<IScopeBuilder> install)
            where TScope : class, IDisposable
            where TScopeImplementation : Scope, TScope;
        void RegisterScope<TScope>(Action<IScopeBuilder, IScopeResolver> install)
            where TScope : Scope;
        void RegisterScope<TScope, TScopeImplementation>(Action<IScopeBuilder, IScopeResolver> install)
            where TScope : class, IDisposable
            where TScopeImplementation : Scope, TScope;
    }
}
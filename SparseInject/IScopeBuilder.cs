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
        public void Register<TKey>(TKey value)
            where TKey : class;
        public void Register<TKey, TImplementation>(TImplementation value)
            where TKey : class
            where TImplementation : class, TKey;
        
        public void RegisterScope<TScope>(Action<IScopeBuilder> install)
            where TScope : Scope;
        public void RegisterScope<TScope, TScopeImplementation>(Action<IScopeBuilder> install)
            where TScope : class, IDisposable
            where TScopeImplementation : Scope;
        
        void RegisterScope<TScope>(Action<IScopeBuilder, IScopeResolver> install)
            where TScope : Scope;
        public void RegisterScope<TScope, TScopeImplementation>(Action<IScopeBuilder, IScopeResolver> install)
            where TScope : class, IDisposable
            where TScopeImplementation : Scope;
    }
}
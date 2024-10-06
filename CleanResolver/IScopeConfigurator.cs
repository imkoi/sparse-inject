using System;

namespace CleanResolver
{
    public interface IScopeConfigurator
    {
        public void Register<TKey>(RegisterType registerType = RegisterType.Transient)
            where TKey : class;
        public void Register<TKey, TImplementation>(RegisterType registerType = RegisterType.Transient)
            where TKey : class
            where TImplementation : class, TKey;
        public void Register<TKey>(TKey value)
            where TKey : class;
        public void Register<TKey, TImplementation>(TImplementation value)
            where TKey : class
            where TImplementation : class, TKey;

        void RegisterScope<TScope>(Action<IScopeConfigurator> install)
            where TScope : Scope;
    }
}
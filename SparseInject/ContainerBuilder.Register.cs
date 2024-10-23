using System;

namespace SparseInject
{
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
            
            var contractId = GetOrAddContractId<TContract>(out var contractType);
            
            AddContract(contractId, contractType, index);

            concrete.SingletonFlag = lifetime == Lifetime.Singleton 
                ? SingletonFlag.Singleton
                : SingletonFlag.NotSingleton;
        }
        
        public void Register<TContract0, TContract1, TConcrete>(Lifetime lifetime = Lifetime.Transient)
            where TContract0 : class
            where TContract1 : class
            where TConcrete : class, TContract0, TContract1
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);
            
            var contractId = GetOrAddContractId<TContract0>(out var contractType);
            
            AddContract(contractId, contractType, index);
            
            contractId = GetOrAddContractId<TContract1>(out contractType);
            
            AddContract(contractId, contractType, index);

            concrete.SingletonFlag = lifetime == Lifetime.Singleton 
                ? SingletonFlag.Singleton
                : SingletonFlag.NotSingleton;
        }
        
        public void Register<TContract0, TContract1, TContract2, TConcrete>(Lifetime lifetime = Lifetime.Transient)
            where TContract0 : class
            where TContract1 : class
            where TContract2 : class
            where TConcrete : class, TContract0, TContract1, TContract2
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);
            
            var contractId = GetOrAddContractId<TContract0>(out var contractType);
            
            AddContract(contractId, contractType, index);
            
            contractId = GetOrAddContractId<TContract1>(out contractType);
            
            AddContract(contractId, contractType, index);
            
            contractId = GetOrAddContractId<TContract2>(out contractType);
            
            AddContract(contractId, contractType, index);

            concrete.SingletonFlag = lifetime == Lifetime.Singleton 
                ? SingletonFlag.Singleton
                : SingletonFlag.NotSingleton;
        }

        public void Register<TConcreteContract>(TConcreteContract value)
            where TConcreteContract : class
        {
            Register<TConcreteContract, TConcreteContract>(value);
        }

        public void Register<TContract, TConcrete>(TConcrete value)
            where TContract : class
            where TConcrete : class, TContract
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);
            
            var contractId = GetOrAddContractId<TContract>(out var contractType);
            
            AddContract(contractId, contractType, index);
            
            concrete.SingletonFlag = SingletonFlag.SingletonWithValue;
            concrete.SingletonValue = value;
        }
        
        public void Register<TContract0, TContract1, TConcrete>(TConcrete value)
            where TContract0 : class
            where TContract1 : class
            where TConcrete : class, TContract0, TContract1
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);
            
            var contractId = GetOrAddContractId<TContract0>(out var contractType);
            
            AddContract(contractId, contractType, index);
            
            contractId = GetOrAddContractId<TContract1>(out contractType);
            
            AddContract(contractId, contractType, index);

            concrete.SingletonFlag = SingletonFlag.SingletonWithValue;
            concrete.SingletonValue = value;
        }
        
        public void Register<TContract0, TContract1, TContract2, TConcrete>(TConcrete value)
            where TContract0 : class
            where TContract1 : class
            where TContract2 : class
            where TConcrete : class, TContract0, TContract1, TContract2
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);
            
            var contractId = GetOrAddContractId<TContract0>(out var contractType);
            
            AddContract(contractId, contractType, index);
            
            contractId = GetOrAddContractId<TContract1>(out contractType);
            
            AddContract(contractId, contractType, index);
            
            contractId = GetOrAddContractId<TContract2>(out contractType);
            
            AddContract(contractId, contractType, index);

            concrete.SingletonFlag = SingletonFlag.SingletonWithValue;
            concrete.SingletonValue = value;
        }
        
        public void RegisterFactory<T>(Func<T> factory)
            where T : class
        {
            RegisterFactory<T, T>(factory);
        }
        
        public void RegisterFactory<T>(Func<IScopeResolver, T> factory)
            where T : class
        {
            RegisterFactory<T, T>(factory);
        }
        
        public void RegisterFactory<TContract, TConcrete>(Func<TContract> factory)
            where TConcrete : class, TContract
        {
            RegisterFactory<TContract, TConcrete>(container => factory.Invoke());
        }
        
        public void RegisterFactory<TContract, TConcrete>(Func<IScopeResolver, TContract> factory)
            where TConcrete : class, TContract
        {
            ref var concrete = ref AddConcrete(typeof(Func<TConcrete>), out var index);
            
            var contractId = GetOrAddContractId<Func<TContract>>(out var contractType);
            
            AddContract(contractId, contractType, index);

            concrete.IsFactory = true;
            concrete.SingletonFlag = SingletonFlag.SingletonWithValue;
            concrete.SingletonValue = factory;
        }
        
        public void RegisterScope<TScopeConcreteContract>(Action<IScopeBuilder> install)
            where TScopeConcreteContract : Scope
        {
            RegisterScope<TScopeConcreteContract, TScopeConcreteContract>(install);
        }

        public void RegisterScope<TScopeContract, TScopeConcrete>(Action<IScopeBuilder> install)
            where TScopeContract : class, IDisposable
            where TScopeConcrete : Scope
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
            where TScopeConcrete : Scope
        {
            ref var concrete = ref AddConcrete(typeof(TScopeConcrete), out var index);
            
            var contractId = GetOrAddContractId<TScopeContract>(out var contractType);
            
            AddContract(contractId, contractType, index);

            concrete.SingletonFlag = SingletonFlag.NotSingleton;
            concrete.ScopeConfigurator = install;
        }
    }
}
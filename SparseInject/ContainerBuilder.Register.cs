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
        public void Register(Action<IScopeBuilder> registerMethod)
        {
            if (registerMethod == null)
            {
                throw new ArgumentNullException(nameof(registerMethod));
            }
            
            registerMethod.Invoke(this);
        }

        public RegistrationOptions Register<TConcreteContract>(Lifetime lifetime = Lifetime.Transient)
            where TConcreteContract : class
        {
            return Register<TConcreteContract, TConcreteContract>(lifetime);
        }

        public RegistrationOptions Register<TContract, TConcrete>(Lifetime lifetime = Lifetime.Transient)
            where TContract : class
            where TConcrete : class, TContract
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);

            AddContract(typeof(TContract), typeof(TContract[]), index);

            if (lifetime == Lifetime.Singleton)
            {
                concrete.MarkSingleton();
            }
            
            return new RegistrationOptions
            {
                _builder = this,
                _concreteIndex = index
            };
        }
        
        public RegistrationOptions Register<TContract0, TContract1, TConcrete>(Lifetime lifetime = Lifetime.Transient)
            where TContract0 : class
            where TContract1 : class
            where TConcrete : class, TContract0, TContract1
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);

            AddContract(typeof(TContract0), typeof(TContract0[]), index);
            AddContract(typeof(TContract1), typeof(TContract1[]), index);

            if (lifetime == Lifetime.Singleton)
            {
                concrete.MarkSingleton();
            }
            
            return new RegistrationOptions
            {
                _builder = this,
                _concreteIndex = index
            };
        }
        
        public RegistrationOptions Register<TContract0, TContract1, TContract2, TConcrete>(Lifetime lifetime = Lifetime.Transient)
            where TContract0 : class
            where TContract1 : class
            where TContract2 : class
            where TConcrete : class, TContract0, TContract1, TContract2
        {
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);
            
            AddContract(typeof(TContract0), typeof(TContract0[]), index);
            AddContract(typeof(TContract1), typeof(TContract1[]), index);
            AddContract(typeof(TContract2), typeof(TContract2[]), index);
            
            if (lifetime == Lifetime.Singleton)
            {
                concrete.MarkSingleton();
            }
            
            return new RegistrationOptions
            {
                _builder = this,
                _concreteIndex = index
            };
        }
    }
}
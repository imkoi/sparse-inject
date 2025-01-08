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
        public RegistrationOptions RegisterValue<TConcreteContract>(TConcreteContract value)
            where TConcreteContract : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            
            ref var concrete = ref AddConcrete(typeof(TConcreteContract), out var index);
  
            AddContract<TConcreteContract>(index);
            
            concrete.MarkSingleton();
            concrete.MarkValue();
            concrete.Value = value;

            if (value is Array)
            {
                concrete.MarkArray();
            }
            
            return new RegistrationOptions
            {
                _builder = this,
                _concreteIndex = index
            };
        }

        public RegistrationOptions RegisterValue<TContract, TConcrete>(TConcrete value)
            where TContract : class
            where TConcrete : class, TContract
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            
            if (typeof(TConcrete) != value.GetType())
            {
                throw new SparseInjectException($"To register value of type '{value.GetType().Name}' " +
                                                $"use 'RegisterValue<{typeof(TContract).Name}, {typeof(TConcrete).Name}, {value.GetType().Name}>(value)' method signature");
            }

            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);

            AddContract<TContract>(index);
            
            concrete.MarkSingleton();
            concrete.MarkValue();
            concrete.Value = value;
            
            if (value is Array)
            {
                concrete.MarkArray();
            }
            
            return new RegistrationOptions
            {
                _builder = this,
                _concreteIndex = index
            };
        }
        
        public RegistrationOptions RegisterValue<TContract0, TContract1, TConcrete>(TConcrete value)
            where TContract0 : class
            where TContract1 : class
            where TConcrete : class, TContract0, TContract1
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);

            AddContract<TContract0>(index);
            AddContract<TContract1>(index);

            concrete.MarkSingleton();
            concrete.MarkValue();
            concrete.Value = value;
            
            if (value is Array)
            {
                concrete.MarkArray();
            }
            
            return new RegistrationOptions
            {
                _builder = this,
                _concreteIndex = index
            };
        }
        
        public RegistrationOptions RegisterValue<TContract0, TContract1, TContract2, TConcrete>(TConcrete value)
            where TContract0 : class
            where TContract1 : class
            where TContract2 : class
            where TConcrete : class, TContract0, TContract1, TContract2
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            
            ref var concrete = ref AddConcrete(typeof(TConcrete), out var index);

            AddContract<TContract0>(index);
            AddContract<TContract1>(index);
            AddContract<TContract2>(index);

            concrete.MarkSingleton();
            concrete.MarkValue();
            concrete.Value = value;
            
            if (value is Array)
            {
                concrete.MarkArray();
            }

            return new RegistrationOptions
            {
                _builder = this,
                _concreteIndex = index
            };
        }
    }
}
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
        public void RegisterScope<TScopeConcreteContract>(Action<IScopeBuilder> install)
            where TScopeConcreteContract : Scope
        {
            RegisterScope<TScopeConcreteContract, TScopeConcreteContract>(install);
        }

        public void RegisterScope<TScopeContract, TScopeConcrete>(Action<IScopeBuilder> install)
            where TScopeContract : class, IDisposable
            where TScopeConcrete : Scope, TScopeContract
        {
            if (install == null)
            {
                throw new ArgumentNullException(nameof(install));
            }
            
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
            if (install == null)
            {
                throw new ArgumentNullException(nameof(install));
            }
            
            ref var concrete = ref AddConcrete(typeof(TScopeConcrete), out var index);

            AddContract(typeof(TScopeContract), typeof(TScopeContract[]), index);

            concrete.MarkScope();
            concrete.Value = install;
        }
    }
}
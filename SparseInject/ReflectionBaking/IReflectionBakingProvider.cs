using System;

namespace SparseInject
{
    public interface IReflectionBakingProvider
    {
        Type[] ConstructorParametersSpan { get; }
        void Initialize();
        InstanceFactoryBase GetInstanceFactory(Type type);
    }
}
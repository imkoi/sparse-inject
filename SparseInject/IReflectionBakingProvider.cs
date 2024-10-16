using System;

namespace SparseInject
{
    public interface IReflectionBakingProvider
    {
        void Initialize();
        IInstanceFactory GetInstanceFactory(Type type);
    }
}
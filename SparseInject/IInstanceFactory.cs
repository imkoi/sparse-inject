using System;

namespace SparseInject
{
    public interface IInstanceFactory
    {
        int ConstructorParametersCount { get; }
        Type[] GetConstructorParameters();
        object Create(object[] parameters);
    }
}
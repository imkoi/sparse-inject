using System;

namespace SparseInject
{
    public interface IScopeResolver : IDisposable
    {
        T Resolve<T>() where T : class;
    }
}
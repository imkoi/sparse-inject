using System;

namespace SparseInject
{
    public abstract class Scope : IDisposable
    {
        internal Container _container;
        
        public virtual void Dispose()
        {
            _container.Dispose();
        }
    }
}
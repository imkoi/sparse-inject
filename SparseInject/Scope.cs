using System;

namespace SparseInject
{
    public abstract class Scope : IDisposable
    {
        internal Container _container;
        
        public void Dispose()
        {
            
        }
    }
}
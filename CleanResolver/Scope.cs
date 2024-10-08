using System;

namespace CleanResolver
{
    public abstract class Scope : IDisposable
    {
        internal Container _container;
        
        public void Dispose()
        {
            
        }
    }
}
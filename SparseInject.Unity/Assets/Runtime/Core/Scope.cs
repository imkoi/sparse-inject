using System;

namespace SparseInject
{
    public abstract class Scope : IDisposable
    {
        internal Container _container { get; private set; }
        
        public virtual void Dispose()
        {
            var parentContainer = _container.GetParentContainer();
            if (parentContainer != null)
            {
                parentContainer.DisposeRequested -= Dispose;
            }
            
            _container.Dispose();
        }

        internal void SetContainer(Container container)
        {
            _container = container;

            var parentContainer = container.GetParentContainer();
            
            parentContainer.DisposeRequested += Dispose;
        }
    }
}
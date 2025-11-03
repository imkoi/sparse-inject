using System.Collections.Generic;
using UnityEngine;

namespace SparseInject
{
    public abstract class MonoContext : MonoBehaviour
    {
        protected abstract IEnumerable<IInstaller> Installers { get; }
        
        protected Container Container => GetOrCreateContainer();
        
        private Container _container;

        private Container GetOrCreateContainer()
        {
            if (_container == null)
            {
                var containerBuilder = new ContainerBuilder();

                foreach (var installer in Installers)
                {
                    containerBuilder.Register(installer.InstallBindings);
                }

                _container = containerBuilder.Build();
            }

            return _container;
        }
    }
}
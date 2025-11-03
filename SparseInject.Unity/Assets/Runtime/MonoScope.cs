using System;
using UnityEngine;

namespace SparseInject
{
    public abstract class MonoScope<TScope> : MonoScopeBase where TScope : Scope
    {
        [SerializeField] 
        private bool _autoLinkInstallers = false;
        
        private Container _container;

        protected override Container GetOrCreateContainer()
        {
            if (_container == null)
            {
                if (TryGetParentScope(out var parentScope))
                {
                    var scope = parentScope.Container.Resolve<Func<TScope>>().Invoke();

                    _container = scope._container;
                }
                else
                {
                    var containerBuilder = new ContainerBuilder();

                    foreach (var installer in Installers)
                    {
                        containerBuilder.Register(installer.InstallBindings);
                    }

                    if (_autoLinkInstallers)
                    {
                        foreach (var installer in AutoLinkInstallersFactory.Create())
                        {
                            containerBuilder.Register(installer.InstallBindings);
                        }
                    }
                    
                    _container = containerBuilder.Build();
                }
            }

            return _container;
        }
    }
}
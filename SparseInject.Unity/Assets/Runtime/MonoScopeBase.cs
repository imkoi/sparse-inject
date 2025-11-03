using System.Collections.Generic;
using UnityEngine;

namespace SparseInject
{
    public abstract class MonoScopeBase : MonoBehaviour
    {
        protected abstract IEnumerable<IInstaller> Installers { get; }

        public Container Container => GetOrCreateContainer();
        protected abstract Container GetOrCreateContainer();
        
        private bool _isParentProcessed;
        private MonoScopeBase _parentScope;
        private bool _hasParentScope;

        protected bool TryGetParentScope(out MonoScopeBase scope)
        {
            if (!_isParentProcessed)
            {
                var parent = transform.parent;

                if (parent != null)
                {
                    _parentScope = (MonoScopeBase) parent.gameObject.GetComponentInParent(typeof(MonoScopeBase), true);
                    _hasParentScope = _parentScope != null;
                }
                
                _isParentProcessed = true;
            }

            scope = _parentScope;
            
            return _hasParentScope;
        }
    }
}
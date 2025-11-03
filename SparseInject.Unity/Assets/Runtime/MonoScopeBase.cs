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

        public bool TryGetParentScope(out MonoScopeBase scope)
        {
            if (!_isParentProcessed)
            {
                _parentScope = GetComponentInParent<MonoScopeBase>();
                _hasParentScope = _parentScope != null;
                
                _isParentProcessed = true;
            }

            scope = _parentScope;
            
            return _hasParentScope;
        }
    }
}
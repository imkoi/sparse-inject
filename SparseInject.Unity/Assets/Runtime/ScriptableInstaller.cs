using UnityEngine;

namespace SparseInject
{
    public abstract class ScriptableInstaller : ScriptableObject, IInstaller
    {
        public abstract void InstallBindings(IScopeBuilder containerBuilder);
    }
}
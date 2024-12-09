using UnityEngine;

namespace SparseInject
{
    public abstract class MonoInstaller : MonoBehaviour, IInstaller
    {
        public abstract void InstallBindings(IScopeBuilder containerBuilder);
    }
}
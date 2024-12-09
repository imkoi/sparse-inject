using UnityEngine;

namespace SparseInject
{
    [CreateAssetMenu(menuName = "SparseInject/CompositeInstaller", fileName = "CompositeInstaller")]
    public class ScriptableCompositeInstaller : ScriptableInstaller, IInstaller
    {
        [SerializeField] 
        private ScriptableInstaller[] _installers;
        
        public override void InstallBindings(IScopeBuilder containerBuilder)
        {
            foreach (var installer in _installers)
            {
                installer.InstallBindings(containerBuilder);
            }
        }
    }
}
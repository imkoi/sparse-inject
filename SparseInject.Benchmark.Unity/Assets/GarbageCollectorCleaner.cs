using SparseInject.BenchmarkFramework;

public class GarbageCollectorCleaner : IResourceCleaner
{
    public void CleanResources()
    {
        #if !UNITY_EDITOR
        UnityEngine.Scripting.GarbageCollector.GCMode = UnityEngine.Scripting.GarbageCollector.Mode.Disabled;
        UnityEngine.Scripting.GarbageCollector.CollectIncremental();
        #endif
    }
}
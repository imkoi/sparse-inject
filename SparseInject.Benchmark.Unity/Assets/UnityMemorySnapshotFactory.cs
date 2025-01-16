using SparseInject.BenchmarkFramework;
using UnityEngine;

public class UnityMemorySnapshotFactory : IMemorySnapshotFactory
{
    public MemorySnapshot Create()
    {
        var totalMemory = -1f;
        
        #if UNITY_ANDROID && !UNITY_EDITOR
        using var debugClass = new AndroidJavaClass("android.os.Debug");
        var pssKb = debugClass.CallStatic<long>("getPss");
        
        totalMemory = pssKb > 0 ? pssKb / 1024f : 0f;
        #endif


        return new MemorySnapshot(totalMemory, totalMemory, totalMemory);
    }

    public MemorySnapshot Create(MemorySnapshot subtract)
    {
        var currentSnapshot = Create();
        
        return new MemorySnapshot(
            currentSnapshot.WorkingSetMb - subtract.WorkingSetMb,
            currentSnapshot.PrivateMemoryMb - subtract.PrivateMemoryMb,
            currentSnapshot.VirtualMemoryMb - subtract.VirtualMemoryMb);
    }
}
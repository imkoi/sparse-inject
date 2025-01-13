using SparseInject.BenchmarkFramework;

public class UnityMemorySnapshotFactory : IMemorySnapshotFactory
{
    public MemorySnapshot Create()
    {
        // unity not provide method to track untracked memory, maybe thats why its called untracked :/

        return new MemorySnapshot(0, 0, 0);
    }

    public MemorySnapshot Create(MemorySnapshot subtract)
    {
        // unity not provide method to track untracked memory, maybe thats why its called untracked :/
            
        return new MemorySnapshot(0, 0, 0);
    }
}
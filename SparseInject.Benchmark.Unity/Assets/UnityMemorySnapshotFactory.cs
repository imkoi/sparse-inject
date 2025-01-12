using SparseInject.BenchmarkFramework;

namespace DefaultNamespace
{
    public class UnityMemorySnapshotFactory : IMemorySnapshotFactory
    {
        public MemorySnapshot Create()
        {
            throw new System.NotImplementedException();
            // unity not provide method to track untracked memory, maybe thats why its called untracked :/
        }

        public MemorySnapshot Create(MemorySnapshot subtract)
        {
            throw new System.NotImplementedException();
            // unity not provide method to track untracked memory, maybe thats why its called untracked :/
        }
    }
}
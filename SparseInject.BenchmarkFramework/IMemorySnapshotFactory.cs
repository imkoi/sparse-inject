namespace SparseInject.BenchmarkFramework
{
    public interface IMemorySnapshotFactory
    {
        public MemorySnapshot Create();
        MemorySnapshot Create(MemorySnapshot subtract);
    }
}
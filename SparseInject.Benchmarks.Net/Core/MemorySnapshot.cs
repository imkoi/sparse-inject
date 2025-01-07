namespace SparseInject.Benchmarks.Core;

public class MemorySnapshot
{
    public float WorkingSetMb { get; }
    public float PrivateMemoryMb { get; }
    public float VirtualMemoryMb { get; }

    public MemorySnapshot(float workingSetMb, float privateMemoryMb, float virtualMemoryMb)
    {
        WorkingSetMb = workingSetMb;
        PrivateMemoryMb = privateMemoryMb;
        VirtualMemoryMb = virtualMemoryMb;
    }
}
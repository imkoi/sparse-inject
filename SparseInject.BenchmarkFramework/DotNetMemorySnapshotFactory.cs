using System.Diagnostics;

namespace SparseInject.BenchmarkFramework;

public class DotNetMemorySnapshotFactory : IMemorySnapshotFactory
{
    public MemorySnapshot Create()
    {
        var currentProcess = Process.GetCurrentProcess();

        var workingSet = currentProcess.WorkingSet64;
        var privateMemory = currentProcess.PrivateMemorySize64;
        var virtualMemory = currentProcess.VirtualMemorySize64;

        var workingSetMb = workingSet / (1024.0f * 1024.0f);
        var privateMemoryMb = privateMemory / (1024.0f * 1024.0f);
        var virtualMemoryMb = virtualMemory / (1024.0f * 1024.0f);

        return new MemorySnapshot(workingSetMb, privateMemoryMb, virtualMemoryMb);
    }
}
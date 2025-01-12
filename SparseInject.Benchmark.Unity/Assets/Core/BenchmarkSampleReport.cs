using System;

namespace SparseInject.BenchmarkFramework
{
    public class BenchmarkSampleReport
    {
        public TimeSpan Duration { get; }
        public float MemoryMb { get; }

        public BenchmarkSampleReport(TimeSpan duration, float memoryMb)
        {
            Duration = duration;
            MemoryMb = memoryMb;
        }
    }
}
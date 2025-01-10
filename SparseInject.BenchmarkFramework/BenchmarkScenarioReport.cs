using System;
using System.Collections.Generic;
using System.Linq;

namespace SparseInject.BenchmarkFramework
{
    public class BenchmarkScenarioReport
    {
        public string Name { get; }
        public IReadOnlyList<BenchmarkSampleReport> SampleReports { get; }
        
        public TimeSpan MinDuration => SampleReports.Min(sample => sample.Duration);
        public TimeSpan AverageDuration => new TimeSpan((long) SampleReports.Average(sample => sample.Duration.Ticks));
        public TimeSpan MaxDuration => SampleReports.Max(sample => sample.Duration);
        public TimeSpan ErrorDuration => new TimeSpan((long) Math.Sqrt(SampleReports.Average(sample => 
            Math.Pow(sample.Duration.Ticks - AverageDuration.Ticks, 2))));
        
        public float MinMemoryMb => SampleReports.Min(sample => sample.MemoryMb);
        public float AverageMemoryMb => SampleReports.Average(sample => sample.MemoryMb);
        public float MaxMemoryMb => SampleReports.Max(sample => sample.MemoryMb);
        public float ErrorMemoryMb => (float) Math.Sqrt(SampleReports.Average(sample => 
            Math.Pow(sample.MemoryMb - AverageMemoryMb, 2)));
        
        public BenchmarkScenarioReport(string name, IReadOnlyList<BenchmarkSampleReport> sampleReports)
        {
            Name = name;
            SampleReports = sampleReports;
        }
    }
}
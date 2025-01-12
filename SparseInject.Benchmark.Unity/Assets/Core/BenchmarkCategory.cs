using System.Collections.Generic;

namespace SparseInject.BenchmarkFramework
{
    public class BenchmarkCategory
    {
        public string Name { get; }
        public IReadOnlyList<Scenario> Benchmarks { get; }
        public int Samples { get; }

        public BenchmarkCategory(string name, Scenario[] benchmarks, int samples)
        {
            Name = name;
            Samples = samples;
            Benchmarks = new List<Scenario>(benchmarks);
        } 
    }
}
namespace SparseInject.Benchmarks.Core;

public class BenchmarkCategory
{
    public string Name { get; }
    public IReadOnlyList<Benchmark> Benchmarks { get; }
    public int Samples { get; }

    public BenchmarkCategory(string name, Benchmark[] benchmarks, int samples)
    {
        Name = name;
        Samples = samples;
        Benchmarks = new List<Benchmark>(benchmarks);
    } 
}
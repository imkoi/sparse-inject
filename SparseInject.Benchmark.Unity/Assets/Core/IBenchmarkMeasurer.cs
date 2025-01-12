namespace SparseInject.BenchmarkFramework
{
    public interface IBenchmarkMeasurer
    {
        void Measure(string categoryName, string benchmarkName);
    }
}
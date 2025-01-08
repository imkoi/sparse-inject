namespace SparseInject.BenchmarkFramework
{
    public interface IBenchmarkMeasurer
    {
        void Measure(string categoryName, string benchmarkName, int samples, string args, IReportStorage storage);
    }
}
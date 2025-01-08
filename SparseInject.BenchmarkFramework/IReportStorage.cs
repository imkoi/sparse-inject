namespace SparseInject.BenchmarkFramework
{
    public interface IReportStorage
    {
        void AddReport(string categoryName, string benchmarkName, string description);
    }
}
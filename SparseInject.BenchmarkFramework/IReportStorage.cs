using System.Collections.Generic;

namespace SparseInject.BenchmarkFramework
{
    public interface IReportStorage
    {
        void ReportSample(string categoryName, string scenarioName, BenchmarkSampleReport report);
        IReadOnlyList<BenchmarkSampleReport> GetSamples(string categoryName, string scenarioName);
        void CleanSamples();
    }
}
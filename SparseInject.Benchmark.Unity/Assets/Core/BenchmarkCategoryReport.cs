using System.Collections.Generic;

namespace SparseInject.BenchmarkFramework
{
    public class BenchmarkCategoryReport
    {
        public string Name { get; }
        public IReadOnlyList<BenchmarkScenarioReport> ScenarioReports { get; }

        public BenchmarkCategoryReport(string name, IReadOnlyList<BenchmarkScenarioReport> scenarioReports)
        {
            Name = name;
            ScenarioReports = scenarioReports;
        }
    }
}
using System.Collections.Generic;
using System.Text;

namespace SparseInject.BenchmarkFramework
{
    public class BenchmarkReport
    {
        public IReadOnlyList<BenchmarkCategoryReport> CategoryReports { get; }

        public BenchmarkReport(List<BenchmarkCategoryReport> categoryReports)
        {
            CategoryReports = categoryReports;
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var categoryReport in CategoryReports)
            {
                foreach (var scenarioReport in categoryReport.ScenarioReports)
                {
                    sb.Append("[").Append(categoryReport.Name).Append("::").Append(scenarioReport.Name).Append("]");
                    sb.Append(" time_ms(");
                    sb.Append("avg: ").Append(scenarioReport.AverageDuration.TotalMilliseconds.ToString("F2")).Append(", ");
                    sb.Append("min: ").Append(scenarioReport.MinDuration.TotalMilliseconds.ToString("F2")).Append(", ");
                    sb.Append("max: ").Append(scenarioReport.MaxDuration.TotalMilliseconds.ToString("F2")).Append(", ");
                    sb.Append("err: ").Append(scenarioReport.ErrorDuration.TotalMilliseconds.ToString("F2")).Append(")");
                    sb.Append(", memory_mb(");
                    sb.Append("avg: ").Append(scenarioReport.AverageMemoryMb.ToString("F2")).Append(", ");
                    sb.Append("min: ").Append(scenarioReport.MinMemoryMb.ToString("F2")).Append(", ");
                    sb.Append("max: ").Append(scenarioReport.MaxMemoryMb.ToString("F2")).Append(", ");
                    sb.Append("err: ").Append(scenarioReport.ErrorMemoryMb.ToString("F2")).AppendLine(")");
                }
            }

            return sb.ToString();
        }
    }
}
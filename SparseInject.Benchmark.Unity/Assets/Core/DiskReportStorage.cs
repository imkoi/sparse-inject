using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SparseInject.BenchmarkFramework
{
    public class DiskReportStorage : IReportStorage
    {
        private readonly string _reportFilePath;

        public DiskReportStorage(string reportFilePath)
        {
            _reportFilePath = reportFilePath;
        }

        public void ReportSample(string categoryName, string scenarioName, BenchmarkSampleReport report)
        {
            using (var writer = new StreamWriter(File.Open(
                       _reportFilePath,
                       FileMode.Append,
                       FileAccess.Write,
                       FileShare.Read)))
            {
                writer.WriteLine(
                    $"[{categoryName}::{scenarioName}] time_ticks({report.Duration.Ticks}); memory_mb({report.MemoryMb})");
            }
        }

        public IReadOnlyList<BenchmarkSampleReport> GetSamples(string categoryName, string scenarioName)
        {
            if (!File.Exists(_reportFilePath))
            {
                return new List<BenchmarkSampleReport>();
            }

            var samples = new List<BenchmarkSampleReport>(10000);

            using (var reader = new StreamReader(File.Open(
                       _reportFilePath,
                       FileMode.Open,
                       FileAccess.Read,
                       FileShare.Read)))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var mainSplit = line.Split(']');
                        var header =
                            mainSplit[0]
                                .Trim('[', ']'); // "[categoryName::scenarioName]" -> "categoryName::scenarioName"
                        var values = mainSplit[1].Trim(); // " time_ticks(value), memory_mb(value)"

                        var headerSplit = header.Split("::");
                        if (headerSplit.Length != 2)
                        {
                            continue;
                        }

                        var fetchedCategoryName = headerSplit[0];

                        if (categoryName != fetchedCategoryName)
                        {
                            continue;
                        }

                        var fetchedScenarioName = headerSplit[1];

                        if (scenarioName != fetchedScenarioName)
                        {
                            continue;
                        }

                        var valuesSplit = values.Split(';');
                        if (valuesSplit.Length != 2)
                        {
                            continue;
                        }

                        var timeTicksPart = valuesSplit[0].Trim().Replace("time_ticks(", "").Replace(")", "");
                        var memoryMbPart = valuesSplit[1].Trim().Replace("memory_mb(", "").Replace(")", "");

                        if (long.TryParse(timeTicksPart, out var timeTicks) &&
                            float.TryParse(memoryMbPart, out var memoryMb))
                        {
                            samples.Add(new BenchmarkSampleReport(TimeSpan.FromTicks(timeTicks), memoryMb));
                        }
                    }
                }
            }

            return samples;
        }

        public void CleanSamples()
        {
            if (File.Exists(_reportFilePath))
            {
                File.Delete(_reportFilePath);
            }
        }
    }
}
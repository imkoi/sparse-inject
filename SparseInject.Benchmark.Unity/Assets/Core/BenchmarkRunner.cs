using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SparseInject.BenchmarkFramework
{
    public class BenchmarkRunner
    {
        private string[] _args;
        private readonly IReportStorage _reportStorage;
        private readonly IMemorySnapshotFactory _memorySnapshotFactory;
        private readonly IResourceCleaner _resourceCleaner;

        private readonly Dictionary<string, BenchmarkCategory> _categories;

        public BenchmarkRunner(
            string[] args,
            IReportStorage reportStorage,
            IMemorySnapshotFactory memorySnapshotFactory,
            IResourceCleaner resourceCleaner)
        {
            _args = args;
            _reportStorage = reportStorage;
            _memorySnapshotFactory = memorySnapshotFactory;
            _resourceCleaner = resourceCleaner;
            _categories = new Dictionary<string, BenchmarkCategory>(8);
        }
    
        public void AddBenchmarkCategory(string categoryName, Scenario[] benchmarks, int samples = 1)
        {
            _categories.Add(categoryName, new BenchmarkCategory(categoryName, benchmarks, samples));
        }

        public BenchmarkReport Run()
        {
            var scenarioInfo = GetScenarioInfoByArguments(_args);

            if (scenarioInfo.scenario == null)
            {
                throw new Exception($"No scenario found for {string.Join(" ", _args)}");
            }

            var sample = SampleScenario(scenarioInfo.scenario);

            _reportStorage.ReportSample(scenarioInfo.category.Name, scenarioInfo.scenario.Name, sample);

            return new BenchmarkReport(new List<BenchmarkCategoryReport>
            {
                new BenchmarkCategoryReport(scenarioInfo.category.Name, new List<BenchmarkScenarioReport>
                {
                    new BenchmarkScenarioReport(scenarioInfo.scenario.Name, new List<BenchmarkSampleReport>
                    {
                        sample
                    })
                })
            });
        }

        private BenchmarkSampleReport SampleScenario(Scenario scenario)
        {
            var stopwatch = Stopwatch.StartNew();
            stopwatch.Stop();
            stopwatch.Restart();
            
            scenario.BeforeExecute();
            
            _resourceCleaner.CleanResources();
            
            var beforeMemorySnapshot = _memorySnapshotFactory.Create();
            var executeCount = scenario.ExecuteCount;

            stopwatch.Restart();

            for (var i = 0; i < executeCount; i++)
            {
                scenario.Execute();
            }
            
            stopwatch.Stop();

            var afterMemorySnapshot = _memorySnapshotFactory.Create(beforeMemorySnapshot);

            var time = TimeSpan.FromTicks(stopwatch.Elapsed.Ticks / executeCount);

            return new BenchmarkSampleReport(time, afterMemorySnapshot.PrivateMemoryMb);
        }

        private (BenchmarkCategory category, Scenario scenario) GetScenarioInfoByArguments(string[] arguments)
        {
            var validArguments = arguments.First(a => a.StartsWith("run-benchmark-"));
            var categoryAndScenario = validArguments.Replace("run-benchmark-", "").Split(":");
            
            foreach (var category in _categories.Values)
            {
                foreach (var benchmark in category.Benchmarks)
                {
                    if (categoryAndScenario.First() == category.Name && categoryAndScenario.Last() == benchmark.Name)
                    {
                        return (category, benchmark);
                    }
                }
            }
        
            return (null, null);
        }
    }
}
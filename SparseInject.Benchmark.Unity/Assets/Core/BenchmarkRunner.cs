using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SparseInject.BenchmarkFramework
{
    public class BenchmarkRunner
    {
        private string[] _args;
        private readonly IReportStorage _reportStorage;
        private readonly IMemorySnapshotFactory _memorySnapshotFactory;
        private readonly IResourceCleaner _resourceCleaner;
        private readonly IBenchmarkMeasurer _benchmarkMeasurer;
        private readonly IProgress<float> _progress;

        private readonly Dictionary<string, BenchmarkCategory> _categories;

        public bool IsRootStart => _args == null || !_args.Any(arg => arg.Contains(BenchmarkConstants.RunBenchmarkCommand)) ;

        public BenchmarkRunner(
            string[] args,
            IReportStorage reportStorage,
            IMemorySnapshotFactory memorySnapshotFactory,
            IResourceCleaner resourceCleaner,
            IBenchmarkMeasurer benchmarkMeasurer,
            IProgress<float> progress)
        {
            _args = args;
            _reportStorage = reportStorage;
            _memorySnapshotFactory = memorySnapshotFactory;
            _resourceCleaner = resourceCleaner;
            _benchmarkMeasurer = benchmarkMeasurer;
            _progress = progress;
            _categories = new Dictionary<string, BenchmarkCategory>(8);
        }
    
        public void AddBenchmarkCategory(string categoryName, Scenario[] benchmarks, int samples = 1)
        {
            _categories.Add(categoryName, new BenchmarkCategory(categoryName, benchmarks, samples));
        }

        public async Task<BenchmarkReport> RunAsync(CancellationToken cancellationToken)
        {
            if (IsRootStart)
            {
                _reportStorage.CleanSamples();

                var totalSamplesCount = _categories.Values.Sum(c => c.Samples * c.Benchmarks.Count);
                var sampleIndex = 0;
                
                foreach (var category in _categories.Values)
                {
                    foreach (var scenario in category.Benchmarks)
                    {
                        var categoryName = category.Name;
                        var scenarioName = scenario.Name;
                        var samples = category.Samples;

                        for (var i = 0; i < samples; i++)
                        {
                            _benchmarkMeasurer.Measure(categoryName, scenarioName);
                            
                            while (_reportStorage.GetSamples(categoryName, scenarioName).Count <= i)
                            {
                                await TaskUtility.WaitForSecondsAsync(TimeSpan.FromSeconds(2f), cancellationToken);
                                
                                Console.WriteLine($"Possible stack at {categoryName}::{scenarioName}");
                            }

                            sampleIndex++;
                            
                            _progress.Report((float)sampleIndex / totalSamplesCount);
                        }
                    }
                }
                
                var categoryReports = new List<BenchmarkCategoryReport>();
                
                foreach (var category in _categories.Values)
                {
                    var scenarioReports = new List<BenchmarkScenarioReport>();
                    
                    foreach (var scenario in category.Benchmarks)
                    {
                        var samples = _reportStorage.GetSamples(
                            category.Name,
                            scenario.Name);
                        
                        scenarioReports.Add(new BenchmarkScenarioReport(scenario.Name, samples));
                    }
                    
                    categoryReports.Add(new BenchmarkCategoryReport(category.Name, scenarioReports));
                }
                
                return new BenchmarkReport(categoryReports);
            }

            var launchArgument = string.Join(" ", _args);
            var scenarioInfo = GetScenarioInfoByArguments(launchArgument);

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

        private (BenchmarkCategory category, Scenario scenario) GetScenarioInfoByArguments(string arguments)
        {
            var categoryAndScenario = arguments.Split(' ').Last().Split(":");
            
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
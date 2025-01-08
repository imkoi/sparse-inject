using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SparseInject.BenchmarkFramework
{
    public class BenchmarkRunner
    {
        private const string RunBenchmarkCommand = "--run-benchmark";
    
        private string[] _args;
        private readonly IReportStorage _reportStorage;
        private readonly IMemorySnapshotFactory _memorySnapshotFactory;
        private readonly IResourceCleaner _resourceCleaner;
        private readonly IBenchmarkMeasurer _benchmarkMeasurer;

        private readonly List<BenchmarkCategory> _categories;

        public bool IsRootStart => _args == null || !_args.Any(arg => arg.Contains(RunBenchmarkCommand)) ;

        public BenchmarkRunner(
            string[] args,
            IReportStorage reportStorage,
            IMemorySnapshotFactory memorySnapshotFactory,
            IResourceCleaner resourceCleaner,
            IBenchmarkMeasurer benchmarkMeasurer)
        {
            _args = args;
            _reportStorage = reportStorage;
            _memorySnapshotFactory = memorySnapshotFactory;
            _resourceCleaner = resourceCleaner;
            _benchmarkMeasurer = benchmarkMeasurer;
            _categories = new List<BenchmarkCategory>(8);
        }
    
        public void AddBenchmarkCategory(string categoryName, Benchmark[] benchmarks, int samples = 1)
        {
            _categories.Add(new BenchmarkCategory(categoryName, benchmarks, samples));
        }

        public string Run()
        {
            if (IsRootStart)
            {
                foreach (var category in _categories)
                {
                    foreach (var benchmark in category.Benchmarks)
                    {
                        var categoryName = category.Name;
                        var benchmarkName = benchmark.Name;
                        var samples = category.Samples;

                        var arguments = $"{RunBenchmarkCommand} {category.Name}:{benchmark.Name}";
                        
                        _benchmarkMeasurer.Measure(categoryName, benchmarkName, samples, arguments, _reportStorage);
                    }
                }
            }
            else
            {
                var launchArgument = string.Join(" ", _args);
                var benchmarkInfo = GetBenchmarkInfoByArguments(launchArgument);
                var benchmark = benchmarkInfo.benchmark;

                RunBenchmark(benchmarkInfo.category.Name, benchmark);
            }

            return string.Empty;
        }

        private void RunBenchmark(string categoryName, Benchmark benchmark)
        {
            var stopwatch = Stopwatch.StartNew();
            stopwatch.Stop();
            stopwatch.Restart();
            
            benchmark.BeforeExecute();
            
            _resourceCleaner.CleanResources();

            stopwatch.Restart();
            benchmark.Execute();
            stopwatch.Stop();

            var memorySnapshot = _memorySnapshotFactory.Create();

            Console.WriteLine($"[{categoryName}] {benchmark.Name}: {stopwatch.Elapsed.TotalMilliseconds.ToString("F2")} ms / {memorySnapshot.PrivateMemoryMb.ToString("F2")} mb");
        }

        private (BenchmarkCategory category, Benchmark benchmark) GetBenchmarkInfoByArguments(string targetArguments)
        {
            foreach (var category in _categories)
            {
                foreach (var benchmark in category.Benchmarks)
                {
                    var arguments = $"--run-benchmark {category.Name}:{benchmark.Name}";

                    if (targetArguments == arguments)
                    {
                        return (category, benchmark);
                    }
                }
            }
        
            return (null, null);
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using SparseInject.BenchmarkFramework;
using SparseInject.Benchmarks.Net;

public class Program
{
    public static async Task Main(string[] args)
    {
        var cancellationTokenSource = new CancellationTokenSource();

        Console.CancelKeyPress += OnCancelKeyPress;

        void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.CancelKeyPress -= OnCancelKeyPress;
            
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

        var progress = new BenchmarkProgress();

        progress.Changed += OnBenchmarkProgressChanged;
        
        var benchmarkRunner = new BenchmarkRunner(
            args,
            new DiskReportStorage("C:/github/sparseinject/SparseInject.Benchmarks.Net/bin/Release/net8.0/temp_report.txt"),
            new DotNetMemorySnapshotFactory(),
            new DotNetResourceCleaner(),
            new DotNetBenchmarkMeasurer(),
            progress);
        
        benchmarkRunner.AddBenchmarkCategory("type-id-provider", new Scenario[]
        {
            new TypeIdProviderScenario(),
            new DictionaryScenario(),
        }, 10);
        
        // TransientBenchmarkUtility.AddCategories(benchmarkRunner, 10);
        // SingletonBenchmarkUtility.AddCategories(benchmarkRunner, 10);
        
        var summary = await benchmarkRunner.RunAsync(cancellationTokenSource.Token);

        progress.Changed -= OnBenchmarkProgressChanged;

        var isBenchmarkLauncher = args == null || args.Length == 0;
        
        if (isBenchmarkLauncher)
        {
            Console.WriteLine(summary);
        }

        OnCancelKeyPress(null, null);

        void OnBenchmarkProgressChanged(float value)
        {
            Console.WriteLine($"Benchmark progress: {value * 100f:F2}");
        }
    }
}
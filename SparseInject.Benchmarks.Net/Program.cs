using System;
using System.Threading;
using System.Threading.Tasks;
using SparseInject.BenchmarkFramework;

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
        
        benchmarkRunner.AddBenchmarkCategory("transient-register", new Scenario[]
        {
            new SparseInjectTransientRegisterScenario(),
            new VContainerTransientRegisterScenario(),
            new AutofacTransientRegisterScenario(),
            new LightInjectTransientRegisterScenario(),
        }, 5);
        
        benchmarkRunner.AddBenchmarkCategory("transient-build", new Scenario[]
        {
            new SparseInjectTransientBuildScenario(),
            new VContainerTransientBuildScenario(),
            new AutofacTransientBuildScenario(),
        }, 5);
        
        benchmarkRunner.AddBenchmarkCategory("transient-register-and-build", new Scenario[]
        {
            new SparseInjectTransientRegisterAndBuildScenario(),
            new VContainerTransientRegisterAndBuildScenario(),
            new AutofacTransientRegisterAndBuildScenario(),
            new LightInjectTransientRegisterAndBuildScenario(),
        }, 5);
        
        benchmarkRunner.AddBenchmarkCategory("transient-resolve", new Scenario[]
        {
            new SparseInjectTransientResolveScenario(),
            new VContainerTransientResolveScenario(),
            new AutofacTransientResolveScenario(),
            new NativeTransientResolveScenario(),
            new LightInjectTransientResolveScenario(),
        }, 5);
        
        benchmarkRunner.AddBenchmarkCategory("transient-total", new Scenario[]
        {
            new SparseInjectTransientTotalScenario(),
            new VContainerTransientTotalScenario(),
            new AutofacTransientTotalScenario(),
            new NativeTransientTotalScenario(),
            new LightInjectTransientTotalScenario(),
        }, 5);
        
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
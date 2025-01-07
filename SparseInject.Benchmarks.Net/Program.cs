using SparseInject.BenchmarkFramework;

namespace SparseInject.Benchmarks.Net;

public class Program
{
    public static void Main(string[] args)
    {
        var benchmarkRunner = new BenchmarkRunner(args, new DotNetMemorySnapshotFactory());
        
        benchmarkRunner.AddBenchmarkCategory("transient-register", new Benchmark[]
        {
            new SparseInjectTransientRegisterBenchmark(),
            new VContainerTransientRegisterBenchmark(),
            new AutofacTransientRegisterBenchmark(),
            new LightInjectTransientRegisterBenchmark(),
        }, 10);
        
        benchmarkRunner.AddBenchmarkCategory("transient-build", new Benchmark[]
        {
            new SparseInjectTransientBuildBenchmark(),
            new VContainerTransientBuildBenchmark(),
            new AutofacTransientBuildBenchmark(),
        }, 10);
        
        benchmarkRunner.AddBenchmarkCategory("transient-register-and-build", new Benchmark[]
        {
            new SparseInjectTransientRegisterAndBuildBenchmark(),
            new VContainerTransientRegisterAndBuildBenchmark(),
            new AutofacTransientRegisterAndBuildBenchmark(),
            //new LightInjectTransientRegisterAndBuildBenchmark(),
        }, 10);
        
        benchmarkRunner.AddBenchmarkCategory("transient-resolve", new Benchmark[]
        {
            new SparseInjectTransientResolveBenchmark(),
            new VContainerTransientResolveBenchmark(),
            new AutofacTransientResolveBenchmark(),
            //new LightInjectTransientResolveBenchmark(),
        }, 10);
        
        benchmarkRunner.AddBenchmarkCategory("transient-total", new Benchmark[]
        {
            new SparseInjectTransientTotalBenchmark(),
            new VContainerTransientTotalBenchmark(),
            new AutofacTransientTotalBenchmark(),
            //new LightInjectTransientTotalBenchmark(),
        }, 10);
        
        var summary = benchmarkRunner.Run();

        var isBenchmarkLauncher = args == null || args.Length == 0;
        
        if (isBenchmarkLauncher)
        {
            Console.ReadLine();
        }
    }
}
using System.Diagnostics;

namespace SparseInject.BenchmarkFramework;

public class BenchmarkRunner
{
    private string[] _args;
    private readonly IMemorySnapshotFactory _memorySnapshotFactory;
    private readonly List<BenchmarkCategory> _categories;

    public BenchmarkRunner(string[] args, IMemorySnapshotFactory memorySnapshotFactory)
    {
        _args = args;
        _memorySnapshotFactory = memorySnapshotFactory;
        _categories = new List<BenchmarkCategory>(8);
    }
    
    public void AddBenchmarkCategory(string categoryName, Benchmark[] benchmarks, int samples = 1)
    {
        _categories.Add(new BenchmarkCategory(categoryName, benchmarks, samples));
    }

    public string Run()
    {
        if (_args == null || _args.Length == 0)
        {
            var args = new List<string>(64);

            foreach (var category in _categories)
            {
                foreach (var benchmark in category.Benchmarks)
                {
                    args.Add($"--run-benchmark {category.Name}:{benchmark.Name}");
                }
            }

            foreach (var arguments in args)
            {
                var executablePath = Process.GetCurrentProcess().MainModule.FileName;

                var processStartInfo = new ProcessStartInfo
                {
                    FileName = executablePath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false, // Required to redirect streams
                    CreateNoWindow = true    // Don't create a window
                };
                
                using (var process = Process.Start(processStartInfo))
                {
                    if (process == null)
                    {
                        throw new InvalidOperationException("Failed to start benchmark process.");
                    }

                    // Read the output streams
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new InvalidOperationException($"Benchmark process exited with code {process.ExitCode}: {error}");
                    }

                    Console.WriteLine(output.Trim());
                }
            }
        }
        else
        {
            var singleArgument = string.Join(" ", _args);

            var benchmarkInfo = GetBenchmarkInfoByArguments(singleArgument);
            var benchmark = benchmarkInfo.benchmark;
            
            var stopwatch = Stopwatch.StartNew();
            stopwatch.Stop();
            stopwatch.Restart();
            
            benchmark.BeforeExecute();
                
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            stopwatch.Restart();
            benchmark.Execute();
            stopwatch.Stop();

            var memorySnapshot = _memorySnapshotFactory.Create();

            Console.WriteLine($"[{benchmarkInfo.category.Name}] {benchmark.Name}: {stopwatch.Elapsed.TotalMilliseconds.ToString("F2")} ms / {memorySnapshot.PrivateMemoryMb.ToString("F2")} mb");
        }

        return string.Empty;
    }

    public string RunBenchmark(Benchmark benchmark)
    {
        var executablePath = Process.GetCurrentProcess().MainModule.FileName;

        var arguments = $"--run-benchmark \"{benchmark.Name}\"";

        var processStartInfo = new ProcessStartInfo
        {
            FileName = executablePath,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false, // Required to redirect streams
            CreateNoWindow = true    // Don't create a window
        };

        try
        {
            using (var process = Process.Start(processStartInfo))
            {
                if (process == null)
                {
                    throw new InvalidOperationException("Failed to start benchmark process.");
                }

                // Read the output streams
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException($"Benchmark process exited with code {process.ExitCode}: {error}");
                }

                return output.Trim();
            }
        }
        catch (Exception ex)
        {
            return $"Error running benchmark {benchmark.Name}: {ex.Message}";
        }
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
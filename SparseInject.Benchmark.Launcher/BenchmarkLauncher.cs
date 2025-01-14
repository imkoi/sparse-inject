using System.Diagnostics;

namespace SparseInject.Benchmark.Launcher;

public static class BenchmarkLauncher
{
    public static void LaunchWithDepth(
        string executablePath,
        IEnumerable<string> benchmarks,
        IEnumerable<string> scenarios,
        int maxDepth,
        int samples)
    {
        var exeArguments = new List<string>();

        foreach (var benchmarksName in benchmarks)
        {
            foreach (var scenarioName in scenarios)
            {
                for (var depth = 0; depth < maxDepth + 1; depth++)
                {
                    var arguments = $"--run-benchmark {benchmarksName}{depth}:{scenarioName}";
            
                    exeArguments.Add(arguments);
                }   
            }
        }
        
        LaunchInternal(executablePath, exeArguments, samples);
    }
    
    public static void Launch(
        string executablePath,
        IEnumerable<string> benchmarks,
        IEnumerable<string> scenarios,
        int sampels)
    {
        var exeArguments = new List<string>();

        foreach (var benchmarksName in benchmarks)
        {
            foreach (var scenarioName in scenarios)
            {
                var arguments = $"--run-benchmark {benchmarksName}:{scenarioName}";
            
                exeArguments.Add(arguments);
            }
        }
        
        LaunchInternal(executablePath, exeArguments, sampels);
    }

    private static void LaunchInternal(string executablePath, List<string> exeArguments, int sampels)
    {
        var benchmarkIndex = 0;

        foreach (var arguments in exeArguments)
        {
            for (var i = 0; i < sampels; i++)
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = executablePath,
                    Arguments = arguments,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(processStartInfo);
        
                if (process == null)
                {
                    throw new InvalidOperationException("Failed to start benchmark process.");
                }

                var error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException($"Benchmark process exited with code {process.ExitCode}: {error}");
                }

                benchmarkIndex++;

                var progress = (benchmarkIndex * 1f / exeArguments.Count / sampels) * 100f;
    
                Console.WriteLine(progress);
            }
        }
    }
}
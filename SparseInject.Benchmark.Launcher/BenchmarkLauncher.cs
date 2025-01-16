using System.Diagnostics;

namespace SparseInject.Benchmark.Launcher;

public static class BenchmarkLauncher
{
    public enum Platform
    {
        Windows,
        Android
    }
    
    public static Task LaunchWithDepth(
        string executablePath,
        IEnumerable<string> benchmarks,
        IEnumerable<string> scenarios,
        Platform platform,
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
                    var arguments = $"run-benchmark-{benchmarksName}{depth}:{scenarioName}";
            
                    exeArguments.Add(arguments);
                }   
            }
        }
        
        return LaunchInternal(executablePath, exeArguments, platform, samples);
    }
    
    public static Task Launch(
        string executablePath,
        IEnumerable<string> benchmarks,
        IEnumerable<string> scenarios,
        Platform platform,
        int sampels)
    {
        var exeArguments = new List<string>();

        foreach (var benchmarksName in benchmarks)
        {
            foreach (var scenarioName in scenarios)
            {
                var arguments = $"run-benchmark-{benchmarksName}:{scenarioName}";
            
                exeArguments.Add(arguments);
            }
        }
        
        return LaunchInternal(executablePath, exeArguments, platform, sampels);
    }

    private static async Task LaunchInternal(string executablePath, List<string> exeArguments, Platform platform, int sampels)
    {
        var benchmarkIndex = 0;

        foreach (var arguments in exeArguments)
        {
            for (var i = 0; i < sampels; i++)
            {
                (bool isSuccess, string error) status;

                if (platform == Platform.Windows)
                {
                    status = await LaunchForWindows(executablePath, arguments);
                }
                else if (platform == Platform.Android)
                {
                    status = await LaunchForAndroid(executablePath, arguments);
                }
                else
                {
                    throw new NotImplementedException($"Platform {platform} is not supported");
                }
                
                if (!status.isSuccess)
                {
                    Console.WriteLine($"Benchmark process exited with error: {status.error}");

                    i--;
                    
                    continue;
                }

                benchmarkIndex++;

                var progress = (benchmarkIndex * 1f / exeArguments.Count / sampels) * 100f;
    
                Console.WriteLine(progress);
            }
        }
    }

    private static Task<(bool isSuccess, string error)> LaunchForWindows(string executablePath, string arguments)
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

        return Task.FromResult((process.ExitCode == 0, error));
    }
    
    private static async Task<(bool isSuccess, string error)> LaunchForAndroid(string packageName, string arguments)
    {
        RunAdbCommand($"shell am start -n {packageName}/com.unity3d.player.UnityPlayerActivity -e unity \"{arguments}\"", out var startProcessError);

        if (!string.IsNullOrEmpty(startProcessError))
        {
            return (false, startProcessError);
        }

        await Task.Delay(250);

        while (RunAdbCommand("shell ps -A", out var getProcessError).Contains(packageName))
        {
            if (!string.IsNullOrEmpty(getProcessError))
            {
                return (false, getProcessError);
            }
            
            await Task.Delay(1000);
        }

        return (true, string.Empty);
    }
    
    private static string RunAdbCommand(string arguments, out string error)
    {
        var systemPaths = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine).Split(";");
        
        var adbPath = systemPaths
            .Select(path => Directory.GetFiles(path, "adb.exe", SearchOption.TopDirectoryOnly).FirstOrDefault())
            .First(path => !string.IsNullOrEmpty(path));
        
        var process = new Process();
        process.StartInfo.FileName = adbPath;
        process.StartInfo.Arguments = arguments;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.Start();

        var output = process.StandardOutput.ReadToEnd().Trim();
        error = process.StandardError.ReadToEnd().Trim();

        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception(error);
        }
        
        error = string.Empty;
        
        return output;
    }
}
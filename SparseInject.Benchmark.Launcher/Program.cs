using System.Diagnostics;

const int maxDepth = 6;
const string executablePath = "C:/github/sparseinject/SparseInject.Benchmark.Unity/Build/il2cpp/SparseInject.Benchmark.Unity.exe";

var benchmarkCategoryNames = new List<string>
{
    "singleton-register-depth",
    "singleton-build-depth",
    "singleton-register-and-build-depth",
    "singleton-first-resolve-depth",
    "singleton-second-resolve-depth",
    "singleton-total-depth",
    "transient-register-depth",
    "transient-build-depth",
    "transient-register-and-build-depth",
    "transient-first-resolve-depth",
    "transient-second-resolve-depth",
    "transient-total-depth",
};

var scenarioNames = new List<string>
{
    "SparseInject",
    "VContainer",
    "Reflex",
    "Zenject"
};

var exeArguments = new List<string>();

foreach (var benchmarksName in benchmarkCategoryNames)
{
    foreach (var scenarioName in scenarioNames)
    {
        for (var depth = 0; depth < maxDepth + 1; depth++)
        {
            var arguments = $"--run-benchmark {benchmarksName}{depth}:{scenarioName}";
            
            exeArguments.Add(arguments);
        }   
    }
}

var benchmarkIndex = 0;

foreach (var arguments in exeArguments)
{
    for (var i = 0; i < 10; i++)
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

        var progress = (benchmarkIndex * 1f / exeArguments.Count / 10) * 100f;
    
        Console.WriteLine(progress);
    }
}

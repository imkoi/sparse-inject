using System;
using System.Diagnostics;
using System.IO;

namespace SparseInject.BenchmarkFramework
{
    public class DotNetBenchmarkMeasurer : IBenchmarkMeasurer
    {
        public void Measure(string categoryName, string benchmarkName)
        {
            var arguments = $"{BenchmarkConstants.RunBenchmarkCommand} {categoryName}:{benchmarkName}";

            var processModule = Process.GetCurrentProcess().MainModule;
            var executablePath = processModule?.FileName;
            
            if (!File.Exists(executablePath))
            {
                throw new FileNotFoundException($"Executable not found: {executablePath}");
            }
            
            if (!string.IsNullOrEmpty(executablePath))
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
            }
        }
    }
}
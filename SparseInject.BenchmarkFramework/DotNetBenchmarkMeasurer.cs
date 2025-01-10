#if NET
using System;
using System.Diagnostics;

namespace SparseInject.BenchmarkFramework
{
    public class DotNetBenchmarkMeasurer : IBenchmarkMeasurer
    {
        public void Measure(string categoryName, string benchmarkName)
        {
            var arguments = $"{BenchmarkConstants.RunBenchmarkCommand} {categoryName}:{benchmarkName}";
            var processModule = Process.GetCurrentProcess().MainModule;
            
            if (processModule != null)
            {
                var executablePath = processModule.FileName;

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
#endif
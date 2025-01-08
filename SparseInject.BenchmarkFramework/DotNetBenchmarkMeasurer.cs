#if NET
using System;
using System.Diagnostics;

namespace SparseInject.BenchmarkFramework
{
    public class DotNetBenchmarkMeasurer : IBenchmarkMeasurer
    {
        public void Measure(string categoryName, string benchmarkName, int samples, string args, IReportStorage storage)
        {
            for (var i = 0; i < samples; i++)
            {
                var executablePath = Process.GetCurrentProcess().MainModule.FileName;

                var processStartInfo = new ProcessStartInfo
                {
                    FileName = executablePath,
                    Arguments = args,
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
                    var output = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new InvalidOperationException($"Benchmark process exited with code {process.ExitCode}: {error}");
                    }

                    Console.WriteLine(output.Trim());
                }
            }
        }
    }
}
#endif
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SparseInject.BenchmarkFramework
{
    public class TaskUtility
    {
        public static async Task WaitForSecondsAsync(TimeSpan timeSpan, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed < timeSpan && !cancellationToken.IsCancellationRequested)
            {
                await Task.Yield();
            }
            
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
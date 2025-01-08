#if NET
using System;

namespace SparseInject.BenchmarkFramework
{
    public class DotNetResourceCleaner : IResourceCleaner
    {
        public void CleanResources()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
#endif
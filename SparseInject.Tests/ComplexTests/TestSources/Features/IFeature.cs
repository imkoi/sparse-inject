using System;
using System.Threading;
using System.Threading.Tasks;

namespace SparseInject.Tests.ComplexTests
{
    public interface IFeature : IDisposable
    {
        public bool IsEnabled { get; }
        public bool IsActive { get; }
        
        Task InitializeOfflineFunctionalAsync(CancellationToken cancellationToken);
        Task InitializeOnlineFunctionalAsync(CancellationToken cancellationToken);
        Task LaunchAsync(CancellationToken cancellationToken);
    }
}
using System.Threading;
using System.Threading.Tasks;

namespace SparseInject.Tests.ComplexTests
{
    public class LeaderboardFeature : Scope, IFeature
    {
        public bool IsEnabled { get; }
        public bool IsActive { get; }
        
        public Task InitializeOfflineFunctionalAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task InitializeOnlineFunctionalAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task LaunchAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
using System.Threading;
using System.Threading.Tasks;

namespace SparseInject.Tests.ComplexTests
{
    public class ShopFeature : Scope, IFeature
    {
        public bool IsEnabled { get; }
        public bool IsActive { get; }
        
        private readonly ICurrencyService _currencyService;

        public ShopFeature(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }
        
        public Task InitializeOfflineFunctionalAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
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
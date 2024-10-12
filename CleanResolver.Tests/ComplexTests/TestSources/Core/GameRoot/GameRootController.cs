using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanResolver.Tests.ComplexTests
{
    public class GameRootController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IFeature[] _features;

        public GameRootController(
            IAuthorizationService authorizationService,
            IFeature[] features)
        {
            _authorizationService = authorizationService;
            _features = features;
        }
        
        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            await Task.WhenAll(_features.Select(feature => feature.InitializeOfflineFunctionalAsync(cancellationToken)));
            
            var authorizationResult = await _authorizationService.LoginAsync(cancellationToken);

            if (authorizationResult.IsOnline)
            {
                await Task.WhenAll(_features.Select(feature => feature.InitializeOnlineFunctionalAsync(cancellationToken)));
            }
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.WhenAll(_features.Select(feature => feature.LaunchAsync(cancellationToken)));
        }
    }
}
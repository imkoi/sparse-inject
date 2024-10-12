using System.Threading;
using System.Threading.Tasks;

namespace CleanResolver.Tests.ComplexTests
{
    public class AuthorizationService : IAuthorizationService
    {
        public Task<AuthorizationResult> LoginAsync(CancellationToken token)
        {
            return Task.FromResult(new AuthorizationResult(true, true));
        }
    }
}
using System.Threading;
using System.Threading.Tasks;

namespace CleanResolver.Tests.ComplexTests
{
    public interface IAuthorizationService
    {
        Task<AuthorizationResult> LoginAsync(CancellationToken token);
    }
}
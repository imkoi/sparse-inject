using System.Threading;
using System.Threading.Tasks;

namespace SparseInject.Tests.ComplexTests
{
    public interface IAuthorizationService
    {
        Task<AuthorizationResult> LoginAsync(CancellationToken token);
    }
}
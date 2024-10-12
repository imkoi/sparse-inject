using System.Threading;
using System.Threading.Tasks;

namespace CleanResolver.Tests.ComplexTests
{
    public interface IPopupService
    {
        public Task ShowPopupAsync(CancellationToken token);
    }
}
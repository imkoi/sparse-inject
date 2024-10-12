using System.Threading;
using System.Threading.Tasks;

namespace SparseInject.Tests.ComplexTests
{
    public interface IPopupService
    {
        public Task ShowPopupAsync(CancellationToken token);
    }
}
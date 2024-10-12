using System.Threading;
using System.Threading.Tasks;

namespace SparseInject.Tests.ComplexTests
{
    public class PopupService : IPopupService
    {
        public Task ShowPopupAsync(CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
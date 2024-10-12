namespace SparseInject.Tests.ComplexTests
{
    public interface IMainScreenService
    {
        public MainScreenNavigationButtonId AddNavigationButton();
        public void RemoveNavigationButton(MainScreenNavigationButtonId navigationButtonId);
    }
}
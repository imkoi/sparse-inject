namespace SparseInject.Tests.ComplexTests
{
    public static class CommonRegistrator
    {
        public static void Register(IScopeBuilder scopeBuilder)
        {
            scopeBuilder.Register<IPopupService, PopupService>();
        }
    }
}
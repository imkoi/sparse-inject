namespace SparseInject.Tests.ComplexTests
{
    public static class CoreRegistrator
    {
        public static void Register(IScopeBuilder scopeBuilder)
        {
            scopeBuilder.Register<GameRootController>();
            
            scopeBuilder.Register<IAuthorizationService, AuthorizationService>(Lifetime.Singleton);
            
            scopeBuilder.Register<ILoadingScreenService, LoadingScreenService>(Lifetime.Singleton);
            
            scopeBuilder.Register<IMainScreenService, MainScreenService>(Lifetime.Singleton);
            
            scopeBuilder.Register<ICurrencyService, CurrencyService>(Lifetime.Singleton);
        }
    }
}
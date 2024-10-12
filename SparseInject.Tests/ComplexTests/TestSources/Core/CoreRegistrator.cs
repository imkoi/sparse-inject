namespace SparseInject.Tests.ComplexTests
{
    public static class CoreRegistrator
    {
        public static void Register(IScopeBuilder scopeBuilder)
        {
            scopeBuilder.Register<GameRootController>();
            
            scopeBuilder.Register<IAuthorizationService, AuthorizationService>(RegisterType.Singleton);
            
            scopeBuilder.Register<ILoadingScreenService, LoadingScreenService>(RegisterType.Singleton);
            
            scopeBuilder.Register<IMainScreenService, MainScreenService>(RegisterType.Singleton);
            
            scopeBuilder.Register<ICurrencyService, CurrencyService>(RegisterType.Singleton);
        }
    }
}
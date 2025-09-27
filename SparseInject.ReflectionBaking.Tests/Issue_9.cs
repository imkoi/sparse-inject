// https://github.com/imkoi/sparse-inject/issues/9

using SparseInject;

namespace Test
{
    public static class Test 
    {
        public static void Register(IScopeBuilder containerBuilder)
        {
            containerBuilder.Register<TestService>(Lifetime.Singleton);
        }
    }

    public class TestService { }
}
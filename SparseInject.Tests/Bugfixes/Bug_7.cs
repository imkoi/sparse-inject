using FluentAssertions;
using NUnit.Framework;

namespace SparseInject.Tests.Bugfixes
{
    public class Bug_7
    {
        interface IGlobalService {}
        class GlobalService : IGlobalService {}

        class SampleScope : Scope {}

        interface IScopedService {}
        class ScopedService : IScopedService {}

        class ScopedServiceWithDependencies
        {
            public ScopedServiceWithDependencies(IGlobalService globalService, IScopedService scopedService)
            {
                
            }
        }
        
        [Test]
        public void Test()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.Register<IGlobalService, GlobalService>();
            containerBuilder.RegisterScope<SampleScope>(sampleScopeBuilder =>
            {
                sampleScopeBuilder.Register<IScopedService, ScopedService>(Lifetime.Singleton);
                sampleScopeBuilder.Register<ScopedServiceWithDependencies>(Lifetime.Singleton);
            });

            var container = containerBuilder.Build();
            var globalScope = container.Resolve<SampleScope>();

            globalScope.Should().NotBeNull();
        }
    }
}
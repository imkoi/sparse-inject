using System.Threading;
using NUnit.Framework;
using SparseInject;
using SparseInject.Tests.ComplexTests;

[TestFixture]
public class ComplexTests
{
    [Test]
    public void Test()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register(CommonRegistrator.Register);
        containerBuilder.Register(CoreRegistrator.Register);
        
        containerBuilder.RegisterScope<IFeature, ShopFeature>((builder, parentScope) =>
        {
            //builder.
        });
        
        containerBuilder.RegisterScope<IFeature, LeaderboardFeature>((builder, parentScope) =>
        {
            //builder.
        });

        var container = containerBuilder.Build();
        
        var gameRootController = container.Resolve<GameRootController>();
        
        gameRootController.InitializeAsync(CancellationToken.None).GetAwaiter().GetResult();
        gameRootController.ExecuteAsync(CancellationToken.None).GetAwaiter().GetResult();
    }
}
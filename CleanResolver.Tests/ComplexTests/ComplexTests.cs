using System.Threading;
using CleanResolver;
using CleanResolver.Tests.ComplexTests;
using CleanResolver.Tests.ComplexTests.Leaderboard;
using CleanResolver.Tests.ComplexTests.Shop;
using NUnit.Framework;

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
        
        // containerBuilder.RegisterScope<IFeature, FeatureA>((builder, parentScope) =>
        // {
        //     //builder.
        // });

        var container = containerBuilder.Build();
        
        var gameRootController = container.Resolve<GameRootController>();
        
        gameRootController.InitializeAsync(CancellationToken.None).GetAwaiter().GetResult();
        gameRootController.ExecuteAsync(CancellationToken.None).GetAwaiter().GetResult();
    }
}
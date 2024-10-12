using NUnit.Framework;
using SparseInject;
using SparseInject.Tests.Scopes;

[TestFixture]
public class ScopeTests
{
    [Test]
    public void ScopeTest()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<GameController>();
            
        containerBuilder.RegisterScope<FeatureScope>(configurator =>
        {
            configurator.Register<IFeaturePopup, FeaturePopup>();
        });

        var container = containerBuilder.Build();

        var gameController = container.Resolve<GameController>();
            
        Assert.DoesNotThrow(gameController.Execute);
    }
    
    [Test]
    public void ScopeTest_2()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<GameController>();
        containerBuilder.Register<RewardService>();
            
        containerBuilder.RegisterScope<FeatureScope>(configurator =>
        {
            configurator.Register<IFeaturePopup, FeatureRewardPopup>();
        });

        var container = containerBuilder.Build();

        var gameController = container.Resolve<GameController>();
            
        Assert.DoesNotThrow(gameController.Execute);
    }
}
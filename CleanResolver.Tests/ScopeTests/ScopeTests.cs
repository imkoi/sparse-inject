using CleanResolver;
using CleanResolver.Tests.Scopes;
using NUnit.Framework;

[TestFixture]
public class ScopeTests
{
    [Test]
    public void ScopeTest()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<GameController>();
            
        containerBuilder.RegisterScope<LevelScope>(configurator =>
        {
            configurator.Register<LevelEndPopup>();
        });

        var container = containerBuilder.Build();

        var gameController = container.Resolve<GameController>();
            
        Assert.DoesNotThrow(gameController.Execute);
    }
}
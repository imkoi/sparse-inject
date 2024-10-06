using NUnit.Framework;

namespace CleanResolver.Tests.Scopes
{
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
            
            container.Resolve<LevelEndPopup>();
            
            Assert.DoesNotThrow(gameController.Execute);
        }
    }
}
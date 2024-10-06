using FluentAssertions;
using NUnit.Framework;

namespace CleanResolver.Tests.Collection
{
    [TestFixture]
    public class CollectionTests
    {
        private ContainerBuilder _containerBuilder;
    
        [SetUp]
        public void Setup()
        {
            _containerBuilder = new ContainerBuilder();
        }
        
        [Test]
        public void TransientBinding()
        {
            _containerBuilder.Register<PlayerController>();
            _containerBuilder.Register<IPlayerControllerProcessor, PlayerMovementProcessor>();
            _containerBuilder.Register<IPlayerControllerProcessor, PlayerShootingProcessor>();
            _containerBuilder.Register<IPlayerControllerProcessor, PlayerAnimationProcessor>();

            var container = _containerBuilder.Build();
            var processors = container.Resolve<IPlayerControllerProcessor[]>();

            processors.Length.Should().Be(3);
        }
        
        [Test]
        public void UnorderedBindings()
        {
            _containerBuilder.Register<IPlayerControllerProcessor, PlayerMovementProcessor>();
            _containerBuilder.Register<IPlayerControllerProcessor, PlayerAnimationProcessor>();
            
            _containerBuilder.Register<PlayerController>();
            
            _containerBuilder.Register<IPlayerControllerProcessor, PlayerShootingProcessor>();

            var container = _containerBuilder.Build();
            var processors = container.Resolve<IPlayerControllerProcessor[]>();
            
            processors.Length.Should().Be(3);
        }
    }
}
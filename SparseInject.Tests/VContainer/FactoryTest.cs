using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SparseInject;

namespace VContainer.Tests
{
    public class Player : IPlayer
    {
        public int MaxHealth { get; set; }
        public IAudioService AudioService { get; set; }
    }
    
    public interface IPlayer
    {
        int MaxHealth { get; }
        IAudioService AudioService { get; }
    }
    
    public interface IAudioService { }

    [TestFixture]
    public class FactoryTest
    {
        [Test]
        public void RegisteredFactory_WhenResolvedAndInvoked_ReturnCorrectValues()
        {
            // Setup
            var builder = new ContainerBuilder();

            builder.RegisterFactory(() => new Player());
            
            var container = builder.Build();

            // Asserts
            var value = container.Resolve<Func<Player>>().Invoke();
            
            value.Should().BeOfType<Player>();
        }
        
        [Test]
        public void RegisteredFactory_WhenResolvedAndInvokedMultipleTimes_ReturnDifferentValues()
        {
            // Setup
            var builder = new ContainerBuilder();

            builder.RegisterFactory(() => new Player());
            
            var container = builder.Build();
            var factory = container.Resolve<Func<Player>>();

            // Asserts
            var firstValue = factory.Invoke();
            var secondValue = factory.Invoke();

            firstValue.Should().NotBe(secondValue);
        }
        
        [Test]
        public void RegisteredFactoryToInterface_WhenResolvedAndInvokedMultipleTimes_ReturnDifferentValues()
        {
            // Setup
            var builder = new ContainerBuilder();

            builder.RegisterFactory<IPlayer>(() => new Player());
            
            var container = builder.Build();
            var factory = container.Resolve<Func<IPlayer>>();

            // Asserts
            var firstValue = factory.Invoke();
            var secondValue = factory.Invoke();

            firstValue.Should().NotBe(secondValue);
        }
        
        [Test]
        public void RegisteredFactory_WhenResolvedMultipleTimes_ReturnSameFactory()
        {
            // Setup
            var builder = new ContainerBuilder();

            builder.RegisterFactory(() => new Player());
            
            var container = builder.Build();
            
            // Asserts
            var firstFactory = container.Resolve<Func<Player>>();
            var secondFactory = container.Resolve<Func<Player>>();

            firstFactory.Should().BeSameAs(secondFactory);
        }
        
        [Test]
        public void UnregisteredFactory_WhenResolved_ThrowProperException()
        {
            // Setup
            var builder = new ContainerBuilder();
            var container = builder.Build();
            
            // Asserts
            container
                .Invoking(subject => subject.Resolve<Func<Player>>())
                .Should()
                .Throw<SparseInjectException>();
        }
        
        [Test]
        public void RegisteredFactoryWithParameter_WhenResolvedAndInvoked_ReturnCorrectValues()
        {
            // Setup
            var builder = new ContainerBuilder();

            builder.RegisterFactory<int, Player>(parameter => new Player
            {
                MaxHealth = parameter
            });
            
            var container = builder.Build();

            // Asserts
            var value = container.Resolve<Func<int, Player>>().Invoke(100);
            
            value.Should().BeOfType<Player>();
            value.MaxHealth.Should().Be(100);
        }
        
        [Test]
        public void RegisteredFactoryWithParameter_WhenResolvedAndInvokedMultipleTimes_ReturnDifferentValues()
        {
            // Setup
            var builder = new ContainerBuilder();

            builder.RegisterFactory<int, Player>(parameter => new Player { MaxHealth = parameter });
            
            var container = builder.Build();
            var factory = container.Resolve<Func<int, Player>>();

            // Asserts
            var firstValue = factory.Invoke(100);
            var secondValue = factory.Invoke(25);

            firstValue.Should().NotBe(secondValue);
            
            firstValue.MaxHealth.Should().Be(100);
            secondValue.MaxHealth.Should().Be(25);
        }
        
        [Test]
        public void RegisteredFactoryToInterfaceWithParameter_WhenResolvedAndInvokedMultipleTimes_ReturnDifferentValues()
        {
            // Setup
            var builder = new ContainerBuilder();

            builder.RegisterFactory<int, IPlayer, Player>(parameter => new Player { MaxHealth = parameter });
            
            var container = builder.Build();
            var factory = container.Resolve<Func<int, IPlayer>>();

            // Asserts
            var firstValue = factory.Invoke(100);
            var secondValue = factory.Invoke(25);

            firstValue.Should().NotBe(secondValue);
            
            firstValue.MaxHealth.Should().Be(100);
            secondValue.MaxHealth.Should().Be(25);
        }
        
        [Test]
        public void RegisteredFactoryWithParameter_WhenResolvedMultipleTimes_ReturnSameFactory()
        {
            // Setup
            var builder = new ContainerBuilder();

            builder.RegisterFactory<int, Player>(parameter => new Player());
            
            var container = builder.Build();
            
            // Asserts
            var firstFactory = container.Resolve<Func<int, Player>>();
            var secondFactory = container.Resolve<Func<int, Player>>();

            firstFactory.Should().Be(secondFactory);
        }
        
        [Test]
        public void UnregisteredFactoryWithParameter_WhenResolved_ThrowProperException()
        {
            // Setup
            var builder = new ContainerBuilder();
            var container = builder.Build();
            
            // Asserts
            container
                .Invoking(subject => subject.Resolve<Func<int, Player>>())
                .Should()
                .Throw<SparseInjectException>();
        }
        
        [Test]
        public void RegisteredFactoryWithContainer_WhenResolvedAndInvoked_ReturnCorrectValue()
        {
            // Setup
            var builder = new ContainerBuilder();
            var audioService = Substitute.For<IAudioService>();

            builder.RegisterValue(audioService);
            builder.RegisterFactory(container => new Player { AudioService = container.Resolve<IAudioService>() });
            
            var container = builder.Build();

            // Asserts
            var value = container.Resolve<Func<Player>>().Invoke();
            
            value.Should().BeOfType<Player>();
            value.AudioService.Should().Be(audioService);
        }
        
        [Test]
        public void RegisteredFactoryWithContainer_WhenResolvedAndInvokedMultipleTimes_ReturnDifferentValues()
        {
            // Setup
            var builder = new ContainerBuilder();
            var audioService = Substitute.For<IAudioService>();

            builder.RegisterValue(audioService);
            builder.RegisterFactory(container => new Player { AudioService = container.Resolve<IAudioService>() });
            
            var container = builder.Build();
            var factory = container.Resolve<Func<Player>>();

            // Asserts
            var firstValue = factory.Invoke();
            var secondValue = factory.Invoke();

            firstValue.Should().NotBe(secondValue);

            firstValue.AudioService.Should().Be(audioService);
            firstValue.AudioService.Should().Be(secondValue.AudioService);
        }
        
        [Test]
        public void RegisteredFactoryToInterfaceWithContainer_WhenResolvedAndInvokedMultipleTimes_ReturnDifferentValues()
        {
            // Setup
            var builder = new ContainerBuilder();
            var audioService = Substitute.For<IAudioService>();

            builder.RegisterValue(audioService);
            builder.RegisterFactory<IPlayer, Player>(container => new Player { AudioService = container.Resolve<IAudioService>() });
            
            var container = builder.Build();
            var factory = container.Resolve<Func<IPlayer>>();

            // Asserts
            var firstValue = factory.Invoke();
            var secondValue = factory.Invoke();

            firstValue.Should().NotBe(secondValue);

            firstValue.AudioService.Should().Be(audioService);
            firstValue.AudioService.Should().Be(secondValue.AudioService);
        }
        
        [Test]
        public void RegisteredFactoryWithContainer_WhenResolvedMultipleTimes_ReturnSameFactory()
        {
            // Setup
            var builder = new ContainerBuilder();
            var audioService = Substitute.For<IAudioService>();

            builder.RegisterValue(audioService);
            builder.RegisterFactory(container => new Player { AudioService = container.Resolve<IAudioService>() });
            
            var container = builder.Build();
            
            // Asserts
            var firstFactory = container.Resolve<Func<Player>>();
            var secondFactory = container.Resolve<Func<Player>>();

            firstFactory.Should().BeSameAs(secondFactory);
        }
        
        [Test]
        public void UnregisteredFactoryWithContainer_WhenResolved_ThrowProperException()
        {
            // Setup
            var builder = new ContainerBuilder();
            var container = builder.Build();
            
            // Asserts
            container
                .Invoking(subject => subject.Resolve<Func<Player>>())
                .Should()
                .Throw<SparseInjectException>();
        }
        
        [Test]
        public void RegisteredFactoryWithContainerAndParameter_WhenResolvedAndInvoked_ReturnCorrectValue()
        {
            // Setup
            var builder = new ContainerBuilder();
            var audioService = Substitute.For<IAudioService>();

            builder.RegisterValue(audioService);
            builder.RegisterFactory<int, Player>(container => maxHealth => new Player
            {
                MaxHealth = maxHealth,
                AudioService = container.Resolve<IAudioService>()
            });
            
            var container = builder.Build();

            // Asserts
            var value = container.Resolve<Func<int, Player>>().Invoke(100);
            
            value.Should().BeOfType<Player>();
            value.AudioService.Should().Be(audioService);
            value.MaxHealth.Should().Be(100);
        }
        
        [Test]
        public void RegisteredFactoryWithContainerAndParameter_WhenResolvedAndInvokedMultipleTimes_ReturnDifferentValues()
        {
            // Setup
            var builder = new ContainerBuilder();
            var audioService = Substitute.For<IAudioService>();

            builder.RegisterValue(audioService);
            builder.RegisterFactory<int, Player>(container => maxHealth => new Player
            {
                MaxHealth = maxHealth,
                AudioService = container.Resolve<IAudioService>()
            });
            
            var container = builder.Build();
            var factory = container.Resolve<Func<int, Player>>();

            // Asserts
            var firstValue = factory.Invoke(100);
            var secondValue = factory.Invoke(25);

            firstValue.Should().NotBe(secondValue);

            firstValue.AudioService.Should().Be(audioService);
            firstValue.AudioService.Should().Be(secondValue.AudioService);
            
            firstValue.MaxHealth.Should().Be(100);
            secondValue.MaxHealth.Should().Be(25);
        }
        
        [Test]
        public void RegisteredFactoryToInterfaceWithContainerAndParameter_WhenResolvedAndInvokedMultipleTimes_ReturnDifferentValues() // TODO: implement
        {
            // Setup
            var builder = new ContainerBuilder();
            var audioService = Substitute.For<IAudioService>();

            builder.RegisterValue(audioService);
            builder.RegisterFactory<int, IPlayer, Player>(container => maxHealth => new Player
            {
                MaxHealth = maxHealth,
                AudioService = container.Resolve<IAudioService>()
            });
            
            var container = builder.Build();
            var factory = container.Resolve<Func<int, IPlayer>>();

            // Asserts
            var firstValue = factory.Invoke(100);
            var secondValue = factory.Invoke(25);

            firstValue.Should().NotBe(secondValue);

            firstValue.AudioService.Should().Be(audioService);
            firstValue.AudioService.Should().Be(secondValue.AudioService);
            
            firstValue.MaxHealth.Should().Be(100);
            secondValue.MaxHealth.Should().Be(25);
        }
        
        [Test]
        public void RegisteredFactoryWithContainerAndParameter_WhenResolvedMultipleTimes_ReturnSameFactory()
        {
            // Setup
            var builder = new ContainerBuilder();
            var audioService = Substitute.For<IAudioService>();

            builder.RegisterValue(audioService);
            builder.RegisterFactory<int, Player>(container => maxHealth => new Player
            {
                MaxHealth = maxHealth,
                AudioService = container.Resolve<IAudioService>()
            });
            
            var container = builder.Build();
            
            // Asserts
            var firstFactory = container.Resolve<Func<int, Player>>();
            var secondFactory = container.Resolve<Func<int, Player>>();

            firstFactory.Should().BeSameAs(secondFactory);
        }
        
        [Test]
        public void UnregisteredFactoryWithContainerAndParameter_WhenResolved_ThrowProperException()
        {
            // Setup
            var builder = new ContainerBuilder();
            var container = builder.Build();
            
            // Asserts
            container
                .Invoking(subject => subject.Resolve<Func<int, Player>>())
                .Should()
                .Throw<SparseInjectException>();
        }
    }
}


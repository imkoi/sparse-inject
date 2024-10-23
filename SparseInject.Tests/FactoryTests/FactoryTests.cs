using System;
using FluentAssertions;
using NUnit.Framework;
using SparseInject;

[TestFixture]
public class FactoryTests
{
    [Test]
    public void Test0()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterFactory<IA>(() => new A());
        
        containerBuilder.RegisterFactory<IB>(container =>
        {
            var otherFactory = container.Resolve<Func<IA>>();
            
            return new B(otherFactory.Invoke());
        });

        var container = containerBuilder.Build();
        var bFactory = container.Resolve<Func<IB>>();
        
        bFactory.Invoke().Should().NotBeNull();
    }

    public class A : IA
    {
        
    }

    public interface IA
    {
        
    }

    public class B : IB
    {
        public B(IA a)
        {

        }
    }
    
    public interface IB
    {
        
    }
}
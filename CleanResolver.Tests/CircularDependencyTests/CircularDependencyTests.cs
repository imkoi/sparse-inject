using System;
using CleanResolver;
using CleanResolver.Tests.CircularDependency;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

[TestFixture]
public class CircularDependencyTests
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
        _containerBuilder.Register<ServiceA>();
        _containerBuilder.Register<ServiceB>();

        Assert.Throws<Exception>(() => _containerBuilder.Build());
    }
}
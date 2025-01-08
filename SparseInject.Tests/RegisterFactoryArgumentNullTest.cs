using System;
using FluentAssertions;
using NUnit.Framework;
using SparseInject;

public class RegisterFactoryArgumentNullTest
{
    [Test]
    public void Container_WhenRegisterNullFactory1_ThrowArgumentNullException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        // Assets
        containerBuilder.Invoking(subject => 
                subject.RegisterFactory(default(Func<IDisposable>)))
            .Should()
            .Throw<ArgumentNullException>()
            .Where(exception => exception.Message.Contains("factory"));
    }
    
    [Test]
    public void Container_WhenRegisterNullFactory2_ThrowArgumentNullException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        // Assets
        containerBuilder.Invoking(subject => 
                subject.RegisterFactory<IDisposable, IDisposable>(default(Func<IDisposable>)))
            .Should()
            .Throw<ArgumentNullException>()
            .Where(exception => exception.Message.Contains("factory"));
    }
    
    [Test]
    public void Container_WhenRegisterNullFactory3_ThrowArgumentNullException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        // Assets
        containerBuilder.Invoking(subject => 
                subject.RegisterFactory(default(Func<IScopeResolver, IDisposable>)))
            .Should()
            .Throw<ArgumentNullException>()
            .Where(exception => exception.Message.Contains("factory"));
    }
    
    [Test]
    public void Container_WhenRegisterNullFactory4_ThrowArgumentNullException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        // Assets
        containerBuilder.Invoking(subject => 
                subject.RegisterFactory<IDisposable, IDisposable>(default(Func<IScopeResolver, IDisposable>)))
            .Should()
            .Throw<ArgumentNullException>()
            .Where(exception => exception.Message.Contains("factory"));
    }
    
    [Test]
    public void Container_WhenRegisterNullFactory5_ThrowArgumentNullException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        // Assets
        containerBuilder.Invoking(subject => 
                subject.RegisterFactory<IDisposable, IDisposable>(default(Func<IScopeResolver, Func<IDisposable>>)))
            .Should()
            .Throw<ArgumentNullException>()
            .Where(exception => exception.Message.Contains("factory"));
    }
    
    [Test]
    public void Container_WhenRegisterNullFactory6_ThrowArgumentNullException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        // Assets
        containerBuilder.Invoking(subject => 
                subject.RegisterFactory(default(Func<int, IDisposable>)))
            .Should()
            .Throw<ArgumentNullException>()
            .Where(exception => exception.Message.Contains("factory"));
    }
    
    [Test]
    public void Container_WhenRegisterNullFactory7_ThrowArgumentNullException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        // Assets
        containerBuilder.Invoking(subject => 
                subject.RegisterFactory(default(Func<IScopeResolver, Func<int, IDisposable>>)))
            .Should()
            .Throw<ArgumentNullException>()
            .Where(exception => exception.Message.Contains("factory"));
    }
    
    [Test]
    public void Container_WhenRegisterNullFactory8_ThrowArgumentNullException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        // Assets
        containerBuilder.Invoking(subject => 
                subject.RegisterFactory<int, IDisposable, IDisposable>(default(Func<int, IDisposable>)))
            .Should()
            .Throw<ArgumentNullException>()
            .Where(exception => exception.Message.Contains("factory"));
    }
    
    [Test]
    public void Container_WhenRegisterNullFactory9_ThrowArgumentNullException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        // Assets
        containerBuilder.Invoking(subject => 
                subject.RegisterFactory<int, IDisposable, IDisposable>(default(Func<IScopeResolver, Func<int, IDisposable>>)))
            .Should()
            .Throw<ArgumentNullException>()
            .Where(exception => exception.Message.Contains("factory"));
    }
}
using System;
using FluentAssertions;
using NUnit.Framework;
using SparseInject;

public class RegisterScopeArgumentNullTest
{
    private class TestScope : Scope { }
    
    [Test]
    public void Container_WhenRegisterNullScope1_ThrowArgumentNullException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        // Assets
        containerBuilder.Invoking(subject => 
                subject.RegisterScope<TestScope>(default(Action<IScopeBuilder>)))
            .Should()
            .Throw<ArgumentNullException>()
            .Where(exception => exception.Message.Contains("install"));
    }
    
    [Test]
    public void Container_WhenRegisterNullScope2_ThrowArgumentNullException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        // Assets
        containerBuilder.Invoking(subject => 
                subject.RegisterScope<TestScope, TestScope>(default(Action<IScopeBuilder>)))
            .Should()
            .Throw<ArgumentNullException>()
            .Where(exception => exception.Message.Contains("install"));
    }
    
    [Test]
    public void Container_WhenRegisterNullScope3_ThrowArgumentNullException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        // Assets
        containerBuilder.Invoking(subject => 
                subject.RegisterScope<TestScope>(default(Action<IScopeBuilder, IScopeResolver>)))
            .Should()
            .Throw<ArgumentNullException>()
            .Where(exception => exception.Message.Contains("install"));
    }
    
    [Test]
    public void Container_WhenRegisterNullScope4_ThrowArgumentNullException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        // Assets
        containerBuilder.Invoking(subject => 
                subject.RegisterScope<TestScope, TestScope>(default(Action<IScopeBuilder, IScopeResolver>)))
            .Should()
            .Throw<ArgumentNullException>()
            .Where(exception => exception.Message.Contains("install"));
    }
}
using System;
using FluentAssertions;
using NUnit.Framework;
using SparseInject;

public class RegisterValueArgumentNullTest
{
    [Test]
    public void Container_WhenRegisterNullValue1_ThrowArgumentNullException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        // Assets
        containerBuilder.Invoking(subject => subject.RegisterValue<IDisposable>(null))
            .Should()
            .Throw<ArgumentNullException>()
            .Where(exception => exception.Message.Contains("value"));
    }
    
    [Test]
    public void Container_WhenRegisterNullValue2_ThrowArgumentNullException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        // Assets
        containerBuilder.Invoking(subject => 
                subject.RegisterValue<IDisposable, IDisposable>(null))
            .Should()
            .Throw<ArgumentNullException>()
            .Where(exception => exception.Message.Contains("value"));
    }
    
    [Test]
    public void Container_WhenRegisterNullValue3_ThrowArgumentNullException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        // Assets
        containerBuilder.Invoking(subject => 
                subject.RegisterValue<IDisposable, IDisposable, IDisposable>(null))
            .Should()
            .Throw<ArgumentNullException>()
            .Where(exception => exception.Message.Contains("value"));
    }
    
    [Test]
    public void Container_WhenRegisterNullValue4_ThrowArgumentNullException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        // Assets
        containerBuilder.Invoking(subject => 
                subject.RegisterValue<IDisposable, IDisposable, IDisposable, IDisposable>(null))
            .Should()
            .Throw<ArgumentNullException>()
            .Where(exception => exception.Message.Contains("value"));
    }
}
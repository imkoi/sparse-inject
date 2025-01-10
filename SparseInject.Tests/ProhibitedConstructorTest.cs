using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SparseInject;

public class ProhibitedConstructorTest
{
    private class ClassWithPrivateConstructor
    {
        private ClassWithPrivateConstructor(IDependency dependency)
        {
            
        }
    }
    
    private class ClassWithProtectedConstructor
    {
        protected ClassWithProtectedConstructor(IDependency dependency)
        {
            
        }
    }
    
    private class ClassWithInternalConstructor
    {
        internal ClassWithInternalConstructor(IDependency dependency)
        {
            
        }
    }
    
    private class ClassWithPrivateAndInternalConstructor
    {
        private ClassWithPrivateAndInternalConstructor(IDependency dependency)
        {
            
        }
        
        internal ClassWithPrivateAndInternalConstructor(IDependency dependency, IDependency dependency2)
        {
            
        }
    }

    public interface IDependency
    {
        
    }
    
    [Test]
    public void RegisteredFactory_WhenBuildContainer_ThrowProperException()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterValue(Substitute.For<IDependency>());
        builder.Register<ClassWithProtectedConstructor>();

        // Asserts
        builder.Invoking(subject => subject.Build())
            .Should()
            .Throw<SparseInjectException>();
    }
    
    [Test]
    public void RegisteredWithProtected_WhenBuildContainer_ThrowProperException()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterValue(Substitute.For<IDependency>());
        builder.Register<ClassWithPrivateConstructor>();

        // Asserts
        builder.Invoking(subject => subject.Build())
            .Should()
            .Throw<SparseInjectException>();
    }
    
    [Test]
    public void RegisteredWithInternal_WhenBuildContainer_ContainerAreBuilt()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterValue(Substitute.For<IDependency>());
        builder.Register<ClassWithInternalConstructor>();

        // Asserts
        builder.Invoking(subject => subject.Build())
            .Should()
            .NotThrow<SparseInjectException>();
    }
    
    [Test]
    public void RegisteredWithInternal_WhenResolve_CouldBeResolved()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterValue(Substitute.For<IDependency>());
        builder.Register<ClassWithInternalConstructor>();
        var container = builder.Build();

        // Asserts
        container.Invoking(subject => subject.Resolve<ClassWithInternalConstructor>())
            .Should()
            .NotThrow<SparseInjectException>();
    }
    
    [Test]
    public void RegisteredWithInternalAndPrivate_WhenBuildContainer_ContainerAreBuilt()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterValue(Substitute.For<IDependency>());
        builder.Register<ClassWithPrivateAndInternalConstructor>();

        // Asserts
        builder.Invoking(subject => subject.Build())
            .Should()
            .NotThrow<SparseInjectException>();
    }
    
    [Test]
    public void RegisteredWithInternalAndPrivate_WhenResolve_CouldBeResolved()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterValue(Substitute.For<IDependency>());
        builder.Register<ClassWithPrivateAndInternalConstructor>();
        var container = builder.Build();

        // Asserts
        container.Invoking(subject => subject.Resolve<ClassWithPrivateAndInternalConstructor>())
            .Should()
            .NotThrow<SparseInjectException>();
    }
}
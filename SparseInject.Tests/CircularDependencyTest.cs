using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SparseInject;

[TestFixture]
public class CircularDependencyTest
{
    private class A0 { public A0(A1 value) { } }

    private class A1 { public A1(A0 value) { } }
    
    [Test]
    public void CaseA_WhenBuild_ThrowProperException()
    {
        // Setup
        var expectedStringsInExceptionMessage = new []
        {
            typeof(A0).ToString(),
            typeof(A1).ToString(),
        };
        
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<A0>();
        containerBuilder.Register<A1>();

        // Asserts
        containerBuilder
            .Invoking(subject => subject.Build())
            .Should()
            .Throw<SparseInjectException>()
            .Where(exception => expectedStringsInExceptionMessage.All(contains => exception.Message.Contains(contains)));
    }
    
    private class B0 { public B0(B1 value) { } }

    private class B1 { public B1(B2 value) { } }
    
    private class B2 { public B2(B0 value) { } }
    
    [Test]
    public void CaseB_WhenBuild_ThrowProperException()
    {
        // Setup
        var expectedStringsInExceptionMessage = new []
        {
            typeof(B0).ToString(),
            typeof(B1).ToString(),
            typeof(B2).ToString(),
        };
        
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<B0>();
        containerBuilder.Register<B1>();
        containerBuilder.Register<B2>();

        // Asserts
        containerBuilder
            .Invoking(subject => subject.Build())
            .Should()
            .Throw<SparseInjectException>()
            .Where(exception => expectedStringsInExceptionMessage.All(contains => exception.Message.Contains(contains)));
    }
    
    private class C0 { public C0(C1 value) { } }
    
    private class C1 : Scope { public C1(C0 value) { } }
    
    [Test]
    public void CaseC_WhenBuild_ThrowProperException()
    {
        // Setup
        var expectedStringsInExceptionMessage = new []
        {
            typeof(C0).ToString(),
            typeof(C1).ToString(),
        };
        
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<C0>();
        containerBuilder.RegisterScope<C1>(scopeBuilder => { });

        // Asserts
        containerBuilder
            .Invoking(subject => subject.Build())
            .Should()
            .Throw<SparseInjectException>()
            .Where(exception => expectedStringsInExceptionMessage.All(contains => exception.Message.Contains(contains)));
    }
    
    private class D0 { public D0(D1 value) { } }
    private class D1 : Scope { public D1(D2 value) { } }
    private class D2 { public D2(D0 value) { } }
    
    [Test]
    public void CaseD_WhenResolveScope_ThrowProperException()
    {
        // Setup
        var expectedStringsInExceptionMessage = new []
        {
            typeof(D0).ToString(),
            typeof(D1).ToString(),
            typeof(D2).ToString(),
        };
        
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<D0>();
        containerBuilder.RegisterScope<D1>(scopeBuilder =>
        {
            scopeBuilder.Register<D2>();
        });

        var container = containerBuilder.Build();

        // Asserts
        container
            .Invoking(subject => subject.Resolve<D0>())
            .Should()
            .Throw<SparseInjectException>()
            .Where(exception => expectedStringsInExceptionMessage.All(contains => exception.Message.Contains(contains)));
    }
    
    private class E0 { public E0(E1 value) { } }
    private class E1 { public E1(E2_1 value, E2_2 empty) { } }
    private class E2_1 { public E2_1(E0 value) { } }
    private class E2_2 { }
    
    [Test]
    public void CaseE_WhenBuild_ThrowProperException()
    {
        // Setup
        var expectedStringsInExceptionMessage = new []
        {
            typeof(E0).ToString(),
            typeof(E1).ToString(),
            typeof(E2_1).ToString(),
        };
        
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<E0>();
        containerBuilder.Register<E1>();
        containerBuilder.Register<E2_1>();
        containerBuilder.Register<E2_2>();

        // Asserts
        containerBuilder
            .Invoking(subject => subject.Build())
            .Should()
            .Throw<SparseInjectException>()
            .Where(exception => expectedStringsInExceptionMessage.All(contains => exception.Message.Contains(contains)));
    }
    
    private class F { public F(F value) { } }
    
    [Test]
    public void CaseF_WhenBuild_ThrowProperException()
    {
        // Setup
        var expectedStringsInExceptionMessage = new []
        {
            typeof(F).ToString(),
        };
        
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<F>();

        // Asserts
        containerBuilder
            .Invoking(subject => subject.Build())
            .Should()
            .Throw<SparseInjectException>()
            .Where(exception => expectedStringsInExceptionMessage.All(contains => exception.Message.Contains(contains)));
    }
}
using FluentAssertions;
using NUnit.Framework;
using SparseInject;

[TestFixture]
public class CircularDependencyTest
{
    private class A0
    {
        public A0(A1 value) { }
    }

    private class A1
    {
        public A1(A0 value) { }
    }
    
    [Test]
    public void CaseA_WhenBuild_ThrowProperException()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<A0>();
        containerBuilder.Register<A1>();

        containerBuilder.Invoking(subject => subject.Build()).Should().Throw<SparseInjectException>();
    }
    
    private class B0
    {
        public B0(B1 value) { }
    }

    private class B1
    {
        public B1(B2 value) { }
    }
    
    private class B2
    {
        public B2(B0 value) { }
    }
    
    [Test]
    public void CaseB_WhenBuild_ThrowProperException()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<B0>();
        containerBuilder.Register<B1>();
        containerBuilder.Register<B2>();

        containerBuilder.Invoking(subject => subject.Build()).Should().Throw<SparseInjectException>();
    }
    
    private class C0
    {
        public C0(C1 value) { }
    }
    
    private class C1 : Scope
    {
        public C1(C0 value) { }
    }
    
    [Test]
    public void CaseC_WhenBuild_ThrowProperException()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<C0>();
        containerBuilder.RegisterScope<C1>(scopeBuilder => { });

        containerBuilder.Invoking(subject => subject.Build()).Should().Throw<SparseInjectException>();
    }
    
    private class D0
    {
        public D0(D1 value) { }
    }
    
    private class D1 : Scope
    {
        public D1(D2 value) { }
    }

    private class D2
    {
        public D2(D0 value) { }
    }
    
    [Test]
    public void CaseD_WhenResolveScope_ThrowProperException()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<D0>();
        containerBuilder.RegisterScope<D1>(scopeBuilder =>
        {
            scopeBuilder.Register<D2>();
        });

        var container = containerBuilder.Build();

        container.Invoking(subject => subject.Resolve<D0>()).Should().Throw<SparseInjectException>();
    }
}
namespace SparseInject.ReflectionBaking.Tests.PartialBaking
{
    public class PartialBakingTestInstaller
    {
        public static void Install(IScopeBuilder scopeBuilder)
        {
            scopeBuilder.Register<BakedDependencyA>();
            scopeBuilder.Register<IDependencyB, BakedDependencyB>();
            scopeBuilder.Register<IDependencyC0, IDependencyC1, BakedDependencyC>();
            scopeBuilder.Register<IDependencyD0, IDependencyD1, IDependencyD2, BakedDependencyD>();
        
            scopeBuilder.RegisterValue<UnbakedDependencyA>(new UnbakedDependencyA());
            scopeBuilder.RegisterValue<IDependencyB, UnbakedDependencyB>(new UnbakedDependencyB());
            scopeBuilder.RegisterValue<IDependencyC0, IDependencyC1, UnbakedDependencyC>(new UnbakedDependencyC());
            scopeBuilder.RegisterValue<IDependencyD0, IDependencyD1, IDependencyD2, UnbakedDependencyD>(new UnbakedDependencyD());
        
            scopeBuilder.RegisterFactory<UnbakedDependencyB>(() => new UnbakedDependencyB());
            scopeBuilder.RegisterFactory<IDependencyB>(() => new UnbakedDependencyB());
        }
    }

    public class BakedDependencyA { }

    public interface IDependencyB { }
    public class BakedDependencyB : IDependencyB { }

    public interface IDependencyC0 { }
    public interface IDependencyC1 { }
    public class BakedDependencyC : IDependencyC0, IDependencyC1 { }

    public interface IDependencyD0 { }
    public interface IDependencyD1 { }
    public interface IDependencyD2 { }
    public class BakedDependencyD : IDependencyD0, IDependencyD1, IDependencyD2 { }

    public class UnbakedDependencyA { }
    public class UnbakedDependencyB : IDependencyB { }
    public class UnbakedDependencyC : IDependencyC0, IDependencyC1 { }
    public class UnbakedDependencyD : IDependencyD0, IDependencyD1, IDependencyD2 { }
}
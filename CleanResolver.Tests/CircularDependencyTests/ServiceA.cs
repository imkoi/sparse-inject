namespace CleanResolver.Tests.CircularDependency
{
    public class ServiceA
    {
        public ServiceA(ServiceB serviceB)
        {
            
        }
    }

    public class ServiceB
    {
        public ServiceB(ServiceA serviceA)
        {
            
        }
    }
}
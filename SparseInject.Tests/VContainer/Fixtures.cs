using System;
using System.Threading;

namespace VContainer.Tests
{
    public interface I1
    {
    }

    interface I2
    {
    }

    interface I3
    {
    }

    interface I4
    {
    }

    interface I5
    {
    }

    interface I6
    {
    }

    interface I7
    {
    }

    interface IGenericService<T>
    {
    }

    interface IGenericService<T1,T2>
    {
    }

    class DisposableServiceA : I1, IDisposable
    {
        public bool Disposed { get; private set; }
        public void Dispose() => Disposed = true;
    }

    class DisposableServiceB : I2, IDisposable
    {
        public bool Disposed { get; private set; }
        public void Dispose() => Disposed = true;
    }

    class NoDependencyServiceA : I2
    {
    }

    class NoDependencyServiceB : I3
    {
    }

    class ServiceA : I4
    {
        public readonly I2 Service2;

        public ServiceA(I2 service2)
        {
            if (service2 is null)
            {
                throw new ArgumentNullException(nameof(service2));
            }
            Service2 = service2;
        }
    }

    class ServiceB : I5
    {
        public readonly I3 Service3;

        public ServiceB(I3 service3)
        {
            if (service3 is null)
            {
                throw new ArgumentNullException(nameof(service3));
            }
            Service3 = service3;
        }
    }

    class ServiceC : I6
    {
        public ServiceC(
            I2 service2,
            I3 service3,
            I4 service4,
            I5 service5)
        {
            if (service2 is null)
            {
                throw new ArgumentNullException(nameof(service2));
            }

            if (service3 is null)
            {
                throw new ArgumentNullException(nameof(service3));
            }

            if (service4 is null)
            {
                throw new ArgumentNullException(nameof(service4));
            }

            if (service5 is null)
            {
                throw new ArgumentNullException(nameof(service5));
            }
        }
    }

    class ServiceD : I7
    {
    }


    struct NoDependencyUnmanagedServiceA
    {
    }

    class MultipleInterfaceServiceA : I1, I2, I3
    {
    }

    class MultipleInterfaceServiceB : I1, I2, I3
    {
    }

    class MultipleInterfaceServiceC : I1, I2, I3
    {
    }

    class MultipleInterfaceServiceD : I1, I2, I3
    {
    }
    
    class HasCircularDependency1
    {
        public HasCircularDependency1(HasCircularDependency2 dependency2)
        {
            if (dependency2 == null)
            {
                throw new ArgumentException();
            }
        }
    }

    class HasCircularDependency2
    {
        public HasCircularDependency2(HasCircularDependency1 dependency1)
        {
            if (dependency1 == null)
            {
                throw new ArgumentException();
            }
        }
    }

    class HasAbstractCircularDependency1
    {
        public HasAbstractCircularDependency1(I2 dependency2)
        {
            if (dependency2 == null)
            {
                throw new ArgumentException();
            }
        }
    }

    class HasAbstractCircularDependency2 : I2
    {
        public HasAbstractCircularDependency2(HasAbstractCircularDependency1 dependency1)
        {
            if (dependency1 == null)
            {
                throw new ArgumentException();
            }
        }
    }

    class HasCircularDependencyMsg1
    {
        public HasCircularDependencyMsg1(HasCircularDependencyMsg2 dependency2)
        {
            if (dependency2 == null)
            {
                throw new ArgumentException();
            }
        }
    }

    class HasCircularDependencyMsg2
    {
        
    }

    class HasGenericDependency
    {
        public readonly IGenericService<NoDependencyServiceA> Service;

        public HasGenericDependency(IGenericService<NoDependencyServiceA> service)
        {
            Service = service;
        }
    }

    class GenericsService<T> : IGenericService<T>, IDisposable
    {
        public readonly T ParameterService;

        public bool Disposed { get; private set; }

        public GenericsService(T parameterService)
        {
            ParameterService = parameterService;
            Disposed = false;
        }

        public void Dispose()
        {
            Disposed = true;
        }
    }

    class GenericsService2<T1,T2> : IGenericService<T1,T2>
    {
        public readonly IGenericService<T2> ParameterService;

        public GenericsService2(IGenericService<T2> parameterService)
        {
            ParameterService = parameterService;
        }
    }

    class GenericsArgumentService
    {
        public readonly GenericsService<I2> GenericsService;

        public GenericsArgumentService(GenericsService<I2> genericsService)
        {
            GenericsService = genericsService;
        }
    }

    class SampleAttribute : Attribute
    {
    }

    class HasInstanceId
    {
        public static void ResetId()
        {
            Interlocked.Exchange(ref instanceCount, 0);
        }

        static int instanceCount;

        public readonly int Id = Interlocked.Increment(ref instanceCount);
    }
}

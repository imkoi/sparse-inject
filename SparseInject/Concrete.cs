using System;
using System.Reflection;

namespace SparseInject
{
    public struct Concrete
    {
        public Type Type;
        public int ConstructorContractsIndex;
        public int ConstructorContractsCount;
        public ConstructorInfo ConstructorInfo;
        public InstanceFactoryBase GeneratedInstanceFactory;
        public int SingletonFlag;
        public object SingletonValue;
        public Action<IScopeBuilder, IScopeResolver> ScopeConfigurator;
        public int FactoryFlag;
    }
}
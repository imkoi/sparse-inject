namespace SparseInject
{
    public abstract class InstanceFactoryBase
    {
        public int ConstructorParametersIndex;
        public int ConstructorParametersCount;
        public abstract object Create(object[] parameters);
    }
}
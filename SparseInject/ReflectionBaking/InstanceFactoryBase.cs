using Unity.IL2CPP.CompilerServices;

namespace SparseInject
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class InstanceFactoryBase
    {
        public int ConstructorParametersIndex;
        public int ConstructorParametersCount;
        public abstract object Create(object[] parameters);
    }
}
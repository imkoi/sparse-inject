namespace SparseInject
{
#if UNITY_2019_1_OR_NEWER
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public abstract class InstanceFactoryBase
    {
        public int ConstructorParametersIndex;
        public int ConstructorParametersCount;
        public abstract object Create(object[] parameters);
    }
}
namespace SparseInject
{
#if UNITY_2017_1_OR_NEWER
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    internal struct ContainerInfo
    {
        public readonly Container ParentContainer;
        public readonly int[] ContractsSparse;
        public readonly Contract[] ContractsDense;
        public readonly int[] ContractsConcretesIndices;
        public readonly Concrete[] Concretes;
        public readonly int[] ConcreteConstructorContractIds;
        public readonly int ConcretesCount;

        public ContainerInfo(
            Container parentContainer,
            int[] contractsSparse,
            Contract[] contractsDense,
            int[] contractsConcretesIndices,
            Concrete[] concretes,
            int[] concreteConstructorContractIds,
            int concretesCount)
        {
            ParentContainer = parentContainer;
            ContractsSparse = contractsSparse;
            ContractsDense = contractsDense;
            ContractsConcretesIndices = contractsConcretesIndices;
            Concretes = concretes;
            ConcreteConstructorContractIds = concreteConstructorContractIds;
            ConcretesCount = concretesCount;
        }
    }
}
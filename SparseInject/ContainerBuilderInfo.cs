#if DEBUG
namespace SparseInject
{
    public struct ContainerBuilderInfo
    {
        public readonly Container ParentContainer;
        public readonly int[] ContractsSparse;
        public readonly Contract[] ContractsDense;
        public readonly int[] ContractsConcretesIndices;
        public readonly Concrete[] Concretes;
        public readonly int ConcretesCount;

        public ContainerBuilderInfo(
            Container parentContainer,
            int[] contractsSparse,
            Contract[] contractsDense,
            int[] contractsConcretesIndices,
            Concrete[] concretes,
            int concretesCount)
        {
            ParentContainer = parentContainer;
            ContractsSparse = contractsSparse;
            ContractsDense = contractsDense;
            ContractsConcretesIndices = contractsConcretesIndices;
            Concretes = concretes;
            ConcretesCount = concretesCount;
        }
    }
}
#endif
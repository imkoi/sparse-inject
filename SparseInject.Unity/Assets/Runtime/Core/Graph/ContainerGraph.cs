using System.Collections.Generic;

namespace SparseInject
{
    public class ContainerGraph
    {
        public IReadOnlyList<ContainerGraph> ParentContainers { get; }
        public IReadOnlyList<ContractGraph> Contracts;

        public ContainerGraph(IReadOnlyList<ContractGraph> contracts, IReadOnlyList<ContainerGraph> parentContainers)
        {
            ParentContainers = parentContainers;
            Contracts = contracts;
        }
    }
}
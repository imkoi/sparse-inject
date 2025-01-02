#if DEBUG
using System.Collections.Generic;

namespace SparseInject
{
    public static class ContainerBuilderExtension
    {
        public static ContainerGraph GetGraph(this ContainerBuilder container)
        {
            var containerInfo = container.GetContainerBuilderInfo();
            var contracts = new List<ContractGraph>();
            var parentContainers = new List<ContainerGraph>();

            if (containerInfo.ParentContainer != null)
            {
                parentContainers.Add(containerInfo.ParentContainer.GetGraph());
            }

            foreach (var contract in containerInfo.ContractsDense)
            {
                if (contract.Type == null && contract.Data == 0)
                {
                    continue;
                }
                
                var concretes = new List<ConcreteGraph>();

                for (var concreteIdx = 0; concreteIdx < contract.GetConcretesCount(); concreteIdx++)
                {
                    var contractConcreteIndex = contract.GetConcretesIndex() + concreteIdx;
                    var concreteIndex = containerInfo.ContractsConcretesIndices[contractConcreteIndex];
                    var concrete = containerInfo.Concretes[concreteIndex];

                    var concreteGraph = new ConcreteGraph(concrete.Type, concrete.IsSingleton(), concrete.HasValue(),
                        concrete.IsArray(), concrete.IsFactory(), concrete.IsScope(), new List<int>(), concrete.Value);
                    
                    concretes.Add(concreteGraph);
                }

                var contractGraph = new ContractGraph(contract.Type, contract.IsCollection(), concretes);
                
                contracts.Add(contractGraph);
            }

            return new ContainerGraph(contracts, parentContainers);
        }
    }
}
#endif
using System.Collections.Generic;

namespace SparseInject
{
    public static class ContainerExtension
    {
        public static ContainerGraph GetGraph(this Container container)
        {
            var containerInfo = container.GetContainerInfo();
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
                    
                    var dependencyIds = new List<int>();

                    for (var i = 0; i < concrete.GetConstructorContractsCount(); i++)
                    {
                        var constructorDependencyIndex = concrete.GetConstructorContractsIndex() + i;
                        var dependencyIndex = containerInfo.ConcreteConstructorContractIds[constructorDependencyIndex];
                        
                        dependencyIds.Add(dependencyIndex);
                    }

                    var concreteGraph = new ConcreteGraph(concrete.Type, concrete.IsSingleton(), concrete.HasValue(),
                        concrete.IsArray(), concrete.IsFactory(), concrete.IsScope(), dependencyIds, concrete.Value);
                    
                    concretes.Add(concreteGraph);
                }

                var contractGraph = new ContractGraph(contract.Type, contract.IsCollection(), concretes);
                
                contracts.Add(contractGraph);
            }
            
            foreach (var contract in contracts)
            {
                foreach (var concrete in contract.Concretes)
                {
                    foreach (var dependencyIndex in concrete.DependencyIndices)
                    {
                        concrete.AddDependency(contracts[dependencyIndex]);
                    }
                }
            }

            return new ContainerGraph(contracts, parentContainers);
        }
    }
}
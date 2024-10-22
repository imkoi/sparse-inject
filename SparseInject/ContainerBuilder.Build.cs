using System;
using System.Reflection;

namespace SparseInject
{
    public partial class ContainerBuilder
    {
        private void ReorderSingleInstances()
        {
            var contractsCount = _dependenciesCount;

            for (var contractIndex = 0; contractIndex < contractsCount; contractIndex++)
            {
                ref var contract = ref _contractsDense[contractIndex];

                if (contract.ConcretesCount > 1 && _contractIds.TryGetValue(contract.Type, out var id))
                {
                    ref var singleContract = ref _contractsDense[_contractsSparse[id]];

                    singleContract.ConcretesIndex = contract.ConcretesIndex + contract.ConcretesCount - 1;
                }
            }
        }
        
        private (int implementationDependenciesCount, ParameterInfo[][] implementationConstructorParameterInfos, Type[][] implementationConstructorParameters, int maxConstructorLength) BuildPrecomputeDependenciesCount()
        {
            var implementationConstructorParameterInfos = new ParameterInfo[_implementationsCount][];
            var implementationConstructorParameters = new Type[_implementationsCount][];
            var implementationDependenciesCount = 0;
            var maxConstructorLength = int.MinValue;
            var concretesCount = _implementationsCount;

            for (var concreteIndex = 0; concreteIndex < concretesCount; concreteIndex++)
            {
                ref var concrete = ref _concretes[concreteIndex];
                var constructorParametersCount = 0;
                    
                if (concrete.SingletonFlag != SingletonFlag.SingletonWithValue)
                {
                    if (ReflectionBakingProviderCache.TryGetInstanceFactory(concrete.Type, out var factory, out var constructorParametersSpan))
                    {
                        constructorParametersCount = factory.ConstructorParametersCount;

                        implementationConstructorParameters[concreteIndex] = constructorParametersSpan;
                        concrete.GeneratedInstanceFactory = factory;
                    }
                    else
                    {
                        concrete.ConstructorInfo = concrete.Type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)[0];
                        var constructorParameters = concrete.ConstructorInfo.GetParameters();

                        constructorParametersCount = constructorParameters.Length;
                        
                        implementationConstructorParameterInfos[concreteIndex] = constructorParameters;
                    }
                }
                
                concrete.ConstructorContractsCount = constructorParametersCount;

                if (constructorParametersCount > maxConstructorLength)
                {
                    maxConstructorLength = constructorParametersCount;
                }

                implementationDependenciesCount += constructorParametersCount;
            }
            
            return (implementationDependenciesCount, implementationConstructorParameterInfos, implementationConstructorParameters, maxConstructorLength);
        }

        private int[] BuildBakeImplementationDependencyIds(int implementationDependenciesCount,
            ParameterInfo[][] implementationConstructorParameterInfos, Type[][] implementationConstructorParameters,
            int maxConstructorLength)
        {
            var generatedInstanceFactoryDependencyIds = new int[maxConstructorLength];
            var dependencyReferences = new int[implementationDependenciesCount];
            var dependencyReferenceIndex = 0;

            var concretesCount = _implementationsCount;
            var concreteConstructorParametersCount = -1;

            for (var concreteIndex = 0; concreteIndex < concretesCount; concreteIndex++)
            {
                ref var concrete = ref _concretes[concreteIndex];

                concrete.ConstructorContractsIndex = dependencyReferenceIndex;

                concreteConstructorParametersCount = concrete.ConstructorContractsCount;

                for (var parameterIndex = 0; parameterIndex < concreteConstructorParametersCount; parameterIndex++)
                {
                    var parameterType = default(Type);
                    var contractId = -1;

                    // TODO: optimize
                    if (implementationConstructorParameters[concreteIndex] != null)
                    {
                        parameterType = implementationConstructorParameters[concreteIndex][
                            concrete.GeneratedInstanceFactory.ConstructorParametersIndex + parameterIndex];
                    }
                    else
                    {
                        parameterType = implementationConstructorParameterInfos[concreteIndex][parameterIndex]
                            .ParameterType;
                    }

                    if (!_contractIds.TryGetValue(parameterType, out contractId))
                    {
                        contractId = _contractIds.Count;

                        _contractIds.Add(parameterType, contractId);
                    }

                    dependencyReferences[parameterIndex + dependencyReferenceIndex] = contractId;
                    generatedInstanceFactoryDependencyIds[parameterIndex] = contractId;
                }

                dependencyReferenceIndex += concreteConstructorParametersCount;
            }

            return dependencyReferences;
        }
    }
}
using System;
using System.Reflection;
using Unity.IL2CPP.CompilerServices;

namespace SparseInject
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public partial class ContainerBuilder
    {
        private (int implementationDependenciesCount, ParameterInfo[][] implementationConstructorParameterInfos, Type[][] implementationConstructorParameters, int maxConstructorLength) BuildPrecomputeDependenciesCount()
        {
            var implementationConstructorParameterInfos = new ParameterInfo[_concretesCount][];
            var implementationConstructorParameters = new Type[_concretesCount][];
            var implementationDependenciesCount = 0;
            var maxConstructorLength = int.MinValue;
            var concretesCount = _concretesCount;

            for (var concreteIndex = 0; concreteIndex < concretesCount; concreteIndex++)
            {
                ref var concrete = ref _concretes[concreteIndex];
                var constructorParametersCount = 0;
                    
                if (!(concrete.IsSingleton() && concrete.HasValue()))
                {
                    if (ReflectionBakingProviderCache.TryGetInstanceFactory(concrete.Type, out var factory, out var constructorParametersSpan))
                    {
                        constructorParametersCount = factory.ConstructorParametersCount;

                        implementationConstructorParameters[concreteIndex] = constructorParametersSpan;
                        concrete.GeneratedInstanceFactory = factory;
                    }
                    else
                    {
                        var constructors = concrete.Type.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                        var constructorParameters = default(ParameterInfo[]);
                        var maxParametersCount = -1;

                        for (var i = 0; i < constructors.Length; i++)
                        {
                            var suspectConstructor = constructors[i];
                                
                            if (suspectConstructor.IsPublic || suspectConstructor.IsAssembly)
                            {
                                var suspectConstructorParameters = suspectConstructor.GetParameters();

                                if (suspectConstructorParameters.Length > maxParametersCount)
                                {
                                    constructorParameters = suspectConstructorParameters;
                                    maxParametersCount = suspectConstructorParameters.Length;
                                    
                                    constructors[0] = suspectConstructor;
                                }
                            }
                        }
                        
                        if (maxParametersCount < 0)
                        {
                            throw new SparseInjectException($"Could not find public or internal constructor for type {concrete.Type}");
                        }
                        
                        concrete.ConstructorInfo = constructors[0];

                        constructorParametersCount = constructorParameters.Length;
                        
                        implementationConstructorParameterInfos[concreteIndex] = constructorParameters;
                    }
                }
                
                concrete.SetConstructorContractsCount(constructorParametersCount);

                if (constructorParametersCount > maxConstructorLength)
                {
                    maxConstructorLength = constructorParametersCount;
                }

                implementationDependenciesCount += constructorParametersCount;
            }

            if (maxConstructorLength < 0)
            {
                maxConstructorLength = 0;
            }
            
            return (implementationDependenciesCount, implementationConstructorParameterInfos, implementationConstructorParameters, maxConstructorLength);
        }

        private int[] BuildBakeImplementationDependencyIds(Type containerType, int implementationDependenciesCount,
            ParameterInfo[][] implementationConstructorParameterInfos, Type[][] implementationConstructorParameters,
            int maxConstructorLength)
        {
            var generatedInstanceFactoryDependencyIds = new int[maxConstructorLength];
            var dependencyReferences = new int[implementationDependenciesCount];
            var dependencyReferenceIndex = 0;

            var concretesCount = _concretesCount;
            var concreteConstructorParametersCount = -1;
            var contractId = -1;

            for (var concreteIndex = 0; concreteIndex < concretesCount; concreteIndex++)
            {
                ref var concrete = ref _concretes[concreteIndex];

                concrete.SetConstructorContractsIndex(dependencyReferenceIndex);

                concreteConstructorParametersCount = concrete.GetConstructorContractsCount();

                for (var parameterIndex = 0; parameterIndex < concreteConstructorParametersCount; parameterIndex++)
                {
                    var parameterType = default(Type);
                    contractId = -1; // contractId

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
                        if (parameterType.IsArray)
                        {
                            var elementType = parameterType.GetElementType()!;

                            if (!_contractIds.TryGetValue(elementType, out contractId))
                            {
                                contractId = _contractIds.Count;

                                _contractIds.Add(elementType, contractId);
                            }
                        }
                        else if (concrete.IsScope())
                        {
                            contractId = _contractIds.Count;

                            _contractIds.Add(parameterType, contractId);
                        }
                        else
                        {
                            throw new SparseInjectException(
                                $"Dependency '{parameterType}' of '{concrete.Type}' not registered inside container");
                        }
                    }

                    dependencyReferences[parameterIndex + dependencyReferenceIndex] = contractId;
                    generatedInstanceFactoryDependencyIds[parameterIndex] = contractId;
                }

                dependencyReferenceIndex += concreteConstructorParametersCount;
            }

            if (containerType != null)
            {
                if (_parentContainer.TryGetConcrete(containerType, out var concreteContainer))
                {
                    concreteConstructorParametersCount = concreteContainer.GetConstructorContractsCount();
                    var constructorContractsIndex = concreteContainer.GetConstructorContractsIndex();
                    
                    for (var i = 0; i < concreteConstructorParametersCount; i++)
                    {
                        contractId = _parentContainer.GetDependencyContractId(constructorContractsIndex + i);
                        
                        if (_contractsSparse[contractId] < 0)
                        {
                            if (!_parentContainer.ContractExist(contractId))
                            {
                                foreach (var pair in _contractIds)
                                {
                                    if (pair.Value != contractId)
                                    {
                                        continue;
                                    }
                                    
                                    throw new SparseInjectException($"Dependency '{pair.Key}' of '{containerType}' not registered");
                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new SparseInjectException($"Container {containerType} not registered");
                }
            }

            return dependencyReferences;
        }
    }
}
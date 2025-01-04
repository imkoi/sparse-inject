using System;
using System.Reflection;

namespace SparseInject
{
#if UNITY_2017_1_OR_NEWER
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public partial class ContainerBuilder
    {
        private (int implementationDependenciesCount, object[][] implementationConstructorParameters, int maxConstructorLength) BuildPrecomputeDependenciesCount()
        {
            var implementationConstructorParameters = new object[_concretesCount][];
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
                        concrete.MarkInstanceFactory();
                    }
                    else
                    {
                        var constructor = ReflectionUtility.GetInjectableConstructor(concrete.Type);
                        
                        concrete.ConstructorInfo = constructor.info;

                        constructorParametersCount = constructor.parameters.Length;
                        
                        implementationConstructorParameters[concreteIndex] = constructor.parameters;
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
            
            return (implementationDependenciesCount, implementationConstructorParameters, maxConstructorLength);
        }

        private int[] BuildBakeImplementationDependencyIds(
            Type containerType,
            int implementationDependenciesCount,
            object[][] implementationConstructorParameters)
        {
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

                    if (concrete.HasInstanceFactory())
                    {
                        parameterType = (Type) implementationConstructorParameters[concreteIndex][
                            concrete.GeneratedInstanceFactory.ConstructorParametersIndex + parameterIndex];
                    }
                    else
                    {
                        parameterType = ((ParameterInfo[])implementationConstructorParameters[concreteIndex])[parameterIndex]
                            .ParameterType;
                    }

                    if (!_contractIds.TryGetValue(parameterType, out contractId))
                    {
                        if (parameterType.IsArray)
                        {
                            var elementType = parameterType.GetElementType();

                            if (!_contractIds.TryGetValue(elementType, out contractId))
                            {
                                contractId = _contractIds.Count;

                                _contractIds.Add(elementType, contractId);

                                contractId = _contractIds.Count;
                                _contractIds.Add(parameterType, contractId);
                            }
                        }
                        else if (concrete.IsScope()) // TODO: is it work as expected?
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
                }

                dependencyReferenceIndex += concreteConstructorParametersCount;
            }

            if (containerType != null)
            {
                var concrete = _parentContainer.GetConcreteByContractType(containerType); // could it be upper current parent container???

                concreteConstructorParametersCount = concrete.GetConstructorContractsCount();
                var constructorContractsIndex = concrete.GetConstructorContractsIndex();
                    
                for (var i = 0; i < concreteConstructorParametersCount; i++) // foreach dependencies of scope
                {
                    contractId = _parentContainer.GetDependencyContractId(constructorContractsIndex + i); // get contractId of dependency

                    if (_contractsSparse[contractId] >= 0) // if dependency exist in scope - skip
                    {
                        continue;
                    }

                    if (!_parentContainer.ContractExist(contractId))
                    {
                        foreach (var pair in _contractIds)
                        {
                            if (pair.Value != contractId)
                            {
                                continue;
                            }

                            if (!pair.Key.IsArray)
                            {
                                throw new SparseInjectException(
                                    $"Dependency '{pair.Key}' of '{containerType}' not registered");
                            }
                        }
                    }
                }
            }

            return dependencyReferences;
        }
    }
}
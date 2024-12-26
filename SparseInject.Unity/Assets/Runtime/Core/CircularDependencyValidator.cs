using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SparseInject
{
#if UNITY_2017_1_OR_NEWER
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public static class CircularDependencyValidator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowIfInvalid(ContainerInfo containerInfo)
        {
            var concretesCount = containerInfo.ConcretesCount;
            
            for (var i = 0; i < concretesCount; i++)
            {
                ThrowIfInvalidRecursive(containerInfo.Concretes, i, i, containerInfo, 0);
            }
        }

        private static void ThrowIfInvalidRecursive(Concrete[] originConcretes, int originConcreteIndex, int concreteIndex,
            ContainerInfo containerInfo, int depth)
        {
            var concretes = containerInfo.Concretes;
            ref var concrete = ref concretes[concreteIndex];

            // Probably need to check the type, because could be identical index for different types
            if (depth > 0 && originConcreteIndex == concreteIndex)
            {
                ConstructExceptionRecursiveByReflection(concrete.Type, new List<Type>(depth), out var exception);

                //TODO: inspect this case
                if (exception != null)
                {
                    throw exception;
                }
            }
            
            var constructorContractsCount = concrete.GetConstructorContractsCount();
            var constructorContractsIndex = concrete.GetConstructorContractsIndex();
            
            var parentContainer = containerInfo.ParentContainer;
            var contractsSparse = containerInfo.ContractsSparse;
            var contractsDense = containerInfo.ContractsDense;
            var contractsConcretesIndices = containerInfo.ContractsConcretesIndices;
            var concreteConstructorContractIds = containerInfo.ConcreteConstructorContractIds;
            
            for (var i = 0; i < constructorContractsCount; i++)
            {
                var containerInfoToCheck = default(ContainerInfo);
                var constructorContractId = concreteConstructorContractIds[i + constructorContractsIndex];
                var constructorContractIndex = contractsSparse[constructorContractId];
                
                if (constructorContractIndex < 0 &&
                    parentContainer != null &&
                    parentContainer.TryFindContainerWithContract(constructorContractId, out var targetContainer))
                {
                    var targetContainerInfo = targetContainer.GetContainerInfo();
                    
                    parentContainer = targetContainerInfo.ParentContainer;
                    contractsSparse = targetContainerInfo.ContractsSparse;
                    contractsDense = targetContainerInfo.ContractsDense;
                    contractsConcretesIndices = targetContainerInfo.ContractsConcretesIndices;
                    concreteConstructorContractIds = targetContainerInfo.ConcreteConstructorContractIds;
                    
                    constructorContractIndex = contractsSparse[constructorContractId]; // don't need to check that exist because was found through TryFindContainerWithContract
                    containerInfoToCheck = targetContainerInfo;
                }
                else
                {
                    containerInfoToCheck = containerInfo;
                }

                if (constructorContractIndex >= 0)
                {
                    ref var constructorContract = ref contractsDense[constructorContractIndex];
                
                    for (var j = 0; j < constructorContract.GetConcretesCount(); j++)
                    {
                        var concreteIdx = contractsConcretesIndices[j + constructorContract.GetConcretesIndex()];
                        
                        ThrowIfInvalidRecursive(originConcretes, originConcreteIndex, concreteIdx,
                            containerInfoToCheck, depth + 1);
                    }
                }
            }
        }
        
        private static void ConstructExceptionRecursiveByReflection(Type type, List<Type> stack, out SparseInjectException exception)
        {
            exception = null;
            
            for (var i = 0; i < stack.Count; i++)
            {
                var dependency = stack[i];
                
                if (type == dependency)
                {
                    stack.Add(type);

                    var sb = new StringBuilder();
                    var ident = 0;
                    
                    sb.AppendLine($"'{type}' contains circular dependency:");
                    
                    foreach (var element in stack)
                    {
                        var identSymbols = new char[ident];

                        for (int j = 0; j < identSymbols.Length; j++)
                        {
                            identSymbols[j] = ' ';
                        }

                        var identText = new string(identSymbols);
                        
                        sb.Append(identText).Append("-> ").AppendLine(element.ToString());
                        
                        ident++;
                    }

                    exception = new SparseInjectException(sb.ToString());
                    
                    return;
                }
            }
        
            stack.Add(type);

            var constructor = ReflectionUtility.GetInjectableConstructor(type);
            
            foreach (var x in constructor.parameters)
            {
                ConstructExceptionRecursiveByReflection(x.ParameterType, stack, out exception);
            }
        
            stack.RemoveAt(stack.Count - 1); // TODO: check if this code reachable
        }
    }
}
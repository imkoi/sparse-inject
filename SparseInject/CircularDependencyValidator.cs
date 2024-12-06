using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.IL2CPP.CompilerServices;

namespace SparseInject
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class CircularDependencyValidator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowIfInvalid(ContainerInfo containerInfo)
        {
            var concretesCount = containerInfo.ConcretesCount;
            
            for (var i = 0; i < concretesCount; i++)
            {
                ThrowIfInvalidRecursive(i, i, containerInfo, 0);
            }
        }

        private static void ThrowIfInvalidRecursive(int originConcreteIndex, int concreteIndex,
            ContainerInfo containerInfo, int depth)
        {
            var concretes = containerInfo.Concretes;
            
            ref var concrete = ref concretes[concreteIndex];

            if (depth > 0 && originConcreteIndex == concreteIndex)
            {
                ThrowRecursiveByReflection(concrete.Type, new List<Type>(depth));
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
                }

                if (constructorContractIndex >= 0)
                {
                    ref var constructorContract = ref contractsDense[constructorContractIndex];
                
                    for (var j = 0; j < constructorContract.GetConcretesCount(); j++)
                    {
                        var concreteId = contractsConcretesIndices[j + constructorContract.GetConcretesIndex()];
                        
                        ThrowIfInvalidRecursive(originConcreteIndex, concreteId, containerInfo, depth + 1);
                    }
                }
            }
        }
        
        private static void ThrowRecursiveByReflection(Type type, List<Type> stack)
        {
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
                        Array.Fill(identSymbols, ' ');
                        
                        var identText = new string(identSymbols);
                        
                        sb.Append(identText).Append("-> ").AppendLine(element.ToString());
                        
                        ident++;
                    }

                    throw new SparseInjectException(sb.ToString());
                }
            }
        
            stack.Add(type);

            var constructor = ReflectionUtility.GetInjectableConstructor(type);
            
            foreach (var x in constructor.parameters)
            {
                ThrowRecursiveByReflection(x.ParameterType, stack);
            }
        
            stack.RemoveAt(stack.Count - 1);
        }
    }
}
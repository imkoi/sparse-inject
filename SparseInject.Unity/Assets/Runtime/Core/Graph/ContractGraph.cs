using System;
using System.Collections.Generic;

namespace SparseInject
{
    public class ContractGraph
    {
        public bool IsCollection { get; }
        public IReadOnlyList<ConcreteGraph> Concretes { get; }
        public Type Type { get; }
        
        public ContractGraph(Type type, bool isCollection, IReadOnlyList<ConcreteGraph> concretes)
        {
            Concretes = concretes;
            Type = type;
            IsCollection = isCollection;
        }
    }
}
#if DEBUG
using System;
using System.Collections.Generic;

namespace SparseInject
{
    public class ConcreteGraph
    {
        public Type Type { get; }
        public IReadOnlyList<ContractGraph> Dependencies => _dependencies;
        public object Value { get; }
        public bool IsSingleton { get; }
        public bool HasValue { get; }
        public bool IsFactory { get; }
        public bool IsScope { get; }
        public bool IsArray { get; }
        
        public IReadOnlyList<int> DependencyIndices { get; }

        private readonly List<ContractGraph> _dependencies;

        public ConcreteGraph(Type type, bool isSingleton, bool hasValue, bool isArray, bool isFactory, bool isScope,
            IReadOnlyList<int> dependencyIndices, object value)
        {
            Type = type;
            Value = value;
            IsSingleton = isSingleton;
            HasValue = hasValue;
            IsArray = isArray;
            IsFactory = isFactory;
            IsScope = isScope;
            DependencyIndices = dependencyIndices;
            
            _dependencies = new List<ContractGraph>();
        }

        internal void AddDependency(ContractGraph dependency)
        {
            _dependencies.Add(dependency);
        }
    }
}
#endif
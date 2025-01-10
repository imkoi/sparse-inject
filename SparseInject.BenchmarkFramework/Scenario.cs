namespace SparseInject.BenchmarkFramework
{
    public abstract class Scenario
    {
        public abstract string Name { get; }
    
        public virtual void BeforeExecute() { }
        public abstract void Execute();
    }
}
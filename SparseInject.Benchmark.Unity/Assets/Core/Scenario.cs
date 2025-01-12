namespace SparseInject.BenchmarkFramework
{
    public abstract class Scenario
    {
        public virtual int ExecuteCount => 1;
        public abstract string Name { get; }
    
        public virtual void BeforeExecute() { }
        public abstract void Execute();
    }
}
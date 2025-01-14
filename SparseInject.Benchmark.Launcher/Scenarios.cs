namespace SparseInject.Benchmark.Launcher;

public class Scenarios
{
    public static readonly List<string> AllContainerScenarios = new List<string>
    {
        "SparseInject",
        "VContainer",
        "Reflex",
        "Zenject"
    };
    
    public static readonly List<string> ManualContainerScenarios = new List<string>
    {
        "ManualResolver",
    };
    
    public static readonly List<string> AllTypeIdProviderScenarios = new List<string>
    {
        "Dictionary",
        "TypeIdProvider"
    };
}
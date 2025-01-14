namespace SparseInject.Benchmark.Launcher;

public static class Categories
{
    public static readonly List<string> Transient = new List<string>
    {
        "transient-register-depth",
        "transient-build-depth",
        "transient-register-and-build-depth",
        "transient-first-resolve-depth",
        "transient-second-resolve-depth",
        "transient-total-depth",
    };
    
    public static readonly List<string> Singleton = new List<string>
    {
        "singleton-register-depth",
        "singleton-build-depth",
        "singleton-register-and-build-depth",
        "singleton-first-resolve-depth",
        "singleton-second-resolve-depth",
        "singleton-total-depth",
    };
    
    public static readonly List<string> TypeIdProvider = new List<string>
    {
        "type-id-provider"
    };
}
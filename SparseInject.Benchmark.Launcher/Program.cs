using SparseInject.Benchmark.Launcher;

const int maxDepth = 6;
const string executablePath = "C:/github/sparseinject/SparseInject.Benchmark.Unity/Build/il2cpp/SparseInject.Benchmark.Unity.exe";

//BenchmarkLauncher.Launch(executablePath, Categories.TypeIdProvider, Scenarios.AllTypeIdProviderScenarios);

// BenchmarkLauncher.Launch(executablePath, new []
// {
//     "transient-register-depth6",
//     "transient-build-depth6",
//     "transient-register-and-build-depth6",
//     "transient-first-resolve-depth6",
//     "transient-second-resolve-depth6",
//     "transient-total-depth6",
// }, Scenarios.AllContainerScenarios, 5);

BenchmarkLauncher.Launch(executablePath, new []
{
    "transient-register-depth6",
    "transient-build-depth6",
    "transient-register-and-build-depth6",
    "transient-first-resolve-depth6",
    "transient-second-resolve-depth6",
    "transient-total-depth6",
}, new List<string> { "SparseInject" }, 1);





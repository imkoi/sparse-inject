using SparseInject.Benchmark.Launcher;

const int maxDepth = 6;
const string il2cppPath = "C:/github/sparseinject/SparseInject.Benchmark.Unity/Build/il2cpp/SparseInject.Benchmark.Unity.exe";
const string il2cppSourceGeneratorsPath = "C:/github/sparseinject/SparseInject.Benchmark.Unity/Build/il2cpp-source-generators/SparseInject.Benchmark.Unity.exe";
const string monoPath = "C:/github/sparseinject/SparseInject.Benchmark.Unity/Build/mono/SparseInject.Benchmark.Unity.exe";
const string monoSourceGeneratorsPath = "C:/github/sparseinject/SparseInject.Benchmark.Unity/Build/mono-source-generators/SparseInject.Benchmark.Unity.exe";
const string androidPackage = "com.voxcake.sparseinject";
const string memoryProfiler = "C:/github/sparseinject/SparseInject.Benchmark.Unity/Build/memory-profile/SparseInject.Benchmark.Unity.exe";
const string last56Build = "C:/github/sparseinject/SparseInject.Benchmark.Unity/Build/il2cpp-source-generators-56/SparseInject.Benchmark.Unity.exe";
const string last57Build = "C:/github/sparseinject/SparseInject.Benchmark.Unity/Build/il2cpp-source-generators-56/SparseInject.Benchmark.Unity.exe";

await BenchmarkLauncher.Launch(last56Build,
    new []
    {
        "transient-register-depth6",
        "transient-build-depth6",
        "transient-register-and-build-depth6",
        "transient-first-resolve-depth6",
        "transient-second-resolve-depth6",
        "transient-total-depth6",
    },
    new []
    {
        "SparseInject",
        "VContainer",
        "Reflex",
        "Zenject"
    },
    BenchmarkLauncher.Platform.Windows, 10);

return;

// await BenchmarkLauncher.Launch(androidPackage, Categories.TypeIdProvider, Scenarios.AllTypeIdProviderScenarios,
//     BenchmarkLauncher.Platform.Android, 10);
//
// await BenchmarkLauncher.Launch(androidPackage, new []
// {
//     "transient-register-depth6",
//     "transient-build-depth6",
//     "transient-register-and-build-depth6",
//     "transient-first-resolve-depth6",
//     "transient-second-resolve-depth6",
//     "transient-total-depth6",
// }, Scenarios.AllContainerScenarios, BenchmarkLauncher.Platform.Android, 10);
//
// await BenchmarkLauncher.Launch(androidPackage, new []
// {
//     "transient-first-resolve-depth6",
//     "transient-second-resolve-depth6",
//     "transient-total-depth6",
// }, Scenarios.ManualContainerScenarios, BenchmarkLauncher.Platform.Android, 10);

await BenchmarkLauncher.Launch(androidPackage, new []
{
    "transient-register-depth6",
    "transient-build-depth6",
    "transient-register-and-build-depth6",
    "transient-first-resolve-depth6",
    "transient-second-resolve-depth6",
    "transient-total-depth6",
}, new List<string> { "SparseInject" }, BenchmarkLauncher.Platform.Android, 10);





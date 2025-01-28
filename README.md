# SparseInject
![main github action workflow](https://github.com/imkoi/sparse-inject/actions/workflows/dotnet.yml/badge.svg) [![MIT license](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

### Dependency Injection Container forged for game development

---
### Overview
#### 🚀 **Fastest**
- ⚡ **20x faster** than Zenject
- ⚡ **7x faster** than Reflex
- ⚡ **2.5x faster** than VContainer
#### 🧠 **Small Memory Footprint**
- 📉 **4x fewer allocations** compared to VContainer
- 📉 **2x smaller allocation size** than VContainer
- 📉 **2x smaller empty heap space** than VContainer
- 📦 **30% smaller build size** compared to VContainer
#### ✨ **Minimalistic**
- 🎮 Build **complex games** with **simple code**
- 🛡️ Avoid features that create dependencies on a specific DI implementation
- ✂️ Easily **exclude specific business logic** from the DI container
- 🔄 Smoothly **migrate from SparseInject to any other container**
#### 🛡️ **Stable**
- ✅ **100% test coverage**, compared to VContainer’s 70% coverage
#### 🌎 **Run Everywhere**
- 🔗 **No dependencies** on specific engines — works with any C# environment
- 📱 **AOT-ready**: Uses minimal reflection to ensure maximum compatibility
- 💻 Supports **Standalone**, **Mobile**, **Console**, **WebGL**, and more!
---
### Installation
#### Unity Package Manager 📂
```
https://github.com/imkoi/sparse-inject.git?path=/SparseInject.Unity/Assets/#1.0.0
```

1. Open **Window** → **Package Manager**.
2. Click the **+** button → **Add package from git URL...**
3. Enter url and click **Add**.

---
### How To
#### Transient
Gives ability to create new instance on each resolve
```csharp 
using System; 

class Program 
{
    static void Main() 
    {
        Console.WriteLine("Hello, SparseInject!");
    }
}
```
#### Singletons
Gives ability to return same instance on each resolve
```csharp 
using System; 

class Program 
{
    static void Main() 
    {
        Console.WriteLine("Hello, SparseInject!");
    }
}
```
#### Collections
Gives ability to resolve array of specific types
```csharp 
using System; 

class Program 
{
    static void Main() 
    {
        Console.WriteLine("Hello, SparseInject!");
    }
}
```
#### Factory
Gives ability to create your instance by specific logic and parameter
```csharp 
using System; 

class Program 
{
    static void Main() 
    {
        Console.WriteLine("Hello, SparseInject!");
    }
}
```
#### Scopes
Gives ability to split perfromance overhead and incapsulate speific types
```csharp 
using System; 

class Program 
{
    static void Main() 
    {
        Console.WriteLine("Hello, SparseInject!");
    }
}
```

---
### Benchmarks
In my performance measurements, I would provide the most relevant benchmark scenarios to demonstrate how **Sparse Inject** scales for your project. These benchmarks are tailored to fit most large projects built with Unity, as these are the projects that **WILL** have critical performance overhead.

##### 1. **Mobile Platform** 📱
- Most big Unity games are mobile games, with a large number of players using slower devices.
##### 2. **Big Code Base with Complex Composition Root** 🏗️
- When a game is developed by 30+ developers over several years, the codebase becomes huge and complex.
##### 3. **IL2CPP Scripting Backend** 🛠️
- iOS requires you to publish IL2CPP builds only.
##### 4. **Release Configuration** 🚀
- No commentary needed 😄.
##### 5. **Isolated Environment Without Prewarm** 🧪
- Each benchmark sample is run in an isolated process, but the application is launched many times to leverage deviation. This ensures we capture realistic performance without pre-allocated VM stuff or prewarmed methods on start.

##### Registrations Sources 📂
- [ReflexTransientRegistrator_Depth6.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FRegistrators%2FReflexTransientRegistrator_Depth6.cs)
- [SparseInjectTransientRegistrator_Depth6.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FRegistrators%2FSparseInjectTransientRegistrator_Depth6.cs)
- [VContainerTransientRegistrator_Depth6.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FRegistrators%2FVContainerTransientRegistrator_Depth6.cs)
- [ZenjectTransientRegistrator_Depth6.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FRegistrators%2FZenjectTransientRegistrator_Depth6.cs)

## Total time
This metric shows the time a user spends on container configuration and the first resolve. **This is the most critical metric affecting loading times**.

#### Scenarios Sources 📂
- [ReflexTransientTotal_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FTotal%2FReflexTransientTotal_Depth6Scenario.cs)
- [SparseInjectTransientTotal_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FTotal%2FSparseInjectTransientTotal_Depth6Scenario.cs)
- [VContainerTransientTotal_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FTotal%2FVContainerTransientTotal_Depth6Scenario.cs)
- [ZenjectTransientTotal_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FTotal%2FZenjectTransientTotal_Depth6Scenario.cs)

![il2cpp-android-total.png](Images%2FPerfromanceV1_0_0%2FAndroid%2Fil2cpp-source-generators%2Fil2cpp-android-total.png)
> [!WARNING]
> While reflex looks pretty fast - you need to remeber that it dont have circular dependency checks
> and has 90x slower first resolve time and 7 times slower next resolve times, because he analyze registered types on first resolve

## Configuration time
This metric shows the time a user spends on container configuration and build.

#### Scenarios Sources 📂
- [ReflexTransientRegisterAndBuild_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FRegisterAndBuild%2FReflexTransientRegisterAndBuild_Depth6Scenario.cs)
- [SparseInjectTransientRegisterAndBuild_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FRegisterAndBuild%2FSparseInjectTransientRegisterAndBuild_Depth6Scenario.cs)
- [VContainerTransientRegisterAndBuild_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FRegisterAndBuild%2FVContainerTransientRegisterAndBuild_Depth6Scenario.cs)
- [ZenjectTransientRegisterAndBuild_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FRegisterAndBuild%2FZenjectTransientRegisterAndBuild_Depth6Scenario.cs)

![il2cpp-android-registration-and-build.png](Images%2FPerfromanceV1_0_0%2FAndroid%2Fil2cpp-source-generators%2Fil2cpp-android-registration-and-build.png)
> [!WARNING]
> Winner is Reflex as he doesn't analyze types on building and doesn't perform circular dependency checks.  
> However, we see that **SparseInject** is **15 times faster** than **VContainer**!

> [!WARNING]
> **Life-hack for VContainer users**: Disable reflection baking, and it will give you a **30% configuration time boost**,  
> while having a relatively slow degradation in resolve time.

---
### Features
- Transient and Singleton bindings - singletons will be disposed on container / scopes dispose
- Value bindings - bind values as singletons
- Collections injection - its possible to inject ISomeType[] collection into constructors
- Scopes - gives possibility to provide new isolated bindings that will be disposed after scope destroy
- Separated binding and resolving stages - you cant bind new dependencies into container on runtime
- Container only injection - there is no possibility to inject methods, properties and fields

---
### Performance
In comparison to VContainer with Reflection:
- 11x faster binding times than VContainer
- 40% faster resolve than VContainer
- 30% less allocations
- Smaller build size than VContainer
- Careful usage of reflection functional - even typeof are always cached

  | Metric                                       | SparseInject    | VContainer      | Difference            |
    |----------------------------------------------|------------------|-----------------|-----------------------|
  | Bind Time                                    | **2923**,563 ms  | **34114,22** ms | **91**.43% **Faster** |
  | Resolve Time [Min]                           | **40**,5466 ms   | **67**,8947 ms  | **40**.26% Faster     |
  | Resolve Time [Avg]                           | **44**,88753 ms  | **72**,28737 ms | **37**.91% Faster     |
  | Resolve Time [Max]                           | **67**,6113 ms   | **97**,1772 ms  | **30**.42% Faster     |
  | Resolve Time [Total Spend]                   | **8079**,756 ms  | **13011**,73 ms | **37**.91% Faster     |
  | **Session Time** [DI time spent per session] | **11003**,319 ms | **47125**,95 ms | **76**.65% **Faster** |

In comparison to VContainer with **Reflection Baking enabled**:
- 15x faster binding times than VContainer
- Same resolve speed on first resolve, 15% slower on next resolves, **BUT** Its important to remember that most games use instances that resolved just once
- 30% less allocations
- Up to 2x smaller build size (depend on amount of methods and fields in classes, 2x improve with constructors only)

| Metric                                       | SparseInject   | VContainer **Reflection Baking** | Difference             |
|----------------------------------------------|-----------------|----------------------------------|------------------------|
| Bind Time                                    | **2974**,29 ms  | **47184**,44 ms                  | **93**.7% **Faster**   |
| Resolve Time [Min]                           | **36**,1111 ms  | **31**,1136 ms                   | **16**.06% Slower      |
| Resolve Time [Avg]                           | **40**,30264 ms | **33**,82388 ms                  | **19**.15% Slower      |
| Resolve Time [Max]                           | **65**,71 ms    | **64**,9757 ms                   | **1**.13%   Slower     |
| Resolve Time [Total Spend]                   | **32161**,5 ms  | **26991**,45 ms                  | **19**.15%    Slower   |
| **Session Time** [DI time spent per session] | **35135**,79 ms | **74175**,89 ms                  | **52**.63%  **Faster** |
--- 
### Allocations
> [!NOTE]
> Its hard to explain how much allocations SparseInject have, but for example in this benchmark on bindings stage:
> SparseInject call _GC.Alloc_ **270**972 times, while VContainer call **618**832 _GC.Alloc_.
> SparseInject allocated 25.7 MB of ram (mostly for reflection) while VContainer allocated 32.7 MB
#### Container Configuration Stage
On this stage we registering dependencies to our container, to get information about types we will want to resolve
- allocated 3 big arrays (default 4k elements) to store info regarding dependencies - could be reallocated when capacity reached (are you making gta 7 or just playing with code in bad way?)
- allocated 2 dictionaries with capacity (default 4k elements) for mapping types with id to not use reflection at runtime at all - allocations depends on type hashCode
- allocated types of dependencies and cached - depend on number of registration of unique types

#### Container Building Stage
On this stage we building container to have ability to resolve instances, but before container build we bake all dependencies and cache reflection
- allocated **one** large array that reference all dependencies foreach implementation - Allocated ONCE
- allocated reflection data regarding constructor, constructor parameters, types of this parameters - Allocations depends on type

#### Resolve Stage
On this stage we resolving our instances
- allocated object foreach instance through FormatterService - depend on number of resolving dependencies
- allocated arrays **in case of Collection injection** - we should provide unique collection foreach instance

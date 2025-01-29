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
- 💯 **100% test coverage**, compared to 60% coverage of competitors
- ✅ **Smaller SparseInject codebase** has 2x more test cases than competitors
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
### Content tree
- [Usages](#usages)
- [Limitations](#-limitations)
- - [Why limitations are important](#why-limitations-are-important)
- - [What limitations exist](#what-limitations-exists)
- [Benchmarks](#benchmarks)
- - [CPU Total Time](#total-time)
- - [CPU Resolve Time](#resolve-time)
- - [CPU Registration And Build  Time](#registration-and-build-time)
- - [Memory Usage and Allocations](#memory-usage-and-allocations)
- [Cons](#cons)

---
### Usages
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
##### Key Goals of Scopes
1. **Encapsulation of Registrations**
   - Scopes allow registrations to be accessible only within the scope where they are defined.
   - When a dependency is resolved in a scope, the scope first checks its own registrations. If the dependency is not found, it looks in the parent scope.
   - Parent scopes do not have access to child scope registrations.
   - Registrations inside a scope are processed only when the scope is created.
2. **Access to Parent Dependencies**
   - Scopes can access dependencies registered in the parent container, allowing reuse of shared services while isolating scope-specific dependencies.
3. **Overriding Registrations in Scopes**
   - Scopes enable overriding of dependencies defined in the parent container, allowing context-specific implementations.
4. **Lifecycle Management**
   - Scopes can manage their own disposables. When a scope is disposed, all disposable instances created within the scope are also disposed.

<details>
<summary>Encapsulating Registrations in a Scope</summary>

```csharp
// GameplayController inject Func<PlayerController> to constructor
containerBuilder.RegisterScope<GameplayController>(innerBuilder =>
{
    innerBuilder.RegisterFactory(scope => new PlayerController(scope.Resolve<IAudioService>()));
});

var container = containerBuilder.Build();

// return instance with injected Func<PlayerController> to constructor
var gameplayController = container.Resolve<GameplayController>();

// will throw exception, because Func<PlayerController> exist only inside GameplayController scope
var playerFactory = container.Resolve<Func<PlayerController>>(); 
```

</details>

<summary>Accessing Parent Scope Registrations in a Child Scope</summary>

```csharp
containerBuilder.Register<RewardService>(Lifetime.Singleton);

// GameplayController inject RewardService
containerBuilder.RegisterScope<GameplayController>(innerBuilder =>
{
    // registrations
});

var container = containerBuilder.Build();

// return instance of type RewardService from root container
var rewardService = container.Resolve<RewardService>();

// return instance with injected RewardService from root container
var gameplayController = container.Resolve<GameplayController>();
```

<summary>Overriding Registrations in a Scope</summary>

```csharp
containerBuilder.Register<IAudioService, MenuAudioService>(Lifetime.Singleton);

// GameplayController inject IAudioService
containerBuilder.RegisterScope<GameplayController>(innerBuilder =>
{
    // GameplayAudioService will use stereo instead of mono sounds
    innerBuilder.Register<IAudioService, GameplayAudioService>(Lifetime.Singleton).MarkDisposable();
});

var container = containerBuilder.Build();

// return instance of type MenuAudioService from root container
var audioService = container.Resolve<IAudioService>(); 

// return instance with injected GameplayAudioService from GameplayController scope
var gameplayController = container.Resolve<GameplayController>();
```

<summary>Lifecycle of Scope and its Registrations</summary>

```csharp
// GameplayController inject IAudioService
containerBuilder.RegisterScope<GameplayController>(innerBuilder =>
{
    //We mark it as disposable, to call Dispose method of instance when scope will be destroyed
    innerBuilder.Register<IAudioService, GameplayAudioService>(Lifetime.Singleton).MarkDisposable();
});

var container = containerBuilder.Build();

// return instance with injected GameplayAudioService
var gameplayController = container.Resolve<GameplayController>();

// this is Scope.Dispose call, it will dispose GameplayAudioService and all data allocated for scope
gameplayController.Dispose(); 
```

<summary>Instantiating a Scope at Specific Time</summary>

```csharp
containerBuilder.RegisterScope<GameplayController>(innerBuilder =>
{
    // registrations
});
containerBuilder.RegisterFactory(scope => scope.Resolve<GameController>());

var container = containerBuilder.Build();

// could be cached and used in any time
var gameplayControllerFactory = container.Resolve<Func<GameplayController>>();

// return instance of scope
var gameplayController = gameplayControllerFactory.Invoke(); 
```
</details>

---
### 🚧 Limitations
### Why Limitations are important?

1. #### ⏱️ Ease of Use
   - A minimal learning curve is essential for developers. The API should be intuitive and easy for newcomers to adopt quickly. Clear, well-known patterns ensure every developer on your project feels comfortable using it without steep onboarding.

2. #### 🔄 Migration Flexibility
   - To ensure an easy transition to other DI containers in the future, the feature set and dependencies on the current implementation must be minimal. This reduces the lock-in effect and allows flexibility for evolving project needs.

3. #### 🚀 Performance by Default
   - In large projects, it's challenging to enforce how APIs are used. Poorly optimized features can severely impact application performance and loading times. For this reason, only performant-by-default features will be implemented, minimizing the risk of misuse.

4. #### 🧠 Encouraging Simplicity and Clean Code
   - By imposing limitations, developers are guided toward writing simpler, cleaner code. This approach promotes maintainable solutions while reducing unnecessary complexity in implementation.

5. #### 👤 Managed by a Single Developer
   - Since the project is managed and supported by one person, maintaining a large feature set would lead to time-consuming edge cases and complexity. Limiting the scope ensures better quality and more sustainable development.

### What Limitations exists?
1. #### ❌ No Inject Attribute
   - Adding an `Inject` attribute introduces a dependency on a specific DI container implementation, making your code harder to test and increasing memory usage.
   - **Instead:** Decouple your logic from views or components that rely on resource-heavy dependencies.

2. #### ❌ No Lazy Injection
   - Lazy injection is excluded because the same behavior can be achieved using factories, offering a more explicit and manageable approach.

3. #### ❌ No Inject Key
   - Inject keys are not supported because factories with parameters can achieve the same functionality, promoting clarity and flexibility.

4. #### ❌ No Conditional Bindings
   - Conditional bindings are omitted because their functionality can be replicated using factories with parameters, avoiding unnecessary complexity.

5. #### ❌ No Open Generics Injection
   - Open generics injection is avoided due to its negative impact on performance, especially in AOT (Ahead-of-Time) scenarios where reflection is costly.
   - **Instead:** Achieve the same functionality using simpler, explicit code.

6. #### ❌ No Runtime Modifications After Container Build
   - Allowing bindings to be modified after the container is configured is considered a bad practice. It introduces unpredictable behavior during the resolve phase.
   - **Instead:** Finalize configurations during the build stage.

7. #### ❌ No Dynamic Type Registration
   - This decision ensures potential registration issues are caught at compile time, improving code clarity and reducing runtime errors.

8. #### ❌ No Unity-Specific Extensions
   - Unity-specific extensions are not provided to avoid dependencies on the Unity engine.
   - **However:** I recommend to register your prefabs as singletons or factories return instance of prefab.

9. #### ❌ No Extra Features Outside DI Responsibility
   - Features like decorators and other unrelated functionalities are not included to maintain a focused and lightweight DI container.
   - **Philosophy:** Keep It Purposeful (KIP).

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


## Resolve time
This metric shows the time a user spends on creating game instances. **This is critical at runtime, to avoid lags and freezes**.

#### Scenarios Sources 📂
- [ReflexTransientFirstResolve_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FFirstResolve%2FReflexTransientFirstResolve_Depth6Scenario.cs)
- [SparseInjectTransientFirstResolve_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FFirstResolve%2FSparseInjectTransientFirstResolve_Depth6Scenario.cs)
- [VContainerTransientFirstResolve_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FFirstResolve%2FVContainerTransientFirstResolve_Depth6Scenario.cs)
- [ZenjectTransientFirstResolve_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FFirstResolve%2FZenjectTransientFirstResolve_Depth6Scenario.cs)
- [ManualTransientFirstResolve_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FFirstResolve%2FManualTransientFirstResolve_Depth6Scenario.cs)
- [ReflexTransientSecondResolve_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FSecondResolve%2FReflexTransientSecondResolve_Depth6Scenario.cs)
- [SparseInjectTransientSecondResolve_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FSecondResolve%2FSparseInjectTransientSecondResolve_Depth6Scenario.cs)
- [VContainerTransientSecondResolve_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FSecondResolve%2FVContainerTransientSecondResolve_Depth6Scenario.cs)
- [ZenjectTransientSecondResolve_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FSecondResolve%2FZenjectTransientSecondResolve_Depth6Scenario.cs)
- [ManualTransientSecondResolve_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FSecondResolve%2FManualTransientSecondResolve_Depth6Scenario.cs)

![il2cpp-android-first-resolve.png](Images%2FPerfromanceV1_0_0%2FAndroid%2Fil2cpp-source-generators%2Fil2cpp-android-first-resolve.png)
> [!NOTE]
> **SparseInject** is **2 times faster than VContainer** and **90x faster than Reflex**.

> [!WARNING]
> ManualResolver - simple [static methods](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FManualResolvers%2FManualResolver_Depth6.cs) that create instances through the native `new` operator
> **Why is SparseInject faster than instancing through the native `new` operator?**  
> This happens because of how Unity handles reference type instantiation:
> 1. On the first method call, Unity allocates or fetches all metadata for the referenced classes in method.
> 2. This metadata is cached in a static variable to avoid fetching it on subsequent calls.
>
> In contrast, **SparseInject** allocates class metadata during the container build stage, so resolving an instance simply fetches the pre-allocated metadata.  
> As a result, resolve through `new` operator is **slower** on the first instance creation compared to a DI container like SparseInject.

![il2cpp-android-second-resolve.png](Images%2FPerfromanceV1_0_0%2FAndroid%2Fil2cpp-source-generators%2Fil2cpp-android-second-resolve.png)
> [!NOTE]
> **SparseInject** is almost **3 times faster than VContainer** and **7x faster than Reflex**.

> [!WARNING]
> Now we see that instancing through simple static methods are 2.5 faster as it not have algorithm to find dependencies


## Registration and Build time
This metric shows the time a user spends on container configuration and build.

#### Scenarios Sources 📂
- [ReflexTransientRegisterAndBuild_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FRegisterAndBuild%2FReflexTransientRegisterAndBuild_Depth6Scenario.cs)
- [SparseInjectTransientRegisterAndBuild_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FRegisterAndBuild%2FSparseInjectTransientRegisterAndBuild_Depth6Scenario.cs)
- [VContainerTransientRegisterAndBuild_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FRegisterAndBuild%2FVContainerTransientRegisterAndBuild_Depth6Scenario.cs)
- [ZenjectTransientRegisterAndBuild_Depth6Scenario.cs](SparseInject.Benchmark.Unity%2FAssets%2FBenchmark%2FScenarios%2FTransient%2FDepth_6%2FRegisterAndBuild%2FZenjectTransientRegisterAndBuild_Depth6Scenario.cs)

![il2cpp-android-registration-and-build.png](Images%2FPerfromanceV1_0_0%2FAndroid%2Fil2cpp-source-generators%2Fil2cpp-android-registration-and-build.png)
> [!Note]
> We see that **SparseInject** is **15 times faster** than **VContainer**!

> [!WARNING]
> However, **Reflex is winner** as he doesn't analyze types on building and doesn't perform circular dependency checks.  

> [!WARNING]
> **Life-hack for VContainer users**: Disable reflection baking, and it will give you a **30% configuration time boost**,  
> while having a relatively slow degradation in resolve time.

## Memory usage and allocations
This metric shows how much memory overhead you will encounter when using DI containers.

Here, I compare memory usage only with **VContainer** because:
1. I'm too lazy to manually profile all containers 😢
2. **VContainer** looks more robust and feature-complete in my opinion.
3. It includes circular dependency checks, which are mandatory for large projects.
4. **VContainer** can minimize reflection usage with source generators, helping to keep the VM size small.

[Sources 📂](Images%2FPerfromanceV1_0_0%2FAndroid%2Fil2cpp-source-generators%2Fmemory)

![allocation-size.png](Images%2FPerfromanceV1_0_0%2FAndroid%2Fil2cpp-source-generators%2Fmemory%2Fallocation-size.png)
![gc-alloc-count.png](Images%2FPerfromanceV1_0_0%2FAndroid%2Fil2cpp-source-generators%2Fmemory%2Fgc-alloc-count.png)
![empty-heap-space.png](Images%2FPerfromanceV1_0_0%2FAndroid%2Fil2cpp-source-generators%2Fmemory%2Fempty-heap-space.png)
> [!NOTE]
> **SparseInject** makes **4 times fewer allocations** than **VContainer**.
>
> **SparseInject** has a **GC allocation size 2 times smaller** than **VContainer**.
>
> **SparseInject** leaves **2 times less empty space** in the heap compared to **VContainer**!

> [!WARNING]
> Metrics was gathered through unity profiler and memory profiler after built and root resolve.

---
### Cons
As we know, nothing in life is perfect, and I want to warn developers❤️ about the cons of **SparseInject**:

1. **Complex Codebase**  
   The codebase is small but complex due to the data structures implemented to maintain high performance. While it’s more enjoyable to explore the implementations of **VContainer** or **Reflex**, this complexity is why **SparseInject** has **100% test coverage**.
2. **Hard Debugging**  
   Debugging can be challenging. To make it easier, I’ve added [ContainerGraph.cs](sparseinject%2FGraph%2FContainerGraph.cs), which helps visualize your container’s structure for better clarity during debugging.
3. **Dotnet 8 AOT Compatibility**  
   While **SparseInject** works with .NET 8 AOT, some reflection calls like `Array.CreateInstance()` can slow it down. It’s still relatively fast, but this is something to keep in mind.
4. **Higher VM Memory Usage**  
   **SparseInject** uses slightly more VM memory than **VContainer** (about 15% more). This is due to its support for jagged collections resolution, which **VContainer** does not support.

SparseInject
![SparseInject.png](Images%2FPerfromanceV1_0_0%2FAndroid%2Fil2cpp-source-generators%2Fmemory%2Fsparse-inject-memory.png)
VContainer
![VContainer.png](Images%2FPerfromanceV1_0_0%2FAndroid%2Fil2cpp-source-generators%2Fmemory%2Fvcontainer-memory.png)

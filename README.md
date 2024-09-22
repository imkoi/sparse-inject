# CleanResolver
DI Container forged for game development

### Performance
In comparson to VContainer in general:
- No allocations on resolve stage except instances
- Few allocations and reallocation on binding stage, small games will mostly have only 6 allocations and 0 reallocations

In comparison to VContainer with Reflection:
- 11x faster binding times than VContainer
- 40% faster resolve than VContainer
- Smaller build size than VContainer
- Careful usage of reflection functional - even typeof are always cached

  | Metric                                       | CleanResolver    | VContainer      | Difference            |
  |----------------------------------------------|------------------|-----------------|-----------------------|
  | Bind Time                                    | **2923**,563 ms  | **34114,22** ms | **91**.43% **Faster** |
  | Resolve Time [Min]                           | **40**,5466 ms   | **67**,8947 ms  | **40**.26% Faster     |
  | Resolve Time [Avg]                           | **44**,88753 ms  | **72**,28737 ms | **37**.91% Faster     |
  | Resolve Time [Max]                           | **67**,6113 ms   | **97**,1772 ms  | **30**.42% Faster     |
  | Resolve Time [Total Spend]                   | **8079**,756 ms  | **13011**,73 ms | **37**.91% Faster     |
  | **Session Time** [DI time spent per session] | **11003**,319 ms | **47125**,95 ms | **76**.65% **Faster** |

In comparison to VContainer with **Reflection Baking enabled**:
- 15x faster binding times than VContainer
- Same resolve speed on first resolve, 15% slower on next resolves
- Up to 2x smaller build size (depend on amount of methods and fields in classes, 2x improve with constructors only)

| Metric                                       | CleanResolver   | VContainer **Reflection Baking** | Difference             |
|----------------------------------------------|-----------------|----------------------------------|------------------------|
| Bind Time                                    | **2974**,29 ms  | **47184**,44 ms                  | **93**.7% **Faster**   |
| Resolve Time [Min]                           | **36**,1111 ms  | **31**,1136 ms                   | **16**.06% Slower      |
| Resolve Time [Avg]                           | **40**,30264 ms | **33**,82388 ms                  | **19**.15% Slower      |
| Resolve Time [Max]                           | **65**,71 ms    | **64**,9757 ms                   | **1**.13%   Slower     |
| Resolve Time [Total Spend]                   | **32161**,5 ms  | **26991**,45 ms                  | **19**.15%    Slower   |
| **Session Time** [DI time spent per session] | **35135**,79 ms | **74175**,89 ms                  | **52**.63%  **Faster** |

### Features
- Separated binding and resolving stages - you cant bind new dependencies into container unless its created subcontainer
- Singleton and Transient bindings - singletons will be disposed when on container / scopes dispose
- Value bindings - bind values as singleton
- Container only injection - there is no possibility to inject methods, properties and fields
- Collections injection - its possible to inject ISomeType[] collection into container
- Scopes - gives possibility to provide new isolated bindings that will be disposed after scope destroy
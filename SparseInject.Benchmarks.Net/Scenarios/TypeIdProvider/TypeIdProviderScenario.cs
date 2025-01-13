using System;
using SparseInject.BenchmarkFramework;

public class TypeIdProviderScenario : Scenario
{
    public override string Name => "TypeIdProvider";
    public override int ExecuteCount => 100;

    private Type[] _types;

    public override void BeforeExecute()
    {
        _types = typeof(TypeIdProviderScenario).Assembly.GetTypes();
    }

    public override void Execute()
    {
        var typesLength = _types.Length;
        var idProvider = new SparseInject.TypeIdProvider();

        for (var i = 0; i < typesLength; i++)
        {
            var type = _types[i];

            idProvider.GetOrAddId(type);
        }

        for (var i = 0; i < typesLength; i++)
        {
            var type = _types[i];

            idProvider.TryGetId(type, out _);
        }
    }
}
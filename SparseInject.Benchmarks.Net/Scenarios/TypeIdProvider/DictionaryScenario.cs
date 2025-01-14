using System;
using System.Collections.Generic;
using SparseInject.BenchmarkFramework;

public class DictionaryScenario : Scenario
{
    public override string Name => "Dictionary";
    public override int ExecuteCount => 1000;

    private Type[] _types;

    public override void BeforeExecute()
    {
        _types = typeof(DictionaryScenario).Assembly.GetTypes();
    }

    public override void Execute()
    {
        var typesLength = _types.Length;
        var idProvider = new Dictionary<Type, int>(typesLength);

        for (var i = 0; i < typesLength; i++)
        {
            var type = _types[i];

            if (!idProvider.TryGetValue(type, out _))
            {
                idProvider.Add(type, i);
            }
        }

        for (var i = 0; i < typesLength; i++)
        {
            var type = _types[i];

            idProvider.TryGetValue(type, out _);
        }
        
        for (var i = 0; i < typesLength; i++)
        {
            var type = _types[i];

            idProvider.TryGetValue(type, out _);
        }
    }
}
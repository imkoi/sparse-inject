// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using SparseInject;

public class Program
{
    public static void Main()
    {
        var containerBuilder = new ContainerBuilder(16 );
        var sw = new Stopwatch();
        var iter = 100;
        
        sw.Restart();
        
        ContainerBinder.BindDeps(containerBuilder);
        
        var container = containerBuilder.Build();
        
        sw.Stop();
        
        var bindTime = sw.ElapsedTicks / 10000f;
        
        sw.Restart();
        
        for (var i = 0; i < iter; i++)
        {
            var highestDependency = container.Resolve<Class0>();
        }
        
        sw.Stop();
        
        var resolveTime = sw.ElapsedTicks / 10000f;
        
        Console.WriteLine($"Bind Time: {bindTime} ms, Resolve Time: {resolveTime / iter} ms");
        Console.WriteLine(container.ToString());
    }
}
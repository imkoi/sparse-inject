using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Utilities
{
    public class VContainerBenchmarkPatcher
    {
        private const string Path = "C:/github/sparseinject/SparseInject.BenchmarkFramework.Net/TransientRegistrators/VContainerTransientContainerRegistrator.cs";
        
        [Ignore("Not needed")]
        [Test]
        public void GenerateDependencies()
        {
            var lines = File.ReadAllLines(Path);

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var signaturePattern = @"public\s+static\s+void\s+Register\s*\(\s*ContainerBuilder\s+(\w+)\)";
                var registerPattern = @"container\.Register<([^>]+)>\(\)";
                
                if (Regex.IsMatch(line, registerPattern))
                {
                    var replacement = "container.Register(typeof($1), global::VContainer.Lifetime.Transient)";
                    
                    lines[i] = Regex.Replace(line, registerPattern, replacement);
                }
                else if (Regex.IsMatch(line, signaturePattern))
                {
                    var replacement = "public static void Register(VContainer.ContainerBuilder $1)";
                    
                    lines[i] = Regex.Replace(line, registerPattern, replacement);
                }
            }
        
            File.WriteAllLines(Path, lines);
        }
    }
}
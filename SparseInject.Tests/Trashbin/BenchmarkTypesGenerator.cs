using System;
using System.IO;
using NUnit.Framework;

namespace Trashbin
{
    [Ignore("Used only to generate classes for benchmarks")]
    public class BenchmarkTypesGenerator
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void Generate(int depth)
        {
            var (generatedCode, types) = Utilities.GenerateClasses(depth);

            var codeLines = generatedCode.Split("\n");

            for (int i = 0; i < codeLines.Length; i++)
            {
                codeLines[i] = codeLines[i].Replace("\n", "").Replace("\r", "");
            }
        
            var fileDirectory = Path.Combine(Utilities.GetRootFolder(), "SparseInject.Benchmarks.Net/BenchmarkTypes")
                .Replace("\\", "/");
            var typesFile = $"{fileDirectory}/BenchmarkTypes_Depth{depth}.cs";

            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }
            
            File.WriteAllLines(typesFile, codeLines);
            
            Console.WriteLine($"Generated files: {types.Count}");
        }
    }
}
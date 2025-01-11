using System.IO;
using NUnit.Framework;

namespace Trashbin
{
    [Ignore("Not used in CI")]
    public class BenchmarkTypesGenerator
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void GenerateBenchmarkTypes(int depth)
        {
            var (generatedCode, _) = Utilities.GenerateClasses(depth);

            var codeLines = generatedCode.Split("\n");

            for (int i = 0; i < codeLines.Length; i++)
            {
                codeLines[i] = codeLines[i].Replace("\n", "").Replace("\r", "");
            }
        
            var typesFile = Path.Combine(Utilities.GetRootFolder(), $"SparseInject.Benchmarks.Net/BenchmarkTypes_Depth{1}.cs");
            File.WriteAllLines(typesFile, codeLines);
        }
    }
}
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Trashbin
{
    [Ignore("Used only to generate classes for benchmarks")]
    public class ManualResolverGenerator
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void Generate(int depth)
        {
            var (_, typeNames) = Utilities.GenerateClasses(depth);
            var manualResolverName = $"ManualResolver_Depth{depth}";
            
            var sb = new StringBuilder();

            sb.AppendLine($"public static class {manualResolverName}");
            sb.AppendLine("{");
            GenerateResolverMethods("Dependency", 1, 2, depth, sb);
            sb.AppendLine("}");
            
            var fileDirectory = Path.Combine(Utilities.GetRootFolder(), "SparseInject.Benchmarks.Net/ManualResolvers")
                .Replace("\\", "/");
            var typesFile = $"{fileDirectory}/{manualResolverName}.cs";

            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }
            
            File.WriteAllText(typesFile, sb.ToString());
        }

        private static void GenerateResolverMethods(string className, int currentLevel, int dependencies, int maxDepth,
            StringBuilder sb)
        {
            var finalClassName = className + $"_Depth{maxDepth}";
            
            sb.AppendLine($"    public static {finalClassName} Create{className}()");
            sb.AppendLine("    {");

            if (currentLevel < maxDepth)
            {
                var dependencyCalls = new List<string>();
                for (int i = 1; i <= dependencies; i++)
                {
                    string depClass = $"{className}D{i}";
                    dependencyCalls.Add($"Create{depClass}()");
                }

                string args = string.Join(", ", dependencyCalls);
                sb.AppendLine($"        return new {finalClassName}({args});");
            }
            else
            {

                sb.AppendLine($"        return new {finalClassName}();");
            }

            sb.AppendLine("    }");
            sb.AppendLine();

            if (currentLevel < maxDepth)
            {
                for (int i = 1; i <= dependencies; i++)
                {
                    string dependentClass = $"{className}D{i}";

                    GenerateResolverMethods(dependentClass, currentLevel + 1, Utilities.GetNextDependenciesCount(dependencies), maxDepth, sb);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Trashbin
{
    public class Utilities
    {
        public static (string source, List<string> typeNames) GenerateClasses(int depth)
        {
            var sb = new StringBuilder();
            var typeNames = new List<string>();

            if (depth < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(depth));
            }

            GenerateClass("Dependency", 1, 2, depth, sb, typeNames);
        
            return (sb.ToString(), typeNames);
        }
    
        private static void GenerateClass(string className, int currentLevel, int dependencies, int maxDepth, StringBuilder sb, List<string> typeNames)
        {
            var finalClassName = className + $"_Depth{maxDepth}";
            
            typeNames.Add(finalClassName);
        
            sb.AppendLine($"public class {finalClassName}");
            sb.AppendLine("{");

            if (currentLevel < maxDepth)
            {
                for (int i = 1; i <= dependencies; i++)
                {
                    string dependentClass = $"{className}D{i}_Depth{maxDepth}";
                    sb.AppendLine($"    private readonly {dependentClass} _d{i};");
                }

                sb.AppendLine();
                sb.Append($"    public {finalClassName}(");

                for (int i = 1; i <= dependencies; i++)
                {
                    string dependentClass = $"{className}D{i}_Depth{maxDepth}";
                    sb.Append($"{dependentClass} d{i}");
                    if (i != dependencies) sb.Append(", ");
                }

                sb.AppendLine(")");
                sb.AppendLine("    {");

                for (int i = 1; i <= dependencies; i++)
                {
                    sb.AppendLine($"        _d{i} = d{i};");
                }

                sb.AppendLine("    }");
            }
            else
            {
                sb.AppendLine($"    public {finalClassName}() {{ }}");
            }

            sb.AppendLine("}");
            sb.AppendLine();

            if (currentLevel < maxDepth)
            {
                for (int i = 1; i <= dependencies; i++)
                {
                    string dependentClass = $"{className}D{i}";
                    GenerateClass(dependentClass, currentLevel + 1, GetNextDependenciesCount(dependencies), maxDepth, sb, typeNames);
                }
            }
        }

        public static int GetNextDependenciesCount(int current)
        {
            return current + 3;
        }
        
        public static string GetRootFolder()
        {
            return string.Join("/", Directory.GetCurrentDirectory().Replace('\\', '/').Split("/").TakeWhile(path => path != "SparseInject.Tests"));
        }
    }
}
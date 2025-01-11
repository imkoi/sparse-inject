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

            if (depth > 0)
            {
                GenerateClass("Class0", 1, 2, depth, sb, typeNames);
            }
            else
            {
                sb.AppendLine("public class Class0_Depth0 { }");
                typeNames.Add("Class0_Depth0");
            }
        
            return (sb.ToString(), typeNames);
        }
    
        private static void GenerateClass(string className, int currentLevel, int dependencies, int maxDepth, StringBuilder sb, List<string> typeNames)
        {
            typeNames.Add(className);
        
            sb.AppendLine($"public class {className}_Depth{currentLevel}");
            sb.AppendLine("{");

            if (currentLevel < maxDepth)
            {
                for (int i = 1; i <= dependencies; i++)
                {
                    string dependentClass = $"{className}Dep{i}";
                    sb.AppendLine($"    private readonly {dependentClass} _{dependentClass.ToLower()};");
                }

                sb.AppendLine();
                sb.Append($"    public {className}(");

                for (int i = 1; i <= dependencies; i++)
                {
                    string dependentClass = $"{className}Dep{i}";
                    sb.Append($"{dependentClass} {dependentClass.ToLower()}");
                    if (i != dependencies) sb.Append(", ");
                }

                sb.AppendLine(")");
                sb.AppendLine("    {");

                for (int i = 1; i <= dependencies; i++)
                {
                    string dependentClass = $"{className}Dep{i}";
                    sb.AppendLine($"        _{dependentClass.ToLower()} = {dependentClass.ToLower()};");
                }

                sb.AppendLine("    }");
            }
            else
            {
                sb.AppendLine($"    public {className}() {{ }}");
            }

            sb.AppendLine("}");
            sb.AppendLine();

            if (currentLevel < maxDepth)
            {
                for (int i = 1; i <= dependencies; i++)
                {
                    string dependentClass = $"{className}Dep{i}";
                    GenerateClass(dependentClass, currentLevel + 1, dependencies * 2, maxDepth, sb, typeNames);
                }
            }
        }
        
        public static string GetRootFolder()
        {
            return string.Join("/", Directory.GetCurrentDirectory().Replace('\\', '/').Split("/").TakeWhile(path => path != "SparseInject.Tests"));
        }
    }
}
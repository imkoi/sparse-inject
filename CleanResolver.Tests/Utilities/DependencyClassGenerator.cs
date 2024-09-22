using System.Text;
using NUnit.Framework;

namespace CleanResolver.Tests.Utilities;

public class DependencyClassGenerator
{
    [Test]
    [TestCase(4)]
    [TestCase(5)]
    [TestCase(6)]
    public void GenerateDependencies(int depth)
    {
        var (generatedCode, typeNames) = GenerateClasses(depth);

        var codeLines = generatedCode.Split("\n");

        for (int i = 0; i < codeLines.Length; i++)
        {
            codeLines[i] = codeLines[i].Replace("\n", "").Replace("\r", "");
        }
        
        File.WriteAllLines("C:/github/sharpdate/Resolver/CleanResolver.Tests/TestSources/GeneratedDependencies.cs", codeLines);


        var sb = new StringBuilder();

        sb.AppendLine("using CleanResolver;");
        sb.AppendLine();
        sb.AppendLine("public static class ContainerBinder");
        sb.AppendLine("{");
        sb.AppendLine("    public static void BindDeps(Container container)");
        sb.AppendLine("    {");
        foreach (var typeName in typeNames)
        {
            sb.AppendLine($"        container.Register<{typeName}>();");
        }
        sb.AppendLine("    }");
        sb.AppendLine("}");

        var binder = sb.ToString().Split("\n");
        
        for (int i = 0; i < binder.Length; i++)
        {
            binder[i] = binder[i].Replace("\n", "").Replace("\r", "");
        }
        
        File.WriteAllLines("C:/github/sharpdate/Resolver/CleanResolver.Tests/TestSources/ContainerBinder.cs", binder);
    }
    
    public static (string source, List<string> typeNames) GenerateClasses(int depth)
    {
        var sb = new StringBuilder();
        var typeNames = new List<string>();
        
        // Начинаем с класса Class0, 2 зависимости
        GenerateClass("Class0", 1, 2, depth, sb, typeNames);
        
        return (sb.ToString(), typeNames);
    }
    
    private static void GenerateClass(string className, int currentLevel, int dependencies, int maxDepth, StringBuilder sb, List<string> typeNames)
    {
        typeNames.Add(className);
        
        sb.AppendLine($"public class {className}");
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
}
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Utilities
{
    public class NativeResolverGenerator
    {
        [Ignore("dont run in unit test")]
        [Test]
        [TestCase(6)]
        public void GenerateNativeResolver(int depth)
        {
            var (generatedCode, typeNames) = GenerateClasses(depth);

            var codeLines = generatedCode.Split("\n");

            for (int i = 0; i < codeLines.Length; i++)
            {
                codeLines[i] = codeLines[i].Replace("\n", "").Replace("\r", "");
            }
        
            File.WriteAllLines($"C:/github/sparseinject/SparseInject.Tests/TestSources/GeneratedDependencies.cs", codeLines);


            var sb = new StringBuilder();

            sb.AppendLine("using SparseInject;");
            sb.AppendLine();
            sb.AppendLine("public static class ContainerBinder");
            sb.AppendLine("{");
            sb.AppendLine("    public static void BindDeps(ContainerBuilder container)");
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
        
            File.WriteAllLines($"C:/github/sparseinject/SparseInject.Tests/TestSources/ContainerBinder.cs", binder);
        }
    
        public static (string source, List<string> typeNames) GenerateClasses(int depth)
        {
            var sb = new StringBuilder();
            var typeNames = new List<string>();

            //GenerateCreationMethods("Class0", 1, 2, depth, sb, typeNames);
        
            return (sb.ToString(), typeNames);
        }
        
        private static void GenerateCreationMethods(string className, int currentLevel, int dependencies, int maxDepth, StringBuilder sb)
        {
            // Генерируем метод для создания экземпляра текущего класса
            sb.AppendLine($"    public static {className} Create{className}()");
            sb.AppendLine("    {");

            if (currentLevel < maxDepth)
            {
                // Создаем зависимости
                for (int i = 1; i <= dependencies; i++)
                {
                    string dependentClass = $"{className}Dep{i}";
                    sb.AppendLine($"        var dep{i} = Create{dependentClass}();");
                }

                // Создаем экземпляр текущего класса
                sb.Append($"        return new {className}(");
                for (int i = 1; i <= dependencies; i++)
                {
                    sb.Append($"dep{i}");
                    if (i != dependencies) sb.Append(", ");
                }
                sb.AppendLine(");");
            }
            else
            {
                // Если достигнута максимальная глубина, создаем экземпляр без зависимостей
                sb.AppendLine($"        return new {className}();");
            }

            sb.AppendLine("    }");
            sb.AppendLine();

            // Рекурсивно генерируем методы для зависимостей
            if (currentLevel < maxDepth)
            {
                for (int i = 1; i <= dependencies; i++)
                {
                    string dependentClass = $"{className}Dep{i}";
                    GenerateCreationMethods(dependentClass, currentLevel + 1, dependencies + 3, maxDepth, sb);
                }
            }
        }
    }
}
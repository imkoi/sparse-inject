using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Trashbin
{
    public class NativeResolverGenerator
    {
        [Ignore("Not need for tests")]
        [Test]
        [TestCase(6)]
        public void GenerateNativeResolver(int depth)
        {
            string resolverCode = GenerateResolverClass(depth);
            File.WriteAllText("C:/github/sparseinject/SparseInject.Benchmarks.Net/NativeResolver.cs", resolverCode);
        }

        public static string GenerateResolverClass(int maxDepth)
        {
            var sb = new StringBuilder();

            sb.AppendLine("public static class NativeResolver");
            sb.AppendLine("{");
            // Начинаем генерацию методов, начиная с Class0
            GenerateResolverMethods("Class0", 1, 2, maxDepth, sb);
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static void GenerateResolverMethods(string className, int currentLevel, int dependencies, int maxDepth,
            StringBuilder sb)
        {
            sb.AppendLine("    [MethodImpl(MethodImplOptions.AggressiveInlining)]");
            sb.AppendLine($"    public static {className} Create{className}()");
            sb.AppendLine("    {");

            if (currentLevel < maxDepth)
            {
                // Генерируем вызовы создания зависимостей
                var dependencyCalls = new List<string>();
                for (int i = 1; i <= dependencies; i++)
                {
                    string depClass = $"{className}Dep{i}";
                    dependencyCalls.Add($"Create{depClass}()");
                }

                // Объединяем вызовы в аргументы конструктора
                string args = string.Join(", ", dependencyCalls);
                sb.AppendLine($"        return new {className}({args});");
            }
            else
            {
                // Если достигли максимальной глубины, создаём экземпляр без зависимостей
                sb.AppendLine($"        return new {className}();");
            }

            sb.AppendLine("    }");
            sb.AppendLine();

            // Рекурсивно генерируем методы для зависимостей текущего класса
            if (currentLevel < maxDepth)
            {
                for (int i = 1; i <= dependencies; i++)
                {
                    string dependentClass = $"{className}Dep{i}";
                    // Для каждого зависимого класса увеличиваем уровень и число зависимостей
                    GenerateResolverMethods(dependentClass, currentLevel + 1, dependencies + 3, maxDepth, sb);
                }
            }
        }
    }
}
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Trashbin
{
    [Ignore("Used only to generate classes for benchmarks")]
    public class BenchmarkRegistratorGenerator
    {
        public enum BenchmarkType
        {
            SparseInject,
            VContainer,
            Autofac,
            LightInject,
            Reflex,
            Zenject,
        }
        
        public enum DefineType
        {
            None,
            DotNetOnly,
            UnityOnly
        }

        private static readonly Dictionary<BenchmarkType, string> _usingMap =
            new Dictionary<BenchmarkType, string>
            {
                { BenchmarkType.SparseInject, "using SparseInject;" },
                { BenchmarkType.VContainer, "using VContainer;" },
                { BenchmarkType.Autofac, "using Autofac;" },
                { BenchmarkType.LightInject, "using LightInject;" },
                { BenchmarkType.Reflex, "using Reflex.Core;" },
                { BenchmarkType.Zenject, "using Zenject;" },
            };

        private static readonly Dictionary<BenchmarkType, string> _builderTypeMap =
            new Dictionary<BenchmarkType, string>
            {
                { BenchmarkType.SparseInject, "ContainerBuilder" },
                { BenchmarkType.VContainer, "ContainerBuilder" },
                { BenchmarkType.Autofac, "ContainerBuilder" },
                { BenchmarkType.LightInject, "ServiceContainer" },
                { BenchmarkType.Reflex, "ContainerBuilder" },
                { BenchmarkType.Zenject, "DiContainer" },
            };

        private static readonly Dictionary<BenchmarkType, string> _registerTransientFormatMap =
            new Dictionary<BenchmarkType, string>
            {
                { BenchmarkType.SparseInject, "builder.Register<{0}>()" },
                { BenchmarkType.VContainer, "builder.Register(typeof({0}), Lifetime.Transient)" },
                { BenchmarkType.Autofac, "builder.RegisterType<{0}>()" },
                { BenchmarkType.LightInject, "builder.Register<{0}>()" },
                { BenchmarkType.Reflex, "builder.AddTransient(typeof({0}))" },
                { BenchmarkType.Zenject, "builder.Bind<{0}>().AsTransient()" },
            };

        private static readonly Dictionary<BenchmarkType, string> _registerSingletonFormatMap =
            new Dictionary<BenchmarkType, string>
            {
                { BenchmarkType.SparseInject, "builder.Register<{0}>(Lifetime.Singleton)" },
                { BenchmarkType.VContainer, "builder.Register(typeof({0}), Lifetime.Singleton)" },
                { BenchmarkType.Autofac, "builder.RegisterType<{0}>().SingleInstance()" },
                { BenchmarkType.LightInject, "builder.RegisterSingleton<{0}>()" },
                { BenchmarkType.Reflex, "builder.AddSingleton(typeof({0}))" },
                { BenchmarkType.Zenject, "builder.Bind<{0}>().AsSingle()" },
            };

        private static readonly Dictionary<BenchmarkType, DefineType> _defineMap =
            new Dictionary<BenchmarkType, DefineType>
            {
                { BenchmarkType.SparseInject, DefineType.None },
                { BenchmarkType.VContainer, DefineType.None },
                { BenchmarkType.Autofac, DefineType.DotNetOnly },
                { BenchmarkType.LightInject, DefineType.DotNetOnly },
                { BenchmarkType.Reflex, DefineType.UnityOnly },
                { BenchmarkType.Zenject, DefineType.UnityOnly },
            };
        
        [TestCase(1, BenchmarkType.SparseInject)]
        [TestCase(2, BenchmarkType.SparseInject)]
        [TestCase(3, BenchmarkType.SparseInject)]
        [TestCase(4, BenchmarkType.SparseInject)]
        [TestCase(5, BenchmarkType.SparseInject)]
        [TestCase(6, BenchmarkType.SparseInject)]
        
        [TestCase(1, BenchmarkType.VContainer)]
        [TestCase(2, BenchmarkType.VContainer)]
        [TestCase(3, BenchmarkType.VContainer)]
        [TestCase(4, BenchmarkType.VContainer)]
        [TestCase(5, BenchmarkType.VContainer)]
        [TestCase(6, BenchmarkType.VContainer)]
        
        [TestCase(1, BenchmarkType.Autofac)]
        [TestCase(2, BenchmarkType.Autofac)]
        [TestCase(3, BenchmarkType.Autofac)]
        [TestCase(4, BenchmarkType.Autofac)]
        [TestCase(5, BenchmarkType.Autofac)]
        [TestCase(6, BenchmarkType.Autofac)]
        
        [TestCase(1, BenchmarkType.LightInject)]
        [TestCase(2, BenchmarkType.LightInject)]
        [TestCase(3, BenchmarkType.LightInject)]
        [TestCase(4, BenchmarkType.LightInject)]
        [TestCase(5, BenchmarkType.LightInject)]
        [TestCase(6, BenchmarkType.LightInject)]
        
        [TestCase(1, BenchmarkType.Reflex)]
        [TestCase(2, BenchmarkType.Reflex)]
        [TestCase(3, BenchmarkType.Reflex)]
        [TestCase(4, BenchmarkType.Reflex)]
        [TestCase(5, BenchmarkType.Reflex)]
        [TestCase(6, BenchmarkType.Reflex)]
        
        [TestCase(1, BenchmarkType.Zenject)]
        [TestCase(2, BenchmarkType.Zenject)]
        [TestCase(3, BenchmarkType.Zenject)]
        [TestCase(4, BenchmarkType.Zenject)]
        [TestCase(5, BenchmarkType.Zenject)]
        [TestCase(6, BenchmarkType.Zenject)]
        public void TransientGenerate(int depth, BenchmarkType benchmarkType)
        {
            var fileName = $"{benchmarkType}TransientRegistrator_Depth{depth}";
            var (_, typeNames) = Utilities.GenerateClasses(depth);
            
            GenerateBenchmarkRegistrator(fileName, typeNames, benchmarkType,
                _usingMap, _builderTypeMap, _registerTransientFormatMap);
        }
        
        [TestCase(1, BenchmarkType.SparseInject)]
        [TestCase(2, BenchmarkType.SparseInject)]
        [TestCase(3, BenchmarkType.SparseInject)]
        [TestCase(4, BenchmarkType.SparseInject)]
        [TestCase(5, BenchmarkType.SparseInject)]
        [TestCase(6, BenchmarkType.SparseInject)]
        
        [TestCase(1, BenchmarkType.VContainer)]
        [TestCase(2, BenchmarkType.VContainer)]
        [TestCase(3, BenchmarkType.VContainer)]
        [TestCase(4, BenchmarkType.VContainer)]
        [TestCase(5, BenchmarkType.VContainer)]
        [TestCase(6, BenchmarkType.VContainer)]
        
        [TestCase(1, BenchmarkType.Autofac)]
        [TestCase(2, BenchmarkType.Autofac)]
        [TestCase(3, BenchmarkType.Autofac)]
        [TestCase(4, BenchmarkType.Autofac)]
        [TestCase(5, BenchmarkType.Autofac)]
        [TestCase(6, BenchmarkType.Autofac)]
        
        [TestCase(1, BenchmarkType.LightInject)]
        [TestCase(2, BenchmarkType.LightInject)]
        [TestCase(3, BenchmarkType.LightInject)]
        [TestCase(4, BenchmarkType.LightInject)]
        [TestCase(5, BenchmarkType.LightInject)]
        [TestCase(6, BenchmarkType.LightInject)]
        
        [TestCase(1, BenchmarkType.Reflex)]
        [TestCase(2, BenchmarkType.Reflex)]
        [TestCase(3, BenchmarkType.Reflex)]
        [TestCase(4, BenchmarkType.Reflex)]
        [TestCase(5, BenchmarkType.Reflex)]
        [TestCase(6, BenchmarkType.Reflex)]
        
        [TestCase(1, BenchmarkType.Zenject)]
        [TestCase(2, BenchmarkType.Zenject)]
        [TestCase(3, BenchmarkType.Zenject)]
        [TestCase(4, BenchmarkType.Zenject)]
        [TestCase(5, BenchmarkType.Zenject)]
        [TestCase(6, BenchmarkType.Zenject)]
        public void SingletonGenerate(int depth, BenchmarkType benchmarkType)
        {
            var fileName = $"{benchmarkType}SingletonRegistrator_Depth{depth}";
            var (_, typeNames) = Utilities.GenerateClasses(depth);
            
            GenerateBenchmarkRegistrator(fileName, typeNames, benchmarkType,
                _usingMap, _builderTypeMap, _registerSingletonFormatMap);
        }

        private void GenerateBenchmarkRegistrator(
            string fileName,
            List<string> types,
            BenchmarkType benchmarkType, 
            Dictionary<BenchmarkType, string> usingMap,
            Dictionary<BenchmarkType, string> builderTypeMap,
            Dictionary<BenchmarkType, string> registerFormatMap)
        {
            var sb = new StringBuilder();
            
            if (benchmarkType == BenchmarkType.VContainer)
            {
                sb.AppendLine("#if UNITY_2017_1_OR_NEWER");
                var registerMethod = registerFormatMap == _registerTransientFormatMap
                    ? "builder.Register<{0}>(Lifetime.Transient)"
                    : "builder.Register<{0}>(Lifetime.Singleton)";
                
                AddBenchmarkRegistrator(fileName, types, sb, benchmarkType, usingMap, builderTypeMap, 
                    new Dictionary<BenchmarkType, string>
                    {
                        { BenchmarkType.VContainer, registerMethod },
                    });
                sb.AppendLine("#endif");
                
                sb.AppendLine("#if NET");
                AddBenchmarkRegistrator(fileName, types, sb, benchmarkType, usingMap, builderTypeMap, registerFormatMap);
                sb.AppendLine("#endif");
            }
            else
            {
                var defineSymbol = string.Empty;

                switch (_defineMap[benchmarkType])
                {
                    case DefineType.UnityOnly:
                        defineSymbol = "UNITY_2017_1_OR_NEWER";
                        break;
                    case DefineType.DotNetOnly:
                        defineSymbol = "NET";
                        break;
                }

                if (!string.IsNullOrEmpty(defineSymbol))
                {
                    sb.AppendLine($"#if {defineSymbol}");
                }
            
                AddBenchmarkRegistrator(fileName, types, sb, benchmarkType, usingMap, builderTypeMap, registerFormatMap);
            
                if (!string.IsNullOrEmpty(defineSymbol))
                {
                    sb.AppendLine("#endif");
                }
            }

            var binder = sb.ToString().Split("\n");
        
            for (var i = 0; i < binder.Length; i++)
            {
                binder[i] = binder[i].Replace("\n", "").Replace("\r", "");
            }
            
            var fileDirectory = Path.Combine(Utilities.GetRootFolder(), "SparseInject.Benchmarks.Net/Registrators")
                .Replace("\\", "/");
            var typesFile = $"{fileDirectory}/{fileName}.cs";

            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }

            File.WriteAllLines(typesFile, binder);
        }
        
        private void AddBenchmarkRegistrator(
            string fileName,
            List<string> types,
            StringBuilder sb,
            BenchmarkType benchmarkType, 
            Dictionary<BenchmarkType, string> usingMap,
            Dictionary<BenchmarkType, string> builderTypeMap,
            Dictionary<BenchmarkType, string> registerFormatMap)
        {
            sb.AppendLine(usingMap[benchmarkType]);
            sb.AppendLine();
            sb.AppendLine($"public static class {fileName}");
            sb.AppendLine("{");
            sb.AppendLine($"    public static void Register({builderTypeMap[benchmarkType]} builder)");
            sb.AppendLine("    {");
            foreach (var typeName in types)
            {
                sb.AppendLine($"        {string.Format(registerFormatMap[benchmarkType], typeName)};");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");
        }
    }
}
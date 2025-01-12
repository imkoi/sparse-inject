using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Trashbin
{
    //[Ignore("Used only to generate classes for benchmarks")]
    public class BenchmarkReplaceNames
    {
        [TestCase(1, "Depth_1")]
        [TestCase(2, "Depth_2")]
        [TestCase(3, "Depth_3")]
        [TestCase(4, "Depth_4")]
        [TestCase(5, "Depth_5")]
        [TestCase(6, "Depth_6")]
        public void TransientGenerate(int depth, string folderName)
        {
            var path = $"C:/github/sparseinject/SparseInject.Benchmarks.Net/Scenarios/Transient/{folderName}";

            var files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories)
                .Select(f => f.Replace("\\", "/"))
                .ToArray();

            foreach (var file in files)
            {
                var split = file.Split('/');
                var fileName = split.Last();

                var currentIndex = depth;
                var suspectDepth = $"_Depth{currentIndex}";
                
                while (!fileName.Contains(suspectDepth) && currentIndex > 0)
                {
                    currentIndex--;
                    suspectDepth = $"_Depth{currentIndex}";
                }

                if (currentIndex > 0)
                {
                    fileName = fileName.Replace($"_Depth{currentIndex}", $"_Depth{depth}");
                    split[split.Length - 1] = fileName; 
                
                    var newName = string.Join("/", split);

                    if (file != newName)
                    {
                        File.Move(file, newName);
                    }
                }
            }
        }
        
        [Test]
        public void AddDefineToScenarios()
        {
            var path = "C:/github/sparseinject/SparseInject.Benchmarks.Net/Scenarios/";

            var files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories)
                .Select(f => f.Replace("\\", "/"))
                .ToArray();

            foreach (var file in files)
            {
                var defineType = BenchmarkRegistratorGenerator.DefineType.None;
                
                var filesName = file.Split('/').Last();
                
                if (filesName.Contains("Autofac") || filesName.Contains("LightInject"))
                {
                    defineType = BenchmarkRegistratorGenerator.DefineType.DotNetOnly;
                }
                else if (filesName.Contains("Zenject") && filesName.Contains("Reflex"))
                {
                    defineType = BenchmarkRegistratorGenerator.DefineType.UnityOnly;
                }
                
                var lines = File.ReadAllLines(file).ToList();
                
                switch (defineType)
                {
                    case BenchmarkRegistratorGenerator.DefineType.UnityOnly:
                        lines.Insert(0, "#if UNITY_2017_1_OR_NEWER");
                        lines.Add("#endif");
                        break;
                    case BenchmarkRegistratorGenerator.DefineType.DotNetOnly:
                        lines.Insert(0, "#if NET");
                        lines.Add("#endif");
                        break;
                }

                if (defineType != BenchmarkRegistratorGenerator.DefineType.None)
                {
                    File.WriteAllLines(file, lines);
                }
            }
        }
    }
}
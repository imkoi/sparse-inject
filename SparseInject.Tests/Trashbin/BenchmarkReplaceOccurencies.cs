using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Trashbin
{
    [Ignore("Used only to generate classes for benchmarks")]
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
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

public class CiCdPushPatchTest
{
    private static readonly HashSet<string> IgnoredFiles = new HashSet<string>()
    {
        "SparseInject.csproj",
        "AssemblyInfo.cs"
    };
    
    private static readonly HashSet<string> IgnoredFolders = new HashSet<string>()
    {
        "bin",
        "obj"
    };
    
    [Test]
    public void BeforePatchStep_SourceFilesToCopy_ContainMetaFilesInsideUnityProject()
    {
        var currentDirectory = string.Join("/", Directory.GetCurrentDirectory().Replace('\\', '/').Split("/").TakeWhile(path => path != "SparseInject.Tests"));

        var dotNetProjectFolder = Path.Combine(currentDirectory, "SparseInject").Replace("\\", "/");
        var unityProjectFolder = Path.Combine(currentDirectory, "SparseInject.Unity/Assets/Runtime/Core").Replace("\\", "/");
        var ignoredFolders = IgnoredFolders.Select(folderName => Path.Combine(dotNetProjectFolder, folderName).Replace("\\", "/"));
        
        Directory.Exists(dotNetProjectFolder).Should().BeTrue();
        Directory.Exists(unityProjectFolder).Should().BeTrue();
        
        var dotNetFiles = Directory.GetFiles(dotNetProjectFolder, "*.cs", SearchOption.AllDirectories)
            .Select(file => file.Replace("\\", "/"))
            .Where(file => !IgnoredFiles.Contains(file.Split("/").Last()))
            .Where(file => !ignoredFolders.Any(file.StartsWith))
            .ToArray();

        var requestedUnityMetaFiles = dotNetFiles
            .Select(file => file.Replace(dotNetProjectFolder, unityProjectFolder))
            .Select(file => file.Replace(".cs", ".cs.meta"))
            .ToArray();

        foreach (var requestedUnityMetaFile in requestedUnityMetaFiles)
        {
            File.Exists(requestedUnityMetaFile).Should().BeTrue($"Requested '{requestedUnityMetaFile}' file not inside unity project");
        }
    }
    
    [Test]
    public void BeforePatchStep_SourceGeneratorsToCopy_ContainMetaFilesInsideUnityProject()
    {
        var sourceGeneratorDllSubPaths = new string[]
        {
            "3.8.0/SparseInject.SourceGenerator3.8.0.dll",
            "4.3.0/SparseInject.SourceGenerator4.3.0.dll"
        };
        
        var currentDirectory = string.Join("/", Directory.GetCurrentDirectory().Replace('\\', '/').Split("/").TakeWhile(path => path != "SparseInject.Tests"));
        
        Console.WriteLine($"Main directory path: {currentDirectory}");
        
        var sourceGeneratorDllPaths = sourceGeneratorDllSubPaths
            .Select(subPath => Path
                .Combine(currentDirectory, Path.Combine("SparseInject.SourceGenerator/bin/Release/netstandard2.0", subPath))
                .Replace("\\", "/"))
            .ToArray();
        var unityProjectFolder = Path
            .Combine(currentDirectory, "SparseInject.Unity/Assets/Editor/DisabledSourceGenerators~")
            .Replace("\\", "/");

        foreach (var sourceGeneratorDllPath in sourceGeneratorDllPaths)
        {
            File.Exists(sourceGeneratorDllPath).Should().BeTrue($"File '{sourceGeneratorDllPath}' does not exist' ");
        }
        
        Directory.Exists(unityProjectFolder).Should().BeTrue();

        foreach (var sourceGeneratorDllPath in sourceGeneratorDllPaths)
        {
            var splitPath = sourceGeneratorDllPath.Split("/");
            var replacedPath = string.Join("/", splitPath.Take(splitPath.Length - 1));
            
            var requestedUnityMetaFile = sourceGeneratorDllPath.Replace(replacedPath, unityProjectFolder).Replace(".dll", ".dll.meta");

            File.Exists(requestedUnityMetaFile).Should().BeTrue($"Requested '{requestedUnityMetaFile}' file not inside unity project");
        }
    }
}
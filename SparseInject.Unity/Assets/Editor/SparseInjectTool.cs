#pragma warning disable CS0162 // Unreachable code detected
#if UNITY_2021_2_OR_NEWER
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class SparseInjectTool
{
    private const string StartPackageName = "com.voxcake.sparseinject@";
    
    private const string EnableReflectionBakingName = "SparseInject/Enable Reflection Baking";
    private const string DisableReflectionBakingName = "SparseInject/Disable Reflection Baking";

    [RuntimeInitializeOnLoadMethod]
    private static void UpdateCheckboxes()
    {
        var isReflectionBakingEnabled = IsSourceGeneratorsEnabled();
        
        Menu.SetChecked(EnableReflectionBakingName, isReflectionBakingEnabled);
        Menu.SetChecked(DisableReflectionBakingName, !isReflectionBakingEnabled);
    }
         
    [MenuItem(EnableReflectionBakingName)]
    private static void EnableReflectionBaking()
    {
        if (IsSourceGeneratorsEnabled())
        {
            Menu.SetChecked(EnableReflectionBakingName, true);
            Menu.SetChecked(DisableReflectionBakingName, false);
            
            AssetDatabase.Refresh();
            
            return;
        }
        
        var packageFolder = GetPackageFolderPath();
        
        if (string.IsNullOrEmpty(packageFolder))
        {
            throw new DirectoryNotFoundException("SparseInject package directory could not be found.");
        }
        
        var enabledSourceGeneratorsFolder = Path
            .Combine(packageFolder, "Editor/EnabledSourceGenerators")
            .Replace("\\", "/");
        var disabledSourceGeneratorsFolder = Path
            .Combine(packageFolder, "Editor/DisabledSourceGenerators~")
            .Replace("\\", "/");

        if (!Directory.Exists(enabledSourceGeneratorsFolder))
        {
            Directory.CreateDirectory(enabledSourceGeneratorsFolder);
        }
        
        var sourceGeneratorFiles = Directory.GetFiles(disabledSourceGeneratorsFolder);

        foreach (var sourceGeneratorFile in sourceGeneratorFiles)
        {
            var targetDestination = sourceGeneratorFile.Replace(disabledSourceGeneratorsFolder, enabledSourceGeneratorsFolder);
            
            File.Move(sourceGeneratorFile, targetDestination);
        }
        
        AssetDatabase.Refresh();
        
        UpdateCheckboxes();
    }
 
    [MenuItem(DisableReflectionBakingName)]
    private static void DisableReflectionBaking()
    {
        if (!IsSourceGeneratorsEnabled())
        {
            Menu.SetChecked(EnableReflectionBakingName, false);
            Menu.SetChecked(DisableReflectionBakingName, true);
            
            AssetDatabase.Refresh();
            
            return;
        }
        
        var packageFolder = GetPackageFolderPath();
        
        if (string.IsNullOrEmpty(packageFolder))
        {
            throw new DirectoryNotFoundException("SparseInject package directory could not be found.");
        }
        
        var enabledSourceGeneratorsFolder = Path
            .Combine(packageFolder, "Editor/EnabledSourceGenerators")
            .Replace("\\", "/");
        var disabledSourceGeneratorsFolder = Path
            .Combine(packageFolder, "Editor/DisabledSourceGenerators~")
            .Replace("\\", "/");
        
        if (!Directory.Exists(enabledSourceGeneratorsFolder))
        {
            Directory.CreateDirectory(enabledSourceGeneratorsFolder);
        }
        
        var sourceGeneratorFiles = Directory.GetFiles(enabledSourceGeneratorsFolder);

        foreach (var sourceGeneratorFile in sourceGeneratorFiles)
        {
            var targetDestination = sourceGeneratorFile.Replace(enabledSourceGeneratorsFolder, disabledSourceGeneratorsFolder);
            
            File.Move(sourceGeneratorFile, targetDestination);
        }
        
        AssetDatabase.Refresh();
        
        UpdateCheckboxes();
    }

    private static bool IsSourceGeneratorsEnabled()
    {
        var packageFolder = GetPackageFolderPath();
        
        if (string.IsNullOrEmpty(packageFolder))
        {
            return false;
        }
        
        var enabledSourceGeneratorsFolder = Path
            .Combine(packageFolder, "Editor/EnabledSourceGenerators")
            .Replace("\\", "/");

        if (!Directory.Exists(enabledSourceGeneratorsFolder))
        {
            return false;
        }
        
        var files = Directory.GetFiles(enabledSourceGeneratorsFolder, "*.dll", SearchOption.AllDirectories);

        return files.Any();
    }

    private static string GetPackageFolderPath()
    {
        var path = Application.dataPath.Replace("\\", "/").Split('/');
        
        path[path.Length - 1] = "Packages";
        var packagesPath = string.Join("/", path);
        
        path[path.Length - 1] = "Library/PackageCache";
        var packagesCachePath = string.Join("/", path);

        var searchFolders = new string[] { packagesPath, packagesCachePath };

        foreach (var rootFolder in searchFolders)
        {
            var packageFolder = Directory.GetDirectories(rootFolder)
                .Select(folder => folder.Replace("\\", "/"))
                .FirstOrDefault(folder => folder.Split('/').Last().StartsWith(StartPackageName));

            if (!string.IsNullOrEmpty(packageFolder))
            {
                return packageFolder;
            }
        }

        return Application.dataPath;
    }
}
#endif

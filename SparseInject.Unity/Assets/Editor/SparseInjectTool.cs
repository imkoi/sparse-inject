using UnityEditor;
using UnityEngine;

public static class SparseInjectTool
{
    private const string EnableReflectionBakingName = "SparseInject/Enable Reflection Baking";
    private const string DisableReflectionBakingName = "SparseInject/Disable Reflection Baking";

    [RuntimeInitializeOnLoadMethod]
    private static void UpdateCheckboxes()
    {
        
    }
         
    [MenuItem(EnableReflectionBakingName)]
    private static void EnableReflectionBaking()
    {
        UpdateCheckboxes();
    }
 
    [MenuItem(DisableReflectionBakingName)]
    private static void DisableReflectionBaking()
    {
        UpdateCheckboxes();
    }
}

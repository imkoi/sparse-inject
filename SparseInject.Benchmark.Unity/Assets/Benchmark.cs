using System;
using System.IO;
using System.Threading;
using SparseInject.BenchmarkFramework;
using SparseInject.Benchmarks.Net;
using UnityEngine;
using UnityEngine.UI;

public class Benchmark : MonoBehaviour
{
    [SerializeField] private Slider _progressSlider;
    
    private CancellationTokenSource _cancellationTokenSource;
    
    private void Awake()
    {
        var commandArgs = Environment.GetCommandLineArgs();
        
        var diskReportStorage = new DiskReportStorage(GetTempBenchmarkReportFile());
        var benchmarkRunner = new BenchmarkRunner(
            commandArgs,
            diskReportStorage,
            new UnityMemorySnapshotFactory(),
            new GarbageCollectorCleaner());

        benchmarkRunner.AddBenchmarkCategory("type-id-provider", new Scenario[]
        {
            new TypeIdProviderScenario(),
            new DictionaryScenario(),
        }, 10);
        
        TransientBenchmarkUtility.AddCategories(benchmarkRunner, 10);
        SingletonBenchmarkUtility.AddCategories(benchmarkRunner, 10);

        try
        {
            benchmarkRunner.Run();
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
        finally
        {
            Application.Quit();
        }
    }

    private string GetTempBenchmarkReportFile()
    {
        return Path.Combine(Application.persistentDataPath, "temp-benchmark-report.txt").Replace("\\", "/");
    }
}

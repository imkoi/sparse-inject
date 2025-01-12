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
    private BenchmarkProgress _progress;
    
    private async void Awake()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _progress = new BenchmarkProgress();
        
        _progress.Changed += ProgressChanged;

        var diskReportStorage = new DiskReportStorage(GetTempBenchmarkReportFile());
        var benchmarkRunner = new BenchmarkRunner(
            Environment.GetCommandLineArgs(),
            diskReportStorage,
            new DotNetMemorySnapshotFactory(),
            new GarbageCollectorCleaner(),
            new DotNetBenchmarkMeasurer(),
            _progress);
        
        TransientBenchmarkUtility.AddCategories(benchmarkRunner, 10);
        SingletonBenchmarkUtility.AddCategories(benchmarkRunner, 10);

        try
        {
            var benchmarkReport = await benchmarkRunner.RunAsync(_cancellationTokenSource.Token);
            
            File.WriteAllText(GetBenchmarkReportFile(), benchmarkReport.ToString());
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
        
        _progress.Changed -= ProgressChanged;
    }

    private void OnDestroy()
    {
        _progress.Changed -= ProgressChanged;
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    private void ProgressChanged(float progress)
    {
        _progressSlider.value = progress;
    }

    private string GetTempBenchmarkReportFile()
    {
        return Path.Combine(Application.persistentDataPath, "temp-benchmark-report.txt").Replace("\\", "/");
    }
    
    private string GetBenchmarkReportFile()
    {
        return Path.Combine(Application.persistentDataPath, "benchmark-report.txt").Replace("\\", "/");
    }
}

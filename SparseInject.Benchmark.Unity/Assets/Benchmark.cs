using System;
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
    
    private void Awake()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _progress = new BenchmarkProgress();
        
        _progress.Changed += ProgressChanged;
        
        var benchmarkRunner = new BenchmarkRunner(
            Environment.GetCommandLineArgs(),
            new DiskReportStorage(""),
            new DotNetMemorySnapshotFactory(),
            null,
            null,
            _progress);
        
        TransientBenchmarkUtility.AddCategories(benchmarkRunner, 10);
        SingletonBenchmarkUtility.AddCategories(benchmarkRunner, 10);
        
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
}

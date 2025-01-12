using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SparseInject.BenchmarkFramework;
using UnityEngine;

public class CsvReportGenerator : MonoBehaviour
{
    [SerializeField] private string _reportPath;
    
    private void Awake()
    {
        var reportPath = _reportPath.Replace("\\", "/");
        var samples = new List<(string category, string scenario, long time)>(10000);

        using var reader = new StreamReader(File.Open(_reportPath, FileMode.Open, FileAccess.Read, FileShare.Read));
        
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();

            if (!string.IsNullOrWhiteSpace(line))
            {
                var mainSplit = line.Split(']');
                var header = mainSplit[0].Trim('[', ']');
                var values = mainSplit[1].Trim();

                var headerSplit = header.Split("::");
                if (headerSplit.Length != 2)
                {
                    continue;
                }

                var fetchedCategoryName = headerSplit[0];
                var fetchedScenarioName = headerSplit[1];

                if (string.IsNullOrEmpty(fetchedCategoryName) || string.IsNullOrEmpty(fetchedScenarioName))
                {
                    continue;
                }

                var valuesSplit = values.Split(';');
                if (valuesSplit.Length != 2)
                {
                    continue;
                }

                var timeTicksPart = valuesSplit[0].Trim().Replace("time_ticks(", "").Replace(")", "");

                if (long.TryParse(timeTicksPart, out var timeTicks))
                {
                    samples.Add((fetchedCategoryName, fetchedScenarioName, timeTicks));
                }
            }
        }

        var categories = GetCategories(samples);
        var report = GenerateReport(categories);
        var csvContents = GetCSVContents(report);

        foreach (var csvContent in csvContents)
        {
            var reportPathSplit = reportPath.Split('/');
            reportPathSplit[reportPathSplit.Length - 1] = $"{csvContent.category}.csv";
            var csvPath = string.Join("/", reportPathSplit);
            
            File.WriteAllLines(csvPath, csvContent.content);
        }
    }

    private Dictionary<string, Dictionary<string, List<long>>> GetCategories(List<(string category, string scenario, long ticks)> samples)
    {
        var categories = new Dictionary<string, Dictionary<string, List<long>>>();

        foreach (var sample in samples)
        {
            if (!categories.TryGetValue(sample.category, out var scenario))
            {
                scenario = new Dictionary<string, List<long>>();
                categories.Add(sample.category, scenario);
            }

            if (!scenario.TryGetValue(sample.scenario, out var times))
            {
                times = new List<long>();
                scenario.Add(sample.scenario, times);
            }
            
            times.Add(sample.ticks);
        }

        return categories;
    }

    private BenchmarkReport GenerateReport(Dictionary<string, Dictionary<string, List<long>>> categories)
    {
        var categoryReports = new List<BenchmarkCategoryReport>();
                
        foreach (var categoryPair in categories)
        {
            var categoryName = categoryPair.Key;
            var scenarioReports = new List<BenchmarkScenarioReport>();
                    
            foreach (var scenarioPair in categoryPair.Value)
            {
                var samples = scenarioPair.Value.Select(value =>
                    new BenchmarkSampleReport(TimeSpan.FromTicks(value), 0))
                    .ToList();
                        
                scenarioReports.Add(new BenchmarkScenarioReport(scenarioPair.Key, samples));
            }
                    
            categoryReports.Add(new BenchmarkCategoryReport(categoryName, scenarioReports));
        }
                
        return new BenchmarkReport(categoryReports);
    }

    private List<(string category, List<string> content)> GetCSVContents(BenchmarkReport report)
    {
        var categories = new List<(string category, List<string> content)>();
    
        foreach (var categoryReport in report.CategoryReports)
        {
            var csvLines = new List<string>();

            csvLines.Add("Container, Time[ms], Average, Min, Max, Dev");

            foreach (var scenarioReport in categoryReport.ScenarioReports)
            {
                var scenarioName = scenarioReport.Name;

                var average = $"{scenarioReport.AverageDuration.TotalMilliseconds:F2}";
                var min = $"{scenarioReport.MinDuration.TotalMilliseconds:F2}";
                var max = $"{scenarioReport.MaxDuration.TotalMilliseconds:F2}";
                var dev = $"{scenarioReport.ErrorDuration.TotalMilliseconds:F2}";

                csvLines.Add($"{scenarioName}, {average}, {min}, {max}, {dev}");
            }

            categories.Add((categoryReport.Name, csvLines));
        }

        return categories;
    }
}
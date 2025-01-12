using SparseInject.BenchmarkFramework;

namespace SparseInject.Benchmarks.Net
{
    public class SingletonBenchmarkUtility
    {
        public static void AddCategories(BenchmarkRunner benchmarkRunner, int samples)
        {
            AddBenchmarkCategoryDepth1(benchmarkRunner, samples);
            AddBenchmarkCategoryDepth2(benchmarkRunner, samples);
            AddBenchmarkCategoryDepth3(benchmarkRunner, samples);
            AddBenchmarkCategoryDepth4(benchmarkRunner, samples);
            AddBenchmarkCategoryDepth5(benchmarkRunner, samples);
            AddBenchmarkCategoryDepth6(benchmarkRunner, samples);
        }

        private static void AddBenchmarkCategoryDepth1(BenchmarkRunner benchmarkRunner, int samples)
        {
            benchmarkRunner.AddBenchmarkCategory("singleton-register-depth1", new Scenario[]
            {
                new SparseInjectSingletonRegister_Depth1Scenario(),
                new VContainerSingletonRegister_Depth1Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-build-depth1", new Scenario[]
            {
                new SparseInjectSingletonBuild_Depth1Scenario(),
                new VContainerSingletonBuild_Depth1Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-register-and-build-depth1", new Scenario[]
            {
                new SparseInjectSingletonRegisterAndBuild_Depth1Scenario(),
                new VContainerSingletonRegisterAndBuild_Depth1Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-first-resolve-depth1", new Scenario[]
            {
                new SparseInjectSingletonFirstResolve_Depth1Scenario(),
                new VContainerSingletonFirstResolve_Depth1Scenario(),
                new ManualSingletonFirstResolve_Depth1Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-second-resolve-depth1", new Scenario[]
            {
                new SparseInjectSingletonSecondResolve_Depth1Scenario(),
                new VContainerSingletonSecondResolve_Depth1Scenario(),
                new ManualSingletonSecondResolve_Depth1Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-total-depth1", new Scenario[]
            {
                new SparseInjectSingletonTotal_Depth1Scenario(),
                new VContainerSingletonTotal_Depth1Scenario(),
                new ManualSingletonTotal_Depth1Scenario(),
            }, samples);
        }

        private static void AddBenchmarkCategoryDepth2(BenchmarkRunner benchmarkRunner, int samples)
        {
            benchmarkRunner.AddBenchmarkCategory("singleton-register-depth2", new Scenario[]
            {
                new SparseInjectSingletonRegister_Depth2Scenario(),
                new VContainerSingletonRegister_Depth2Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-build-depth2", new Scenario[]
            {
                new SparseInjectSingletonBuild_Depth2Scenario(),
                new VContainerSingletonBuild_Depth2Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-register-and-build-depth2", new Scenario[]
            {
                new SparseInjectSingletonRegisterAndBuild_Depth2Scenario(),
                new VContainerSingletonRegisterAndBuild_Depth2Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-first-resolve-depth2", new Scenario[]
            {
                new SparseInjectSingletonFirstResolve_Depth2Scenario(),
                new VContainerSingletonFirstResolve_Depth2Scenario(),
                new ManualSingletonFirstResolve_Depth2Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-second-resolve-depth2", new Scenario[]
            {
                new SparseInjectSingletonSecondResolve_Depth2Scenario(),
                new VContainerSingletonSecondResolve_Depth2Scenario(),
                new ManualSingletonSecondResolve_Depth2Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-total-depth2", new Scenario[]
            {
                new SparseInjectSingletonTotal_Depth2Scenario(),
                new VContainerSingletonTotal_Depth2Scenario(),
                new ManualSingletonTotal_Depth2Scenario(),
            }, samples);
        }

        private static void AddBenchmarkCategoryDepth3(BenchmarkRunner benchmarkRunner, int samples)
        {
            benchmarkRunner.AddBenchmarkCategory("singleton-register-depth3", new Scenario[]
            {
                new SparseInjectSingletonRegister_Depth3Scenario(),
                new VContainerSingletonRegister_Depth3Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-build-depth3", new Scenario[]
            {
                new SparseInjectSingletonBuild_Depth3Scenario(),
                new VContainerSingletonBuild_Depth3Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-register-and-build-depth3", new Scenario[]
            {
                new SparseInjectSingletonRegisterAndBuild_Depth3Scenario(),
                new VContainerSingletonRegisterAndBuild_Depth3Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-first-resolve-depth3", new Scenario[]
            {
                new SparseInjectSingletonFirstResolve_Depth3Scenario(),
                new VContainerSingletonFirstResolve_Depth3Scenario(),
                new ManualSingletonFirstResolve_Depth3Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-second-resolve-depth3", new Scenario[]
            {
                new SparseInjectSingletonSecondResolve_Depth3Scenario(),
                new VContainerSingletonSecondResolve_Depth3Scenario(),
                new ManualSingletonSecondResolve_Depth3Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-total-depth3", new Scenario[]
            {
                new SparseInjectSingletonTotal_Depth3Scenario(),
                new VContainerSingletonTotal_Depth3Scenario(),
                new ManualSingletonTotal_Depth3Scenario(),
            }, samples);
        }

        private static void AddBenchmarkCategoryDepth4(BenchmarkRunner benchmarkRunner, int samples)
        {
            benchmarkRunner.AddBenchmarkCategory("singleton-register-depth4", new Scenario[]
            {
                new SparseInjectSingletonRegister_Depth4Scenario(),
                new VContainerSingletonRegister_Depth4Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-build-depth4", new Scenario[]
            {
                new SparseInjectSingletonBuild_Depth4Scenario(),
                new VContainerSingletonBuild_Depth4Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-register-and-build-depth4", new Scenario[]
            {
                new SparseInjectSingletonRegisterAndBuild_Depth4Scenario(),
                new VContainerSingletonRegisterAndBuild_Depth4Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-first-resolve-depth4", new Scenario[]
            {
                new SparseInjectSingletonFirstResolve_Depth4Scenario(),
                new VContainerSingletonFirstResolve_Depth4Scenario(),
                new ManualSingletonFirstResolve_Depth4Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-second-resolve-depth4", new Scenario[]
            {
                new SparseInjectSingletonSecondResolve_Depth4Scenario(),
                new VContainerSingletonSecondResolve_Depth4Scenario(),
                new ManualSingletonSecondResolve_Depth4Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-total-depth4", new Scenario[]
            {
                new SparseInjectSingletonTotal_Depth4Scenario(),
                new VContainerSingletonTotal_Depth4Scenario(),
                new ManualSingletonTotal_Depth4Scenario(),
            }, samples);
        }

        private static void AddBenchmarkCategoryDepth5(BenchmarkRunner benchmarkRunner, int samples)
        {
            benchmarkRunner.AddBenchmarkCategory("singleton-register-depth5", new Scenario[]
            {
                new SparseInjectSingletonRegister_Depth5Scenario(),
                new VContainerSingletonRegister_Depth5Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-build-depth5", new Scenario[]
            {
                new SparseInjectSingletonBuild_Depth5Scenario(),
                new VContainerSingletonBuild_Depth5Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-register-and-build-depth5", new Scenario[]
            {
                new SparseInjectSingletonRegisterAndBuild_Depth5Scenario(),
                new VContainerSingletonRegisterAndBuild_Depth5Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-first-resolve-depth5", new Scenario[]
            {
                new SparseInjectSingletonFirstResolve_Depth5Scenario(),
                new VContainerSingletonFirstResolve_Depth5Scenario(),
                new ManualSingletonFirstResolve_Depth5Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-second-resolve-depth5", new Scenario[]
            {
                new SparseInjectSingletonSecondResolve_Depth5Scenario(),
                new VContainerSingletonSecondResolve_Depth5Scenario(),
                new ManualSingletonSecondResolve_Depth5Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-total-depth5", new Scenario[]
            {
                new SparseInjectSingletonTotal_Depth5Scenario(),
                new VContainerSingletonTotal_Depth5Scenario(),
                new ManualSingletonTotal_Depth5Scenario(),
            }, samples);
        }

        private static void AddBenchmarkCategoryDepth6(BenchmarkRunner benchmarkRunner, int samples)
        {
            benchmarkRunner.AddBenchmarkCategory("singleton-register-depth6", new Scenario[]
            {
                new SparseInjectSingletonRegister_Depth6Scenario(),
                new VContainerSingletonRegister_Depth6Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-build-depth6", new Scenario[]
            {
                new SparseInjectSingletonBuild_Depth6Scenario(),
                new VContainerSingletonBuild_Depth6Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-register-and-build-depth6", new Scenario[]
            {
                new SparseInjectSingletonRegisterAndBuild_Depth6Scenario(),
                new VContainerSingletonRegisterAndBuild_Depth6Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-first-resolve-depth6", new Scenario[]
            {
                new SparseInjectSingletonFirstResolve_Depth6Scenario(),
                new VContainerSingletonFirstResolve_Depth6Scenario(),
                new ManualSingletonFirstResolve_Depth6Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-second-resolve-depth6", new Scenario[]
            {
                new SparseInjectSingletonSecondResolve_Depth6Scenario(),
                new VContainerSingletonSecondResolve_Depth6Scenario(),
                new ManualSingletonSecondResolve_Depth6Scenario(),
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-total-depth6", new Scenario[]
            {
                new SparseInjectSingletonTotal_Depth6Scenario(),
                new VContainerSingletonTotal_Depth6Scenario(),
                new ManualSingletonTotal_Depth6Scenario(),
            }, samples);
        }
    }
}
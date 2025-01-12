using SparseInject.BenchmarkFramework;

namespace SparseInject.Benchmarks.Net
{
    public static class SingletonBenchmarkUtility
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
#if NET
                new AutofacSingletonRegister_Depth1Scenario(),
                new LightInjectSingletonRegister_Depth1Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonRegister_Depth1Scenario(),
                new ZenjectSingletonRegister_Depth1Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-build-depth1", new Scenario[]
            {
                new SparseInjectSingletonBuild_Depth1Scenario(),
                new VContainerSingletonBuild_Depth1Scenario(),
#if NET
                new AutofacSingletonBuild_Depth1Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonBuild_Depth1Scenario(),
                new ZenjectSingletonBuild_Depth1Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-register-and-build-depth1", new Scenario[]
            {
                new SparseInjectSingletonRegisterAndBuild_Depth1Scenario(),
                new VContainerSingletonRegisterAndBuild_Depth1Scenario(),
#if NET
                new AutofacSingletonRegisterAndBuild_Depth1Scenario(),
                new LightInjectSingletonRegisterAndBuild_Depth1Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonRegisterAndBuild_Depth1Scenario(),
                new ZenjectSingletonRegisterAndBuild_Depth1Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-first-resolve-depth1", new Scenario[]
            {
                new SparseInjectSingletonFirstResolve_Depth1Scenario(),
                new VContainerSingletonFirstResolve_Depth1Scenario(),
                new ManualSingletonFirstResolve_Depth1Scenario(),
#if NET
                new AutofacSingletonFirstResolve_Depth1Scenario(),
                new LightInjectSingletonFirstResolve_Depth1Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonFirstResolve_Depth1Scenario(),
                new ZenjectSingletonFirstResolve_Depth1Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-second-resolve-depth1", new Scenario[]
            {
                new SparseInjectSingletonSecondResolve_Depth1Scenario(),
                new VContainerSingletonSecondResolve_Depth1Scenario(),
                new ManualSingletonSecondResolve_Depth1Scenario(),
#if NET
                new AutofacSingletonSecondResolve_Depth1Scenario(),
                new LightInjectSingletonSecondResolve_Depth1Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonSecondResolve_Depth1Scenario(),
                new ZenjectSingletonSecondResolve_Depth1Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-total-depth1", new Scenario[]
            {
                new SparseInjectSingletonTotal_Depth1Scenario(),
                new VContainerSingletonTotal_Depth1Scenario(),
                new ManualSingletonTotal_Depth1Scenario(),
#if NET
                new AutofacSingletonTotal_Depth1Scenario(),
                new LightInjectSingletonTotal_Depth1Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonTotal_Depth1Scenario(),
                new ZenjectSingletonTotal_Depth1Scenario(),
#endif
            }, samples);
        }

        private static void AddBenchmarkCategoryDepth2(BenchmarkRunner benchmarkRunner, int samples)
        {
            benchmarkRunner.AddBenchmarkCategory("singleton-register-depth2", new Scenario[]
            {
                new SparseInjectSingletonRegister_Depth2Scenario(),
                new VContainerSingletonRegister_Depth2Scenario(),
#if NET
                new AutofacSingletonRegister_Depth2Scenario(),
                new LightInjectSingletonRegister_Depth2Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonRegister_Depth2Scenario(),
                new ZenjectSingletonRegister_Depth2Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-build-depth2", new Scenario[]
            {
                new SparseInjectSingletonBuild_Depth2Scenario(),
                new VContainerSingletonBuild_Depth2Scenario(),
#if NET
                new AutofacSingletonBuild_Depth2Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonBuild_Depth2Scenario(),
                new ZenjectSingletonBuild_Depth2Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-register-and-build-depth2", new Scenario[]
            {
                new SparseInjectSingletonRegisterAndBuild_Depth2Scenario(),
                new VContainerSingletonRegisterAndBuild_Depth2Scenario(),
#if NET
                new AutofacSingletonRegisterAndBuild_Depth2Scenario(),
                new LightInjectSingletonRegisterAndBuild_Depth2Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonRegisterAndBuild_Depth2Scenario(),
                new ZenjectSingletonRegisterAndBuild_Depth2Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-first-resolve-depth2", new Scenario[]
            {
                new SparseInjectSingletonFirstResolve_Depth2Scenario(),
                new VContainerSingletonFirstResolve_Depth2Scenario(),
                new ManualSingletonFirstResolve_Depth2Scenario(),
#if NET
                new AutofacSingletonFirstResolve_Depth2Scenario(),
                new LightInjectSingletonFirstResolve_Depth2Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonFirstResolve_Depth2Scenario(),
                new ZenjectSingletonFirstResolve_Depth2Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-second-resolve-depth2", new Scenario[]
            {
                new SparseInjectSingletonSecondResolve_Depth2Scenario(),
                new VContainerSingletonSecondResolve_Depth2Scenario(),
                new ManualSingletonSecondResolve_Depth2Scenario(),
#if NET
                new AutofacSingletonSecondResolve_Depth2Scenario(),
                new LightInjectSingletonSecondResolve_Depth2Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonSecondResolve_Depth2Scenario(),
                new ZenjectSingletonSecondResolve_Depth2Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-total-depth2", new Scenario[]
            {
                new SparseInjectSingletonTotal_Depth2Scenario(),
                new VContainerSingletonTotal_Depth2Scenario(),
                new ManualSingletonTotal_Depth2Scenario(),
#if NET
                new AutofacSingletonTotal_Depth2Scenario(),
                new LightInjectSingletonTotal_Depth2Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonTotal_Depth2Scenario(),
                new ZenjectSingletonTotal_Depth2Scenario(),
#endif
            }, samples);
        }

        private static void AddBenchmarkCategoryDepth3(BenchmarkRunner benchmarkRunner, int samples)
        {
            benchmarkRunner.AddBenchmarkCategory("singleton-register-depth3", new Scenario[]
            {
                new SparseInjectSingletonRegister_Depth3Scenario(),
                new VContainerSingletonRegister_Depth3Scenario(),
#if NET
                new AutofacSingletonRegister_Depth3Scenario(),
                new LightInjectSingletonRegister_Depth3Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonRegister_Depth3Scenario(),
                new ZenjectSingletonRegister_Depth3Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-build-depth3", new Scenario[]
            {
                new SparseInjectSingletonBuild_Depth3Scenario(),
                new VContainerSingletonBuild_Depth3Scenario(),
#if NET
                new AutofacSingletonBuild_Depth3Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonBuild_Depth3Scenario(),
                new ZenjectSingletonBuild_Depth3Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-register-and-build-depth3", new Scenario[]
            {
                new SparseInjectSingletonRegisterAndBuild_Depth3Scenario(),
                new VContainerSingletonRegisterAndBuild_Depth3Scenario(),
#if NET
                new AutofacSingletonRegisterAndBuild_Depth3Scenario(),
                new LightInjectSingletonRegisterAndBuild_Depth3Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonRegisterAndBuild_Depth3Scenario(),
                new ZenjectSingletonRegisterAndBuild_Depth3Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-first-resolve-depth3", new Scenario[]
            {
                new SparseInjectSingletonFirstResolve_Depth3Scenario(),
                new VContainerSingletonFirstResolve_Depth3Scenario(),
                new ManualSingletonFirstResolve_Depth3Scenario(),
#if NET
                new AutofacSingletonFirstResolve_Depth3Scenario(),
                new LightInjectSingletonFirstResolve_Depth3Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonFirstResolve_Depth3Scenario(),
                new ZenjectSingletonFirstResolve_Depth3Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-second-resolve-depth3", new Scenario[]
            {
                new SparseInjectSingletonSecondResolve_Depth3Scenario(),
                new VContainerSingletonSecondResolve_Depth3Scenario(),
                new ManualSingletonSecondResolve_Depth3Scenario(),
#if NET
                new AutofacSingletonSecondResolve_Depth3Scenario(),
                new LightInjectSingletonSecondResolve_Depth3Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonSecondResolve_Depth3Scenario(),
                new ZenjectSingletonSecondResolve_Depth3Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-total-depth3", new Scenario[]
            {
                new SparseInjectSingletonTotal_Depth3Scenario(),
                new VContainerSingletonTotal_Depth3Scenario(),
                new ManualSingletonTotal_Depth3Scenario(),
#if NET
                new AutofacSingletonTotal_Depth3Scenario(),
                new LightInjectSingletonTotal_Depth3Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonTotal_Depth3Scenario(),
                new ZenjectSingletonTotal_Depth3Scenario(),
#endif
            }, samples);
        }

        private static void AddBenchmarkCategoryDepth4(BenchmarkRunner benchmarkRunner, int samples)
        {
            benchmarkRunner.AddBenchmarkCategory("singleton-register-depth4", new Scenario[]
            {
                new SparseInjectSingletonRegister_Depth4Scenario(),
                new VContainerSingletonRegister_Depth4Scenario(),
#if NET
                new AutofacSingletonRegister_Depth4Scenario(),
                new LightInjectSingletonRegister_Depth4Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonRegister_Depth4Scenario(),
                new ZenjectSingletonRegister_Depth4Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-build-depth4", new Scenario[]
            {
                new SparseInjectSingletonBuild_Depth4Scenario(),
                new VContainerSingletonBuild_Depth4Scenario(),
#if NET
                new AutofacSingletonBuild_Depth4Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonBuild_Depth4Scenario(),
                new ZenjectSingletonBuild_Depth4Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-register-and-build-depth4", new Scenario[]
            {
                new SparseInjectSingletonRegisterAndBuild_Depth4Scenario(),
                new VContainerSingletonRegisterAndBuild_Depth4Scenario(),
#if NET
                new AutofacSingletonRegisterAndBuild_Depth4Scenario(),
                new LightInjectSingletonRegisterAndBuild_Depth4Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonRegisterAndBuild_Depth4Scenario(),
                new ZenjectSingletonRegisterAndBuild_Depth4Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-first-resolve-depth4", new Scenario[]
            {
                new SparseInjectSingletonFirstResolve_Depth4Scenario(),
                new VContainerSingletonFirstResolve_Depth4Scenario(),
                new ManualSingletonFirstResolve_Depth4Scenario(),
#if NET
                new AutofacSingletonFirstResolve_Depth4Scenario(),
                new LightInjectSingletonFirstResolve_Depth4Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonFirstResolve_Depth4Scenario(),
                new ZenjectSingletonFirstResolve_Depth4Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-second-resolve-depth4", new Scenario[]
            {
                new SparseInjectSingletonSecondResolve_Depth4Scenario(),
                new VContainerSingletonSecondResolve_Depth4Scenario(),
                new ManualSingletonSecondResolve_Depth4Scenario(),
#if NET
                new AutofacSingletonSecondResolve_Depth4Scenario(),
                new LightInjectSingletonSecondResolve_Depth4Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonSecondResolve_Depth4Scenario(),
                new ZenjectSingletonSecondResolve_Depth4Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-total-depth4", new Scenario[]
            {
                new SparseInjectSingletonTotal_Depth4Scenario(),
                new VContainerSingletonTotal_Depth4Scenario(),
                new ManualSingletonTotal_Depth4Scenario(),
#if NET
                new AutofacSingletonTotal_Depth4Scenario(),
                new LightInjectSingletonTotal_Depth4Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonTotal_Depth4Scenario(),
                new ZenjectSingletonTotal_Depth4Scenario(),
#endif
            }, samples);
        }

        private static void AddBenchmarkCategoryDepth5(BenchmarkRunner benchmarkRunner, int samples)
        {
            benchmarkRunner.AddBenchmarkCategory("singleton-register-depth5", new Scenario[]
            {
                new SparseInjectSingletonRegister_Depth5Scenario(),
                new VContainerSingletonRegister_Depth5Scenario(),
#if NET
                new AutofacSingletonRegister_Depth5Scenario(),
                new LightInjectSingletonRegister_Depth5Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonRegister_Depth5Scenario(),
                new ZenjectSingletonRegister_Depth5Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-build-depth5", new Scenario[]
            {
                new SparseInjectSingletonBuild_Depth5Scenario(),
                new VContainerSingletonBuild_Depth5Scenario(),
#if NET
                new AutofacSingletonBuild_Depth5Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonBuild_Depth5Scenario(),
                new ZenjectSingletonBuild_Depth5Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-register-and-build-depth5", new Scenario[]
            {
                new SparseInjectSingletonRegisterAndBuild_Depth5Scenario(),
                new VContainerSingletonRegisterAndBuild_Depth5Scenario(),
#if NET
                new AutofacSingletonRegisterAndBuild_Depth5Scenario(),
                new LightInjectSingletonRegisterAndBuild_Depth5Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonRegisterAndBuild_Depth5Scenario(),
                new ZenjectSingletonRegisterAndBuild_Depth5Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-first-resolve-depth5", new Scenario[]
            {
                new SparseInjectSingletonFirstResolve_Depth5Scenario(),
                new VContainerSingletonFirstResolve_Depth5Scenario(),
                new ManualSingletonFirstResolve_Depth5Scenario(),
#if NET
                new AutofacSingletonFirstResolve_Depth5Scenario(),
                new LightInjectSingletonFirstResolve_Depth5Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonFirstResolve_Depth5Scenario(),
                new ZenjectSingletonFirstResolve_Depth5Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-second-resolve-depth5", new Scenario[]
            {
                new SparseInjectSingletonSecondResolve_Depth5Scenario(),
                new VContainerSingletonSecondResolve_Depth5Scenario(),
                new ManualSingletonSecondResolve_Depth5Scenario(),
#if NET
                new AutofacSingletonSecondResolve_Depth5Scenario(),
                new LightInjectSingletonSecondResolve_Depth5Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonSecondResolve_Depth5Scenario(),
                new ZenjectSingletonSecondResolve_Depth5Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-total-depth5", new Scenario[]
            {
                new SparseInjectSingletonTotal_Depth5Scenario(),
                new VContainerSingletonTotal_Depth5Scenario(),
                new ManualSingletonTotal_Depth5Scenario(),
#if NET
                new AutofacSingletonTotal_Depth5Scenario(),
                new LightInjectSingletonTotal_Depth5Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonTotal_Depth5Scenario(),
                new ZenjectSingletonTotal_Depth5Scenario(),
#endif
            }, samples);
        }

        private static void AddBenchmarkCategoryDepth6(BenchmarkRunner benchmarkRunner, int samples)
        {
            benchmarkRunner.AddBenchmarkCategory("singleton-register-depth6", new Scenario[]
            {
                new SparseInjectSingletonRegister_Depth6Scenario(),
                new VContainerSingletonRegister_Depth6Scenario(),
#if NET
                new AutofacSingletonRegister_Depth6Scenario(),
                new LightInjectSingletonRegister_Depth6Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonRegister_Depth6Scenario(),
                new ZenjectSingletonRegister_Depth6Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-build-depth6", new Scenario[]
            {
                new SparseInjectSingletonBuild_Depth6Scenario(),
                new VContainerSingletonBuild_Depth6Scenario(),
#if NET
                new AutofacSingletonBuild_Depth6Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonBuild_Depth6Scenario(),
                new ZenjectSingletonBuild_Depth6Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-register-and-build-depth6", new Scenario[]
            {
                new SparseInjectSingletonRegisterAndBuild_Depth6Scenario(),
                new VContainerSingletonRegisterAndBuild_Depth6Scenario(),
#if NET
                new AutofacSingletonRegisterAndBuild_Depth6Scenario(),
                new LightInjectSingletonRegisterAndBuild_Depth6Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonRegisterAndBuild_Depth6Scenario(),
                new ZenjectSingletonRegisterAndBuild_Depth6Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-first-resolve-depth6", new Scenario[]
            {
                new SparseInjectSingletonFirstResolve_Depth6Scenario(),
                new VContainerSingletonFirstResolve_Depth6Scenario(),
                new ManualSingletonFirstResolve_Depth6Scenario(),
#if NET
                new AutofacSingletonFirstResolve_Depth6Scenario(),
                new LightInjectSingletonFirstResolve_Depth6Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonFirstResolve_Depth6Scenario(),
                new ZenjectSingletonFirstResolve_Depth6Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-second-resolve-depth6", new Scenario[]
            {
                new SparseInjectSingletonSecondResolve_Depth6Scenario(),
                new VContainerSingletonSecondResolve_Depth6Scenario(),
                new ManualSingletonSecondResolve_Depth6Scenario(),
#if NET
                new AutofacSingletonSecondResolve_Depth6Scenario(),
                new LightInjectSingletonSecondResolve_Depth6Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonSecondResolve_Depth6Scenario(),
                new ZenjectSingletonSecondResolve_Depth6Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("singleton-total-depth6", new Scenario[]
            {
                new SparseInjectSingletonTotal_Depth6Scenario(),
                new VContainerSingletonTotal_Depth6Scenario(),
                new ManualSingletonTotal_Depth6Scenario(),
#if NET
                new AutofacSingletonTotal_Depth6Scenario(),
                new LightInjectSingletonTotal_Depth6Scenario(),
#endif
#if UNITY_2017_1_OR_NEWER
                new ReflexSingletonTotal_Depth6Scenario(),
                new ZenjectSingletonTotal_Depth6Scenario(),
#endif
            }, samples);
        }
    }
}
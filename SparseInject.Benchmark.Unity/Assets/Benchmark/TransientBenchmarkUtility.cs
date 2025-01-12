using SparseInject.BenchmarkFramework;

namespace SparseInject.Benchmarks.Net
{
    public class TransientBenchmarkUtility
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
            benchmarkRunner.AddBenchmarkCategory("transient-register-depth1", new Scenario[]
            {
                new SparseInjectTransientRegister_Depth1Scenario(),
                new VContainerTransientRegister_Depth1Scenario(),
#if NET
                new AutofacTransientRegister_Depth1Scenario(),
                new LightInjectTransientRegister_Depth1Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-build-depth1", new Scenario[]
            {
                new SparseInjectTransientBuild_Depth1Scenario(),
                new VContainerTransientBuild_Depth1Scenario(),
#if NET
                new AutofacTransientBuild_Depth1Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-register-and-build-depth1", new Scenario[]
            {
                new SparseInjectTransientRegisterAndBuild_Depth1Scenario(),
                new VContainerTransientRegisterAndBuild_Depth1Scenario(),
#if NET
                new AutofacTransientRegisterAndBuild_Depth1Scenario(),
                new LightInjectTransientRegisterAndBuild_Depth1Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-first-resolve-depth1", new Scenario[]
            {
                new SparseInjectTransientFirstResolve_Depth1Scenario(),
                new VContainerTransientFirstResolve_Depth1Scenario(),
                new ManualTransientFirstResolve_Depth1Scenario(),
#if NET
                new AutofacTransientFirstResolve_Depth1Scenario(),
                new LightInjectTransientFirstResolve_Depth1Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-second-resolve-depth1", new Scenario[]
            {
                new SparseInjectTransientSecondResolve_Depth1Scenario(),
                new VContainerTransientSecondResolve_Depth1Scenario(),
                new ManualTransientSecondResolve_Depth1Scenario(),
#if NET
                new AutofacTransientSecondResolve_Depth1Scenario(),
                new LightInjectTransientSecondResolve_Depth1Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-total-depth1", new Scenario[]
            {
                new SparseInjectTransientTotal_Depth1Scenario(),
                new VContainerTransientTotal_Depth1Scenario(),
                new ManualTransientTotal_Depth1Scenario(),
#if NET
                new AutofacTransientTotal_Depth1Scenario(),
                new LightInjectTransientTotal_Depth1Scenario(),
#endif
            }, samples);
        }

        private static void AddBenchmarkCategoryDepth2(BenchmarkRunner benchmarkRunner, int samples)
        {
            benchmarkRunner.AddBenchmarkCategory("transient-register-depth2", new Scenario[]
            {
                new SparseInjectTransientRegister_Depth2Scenario(),
                new VContainerTransientRegister_Depth2Scenario(),
#if NET
                new AutofacTransientRegister_Depth2Scenario(),
                new LightInjectTransientRegister_Depth2Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-build-depth2", new Scenario[]
            {
                new SparseInjectTransientBuild_Depth2Scenario(),
                new VContainerTransientBuild_Depth2Scenario(),
#if NET
                new AutofacTransientBuild_Depth2Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-register-and-build-depth2", new Scenario[]
            {
                new SparseInjectTransientRegisterAndBuild_Depth2Scenario(),
                new VContainerTransientRegisterAndBuild_Depth2Scenario(),
#if NET
                new AutofacTransientRegisterAndBuild_Depth2Scenario(),
                new LightInjectTransientRegisterAndBuild_Depth2Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-first-resolve-depth2", new Scenario[]
            {
                new SparseInjectTransientFirstResolve_Depth2Scenario(),
                new VContainerTransientFirstResolve_Depth2Scenario(),
                new ManualTransientFirstResolve_Depth2Scenario(),
#if NET
                new AutofacTransientFirstResolve_Depth2Scenario(),
                new LightInjectTransientFirstResolve_Depth2Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-second-resolve-depth2", new Scenario[]
            {
                new SparseInjectTransientSecondResolve_Depth2Scenario(),
                new VContainerTransientSecondResolve_Depth2Scenario(),
                new ManualTransientSecondResolve_Depth2Scenario(),
#if NET
                new AutofacTransientSecondResolve_Depth2Scenario(),
                new LightInjectTransientSecondResolve_Depth2Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-total-depth2", new Scenario[]
            {
                new SparseInjectTransientTotal_Depth2Scenario(),
                new VContainerTransientTotal_Depth2Scenario(),
                new ManualTransientTotal_Depth2Scenario(),
#if NET
                new AutofacTransientTotal_Depth2Scenario(),
                new LightInjectTransientTotal_Depth2Scenario(),
#endif
            }, samples);
        }

        private static void AddBenchmarkCategoryDepth3(BenchmarkRunner benchmarkRunner, int samples)
        {
            benchmarkRunner.AddBenchmarkCategory("transient-register-depth3", new Scenario[]
            {
                new SparseInjectTransientRegister_Depth3Scenario(),
                new VContainerTransientRegister_Depth3Scenario(),
#if NET
                new AutofacTransientRegister_Depth3Scenario(),
                new LightInjectTransientRegister_Depth3Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-build-depth3", new Scenario[]
            {
                new SparseInjectTransientBuild_Depth3Scenario(),
                new VContainerTransientBuild_Depth3Scenario(),
#if NET
                new AutofacTransientBuild_Depth3Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-register-and-build-depth3", new Scenario[]
            {
                new SparseInjectTransientRegisterAndBuild_Depth3Scenario(),
                new VContainerTransientRegisterAndBuild_Depth3Scenario(),
#if NET
                new AutofacTransientRegisterAndBuild_Depth3Scenario(),
                new LightInjectTransientRegisterAndBuild_Depth3Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-first-resolve-depth3", new Scenario[]
            {
                new SparseInjectTransientFirstResolve_Depth3Scenario(),
                new VContainerTransientFirstResolve_Depth3Scenario(),
                new ManualTransientFirstResolve_Depth3Scenario(),
#if NET
                new AutofacTransientFirstResolve_Depth3Scenario(),
                new LightInjectTransientFirstResolve_Depth3Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-second-resolve-depth3", new Scenario[]
            {
                new SparseInjectTransientSecondResolve_Depth3Scenario(),
                new VContainerTransientSecondResolve_Depth3Scenario(),
                new ManualTransientSecondResolve_Depth3Scenario(),
#if NET
                new AutofacTransientSecondResolve_Depth3Scenario(),
                new LightInjectTransientSecondResolve_Depth3Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-total-depth3", new Scenario[]
            {
                new SparseInjectTransientTotal_Depth3Scenario(),
                new VContainerTransientTotal_Depth3Scenario(),
                new ManualTransientTotal_Depth3Scenario(),
#if NET
                new AutofacTransientTotal_Depth3Scenario(),
                new LightInjectTransientTotal_Depth3Scenario(),
#endif
            }, samples);
        }

        private static void AddBenchmarkCategoryDepth4(BenchmarkRunner benchmarkRunner, int samples)
        {
            benchmarkRunner.AddBenchmarkCategory("transient-register-depth4", new Scenario[]
            {
                new SparseInjectTransientRegister_Depth4Scenario(),
                new VContainerTransientRegister_Depth4Scenario(),
#if NET
                new AutofacTransientRegister_Depth4Scenario(),
                new LightInjectTransientRegister_Depth4Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-build-depth4", new Scenario[]
            {
                new SparseInjectTransientBuild_Depth4Scenario(),
                new VContainerTransientBuild_Depth4Scenario(),
#if NET
                new AutofacTransientBuild_Depth4Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-register-and-build-depth4", new Scenario[]
            {
                new SparseInjectTransientRegisterAndBuild_Depth4Scenario(),
                new VContainerTransientRegisterAndBuild_Depth4Scenario(),
#if NET
                new AutofacTransientRegisterAndBuild_Depth4Scenario(),
                new LightInjectTransientRegisterAndBuild_Depth4Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-first-resolve-depth4", new Scenario[]
            {
                new SparseInjectTransientFirstResolve_Depth4Scenario(),
                new VContainerTransientFirstResolve_Depth4Scenario(),
                new ManualTransientFirstResolve_Depth4Scenario(),
#if NET
                new AutofacTransientFirstResolve_Depth4Scenario(),
                new LightInjectTransientFirstResolve_Depth4Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-second-resolve-depth4", new Scenario[]
            {
                new SparseInjectTransientSecondResolve_Depth4Scenario(),
                new VContainerTransientSecondResolve_Depth4Scenario(),
                new ManualTransientSecondResolve_Depth4Scenario(),
#if NET
                new AutofacTransientSecondResolve_Depth4Scenario(),
                new LightInjectTransientSecondResolve_Depth4Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-total-depth4", new Scenario[]
            {
                new SparseInjectTransientTotal_Depth4Scenario(),
                new VContainerTransientTotal_Depth4Scenario(),
                new ManualTransientTotal_Depth4Scenario(),
#if NET
                new AutofacTransientTotal_Depth4Scenario(),
                new LightInjectTransientTotal_Depth4Scenario(),
#endif
            }, samples);
        }

        private static void AddBenchmarkCategoryDepth5(BenchmarkRunner benchmarkRunner, int samples)
        {
            benchmarkRunner.AddBenchmarkCategory("transient-register-depth5", new Scenario[]
            {
                new SparseInjectTransientRegister_Depth5Scenario(),
                new VContainerTransientRegister_Depth5Scenario(),
#if NET
                new AutofacTransientRegister_Depth5Scenario(),
                new LightInjectTransientRegister_Depth5Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-build-depth5", new Scenario[]
            {
                new SparseInjectTransientBuild_Depth5Scenario(),
                new VContainerTransientBuild_Depth5Scenario(),
#if NET
                new AutofacTransientBuild_Depth5Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-register-and-build-depth5", new Scenario[]
            {
                new SparseInjectTransientRegisterAndBuild_Depth5Scenario(),
                new VContainerTransientRegisterAndBuild_Depth5Scenario(),
#if NET
                new AutofacTransientRegisterAndBuild_Depth5Scenario(),
                new LightInjectTransientRegisterAndBuild_Depth5Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-first-resolve-depth5", new Scenario[]
            {
                new SparseInjectTransientFirstResolve_Depth5Scenario(),
                new VContainerTransientFirstResolve_Depth5Scenario(),
                new ManualTransientFirstResolve_Depth5Scenario(),
#if NET
                new AutofacTransientFirstResolve_Depth5Scenario(),
                new LightInjectTransientFirstResolve_Depth5Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-second-resolve-depth5", new Scenario[]
            {
                new SparseInjectTransientSecondResolve_Depth5Scenario(),
                new VContainerTransientSecondResolve_Depth5Scenario(),
                new ManualTransientSecondResolve_Depth5Scenario(),
#if NET
                new AutofacTransientSecondResolve_Depth5Scenario(),
                new LightInjectTransientSecondResolve_Depth5Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-total-depth5", new Scenario[]
            {
                new SparseInjectTransientTotal_Depth5Scenario(),
                new VContainerTransientTotal_Depth5Scenario(),
                new ManualTransientTotal_Depth5Scenario(),
#if NET
                new AutofacTransientTotal_Depth5Scenario(),
                new LightInjectTransientTotal_Depth5Scenario(),
#endif
            }, samples);
        }

        private static void AddBenchmarkCategoryDepth6(BenchmarkRunner benchmarkRunner, int samples)
        {
            benchmarkRunner.AddBenchmarkCategory("transient-register-depth6", new Scenario[]
            {
                new SparseInjectTransientRegister_Depth6Scenario(),
                new VContainerTransientRegister_Depth6Scenario(),
#if NET
                new AutofacTransientRegister_Depth6Scenario(),
                new LightInjectTransientRegister_Depth6Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-build-depth6", new Scenario[]
            {
                new SparseInjectTransientBuild_Depth6Scenario(),
                new VContainerTransientBuild_Depth6Scenario(),
#if NET
                new AutofacTransientBuild_Depth6Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-register-and-build-depth6", new Scenario[]
            {
                new SparseInjectTransientRegisterAndBuild_Depth6Scenario(),
                new VContainerTransientRegisterAndBuild_Depth6Scenario(),
#if NET
                new AutofacTransientRegisterAndBuild_Depth6Scenario(),
                new LightInjectTransientRegisterAndBuild_Depth6Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-first-resolve-depth6", new Scenario[]
            {
                new SparseInjectTransientFirstResolve_Depth6Scenario(),
                new VContainerTransientFirstResolve_Depth6Scenario(),
                new ManualTransientFirstResolve_Depth6Scenario(),
#if NET
                new AutofacTransientFirstResolve_Depth6Scenario(),
                new LightInjectTransientFirstResolve_Depth6Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-second-resolve-depth6", new Scenario[]
            {
                new SparseInjectTransientSecondResolve_Depth6Scenario(),
                new VContainerTransientSecondResolve_Depth6Scenario(),
                new ManualTransientSecondResolve_Depth6Scenario(),
#if NET
                new AutofacTransientSecondResolve_Depth6Scenario(),
                new LightInjectTransientSecondResolve_Depth6Scenario(),
#endif
            }, samples);

            benchmarkRunner.AddBenchmarkCategory("transient-total-depth6", new Scenario[]
            {
                new SparseInjectTransientTotal_Depth6Scenario(),
                new VContainerTransientTotal_Depth6Scenario(),
                new ManualTransientTotal_Depth6Scenario(),
#if NET
                new AutofacTransientTotal_Depth6Scenario(),
                new LightInjectTransientTotal_Depth6Scenario(),
#endif
            }, samples);
        }
    }
}
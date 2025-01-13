namespace SparseInject
{
#if UNITY_2017_1_OR_NEWER
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public static class ArrayUtilities
    {
        public static void Fill(int[] array, int value)
        {
            var count = array.Length;
            var batchIterations = count / 32;

            for (var i = 0; i < batchIterations; i += 32)
            {
                array[i] = value;
                array[i + 1] = value;
                array[i + 2] = value;
                array[i + 3] = value;
                array[i + 4] = value;
                array[i + 5] = value;
                array[i + 6] = value;
                array[i + 7] = value;
                array[i + 8] = value;
                array[i + 9] = value;
                array[i + 10] = value;
                array[i + 11] = value;
                array[i + 12] = value;
                array[i + 13] = value;
                array[i + 14] = value;
                array[i + 15] = value;
                array[i + 16] = value;
                array[i + 17] = value;
                array[i + 18] = value;
                array[i + 19] = value;
                array[i + 20] = value;
                array[i + 21] = value;
                array[i + 22] = value;
                array[i + 23] = value;
                array[i + 24] = value;
                array[i + 25] = value;
                array[i + 26] = value;
                array[i + 27] = value;
                array[i + 28] = value;
                array[i + 29] = value;
                array[i + 30] = value;
                array[i + 31] = value;
            }

            for (var i = batchIterations * 32; i < count; i++)
            {
                array[i] = value;
            }
        }
        
        public static void Fill(int[] array, int value, int startFrom)
        {
            var count = array.Length - startFrom;
            var batchIterations = count / 32;
            var lastIterationsStart = startFrom + batchIterations * 32;

            if (batchIterations < 1)
            {
                lastIterationsStart = startFrom;
            }

            var batchTo = startFrom + batchIterations * 32;

            for (var i = startFrom; i < batchTo; i += 32)
            {
                array[i] = value;
                array[i + 1] = value;
                array[i + 2] = value;
                array[i + 3] = value;
                array[i + 4] = value;
                array[i + 5] = value;
                array[i + 6] = value;
                array[i + 7] = value;
                array[i + 8] = value;
                array[i + 9] = value;
                array[i + 10] = value;
                array[i + 11] = value;
                array[i + 12] = value;
                array[i + 13] = value;
                array[i + 14] = value;
                array[i + 15] = value;
                array[i + 16] = value;
                array[i + 17] = value;
                array[i + 18] = value;
                array[i + 19] = value;
                array[i + 20] = value;
                array[i + 21] = value;
                array[i + 22] = value;
                array[i + 23] = value;
                array[i + 24] = value;
                array[i + 25] = value;
                array[i + 26] = value;
                array[i + 27] = value;
                array[i + 28] = value;
                array[i + 29] = value;
                array[i + 30] = value;
                array[i + 31] = value;
            }

            for (var i = lastIterationsStart; i < array.Length; i++)
            {
                array[i] = value;
            }
        }
    }
}
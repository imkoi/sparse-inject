using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace CleanResolver
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class ArrayUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Insert<T>(ref T[] array, int elementsCount, T value, int index)
        {
            
        }
    }
}
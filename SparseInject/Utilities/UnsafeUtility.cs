using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace SparseInject
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class UnsafeUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T Cast<T>(object value) where T : class
        {
#if UNITY_ENGINE
            return Unity.Collections.LowLevel.Unsafe.UnsafeUtility.As<object, T>(ref value);
#endif
            return Unsafe.As<T>(value);
        }

        // public static unsafe ref T As<U, T>(ref U from)
        // {
        //     unsafe
        //     {
        //         return (T&) ref from;
        //     }
        // }
    }
}
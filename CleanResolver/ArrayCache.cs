using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace CleanResolver
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    internal static class ArrayCache
    {
        internal static object[][] _cache;
        private static object[] _reserved = new object[1024 * 64];
        private static int _count;

        private static Reserved _originalReserved = new Reserved()
        {
            Array = _reserved
        };

        public static object[][] GetConstructorParametersPool(int maxConstructorLength)
        {
            var requestedArrayLength = maxConstructorLength + 1;
            
            if (_cache == null)
            {
                _cache = new object[requestedArrayLength][];
            
                for (var i = 0; i < requestedArrayLength; i++)
                {
                    _cache[i] = new object[i];
                }

                return _cache;
            }
            
            var arrayLength = _cache.Length;

            if (requestedArrayLength >= arrayLength)
            {
                Array.Resize(ref _cache, requestedArrayLength);

                for (var i = arrayLength; i < requestedArrayLength; i++)
                {
                    _cache[i] = new object[i];
                }
            }

            return _cache;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Reserved PullReserved(int length)
        {
            ref var reserved = ref _originalReserved;
            
            reserved.StartIndex = _count;

            _count += length;

            return reserved;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushReserved(int count)
        {
            _count -= count;
        }
        
        public struct Reserved
        {
            public object[] Array;
            public int StartIndex;
        }
    }
}
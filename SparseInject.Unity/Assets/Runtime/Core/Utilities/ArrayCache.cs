using System;
using System.Runtime.CompilerServices;

namespace SparseInject
{
#if UNITY_2017_1_OR_NEWER
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    internal static class ArrayCache
    {
        internal static object[][] _cache;
        private static object[] _reserved = new object[1024];
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
            var arrayLen = _originalReserved.Array.Length;
            var freeSlots = arrayLen - _count;

            if (freeSlots - length < 0)
            {
                Array.Resize(ref _originalReserved.Array, (arrayLen + Math.Abs(freeSlots - length)) * 2);
            }
            
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
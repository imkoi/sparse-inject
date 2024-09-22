using Unity.IL2CPP.CompilerServices;

namespace CleanResolver
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    internal static class ArrayCache<T>
    {
        private static T[][] _cache;
        private static T[] _reserved = new T[1024 * 64];
        private static int _count;

        private static Reserved _originalReserved = new Reserved()
        {
            Array = _reserved
        };

        public static T[] Pull(int length)
        {
            if (_cache == null)
            {
                _cache = new T[64][];

                for (var i = 0; i < 64; i++)
                {
                    _cache[i] = new T[i];
                }
            }

            return _cache[length];
        }

        public static void Push(T[] array)
        {
            _cache[array.Length] = array;
        }

        public static Reserved PullReserved(int length)
        {
            ref var reserved = ref _originalReserved;

            reserved.Count = length;
            reserved.StartIndex = _count;

            _count += length;

            return reserved;
        }

        public static void PushReserved(ref Reserved reserved)
        {
            _count -= reserved.Count;
        }
        
        public struct Reserved
        {
            public T[] Array;
            public int StartIndex;
            public int Count;
        }
    }
}
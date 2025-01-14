using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace SparseInject
{
#if UNITY_2017_1_OR_NEWER
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class TypeIdProvider
    {
        public const int MaxCapacity = 16777216;

        private readonly float _resizeFactor;

        private struct Entry
        {
            public int Value;
            public int Hash;
            public Type Key;
        }

        private Entry[] _entries;

        private int _count;
        private int _capacity;
        private int _resizeThreshold;

        public int Count => _count;

        public TypeIdProvider(int capacity = 1024, float resizeFactor = 0.75f)
        {
            if (capacity >= MaxCapacity)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "The capacity must be less than 8388608.");
            }
            
            capacity *= 2;

            _resizeFactor = resizeFactor;
            _capacity = NextPowerOfTwo(capacity);

            if (_capacity > MaxCapacity)
            {
                _capacity = MaxCapacity;
            }

            _entries = new Entry[_capacity];

            _resizeThreshold = (int) (_capacity * _resizeFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetOrAddId(Type key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var capacity = TryResize();
            var hashCode = key.GetHashCode() & 0x7FFFFFFF;
            var mask = capacity - 1;
            var index = hashCode & mask;

            ref var entry = ref _entries[index];

            while (entry.Value != 0 && !(entry.Hash == hashCode && entry.Key == key))
            {
                index = (index + 1) & mask;
                entry = ref _entries[index];
            }

            if (entry.Value != 0)
            {
                return entry.Value - 1;
            }

            entry.Key = key;
            entry.Hash = hashCode;
            entry.Value = ++_count;

            return _count - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetId(Type key, out int id)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var capacity = _capacity;
            var hashCode = key.GetHashCode() & 0x7FFFFFFF;
            var mask = capacity - 1;
            var index = hashCode & mask;
            
            ref var entry = ref _entries[index];

            while (entry.Value != 0 && !(entry.Hash == hashCode && entry.Key == key))
            {
                index = (index + 1) & mask;
                entry = ref _entries[index];
            }

            id = entry.Value - 1;
            
            return id >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int TryResize()
        {
            if (_count >= _resizeThreshold)
            {
                var newCapacity = _capacity << 1;

                if (newCapacity > MaxCapacity)
                {
                    throw new IndexOutOfRangeException("Reached the maximum capacity.");
                }

                Resize(newCapacity);
            }

            return _capacity;
        }

        [ExcludeFromCodeCoverage]
        private void Resize(int newCapacity)
        {
            var oldEntries = _entries;
            var oldLength = oldEntries.Length;
            var newEntries = new Entry[newCapacity];
            var mask = newCapacity - 1;
            
            ref var newEntry = ref newEntries[0];
            
            for (var i = 0; i < oldLength; i++)
            {
                var oldEntry = oldEntries[i];
                
                if (oldEntry.Value != 0)
                {
                    var index = oldEntry.Hash & mask;
                    newEntry = ref newEntries[index];
                    
                    while (newEntry.Value != 0)
                    {
                        index = (index + 1) & mask;
                        newEntry = ref newEntries[index];
                    }

                    newEntry = oldEntry;
                }
            }
            
            _capacity = newCapacity;
            _resizeThreshold = (int) (newCapacity * _resizeFactor);
            _entries = newEntries;
        }

        private static int NextPowerOfTwo(int x)
        {
            if (x < 2)
            {
                return 2;
            }

            x--;
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;
            x++;
            
            return x;
        }
    }
}

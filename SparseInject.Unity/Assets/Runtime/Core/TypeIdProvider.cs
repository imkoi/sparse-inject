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
            if (capacity < 8)
            {
                capacity = 8;
            }

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
        public bool TryAdd(Type key, out int value)
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
                value = entry.Value - 1;
                
                return false;
            }

            entry.Key = key;
            entry.Hash = hashCode;
            entry.Value = ++_count;
            
            value = entry.Value - 1;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(Type key, out int id)
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

                ThrowIfCapacityExceededMaximum(newCapacity);

                Resize(newCapacity);
            }

            return _capacity;
        }

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

        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ThrowIfCapacityExceededMaximum(int capacity)
        {
            if (capacity > MaxCapacity)
            {
                throw new IndexOutOfRangeException("Reached the maximum capacity.");
            }
        }

        [ExcludeFromCodeCoverage]
        internal bool ContainsValue(int value, out Type key)
        {
            value++;
            
            var entries = _entries;
            var count = entries.Length;
            var batchCount = count / 8 * 8;

            var mask = -1;

            for (var i = 0; i < batchCount; i += 8)
            {
                mask = 0;
                mask |= (entries[i].Value == value ? 1 : 0) << 0;
                mask |= (entries[i + 1].Value == value ? 1 : 0) << 1;
                mask |= (entries[i + 2].Value == value ? 1 : 0) << 2;
                mask |= (entries[i + 3].Value == value ? 1 : 0) << 3;
                mask |= (entries[i + 4].Value == value ? 1 : 0) << 4;
                mask |= (entries[i + 5].Value == value ? 1 : 0) << 5;
                mask |= (entries[i + 6].Value == value ? 1 : 0) << 6;
                mask |= (entries[i + 7].Value == value ? 1 : 0) << 7;

                if (mask != 0)
                {
                    return ContainsValue(value, out key, i);
                }
            }

            key = null;
            
            return false;
        }
        
        [ExcludeFromCodeCoverage]
        private bool ContainsValue(int value, out Type key, int startIndex)
        {
            key = null;
            
            var entries = _entries;
            var targetIndex = -1;

            if (entries[startIndex].Value == value)
            {
                targetIndex = 0;
            }
            else if (entries[startIndex + 1].Value == value)
            {
                targetIndex = 1;
            }
            else if (entries[startIndex + 2].Value == value)
            {
                targetIndex = 2;
            }
            else if (entries[startIndex + 3].Value == value)
            {
                targetIndex = 3;
            }
            else if (entries[startIndex + 4].Value == value)
            {
                targetIndex = 4;
            }
            else if (entries[startIndex + 5].Value == value)
            {
                targetIndex = 5;
            }
            else if (entries[startIndex + 6].Value == value)
            {
                targetIndex = 6;
            }
            else if (entries[startIndex + 7].Value == value)
            {
                targetIndex = 7;
            }

            key = entries[startIndex + targetIndex].Key;

            return targetIndex >= 0;
        }

        private static int NextPowerOfTwo(int x)
        {
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

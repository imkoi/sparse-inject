using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SparseInject
{
#if UNITY_2017_1_OR_NEWER
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class TypeIdProvider
    {
        private readonly float _resizeFactor;
        public int MaxCapacity = 16777216;
        
        internal static int[] _primes = new int[]
        {
            3, 7, 13, 31, 61, 127, 251, 509, 1021, 2039, 4093, 8191, 16381, 32749, 65521,
            131071, 262139, 524287, 1048573, 2097143, 4194301, 8388593, 16777213, 33554467
        };

        [StructLayout(LayoutKind.Sequential)]
        private struct Entry
        {
            public int Value;
            public int HashCode;
            public Type Key;
        }

        private Entry[] _entries;
        private int _count;
        private int _capacity;

        private int _primeIndex;
        private int _resizeThreshold;

        public int Count => _count;

        public TypeIdProvider(int capacity = 1024, float resizeFactor = 0.75f)
        {
            if (capacity >= MaxCapacity)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "The capacity must be less than 8388608.");
            }

            capacity = (int) (capacity * (1f + 1f - resizeFactor));

            _resizeFactor = resizeFactor;

            var prime = _primes[_primeIndex];
            while (prime < capacity)
            {
                _primeIndex++;
                prime = _primes[_primeIndex];
            }

            _resizeThreshold = (int)(prime * _resizeFactor);
            _capacity = prime;
            _entries = new Entry[prime];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetOrAddId(Type key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var capacity = TryResize();
            var entries = _entries;
            var hashCode = key.GetHashCode() & 0x7FFFFFFF;
            var originalIndex = hashCode % capacity;
            var index = originalIndex;

            ref var entry = ref entries[index];

            while (entry.Value != 0 && !(entry.HashCode == hashCode && entry.Key == key))
            {
                index = (index + 1) % capacity;
                entry = ref entries[index];
            }

            if (entry.Value == 0)
            {
                entry.Key = key;
                entry.HashCode = hashCode;
                entry.Value = ++_count;
            }

            return entry.Value - 1;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetId(Type key, out int id)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var capacity = _capacity;
            var entries = _entries;
            var hashCode = key.GetHashCode() & 0x7FFFFFFF;
            var index = hashCode % capacity;

            ref var entry = ref entries[index];

            while (entry.Value != 0 && !(entry.HashCode == hashCode && entry.Key == key))
            {
                index = (index + 1) % capacity;
                entry = ref entries[index];
            }

            id = entry.Value - 1;

            return entry.Value != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int TryResize()
        {
            if (_count >= _resizeThreshold)
            {
                _primeIndex++;

                if (_primeIndex >= _primes.Length)
                {
                    throw new IndexOutOfRangeException("The prime index is out of range.");
                }

                _capacity = _primes[_primeIndex];
                
                _resizeThreshold = (int) (_capacity * _resizeFactor);

                Resize(_capacity);
            }

            return _capacity;
        }
        
        // method has unrolled loop and entries are rarely will be dense, so i skip it
        [ExcludeFromCodeCoverage]
        private void Resize(int newCapacity)
        {
            const int batchCount = 8;

            var newEntries = new Entry[newCapacity];
            var oldEntries = _entries;
            var oldCount = oldEntries.Length;

            ref var newEntry = ref newEntries[0];
            ref var oldEntry = ref oldEntries[0];
            var index = -1;
 
            for (var i = 0; i < oldCount; i++)
            {
                oldEntry = ref oldEntries[i];
            
                if (oldEntry.Value != 0)
                {
                    index = oldEntry.HashCode % newCapacity;
            
                    newEntry = ref newEntries[index];
            
                    while (newEntry.Value != 0)
                    {
                        index = (index + 1) % newCapacity;
                        newEntry = ref newEntries[index];
                    }
            
                    newEntry.Key = oldEntry.Key;
                    newEntry.HashCode = oldEntry.HashCode;
                    newEntry.Value = oldEntry.Value;
                }
            }

            _capacity = newCapacity;
            _entries = newEntries;
        }
    }
}
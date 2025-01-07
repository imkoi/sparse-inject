using System;
using System.Runtime.CompilerServices;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#if UNITY_2017_1_OR_NEWER
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
public class TypeIdProvider
{
    private static readonly int[] _primes = new int[]
    {
        3, 7, 13, 31, 61, 127, 251, 509, 1021, 2039, 4093, 8191, 16381, 32749, 65521,
        131071, 262139, 524287, 1048573, 2097143, 4194301, 8388593, 16777213, 33554393
    };
    
    private struct Entry
    {
        public Type Key;
        public int Value;
    }

    private Entry[] _entries;
    private int _count;
    private int _capacity;

    private int _primeIndex;
    
    public int Count => _count;

    private const int BitMask = 1 << 25;

    public TypeIdProvider(int capacity = 128)
    {
        capacity = (int)(capacity * 2f);

        if (capacity > 8388607 * 2)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "The capacity must be less than 8388608.");
        }
        
        var prime = _primes[_primeIndex];
        while (prime < capacity)
        {
            _primeIndex++;
            prime = _primes[_primeIndex];
        }

        _capacity = prime;
        _entries = new Entry[prime];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetId(Type key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }
        
        var capacity = TryResize();
        var entries = _entries;
        var originalIndex = (key.GetHashCode() & 0x7FFFFFFF) % capacity;
        var index = originalIndex;
        
        ref var entry = ref entries[index];

        while ((entry.Value & BitMask) != 0 && entry.Key != key)
        {
            index = (index + 1) % capacity;
            entry = ref entries[index];
        }

        if (index != originalIndex)
        {
            entry.Key = key;
            entry.Value = _count | BitMask;
        
            _count++;
        }

        return entry.Value & 0x01FFFFFF;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int TryResize()
    {
        if (_count >= _capacity * 0.5f) // multiply _capacity by some number to make less hash collisions
        {
            _primeIndex++;

            if (_primeIndex >= _primes.Length)
            {
                throw new IndexOutOfRangeException("The prime index is out of range.");
            }

            _capacity = _primes[_primeIndex];
            
            Resize(_capacity);
        }
        
        return _capacity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Resize(int newCapacity)
    {
        if (newCapacity > 8388607 * 2)
        {
            throw new ArgumentOutOfRangeException("The capacity must be less than 8388608.");
        }
        
        var newEntries = new Entry[newCapacity];
        var oldEntries = _entries;
        var oldCount = oldEntries.Length;

        for (var i = 0; i < oldCount; i++)
        {
            ref var oldEntry = ref oldEntries[i];

            if ((oldEntry.Value & BitMask) != 0)
            {
                var index = (oldEntry.Key.GetHashCode() & 0x7FFFFFFF) % newCapacity;
                
                ref var newEntry = ref newEntries[index];

                while ((newEntry.Value & BitMask) != 0)
                {
                    index = (index + 1) % newCapacity;
                    newEntry = ref newEntries[index];
                }

                newEntry.Key = oldEntry.Key;
                newEntry.Value = oldEntry.Value;
            }
        }
        
        _capacity = newCapacity;
        _entries = newEntries;
    }
}

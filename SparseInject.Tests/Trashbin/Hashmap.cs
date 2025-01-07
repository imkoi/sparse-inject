using System;
using System.Runtime.CompilerServices;

public class Hashmap<TKey, TValue> where TKey : class
{
    private static readonly int[] _primes = new int[]
    {
        3, 7, 13, 31, 61, 127, 251, 509, 1021, 2039, 4093, 8191, 16381, 32749, 65521,
        131071, 262139, 524287, 1048573, 2097143, 4194301, 8388593, 16777213, 33554393
    };
    
    private struct Entry
    {
        public int HashCode;
        public TKey Key;
        public TValue Value;
    }

    private Entry[] _entries;
    private int _count;
    private int _capacity;

    private int _primeIndex;

    public Hashmap(int capacity = 16)
    {
        capacity = (int)(capacity * 1.25f);
        
        var prime = _primes[_primeIndex];
        while (prime < capacity)
        {
            _primeIndex++;
            prime = _primes[_primeIndex];
        }

        _capacity = prime;
        _entries = new Entry[prime];

        for (var i = 0; i < prime; i++)
        {
            _entries[i].HashCode = -1;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(TKey key, TValue value)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        if (_count >= _capacity * 0.75f)
        {
            _primeIndex++;
            
            Resize(_primes[_primeIndex]);
        }

        var capacity = _capacity;

        var hashCode = key.GetHashCode() & 0x7FFFFFFF;
        var index = hashCode % capacity;
        
        ref var entry = ref _entries[index];

        while (entry.HashCode >= 0)
        {
            if (entry.HashCode == hashCode && entry.Key == key)
            {
                entry.Value = value;
                return;
            }
            
            index = (index + 1) % capacity;
            entry = ref _entries[index];
        }

        entry.HashCode = hashCode;
        entry.Key = key;
        entry.Value = value;

        _count++;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Remove(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        var capacity = _capacity;

        var hashCode = key.GetHashCode() & 0x7FFFFFFF;
        var index = hashCode % capacity;
        
        ref var entry = ref _entries[index];

        while (entry.HashCode >= 0)
        {
            if (entry.HashCode == hashCode && entry.Key == key)
            {
                entry.HashCode = -1;

                _count--;
                
                return;
            }
            
            index = (index + 1) % capacity;
            entry = ref _entries[index];
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        var capacity = _capacity;

        var hashCode = key.GetHashCode() & 0x7FFFFFFF;
        var index = hashCode % capacity;
        
        ref var entry = ref _entries[index];

        while (entry.HashCode >= 0)
        {
            if (entry.HashCode == hashCode && entry.Key == key)
            {
                return true;
            }
            
            index = (index + 1) % capacity;
            entry = ref _entries[index];
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetValue(TKey key, out TValue value)
    {
        var capacity = _capacity;

        var hashCode = key.GetHashCode() & 0x7FFFFFFF;
        var index = hashCode % capacity;
        
        ref var entry = ref _entries[index];

        while (entry.HashCode >= 0)
        {
            if (entry.HashCode == hashCode && entry.Key == key)
            {
                value = entry.Value;
                return true;
            }
            
            index = (index + 1) % capacity;
            entry = ref _entries[index];
        }

        value = default;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Resize(int newCapacity)
    {
        var newEntries = new Entry[newCapacity];
        var oldEntries = _entries;
        var oldCount = oldEntries.Length;
        
        Array.Fill(newEntries, new Entry { HashCode = -1 });

        for (var i = 0; i < oldCount; i++)
        {
            ref var oldEntry = ref oldEntries[i];

            if (oldEntry.HashCode >= 0)
            {
                var index = oldEntry.HashCode % newCapacity;
                
                ref var entry = ref newEntries[index];

                while (entry.HashCode >= 0)
                {
                    index = (index + 1) % newCapacity;
                    entry = ref newEntries[index];
                }
                
                entry.HashCode = oldEntry.HashCode;
                entry.Key = oldEntry.Key;
                entry.Value = oldEntry.Value;
            }
        }
        
        _capacity = newCapacity;
        _entries = newEntries;
    }
}

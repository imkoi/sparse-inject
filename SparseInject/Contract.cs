using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace SparseInject
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public struct Contract
    {
        public Type Type;
        public ulong Data; // 64 bit: 24 bit index, 24 bit count, 1 bit isSingleton, 1 bit isFactory, 1 bit isScope, 1 bit isArray, 1 bit HasValue, 1 bit hasInstanceFactory

        // Bit positions and masks
        private const ulong IndexMask = (1UL << 24) - 1UL;

        private const int CountShift = 24;
        private const ulong CountMask = ((1UL << 24) - 1UL) << CountShift;

        private const int IsArrayShift = 48;
        private const ulong IsArrayMask = 1UL << IsArrayShift;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsCollection()
        {
            return (Data & IsArrayMask) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MarkCollection(bool value)
        {
            if (value)
                Data |= IsArrayMask;
            else
                Data &= ~IsArrayMask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetConcretesIndex()
        {
            return (int)(Data & IndexMask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetConcretesIndex(int value)
        {
            Data = (Data & ~IndexMask) | ((ulong)(uint)value & IndexMask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetConcretesCount()
        {
            return (int)((Data >> CountShift) & ((1UL << 24) - 1UL));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetConcretesCount(int value)
        {
            Data = (Data & ~CountMask) | (((ulong)(uint)value << CountShift) & CountMask);
        }
    }
}
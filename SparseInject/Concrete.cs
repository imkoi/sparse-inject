using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace SparseInject
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public struct Concrete
    {
        public Type Type;
        public ulong Data; // 64 bit: 24 bit index, 24 bit count, 1 bit isSingleton, 1 bit isFactory, 1 bit isScope, 1 bit isArray, 1 bit HasValue, 1 bit hasInstanceFactory
        public ConstructorInfo ConstructorInfo;
        public InstanceFactoryBase GeneratedInstanceFactory;
        public object Value;

        // Bit positions and masks
        private const int IndexShift = 0;
        private const ulong IndexMask = (1UL << 24) - 1UL;

        private const int CountShift = 24;
        private const ulong CountMask = ((1UL << 24) - 1UL) << CountShift;

        private const int IsSingletonShift = 48;
        private const ulong IsSingletonMask = 1UL << IsSingletonShift;

        private const int IsFactoryShift = 49;
        private const ulong IsFactoryMask = 1UL << IsFactoryShift;

        private const int IsScopeShift = 50;
        private const ulong IsScopeMask = 1UL << IsScopeShift;

        private const int HasValueShift = 52;
        private const ulong HasValueMask = 1UL << HasValueShift;

        private const int HasInstanceFactoryShift = 53;
        private const ulong HasInstanceFactoryMask = 1UL << HasInstanceFactoryShift;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsSingleton()
        {
            return (Data & IsSingletonMask) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFactory()
        {
            return (Data & IsFactoryMask) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsScope()
        {
            return (Data & IsScopeMask) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasValue()
        {
            return (Data & HasValueMask) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasInstanceFactory()
        {
            return (Data & HasInstanceFactoryMask) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MarkSingleton()
        {
            Data |= IsSingletonMask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MarkFactory(bool value)
        {
            if (value)
                Data |= IsFactoryMask;
            else
                Data &= ~IsFactoryMask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MarkScope()
        {
            Data |= IsScopeMask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MarkValue()
        {
            Data |= HasValueMask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MarkInstanceFactory()
        {
            Data |= HasInstanceFactoryMask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetConstructorContractsIndex()
        {
            return (int)(Data & IndexMask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetConstructorContractsIndex(int value)
        {
            Data = (Data & ~IndexMask) | ((ulong)(uint)value & IndexMask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetConstructorContractsCount()
        {
            return (int)((Data >> CountShift) & ((1UL << 24) - 1UL));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetConstructorContractsCount(int value)
        {
            Data = (Data & ~CountMask) | (((ulong)(uint)value << CountShift) & CountMask);
        }
    }
}

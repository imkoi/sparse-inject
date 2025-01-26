using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SparseInject
{
#if UNITY_2017_1_OR_NEWER
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    internal struct Concrete
    {
        public Type Type;
        public ulong Data; // 64 bit: 24 bit index, 24 bit count, 1 bit isSingleton, 1 bit isFactory, 1 bit isScope, 1 bit isArray, 1 bit HasValue, 1 bit hasInstanceFactory
        public ConstructorInfo ConstructorInfo;
        public InstanceFactoryBase GeneratedInstanceFactory;
        public object Value;
        
#if NET && DEBUG
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public bool DebugIsSingleton => IsSingleton();
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public bool DebugIsFactory => IsFactory();
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public bool DebugIsScope => IsScope();
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public bool DebugIsArray => IsArray();
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public bool DebugIsDisposable => IsDisposable();
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public bool DebugHasValue => HasValue();
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public bool DebugHasInstanceFactory => HasInstanceFactory();
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public int DebugConstructorContractsIndex => GetConstructorContractsIndex();
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public int DebugConstructorContractsCount => GetConstructorContractsCount();
#endif

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
        
        private const int IsArrayShift = 51;
        private const ulong IsArrayMask = 1UL << IsArrayShift;

        private const int HasValueShift = 52;
        private const ulong HasValueMask = 1UL << HasValueShift;

        private const int HasInstanceFactoryShift = 53;
        private const ulong HasInstanceFactoryMask = 1UL << HasInstanceFactoryShift;
        
        private const int IsDisposableShift = 54;
        private const ulong IsDisposableMask = 1UL << IsDisposableShift;

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
        public bool IsArray()
        {
            return (Data & IsArrayMask) != 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsDisposable()
        {
            return (Data & IsDisposableMask) != 0;
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
        public void MarkArray()
        {
            Data |= IsArrayMask;
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
        public void MarkDisposable()
        {
            Data |= IsDisposableMask;
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

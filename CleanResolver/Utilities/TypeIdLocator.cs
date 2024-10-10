using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace CleanResolver
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    internal static class TypeIdLocator
    {
        private static int _currentDependencyId;
        private static readonly Dictionary<Type, int> _dependencyTypeToIdMap = new Dictionary<Type, int>(4096);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AddDependencyId(Type type)
        {
            var id = _currentDependencyId;

            _dependencyTypeToIdMap.Add(type, id);

            _currentDependencyId++;

            return id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDependencyId(Type type)
        {
            return _dependencyTypeToIdMap[type];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TryGetDependencyId(Type type)
        {
            return _dependencyTypeToIdMap.TryGetValue(type, out var value) ? value : -1;
        }
    }
}
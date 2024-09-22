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
        private static int _currentImplementationId;
        private static readonly Dictionary<Type, int> _dependencyTypeToIdMap = new Dictionary<Type, int>(4096);
        private static readonly Dictionary<Type, int> _implementationTypeToIdMap = new Dictionary<Type, int>(4096);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AddDependencyId(Type type)
        {
            var id = _currentDependencyId;

            _dependencyTypeToIdMap.Add(type, id);

            _currentDependencyId++;

            return id;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AddImplementationId(Type type)
        {
            var id = _currentImplementationId;

            _implementationTypeToIdMap.Add(type, id);

            _currentImplementationId++;

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
            return _dependencyTypeToIdMap.GetValueOrDefault(type, -1);
        }
    }
}
using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace CleanResolver
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    internal static class TypeCompileInfo<T>
    {
        private static int _dependencyId = -1;
        private static int _implementationId = -1;
        public static readonly Type Type = typeof(T);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RegisterDependency(out int id, out Type type)
        {
            if (_dependencyId < 0)
            {
                _dependencyId = TypeIdLocator.AddDependencyId(Type);
                
                id = _dependencyId;
                type = Type;
                
                return true;
            }

            id = _dependencyId;
            type = Type;

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetRuntimeDependencyId()
        {
            if (_dependencyId < 0)
            {
                var elementType = Type.GetElementType();

                return TypeIdLocator.GetDependencyId(elementType);
            }

            return _dependencyId;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RegisterImplementation(out int id, out Type type)
        {
            if (_implementationId < 0)
            {
                _implementationId = TypeIdLocator.AddImplementationId(Type);
                
                id = _implementationId;
                type = Type;
                
                return true;
            }

            id = _implementationId;
            type = Type;

            return false;
        }
    }
}

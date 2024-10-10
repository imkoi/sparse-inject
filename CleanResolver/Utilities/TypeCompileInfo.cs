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
        public static readonly Type Type = typeof(T);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetId(out Type type)
        {
            if (_dependencyId < 0)
            {
                _dependencyId = TypeIdLocator.AddDependencyId(Type);
            }

            type = Type;

            return _dependencyId;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetRuntimeDependencyId()
        {
            if (_dependencyId < 0 && Type.IsArray)
            {
                var elementType = Type.GetElementType();

                return TypeIdLocator.GetDependencyId(elementType);
            }

            return _dependencyId;
        }
    }
}
